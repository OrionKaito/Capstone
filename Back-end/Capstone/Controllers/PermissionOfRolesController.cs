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
    public class PermissionOfRolesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPermissionOfRoleService _permissionOfRoleService;

        public PermissionOfRolesController(IMapper mapper, IPermissionOfRoleService permissionOfRoleService)
        {
            _mapper = mapper;
            _permissionOfRoleService = permissionOfRoleService;
        }

        //POST: api/PermissionOfRoles
        [HttpPost]
        public ActionResult<PermissionOfRole> PostPermissionOfRole(PermissionOfRoleCM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var checkExist = _permissionOfRoleService.CheckExist(model.PermissionID, model.RoleID);
                if (checkExist != null) return BadRequest("Existed!");

                PermissionOfRole permissionOfRole = new PermissionOfRole();
                permissionOfRole = _mapper.Map<PermissionOfRole>(model);
                _permissionOfRoleService.Create(permissionOfRole);
                return StatusCode(201, permissionOfRole.ID);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/PermissionOfRoles
        [HttpGet]
        public ActionResult<IEnumerable<PermissionOfRole>> GetPermissionOfRoles()
        {
            try
            {
                List<PermissionOfRoleVM> result = new List<PermissionOfRoleVM>();
                var data = _permissionOfRoleService.GetAll();
                foreach (var item in data)
                {
                    result.Add(_mapper.Map<PermissionOfRoleVM>(item));
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/PermissionOfRoles/GetByPermission
        [HttpGet("GetByPermission")]
        public ActionResult<IEnumerable<PermissionOfRole>> GetByPermission(Guid ID)
        {
            try
            {
                List<PermissionOfRoleVM> result = new List<PermissionOfRoleVM>();
                var data = _permissionOfRoleService.GetByPermission(ID);
                if (data.Count() == 0) return NotFound("List empty");
                foreach (var item in data)
                {
                    result.Add(_mapper.Map<PermissionOfRoleVM>(item));
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/PermissionOfRoles/GetByRole
        [HttpGet("GetByRole")]
        public ActionResult<IEnumerable<PermissionOfRole>> GetByRole(Guid ID)
        {
            try
            {
                List<PermissionOfRoleVM> result = new List<PermissionOfRoleVM>();
                var data = _permissionOfRoleService.GetByRole(ID);
                if (data.Count() == 0) return NotFound("List empty");
                foreach (var item in data)
                {
                    result.Add(_mapper.Map<PermissionOfRoleVM>(item));
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/PermissionOfRoles
        [HttpGet("GetByID")]
        public ActionResult<RoleOfGroup> GetPermissionOfRole(Guid ID)
        {
            try
            {
                var rs = _permissionOfRoleService.GetByID(ID);
                if (rs == null) return NotFound("ID not found");
                PermissionOfRoleVM result = _mapper.Map<PermissionOfRoleVM>(rs);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // PUT: api/PermissionOfRoles
        [HttpPut]
        public IActionResult PutPermissionOfRole(PermissionOfRoleUM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var permissionOfRoleInDb = _permissionOfRoleService.GetByID(model.ID);
                if (permissionOfRoleInDb == null) return BadRequest("ID not found!");
                _mapper.Map(model, permissionOfRoleInDb);
                _permissionOfRoleService.Save();
                return Ok("success");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/PermissionOfRoles
        [HttpDelete]
        public ActionResult DeletePermissionOfRole(Guid ID)
        {
            try
            {
                var permissionOfRoleInDb = _permissionOfRoleService.GetByID(ID);
                if (permissionOfRoleInDb == null) return NotFound("ID not found!");
                permissionOfRoleInDb.IsDeleted = true;
                _permissionOfRoleService.Save();
                return Ok("success");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}