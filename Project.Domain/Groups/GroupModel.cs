using Project.Generic;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

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
    public List<IdNameModel> Channels { get; set; } = [];
    [SwaggerSchema(ReadOnly = true)]
    public ICollection<string> Users { get; set; } = [];
}