public partial class Results
{
	public static Result<T> InvalidOperation<T>(string Detail)
		=> Problem<T>("InvalidOperation", Detail);


}