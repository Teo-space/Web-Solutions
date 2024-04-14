public record Result
{
    public required bool Success { get; init; }
    public required string Type { get; init; }
    public required string Detail { get; init; }
    public required IReadOnlyCollection<FieldError> Errors { get; init; } = new List<FieldError>();



    public static implicit operator string(Result Result)
        => $"Result(Success:{Result.Success}, Type: {Result.Type}, Detail: {Result.Detail}, Errors: {Result.Errors.Count}) ";


    public override string ToString()
    {
        return @$"Result
Type: {this.Type}
Success:{this.Success}
Detail: {this.Detail}
Errors: {this.Errors.Count}";
    }
}