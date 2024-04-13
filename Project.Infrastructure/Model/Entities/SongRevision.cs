namespace Project.Infrastructure.Model.Entities;

public partial class SongRevision
{
    public Guid Id { get; set; }

    public Guid SongId { get; set; }

    public string? RevisionName { get; set; }

    public string CreatedByUserId { get; set; } = null!;

    public DateTime CreatedUtc { get; set; }

    public Guid? FileMetaDataId { get; set; }

    public int LengthMilliseconds { get; set; }

    public virtual AspNetUser CreatedByUser { get; set; } = null!;

    public virtual FileMetadatum? FileMetaData { get; set; }

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual Song Song { get; set; } = null!;
}
