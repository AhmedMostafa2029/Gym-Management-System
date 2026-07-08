using GymSystem.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Classes
{
    public class AttachmentServices : IAttachmentServices
    {

        private readonly long maxFileSize = 5 * 1024 * 1024; // 5 MB
        private readonly string[] allowedExtentions = { ".jpg", ".png", ".jpeg" };
        private readonly ILogger<AttachmentServices> _logger;
        private readonly IWebHostEnvironment env;

        public AttachmentServices(ILogger<AttachmentServices> logger , IWebHostEnvironment env)
        {
            _logger = logger;
            this.env = env;
        }

        public bool Delete(string fileName, string FolderName)
        {
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(FolderName))
                return false;

            try
            {
                var filePath = Path.Combine(env.WebRootPath, FolderName, fileName);

                if (!File.Exists(filePath))
                    return false;
                File.Delete(filePath);
                return true;

            }catch(Exception ex)
            {
                _logger.LogError(ex, "Fialed to Delete The Attechment");
                return false;
            }

        }

        public (Stream stream, string contentType)? GetFile(string fileName, string FolderName)
        {
            throw new NotImplementedException();
        }

        public async Task<string?> UploadAsync(Stream fileStream, string fileNmae, string FolderName, CancellationToken ct = default)
        {
            if (fileStream == null || !fileStream.CanRead)
                return null;

            if (fileStream.Length == 0)
                return null;

            if(fileStream.Length >  maxFileSize)
            {
                _logger.LogWarning("Rejected File is Large");
                return null;
            }

            var extention = Path.GetExtension(fileNmae);

            if(string.IsNullOrEmpty(extention) || !allowedExtentions.Contains(extention))
            {
                _logger.LogWarning("Rejected Error Extention File");
                return null;
            }

            var UploadFolder = Path.Combine(env.WebRootPath, FolderName);
            Directory.CreateDirectory(UploadFolder);

            var StoredFileNme = $"{Guid.NewGuid()}{extention}";

            var filePath = Path.Combine(UploadFolder, StoredFileNme);

            try
            {
                await using var fs = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write, FileShare.None);

                await fileStream.CopyToAsync(fs);
                return StoredFileNme;
            }catch(Exception ex)
            {
                _logger.LogError(ex, "Filed upload File!");
                return null;
            }

        }
    }
}
