using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Interfaces
{
    public interface IAttachmentServices
    {
        Task<string?> UploadAsync(Stream fileStream, string fileNmae, string FolderName, CancellationToken ct = default);

        bool Delete(string fileName, string FolderName);

        (Stream stream, string contentType)? GetFile(string fileName, string FolderName);

    }
}
