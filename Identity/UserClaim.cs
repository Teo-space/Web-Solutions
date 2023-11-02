
public class UserClaim : UserClaim<TKey>
{

}

public abstract class UserClaim<TKeys> : IdentityUserClaim<TKeys> where TKeys : IEquatable<TKeys>
{
    public virtual User User { get; set; }
}
