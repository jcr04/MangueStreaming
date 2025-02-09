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

            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var clickableUrl = $"{baseUrl}/api/Videos/play/{id}";
            return Ok(new { url = clickableUrl });
        }


        /// <summary>
        /// Retorna os metadados de todos os vídeos (cada um com a URL clicável).
        /// </summary>
        [HttpGet("all")]
        public async Task<IActionResult> GetAllVideos()
        {
            var command = new RetornaTodosVideosCommand();
            var videoList = await _retornaTodosVideosCommandHandler.Handle(command, CancellationToken.None);

            // Obtenha a base URL, ex.: "https://localhost:7159"
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            // Para cada vídeo, construa a URL para o endpoint de streaming (play)
            var result = videoList.Select(video => new
            {
                id = video.Id,
                url = $"{baseUrl}/api/Videos/play/{video.Id}",
                title = video.Title,
            }).ToList();

            return Ok(result);
        }


        [HttpGet("play/{id:guid}")]
        public async Task<IActionResult> PlayVideo(Guid id)
        {
            var command = new RetornaVideoCommand(id);
            var videoDto = await _retornaVideoCommandHandler.Handle(command, CancellationToken.None);
            if (videoDto == null)
            {
                return NotFound("Vídeo não encontrado.");
            }

            if (!System.IO.File.Exists(videoDto.Url))
            {
                return NotFound("Arquivo de vídeo não encontrado.");
            }

            var stream = new FileStream(videoDto.Url, FileMode.Open, FileAccess.Read, FileShare.Read);

            // Opcional: configure Content-Disposition para exibir inline
            Response.Headers.Add("Content-Disposition", "inline; filename=\"video.mp4\"");

            return new FileStreamResult(stream, "video/mp4");
        }
    }
}
