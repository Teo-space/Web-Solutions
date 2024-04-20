using Web.Forums.Domain.Owned;

namespace Web.Forums.Domain.Aggregate;

public sealed partial class Post
{

	public UserAction CreatedBy { get; set; }
	public UserAction UpdatedBy { get; set; }

	public void SetCreatedBy(PrincipalUser user)
		=> CreatedBy = new UserAction(user.UserId, user.UserName, DateTime.Now);
	public void SetCreatedBy(Guid UserId, string UserName)
		=> CreatedBy = new UserAction(UserId, UserName, DateTime.Now);

	public void SetUpdatedBy(PrincipalUser user)
		=> UpdatedBy = new UserAction(user.UserId, user.UserName, DateTime.Now);
	public void SetUpdatedBy(Guid UserId, string UserName)
		=> UpdatedBy = new UserAction(UserId, UserName, DateTime.Now);

}
