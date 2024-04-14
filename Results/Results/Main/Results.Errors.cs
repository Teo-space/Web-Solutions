public partial class Results
{
    public static Result<T> Errors<T>(string Type, string Detail, IReadOnlyCollection<FieldError> errors) 
        => new Result<T>()
    {
        Value = default(T)!,
        Success = false,
        Type = Type,
        Detail = Detail,
        Errors = errors
    };


    public static Result<T> Errors<T>(string Type, string Detail, List<FieldError> errors) 
        => Errors<T>(Type, Detail, errors.AsReadOnly());

    public static Result<T> Errors<T>(string Type, string Detail, IEnumerable<FieldError> errors) 
        => Errors<T>(Type, Detail, errors.ToArray());


    public static Result<T> Errors<T>(T Value, string Type, string Detail, IReadOnlyCollection<FieldError> errors) 
		=> new Result<T>()
    {
        Value = Value,
        Success = false,
        Type = Type,
        Detail = Detail,
        Errors = errors
    };

    public static Result<T> Errors<T>(T Value, string Type, string Detail, List<FieldError> errors)
        => Errors<T>(Value, Type, Detail, errors.AsReadOnly());
    public static Result<T> Errors<T>(T Value, string Type, string Detail, IEnumerable<FieldError> errors)
        => Errors<T>(Value, Type, Detail, errors.ToArray());
}
