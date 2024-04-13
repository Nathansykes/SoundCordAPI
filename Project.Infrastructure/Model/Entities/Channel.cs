namespace Project.Infrastructure.Model.Entities;

public partial class Channel
{
    public Guid Id { get; set; }

    public Guid GroupId { get; set; }

    public string ChannelName { get; set; } = null!;

    public virtual Group Group { get; set; } = null!;

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual Song? Song { get; set; }
}
