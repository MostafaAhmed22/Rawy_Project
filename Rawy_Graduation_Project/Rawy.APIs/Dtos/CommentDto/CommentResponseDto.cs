﻿namespace Rawy.APIs.Dtos.CommentDto
{
    public class CommentResponseDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string WriterName { get; set; }

        public string StoryTitle { get; set; }
    }
}
