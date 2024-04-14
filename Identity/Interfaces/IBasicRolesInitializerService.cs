namespace Identity.Interfaces;

public interface IBasicRolesInitializerService
{
	Task<List<string>> Initialize();
}
