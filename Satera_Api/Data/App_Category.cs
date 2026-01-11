using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelToSQLiteConverter.Data
{
    public class App_Category
    {
        public int Id { get; set; }
        public string App_name { get; set; } = string.Empty;
        public string App_generic_name { get; set; } = string.Empty;
        public string Perc_users { get; set; } = string.Empty;
        public string Training_Coding_1 { get; set; } = string.Empty;
        public string Training_Coding_2 { get; set; } = string.Empty;
        public string Training_Coding_all { get; set; } = string.Empty;
        public string Rater1 { get; set; } = string.Empty;
        public string Rater2 { get; set; } = string.Empty;
        public string Final_Rating { get; set; } = string.Empty;


    }
}
