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
    public class ActionTypesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IActionTypeService _actionTypeService;

        public ActionTypesController(IMapper mapper, IActionTypeService actionTypeService)
        {
            _mapper = mapper;
            _actionTypeService = actionTypeService;
        }

        // GET: api/ActionTypes
        [HttpGet]
        public ActionResult<IEnumerable<ActionType>> GetActionTypes()
        {
            List<ActionTypeVM> result = new List<ActionTypeVM>();
            var actionType = new ActionTypeVM();
            var data = _actionTypeService.GetAll();
            foreach (var item in data)
            {
                result.Add(_mapper.Map<ActionTypeVM>(item));
            }
            return Ok(result);
        }

        // GET: api/ActionTypes/5
        [HttpGet("{id}")]
        public ActionResult<ActionType> GetActionType(Guid ID)
        {
            var rs = _actionTypeService.GetByID(ID);
            if (rs == null) return NotFound("ID not found!");
            ActionTypeVM result = _mapper.Map<ActionTypeVM>(rs);
            return Ok(result);
        }

        // POST: api/ActionTypes
        [HttpPost]
        public ActionResult<ActionType> PostActionType(ActionTypeCM model)
        {
            ActionType actionType = new ActionType();
            try
            {
                actionType = _mapper.Map<ActionType>(model);
                _actionTypeService.Create(actionType);
                _actionTypeService.Save();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return StatusCode(201, actionType.ActionTypeID);
        }

        // PUT: api/ActionTypes/5
        [HttpPut]
        public IActionResult PutActionType(ActionTypeUM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var actionTypeInDb = _actionTypeService.GetByID(model.ActionTypeID);
                if (actionTypeInDb == null) return NotFound("ID not found!");
                //_mapper.Map(model, actionTypeInDb);
                _actionTypeService.Save();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok("success");
        }

        // DELETE: api/ActionTypes/5
        [HttpDelete]
        public IActionResult DeleteActionType(Guid ID)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var actionTypeInDb = _actionTypeService.GetByID(ID);
                if (actionTypeInDb == null) return NotFound("ID not found!");
                actionTypeInDb.IsDelete = true;
                _actionTypeService.Save();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok("success");
        }
    }
}