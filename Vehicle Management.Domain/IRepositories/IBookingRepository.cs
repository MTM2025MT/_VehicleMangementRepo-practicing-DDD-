using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vehicle_Management.Domain.Aggregates.BookingAggregate;
using Vehicle_Management.Domain.Aggregates.VehicleAggregate;

namespace Vehicle_Management.Domain.IRepositories
{
    public interface IBookingRepository
    {
        public Task<Booking?> GetByIdAsync(int id);
        public Task<Booking> UpdateAsync(Booking booking);
        public Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Booking>> GetAllAsync();
        public Task<Booking> AddAsync(Booking booking);
    }
}
