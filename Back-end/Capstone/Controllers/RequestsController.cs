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
        private readonly UserManager<User> _userManager;

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

                    ID = Guid.NewGuid(),
                    IsDeleted = false,
                };

                _notificationService.Create(notification);

                //UserNotification
                var workflowTemplateAction = _workFlowTemplateActionService.GetByID(model.NextStepID);

                if (workflowTemplateAction.IsApprovedByLineManager)
                {
                    var user = _userManager.FindByIdAsync(userID).Result;

                    UserNotification userNotification = new UserNotification
                    {
                        NotificationID = notification.ID,
                        UserID = user.Id,
                    };
                    _userNotificationService.Create(userNotification);

                }
                else
                {
                    var users = _userService.getUsersByPermissionID(workflowTemplateAction.PermissionToUseID.GetValueOrDefault());

                    if (users != null && users.Any())
                    {
                        foreach (var user in users)
                        {
                            UserNotification userNotification = new UserNotification
                            {
                                NotificationID = notification.ID,
                                UserID = user.Id,
                            };
                            _userNotificationService.Create(userNotification);
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
        public ActionResult ApproveRequest(RequestApproveCM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                //Begin transaction
                _requestService.BeginTransaction();

                var currentUser = HttpContext.User;
                var userID = currentUser.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

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

                if (!nextStep.IsEnd) //If this last step or not
                {
                    //Notification
                    Notification notification = new Notification
                    {
                        EventID = requestAction.ID,
                        NotificationType = NotificationEnum.ReceivedRequest,
                        CreateDate = DateTime.Now,

                        ID = Guid.NewGuid(),
                        IsDeleted = false,
                    };

                    _notificationService.Create(notification);

                    //UserNotification

                    if (nextStep.IsApprovedByLineManager)
                    {
                        var ownerID = _requestService.GetByID(model.RequestID).InitiatorID;
                        var managerID = _userManager.FindByIdAsync(ownerID).Result.ManagerID;

                        UserNotification userNotification = new UserNotification
                        {
                            NotificationID = notification.ID,
                            UserID = managerID,
                        };
                        _userNotificationService.Create(userNotification);

                    }
                    else
                    {
                        var users = _userService.getUsersByPermissionID(nextStep.PermissionToUseID.GetValueOrDefault());

                        if (users != null && users.Any())
                        {
                            foreach (var user in users)
                            {
                                UserNotification userNotification = new UserNotification
                                {
                                    NotificationID = notification.ID,
                                    UserID = user.Id,
                                };
                                _userNotificationService.Create(userNotification);
                            }
                        }
                    }
                }
                else
                {
                    //Notification
                    Notification notification = new Notification
                    {
                        EventID = requestAction.ID,
                        NotificationType = NotificationEnum.CompletedRequest,
                        CreateDate = DateTime.Now,

                        ID = Guid.NewGuid(),
                        IsDeleted = false,
                    };

                    _notificationService.Create(notification);

                    var ownerID = _requestService.GetByID(model.RequestID).InitiatorID;
                    UserNotification userNotification = new UserNotification
                    {
                        NotificationID = notification.ID,
                        UserID = ownerID,
                    };
                    _userNotificationService.Create(userNotification);
                }

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

        // GET: api/Requests
        [HttpGet]
        public ActionResult<IEnumerable<RequestVM>> GetRequests()
        {
            try
            {
                List<RequestVM> result = new List<RequestVM>();
                var data = _requestService.GetAll();
                foreach (var item in data)
                {
                    result.Add(_mapper.Map<RequestVM>(item));
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/Requests/GetByID
        [HttpGet("GetByID")]
        public ActionResult<RequestVM> GetRequest(Guid ID)
        {
            try
            {
                var rs = _requestService.GetByID(ID);
                if (rs == null) return BadRequest(WebConstant.NotFound);
                RequestVM result = _mapper.Map<RequestVM>(rs);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("GetByID")]
        public ActionResult<RequestResultVM> GetRequestResult(Guid requestActionID)
        {
            try
            {
                var requestAction = _requestActionService.GetByID(requestActionID);
                var request = _requestService.GetByID(requestAction.RequestID);

                if (request == null) return BadRequest(WebConstant.NotFound);

                var workflow = _workFlowTemplateService.GetByID(request.ID);

                //get list workFlowTemplateActionConnection by workFlowTemplateActionID (nextStepID)
                var workFlowTemplateActionConnection = _workFlowTemplateActionConnectionService
                    .GetByFromIDAndToID(requestActionID, requestAction.NextStepID.GetValueOrDefault());

                if (workFlowTemplateActionConnection == null)
                    return BadRequest("NextStep's ID or RequestAction's " + WebConstant.NotFound);

                RequestResultVM result = new RequestResultVM
                {
                    WorkFlowTemplateName = workflow.Name,
                    Status = _connectionTypeService
                        .GetByID(workFlowTemplateActionConnection.ConnectionTypeID)
                        .Name,
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

        [HttpGet("GetRequestHandleForm")]
        public ActionResult<HandleFormVM> GetHandleForm(Guid requestActionID)
        {
            try
            {
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

                ///
                /// BUGGGGGGGGGGGGGGGGGGGGGGGGGG
                /// //gGUBAsdasdasd

                foreach (var staffAction in staffActions)
                {
                    var staffRequestValuesss = _requestValueService.GetByRequestActionID(staffAction.ID).Select(r => new RequestValueVM
                    {
                        ID = r.ID,
                        Key = r.Key,
                        Value = r.Value,
                    });

                    StaffRequestActionVM staffRequestAction = new StaffRequestActionVM()
                    {
                        Name = _userManager.FindByIdAsync(staffAction.ActorID).Result.FullName,
                        CreateDate = staffAction.CreateDate,
                        RequestValues = staffRequestValuesss,
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

        // PUT: api/Requests
        // Người gửi không có quyền update
        //[HttpPut]
        //public IActionResult PutRequest(RequestUM model)
        //{
        //    if (!ModelState.IsValid) return BadRequest(ModelState);

        //    try
        //    {
        //        var requestInDb = _requestService.GetByID(model.ID);
        //        if (requestInDb == null) return BadRequest(WebConstant.NotFound);
        //        _mapper.Map(model, requestInDb);
        //        _requestService.Save();
        //        return Ok(WebConstant.Success);
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.Message);
        //    }
        //}

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
    }
}
