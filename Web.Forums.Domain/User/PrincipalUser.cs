using System.Security.Claims;


public class PrincipalUser
{

	public Guid UserId { get; private set; }
	public string UserName { get; private set; }
	public IReadOnlyCollection<string> Roles { get; private set; } = new List<string>();


	public bool IsValid { get; private set; } = true;

	public bool IsInRole(string role) => Roles.Contains(role);

	public PrincipalUser(Guid UserId, string UserName)
	{
		this.UserId = UserId;
		this.UserName = UserName;
	}

	public PrincipalUser(Guid UserId, string UserName, IReadOnlyCollection<string> Roles)
	{
		this.UserId = UserId;
		this.UserName = UserName;
		this.Roles = Roles;
	}


	public PrincipalUser(ClaimsPrincipal principal)
	{
		{
			string stringUserId = principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (string.IsNullOrEmpty(stringUserId))
			{
				IsValid = false;
				return;
			}
			if (Guid.TryParse(stringUserId, out var id))
			{
				UserId = id;
			}
			else
			{
				IsValid = false;
				return;
			}
		}
		{
			UserName = principal?.FindFirst(ClaimTypes.Name)?.Value;
			if (string.IsNullOrEmpty(UserName))
			{
				IsValid = false;
				return;
			}
		}
		{
			Roles = principal.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).ToList();
		}
	}


}
