namespace Identity.RootUser
{
	public interface IRootUserInitializerService
	{
		Task<List<string>> InitializeRootUser();
		Task<List<string>> AddRootUserToBasicRoles();
	}

}
