using System.Security.Claims;

namespace Identity.UserAccessorService;

public interface IUserAccessor
{
	ClaimsPrincipal User { get; }


	ClaimsPrincipal GetUserThrowIfIsNull()
	{
		if(User is null)
		{
			throw new UnauthorizedAccessException();
		}
		return User;
	}


	string? FindValue(string type) => User?.FindFirst(type)?.Value;


	TKey UserId
	{
		get
		{
			var stringId = FindValue(ClaimTypes.NameIdentifier)
				?? throw new NullReferenceException(nameof(ClaimTypes.NameIdentifier));
			return TKey.Parse(stringId);
		}
	}

	string NameIdentifier => FindValue(ClaimTypes.NameIdentifier) 
		?? throw new NullReferenceException(nameof(ClaimTypes.NameIdentifier));

	string UserName => FindValue(ClaimTypes.Name)
		?? throw new NullReferenceException(nameof(ClaimTypes.Name));

	string Email => FindValue(ClaimTypes.Email)
		?? throw new NullReferenceException(nameof(ClaimTypes.Email));




	Claim? FindFirst(string type) => User?.FindFirst(type);

	IEnumerable<Claim>? FindAll(string type) => User?.FindAll(type);

	bool HasClaim(Predicate<Claim> match) => User.HasClaim(match);

	bool HasClaim(string type, string value) => User.HasClaim(type, value);


	bool IsInRole(string role) => User.IsInRole(role);







}




