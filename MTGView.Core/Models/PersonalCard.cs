namespace MTGView.Core.Models;

public class PersonalCard
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string SetCode { get; set; } = null!;

    public virtual ICollection<PersonalCardMapping> CardMappings { get; set; } = new HashSet<PersonalCardMapping>(20);
}

