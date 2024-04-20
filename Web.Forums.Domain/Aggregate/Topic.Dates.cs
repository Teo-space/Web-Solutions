﻿using Web.Forums.Domain.Owned;

namespace Web.Forums.Domain.Aggregate;

public sealed partial class Topic
{
	public UserAction CreatedBy { get; private set; }
	public UserAction UpdatedBy { get; private set; }
	public UserAction RepliedBy { get; private set; }


	public void SetCreatedBy(PrincipalUser user)
		=> CreatedBy = new UserAction(user.UserId, user.UserName, DateTime.Now);
	public void SetCreatedBy(Guid UserId, string UserName)
		=> CreatedBy = new UserAction(UserId, UserName, DateTime.Now);

	public void SetUpdatedBy(PrincipalUser user)
		=> UpdatedBy = new UserAction(user.UserId, user.UserName, DateTime.Now);
	public void SetUpdatedBy(Guid UserId, string UserName)
		=> UpdatedBy = new UserAction(UserId, UserName, DateTime.Now);

	public void SetRepliedBy(PrincipalUser user)
		=> RepliedBy = new UserAction(user.UserId, user.UserName, DateTime.Now);
	public void SetRepliedBy(Guid UserId, string UserName)
		=> RepliedBy = new UserAction(UserId, UserName, DateTime.Now);

}
