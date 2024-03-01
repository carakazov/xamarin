using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Model;

[Table("Generation")]
public class GenerationEntity
{
    [Key]
    public int Id { get; set; }

    public UserEntity User { get; set; }

    public string Image { get; set; }

    public string Promt { get; set; }

    public int Rating {  get; set; }

    public bool IsPublic { get; set; }
}