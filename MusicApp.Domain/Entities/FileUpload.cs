using System;

namespace MusicApp.Domain.Entities;

public class FileUpload
{
    public Guid Id { get; private set; }
    public string? FileName { get; private set; }  
    public string? FilePath { get; private set; }
    public long FileSize { get; private set; }
    public DateTime UploadDate { get; private set; }
    public FileType Type { get; private set; }

    public  FileUpload(string fileName, string filePath, long fileSize, FileType type)
    {
        Id = Guid.NewGuid();
        FileName = fileName;
        FilePath = filePath;
        FileSize = fileSize;
        UploadDate = DateTime.Now;
        Type = type;
    }

}

public enum FileType
{
    Track,
    Cover,
    Avatar
}


