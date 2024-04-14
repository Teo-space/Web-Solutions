namespace Identity.Interfaces
{
	public interface IRootUserInitializerService
	{
		Task<List<string>> InitializeRootUser();
		Task<List<string>> AddRootUserToBasicRoles();
	}

}
