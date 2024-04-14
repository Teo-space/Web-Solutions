public static class ResultTypedExtensions
{
    public static Result<T> Ok<T>(this T result) where T : class => Results.Ok(result);


    public static Result<IReadOnlyCollection<T>> Ok<T>(this IReadOnlyCollection<T> objects) where T : class
        => Results.Ok(objects);

    public static Result<IReadOnlyCollection<T>> Ok<T>(this List<T> objects) where T : class
        => Results.Ok(objects as IReadOnlyCollection<T>);

    public static Result<IReadOnlyCollection<T>> Ok<T>(this IEnumerable<T> objects) where T : class
        => Results.Ok(objects.ToArray() as IReadOnlyCollection<T>);


}