
public class RoleClaim : RoleClaim<TKey>
{

}

public abstract class RoleClaim<TKeys> : IdentityRoleClaim<TKeys> where TKeys : IEquatable<TKeys>
{
    public virtual Role Role { get; set; }
}