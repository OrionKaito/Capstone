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
    public class RolesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRoleService _roleService;

        public RolesController(IMapper mapper, IRoleService roleService)
        {
            _mapper = mapper;
            _roleService = roleService;
        }

        // POST: api/Roles
        [HttpPost]
        public ActionResult PostRole(RoleCM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var nameExist = _roleService.GetByName(model.Name);
                if (nameExist != null) return BadRequest("Role" + WebConstant.NameExisted);

                Role role = new Role();
                role = _mapper.Map<Role>(model);
                _roleService.Create(role);
                return StatusCode(201, role.ID);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/Roles
        [HttpGet]
        public ActionResult<IEnumerable<RoleVM>> GetRoles()
        {
            try
            {
                List<RoleVM> result = new List<RoleVM>();
                var data = _roleService.GetAll();
                foreach (var item in data)
                {
                    result.Add(_mapper.Map<RoleVM>(item));
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/Roles
        [HttpGet("GetByUserID")]
        public ActionResult<IEnumerable<string>> GetByUserID(string ID)
        {
            try
            {
                List<string> result = new List<string>();
                var data = _roleService.GetByUserID(ID);
                foreach (var item in data)
                {
                    result.Add(item);
                }

                if (result.Count == 0) return BadRequest(WebConstant.NotFound);

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/Roles/GetByID
        [HttpGet("GetByID")]
        public ActionResult<RoleVM> GetRole(Guid ID)
        {
            try
            {
                var rs = _roleService.GetByID(ID);
                if (rs == null) return BadRequest(WebConstant.NotFound);
                RoleVM result = _mapper.Map<RoleVM>(rs);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // PUT: api/Roles
        [HttpPut]
        public IActionResult PutRole(RoleUM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var nameExist = _roleService.GetByName(model.Name);
                if (nameExist != null) return BadRequest("Role" + WebConstant.NameExisted);

                var roleInDb = _roleService.GetByID(model.ID);
                if (roleInDb == null) return BadRequest(WebConstant.NotFound);

                _mapper.Map(model, roleInDb);
                _roleService.Save();
                return Ok(WebConstant.Success);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/Roles
        [HttpDelete]
        public ActionResult DeleteRole(Guid ID)
        {
            try
            {
                var roleInDb = _roleService.GetByID(ID);
                if (roleInDb == null) return BadRequest(WebConstant.NotFound);

                roleInDb.IsDeleted = true;
                _roleService.Save();
                return Ok(WebConstant.Success);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("ToggleRole")]
        public ActionResult ToggleRole(Guid ID)
        {
            try
            {
                var roleInDb = _roleService.GetByID(ID);
                if (roleInDb == null) return BadRequest(WebConstant.NotFound);

                if (roleInDb.IsDeleted == false)
                {
                    roleInDb.IsDeleted = true;
                }
                else
                {
                    roleInDb.IsDeleted = false;
                }
                _roleService.Save();
                return Ok(WebConstant.Success);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}