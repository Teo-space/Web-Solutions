namespace Web.Forums.Domain.Owned;



public record UserAction
{
	public Guid UserId {  get; init; } 
	public string UserName {  get; init; } 
	public DateTime At {  get; init; }

	public static UserAction Create(Guid UserId, string UserName)
	{
		return new UserAction()
		{
			UserId = UserId,
			UserName = UserName,
			At = DateTime.Now,
		};
	}
}



