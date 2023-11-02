namespace Identity.BasicRoles
{
	public interface IBasicRolesInitializerService
	{
		Task<List<string>> Initialize();
	}

}
