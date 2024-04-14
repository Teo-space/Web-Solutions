public record FieldError
{
    public required string FieldName { get; init; }
    public required IReadOnlyCollection<string> ErrorMessages { get; init; }
}