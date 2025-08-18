using PowerStore.Core.Contract;
using PowerStore.Core.Contract.Dtos.Identity;
using PowerStore.Core.Contract.Dtos.Passenger;
using PowerStore.Core.Contract.IdentityInterface;
using PowerStore.Core.Entities;
using PowerStore.Service.Mappers;
using System;
using System.Linq.Expressions;

namespace PowerStore.Service.Identity
{
    public class DriverService : IDriverService
    {
        private readonly DriverMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public DriverService(IUnitOfWork unitOfWork, DriverMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public List<DriverDto> GetBy(Expression<Func<Driver, bool>> predicate)
        {
            return _mapper.MapFromSourceToDestination(_unitOfWork.Repositoy<Driver>().GetBy(predicate));
        }
        public int Add(DriverDto driver)
        {
            try
            {
                _unitOfWork.Repositoy<Driver>().Add(_mapper.MapFromDestinationToSource(driver));
                return _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine(ex.Message);

                // Return a default value (e.g., -1 to indicate an error)
                return -1;
            }
        }

        public async Task<int> Update(DriverDto driver)
        {
            try
            {
                var driver_ = await _unitOfWork.Repositoy<Driver>().Find(x => x.Id == driver.Id);
                if (driver_ != null)
                {
                    driver_ = _mapper.MapFromDestinationToSource(driver);
                    _unitOfWork.Repositoy<Driver>().Update(driver_);
                }
                return _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine(ex.Message);

                // Return a default value (e.g., -1 to indicate an error)
                return -1;
            }
        }
        public async Task<List<DriverDto>> GetAllWithUser()
        {
            List<Driver> drivers = await _unitOfWork.Repositoy<Driver>().GetAll(p => p.User);
            return _mapper.MapFromSourceToDestination(drivers);
        }
    }
}
