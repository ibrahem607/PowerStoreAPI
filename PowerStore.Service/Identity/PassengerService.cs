using Microsoft.EntityFrameworkCore;
using PowerStore.Core.Contract;
using PowerStore.Core.Contract.Dtos.Passenger;
using PowerStore.Core.Contract.IdentityInterface;
using PowerStore.Core.Entities;
using PowerStore.Service.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PowerStore.Service.Identity
{
    public class PassengerService : IPassengerService
    {
        private readonly PassengerMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public PassengerService(IUnitOfWork unitOfWork, PassengerMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public List<PassengerDto> GetBy(Expression<Func<Passenger, bool>> predicate)
        {
            var data =  _unitOfWork.Repositoy<Passenger>().GetBy(predicate);
            return _mapper.MapFromSourceToDestination(data);
        }

        public int Add(PassengerDto passenger) { 
            _unitOfWork.Repositoy<Passenger>().Add(_mapper.MapFromDestinationToSource(passenger));
            return _unitOfWork.Complete();
        }
        public Task GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Passenger? GetByUserId(string userId)
        {
            var data= _unitOfWork.Repositoy<Passenger>().GetBy(p => p.UserId == userId);
            return data.FirstOrDefault();
        }
        public async Task<List<PassengerDto>> GetAllWithUser()
        {
            var passengers = await _unitOfWork.Repositoy<Passenger>().GetAll(p => p.User);
            return _mapper.MapFromSourceToDestination(passengers);

        }
    }
}
