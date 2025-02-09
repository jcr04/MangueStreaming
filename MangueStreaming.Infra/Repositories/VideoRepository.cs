using MangueStreaming.Domain.Models;
using MangueStreaming.Domain.Repositories;
using MangueStreaming.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace MangueStreaming.Infra.Repositories
{
    public class VideoRepository : IVideoRepository
    {
        private readonly MangueStreamingDbContext _dbContext;

        public VideoRepository(MangueStreamingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Video video)
        {
            await _dbContext.Videos.AddAsync(video);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Video?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Videos.FirstOrDefaultAsync(v => v.Id == id);
        }
    }
}
