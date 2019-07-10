using Capstone.Data.Infrastructrure;
using Capstone.Data.Repositories;
using Capstone.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Capstone.Service
{
    public interface IUserNotificationService
    {
        IEnumerable<UserNotification> GetAll();
        IEnumerable<UserNotification> GetByUserIDAndNotificationType(string ID, NotificationEnum notificationType);
        int GetNumberOfNotification(string ID);
        UserNotification GetByID(Guid ID);
        void Create(UserNotification userNotification);
        void Delete(UserNotification userNotification);
        void Save();
    }

    public class UserNotificationService : IUserNotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserNotificationRepository _userNotificationRepository;
        private readonly INotificationRepository _notificationRepository;

        public UserNotificationService(IUnitOfWork unitOfWork, IUserNotificationRepository userNotificationRepository
            , INotificationRepository notificationRepository)
        {
            _unitOfWork = unitOfWork;
            _userNotificationRepository = userNotificationRepository;
            _notificationRepository = notificationRepository;
        }

        public int GetNumberOfNotification(string ID)
        {
            return _userNotificationRepository.GetMany(u => u.IsDeleted == false && u.IsRead == false && u.UserID.Equals(ID)).Count();
        }

        public void Create(UserNotification userNotification)
        {
            _userNotificationRepository.Add(userNotification);
            _unitOfWork.Commit();
        }

        public void Delete(UserNotification userNotification)
        {
            _userNotificationRepository.Delete(userNotification);
            _unitOfWork.Commit();
        }

        public IEnumerable<UserNotification> GetAll()
        {
            return _userNotificationRepository.GetAll().Where(u => u.IsDeleted == false);
        }

        public UserNotification GetByID(Guid ID)
        {
            return _userNotificationRepository.GetById(ID);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public IEnumerable<UserNotification> GetByUserIDAndNotificationType(string ID, NotificationEnum notificationType)
        {
            List<UserNotification> userNotificationList = new List<UserNotification>();

            var notificationList = _notificationRepository.GetByNotificationType(notificationType);

            foreach (var item in notificationList)
            {
                userNotificationList.Add(_userNotificationRepository.GetMany(u => u.IsDeleted == false 
                && u.UserID.Equals(ID) && u.NotificationID == item.ID).FirstOrDefault());
            }
            return userNotificationList;
        }
    }
}
