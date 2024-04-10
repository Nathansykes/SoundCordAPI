using System;
using System.Collections.Generic;

namespace Project.Infrastructure.Model.Entities;

public partial class AspNetUser
{
    public string Id { get; set; } = null!;

    public string? UserName { get; set; }

    public string? NormalizedUserName { get; set; }

    public string? Email { get; set; }

    public string? NormalizedEmail { get; set; }

    public bool EmailConfirmed { get; set; }

    public string? PasswordHash { get; set; }

    public string? SecurityStamp { get; set; }

    public string? ConcurrencyStamp { get; set; }

    public string? PhoneNumber { get; set; }

    public bool PhoneNumberConfirmed { get; set; }

    public bool TwoFactorEnabled { get; set; }

    public DateTimeOffset? LockoutEnd { get; set; }

    public bool LockoutEnabled { get; set; }

    public int AccessFailedCount { get; set; }

    public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; } = new List<AspNetUserClaim>();

    public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; } = new List<AspNetUserLogin>();

    public virtual ICollection<AspNetUserToken> AspNetUserTokens { get; set; } = new List<AspNetUserToken>();

    public virtual ICollection<FileMetadatum> FileMetadata { get; set; } = new List<FileMetadatum>();

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual ICollection<SongRevision> SongRevisions { get; set; } = new List<SongRevision>();

    public virtual ICollection<Song> Songs { get; set; } = new List<Song>();

    public virtual ICollection<Group> GroupsNavigation { get; set; } = new List<Group>();

    public virtual ICollection<AspNetRole> Roles { get; set; } = new List<AspNetRole>();
}
