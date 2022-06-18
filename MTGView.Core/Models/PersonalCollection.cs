namespace MTGView.Core.Models;

public class PersonalCollection
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<PersonalCardMapping> CardMappings { get; set; } = new HashSet<PersonalCardMapping>(20);
}

