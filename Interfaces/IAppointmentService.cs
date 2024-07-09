public interface IAppointmentService
{
  Task<IEnumerable<AppointmentDto>> GetAvailabilityAsync();
  Task SaveAvailabilityAsync(Guid providerId, List<(DateTime start, DateTime end)> availability);
  Task<bool> ReserveAppointmentAsync(Guid clientId, Guid appointmentId);
  Task<bool> ConfirmAppointmentAsync(Guid clientId, Guid appointmentId);
}