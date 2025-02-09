using System;
using System.Threading.Tasks;
using MangueStreaming.Domain.Models;

namespace MangueStreaming.Domain.Repositories
{
    public interface IRetornaVideoRepository
    {
        Task<VideoUrlDto?> GetByIdAsync(Guid id);
    }
}
