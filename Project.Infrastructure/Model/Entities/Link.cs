namespace Project.Infrastructure.Model.Entities;

public partial class Link
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string LinkUrl { get; set; } = null!;

    public string? Icon { get; set; }

    public int SortOrder { get; set; }
}
