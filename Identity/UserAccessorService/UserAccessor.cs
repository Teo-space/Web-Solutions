using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Identity.UserAccessorService;


internal class UserAccessor(IHttpContextAccessor accessor) : IUserAccessor
{
	public ClaimsPrincipal User => accessor?.HttpContext?.User 
		?? throw new ArgumentNullException(nameof(User));



}