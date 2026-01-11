using Microsoft.EntityFrameworkCore;
using Satera_Api.Application;

namespace Satera_Api.ML
{
    public class DataPreparationHandler(
        IAppDbContext dbContext,
    {

        private const int FuzzyMatchThreshold = 75;

        public async Task<DataPreparationResults> Handle(GetMLAnalysisCommand inputData, CancellationToken cancellation)
        {

            var appDictionary = await dbContext
                .App_Categories
                .AsNoTracking()
                .ToDictionaryAsync(key => key.App_name, val => val.Final_Rating, cancellation);

            var gwaScaled = ScaleGwa(inputData.Gwa);
            var usageSeconds = inputData.TotalScreenTime;

            var sessionDepthLog = CalculateSessionDepthLog(inputData.TotalScreenTime, inputData.Pickups);

            var unlockIntensityLog = CalculateUnlockIntensityLog(inputData.DeviceUnlocks, inputData.TotalScreenTime);
            var pickupsLog = ApplyLogTransform(inputData.Pickups);

            var groupedUsage = CategorizeAndAggregate(inputData.Apps, appDictionary);

            var academicLog = ApplyLogTransform(groupedUsage.Academic);
            var productivityLog = ApplyLogTransform(groupedUsage.Productivity);
            var socialLog = ApplyLogTransform(groupedUsage.Social);
            var entertainmentLog = ApplyLogTransform(groupedUsage.Entertainment);

            var focusRatioLog = CalculateFocusRatioLog(groupedUsage);
            var academicEfficiencyLog = CalculateAcademicEfficiencyLog(groupedUsage, gwaScaled);

            return new DataPreparationResults(
                usageSeconds,
                gwaScaled,
                academicLog,
                productivityLog,
                socialLog,
                entertainmentLog,
                focusRatioLog,
                academicEfficiencyLog,
                unlockIntensityLog,
                sessionDepthLog,
                pickupsLog
                );

        }
        private float ScaleGwa(float gwa)
        {
            const float min = 1.0f;
            const float max = 5.0f;

            return (gwa - min) / (max - min);
        }
        private float CalculateSessionDepthLog(int totalscreentime, int pickups)
        {
            float sessionDepth = (float)totalscreentime / (float)(pickups + 1);
            return ApplyLogTransform(sessionDepth);
        }
        private float CalculateUnlockIntensityLog(int deviceUnlocks, int totalScreenTime)
        {
            float intensity = (float)deviceUnlocks / (float)(totalScreenTime + 1);

            return ApplyLogTransform(intensity);
        }
        private float ApplyLogTransform(float rawNumber)
        {
            return MathF.Log(rawNumber + 1);
        }

        private float CalculateAcademicEfficiencyLog(UsageGroups groups, float gwaScaled)
        {
            float effiency = groups.Academic / (float)(gwaScaled+0.1);

            return ApplyLogTransform(effiency);
        }

        private float CalculateFocusRatioLog(UsageGroups groups)
        {
            float focus = groups.Academic + groups.Productivity;
            float distraction = groups.Social + groups.Entertainment;

            return ApplyLogTransform(focus / (distraction + 1));
        }

        private UsageGroups CategorizeAndAggregate(App[] apps, Dictionary<string, string> categoryLookup)
        {
            var categoryTotals = new Dictionary<string, int>();

            foreach(var app in apps)
            {
                var match = FindBestFuzzyMatch(app.PackageName, categoryLookup.Keys);

                if(match.Score >= FuzzyMatchThreshold)
                {
                    var category = categoryLookup[match.MatchedCategory];

                    if(!categoryTotals.ContainsKey(category))
                    {
                        categoryTotals[category] = 0;
                    }

                    categoryTotals[category] += app.TotalTimeInForeground;
                }
            }

            return GroupCategories(categoryTotals);
        }

        private UsageGroups GroupCategories(Dictionary<string, int> categoryTotals)
        {
            int academic = 0;
            int social = 0;
            int entertainment = 0;
            int productivity = 0;

            foreach(var kvp in categoryTotals)
            {
                switch(kvp.Key)
                {
                    case "Education":
                        academic += kvp.Value;
                        break;
                    case "Social":
                        social += kvp.Value;
                        break;
                    case "Entertainment":
                        entertainment += kvp.Value;
                        break;
                    case "Productivity":
                        productivity += kvp.Value;
                        break;
                }
            }

            return new UsageGroups(
                Academic: academic,
                Productivity: productivity,
                Social: social,
                Entertainment: entertainment);
        }

        private (string MatchedCategory, float Score) FindBestFuzzyMatch(string input, IEnumerable<string> categories)
        {
            string bestMatch = string.Empty;
            float bestScore = 0f;

            foreach (var category in categories)
            {
                float score = LevenshteinRatio(input, category);

                if (score > bestScore)
                {
                    bestScore = score;
                    bestMatch = category;
                }
            }
            return (bestMatch, bestScore);
        }

        private float LevenshteinRatio(string s1, string s2)
        {
            int distance = LevenshteinDistance(s1, s2);
            int maxLength = Math.Max(s1.Length, s2.Length);
            return maxLength == 0 ? 1.0f : (float)(maxLength - distance) / maxLength;
        }

        private int LevenshteinDistance(string s, string t)
        {
            int[,] d = new int[s.Length + 1, t.Length + 1];

            for (int i = 0; i <= s.Length; i++) d[i, 0] = i;
            for (int j = 0; j <= t.Length; j++) d[0, j] = j;

            for (int i = 1; i <= s.Length; i++)
            {
                for (int j = 1; j <= t.Length; j++)
                {
                    int cost = s[i - 1] == t[j - 1] ? 0 : 1;

                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }

            return d[s.Length, t.Length];
        }


        private sealed record UsageGroups(
            int Academic,
            int Productivity,
            int Social,
            int Entertainment);


    }
}
