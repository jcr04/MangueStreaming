using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using MangueStreaming.Application.Commands;
using MangueStreaming.Application.Commands.CriarVideo;
using MangueStreaming.Application.Commands.RetornarVideo;
using MangueStreaming.Application.Interfaces;
using MangueStreaming.Domain.Models;

namespace MangueStreaming.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VideosController : ControllerBase
    {
        private readonly ICommandHandler<CreateVideoCommand, Guid> _createVideoCommandHandler;
        private readonly ICommandHandler<RetornaVideoCommand, VideoUrlDto?> _retornaVideoCommandHandler;
        private readonly ICommandHandler<RetornaTodosVideosCommand, List<VideoUrlDto>> _retornaTodosVideosCommandHandler;

        public VideosController(
            ICommandHandler<CreateVideoCommand, Guid> createVideoCommandHandler,
            ICommandHandler<RetornaVideoCommand, VideoUrlDto?> retornaVideoCommandHandler,
            ICommandHandler<RetornaTodosVideosCommand, List<VideoUrlDto>> retornaTodosVideosCommandHandler)
        {
            _createVideoCommandHandler = createVideoCommandHandler;
            _retornaVideoCommandHandler = retornaVideoCommandHandler;
            _retornaTodosVideosCommandHandler = retornaTodosVideosCommandHandler;
        }

        /// <summary>
        /// Cria um novo vídeo.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateVideo([FromBody] CreateVideoCommand command)
        {
            var videoId = await _createVideoCommandHandler.Handle(command, CancellationToken.None);
            return CreatedAtAction(nameof(GetVideo), new { id = videoId }, new { id = videoId });
        }

        /// <summary>
        /// Retorna os metadados de um vídeo (neste exemplo, somente a URL clicável).
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetVideo(Guid id)
        {
            var command = new RetornaVideoCommand(id);
            var videoDto = await _retornaVideoCommandHandler.Handle(command, CancellationToken.None);
            if (videoDto == null)
            {
                return NotFound();
            }

            // Converte o caminho local em uma URI do tipo file:///
            var fileUri = new Uri(videoDto.Url).AbsoluteUri;
            return Ok(new { url = fileUri });
        }

        /// <summary>
        /// Retorna os metadados de todos os vídeos (cada um com a URL clicável).
        /// </summary>
        [HttpGet("all")]
        public async Task<IActionResult> GetAllVideos()
        {
            var command = new RetornaTodosVideosCommand();
            var videoList = await _retornaTodosVideosCommandHandler.Handle(command, CancellationToken.None);

            // Converte cada caminho local em uma URI do tipo file:///.
            // Caso videoDto.Url seja, por exemplo, "C:\Users\Usuario\Documents\videos\video1.mp4"
            // new Uri(...) gerará "file:///C:/Users/Usuario/Documents/videos/video1.mp4"
            var result = videoList.Select(video => new
            {
                id = video.Id,
                url = new Uri(video.Url).AbsoluteUri
            }).ToList();

            return Ok(result);
        }
    }
}
