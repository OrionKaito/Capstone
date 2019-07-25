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
        IEnumerable<UserNotification> GetByUserIDAndNotificationType(string ID, NotificationEnum notificationType, bool isCheckHandled);
        IEnumerable<UserNotification> GetByUserIDAndIsSend(string ID, bool isSend);
        IEnumerable<UserNotification> GetByUserID(string ID);
        int GetNumberOfNotificationByType(NotificationEnum notificationType, string ID);
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

        public int GetNumberOfNotificationByType(NotificationEnum notificationType, string ID)
        {
            var notification = _notificationRepository.GetByNotificationTypeAndIsHandled(notificationType);
            int numberOfNotification = 0;
            foreach (var item in notification)
            {
                if (notificationType == NotificationEnum.CompletedRequest)
                {
                    numberOfNotification += _userNotificationRepository.GetMany(u => u.IsDeleted == false && u.IsRead == false && u.UserID.Equals(ID) && u.NotificationID == item.ID).Count();
                } else if (notificationType == NotificationEnum.ReceivedRequest)
                {
                    numberOfNotification += _userNotificationRepository.GetMany(u => u.IsDeleted == false && u.UserID.Equals(ID) && u.NotificationID == item.ID).Count();
                }
            }
            return numberOfNotification;
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

        public IEnumerable<UserNotification> GetByUserIDAndNotificationType(string ID, NotificationEnum notificationType, bool isCheckHandled)
        {
            List<UserNotification> userNotificationList = new List<UserNotification>();

            List<Notification> notificationList = new List<Notification>();

            if (isCheckHandled)
            {
                notificationList.AddRange(_notificationRepository.GetByNotificationTypeAndIsHandled(notificationType).OrderByDescending(u => u.CreateDate));
            }
            else
            {
                notificationList.AddRange(_notificationRepository.GetByNotificationType(notificationType).OrderByDescending(u => u.CreateDate));
            }

            foreach (var item in notificationList)
            {
                var userNotification = _userNotificationRepository.GetMany(u => u.IsDeleted == false
                && u.UserID.Equals(ID) && u.NotificationID == item.ID).FirstOrDefault();
                if (userNotification != null)
                {
                    userNotificationList.Add(userNotification);
                }
            }
            return userNotificationList;
        }

        public IEnumerable<UserNotification> GetByUserIDAndIsSend(string ID, bool isSend)
        {
            return _userNotificationRepository.GetMany(u => u.UserID.Equals(ID) && u.IsSend == isSend);
        }

        public IEnumerable<UserNotification> GetByUserID(string ID)
        {
            return _userNotificationRepository.GetMany(u => u.UserID.Equals(ID) && u.IsDeleted == false && u.IsRead == false);
        }

        public int GetNumberOfNotification(string ID)
        {
            return _userNotificationRepository.GetMany(u => u.IsDeleted == false && u.IsRead == false && u.UserID.Equals(ID)).Count();
        }
    }
}
