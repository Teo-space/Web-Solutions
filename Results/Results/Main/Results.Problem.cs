public partial class Results
{
    public static Result<T> Problem<T>(T Value, string Type, string Detail) => new Result<T>()
    {
        Value = Value,
        Success = false,
        Type = Type,
        Detail = Detail,
        Errors = Empty
    };


    public static Result<T> Problem<T>(string Type, string Detail) => new Result<T>()
    {
        Value = default(T)!,
        Success = false,
        Type = Type,
        Detail = Detail,
        Errors = Empty
    };

}
