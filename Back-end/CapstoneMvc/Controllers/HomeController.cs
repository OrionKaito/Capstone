using Capstone.Model;
using Capstone.Service;
using Capstone.Service.Helper;
using Capstone.ViewModel;
using CapstoneMvc.Models;
using Microsoft.AspNetCore.DataProtection;
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
        private readonly IDataProtector _dataProtector;

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
            , IConfiguration configuration
            , IDataProtectionProvider provider)
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
            _dataProtector = provider.CreateProtector(WebConstant.Purpose);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ApproveRequest(string content)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                //Begin transaction
                _requestService.BeginTransaction();

                //Decrypt chuỗi content
                content = _dataProtector.Unprotect(content);

                RequestApproveByMailCM model = new RequestApproveByMailCM
                {
                    RequestID = Guid.Parse(content.Substring(content.IndexOf(WebConstant.RequestID) + WebConstant.RequestID.Count() + 1, 36)),
                    RequestActionID = Guid.Parse(content.Substring(content.IndexOf(WebConstant.RequestActionID) + WebConstant.RequestActionID.Count() + 1, 36)),
                    NextStepID = Guid.Parse(content.Substring(content.IndexOf(WebConstant.NextStepID) + WebConstant.NextStepID.Count() + 1, 36)),
                };

                var currentRequestAction = _requestActionService.GetByID(model.RequestActionID);
                if (currentRequestAction.Status == StatusEnum.Handled)
                {
                    Message handled = new Message
                    {
                        strMessage = WebConstant.RequestIsHandled,
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
                    Status = StatusEnum.Pending,
                    RequestID = model.RequestID,
                    ActorEmail = _workFlowTemplateActionService.GetByID(currentRequestAction.NextStepID.GetValueOrDefault()).ToEmail,
                    NextStepID = model.NextStepID,
                    CreateDate = DateTime.Now,
                    WorkFlowTemplateActionID = currentRequestAction.NextStepID,
                };

                //Lấy phần thông tin của người gửi request
                var startActionTemplate = _workFlowTemplateActionService.GetStartByWorkFlowID(request.WorkFlowTemplateID);
                var userAction = _requestActionService.GetStartAction(startActionTemplate.ID, request.ID);

                //Lấy file của user
                var requestFiles = _requestFileService.GetByRequestActionID(userAction.ID).Select(r => new RequestFileVM
                {
                    ID = r.ID,
                    Path = r.Path,
                    IsDeleted = r.IsDeleted,
                });

                //list file path to send email
                List<string> filePaths = new List<string>();
                if (!requestFiles.IsNullOrEmpty())
                {
                    foreach (var requestFile in requestFiles)
                    {
                        filePaths.Add(requestFile.Path);
                    }
                }

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

                    //Lấy connection dựa vào action trước và kế tiếp
                    var workFlowTemplateActionConnection = _workFlowTemplateActionConnectionService
                        .GetByFromIDAndToID(currentRequestAction.NextStep.ID, model.NextStepID);

                    //kiểm tra connection coi có bị hangfire không?
                    //nếu có thì để status của requestAction là Hangfire
                    if (workFlowTemplateActionConnection.TimeInterval > 0)
                    {
                        requestAction.Status = StatusEnum.Hangfire;
                        _requestActionService.Save();
                    }
                    else
                    {
                        if (nextStep.IsApprovedByInitiator)
                        {
                            var initiator = _userManager.FindByIdAsync(request.InitiatorID).Result;
                            PushNotificationToUser(initiator.Id, "Received Request", WebConstant.ReceivedRequestMessage, notification);
                        }

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
                            var users = _userService.GetUsersByPermissionID(nextStep.PermissionToUseID.GetValueOrDefault());
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
                            //lấy giá trị form mà user input
                            var userRequestValue = _requestValueService.GetByRequestActionID(userAction.ID);
                            Dictionary<string, string> dynamicform = new Dictionary<string, string>();

                            foreach (var item in userRequestValue)
                            {
                                dynamicform.Add(item.Key, item.Value);
                            }

                            //lấy comment của staff
                            var staffRequestValue = _requestValueService.GetByRequestActionID(requestAction.ID);
                            Dictionary<string, string> comments = new Dictionary<string, string>();
                            if (!staffRequestValue.IsNullOrEmpty())
                            {
                                comments.Add("Name", _userManager.FindByIdAsync(requestAction.ActorID).Result.FullName);
                                int i = 0;
                                foreach (var item in staffRequestValue)
                                {
                                    i++;
                                    comments.Add(item.Key + i, item.Value);
                                }
                            }

                            Dictionary<string, string> listButton = new Dictionary<string, string>();
                            var connections = _workFlowTemplateActionConnectionService.GetByFromWorkflowTemplateActionID(nextStep.ID);
                            string url = "";
                            foreach (var connection in connections)
                            {
                                url = (_configuration["UrlCapstoneMvc"]
                                        + "/home/ApproveRequest/?content="
                                        + _dataProtector.Protect("RequestID="
                                            + request.ID
                                            + "&RequestActionID="
                                            + requestAction.ID
                                            + "&NextStepID="
                                            + connection.ToWorkFlowTemplateActionID)
                                        );
                                listButton.Add(url, connection.ConnectionType.Name);
                            }

                            string message = _emailService.GenerateMessageTest(nextStep.ToEmail
                                , "Dynamic Workflow"
                                , _workFlowTemplateService.GetByID(request.WorkFlowTemplateID).Name
                                , nextStep.Name
                                , dynamicform
                                , comments
                                , listButton);

                            _emailService.SendMail(nextStep.ToEmail, "You receive", message, filePaths);
                        }
                    }
                }
                else // Nếu nó là action cuối cùng (kết quả) thì gửi về cho người gửi request
                {
                    //Cập nhật request
                    request.IsCompleted = true;
                    request.CurrentRequestActionID = requestAction.ID;
                    requestAction.Status = StatusEnum.Handled;
                    _requestActionService.Save();
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
                    strMessage = "Your " + requestAction.WorkFlowTemplateAction.Name + " for request " + request.WorkFlowTemplate.Name
                    + " of " + request.Initiator.FullName + " successfully.",
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
