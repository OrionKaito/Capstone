﻿using AutoMapper;
using Capstone.Model;
using Capstone.Service;
using Capstone.Service.Helper;
using Capstone.ViewModel;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<User> _userManager;
        private readonly IGroupService _groupService;

        public UserGroupsController(IMapper mapper, IUserGroupService userGroupService, UserManager<User> userManager, IGroupService groupService)
        {
            _mapper = mapper;
            _userGroupService = userGroupService;
            _userManager = userManager;
            _groupService = groupService;
        }

        // POST: api/UserGroups
        [HttpPost]
        public ActionResult PostUserGroup(UserGroupCM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var checkExist = _userGroupService.CheckExist(model.UserId, model.GroupID);
                if (checkExist != null) return BadRequest("Existed!");

                UserGroup userGroup = new UserGroup();
                userGroup = _mapper.Map<UserGroup>(model);
                _userGroupService.Create(userGroup);
                return StatusCode(201, userGroup.ID);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/UserGroups
        [HttpGet("GetByUserID")]
        public ActionResult<IEnumerable<UserGroupVM>> GetByUserID(string ID)
        {
            try
            {
                List<UserGroupVM> result = new List<UserGroupVM>();
                var data = _userGroupService.GetByUserID(ID);
                if (data.Count() == 0) return NotFound(WebConstant.EmptyList);
                foreach (var item in data)
                {
                    result.Add(new UserGroupVM
                    {
                        ID = item.ID,
                        UserId = item.UserID,
                        FullName = _userManager.FindByIdAsync(item.UserID).Result.FullName,
                        GroupID = item.GroupID,
                        GroupName = _groupService.GetByID(item.GroupID).Name
                    });
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/UserGroups/5
        [HttpGet("GetByUserGroupID")]
        public ActionResult<UserGroupVM> GetByUserGroupID(Guid ID)
        {
            try
            {
                var userGroup = _userGroupService.GetByID(ID);
                if (userGroup == null) return NotFound(WebConstant.NotFound);

                UserGroupVM result = new UserGroupVM
                {
                    ID = userGroup.ID,
                    UserId = userGroup.UserID,
                    FullName = _userManager.FindByIdAsync(userGroup.UserID).Result.FullName,
                    GroupID = userGroup.GroupID,
                    GroupName = _groupService.GetByID(userGroup.GroupID).Name
                };
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // PUT: api/Usergroups
        [HttpPut]
        public ActionResult PutUserGroup(UserGroupUM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var checkExist = _userGroupService.CheckExist(model.UserId, model.GroupID);
                if (checkExist != null) return BadRequest("Existed!");

                var userGroupInDb = _userGroupService.GetByID(model.ID);
                if (userGroupInDb == null) return BadRequest(WebConstant.NotFound);
                _mapper.Map(model, userGroupInDb);
                _userGroupService.Save();
                return Ok(WebConstant.Success);
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
                if (userGroupInDb == null) return NotFound(WebConstant.NotFound);
                userGroupInDb.IsDeleted = true;
                _userGroupService.Save();
                return Ok(WebConstant.Success);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}