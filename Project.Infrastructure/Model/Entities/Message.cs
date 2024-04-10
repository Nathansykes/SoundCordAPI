namespace Project.Infrastructure.Model.Entities;

public partial class Message
{
    public Guid Id { get; set; }

    public string Content { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public DateTime Utc { get; set; }

    public virtual ChannelMessage? ChannelMessage { get; set; }

    public virtual AspNetUser User { get; set; } = null!;
}
