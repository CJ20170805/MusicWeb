using System;
using Microsoft.AspNetCore.Http;
using MusicApp.Application.Interfaces;
using MusicApp.Domain.Entities;
using MusicApp.Domain.Interfaces;
using Microsoft.Extensions.Hosting;

namespace MusicApp.Application.Services;

public class FileUploadService : IFileUploadService
{
    private readonly IFileUploadRepository _fileRepository;
    private readonly string _rootPath;

    public FileUploadService(IFileUploadRepository fileRepository, IHostEnvironment env)
    {
        _fileRepository = fileRepository;
        _rootPath = Path.Combine(env.ContentRootPath, "wwwroot","storage");
    }
    public async Task<Guid> UploadFileAsync(Stream fileStream, string fileName, FileType fileType)
    {
        ValidateFile(fileStream, fileName, fileType);

        string uploadFolder = GetUploadFolder(fileType);
        Console.WriteLine($"Upload folder: {uploadFolder}");
        string filePath = Path.Combine(uploadFolder, fileName);
        long fileSize = fileStream.Length;

        Console.WriteLine($"Uploading file: {filePath}");

        // Ensure the directory exists
        Directory.CreateDirectory(uploadFolder);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await fileStream.CopyToAsync(stream);
        }

        Console.WriteLine($"File uploaded: {filePath}");

        // get rid of /wwwroot/ part of the path
        filePath = filePath.Substring(filePath.IndexOf("storage"));

        var uploadedFile = new FileUpload
        (
            fileName,
            filePath,
            fileSize,
            fileType
        );

        await _fileRepository.AddAsync(uploadedFile);
        return uploadedFile.Id;
    }


    private void ValidateFile(Stream fileStream, string fileName, FileType fileType)
    {
        if (fileStream == null || fileStream.Length == 0 || string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentException("Invalid file");
        }
    }

    private string GetUploadFolder(FileType fileType)
    {
        string relativePath = fileType switch
        {
            FileType.Cover => "covers",
            FileType.Track => "tracks",
            FileType.Avatar => "avatars",
            _ => throw new ArgumentException("Invalid file type")
        };

        // Get the absolute path to the folder within wwwroot
        return Path.Combine(_rootPath, relativePath);
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
    private void ValidateFile(IFormFile file, FileType fileType)
    {
        if (file == null || file.Length == 0)
        {
            throw new ArgumentException("Invalid file");
        }
    }
}