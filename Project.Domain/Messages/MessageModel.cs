using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Domain.Messages;
public class MessageModel
{
    [SwaggerSchema(ReadOnly = true)]
    public Guid Id { get; set; }

    [SwaggerSchema(ReadOnly = true)]
    public Guid ChannelId { get; set; }

    [SwaggerSchema(ReadOnly = true)]
    public string User { get; set; } = "";

    [Required(AllowEmptyStrings = false)]
    public string Content { get; set; } = "";

    [SwaggerSchema(ReadOnly = true)]
    public DateTime CreatedAt { get; set; }
}
