public  class UserLogin: UserLogin<TKey>
{

}

public abstract class UserLogin<TKeys> : IdentityUserLogin<TKeys> where TKeys : IEquatable<TKeys>
{
    public virtual User User { get; set; }
}