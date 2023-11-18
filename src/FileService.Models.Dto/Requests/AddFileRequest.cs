namespace UniversityHelper.FileService.Models.Dto.Requests
{
    public record AddFileRequest
    {
        public string Content { get; set; }
        public string Extension { get; set; }
        public string Name { get; set; }
    }
}
