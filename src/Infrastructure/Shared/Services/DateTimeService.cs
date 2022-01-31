using System;
using Application.Interfaces;

namespace Infrastructure.Shared.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime Now => DateTime.Now;
    }
}
