using System;
using System.Threading.Tasks;
using MangueStreaming.Domain.Models;

namespace MangueStreaming.Domain.Repositories
{
    public interface IVideoRepository
    {
        Task AddAsync(Video video);
        Task<Video?> GetByIdAsync(Guid id);
    }
}
