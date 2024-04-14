public partial class Results
{
	public static Result<T> Deleted<T>(string Detail)
		=> Problem<T>("Deleted", Detail);


}