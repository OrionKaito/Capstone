using AutoMapper;
using Capstone.Helper;
using Capstone.Model;
using Capstone.Service;
using Capstone.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Capstone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserNotificationsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserNotificationService _userNotificationService;
        private readonly IRequestService _requestService;
        private readonly UserManager<User> _userManager;
        private readonly INotificationService _notificationService;
        private readonly IRequestActionService _requestActionService;

        public UserNotificationsController(IMapper mapper, IUserNotificationService userNotificationService,
            IRequestService requestService, UserManager<User> userManager, INotificationService notificationService
            , IRequestActionService requestActionService)
        {
            _mapper = mapper;
            _userNotificationService = userNotificationService;
            _requestService = requestService;
            _userManager = userManager;
            _notificationService = notificationService;
            _requestActionService = requestActionService;
        }

        // POST: api/UserNotifications
        [HttpPost]
        public ActionResult PostUserNotification(UserNotificationCM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                UserNotification userNotification = new UserNotification();
                userNotification = _mapper.Map<UserNotification>(model);
                _userNotificationService.Create(userNotification);
                return StatusCode(201, userNotification.ID);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/UserNotifications
        [HttpGet]
        public ActionResult<IEnumerable<UserNotificationVM>> GetUserNotifications()
        {
            try
            {
                List<UserNotificationVM> result = new List<UserNotificationVM>();
                var data = _userNotificationService.GetAll();
                foreach (var item in data)
                {
                    result.Add(_mapper.Map<UserNotificationVM>(item));
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/UserNotifications/5
        [HttpGet("GetByID")]
        public ActionResult<UserNotificationVM> GetUserNotification(Guid ID)
        {
            try
            {
                var rs = _userNotificationService.GetByID(ID);
                if (rs == null) return NotFound(WebConstant.NotFound);

                UserNotificationVM result = _mapper.Map<UserNotificationVM>(rs);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/UserNotifications
        [HttpGet("GetNumberOfNotification")]
        public ActionResult<int> GetNumberOfNotification()
        {
            try
            {
                var currentUser = HttpContext.User;
                var userID = currentUser.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

                var userInDB = _userManager.FindByIdAsync(userID).Result;
                if (userInDB == null) return BadRequest(WebConstant.UserNotExist);

                int result = _userNotificationService.GetNumberOfNotification(userID);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("GetNotificationByUserId")]
        public ActionResult GetNotificationByUserId()
        {
            try
            {
                var currentUser = HttpContext.User;
                var userID = currentUser.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

                var userInDB = _userManager.FindByIdAsync(userID).Result;
                if (userInDB == null) return BadRequest(WebConstant.UserNotExist);

                var notification = _userNotificationService.GetByUserID(userID);

                if (notification == null)
                {
                    return Ok(WebConstant.NoNotificationYet);
                }

                var data = new List<NotificationViewModel>();

                foreach (var item in notification)
                {
                    var notificationInDb = _notificationService.GetByID(item.NotificationID);
                    var requestAction = _requestActionService.GetByID(notificationInDb.EventID);
                    var request = _requestService.GetByID(requestAction.RequestID);

                    if (notificationInDb.NotificationType == NotificationEnum.UpdatedWorkflow)
                    {
                        //đang làm sai
                        var result = new NotificationViewModel
                        {
                            ActorName = _userManager.FindByIdAsync(request.InitiatorID).Result.FullName,
                            EventID = notificationInDb.EventID,
                            Message = WebConstant.WorkflowUpdateMessage,
                            NotificationType = notificationInDb.NotificationType,
                            CreateDate = notificationInDb.CreateDate,
                            NotificationTypeName = "UpdatedWorkflow"
                        };
                        data.Add(result);
                    }
                    else if (notificationInDb.NotificationType == NotificationEnum.CompletedRequest)
                    {
                        var result = new NotificationViewModel
                        {
                            ActorName = _userManager.FindByIdAsync(request.InitiatorID).Result.FullName,
                            EventID = notificationInDb.EventID,
                            Message = WebConstant.CompletedRequestMessage,
                            NotificationType = notificationInDb.NotificationType,
                            NotificationTypeName = notificationInDb.NotificationType.ToString(),
                            CreateDate = notificationInDb.CreateDate,
                            IsHandled = item.IsHandled
                        };
                        data.Add(result);
                    }
                    else if (notificationInDb.NotificationType == NotificationEnum.ReceivedRequest)
                    {
                        var result = new NotificationViewModel
                        {
                            ActorName = _userManager.FindByIdAsync(request.InitiatorID).Result.FullName,
                            EventID = notificationInDb.EventID,
                            Message = WebConstant.ReceivedRequestMessage,
                            NotificationType = notificationInDb.NotificationType,
                            NotificationTypeName = notificationInDb.NotificationType.ToString(),
                            CreateDate = notificationInDb.CreateDate,
                            IsHandled = item.IsHandled
                        };
                        data.Add(result);
                    }
                    //else if (notificationInDb.NotificationType == NotificationEnum.DeniedRequest)
                    //{
                    //    var result = new NotificationViewModel
                    //    {
                    //        ActorName = _userManager.FindByIdAsync(request.InitiatorID).Result.FullName,
                    //        EventID = notificationInDb.EventID,
                    //        Message = WebConstant.DeniedRequestMessage,
                    //        NotificationType = notificationInDb.NotificationType,
                    //        NotificationTypeName = notificationInDb.NotificationType.ToString(),
                    //        IsHandled = item.IsHandled
                    //    };
                    //    data.Add(result);
                    //}
                    else
                    {
                        return NotFound();
                    }
                }

                var notficications = _userNotificationService.GetByUserID(userID);

                foreach (var item in notficications)
                {
                    item.IsRead = true;
                }
                _notificationService.Save();

                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // PUT: api/UserNotifications/5
        [HttpPut]
        public IActionResult PutUserNotification(UserNotificationUM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var userNotificationInDb = _userNotificationService.GetByID(model.ID);
                if (userNotificationInDb == null) return NotFound(WebConstant.NotFound);

                _mapper.Map(model, userNotificationInDb);
                _userNotificationService.Save();
                return Ok(WebConstant.Success);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/UserNotifications/5
        [HttpDelete]
        public ActionResult DeleteUserNotification(Guid ID)
        {
            try
            {
                var userNotificationInDb = _userNotificationService.GetByID(ID);
                if (userNotificationInDb == null) return NotFound(WebConstant.NotFound);

                userNotificationInDb.IsDeleted = true;
                _userNotificationService.Save();
                return Ok(WebConstant.Success);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}