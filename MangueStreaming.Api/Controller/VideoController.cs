using System.IO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using MangueStreaming.Application.Commands;
using MangueStreaming.Application.Commands.RetornarVideo;
using MangueStreaming.Application.Interfaces;
using MangueStreaming.Application.Commands.CriarVideo;
using MangueStreaming.Domain.Models;

namespace MangueStreaming.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VideosController : ControllerBase
    {
        private readonly ICommandHandler<CreateVideoCommand, Guid> _createVideoCommandHandler;
        private readonly ICommandHandler<RetornaVideoCommand, VideoUrlDto?> _retornaVideoCommandHandler;

        public VideosController(
            ICommandHandler<CreateVideoCommand, Guid> createVideoCommandHandler,
            ICommandHandler<RetornaVideoCommand, VideoUrlDto?> retornaVideoCommandHandler)
        {
            _createVideoCommandHandler = createVideoCommandHandler;
            _retornaVideoCommandHandler = retornaVideoCommandHandler;
        }

        /// <summary>
        /// Cria um novo vídeo.
        /// </summary>
        /// <param name="command">Dados do vídeo a ser criado</param>
        /// <returns>Id do vídeo criado</returns>
        [HttpPost]
        public async Task<IActionResult> CreateVideo([FromBody] CreateVideoCommand command)
        {
            var videoId = await _createVideoCommandHandler.Handle(command, CancellationToken.None);
            return CreatedAtAction(nameof(GetVideo), new { id = videoId }, new { id = videoId });
        }

        /// <summary>
        /// Retorna os metadados de um vídeo (neste exemplo, somente a URL clicável).
        /// </summary>
        /// <param name="id">Id do vídeo</param>
        /// <returns>URL do vídeo</returns>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetVideo(Guid id)
        {
            var command = new RetornaVideoCommand(id);
            var videoDto = await _retornaVideoCommandHandler.Handle(command, CancellationToken.None);
            if (videoDto == null)
            {
                return NotFound();
            }

            // Supondo que videoDto.Url contenha um caminho local, ex:
            // "C:\Users\Usuario\Documents\videos\prazeramor.mp4"
            // Convertemos para uma URI do tipo file:///
            // Método 1: Usando o construtor de Uri (se o caminho for absoluto)
            var fileUri = new Uri(videoDto.Url).AbsoluteUri;

            return Ok(new { url = fileUri });
        }
    }
}
