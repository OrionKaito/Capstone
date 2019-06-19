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
    public class GroupsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IGroupService _groupService;

        public GroupsController(IMapper mapper, IGroupService groupService)
        {
            _mapper = mapper;
            _groupService = groupService;
        }

        // POST: api/Groups
        [HttpPost]
        public ActionResult PostGroup(GroupCM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var nameExist = _groupService.GetByName(model.Name);
                if (nameExist != null) return BadRequest("Group" + WebConstant.NameExisted);

                Group group = new Group();
                group = _mapper.Map<Group>(model);
                _groupService.Create(group);
                return StatusCode(201, group.ID);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/Groups
        [HttpGet]
        public ActionResult<IEnumerable<GroupVM>> GetGroups()
        {
            try
            {
                List<GroupVM> result = new List<GroupVM>();
                var data = _groupService.GetAll();
                foreach (var item in data)
                {
                    result.Add(_mapper.Map<GroupVM>(item));
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/Groups/GetByUserID
        [HttpGet("GetByUserID")]
        public ActionResult<IEnumerable<string>> GetByUserID(string ID)
        {
            try
            {
                List<string> result = new List<string>();
                var data = _groupService.GetByUserID(ID);
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

        // GET: api/Groups/5
        [HttpGet("GetByID")]
        public ActionResult<GroupVM> GetGroup(Guid ID)
        {
            try
            {
                var rs = _groupService.GetByID(ID);
                if (rs == null) return NotFound(WebConstant.NotFound);
                GroupVM result = _mapper.Map<GroupVM>(rs);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        // PUT: api/Groups/5
        [HttpPut]
        public IActionResult PutGroup(GroupUM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var nameExist = _groupService.GetByName(model.Name);
                if (nameExist != null) return BadRequest("Group" + WebConstant.NameExisted);

                var groupInDb = _groupService.GetByID(model.ID);
                if (groupInDb == null) return NotFound(WebConstant.NotFound);
                _mapper.Map(model, groupInDb);
                _groupService.Save();
                return Ok(WebConstant.Success);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/Groups/5
        [HttpDelete]
        public ActionResult DeleteGroup(Guid ID)
        {
            try
            {
                var groupInDb = _groupService.GetByID(ID);
                if (groupInDb == null) return NotFound(WebConstant.NotFound);
                groupInDb.IsDeleted = true;
                _groupService.Save();
                return Ok(WebConstant.Success);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
