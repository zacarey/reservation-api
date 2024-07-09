using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


[Table("provider")]
public class Provider
{
  [Key]
  public Guid provider_id { get; set; }
  public string? name { get; set; }
}