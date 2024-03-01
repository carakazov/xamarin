using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Model;

[Table("History")]
public class HistoryEntity
{
    [Key]
    public int Id { get; set; }
    
    public UserEntity User { get; set; }
    
    public string Image { get; set; }
    
    public string Result { get; set; }
}