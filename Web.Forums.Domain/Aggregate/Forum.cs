﻿namespace Web.Forums.Domain.Aggregate;

using System.Security.Claims;
using Web.Forums.Domain.Enums;
using Web.Forums.Domain.Owned;
using Web.Forums.Domain.Permissions;

public sealed partial class Forum
{
	public IDType? ParentForumId { get; set; }
	public Forum ParentForum { get; set; }

	public IDType ForumId { get; set; }


	public string Title { get; set; }
	public string Description { get; set; }

	public bool Closed { get; set; } = false;
	public bool IsClosed => Closed || (this.ParentForum != null && this.ParentForum.Closed);
	public bool Deleted { get; set; } = false;

	public ulong Views { get; private set; } = 0;
	public ulong Viewed() => Views++;

}
