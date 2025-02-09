using System;
using System.Threading;
using System.Threading.Tasks;
using MangueStreaming.Application.Interfaces;
using MangueStreaming.Domain.Models;
using MangueStreaming.Domain.Repositories;

namespace MangueStreaming.Application.Commands.RetornarVideo
{
    public class RetornaVideoCommandHandler : ICommandHandler<RetornaVideoCommand, VideoUrlDto?>
    {
        private readonly IRetornaVideoRepository _videoRepository;

        public RetornaVideoCommandHandler(IRetornaVideoRepository videoRepository)
        {
            _videoRepository = videoRepository;
        }

        public async Task<VideoUrlDto?> Handle(RetornaVideoCommand command, CancellationToken cancellationToken)
        {
            return await _videoRepository.GetByIdAsync(command.VideoId);
        }
    }
}
