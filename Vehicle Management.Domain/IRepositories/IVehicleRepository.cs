using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vehicle_Management.Domain.Aggregates;
using Vehicle_Management.Domain.Aggregates.VehicleAggregate;

namespace Vehicle_Management.Domain.IRepositories
{
    public interface IVehicleRepository
    {
        Task<Vehicle> GetByIdAsync(string id);
        Task<IEnumerable<Vehicle>> GetAllAsync();
        Task<Vehicle> AddAsync(Vehicle vehicle);
        Task UpdateAsync(Vehicle vehicle);
        Task DeleteAsync(Vehicle vehicle);
    }
}
