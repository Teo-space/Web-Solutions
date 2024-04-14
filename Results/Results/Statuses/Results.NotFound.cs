public partial class Results
{
	public static Result<T> NotFound<T>(string Detail)
		=> Problem<T>("NotFound", Detail);

	public static Result<T> NotFoundById<T>(object Id)
		=> NotFound<T>($"Not found {typeof(T)} By Id: {Id}");

	public static Result<T> ParentNotFound<T>(string Detail)
		=> Problem<T>("ParentNotFound", Detail);
	public static Result<T> ParentNotFoundById<T>(object Id)
		=> ParentNotFound<T>($"Parent Not found {typeof(T)} By Id: {Id}");

}