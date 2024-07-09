using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


[Table("client")]
public class Client
{
  [Key]
  public Guid client_id { get; set; }
  public string? name { get; set; }
}