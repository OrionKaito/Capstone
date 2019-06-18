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
        private readonly INotificationService _notificationService;
        private readonly IUserNotificationService _userNotificationService;

        public RequestsController(IMapper mapper, IRequestService requestService, 
            INotificationService notificationService, IUserNotificationService userNotificationService)
        {
            _mapper = mapper;
            _requestService = requestService;
            _notificationService = notificationService;
            _userNotificationService = userNotificationService;
        }

        // POST: api/Requests
        [HttpPost]
        public ActionResult<RequestVM> PostRequest(RequestCM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var currentUSer = HttpContext.User;
                var userID = currentUSer.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

                Request request = _mapper.Map<Request>(model);
                request.InitiatorID = userID;

                request.CreateDate = DateTime.Now;
                _requestService.Create(request);

                Notification notification = new Notification
                {
                    DateTime = DateTime.Now,
                    EventID = request.ID,
                    NotificationType = NotificationEnum.ReceivedRequest,
                };

                _notificationService.Create(notification);

                // chỗ này thêm thằng nhận request

                return StatusCode(201, request.ID);
            }
            catch (Exception e)
            {
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
        public ActionResult<Request> GetRequest(Guid ID)
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
        [HttpPut]
        public IActionResult PutRequest(RequestUM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var requestInDb = _requestService.GetByID(model.ID);
                if (requestInDb == null) return BadRequest(WebConstant.NotFound);
                _mapper.Map(model, requestInDb);
                _requestService.Save();
                return Ok(WebConstant.Success);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/Requests/5
        [HttpDelete("{id}")]
        public ActionResult DeleteRequest(Guid ID)
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
