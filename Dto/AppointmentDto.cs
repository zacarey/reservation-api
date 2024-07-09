public class AppointmentDto
{
  public Guid appointment_id { get; set; }
  public Guid provider_id { get; set; }
  public DateTime timeslot { get; set; }
}