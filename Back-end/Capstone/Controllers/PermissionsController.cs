using AutoMapper;
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
        public ActionResult<Permission> PostPermission(PermissionCM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                Permission permission = new Permission();
                permission = _mapper.Map<Permission>(model);
                _permissionService.Create(permission);
                _permissionService.Save();
                return StatusCode(201, "Permission is created!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/Permissions
        [HttpGet]
        public ActionResult<IEnumerable<Permission>> GetPermissions()
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
        public ActionResult<IEnumerable<string>> GetByUserID(string ID)
        {
            try
            {
                List<string> result = new List<string>();
                var data = _permissionService.GetByUserID(ID);
                foreach (var item in data)
                {
                    result.Add(item);
                }
                if (result.Count == 0) return BadRequest("ID not found!");
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/Permissions/GetByID
        [HttpGet("GetByID")]
        public ActionResult<Permission> GetPermission(Guid ID)
        {
            try
            {
                var rs = _permissionService.GetByID(ID);
                if (rs == null) return BadRequest("ID not found!");
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
                var permissionInDb = _permissionService.GetByID(model.ID);
                if (permissionInDb == null) return BadRequest("ID not found!");
                _mapper.Map(model, permissionInDb);
                _permissionService.Save();
                return Ok("success");
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
                if (permissionInDb == null) return BadRequest("ID not found!");
                permissionInDb.IsDelete = true;
                _permissionService.Save();
                return Ok("success");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}