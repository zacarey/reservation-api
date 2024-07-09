using Microsoft.AspNetCore.Mvc;

[ApiController]
// Consider annotating for authorization
[Route("appointment")]
public class AppointmentController : ControllerBase
{
  private readonly IAppointmentService _service;

  public AppointmentController(
    IAppointmentService service
  )
  {
    _service = service;
  }

  // Consider annotating endpoints for correct role authorization (e.g. client vs provider)
  [HttpGet]
  public async Task<IEnumerable<AppointmentDto>> GetAvailability()
    => await _service.GetAvailabilityAsync();

  [HttpPost]
  public async Task SaveAvailability([FromBody] AvailabilityRequest req)
    => await _service.SaveAvailabilityAsync(req.ProviderId, req.Availability);

  [HttpPost("reserve")]
  public async Task ReserveAppointment([FromBody] AppointmentRequest req)
    => await _service.ReserveAppointmentAsync(req.ClientId, req.AppointmentId);

  [HttpPost("confirm")]
  public async Task ConfirmAppointment([FromBody] AppointmentRequest req)
    => await _service.ConfirmAppointmentAsync(req.ClientId, req.AppointmentId);
}

public class AvailabilityRequest
{
  public Guid ProviderId { get; set; }
  public List<(DateTime, DateTime)> Availability { get; set; } = new List<(DateTime, DateTime)>();
}

public class AppointmentRequest
{
  public Guid ClientId { get; set; }
  public Guid AppointmentId { get; set; }
}