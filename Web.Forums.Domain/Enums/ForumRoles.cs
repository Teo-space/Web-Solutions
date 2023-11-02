namespace Web.Forums.Domain.Enums;


public enum ForumRoles
{
	ForumRoleBasic = 0,

	ForumRoleTrusted = 1000,

	ForumRoleModerator = 3000,

	ForumRoleAdmin = int.MaxValue,
	ForumRoleBanned = int.MinValue,
}
