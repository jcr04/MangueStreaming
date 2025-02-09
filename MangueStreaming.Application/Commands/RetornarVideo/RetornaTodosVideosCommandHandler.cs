using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MangueStreaming.Application.Interfaces;
using MangueStreaming.Domain.Models;
using MangueStreaming.Domain.Repositories;

namespace MangueStreaming.Application.Commands.RetornarVideo
{
    public class RetornaTodosVideosCommandHandler : ICommandHandler<RetornaTodosVideosCommand, List<VideoUrlDto>>
    {
        private readonly IRetornaVideoRepository _videoRepository;

        public RetornaTodosVideosCommandHandler(IRetornaVideoRepository videoRepository)
        {
            _videoRepository = videoRepository;
        }

        public async Task<List<VideoUrlDto>> Handle(RetornaTodosVideosCommand command, CancellationToken cancellationToken)
        {
            return await _videoRepository.GetAllAsync();
        }
    }
}
