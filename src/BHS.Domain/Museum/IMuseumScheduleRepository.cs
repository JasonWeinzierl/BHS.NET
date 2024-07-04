using BHS.Contracts.Museum;

namespace BHS.Domain.Museum;

public interface IMuseumScheduleRepository
{
    Task<MuseumSchedule?> GetSchedule(CancellationToken cancellationToken = default);
    Task<MuseumSchedule> UpdateSchedule(MuseumSchedule schedule, CancellationToken cancellationToken = default);
}
