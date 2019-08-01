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
using System.Threading.Tasks;

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
            )
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
        }

        // POST: api/Requests
        [HttpPost]
        public async Task<ActionResult> PostRequest(RequestCM model)
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

                //Notification
                Notification notification = new Notification
                {
                    EventID = requestAction.ID,
                    NotificationType = NotificationEnum.ReceivedRequest,
                    CreateDate = DateTime.Now,
                };

                _notificationService.Create(notification);

                //UserNotification
                var workflowTemplateAction = _workFlowTemplateActionService.GetByID(model.NextStepID);

                if (workflowTemplateAction.IsApprovedByLineManager)
                {
                    //Push notification
                    PushNotificationToUser(userID, "Received Request", WebConstant.ReceivedRequestMessage, notification);
                }
                else
                {
                    var users = _userService.getUsersByPermissionID(workflowTemplateAction.PermissionToUseID.GetValueOrDefault());

                    if (users != null && users.Any())
                    {
                        foreach (var user in users)
                        {
                            //Push notification
                            PushNotificationToUser(user.Id, "Received Request", WebConstant.ReceivedRequestMessage, notification);
                        }
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
        public async Task<ActionResult> ApproveRequest(RequestApproveCM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                //Get Notification By RequestActionID
                var notificationByRequestActionID = _notificationService.GetByRequestActionID(model.RequestActionID);

                //check if IsHandled request
                if (notificationByRequestActionID.IsHandled == true)
                {
                    return BadRequest(WebConstant.RequestIsHandled);
                }


                //Begin transaction
                _requestService.BeginTransaction();

                var currentUser = HttpContext.User;
                var userID = currentUser.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

                //Lấy permission để kiểm tra user có quyền không
                List<Guid> userPermissions = new List<Guid>();
                userPermissions = _permissionService.GetByUserID(userID).Select(u => u.ID).ToList();

                var currentRequestAction = _requestActionService.GetByID(model.RequestActionID);

                if (!userPermissions.Contains(currentRequestAction.WorkFlowTemplateAction.PermissionToUseID.GetValueOrDefault()))
                {
                    return BadRequest(WebConstant.AccessDined);
                }

                //Hết kiểm tra
                var request = _requestService.GetByID(model.RequestID);

                //RequestAction
                RequestAction requestAction = new RequestAction
                {
                    Status = model.Status,
                    RequestID = model.RequestID,
                    ActorID = userID,
                    NextStepID = model.NextStepID,
                    CreateDate = DateTime.Now,
                };

                _requestActionService.Create(requestAction);

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
                    var users = _userService.getUsersByPermissionID(nextStep.PermissionToUseID.GetValueOrDefault());

                    if (users.IsNullOrEmpty())
                    {
                        foreach (var user in users)
                        {
                            //Push notification
                            PushNotificationToUser(user.Id, "Received Request", WebConstant.ReceivedRequestMessage, notification);
                        }
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

                //set IsHandled
                notificationByRequestActionID.IsHandled = true;
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
        public ActionResult<IEnumerable<MyRequestVM>> GetMyRequests()
        {
            try
            {
                var userID = HttpContext.User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value;

                var requests = _requestService.GetByUserID(userID).Select(r => new MyRequestVM
                {
                    ID = r.ID,
                    CreateDate = r.CreateDate,
                    CurrentRequestActionID = r.CurrentRequestActionID,
                    CurrentRequestActionName = _requestActionService.GetByID(r.CurrentRequestActionID).WorkFlowTemplateAction.Name,
                    Description = r.Description,
                    WorkFlowTemplateID = r.WorkFlowTemplateID,
                    WorkFlowTemplateName = r.WorkFlowTemplate.Name,
                    IsCompleted = r.IsCompleted,
                    IsDeleted = r.IsDeleted,
                });

                return Ok(requests);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("GetRequestResult")]
        public ActionResult<RequestResultVM> GetRequestResult(Guid requestActionID, Guid? userNotificationID)
        {
            try
            {
                //** Get WorkFlowName's Name **//
                var requestAction = _requestActionService.GetByID(requestActionID);
                var request = _requestService.GetByID(requestAction.RequestID);

                if (request == null) return BadRequest("RequestAction" + WebConstant.NotFound);

                //Set lại trạng thái isRead của userNotification khi user click vào 
                if (userNotificationID != null)
                {
                    var userNotification = _userNotificationService.GetByID(userNotificationID.GetValueOrDefault());

                    if (userNotification == null) return BadRequest("UserNotification" + WebConstant.NotFound);
                    userNotification.IsRead = true;
                    _userNotificationService.Save();
                }

                var workflow = _workFlowTemplateService.GetByID(request.WorkFlowTemplateID);

                //** Get Final Status **//
                if (requestAction.NextStepID.GetValueOrDefault() == null) return BadRequest("NextStep's " + WebConstant.NotFound);

                var status = _workFlowTemplateActionService.GetByID(requestAction.NextStepID.GetValueOrDefault()).Name;

                //** Get List Staff Request Action **//
                List<RequestResultStaffActionVM> staffResult = new List<RequestResultStaffActionVM>();
                var staffActions = _requestActionService.GetExceptActorIDAndRequestID(request.InitiatorID, request.ID);

                foreach (var staffAction in staffActions)
                {
                    var staffWorkflowaction = _workFlowTemplateActionService.GetByID(staffAction.NextStepID.GetValueOrDefault());

                    var staffStatus = _workFlowTemplateActionService.GetByID(staffWorkflowaction.ID).Name;

                    RequestResultStaffActionVM staffRequestAction = new RequestResultStaffActionVM()
                    {
                        FullName = _userManager.FindByIdAsync(staffAction.ActorID).Result.FullName,
                        UserName = _userManager.FindByIdAsync(staffAction.ActorID).Result.UserName,
                        CreateDate = staffAction.CreateDate,
                        Status = staffStatus,
                    };

                    staffResult.Add(staffRequestAction);
                }

                RequestResultVM result = new RequestResultVM
                {
                    WorkFlowTemplateName = workflow.Name,
                    Status = status,
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

                //get workFlowTemplateAction by workFlowTemplateActionID
                var workFlowTemplateAction = _workFlowTemplateActionService.GetStartByWorkFlowID(workFlowTemplateID);
                if (workFlowTemplateAction == null) return BadRequest(WebConstant.NotFound);

                //get actionType by actionTypeID in workFlowTemplateAction
                var actionType = _actionTypeService.GetByID(workFlowTemplateAction.ActionTypeID);

                //get list workFlowTemplateActionConnection by workFlowTemplateActionID (nextStepID)
                var workFlowTemplateActionConnection = _workFlowTemplateActionConnectionService
                    .GetByFromWorkflowTemplateActionID(workFlowTemplateAction.ID);

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
        public ActionResult<IEnumerable<RequestVM>> GetRequestsToHandleByPermission()
        {
            //Lấy các permission của user
            var userID = HttpContext.User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value;
            var permissions = _permissionService.GetByUserID(userID);
            List<Guid> permissionsID = new List<Guid>();
            foreach (var permission in permissions)
            {
                permissionsID.Add(permission.ID);
            }
            //Lấy các request mà chưa xong và requestaction có permission của user
            var requests = _requestService.GetRequestToApproveByPermissions(permissionsID).Select(r => new RequestVM
            {
                ID = r.ID,
                CreateDate = r.CreateDate.GetValueOrDefault(),
                Description = r.Description,
                InitiatorID = r.InitiatorID,
                InitiatorName = r.User.FullName,
                WorkFlowTemplateID = r.WorkFlowTemplateID,
                WorkFlowTemplateName = r.WorkFlowTemplate.Name,
                RequestActionID = r.CurrentRequestActionID,
            });

            if (requests.IsNullOrEmpty())
            {
                return Ok(WebConstant.NoRequestYet);
            }

            return Ok(requests);
        }

        [HttpGet("GetRequestHandleForm")]
        public ActionResult<HandleFormVM> GetHandleForm(Guid requestActionID)
        {
            try
            {
                var notificationByRequestActionID = _notificationService.GetByRequestActionID(requestActionID);
                if (notificationByRequestActionID.IsHandled == true)
                {
                    return BadRequest(WebConstant.RequestIsHandled);
                }

                //** Get Request **//
                var requestAction = _requestActionService.GetByID(requestActionID);
                var request = _requestService.GetByID(requestAction.RequestID);

                //** Get User Request Action **//
                var userAction = _requestActionService.GetByActorID(request.InitiatorID, request.ID);

                var requestFiles = _requestFileService.GetByRequestActionID(userAction.ID).Select(r => new RequestFileVM
                {
                    ID = r.ID,
                    Path = r.Path,
                    IsDeleted = r.IsDeleted,
                });

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

                //** Get List Staff Request Action **//
                List<StaffRequestActionVM> staffRequestActions = new List<StaffRequestActionVM>();
                var staffActions = _requestActionService.GetExceptActorIDAndRequestID(request.InitiatorID, request.ID);

                foreach (var staffAction in staffActions)
                {
                    var staffRequestValuesss = _requestValueService.GetByRequestActionID(staffAction.ID).Select(r => new RequestValueVM
                    {
                        ID = r.ID,
                        Key = r.Key,
                        Value = r.Value,
                    });

                    var staffWorkflowaction = _workFlowTemplateActionService.GetByID(staffAction.NextStepID.GetValueOrDefault());

                    var staffStatus = _workFlowTemplateActionService.GetByID(staffWorkflowaction.ID).Name;

                    StaffRequestActionVM staffRequestAction = new StaffRequestActionVM()
                    {
                        FullName = _userManager.FindByIdAsync(staffAction.ActorID).Result.FullName,
                        UserName = _userManager.FindByIdAsync(staffAction.ActorID).Result.UserName,
                        CreateDate = staffAction.CreateDate,
                        RequestValues = staffRequestValuesss,
                        Status = staffStatus,
                    };

                    staffRequestActions.Add(staffRequestAction);
                }

                //** Get Connections && ActionType **//
                List<ConnectionVM> connections = new List<ConnectionVM>();

                var workFlowTemplateAction = _workFlowTemplateActionService
                    .GetByID(requestAction.NextStepID.GetValueOrDefault());

                if (workFlowTemplateAction == null) return BadRequest(WebConstant.NotFound);

                //Get actionType by actionTypeID in workFlowTemplateAction
                var actionType = _actionTypeService
                    .GetByID(workFlowTemplateAction.ActionTypeID);

                //Get list workFlowTemplateActionConnection by workFlowTemplateActionID (nextStepID)
                var workFlowTemplateActionConnection = _workFlowTemplateActionConnectionService
                    .GetByFromWorkflowTemplateActionID(workFlowTemplateAction.ID);

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

                HandleFormVM form = new HandleFormVM
                {
                    InitiatorName = _userManager.FindByIdAsync(request.InitiatorID).Result.FullName,
                    WorkFlowTemplateName = _workFlowTemplateService.GetByID(request.WorkFlowTemplateID).Name,
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
