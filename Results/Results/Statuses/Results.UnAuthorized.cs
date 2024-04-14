public partial class Results
{
	public static Result<T> UnAuthorizedResult<T>(string Detail)
		=> Problem<T>("UnAuthorized", Detail);

	public static Result<T> NotEnoughPermissions<T>(string Detail)
		=> Problem<T>("NotEnoughPermissions", Detail);

	public static Result<T> NotEnoughPermissions<T>()
		=> Problem<T>("NotEnoughPermissions", "Not Enought Permissions for action");




}