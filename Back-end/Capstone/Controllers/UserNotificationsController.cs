using AutoMapper;
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
    public class UserNotificationsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserNotificationService _userNotificationService;

        public UserNotificationsController(IMapper mapper, IUserNotificationService userNotificationService)
        {
            _mapper = mapper;
            _userNotificationService = userNotificationService;
        }

        // POST: api/UserNotifications
        [HttpPost]
        public ActionResult<UserNotification> PostUserNotification(UserNotificationCM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                UserNotification userNotification = new UserNotification();
                userNotification = _mapper.Map<UserNotification>(model);
                _userNotificationService.Create(userNotification);
                return StatusCode(201, userNotification.ID);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/UserNotifications
        [HttpGet]
        public ActionResult<IEnumerable<UserNotification>> GetUserNotifications()
        {
            try
            {
                List<UserNotificationVM> result = new List<UserNotificationVM>();
                var data = _userNotificationService.GetAll();
                foreach (var item in data)
                {
                    result.Add(_mapper.Map<UserNotificationVM>(item));
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/UserNotifications/5
        [HttpGet("GetByID")]
        public ActionResult<UserNotification> GetUserNotification(Guid ID)
        {
            try
            {
                var rs = _userNotificationService.GetByID(ID);
                if (rs == null) return NotFound("ID not found!");

                UserNotificationVM result = _mapper.Map<UserNotificationVM>(rs);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // PUT: api/UserNotifications/5
        [HttpPut]
        public IActionResult PutUserNotification(UserNotificationUM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var userNotificationInDb = _userNotificationService.GetByID(model.ID);
                if (userNotificationInDb == null) return NotFound("ID not found!");

                _mapper.Map(model, userNotificationInDb);
                _userNotificationService.Save();
                return Ok("success");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/UserNotifications/5
        [HttpDelete]
        public ActionResult DeleteUserNotification(Guid ID)
        {
            try
            {
                var userNotificationInDb = _userNotificationService.GetByID(ID);
                if (userNotificationInDb == null) return NotFound("ID not found!");

                userNotificationInDb.IsDeleted = true;
                _userNotificationService.Save();
                return Ok("success");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}