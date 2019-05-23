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
    public class GroupsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IGroupService _groupService;

        public GroupsController(IMapper mapper, IGroupService groupService)
        {
            _mapper = mapper;
            _groupService = groupService;
        }

        // GET: api/Groups
        [HttpGet]
        public ActionResult<IEnumerable<Group>> GetGroups()
        {
            List<GroupVM> result = new List<GroupVM>();
            var group = new GroupVM();
            var data = _groupService.GetAll();
            foreach (var item in data)
            {
                result.Add(_mapper.Map<GroupVM>(item));
            }
            return Ok(result);
        }

        // GET: api/Groups/5
        [HttpGet("{id}")]
        public ActionResult<Group> GetGroup(Guid ID)
        {
            var rs = _groupService.GetByID(ID);
            if (rs == null) return NotFound("ID not found!");
            GroupVM result = _mapper.Map<GroupVM>(rs);
            return Ok(result);
        }

        // POST: api/Groups
        [HttpPost]
        public ActionResult<Group> PostGroup(GroupCM model)
        {
            Group group = new Group();
            try
            {
                group = _mapper.Map<Group>(model);
                _groupService.Create(group);
                _groupService.Save();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return StatusCode(201, group.ID);
        }

        // PUT: api/Groups/5
        [HttpPut]
        public IActionResult PutGroup(GroupUM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var groupInDb = _groupService.GetByID(model.ID);
                if (groupInDb == null) return NotFound("ID not found!");
                //_mapper.Map(model, groupInDb);
                _groupService.Save();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok("success");
        }

        // DELETE: api/Groups/5
        [HttpDelete]
        public IActionResult DeleteGroup(Guid ID)
        {
            try
            {
                var groupInDb = _groupService.GetByID(ID);
                if (groupInDb == null) return NotFound("ID not found!");
                groupInDb.IsDelete = true;
                _groupService.Save();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok("success");
        }
    }
}
