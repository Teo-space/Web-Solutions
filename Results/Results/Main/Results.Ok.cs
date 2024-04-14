public partial class Results
{
    public static Result<T> Ok<T>(T Value) => new Result<T>()
    {  
        Value = Value ,
        Success = true,
        Type = "OkResult",
        Detail = null!,
        Errors = Empty
    };

}
