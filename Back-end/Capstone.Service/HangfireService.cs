using Capstone.Data.Infrastructrure;
using Capstone.Data.Repositories;
using Capstone.Model;
using Capstone.Service.Helper;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Capstone.Service
{
    public interface IHangfireService
    {
        void HandleByHangfire();
    }
    public class HangfireService : IHangfireService
    {
        private readonly IWorkFlowTemplateRepository _workFlowTemplateRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRequestRepository _requestRepository;
        private readonly IRequestActionRepository _requestActionRepository;
        private readonly IWorkFlowTemplateActionConnectionRepository _workFlowTemplateActionConnectionRepository;
        private readonly IWorkFlowTemplateActionRepository _workFlowTemplateActionRepository;
        private readonly IUserDeviceRepository _userDeviceRepository;
        private readonly IUserNotificationRepository _userNotificationRepository;
        private readonly IEmailService _emailService;
        private readonly IRequestService _requestService;
        private readonly INotificationService _notificationService;
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly IRequestValueService _requestValueService;
        private readonly IRequestFileService _requestFileService;
        private readonly IDataProtector _dataProtector;

        public HangfireService(IUnitOfWork unitOfWork, IRequestRepository requestRepository
            , IRequestActionRepository requestActionRepository
            , IWorkFlowTemplateActionConnectionRepository workFlowTemplateActionConnectionRepository
            , IWorkFlowTemplateActionRepository workFlowTemplateActionRepository
            , IUserDeviceRepository userDeviceRepository
            , IUserNotificationRepository userNotificationRepository
            , IEmailService emailService
            , IRequestService requestService, INotificationService notificationService
            , IConfiguration configuration
            , IRequestValueService requestValueService
            , IRequestFileService requestFileService
            , IWorkFlowTemplateRepository workFlowTemplateRepository
            , UserManager<User> userManager
            , IUserService userService
            , IDataProtectionProvider provider)
        {
            _unitOfWork = unitOfWork;
            _requestRepository = requestRepository;
            _requestActionRepository = requestActionRepository;
            _workFlowTemplateActionConnectionRepository = workFlowTemplateActionConnectionRepository;
            _workFlowTemplateActionRepository = workFlowTemplateActionRepository;
            _userDeviceRepository = userDeviceRepository;
            _userNotificationRepository = userNotificationRepository;
            _emailService = emailService;
            _requestService = requestService;
            _notificationService = notificationService;
            _configuration = configuration;
            _requestValueService = requestValueService;
            _requestFileService = requestFileService;
            _workFlowTemplateRepository = workFlowTemplateRepository;
            _userService = userService;
            _userManager = userManager;
            _dataProtector = provider.CreateProtector(WebConstant.Purpose);
        }

        public void HandleByHangfire()
        {
            var requestActionList = _requestActionRepository.GetActionByStatus(StatusEnum.Hangfire);
            foreach (var requestAction in requestActionList)
            {
                //Lấy connection dựa vào action trước và kế tiếp
                var workFlowTemplateActionConnection = _workFlowTemplateActionConnectionRepository
                    .GetByFromIDAndToID(requestAction.WorkFlowTemplateActionID.GetValueOrDefault(), requestAction.NextStepID.GetValueOrDefault());

                DateTime resultTime = new DateTime();

                // Kiểm tra type là phút
                if (workFlowTemplateActionConnection.Type == TimeEnum.Minutes)
                {
                    resultTime = requestAction.CreateDate.AddMinutes(workFlowTemplateActionConnection.TimeInterval);
                }
                // Kiểm tra type là giờ
                if (workFlowTemplateActionConnection.Type == TimeEnum.Hours)
                {
                    resultTime = requestAction.CreateDate.AddHours(workFlowTemplateActionConnection.TimeInterval);
                }
                // Kiểm tra type là ngày
                if (workFlowTemplateActionConnection.Type == TimeEnum.Days)
                {
                    resultTime = requestAction.CreateDate.AddDays(workFlowTemplateActionConnection.TimeInterval);
                }

                DateTime now = DateTime.Now;
                DateTime afterFiveMinutes = now.AddMinutes(2);

                //Kiểm tra giờ của requestAction nằm giữa giờ hiện tại và 5p sau của giờ hiện tại
                if ((DateTime.Compare(resultTime, now) > 0) && (DateTime.Compare(resultTime, afterFiveMinutes) < 0))
                {
                    requestAction.Status = StatusEnum.Pending;
                    Save();

                    var workflowTemplateAction = _workFlowTemplateActionRepository.GetByID(requestAction.NextStepID.GetValueOrDefault());
                    var notification = _notificationService.GetByRequestActionID(requestAction.ID);

                    if (workflowTemplateAction.IsApprovedByLineManager)
                    {
                        var manager = _userManager.FindByIdAsync(requestAction.Request.InitiatorID).Result;
                        var managerID = manager.LineManagerID;
                        if (managerID != "" || !string.IsNullOrEmpty(managerID))
                        {
                            //Push notification
                            PushNotificationToUser(managerID, workflowTemplateAction.WorkFlowTemplate.Name, "You received a request from " + requestAction.Request.Initiator.FullName, notification);
                        }
                    }
                    if (workflowTemplateAction.PermissionToUseID.HasValue)
                    {
                        var users = _userService.GetUsersByPermissionID(workflowTemplateAction.PermissionToUseID.GetValueOrDefault());

                        if (users != null && users.Any())
                        {
                            foreach (var user in users)
                            {
                                //Push notification
                                PushNotificationToUser(user.Id, "Received Request", "You received a request from " + requestAction.Request.Initiator.FullName, notification);
                            }
                        }
                    }
                    if (!workflowTemplateAction.ToEmail.IsNullOrEmpty())
                    {
                        var requestValue = _requestValueService.GetByRequestActionID(requestAction.ID);

                        Dictionary<string, string> dynamicform = new Dictionary<string, string>();

                        foreach (var item in requestValue)
                        {
                            dynamicform.Add(item.Key, item.Value);
                        }

                        Dictionary<string, string> listButton = new Dictionary<string, string>();
                        var connections = _workFlowTemplateActionConnectionRepository.GetByFromWorkflowTemplateActionID(workflowTemplateAction.ID);
                        string url = "";
                        foreach (var connection in connections)
                        {
                            url = (_configuration["UrlCapstoneMvc"]
                                    + "/home/ApproveRequest/?content="
                                    + _dataProtector.Protect("RequestID="
                                        + requestAction.Request.ID
                                        + "&RequestActionID="
                                        + requestAction.ID
                                        + "&NextStepID="
                                        + connection.ToWorkFlowTemplateActionID)
                                    );
                            listButton.Add(url, connection.ConnectionType.Name);
                        }

                        string message = _emailService.GenerateMessageTest(workflowTemplateAction.ToEmail
                            , "Dynamic Workflow"
                            , _workFlowTemplateRepository.GetById(requestAction.Request.WorkFlowTemplateID).Name
                            , workflowTemplateAction.Name
                            , dynamicform
                            , listButton);

                        _emailService.SendMail(workflowTemplateAction.ToEmail, "You receive request.", message, new List<string>());
                    }
                }
            }
        }
        public void Save()
        {
            _unitOfWork.Commit();
        }
        private async void PushNotificationToUser(string userID, string title, string body, Notification notification)
        {
            //get list user device by userID
            var userDeviceList = _userDeviceRepository.GetDeviceTokenByUserID(userID);

            if (userDeviceList.Count() == 0)
            {
                UserNotification userNotification = new UserNotification
                {
                    NotificationID = notification.ID,
                    UserID = userID,
                    IsSend = false,
                };
                _userNotificationRepository.Add(userNotification);
                _unitOfWork.Commit();
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
                _userNotificationRepository.Add(userNotification);
                _unitOfWork.Commit();

                string[] deviceTokens = deviceTokenList.ToArray();

                bool sent = await PushNotification.SendMessageAsync(deviceTokens, title, body);
            }
        }
    }

}
