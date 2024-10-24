using System;
using Microsoft.AspNetCore.Http;
using MusicApp.Domain.Entities;

namespace MusicApp.Application.Interfaces;

public interface IFileUploadService
{
    Task<Guid> UploadFileAsync(Stream fileStream, string fileName, FileType fileType);
    Task<Guid> UploadFileAsync(IFormFile file, FileType fileType);
}
