
public class User : User<TKey> 
{ 

}

public abstract class User<TKeys> : IdentityUser<TKeys> where TKeys : IEquatable<TKeys>
{
    public virtual ICollection<UserClaim> Claims { get; set; }
    public virtual ICollection<UserLogin> Logins { get; set; }
    public virtual ICollection<UserToken> Tokens { get; set; }
    public virtual ICollection<UserRole> UserRoles { get; set; }
}


