namespace Backend.Src.RestDto
{
    public class GenerationDto
    {
        public int Id { get; set; }

        public string Image { get; set; }

        public string Promt { get; set; }

        public int Rating { get; set; }

        public bool IsPublic { get; set; }
    }
}
