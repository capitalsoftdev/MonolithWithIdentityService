using App.Application.Common.Interfaces;

namespace App.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}
