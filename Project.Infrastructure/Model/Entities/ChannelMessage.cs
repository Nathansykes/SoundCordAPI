using System;
using System.Collections.Generic;

namespace Project.Infrastructure.Model.Entities;

public partial class ChannelMessage
{
    public Guid MessageId { get; set; }

    public Guid ChannelId { get; set; }

    public Guid? SongRevisionId { get; set; }

    public virtual Channel Channel { get; set; } = null!;

    public virtual Message Message { get; set; } = null!;

    public virtual SongRevision? SongRevision { get; set; }
}
