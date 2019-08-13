using AutoMapper;
using Capstone.Service;
using Capstone.Service.Helper;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Capstone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;
        private readonly IWorkFlowTemplateService _workFlowService;

        public NotificationsController(IMapper mapper, INotificationService notificationService, IWorkFlowTemplateService workFlowService)
        {
            _mapper = mapper;
            _notificationService = notificationService;
            _workFlowService = workFlowService;
        }

        // DELETE: api/Notifications/5
        [HttpDelete]
        public ActionResult DeleteNotification(Guid ID)
        {
            try
            {
                var notificationInDb = _notificationService.GetByID(ID);
                if (notificationInDb == null) return NotFound(WebConstant.NotFound);

                notificationInDb.IsDeleted = true;
                _notificationService.Save();
                return Ok(WebConstant.Success);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}