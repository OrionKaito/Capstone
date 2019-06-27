﻿using AutoMapper;
using Capstone.Helper;
using Capstone.Model;
using Capstone.Service;
using Capstone.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

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


        // POST: api/Notifications
        [HttpPost]
        public ActionResult PostNotification(NotificationCM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                Notification notification = new Notification();
                notification = _mapper.Map<Notification>(model);
                _notificationService.Create(notification);
                return StatusCode(201, notification.ID);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/Notificaions
        [HttpGet]
        public ActionResult<IEnumerable<NotificationVM>> GetNotifications()
        {
            try
            {
                List<NotificationVM> result = new List<NotificationVM>();
                var data = _notificationService.GetAll();
                foreach (var item in data)
                {
                    result.Add(_mapper.Map<NotificationVM>(item));
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/Notifications/5
        [HttpGet("GetByID")]
        public ActionResult<NotificationVM> GetNotification(Guid ID)
        {
            try
            {
                var rs = _notificationService.GetByID(ID);
                if (rs == null) return NotFound(WebConstant.NotFound);

                NotificationVM result = _mapper.Map<NotificationVM>(rs);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // PUT: api/Notifications/5
        [HttpPut]
        public IActionResult PutNotification(NotificationUM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var notificationInDb = _notificationService.GetByID(model.ID);
                if (notificationInDb == null) return NotFound(WebConstant.NotFound);
                
                _mapper.Map(model, notificationInDb);
                _notificationService.Save();
                return Ok(WebConstant.Success);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
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