namespace Domain.Entities
{
    public class FileMetadata
    {
        public string Key { get; set; }
        public string UserId { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
