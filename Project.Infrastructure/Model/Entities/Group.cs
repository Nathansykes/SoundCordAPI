namespace Project.Infrastructure.Model.Entities;

public partial class Group
{
    public Guid Id { get; set; }

    public string GroupName { get; set; } = null!;

    public string CreatedByUserId { get; set; } = null!;

    public virtual ICollection<Channel> Channels { get; set; } = new List<Channel>();

    public virtual AspNetUser CreatedByUser { get; set; } = null!;

    public virtual ICollection<AspNetUser> Users { get; set; } = new List<AspNetUser>();
}
