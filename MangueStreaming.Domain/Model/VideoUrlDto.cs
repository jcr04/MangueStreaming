using System;

namespace MangueStreaming.Domain.Models
{
    public class VideoUrlDto
    {
        public Guid Id { get; set; }
        public string Url { get; set; } = string.Empty;
    }
}
