using Microsoft.EntityFrameworkCore;

public class AppointmentService : IAppointmentService
{
  private readonly DatabaseContext _context;
  public AppointmentService(DatabaseContext context)
  {
    _context = context;
  }

  public async Task<bool> ConfirmAppointmentAsync(Guid clientId, Guid appointmentId)
  {
    var appointment = await _context.Appointments.FindAsync(appointmentId);

    if (appointment is null || appointment.reserved_by_client_id != clientId) // do not allow confirmation for appointments not reserved by client
    {
      return false;
    }

    appointment.confirmed = true;
    await _context.SaveChangesAsync();
    return true;
  }

  public async Task<IEnumerable<AppointmentDto>> GetAvailabilityAsync()
  {
    var currentTime = DateTime.UtcNow;
    var minNoticeDate = currentTime.AddHours(24);
    var expiredReservationDate = currentTime.AddMinutes(-30);

    var availabilities = await _context.Appointments
      .Where(i =>
        i.timeslot > minNoticeDate // appt is in the future and provides 24hr notice
        && !i.confirmed // appt isn't confirmed
        &&
          (
            i.reserved_at == null
            || i.reserved_at < expiredReservationDate // filter out reservations made in the last 30 min
          )
        ).ToListAsync();

    return availabilities.Select(i => new AppointmentDto
    {
      appointment_id = i.appointment_id,
      provider_id = i.provider_id,
      timeslot = i.timeslot,
    });
  }

  public async Task<bool> ReserveAppointmentAsync(Guid clientId, Guid appointmentId)
  {
    var currentTime = DateTime.UtcNow;
    var validRsvpDate = currentTime.AddMinutes(-30);

    var appointment = await _context.Appointments
      .FindAsync(appointmentId);

    if (appointment is null || appointment.confirmed ||
      (appointment.reserved_by_client_id != clientId && appointment.reserved_at is not null && appointment.reserved_at > validRsvpDate)
    )
    {
      return false;
    }

    appointment.reserved_by_client_id = clientId;
    appointment.reserved_at = currentTime;

    await _context.SaveChangesAsync();
    return true;
  }

  // assumption- provider may have multiple timeslots concurrently 
  // assumption- timeslots start on the hour in 15 min increments
  public async Task SaveAvailabilityAsync(Guid providerId, List<(DateTime start, DateTime end)> availability)
  {
    List<Appointment> appointments = new();
    foreach (var period in availability)
    {
      var sessionStartTime = period.start;
      var sessionEndTime = period.end.AddMinutes(-1);
      while (sessionStartTime < sessionEndTime)
      {
        var curAppointment = new Appointment
        {
          provider_id = providerId,
          timeslot = sessionStartTime,
        };
        appointments.Add(curAppointment);
        sessionStartTime = sessionStartTime.AddMinutes(15);
      }
    }

    if (appointments.Count == 0) return;

    await _context.Appointments.AddRangeAsync(appointments);
    await _context.SaveChangesAsync();
  }
}