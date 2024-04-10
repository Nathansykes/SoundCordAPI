using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Project.Domain.Groups;

public class GroupModel
{
    [SwaggerSchema(ReadOnly = true)]
    public Guid Id { get; set; }

    [Required]
    public string GroupName { get; set; } = "";


    [SwaggerSchema(ReadOnly = true)]
    public string CreatedByUserId { get; set; } = "";
    [SwaggerSchema(ReadOnly = true)]
    public Dictionary<Guid, string> Channels { get; set; } = [];
    [SwaggerSchema(ReadOnly = true)]
    public ICollection<string> Users { get; set; } = [];
}