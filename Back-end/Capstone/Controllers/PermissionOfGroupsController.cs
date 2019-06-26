using AutoMapper;
using Capstone.Helper;
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
    public class PermissionOfGroupsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPermissionOfGroupService _permissionOfGroupService;

        public PermissionOfGroupsController(IMapper mapper, IPermissionOfGroupService permissionOfGroupService)
        {
            _mapper = mapper;
            _permissionOfGroupService = permissionOfGroupService;
        }

        //POST: api/PermissionOfGroups
        [HttpPost]
        public ActionResult PostPermissionOfGroup(PermissionOfGroupCM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var checkExist = _permissionOfGroupService.CheckExist(model.PermissionID, model.GroupID);
                if (checkExist != null) return BadRequest("Existed!");

                PermissionOfGroup permissionOfGroup = new PermissionOfGroup();
                permissionOfGroup = _mapper.Map<PermissionOfGroup>(model);
                _permissionOfGroupService.Create(permissionOfGroup);
                return StatusCode(201, permissionOfGroup.ID);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/PermissionOfGroups
        [HttpGet]
        public ActionResult<IEnumerable<PermissionOfGroupVM>> GetPermissionOfGroups()
        {
            try
            {
                List<PermissionOfGroupVM> result = new List<PermissionOfGroupVM>();
                var data = _permissionOfGroupService.GetAll();
                foreach (var item in data)
                {
                    result.Add(_mapper.Map<PermissionOfGroupVM>(item));
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/PermissionOfGroups/GetByPermission
        [HttpGet("GetByPermission")]
        public ActionResult<IEnumerable<PermissionOfGroupVM>> GetByPermission(Guid ID)
        {
            try
            {
                List<PermissionOfGroupVM> result = new List<PermissionOfGroupVM>();
                var data = _permissionOfGroupService.GetByPermission(ID);
                if (data.Count() == 0) return NotFound(WebConstant.EmptyList);
                foreach (var item in data)
                {
                    result.Add(_mapper.Map<PermissionOfGroupVM>(item));
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/PermissionOfGroups/GetByGroup
        [HttpGet("GetByGroup")]
        public ActionResult<IEnumerable<PermissionOfGroupVM>> GetByGroup(Guid ID)
        {
            try
            {
                List<PermissionOfGroupVM> result = new List<PermissionOfGroupVM>();
                var data = _permissionOfGroupService.GetByGroup(ID);
                if (data.Count() == 0) return NotFound(WebConstant.EmptyList);
                foreach (var item in data)
                {
                    result.Add(_mapper.Map<PermissionOfGroupVM>(item));
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/PermissionOfGroups
        [HttpGet("GetByID")]
        public ActionResult<PermissionOfGroupVM> GetPermissionOfGroup(Guid ID)
        {
            try
            {
                var rs = _permissionOfGroupService.GetByID(ID);
                if (rs == null) return NotFound(WebConstant.NotFound);
                PermissionOfGroupVM result = _mapper.Map<PermissionOfGroupVM>(rs);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // PUT: api/PermissionOfGroups
        [HttpPut]
        public IActionResult PutPermissionOfGroup(PermissionOfGroupUM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var permissionOfGroupInDb = _permissionOfGroupService.GetByID(model.ID);
                if (permissionOfGroupInDb == null) return BadRequest(WebConstant.NotFound);
                _mapper.Map(model, permissionOfGroupInDb);
                _permissionOfGroupService.Save();
                return Ok(WebConstant.Success);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/PermissionOfGroups
        [HttpDelete]
        public ActionResult DeletePermissionOfGroup(Guid ID)
        {
            try
            {
                var permissionOfGroupInDb = _permissionOfGroupService.GetByID(ID);
                if (permissionOfGroupInDb == null) return NotFound(WebConstant.NotFound);
                permissionOfGroupInDb.IsDeleted = true;
                _permissionOfGroupService.Save();
                return Ok(WebConstant.Success);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}