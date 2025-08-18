using PowerStore.Core.Contract.Dtos.Passenger;
using PowerStore.Core.Entities;
using System.Linq.Expressions;

namespace PowerStore.Core.Contract.IdentityInterface
{
    public interface IPassengerService
    {
        List<PassengerDto> GetBy(Expression<Func<Passenger, bool>> predicate);
        int Add(PassengerDto passenger);

        Task GetByIdAsync(string id);
        Passenger? GetByUserId(string userId);
        Task<List<PassengerDto>> GetAllWithUser();

    }
}
