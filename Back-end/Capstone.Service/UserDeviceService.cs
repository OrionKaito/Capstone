using Capstone.Data.Infrastructrure;
using Capstone.Data.Repositories;
using Capstone.Model;
using System.Collections.Generic;
using System.Linq;

namespace Capstone.Service
{
    public interface IUserDeviceService
    {
        void Create(UserDevice userDevice);
        UserDevice GetByUserIDAndDeviceToken(string userID, string deviceToken);
        IEnumerable<UserDevice> GetDeviceTokenByUserID(string userID);
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
            _unitOfWork.Commit();
        }

        public void Delete(UserDevice userDevice)
        {
            _userDeviceRepository.Delete(userDevice);
            _unitOfWork.Commit();
        }

        public UserDevice GetByUserIDAndDeviceToken(string userID, string deviceToken)
        {
            return _userDeviceRepository.GetMany(u => u.UserID.Equals(userID) && u.DeviceToken.Equals(deviceToken)).FirstOrDefault();
        }

        public IEnumerable<UserDevice> GetDeviceTokenByUserID(string userID)
        {
            return _userDeviceRepository.GetMany(u => u.UserID.Equals(userID));
        }
    }
}
