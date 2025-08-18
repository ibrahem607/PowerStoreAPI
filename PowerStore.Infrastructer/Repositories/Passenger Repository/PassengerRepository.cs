using Microsoft.EntityFrameworkCore;
using PowerStore.Core.Contract.Passenger_Contract;
using PowerStore.Core.Entities;
using PowerStore.Infrastructer.Data.Context;

namespace PowerStore.Infrastructer.Repositories.Passenger_Repository
{
    public class PassengerRepository : GenaricRepository<Passenger>
    {
        public PassengerRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public Task GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Passenger?> GetByUserId(string userId)
            => await _context.Passengers.Where(p => p.UserId == userId).AsNoTracking().FirstOrDefaultAsync();


    }
}
