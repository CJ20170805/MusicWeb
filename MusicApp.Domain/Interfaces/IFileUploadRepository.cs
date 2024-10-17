using System;
using MusicApp.Domain.Entities;

namespace MusicApp.Domain.Interfaces;

public interface IFileUploadRepository
{
    Task AddAsync(FileUpload file);
    Task<FileUpload> GetByIdAsync(Guid id);
    Task DeleteAsync(Guid id);
    Task UpdateAsync(FileUpload file);
}
