using System;
using System.Collections.Generic;

namespace Project.Infrastructure.Model.Entities;

public partial class Song
{
    public Guid Id { get; set; }

    public string SongName { get; set; } = null!;

    public Guid GroupId { get; set; }

    public string CreatedByUserId { get; set; } = null!;

    public Guid ChannelId { get; set; }

    public virtual Channel Channel { get; set; } = null!;

    public virtual AspNetUser CreatedByUser { get; set; } = null!;

    public virtual Group Group { get; set; } = null!;

    public virtual ICollection<SongRevision> SongRevisions { get; set; } = new List<SongRevision>();
}
