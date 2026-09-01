using Vehicle_Management.Domain.Aggregates.VehicleAggregate;
using Vehicle_Management.Domain.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Vehicle_Management.Infrastructure.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly Context _context;

        public VehicleRepository(Context context)
        {
            _context = context;
        }

        public async Task<Vehicle> GetByIdAsync(string id)
        {
            return await _context.Vehicles.FirstOrDefaultAsync(v => v.License_Plate == id);
        }

        public async Task<IEnumerable<Vehicle>> GetAllAsync()
        {
            return await _context.Vehicles.ToListAsync();
        }

        public async Task<Vehicle> AddAsync(Vehicle vehicle)
        {
            await _context.Vehicles.AddAsync(vehicle);
            return vehicle;
        }

        public async Task UpdateAsync(Vehicle vehicle)
        {
            _context.Vehicles.Update(vehicle);
        }

        public async Task DeleteAsync(Vehicle vehicle)
        {
            _context.Vehicles.Remove(vehicle);
        }
    }
}