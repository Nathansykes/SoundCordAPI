namespace Project.Infrastructure.Model.Entities;

public partial class Message
{
    public Guid Id { get; set; }

    public Guid ChannelId { get; set; }

    public string Content { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public DateTime Utc { get; set; }

    public Guid? SongRevisionId { get; set; }

    public int? SongTimestampMilliseconds { get; set; }

    public virtual Channel Channel { get; set; } = null!;

    public virtual SongRevision? SongRevision { get; set; }

    public virtual AspNetUser User { get; set; } = null!;
}
