using System;

namespace MangueStreaming.Domain.Models
{
    public class Video
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string Url { get; private set; }
        public DateTime UploadedAt { get; private set; }

        public Video(Guid id, string title, string description, string url, DateTime uploadedAt)
        {
            Id = id;
            Title = title;
            Description = description;
            Url = url;
            UploadedAt = uploadedAt;
        }
    }
}
