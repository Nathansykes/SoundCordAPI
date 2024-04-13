namespace Project.Infrastructure.Model.Entities;

public partial class FileMetadatum
{
    public Guid Id { get; set; }

    public string UploadedByUserId { get; set; } = null!;

    public DateTime UploadedUtc { get; set; }

    public string OriginalFileName { get; set; } = null!;

    public string OriginalExtension { get; set; } = null!;

    public string NewFileName { get; set; } = null!;

    public string ContentHash { get; set; } = null!;

    public Guid Directory { get; set; }

    public string FileShare { get; set; } = null!;

    public virtual ICollection<SongRevision> SongRevisions { get; set; } = new List<SongRevision>();

    public virtual AspNetUser UploadedByUser { get; set; } = null!;
}
