using Web.Forums.Domain.Owned;

namespace Web.Forums.Domain.Aggregate;

public sealed partial class Topic
{
	public UserAction CreatedBy { get; private set; }
	public UserAction UpdatedBy { get; private set; }
	public UserAction RepliedBy { get; private set; }


	public void SetCreatedBy(PrincipalUser user)
		=> CreatedBy = UserAction.Create(user.UserId, user.UserName);
	public void SetCreatedBy(Guid UserId, string UserName)
		=> CreatedBy = UserAction.Create(UserId, UserName);

	public void SetUpdatedBy(PrincipalUser user)
		=> UpdatedBy = UserAction.Create(user.UserId, user.UserName);
	public void SetUpdatedBy(Guid UserId, string UserName)
		=> UpdatedBy = UserAction.Create(UserId, UserName);

	public void SetRepliedBy(PrincipalUser user)
		=> RepliedBy = UserAction.Create(user.UserId, user.UserName);
	public void SetRepliedBy(Guid UserId, string UserName)
		=> RepliedBy = UserAction.Create(UserId, UserName);

}
