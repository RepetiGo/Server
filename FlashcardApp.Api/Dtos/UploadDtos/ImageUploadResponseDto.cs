﻿namespace FlashcardApp.Api.Dtos.UploadDtos
{
    public class ImageUploadResponseDto
    {
        public bool IsSuccess { get; set; }
        public string SecureUrl { get; set; } = string.Empty;
        public string PublicId { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
