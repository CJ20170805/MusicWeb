using System;
using MusicApp.Domain.Entities;
using MusicApp.Domain.Interfaces;
using MusicApp.Infrastructure.Data;

namespace MusicApp.Infrastructure.Repositories;

public class FileUploadRepository : IFileUploadRepository
{
    private readonly MusicDbContext _context;
    public FileUploadRepository(MusicDbContext context)
    {
        _context = context;
    }
    public async Task AddAsync(FileUpload file)
    {
        await _context.FileUploads.AddAsync(file);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var file = _context.FileUploads.Find(id);
        if (file != null)
        {
            _context.FileUploads.Remove(file);
            await _context.SaveChangesAsync();
        }
    }

    public Task<FileUpload> GetByIdAsync(Guid id)
    {
        var file = _context.FileUploads.Find(id);
        if (file == null)
        {
            throw new Exception("File not found");
        }
        return Task.FromResult(file);
    }

    public Task UpdateAsync(FileUpload file)
    {
       var existingFile = _context.FileUploads.Find(file.Id);
       if (existingFile == null)
       {
           throw new Exception("File not found");
       }
       _context.Entry(existingFile).CurrentValues.SetValues(file);
       return _context.SaveChangesAsync();
    }
}
