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
        private readonly IPermissionService _permissionService;
        private readonly IGroupService _groupService;

        public PermissionOfGroupsController(IMapper mapper, IPermissionOfGroupService permissionOfGroupService, IPermissionService permissionService
            , IGroupService groupService)
        {
            _mapper = mapper;
            _permissionOfGroupService = permissionOfGroupService;
            _permissionService = permissionService;
            _groupService = groupService;
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
                    result.Add(new PermissionOfGroupVM {
                        ID = item.ID,
                        PermissionID = item.PermissionID,
                        PermissionName = _permissionService.GetByID(item.PermissionID).Name,
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

        // GET: api/PermissionOfGroups
        [HttpGet("GetPermissionByGroup")]
        public ActionResult<IEnumerable<PermissionOfGroupViewModel>> GetPermissionByGroup()
        {
            try
            {
                List<PermissionOfGroupViewModel> result = new List<PermissionOfGroupViewModel>();
                var groupList = _groupService.GetAll();
                foreach (var group in groupList)
                {
                    result.Add(new PermissionOfGroupViewModel {
                        GroupID = group.ID,
                        GroupName = group.Name,
                        Permissions = _permissionOfGroupService.GetByGroup(group.ID)
                                        .Select(p => new PermissionsViewModel
                                        {
                                            PermissionOfGroupID = p.ID,
                                            PermissionID = p.PermissionID,
                                            PermissionName = _permissionService.GetByID(p.PermissionID).Name
                                        }),
                    });
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
                    result.Add(new PermissionOfGroupVM
                    {
                        ID = item.ID,
                        PermissionID = item.PermissionID,
                        PermissionName = _permissionService.GetByID(item.PermissionID).Name,
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
                    result.Add(new PermissionOfGroupVM
                    {
                        ID = item.ID,
                        PermissionID = item.PermissionID,
                        PermissionName = _permissionService.GetByID(item.PermissionID).Name,
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

        // GET: api/PermissionOfGroups
        [HttpGet("GetByID")]
        public ActionResult<PermissionOfGroupVM> GetPermissionOfGroup(Guid ID)
        {
            try
            {
                var permissionOfGroup = _permissionOfGroupService.GetByID(ID);
                if (permissionOfGroup == null) return NotFound(WebConstant.NotFound);
                PermissionOfGroupVM result = new PermissionOfGroupVM
                {
                    ID = permissionOfGroup.ID,
                    PermissionID = permissionOfGroup.PermissionID,
                    PermissionName = _permissionService.GetByID(permissionOfGroup.PermissionID).Name,
                    GroupID = permissionOfGroup.GroupID,
                    GroupName = _groupService.GetByID(permissionOfGroup.GroupID).Name
                };
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