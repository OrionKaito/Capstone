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
        private readonly IWorkFlowTemplateService _workFlowTemplateService;

        public UserNotificationsController(IMapper mapper, IUserNotificationService userNotificationService,
            IRequestService requestService, UserManager<User> userManager, INotificationService notificationService
            , IRequestActionService requestActionService, IWorkFlowTemplateService workFlowTemplateService)
        {
            _mapper = mapper;
            _userNotificationService = userNotificationService;
            _requestService = requestService;
            _userManager = userManager;
            _notificationService = notificationService;
            _requestActionService = requestActionService;
            _workFlowTemplateService = workFlowTemplateService;
        }

        // GET: api/UserNotifications
        //[HttpGet("GetNumberOfNotification")]
        //public ActionResult<int> GetNumberOfNotification(NotificationEnum notificationType)
        //{
        //    try
        //    {
        //        var currentUser = HttpContext.User;
        //        var userID = currentUser.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

        //        var userInDB = _userManager.FindByIdAsync(userID).Result;
        //        if (userInDB == null) return BadRequest(WebConstant.UserNotExist);

        //        int result = _userNotificationService.GetNumberOfNotificationByType(notificationType, userID);
        //        return Ok(result);
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.Message);
        //    }
        //}

        //[HttpGet("GetNotificationByUserId")]
        //public ActionResult GetNotificationByUserId(NotificationEnum notificationType)
        //{
        //    try
        //    {
        //        var currentUser = HttpContext.User;
        //        var userID = currentUser.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

        //        var userInDB = _userManager.FindByIdAsync(userID).Result;
        //        if (userInDB == null) return BadRequest(WebConstant.UserNotExist);

        //        List<NotificationViewModel> result = new List<NotificationViewModel>();
        //        List<UserNotification> userNotifications = new List<UserNotification>();

        //        if (notificationType == NotificationEnum.CompletedRequest || notificationType == NotificationEnum.UpdatedWorkflow)
        //        {
        //            userNotifications.AddRange(_userNotificationService.GetByUserIDAndNotificationType(userID, notificationType, false));
        //        }

        //        else if (notificationType == NotificationEnum.ReceivedRequest)
        //        {
        //            userNotifications.AddRange(_userNotificationService.GetByUserIDAndNotificationType(userID, notificationType, true));
        //        }

        //        else
        //        {
        //            return BadRequest("Notification Type Not Found!");
        //        }

        //        if (!userNotifications.Any())
        //        {
        //            return Ok(WebConstant.EmptyList);
        //        }

        //        foreach (var userNotification in userNotifications)
        //        {
        //            var notificationInDb = _notificationService.GetByID(userNotification.NotificationID);

        //            var requestAction = _requestActionService.GetByID(notificationInDb.EventID);

        //            var request = _requestService.GetByID(requestAction.RequestID);

        //            var notificationVM = new NotificationViewModel
        //            {
        //                WorkflowName = _workFlowTemplateService.GetByID(request.WorkFlowTemplateID).Name,
        //                UserNotificationID = userNotification.ID,
        //                ActorName = _userManager.FindByIdAsync(request.InitiatorID).Result.FullName,
        //                EventID = notificationInDb.EventID,
        //                Message = notificationType == NotificationEnum.CompletedRequest ? WebConstant.CompletedRequestMessage : WebConstant.ReceivedRequestMessage,
        //                NotificationType = notificationInDb.NotificationType,
        //                NotificationTypeName = notificationInDb.NotificationType.ToString(),
        //                CreateDate = notificationInDb.CreateDate,
        //                IsRead = userNotification.IsRead,
        //                IsHandled = notificationInDb.IsHandled
        //            };
        //            result.Add(notificationVM);
        //            //userNotification.IsRead = true;
        //            //_userNotificationService.Save();
        //        }

        //        //return Ok(WebConstant.NoNotificationYet);


        //        //var data = new List<NotificationViewModel>();

        //        //foreach (var item in notification)
        //        //{
        //        //    var notificationInDb = _notificationService.GetByID(item.NotificationID);
        //        //    var requestAction = _requestActionService.GetByID(notificationInDb.EventID);
        //        //    var request = _requestService.GetByID(requestAction.RequestID);

        //        //    if (notificationType == NotificationEnum.CompletedRequest && notificationInDb.NotificationType == NotificationEnum.CompletedRequest)
        //        //    {
        //        //        var result = new NotificationViewModel
        //        //        {
        //        //            ActorName = _userManager.FindByIdAsync(request.InitiatorID).Result.FullName,
        //        //            EventID = notificationInDb.EventID,
        //        //            Message = WebConstant.CompletedRequestMessage,
        //        //            NotificationType = notificationInDb.NotificationType,
        //        //            NotificationTypeName = notificationInDb.NotificationType.ToString(),
        //        //            CreateDate = notificationInDb.CreateDate,
        //        //            IsHandled = item.IsHandled
        //        //        };
        //        //        data.Add(result);
        //        //    }
        //        //    else
        //        //    {
        //        //        if (notificationInDb.NotificationType == NotificationEnum.UpdatedWorkflow)
        //        //        {
        //        //            //đang làm sai
        //        //            var result = new NotificationViewModel
        //        //            {
        //        //                ActorName = _userManager.FindByIdAsync(request.InitiatorID).Result.FullName,
        //        //                EventID = notificationInDb.EventID,
        //        //                Message = WebConstant.WorkflowUpdateMessage,
        //        //                NotificationType = notificationInDb.NotificationType,
        //        //                CreateDate = notificationInDb.CreateDate,
        //        //                NotificationTypeName = "UpdatedWorkflow"
        //        //            };
        //        //            data.Add(result);
        //        //        }
        //        //        else if (notificationInDb.NotificationType == NotificationEnum.ReceivedRequest)
        //        //        {
        //        //            var result = new NotificationViewModel
        //        //            {
        //        //                ActorName = _userManager.FindByIdAsync(request.InitiatorID).Result.FullName,
        //        //                EventID = notificationInDb.EventID,
        //        //                Message = WebConstant.ReceivedRequestMessage,
        //        //                NotificationType = notificationInDb.NotificationType,
        //        //                NotificationTypeName = notificationInDb.NotificationType.ToString(),
        //        //                CreateDate = notificationInDb.CreateDate,
        //        //                IsHandled = item.IsHandled
        //        //            };
        //        //            data.Add(result);
        //        //        }
        //        //        else
        //        //        {
        //        //            return NotFound();
        //        //        }
        //        //    }
        //        //}

        //        return Ok(result);
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.Message);
        //    }
        //}

        // GET: api/UserNotifications
        [HttpGet("GetNumberNotification")]
        public ActionResult<int> GetNumberNotification()
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

        [HttpGet("GetNotificationByUser")]
        public ActionResult<IEnumerable<UserNotificationPaginVM>> GetNotificationByUser(int? numberOfPage, int? NumberOfRecord)
        {
            var page = numberOfPage ?? 1;
            var count = NumberOfRecord ?? WebConstant.DefaultPageRecordCount;

            var currentUser = HttpContext.User;
            var userID = currentUser.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

            var userInDB = _userManager.FindByIdAsync(userID).Result;
            if (userInDB == null) return BadRequest(WebConstant.UserNotExist);

            var userNotification = _userNotificationService.GetByUserID(userID);

            if (userNotification == null)
            {
                return Ok(WebConstant.NoNotificationYet);
            }

            var data = new List<UserNotificationVM>();
            //var listUserInRequest = _requestService.GetByUserID(userID);

            foreach (var item in userNotification)
            {
                var notificationInDb = _notificationService.GetByID(item.NotificationID);
                var requestAction = _requestActionService.GetByID(notificationInDb.EventID);
                var request = _requestService.GetByID(requestAction.RequestID);

                if (notificationInDb.NotificationType == NotificationEnum.ReceivedRequest)
                {
                    var result = new UserNotificationVM
                    {
                        WorkflowName = request.WorkFlowTemplate.Name,
                        UserNotificationID = item.ID,
                        ActorName = request.Initiator.FullName,
                        EventID = notificationInDb.EventID,
                        Message = WebConstant.ReceivedRequestMessage,
                        NotificationType = notificationInDb.NotificationType,
                        NotificationTypeName = notificationInDb.NotificationType.ToString(),
                        CreateDate = notificationInDb.CreateDate,
                        IsRead = item.IsRead,
                        IsHandled = notificationInDb.IsHandled
                    };
                    data.Add(result);
                }
                else if (notificationInDb.NotificationType == NotificationEnum.CompletedRequest)
                {
                    var result = new UserNotificationVM
                    {
                        WorkflowName = request.WorkFlowTemplate.Name,
                        UserNotificationID = item.ID,
                        ActorName = request.Initiator.FullName,
                        EventID = notificationInDb.EventID,
                        Message = WebConstant.CompletedRequestMessage,
                        NotificationType = notificationInDb.NotificationType,
                        NotificationTypeName = notificationInDb.NotificationType.ToString(),
                        CreateDate = notificationInDb.CreateDate,
                        IsRead = item.IsRead,
                        IsHandled = notificationInDb.IsHandled
                    };
                    data.Add(result);
                }
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

            UserNotificationPaginVM userNotificationPaginVM = new UserNotificationPaginVM
            {
                TotalRecord = data.Count(),
                UserNotifications = data.Skip((page - 1) * count).Take(count),
            };

            return Ok(userNotificationPaginVM);
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