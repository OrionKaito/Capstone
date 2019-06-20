using AutoMapper;
using Capstone.Helper;
using Capstone.Model;
using Capstone.Service;
using Capstone.ViewModel;
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
        private readonly IMapper _mapper;
        private readonly IRequestService _requestService;
        private readonly IRequestActionService _requestActionService;
        private readonly IRequestValueService _requestValueService;
        private readonly IRequestFileService _requestFileService;
        private readonly INotificationService _notificationService;
        private readonly IUserNotificationService _userNotificationService;
        private readonly IUserService _userService;
        private readonly IWorkFlowTemplateActionService _workFlowTemplateActionService;

        public RequestsController(IMapper mapper, IRequestService requestService
            , IRequestActionService requestActionService
            , IRequestValueService requestValueService
            , IRequestFileService requestFileService
            , INotificationService notificationService
            , IUserNotificationService userNotificationService
            , IUserService userService, IWorkFlowTemplateActionService workFlowTemplateActionService)
        {
            _mapper = mapper;
            _requestService = requestService;
            _requestActionService = requestActionService;
            _requestValueService = requestValueService;
            _requestFileService = requestFileService;
            _notificationService = notificationService;
            _userNotificationService = userNotificationService;
            _userService = userService;
            _workFlowTemplateActionService = workFlowTemplateActionService;
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

                var currentUSer = HttpContext.User;
                var userID = currentUSer.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

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
                    Status = model.Status,
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
                RequestFile requestFile = new RequestFile
                {
                    Path = model.ImagePath,
                    RequestActionID = requestAction.ID,
                };

                _requestFileService.Create(requestFile);
                //Notification
                Notification notification = new Notification
                {
                    EventID = request.ID,
                    NotificationType = NotificationEnum.ReceivedRequest,
                    CreateDate = DateTime.Now,

                    ID = Guid.NewGuid(),
                    IsDeleted = false,
                };

                _notificationService.Create(notification);

                //UserNotification
                var workflowTemplateAction = _workFlowTemplateActionService.GetByID(model.NextStepID);
                var users = _userService.getUsersByPermissionID(workflowTemplateAction.PermissionToUseID);

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
