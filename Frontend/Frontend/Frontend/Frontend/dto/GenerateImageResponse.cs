namespace Frontend.dto
{
    public class GenerateImageResponse
    {
        public string Base64Image { get; set; }
        public int Id { get; set; }
        
        public int Rating { get; set; }
        public bool IsPublic { get; set; }
    }
}