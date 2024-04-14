using Project.Domain.Files.FileUploading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Infrastructure.Model.Entities;

public partial class FileMetadatum : IFileModel
{
    string? IFileModel.Content { get; }
    string IFileInfo.Extension => OriginalExtension;
}