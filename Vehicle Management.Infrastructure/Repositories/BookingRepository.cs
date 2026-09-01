using Vehicle_Management.Domain.Aggregates.BookingAggregate;
using Vehicle_Management.Domain.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Vehicle_Management.Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly Context _context;

        public BookingRepository(Context context)
        {
            _context = context;
        }

        public async Task<Booking?> GetByIdAsync(int id)
        {
            return await _context.Bookings.FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Booking> UpdateAsync(Booking booking)
        {
            _context.Bookings.Update(booking);
 
            return booking;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var booking = await GetByIdAsync(id);
            if (booking == null)
                return false;

            _context.Bookings.Remove(booking);
 
            return true;
        }

        public async Task<IEnumerable<Booking>> GetAllAsync()
        {
            return await _context.Bookings.ToListAsync();
        }

        public async Task<Booking> AddAsync(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
 
            return booking;
        }
    }
}