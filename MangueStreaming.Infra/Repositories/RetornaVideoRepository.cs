using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using MangueStreaming.Domain.Models;
using MangueStreaming.Domain.Repositories;
using MangueStreaming.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace MangueStreaming.Infra.Repositories
{
    public class RetornaVideoRepository : IRetornaVideoRepository
    {
        private readonly MangueStreamingDbContext _context;

        public RetornaVideoRepository(MangueStreamingDbContext context)
        {
            _context = context;
        }

        public async Task<VideoUrlDto?> GetByIdAsync(Guid id)
        {
            return await _context.Videos
                .Where(v => v.Id == id)
                .Select(v => new VideoUrlDto
                {
                    Id = v.Id,
                    Url = v.Url,
                    Title = v.Title,
                })
                .FirstOrDefaultAsync();
        }

        public async Task<List<VideoUrlDto>> GetAllAsync()
        {
            return await _context.Videos
                .Select(v => new VideoUrlDto
                {
                    Id = v.Id,
                    Url = v.Url,
                    Title = v.Title,
                })
                .ToListAsync();
        }
    }
}
