using System;
using System.Threading;
using System.Threading.Tasks;
using MangueStreaming.Application.Interfaces;
using MangueStreaming.Domain.Models;
using MangueStreaming.Domain.Repositories;

namespace MangueStreaming.Application.Commands.CriarVideo
{
    public class CreateVideoCommandHandler : ICommandHandler<CreateVideoCommand, Guid>
    {
        private readonly IVideoRepository _videoRepository;

        public CreateVideoCommandHandler(IVideoRepository videoRepository)
        {
            _videoRepository = videoRepository;
        }

        public async Task<Guid> Handle(CreateVideoCommand command, CancellationToken cancellationToken)
        {
            var video = new Video(
                Guid.NewGuid(),
                command.Title,
                command.Description,
                command.Url,
                DateTime.UtcNow
            );

            await _videoRepository.AddAsync(video);
            return video.Id;
        }
    }
}
