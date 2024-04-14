
public  class UserToken : UserToken<TKey>
{

}

public abstract class UserToken<TKeys> : IdentityUserToken<TKeys> where TKeys : IEquatable<TKeys>
{
    public virtual User User { get; set; }
}
