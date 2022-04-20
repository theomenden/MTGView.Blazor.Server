#nullable disable
namespace MTGView.Core.Models;
public class PersonalCardMapping
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid PersonalCollectionId { get; set; }
    public int CardId { get; set; }

    public virtual PersonalCollection Collection { get; set; }

    public virtual PersonalCard Card { get; set; }
}