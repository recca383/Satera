namespace Satera_Api;

public static class CustomResults
{
    public static IResult Problem(Result result)
    {
        if(result.IsSuccess)
        {
            throw new InvalidOperationException();
        }

        return Results.Problem(
            title: GetTitle(result.Error),
            detail: GetDetail(result.Error),
            type: GetType(result.Error.Type),
            statusCode: GetStatusCode(result.Error.Type),
            extensions: GetErrors(result));
    }

    private static string? GetTitle(Error error) =>
        error.Type switch
        {
            ErrorType.NotFound => "Resource Not Found",
            ErrorType.Conflict => "Conflict Occurred",
            ErrorType.Validation => "Validation Error",
            ErrorType.Unauthorized => "Unauthorized Access",
            ErrorType.Forbidden => "Forbidden Access",
            ErrorType.Problem => "An Error Occurred",
            _ => "An Unexpected Error Occurred"
        };

    private static string? GetDetail(Error error) =>
        error.Type switch
        {
            ErrorType.NotFound => error.Description,
            ErrorType.Conflict => error.Description,
            ErrorType.Validation => error.Description,
            ErrorType.Unauthorized => error.Description,
            ErrorType.Forbidden => error.Description,
            ErrorType.Problem => error.Description,
            _ => "An unexpected error occurred"
        };

    private static string? GetType(ErrorType errorType) =>
        errorType switch
        {
            ErrorType.NotFound => "https://httpstatuses.com/404",
            ErrorType.Conflict => "https://httpstatuses.com/409",
            ErrorType.Validation => "https://httpstatuses.com/400",
            ErrorType.Unauthorized => "https://httpstatuses.com/401",
            ErrorType.Forbidden => "https://httpstatuses.com/403",
            ErrorType.Problem => "https://httpstatuses.com/500",
            _ => "https://httpstatuses.com/500"
        };


    private static int? GetStatusCode(ErrorType errorType) =>
        errorType switch
        {
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            ErrorType.Problem => StatusCodes.Status500InternalServerError,
            _ => 500
        };
    

    private static Dictionary<string, object?>? GetErrors(Result result)
    {
        return [];
    }

    

    

    
}
