using Web.Forums.Domain.Owned;

namespace Web.Forums.Domain.Aggregate;

public sealed partial class Post
{

	public UserAction CreatedBy { get; set; }
	public UserAction UpdatedBy { get; set; }

	public void SetCreatedBy(PrincipalUser user)
		=> CreatedBy = UserAction.Create(user.UserId, user.UserName);
	public void SetCreatedBy(Guid UserId, string UserName)
		=> CreatedBy = UserAction.Create(UserId, UserName);

	public void SetUpdatedBy(PrincipalUser user)
		=> UpdatedBy = UserAction.Create(user.UserId, user.UserName);
	public void SetUpdatedBy(Guid UserId, string UserName)
		=> UpdatedBy = UserAction.Create(UserId, UserName);

}
