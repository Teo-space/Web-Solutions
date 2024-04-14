public partial class Results
{
	public static Result<T> Exception<T>(string Type, string Detail) 
		=> Problem<T>("Exception", Detail);


}