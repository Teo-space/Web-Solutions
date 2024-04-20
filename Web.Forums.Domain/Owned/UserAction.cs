namespace Web.Forums.Domain.Owned;



public record UserAction
(
	Guid UserId, 
	string UserName, 
	DateTime At
);



