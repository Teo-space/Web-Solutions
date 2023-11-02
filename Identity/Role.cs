public class Role : Role<TKey>
{
    public Role() : base()
    {
    }

    public Role(string roleName) : base(roleName)
    {

    }
}

public abstract class Role<TKeys> : IdentityRole<TKeys> where TKeys : IEquatable<TKeys>
{
    public Role() : base()
    {
    }

    public Role(string roleName) : base(roleName)
    {

    }
    public virtual ICollection<UserRole> UserRoles { get; set; }
    public virtual ICollection<RoleClaim> RoleClaims { get; set; }
}

















