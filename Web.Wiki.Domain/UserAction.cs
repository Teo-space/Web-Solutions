namespace Web.Wiki.Domain;

public record UserAction(Guid UserId, string UserName, DateTime At)
{
}
