using AutoMapper;
using Capstone.Model;
using Capstone.Service;
using Capstone.Service.Helper;
using Capstone.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Capstone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRolesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserRoleService _userRoleService;
        private readonly UserManager<User> _userManager;
        private readonly IRoleService _roleService;

        public UserRolesController(IMapper mapper, IUserRoleService UserRoleService, UserManager<User> userManager, IRoleService roleService)
        {
            _mapper = mapper;
            _userRoleService = UserRoleService;
            _userManager = userManager;
            _roleService = roleService;
        }

        // POST: api/UserRoles
        [HttpPost]
        public ActionResult PostUserRole(UserRoleCM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var checkExist = _userRoleService.CheckExist(model.UserID, model.RoleID);
                if (checkExist != null) return BadRequest("Existed!");

                var checkRole = _userRoleService.GetByUserID(model.UserID);
                if (checkRole != null) return BadRequest("This user have a role!");

                UserRole userRole = new UserRole();
                userRole = _mapper.Map<UserRole>(model);
                _userRoleService.Create(userRole);
                return StatusCode(201, userRole.ID);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/UserRoles
        [HttpGet("GetByUserID")]
        public ActionResult<UserRoleVM> GetByUserID(string ID)
        {
            try
            {
                UserRoleVM result = new UserRoleVM();
                var data = _userRoleService.GetByUserID(ID);
                if (data == null) return NotFound("This user do not have role!");
                result = new UserRoleVM
                {
                    ID = data.ID,
                    UserID = data.UserID,
                    FullName = data.User.FullName,
                    RoleID = data.RoleID,
                    RoleName = data.Role.Name
                };
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/UserRoles/5
        [HttpGet("GetByUserRoleID")]
        public ActionResult<UserRoleVM> GetByUserRoleID(Guid ID)
        {
            try
            {
                var userRole = _userRoleService.GetByID(ID);
                if (userRole == null) return NotFound(WebConstant.NotFound);
                UserRoleVM result = new UserRoleVM
                {
                    ID = userRole.ID,
                    UserID = userRole.UserID,
                    FullName = _userManager.FindByIdAsync(userRole.UserID).Result.FullName,
                    RoleID = userRole.RoleID,
                    RoleName = _roleService.GetByID(userRole.RoleID).Name
                };
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // PUT: api/UserRoles
        [HttpPut]
        public IActionResult PutUserRole(UserRoleUM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var checkExist = _userRoleService.CheckExist(model.UserID, model.RoleID);
                if (checkExist != null) return BadRequest("Existed!");

                var userRoleInDb = _userRoleService.GetByID(model.ID);
                if (userRoleInDb == null) return BadRequest(WebConstant.NotFound);
                _mapper.Map(model, userRoleInDb);
                _userRoleService.Save();
                return Ok(WebConstant.Success);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/UserRoles
        [HttpDelete]
        public ActionResult DeleteUserRole(Guid ID)
        {
            try
            {
                var userRoleInDb = _userRoleService.GetByID(ID);
                if (userRoleInDb == null) return NotFound(WebConstant.NotFound);
                userRoleInDb.IsDeleted = true;
                _userRoleService.Save();
                return Ok(WebConstant.Success);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}