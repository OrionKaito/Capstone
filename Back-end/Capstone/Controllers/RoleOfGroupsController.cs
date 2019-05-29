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
    public class RoleOfGroupsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRoleOfGroupService _roleOfGroupService;

        public RoleOfGroupsController(IMapper mapper, IRoleOfGroupService roleOfGroupService)
        {
            _mapper = mapper;
            _roleOfGroupService = roleOfGroupService;
        }

        // POST: api/RoleOfGroups
        [HttpPost]
        public ActionResult<RoleOfGroup> PostRoleOfGroup(RoleOfGroupCM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var checkExist = _roleOfGroupService.CheckExist(model.RoleID, model.GroupID);
                if (checkExist != null) return BadRequest("Existed!");

                RoleOfGroup roleOfGroup = new RoleOfGroup();
                roleOfGroup = _mapper.Map<RoleOfGroup>(model);
                _roleOfGroupService.Create(roleOfGroup);
                return StatusCode(201, roleOfGroup.ID);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/RoleOfGroups
        [HttpGet]
        public ActionResult<IEnumerable<RoleOfGroup>> GetRoleOfGroups()
        {
            try
            {
                List<RoleOfGroupVM> result = new List<RoleOfGroupVM>();
                var data = _roleOfGroupService.GetAll();
                foreach (var item in data)
                {
                    result.Add(_mapper.Map<RoleOfGroupVM>(item));
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/RoleOfGroups/GetByGroup
        [HttpGet("GetByGroup")]
        public ActionResult<IEnumerable<RoleOfGroup>> GetByGroup(Guid ID)
        {
            try
            {
                List<RoleOfGroupVM> result = new List<RoleOfGroupVM>();
                var data = _roleOfGroupService.GetByGroup(ID);
                if (data.Count() == 0) return NotFound("List empty");
                foreach (var item in data)
                {
                    result.Add(_mapper.Map<RoleOfGroupVM>(item));
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/RoleOfGroups/GetByRole
        [HttpGet("GetByRole")]
        public ActionResult<IEnumerable<RoleOfGroup>> GetByRole(Guid ID)
        {
            try
            {
                List<RoleOfGroupVM> result = new List<RoleOfGroupVM>();
                var data = _roleOfGroupService.GetByRole(ID);
                if (data.Count() == 0) return NotFound("List empty");
                foreach (var item in data)
                {
                    result.Add(_mapper.Map<RoleOfGroupVM>(item));
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/RoleOfGroups/5
        [HttpGet("GetByID")]
        public ActionResult<RoleOfGroup> GetRoleOfGroup(Guid ID)
        {
            try
            {
                var rs = _roleOfGroupService.GetByID(ID);
                if (rs == null) return NotFound("ID not found");
                RoleOfGroupVM result = _mapper.Map<RoleOfGroupVM>(rs);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // PUT: api/RoleOfGroups
        [HttpPut]
        public IActionResult PutRoleOfGroups(RoleOfGroupUM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var checkExist = _roleOfGroupService.CheckExist(model.RoleID, model.GroupID);
                if (checkExist != null) return BadRequest("Existed!");

                var roleOfGroupInDb = _roleOfGroupService.GetByID(model.ID);
                if (roleOfGroupInDb == null) return BadRequest("ID not found!");
                _mapper.Map(model, roleOfGroupInDb);
                _roleOfGroupService.Save();
                return Ok("success");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/RoleOfGroups
        [HttpDelete]
        public ActionResult DeleteRoleOfGroup(Guid ID)
        {
            try
            {
                var roleOfGroupInDb = _roleOfGroupService.GetByID(ID);
                if (roleOfGroupInDb == null) return NotFound("ID not found!");
                roleOfGroupInDb.IsDeleted = true;
                _roleOfGroupService.Save();
                return Ok("success");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}