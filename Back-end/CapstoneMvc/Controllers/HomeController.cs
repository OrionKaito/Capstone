﻿using Capstone.Helper;
using Capstone.Model;
using Capstone.Service;
using CapstoneMvc.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CapstoneMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRequestService _requestService;
        private readonly IRequestActionService _requestActionService;
        private readonly IRequestValueService _requestValueService;
        private readonly IRequestFileService _requestFileService;
        private readonly INotificationService _notificationService;
        private readonly IUserNotificationService _userNotificationService;
        private readonly IUserService _userService;
        private readonly IWorkFlowTemplateActionService _workFlowTemplateActionService;
        private readonly IWorkFlowTemplateActionConnectionService _workFlowTemplateActionConnectionService;
        private readonly IConnectionTypeService _connectionTypeService;
        private readonly IWorkFlowTemplateService _workFlowTemplateService;
        private readonly IPermissionService _permissionService;
        private readonly UserManager<User> _userManager;
        private readonly IUserDeviceService _userDeviceService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public HomeController(IRequestService requestService
            , IRequestActionService requestActionService
            , IRequestValueService requestValueService
            , IRequestFileService requestFileService
            , INotificationService notificationService
            , IUserNotificationService userNotificationService
            , IUserService userService
            , IWorkFlowTemplateActionService workFlowTemplateActionService
            , IWorkFlowTemplateActionConnectionService workFlowTemplateActionConnectionService
            , IConnectionTypeService connectionTypeService
            , IWorkFlowTemplateService workFlowTemplateService
            , IPermissionService permissionService
            , UserManager<User> userManager
            , IUserDeviceService userDeviceService
            , IEmailService emailService
            , IConfiguration configuration)
        {
            _requestService = requestService;
            _requestActionService = requestActionService;
            _requestValueService = requestValueService;
            _requestFileService = requestFileService;
            _notificationService = notificationService;
            _userNotificationService = userNotificationService;
            _userService = userService;
            _workFlowTemplateActionService = workFlowTemplateActionService;
            _workFlowTemplateActionConnectionService = workFlowTemplateActionConnectionService;
            _connectionTypeService = connectionTypeService;
            _workFlowTemplateService = workFlowTemplateService;
            _permissionService = permissionService;
            _userManager = userManager;
            _userDeviceService = userDeviceService;
            _emailService = emailService;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ApproveRequest(RequestApproveCM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                //Begin transaction
                _requestService.BeginTransaction();

                var currentRequestAction = _requestActionService.GetByID(model.RequestActionID);
                if (currentRequestAction.Status == StatusEnum.Handled)
                {
                    Message handled = new Message
                    {
                        strMessage = "Submit success! But request is handled...",
                    };
                    return View(handled);
                }

                //Cập nhật đã xử lý
                currentRequestAction.Status = StatusEnum.Handled;

                //Hết kiểm tra
                var request = _requestService.GetByID(model.RequestID);

                //RequestAction
                RequestAction requestAction = new RequestAction
                {
                    Status = model.Status,
                    RequestID = model.RequestID,
                    ActorEmail = model.ActorEmail,
                    NextStepID = model.NextStepID,
                    CreateDate = DateTime.Now,
                };

                _requestActionService.Create(requestAction);

                var nextStep = _workFlowTemplateActionService.GetByID(model.NextStepID);

                if (!nextStep.IsEnd) //Kiểm tra đây không phải action cuối cùng
                {
                    //Cập nhật request
                    request.CurrentRequestActionID = requestAction.ID;
                    _requestService.Save();

                    //Notification
                    Notification notification = new Notification
                    {
                        EventID = requestAction.ID,
                        NotificationType = NotificationEnum.ReceivedRequest,
                        CreateDate = DateTime.Now,
                    };

                    _notificationService.Create(notification);

                    //UserNotification

                    if (nextStep.IsApprovedByLineManager)
                    {
                        var ownerID = _requestService.GetByID(model.RequestID).InitiatorID;
                        var manager = _userManager.FindByIdAsync(ownerID).Result;
                        var managerID = manager.LineManagerID;

                        if (managerID != "" || !string.IsNullOrEmpty(managerID))
                        {
                            //Push notification
                            PushNotificationToUser(managerID, "Received Request", WebConstant.ReceivedRequestMessage, notification);
                        }
                    }
                    //Lấy các user có permission xử lý action để gửi notification
                    if (nextStep.PermissionToUseID.HasValue)
                    {
                        var users = _userService.getUsersByPermissionID(nextStep.PermissionToUseID.GetValueOrDefault());

                        if (users != null && users.Any())
                        {
                            foreach (var user in users)
                            {
                                //Push notification
                                PushNotificationToUser(user.Id, "Received Request", WebConstant.ReceivedRequestMessage, notification);
                            }
                        }
                    }
                    if (!nextStep.ToEmail.IsNullOrEmpty())
                    {
                        var requestValue = _requestValueService.GetByRequestActionID(requestAction.ID);

                        Dictionary<string, string> dynamicform = new Dictionary<string, string>();

                        foreach (var item in requestValue)
                        {
                            dynamicform.Add(item.Key, item.Value);
                        }

                        Dictionary<string, string> listButton = new Dictionary<string, string>();
                        var connections = _workFlowTemplateActionConnectionService.GetByFromWorkflowTemplateActionID(nextStep.ID);
                        foreach (var connection in connections)
                        {
                            listButton.Add(_configuration["UrlCapstoneMvc"]
                                + "/home/ApproveRequest/?RequestID="
                                + request.ID
                                + "&RequestActionID="
                                + requestAction.ID
                                + "&Status="
                                + StatusEnum.Pending
                                + "&NextStepID="
                                + connection.ToWorkFlowTemplateActionID
                                + "&ActorEmail="
                                + nextStep.ToEmail, connection.ConnectionType.Name);
                        }

                        string message = _emailService.GenerateMessageTest(nextStep.ToEmail
                            , "Dynamic Workflow"
                            , _workFlowTemplateService.GetByID(request.WorkFlowTemplateID).Name
                            , nextStep.Name
                            , dynamicform
                            , listButton);

                        _emailService.SendMail(nextStep.ToEmail, "You receive", message);
                    }
                }
                else // Nếu nó là action cuối cùng (kết quả) thì gửi về cho người gửi request
                {
                    //Cập nhật request
                    request.IsCompleted = true;
                    request.CurrentRequestActionID = requestAction.ID;
                    _requestService.Save();

                    //Notification
                    Notification notification = new Notification
                    {
                        EventID = requestAction.ID,
                        NotificationType = NotificationEnum.CompletedRequest,
                        CreateDate = DateTime.Now,
                    };

                    _notificationService.Create(notification);

                    var ownerID = _requestService.GetByID(model.RequestID).InitiatorID;
                    var owner = _userManager.FindByIdAsync(ownerID).Result;

                    //Push notification
                    PushNotificationToUser(ownerID, "Completed Request", WebConstant.CompletedRequestMessage, notification);
                }

                _notificationService.Save();
                _requestService.CommitTransaction();

                Message messageResult = new Message
                {
                    strMessage = "Submit success!",
                };
                //End transaction
                return View(messageResult);
            }

            catch (Exception e)
            {
                _requestService.RollBack();
                Message messageResult = new Message
                {
                    strMessage = "Submit fail!" + e.Message,
                };
                return View(messageResult);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async void PushNotificationToUser(string userID, string title, string body, Notification notification)
        {
            //get list user device by userID
            var userDeviceList = _userDeviceService.GetDeviceTokenByUserID(userID);

            if (userDeviceList.Count() == 0)
            {
                UserNotification userNotification = new UserNotification
                {
                    NotificationID = notification.ID,
                    UserID = userID,
                    IsSend = false,
                };
                _userNotificationService.Create(userNotification);
            }
            else
            {
                List<string> deviceTokenList = new List<string>();
                foreach (var userDevice in userDeviceList)
                {
                    deviceTokenList.Add(userDevice.DeviceToken);
                }

                UserNotification userNotification = new UserNotification
                {
                    NotificationID = notification.ID,
                    UserID = userID,
                    IsSend = true,
                };
                _userNotificationService.Create(userNotification);

                string[] deviceTokens = deviceTokenList.ToArray();

                bool sent = await PushNotification.SendMessageAsync(deviceTokens, title, body);
            }
        }
    }
}