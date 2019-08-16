using AutoMapper;
using Capstone.Model;
using Capstone.Service;
using Capstone.Service.Helper;
using Capstone.ViewModel;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Capstone.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        private readonly IActionTypeService _actionTypeService;
        private readonly IMapper _mapper;
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

        public RequestsController(IActionTypeService actionTypeService
            , IMapper mapper
            , IRequestService requestService
            , IRequestActionService requestActionService
            , IRequestValueService requestValueService
            , IRequestFileService requestFileService
            , INotificationService notificationService
            , IUserNotificationService userNotificationService
            , IUserService userService
            , IWorkFlowTemplateActionService workFlowTemplateActionService
            , IWorkFlowTemplateActionConnectionService workFlowTemplateActionConnectionService
            , IConnectionTypeService connectionTypeService
            , UserManager<User> userManager
            , IWorkFlowTemplateService workFlowTemplateService
            , IPermissionService permissionService
            , IUserDeviceService userDeviceService
            , IEmailService emailService
            , IConfiguration configuration
            , IDataProtectionProvider provider)
        {
            _actionTypeService = actionTypeService;
            _mapper = mapper;
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
            _userManager = userManager;
            _workFlowTemplateService = workFlowTemplateService;
            _permissionService = permissionService;
            _userDeviceService = userDeviceService;
            _emailService = emailService;
            _configuration = configuration;
            _dataProtector = provider.CreateProtector(WebConstant.Purpose);
        }

        // POST: api/Requests
        [HttpPost]
        public ActionResult PostRequest(RequestCM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                //Begin transaction
                _requestService.BeginTransaction();

                var currentUser = HttpContext.User;
                var userID = currentUser.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

                //Request
                Request request = new Request
                {
                    InitiatorID = userID,
                    Description = model.Description,
                    WorkFlowTemplateID = model.WorkFlowTemplateID,
                    CreateDate = DateTime.Now,
                };

                _requestService.Create(request);

                //RequestAction
                RequestAction requestAction = new RequestAction
                {
                    Status = StatusEnum.Pending,
                    RequestID = request.ID,
                    ActorID = userID,
                    WorkFlowTemplateActionID = model.WorkFlowTemplateActionID,
                    NextStepID = model.NextStepID,
                    CreateDate = DateTime.Now,
                };

                _requestActionService.Create(requestAction);

                //Cập nhật current request action
                request.CurrentRequestActionID = requestAction.ID;

                //RequestValue
                foreach (var value in model.ActionValues)
                {
                    RequestValue requestValue = new RequestValue
                    {
                        Key = value.Key,
                        Value = value.Value,
                        RequestActionID = requestAction.ID,
                    };

                    _requestValueService.Create(requestValue);
                }

                //RequestFile
                foreach (var path in model.ImagePaths)
                {
                    RequestFile requestFile = new RequestFile
                    {
                        Path = path,
                        RequestActionID = requestAction.ID,
                    };
                    _requestFileService.Create(requestFile);
                }


                //list file path to send email
                List<string> filePaths = new List<string>();
                if (!model.ImagePaths.IsNullOrEmpty())
                {
                    foreach (var path in model.ImagePaths)
                    {
                        filePaths.Add(path);
                    }
                }

                //Notification
                Notification notification = new Notification
                {
                    EventID = requestAction.ID,
                    NotificationType = NotificationEnum.ReceivedRequest,
                    CreateDate = DateTime.Now,
                };

                _notificationService.Create(notification);

                var workflowTemplateAction = _workFlowTemplateActionService.GetByID(model.NextStepID);

                //Lấy connection dựa vào action trước và kế tiếp
                var workFlowTemplateActionConnection = _workFlowTemplateActionConnectionService
                    .GetByFromIDAndToID(model.WorkFlowTemplateActionID, model.NextStepID);

                //kiểm tra connection coi có bị hangfire không?
                //nếu có thì để status của requestAction là Hangfire
                if (workFlowTemplateActionConnection.TimeInterval > 0)
                {
                    requestAction.Status = StatusEnum.Hangfire;
                    _requestActionService.Save();
                }
                else
                {
                    if (workflowTemplateAction.IsApprovedByLineManager)
                    {
                        var manager = _userManager.FindByIdAsync(userID).Result;
                        var managerID = manager.LineManagerID;
                        if (managerID != "" || !string.IsNullOrEmpty(managerID))
                        {
                            //Push notification
                            PushNotificationToUser(managerID, "Received Request", WebConstant.ReceivedRequestMessage, notification);
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
                                PushNotificationToUser(user.Id, "Received Request", WebConstant.ReceivedRequestMessage, notification);
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
                        var connections = _workFlowTemplateActionConnectionService.GetByFromWorkflowTemplateActionID(workflowTemplateAction.ID);
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

                        string message = _emailService.GenerateMessageTest(workflowTemplateAction.ToEmail
                            , "Dynamic Workflow"
                            , _workFlowTemplateService.GetByID(request.WorkFlowTemplateID).Name
                            , workflowTemplateAction.Name
                            , dynamicform
                            , null
                            , listButton);

                        _emailService.SendMail(workflowTemplateAction.ToEmail, "You receive request.", message, filePaths);

                    }
                }
                //End transaction
                _requestService.CommitTransaction();

                return StatusCode(201, request.ID);
            }
            catch (Exception e)
            {
                _requestService.RollBack();
                return BadRequest(e.Message);
            }
        }

        [HttpPost("ApproveRequest")]
        public ActionResult ApproveRequest(RequestApproveCM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                //Begin transaction
                _requestService.BeginTransaction();

                var currentUser = HttpContext.User;
                var userID = currentUser.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

                //Lấy permission để kiểm tra user có quyền không
                List<Guid> userPermissions = new List<Guid>();
                userPermissions = _permissionService.GetByUserID(userID).Select(u => u.ID).ToList();

                var currentRequestAction = _requestActionService.GetByID(model.RequestActionID);

                //Kiểm tra request action đã được xử lý chưa
                if (currentRequestAction.Status == StatusEnum.Handled)
                {
                    return BadRequest(WebConstant.RequestIsHandled);
                }

                if (currentRequestAction.Status == StatusEnum.Hangfire)
                {
                    return BadRequest(WebConstant.RequestHangfire);
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
                    ActorID = userID,
                    NextStepID = model.NextStepID,
                    CreateDate = DateTime.Now,
                    WorkFlowTemplateActionID = currentRequestAction.NextStepID,
                };

                _requestActionService.Create(requestAction);

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

                //RequestValue
                foreach (var value in model.ActionValues)
                {
                    RequestValue requestValue = new RequestValue
                    {
                        Key = value.Key,
                        Value = value.Value,
                        RequestActionID = requestAction.ID,
                    };

                    _requestValueService.Create(requestValue);
                }

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
                            if (!userPermissions.Contains(currentRequestAction.NextStep.PermissionToUseID.GetValueOrDefault()))
                            {
                                return BadRequest(WebConstant.AccessDined);
                            }
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

                //End transaction
                _requestService.CommitTransaction();

                return StatusCode(201, requestAction.ID);
            }
            catch (Exception e)
            {
                _requestService.RollBack();
                return BadRequest(e.Message);
            }
        }

        [HttpGet("GetMyRequests")]
        public ActionResult<IEnumerable<MyRequestPaginVM>> GetMyRequests(int? numberOfPage, int? NumberOfRecord)
        {
            try
            {
                var page = numberOfPage ?? 1;
                var count = NumberOfRecord ?? WebConstant.DefaultPageRecordCount;

                var userID = HttpContext.User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value;

                var requests = _requestService.GetByUserID(userID).Select(r => new MyRequestVM
                {
                    ID = r.ID,
                    CreateDate = r.CreateDate,
                    CurrentRequestActionID = r.CurrentRequestActionID,
                    CurrentRequestActionName = _requestActionService.GetByID(r.CurrentRequestActionID).NextStep.Name,
                    Description = r.Description,
                    WorkFlowTemplateID = r.WorkFlowTemplateID,
                    WorkFlowTemplateName = r.WorkFlowTemplate.Name,
                    IsCompleted = r.IsCompleted,
                    IsDeleted = r.IsDeleted,
                }).OrderByDescending(r => r.CreateDate);

                MyRequestPaginVM myRequestPaginVM = new MyRequestPaginVM
                {
                    TotalRecord = requests.Count(),
                    MyRequests = requests.Skip((page - 1) * count).Take(count),
                };

                return Ok(myRequestPaginVM);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("GetRequestResult")]
        public ActionResult<RequestResultVM> GetRequestResult(Guid requestActionID)
        {
            try
            {
                //Lấy ra request và request action
                var requestAction = _requestActionService.GetByID(requestActionID);
                var request = _requestService.GetByID(requestAction.RequestID);

                if (request == null) return BadRequest("RequestAction" + WebConstant.NotFound);

                var workflow = _workFlowTemplateService.GetByID(request.WorkFlowTemplateID);

                //*Lấy kết quả cuối cùng
                var finalStatus = _workFlowTemplateActionService.GetByID(requestAction.NextStepID.GetValueOrDefault()).Name;

                //Lấy phần thông tin của những người duyệt 
                List<StaffRequestActionVM> staffResult = new List<StaffRequestActionVM>();
                var startActionTemplate = _workFlowTemplateActionService.GetStartByWorkFlowID(workflow.ID);
                var staffActions = _requestActionService.GetExceptStartAction(startActionTemplate.ID, request.ID);

                foreach (var staffAction in staffActions)
                {
                    var staffRequestValues = _requestValueService.GetByRequestActionID(staffAction.ID).Select(r => new RequestValueVM
                    {
                        ID = r.ID,
                        Key = r.Key,
                        Value = r.Value,
                    });

                    var staffStatus = "";

                    var toWorkflowTemplateActionConnection = _workFlowTemplateActionConnectionService.GetByToWorkflowTemplateActionID(staffAction.WorkFlowTemplateAction.ID);

                    staffStatus = _workFlowTemplateActionConnectionService
                        .GetByFromIDAndToID(staffAction.WorkFlowTemplateActionID.GetValueOrDefault(), staffAction.NextStep.ID)
                        .ConnectionType.Name;

                    StaffRequestActionVM staffRequestAction = new StaffRequestActionVM()
                    {
                        FullName = staffAction.Actor == null ? staffAction.ActorEmail : staffAction.Actor.FullName,
                        UserName = staffAction.ActorID == null ? staffAction.ActorEmail : _userManager.FindByIdAsync(staffAction.ActorID).Result.UserName,
                        CreateDate = staffAction.CreateDate,
                        RequestValues = staffRequestValues,
                        Status = staffStatus,
                        WorkFlowActionName = staffAction.WorkFlowTemplateAction.Name,
                    };

                    staffResult.Add(staffRequestAction);
                }

                RequestResultVM result = new RequestResultVM
                {
                    WorkFlowTemplateName = workflow.Name,
                    Status = finalStatus,
                    StaffResult = staffResult,
                };

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("GetRequestForm")]
        public ActionResult<RequestFormVM> GetRequestForm(Guid workFlowTemplateID)
        {
            try
            {
                List<ConnectionVM> connections = new List<ConnectionVM>();

                //Lấy template action đầu tiên của quy trình
                var startActionTemplate = _workFlowTemplateActionService.GetStartByWorkFlowID(workFlowTemplateID);
                if (startActionTemplate == null) return BadRequest(WebConstant.NotFound);

                //Lấy form động
                var actionType = _actionTypeService.GetByID(startActionTemplate.ActionTypeID.GetValueOrDefault());

                //Lấy các connection để thể hiện các bước tiếp theo bằng button
                var workFlowTemplateActionConnection = _workFlowTemplateActionConnectionService
                    .GetByFromWorkflowTemplateActionID(startActionTemplate.ID);

                foreach (var item in workFlowTemplateActionConnection)
                {
                    connections.Add(new ConnectionVM
                    {
                        NextStepID = _workFlowTemplateActionService
                        .GetByID(item.ToWorkFlowTemplateActionID)
                        .ID,

                        ConnectionTypeName = _connectionTypeService
                        .GetByID(item.ConnectionTypeID)
                        .Name,

                        ConnectionID = _connectionTypeService
                        .GetByID(item.ConnectionTypeID)
                        .ID


                    });
                }

                RequestFormVM form = new RequestFormVM
                {
                    WorkFlowName = _workFlowTemplateService.GetByID(workFlowTemplateID).Name,
                    WorkFlowTemplateActionName = startActionTemplate.Name,
                    WorkFlowTemplateActionID = startActionTemplate.ID,
                    Connections = connections,
                    ActionType = _mapper.Map<ActionTypeVM>(actionType)
                };

                return Ok(form);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("GetRequestsToHandleByPermission")]
        public ActionResult<IEnumerable<RequestPaginVM>> GetRequestsToHandleByPermission(int? numberOfPage, int? NumberOfRecord)
        {
            var page = numberOfPage ?? 1;
            var count = NumberOfRecord ?? WebConstant.DefaultPageRecordCount;
            //Lấy các permission của user
            var userID = HttpContext.User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value;
            var permissions = _permissionService.GetByUserID(userID);
            List<Guid> permissionsID = new List<Guid>();
            foreach (var permission in permissions)
            {
                permissionsID.Add(permission.ID);
            }

            //Lấy các request mà chưa xong và request action có permission của user
            List<RequestVM> requests = _requestService.GetRequestToApproveByPermissions(permissionsID).Select(r => new RequestVM
            {
                ID = r.ID,
                CreateDate = r.CreateDate.GetValueOrDefault(),
                Description = r.Description,
                InitiatorID = r.InitiatorID,
                InitiatorName = r.Initiator.FullName,
                WorkFlowTemplateID = r.WorkFlowTemplateID,
                WorkFlowTemplateName = r.WorkFlowTemplate.Name,
                RequestActionID = r.CurrentRequestActionID,
            }).ToList();

            //Lấy các request mà chưa xong và có approve by line manager
            var requestApproveByManager = _requestService.GetRequestToApproveByLineManager();
            //List<RequestVM> listRequestApproveByManager = new List<RequestVM>();
            foreach (var item in requestApproveByManager)
            {
                if (!item.Initiator.LineManagerID.IsNullOrEmpty() && item.Initiator.LineManagerID.Equals(userID))
                {
                    requests.Add(new RequestVM
                    {
                        ID = item.ID,
                        CreateDate = item.CreateDate.GetValueOrDefault(),
                        Description = item.Description,
                        InitiatorID = item.InitiatorID,
                        InitiatorName = item.Initiator.FullName,
                        WorkFlowTemplateID = item.WorkFlowTemplateID,
                        WorkFlowTemplateName = item.WorkFlowTemplate.Name,
                        RequestActionID = item.CurrentRequestActionID,
                    });
                }
            }
            
            var allRequestApproveByUser = requests.OrderByDescending(r => r.CreateDate).ToList();

            if (allRequestApproveByUser.IsNullOrEmpty())
            {
                return Ok(new RequestPaginVM{
                    Requests = new List<RequestVM>()
                });
            }

            RequestPaginVM requestPaginVM = new RequestPaginVM
            {
                TotalRecord = allRequestApproveByUser.Count(),
                Requests = allRequestApproveByUser.Skip((page - 1) * count).Take(count),
            };

            return Ok(requestPaginVM);
        }

        [HttpGet("GetRequestHandleForm")]
        public ActionResult<HandleFormVM> GetHandleForm(Guid requestActionID)
        {
            try
            {
                //Lấy request
                var requestAction = _requestActionService.GetByID(requestActionID);
                var request = _requestService.GetByID(requestAction.RequestID);

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

                //Lấy form data gồm nhiều field
                var userRequestValues = _requestValueService.GetByRequestActionID(userAction.ID).Select(r => new RequestValueVM
                {
                    ID = r.ID,
                    Key = r.Key,
                    Value = r.Value,
                });

                UserRequestActionVM userRequestAction = new UserRequestActionVM()
                {
                    RequestFiles = requestFiles,
                    RequestValues = userRequestValues,
                };

                //Lấy phần thông tin của những người duyệt trước
                List<StaffRequestActionVM> staffRequestActions = new List<StaffRequestActionVM>();
                var staffActions = _requestActionService.GetExceptStartAction(startActionTemplate.ID, request.ID);

                foreach (var staffAction in staffActions)
                {
                    var staffRequestValues = _requestValueService.GetByRequestActionID(staffAction.ID).Select(r => new RequestValueVM
                    {
                        ID = r.ID,
                        Key = r.Key,
                        Value = r.Value,
                    });

                    var staffWorkflowaction = _workFlowTemplateActionService.GetByID(staffAction.NextStepID.GetValueOrDefault());

                    var staffStatus = _workFlowTemplateActionConnectionService
                        .GetByFromIDAndToID(staffAction.WorkFlowTemplateAction.ID, staffAction.NextStep.ID)
                        .ConnectionType.Name; ;

                    StaffRequestActionVM staffRequestAction = new StaffRequestActionVM()
                    {
                        FullName = staffAction.Actor.FullName,
                        UserName = _userManager.FindByIdAsync(staffAction.ActorID).Result.UserName,
                        CreateDate = staffAction.CreateDate,
                        RequestValues = staffRequestValues,
                        Status = staffStatus,
                        WorkFlowActionName = staffAction.WorkFlowTemplateAction.Name,
                    };

                    staffRequestActions.Add(staffRequestAction);
                }

                //Lấy template action
                var workFlowTemplateAction = _workFlowTemplateActionService
                    .GetByID(requestAction.NextStepID.GetValueOrDefault());

                if (workFlowTemplateAction == null) return BadRequest(WebConstant.NotFound);

                //Lấy form 
                var actionType = workFlowTemplateAction.ActionType;

                //Lấy các connection tới các action tiếp theo được thể hiện bằng các button
                var templateConnection = _workFlowTemplateActionConnectionService
                    .GetByFromWorkflowTemplateActionID(workFlowTemplateAction.ID);

                List<ConnectionVM> connections = new List<ConnectionVM>();
                foreach (var connection in templateConnection)
                {
                    connections.Add(new ConnectionVM
                    {
                        NextStepID = connection.ToWorkFlowTemplateActionID,

                        ConnectionTypeName = connection.ConnectionType.Name,

                        ConnectionID = connection.ConnectionType.ID
                    });
                }

                HandleFormVM form = new HandleFormVM
                {
                    InitiatorName = request.Initiator.FullName,
                    WorkFlowTemplateName = request.WorkFlowTemplate.Name,
                    WorkFlowTemplateActionID = workFlowTemplateAction.ID,
                    WorkFlowTemplateActionName = workFlowTemplateAction.Name,
                    Connections = connections,
                    ActionType = _mapper.Map<ActionTypeVM>(actionType),
                    Request = _mapper.Map<RequestVM>(request),
                    UserRequestAction = userRequestAction,
                    StaffRequestActions = staffRequestActions,
                };

                return Ok(form);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/Requests/5
        [HttpDelete("{id}")]
        public ActionResult DeleteRequest([FromHeader]Guid ID)
        {
            try
            {
                var requestInDb = _requestService.GetByID(ID);
                if (requestInDb == null) return BadRequest(WebConstant.NotFound);

                requestInDb.IsDeleted = true;
                _requestService.Save();
                return Ok(WebConstant.Success);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
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
