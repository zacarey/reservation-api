using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


[Table("appointment")]
public class Appointment
{
  [Key]
  public Guid appointment_id { get; set; }
  public Guid provider_id { get; set; }
  public DateTime timeslot { get; set; }
  public DateTime? reserved_at { get; set; }
  public Guid? reserved_by_client_id { get; set; }
  public bool confirmed { get; set; } = false;

  [ForeignKey("provider_id")]
  public Provider Provider { get; set; } = null!;

  [ForeignKey("reserved_by_client_id")]
  public Client? Client { get; set; }
}