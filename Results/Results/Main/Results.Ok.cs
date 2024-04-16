public partial class Results
{
	public static Result<T> Ok<T>(T Value) => Results.Create<T>(Value, true, "OkResult", null!);

}
