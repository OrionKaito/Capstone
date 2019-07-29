using Capstone.Data.Infrastructrure;
using Capstone.Data.Repositories;
using Capstone.Model;

namespace Capstone.Service
{
    public interface IUserDeviceService
    {
        void Create(UserDevice userDevice);
        void Delete(UserDevice userDevice);
    }

    public class UserDeviceService : IUserDeviceService
    {
        private readonly IUserDeviceRepository _userDeviceRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserDeviceService(IUserDeviceRepository userDeviceRepository, IUnitOfWork unitOfWork)
        {
            _userDeviceRepository = userDeviceRepository;
            _unitOfWork = unitOfWork;
        }

        public void Create(UserDevice userDevice)
        {
            _userDeviceRepository.Add(userDevice);
        }

        public void Delete(UserDevice userDevice)
        {
            _userDeviceRepository.Delete(userDevice);
        }
    }
}
