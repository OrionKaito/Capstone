using Capstone.Data.Infrastructrure;
using Capstone.Data.Repositories;
using Capstone.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Capstone.Service
{
    public interface INotificationService
    {
        IEnumerable<Notification> GetAll();
        Notification GetByID(Guid ID);
        IEnumerable<Notification> GetByNotificationType(NotificationEnum notificationType);
        void Create(Notification notification);
        void Delete(Notification notification);
        void Save();
    }

    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public NotificationService(INotificationRepository notificationRepository, IUnitOfWork unitOfWork)
        {
            _notificationRepository = notificationRepository;
            _unitOfWork = unitOfWork;
        }

        public void Create(Notification notification)
        {
            _notificationRepository.Add(notification);
            _unitOfWork.Commit();
        }

        public void Delete(Notification notification)
        {
            _notificationRepository.Add(notification);
            _unitOfWork.Commit();
        }

        public IEnumerable<Notification> GetAll()
        {
            return _notificationRepository.GetAll().Where(n => n.IsDeleted == false);
        }

        public Notification GetByID(Guid ID)
        {
            return _notificationRepository.GetById(ID);
        }

        public IEnumerable<Notification> GetByNotificationType(NotificationEnum notificationType)
        {
            return _notificationRepository.GetByNotificationType(notificationType);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}
