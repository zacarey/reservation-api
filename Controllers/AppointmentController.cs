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
  {
    List<(DateTime start, DateTime end)> availability = req.Availability.Select(i => (i.Start, i.End)).ToList();
    await _service.SaveAvailabilityAsync(req.ProviderId, availability);
  }

  [HttpPost("reserve")]
  public async Task<IActionResult> ReserveAppointment([FromBody] AppointmentRequest req)
    => Ok(await _service.ReserveAppointmentAsync(req.ClientId, req.AppointmentId) ? "Reserved" : "Not Reserved");

  [HttpPost("confirm")]
  public async Task<IActionResult> ConfirmAppointment([FromBody] AppointmentRequest req)
    => Ok(await _service.ConfirmAppointmentAsync(req.ClientId, req.AppointmentId) ? "Confirmed" : "Not Confirmed");
}

public class AvailabilityRequest
{
  public Guid ProviderId { get; set; }
  public List<AvailabilityStartEndRequest> Availability { get; set; } = new List<AvailabilityStartEndRequest>();
}

public class AvailabilityStartEndRequest
{
  public DateTime Start { get; set; }
  public DateTime End { get; set; }
}

public class AppointmentRequest
{
  public Guid ClientId { get; set; }
  public Guid AppointmentId { get; set; }
}