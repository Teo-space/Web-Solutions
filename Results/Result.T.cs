public record Result<T> : Result
{
	public required T Value { get; init; }

	public static implicit operator T(Result<T> Result) => Result.Value;
	public static implicit operator Result<T>(T o) => Results.Ok<T>(o);


	public static implicit operator string(Result<T> Result)
		=> $"Result<{typeof(T)}>(Success:{Result.Success}, Type: {Result.Type}) Value: {Result?.Value?.ToString()}";


	public override string ToString()
	{
		return @$"Result<{typeof(T)}>()
Type: {this.Type}
Success:{this.Success}
Detail: {this.Detail}
Value: {this?.Value?.ToString()}";
	}
}