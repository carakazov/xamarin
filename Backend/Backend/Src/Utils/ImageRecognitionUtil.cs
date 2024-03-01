namespace Backend.Utils;

public class ImageRecognitionUtil
{
    private static readonly List<String> RESULTS = new List<string>()
    {
        "Розы",
        "Ландыши",
        "Георгины",
        "Цветок, определить не получается",
        "Это вообще не цветок",
        "Ромашка"
    };
    
    public static string RecognizeImage(string base64Image)
    {
        var random = new Random();
        int index = random.Next(RESULTS.Count);
        return RESULTS[index];
    }
}