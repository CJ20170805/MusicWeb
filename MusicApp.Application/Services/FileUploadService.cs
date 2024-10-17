using System;
using Microsoft.AspNetCore.Http;
using MusicApp.Application.Interfaces;
using MusicApp.Domain.Entities;
using MusicApp.Domain.Interfaces;

namespace MusicApp.Application.Services;

public class FileUploadService : IFileUploadService
{
    private readonly IFileUploadRepository _fileRepository;
    public FileUploadService(IFileUploadRepository fileRepository)
    {
        _fileRepository = fileRepository;
    }
    public async Task<Guid> UploadFileAsync(IFormFile file, FileType fileType)
    {
        ValidateFile(file, fileType);

        string uploadFolder = GetUploadFolder(fileType);
        string filePath = Path.Combine(uploadFolder, file.FileName);
        long fileSize = file.Length;

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
           await file.CopyToAsync(stream);
        }

        var uploadedFile = new FileUpload
        (
            file.FileName,
            filePath,
            fileSize,
            fileType
        );

        // save
        await _fileRepository.AddAsync(uploadedFile);

        return uploadedFile.Id;
    }
    
    private string GetUploadFolder(FileType fileType)
    {
        return fileType switch
        {
            FileType.Cover => "/Users/garycho/Codes/FileUpload/Covers",
            FileType.Track => "/Users/garycho/Codes/FileUpload/Tracks",
            FileType.Avatar => "/Users/garycho/Codes/FileUpload/Avatars",
            _ => throw new ArgumentException("Invalid file type")
        };
    }

    private void ValidateFile(IFormFile file, FileType fileType)
    {
         if (file == null || file.Length == 0)
        {
            throw new ArgumentException("Invalid file");
        }
    }
}
