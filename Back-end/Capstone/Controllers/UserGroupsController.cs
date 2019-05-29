using AutoMapper;
using Capstone.Model;
using Capstone.Service;
using Capstone.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Capstone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserGroupsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserGroupService _userGroupService;

        public UserGroupsController(IMapper mapper, IUserGroupService userGroupService)
        {
            _mapper = mapper;
            _userGroupService = userGroupService;
        }

        // POST: api/UserGroups
        [HttpPost]
        public ActionResult PostUserGroup(UserGroupCM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                UserGroup userGroup = new UserGroup();
                userGroup = _mapper.Map<UserGroup>(model);
                _userGroupService.Create(userGroup);
                _userGroupService.Save();
                return StatusCode(201, "User Group Created!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/UserGroups
        [HttpGet("GetByUserID")]
        public ActionResult<IEnumerable<UserGroup>> GetByUserID(string ID)
        {
            try
            {
                List<UserGroupVM> result = new List<UserGroupVM>();
                var data = _userGroupService.GetByUserID(ID);
                if (data.Count() == 0) return NotFound("List empty");
                foreach (var item in data)
                {
                    result.Add(_mapper.Map<UserGroupVM>(item));
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/UserGroups/5
        [HttpGet]
        public ActionResult<UserGroup> GetUserGroup(Guid ID)
        {
            try
            {
                var rs = _userGroupService.GetByID(ID);
                if (rs == null) return NotFound("ID not found");
                UserGroupVM result = _mapper.Map<UserGroupVM>(rs);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // PUT: api/Usergroups
        [HttpPut]
        public IActionResult PutUserGroup(UserGroupUM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var userGroupInDb = _userGroupService.GetByID(model.ID);
                if (userGroupInDb == null) return BadRequest("ID not found!");
                _mapper.Map(model, userGroupInDb);
                _userGroupService.Save();
                return Ok("success");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        // DELETE: api/Usergroups
        [HttpDelete]
        public ActionResult DeleteUserGroup(Guid ID)
        {
            try
            {
                var userGroupInDb = _userGroupService.GetByID(ID);
                if (userGroupInDb == null) return NotFound("ID not found!");
                userGroupInDb.IsDelete = true;
                _userGroupService.Save();
                return Ok("success");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}