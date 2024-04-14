
public class UserRole : UserRole<TKey>
{

}

public abstract class UserRole<TKeys> : IdentityUserRole<TKeys> where TKeys : IEquatable<TKeys>
{
    public virtual User User { get; set; }
    public virtual Role Role { get; set; }
}