using AutoMapper;
using Capstone.Model;
using Capstone.Service;
using Capstone.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Capstone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;
        private readonly IUserNotificationService _userNotificationService;
        private readonly IWorkFlowTemplateService _workFlowService;
        private readonly IRequestService _requestService;
        private readonly UserManager<User> _userManager;

        public NotificationsController(IMapper mapper, INotificationService notificationService, IUserNotificationService userNotificationService,
            IWorkFlowTemplateService workFlowService, IRequestService requestService, UserManager<User> userManager)
        {
            _mapper = mapper;
            _notificationService = notificationService;
            _userNotificationService = userNotificationService;
            _workFlowService = workFlowService;
            _requestService = requestService;
            _userManager = userManager;
        }


        // POST: api/Notifications
        [HttpPost]
        public ActionResult<Notification> PostNotification(NotificationCM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                Notification notification = new Notification();
                notification = _mapper.Map<Notification>(model);
                _notificationService.Create(notification);
                return StatusCode(201, notification.ID);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/Notificaions
        [HttpGet]
        public ActionResult<IEnumerable<Notification>> GetNotifications()
        {
            try
            {
                List<NotificationVM> result = new List<NotificationVM>();
                var data = _notificationService.GetAll();
                foreach (var item in data)
                {
                    result.Add(_mapper.Map<NotificationVM>(item));
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/Notifications/5
        [HttpGet("GetByID")]
        public ActionResult<Notification> GetNotification(Guid ID)
        {
            try
            {
                var rs = _notificationService.GetByID(ID);
                if (rs == null) return NotFound("ID not found!");

                NotificationVM result = _mapper.Map<NotificationVM>(rs);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/UserNotifications
        [HttpGet("GetNumberOfNotification")]
        public ActionResult<int> GetNumberOfNotification(string ID)
        {
            try
            {
                var userInDB = _userManager.FindByIdAsync(ID).Result;
                if (userInDB == null) return BadRequest("ID not found!");

                int result = _userNotificationService.GetNumberOfNotification(ID);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("GetByUserID")]
        public ActionResult GetByUserId(string ID)
        {
            var userInDB = _userManager.FindByIdAsync(ID).Result;
            if (userInDB == null) return BadRequest("ID not found!");

            var notification = _userNotificationService.GetByUserID(ID);

            if (notification == null)
            {
                return Ok("There are not any notfication yet");
            }

            var data = new List<NotificationViewModel>();
            var listUserInRequest = _requestService.GetByUserID(ID);

            foreach (var item in notification)
            {
                var notificationInDb = _notificationService.GetByID(item.NotificationID);
                if (notificationInDb.NotificationType == NotificationType.UpdatedWorkflow)
                {
                    var result = new NotificationViewModel
                    {
                        Fullname = _userManager.FindByIdAsync(ID).Result.FullName,
                        EventID = notificationInDb.EventID,
                        Message = "have update workflow",
                        NotificationType = notificationInDb.NotificationType
                    };
                    data.Add(result);
                }
                else if (notificationInDb.NotificationType == NotificationType.AcceptedRequest)
                {
                    foreach (var userInRequest in listUserInRequest)
                    {
                        var result = new NotificationViewModel
                        {
                            Fullname = _userManager.FindByIdAsync(ID).Result.FullName,
                            EventID = notificationInDb.EventID,
                            Message = "Your request are accepted",
                            NotificationType = notificationInDb.NotificationType,
                            ApproverName = _userManager.FindByIdAsync(userInRequest.InitiatorID).Result.FullName
                        };
                        data.Add(result);
                    }
                }
                else if (notificationInDb.NotificationType == NotificationType.ReceivedRequest)
                {
                    foreach (var userInRequest in listUserInRequest)
                    {
                        var result = new NotificationViewModel
                        {
                            Fullname = _userManager.FindByIdAsync(ID).Result.FullName,
                            EventID = notificationInDb.EventID,
                            Message = "You received request",
                            NotificationType = notificationInDb.NotificationType,
                            ApproverName = _userManager.FindByIdAsync(userInRequest.InitiatorID).Result.FullName
                        };
                        data.Add(result);
                    }
                }
                else if (notificationInDb.NotificationType == NotificationType.DeniedRequest)
                {
                    foreach (var userInRequest in listUserInRequest)
                    {
                        var result = new NotificationViewModel
                        {
                            Fullname = _userManager.FindByIdAsync(ID).Result.FullName,
                            EventID = notificationInDb.EventID,
                            Message = "Your request are denied",
                            NotificationType = notificationInDb.NotificationType,
                            ApproverName = _userManager.FindByIdAsync(userInRequest.InitiatorID).Result.FullName
                        };
                        data.Add(result);
                    }
                }
                else
                {
                    return NotFound();
                }
            }

            var notficications = _userNotificationService.GetByUserID(ID);

            foreach (var item in notficications)
            {
                item.IsRead = true;
            }
            _notificationService.Save();
            
            return Ok(data);
        }

        // PUT: api/Notifications/5
        [HttpPut]
        public IActionResult PutNotification(NotificationUM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var notificationInDb = _notificationService.GetByID(model.ID);
                if (notificationInDb == null) return NotFound("ID not found!");
                
                _mapper.Map(model, notificationInDb);
                _notificationService.Save();
                return Ok("success");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/Notifications/5
        [HttpDelete]
        public ActionResult DeleteNotification(Guid ID)
        {
            try
            {
                var notificationInDb = _notificationService.GetByID(ID);
                if (notificationInDb == null) return NotFound("ID not found!");

                notificationInDb.IsDeleted = true;
                _notificationService.Save();
                return Ok("success");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}