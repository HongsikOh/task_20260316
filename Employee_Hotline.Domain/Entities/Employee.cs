namespace Employee_Hotline.Domain.Entities;

public sealed class Employee
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Tel { get; private set; } = string.Empty;
    public DateTime JoinedAt { get; private set; }

    private Employee() { }

    public static Employee Create(
        string name, string email, string tel, DateTime joinedAt)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        ArgumentException.ThrowIfNullOrWhiteSpace(tel);

        return new Employee
        {
            Id = Guid.NewGuid(),
            Name = name,
            Email = email,
            Tel = tel,
            JoinedAt = joinedAt
        };
    }
}