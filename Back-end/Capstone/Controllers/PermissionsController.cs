using AutoMapper;
using Capstone.Helper;
using Capstone.Model;
using Capstone.Service;
using Capstone.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Capstone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPermissionService _permissionService;

        public PermissionsController(IMapper mapper, IPermissionService permissionService)
        {
            _mapper = mapper;
            _permissionService = permissionService;
        }

        // POST: api/Permissions
        [HttpPost]
        public ActionResult PostPermission(PermissionCM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var nameExist = _permissionService.GetByName(model.Name);
                if (nameExist != null) return BadRequest("Permission" + WebConstant.NameExisted);

                Permission permission = new Permission();
                permission = _mapper.Map<Permission>(model);
                _permissionService.Create(permission);
                return StatusCode(201, permission.ID);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/Permissions
        [HttpGet]
        public ActionResult<IEnumerable<PermissionVM>> GetPermissions()
        {
            try
            {
                List<PermissionVM> result = new List<PermissionVM>();
                var data = _permissionService.GetIsDelete();
                foreach (var item in data)
                {
                    result.Add(_mapper.Map<PermissionVM>(item));
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/Permissions
        [HttpGet("GetAllPermission")]
        public ActionResult<IEnumerable<PermissionVM>> GetAllPermissions()
        {
            try
            {
                List<PermissionVM> result = new List<PermissionVM>();
                var data = _permissionService.GetAll();
                foreach (var item in data)
                {
                    result.Add(_mapper.Map<PermissionVM>(item));
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/Permissions
        [HttpGet("GetByUserID")]
        public ActionResult<IEnumerable<PermissionVM>> GetByUserID(string ID)
        {
            try
            {
                List<PermissionVM> result = new List<PermissionVM>();
                var data = _permissionService.GetByUserID(ID);
                if (data == null) return BadRequest(WebConstant.NotFound);
                foreach (var item in data)
                {
                    result.Add(_mapper.Map<PermissionVM>(item));
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/Permissions/GetByID
        [HttpGet("GetByID")]
        public ActionResult<PermissionVM> GetPermission(Guid ID)
        {
            try
            {
                var rs = _permissionService.GetByID(ID);
                if (rs == null) return BadRequest(WebConstant.NotFound);
                PermissionVM result = _mapper.Map<PermissionVM>(rs);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // PUT: api/Permissions
        [HttpPut]
        public IActionResult PutPermission(PermissionUM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var nameExist = _permissionService.GetByName(model.Name);
                if (nameExist != null) return BadRequest("Permission" + WebConstant.NameExisted);

                var permissionInDb = _permissionService.GetByID(model.ID);
                if (permissionInDb == null) return BadRequest(WebConstant.NotFound);
                _mapper.Map(model, permissionInDb);
                _permissionService.Save();
                return Ok(WebConstant.Success);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/Permissions
        [HttpDelete]
        public ActionResult DeleteRole(Guid ID)
        {
            try
            {
                var permissionInDb = _permissionService.GetByID(ID);
                if (permissionInDb == null) return BadRequest(WebConstant.NotFound);
                permissionInDb.IsDeleted = true;
                _permissionService.Save();
                return Ok(WebConstant.Success);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}