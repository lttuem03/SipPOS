namespace SipPOS.Models.General;

/// <summary>
/// Represents a position in the store.
/// </summary>
public class Position : IComparable<Position>
{
    /// <summary>
    /// Gets the access level of the position.
    /// </summary>
    public int AccessLevel
    {
        get;
    }

    /// <summary>
    /// Gets the name of the position.
    /// </summary>
    public string Name
    {
        get;
    }

    /// <summary>
    /// Represents the Staff position.
    /// </summary>
    public static readonly Position Staff = new(1, "Nhân viên cửa hàng");

    /// <summary>
    /// Represents the Assistant Manager position.
    /// </summary>
    public static readonly Position AssistantManager = new(2, "Trợ lý cửa hàng");

    /// <summary>
    /// Represents the Store Manager position.
    /// </summary>
    public static readonly Position StoreManager = new(3, "Quản lý cửa hàng");

    /// <summary>
    /// Compares the current position with another position based on access level.
    /// </summary>
    /// <param name="other">The other position to compare to.</param>
    /// <returns>A value indicating the relative order of the positions being compared.</returns>
    public int CompareTo(Position? other)
    {
        if (other == null)
        {
            return -1;
        }

        return AccessLevel.CompareTo(other.AccessLevel);
    }

    /// <summary>
    /// Returns the name of the position.
    /// </summary>
    /// <returns>The name of the position.</returns>
    public override string ToString()
    {
        return Name;
    }

    /// <summary>
    /// Gets a position based on the given prefix.
    /// </summary>
    /// <param name="prefix">The prefix of the position.</param>
    /// <returns>The position corresponding to the prefix, or null if no match is found.</returns>
    public static Position FromPrefix(string prefix)
    {
        switch (prefix)
        {
            case "ST":
                return Staff;
            case "AM":
                return AssistantManager;
            case "SM":
                return StoreManager;
        }

        // if unknown prefix
        return Staff;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Position"/> class.
    /// </summary>
    /// <param name="accessLevel">The access level of the position.</param>
    /// <param name="name">The name of the position.</param>
    private Position(int accessLevel, string name)
    {
        AccessLevel = accessLevel;
        Name = name;
    }
}
