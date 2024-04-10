using System;
using System.Collections.Generic;

namespace Project.Infrastructure.Model.Entities;

public partial class Channel
{
    public Guid Id { get; set; }

    public Guid GroupId { get; set; }

    public string ChannelName { get; set; } = null!;

    public virtual ICollection<ChannelMessage> ChannelMessages { get; set; } = new List<ChannelMessage>();

    public virtual Group Group { get; set; } = null!;

    public virtual Song? Song { get; set; }
}
