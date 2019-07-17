using AutoMapper;
using Capstone.Helper;
using Capstone.Model;
using Capstone.Service;
using Capstone.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Capstone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActionTypesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IActionTypeService _actionTypeService;
        private readonly IWorkFlowTemplateActionService _workFlowTemplateActionService;
        private readonly IWorkFlowTemplateActionConnectionService _workFlowTemplateActionConnectionService;
        private readonly IConnectionTypeService _connectionTypeService;

        public ActionTypesController(
            IMapper mapper, 
            IActionTypeService actionTypeService,
            IConnectionTypeService connectionTypeService,
            IWorkFlowTemplateActionService workFlowTemplateActionService,
             IWorkFlowTemplateActionConnectionService workFlowTemplateActionConnectionService)
        {
            _mapper = mapper;
            _actionTypeService = actionTypeService;
            _workFlowTemplateActionService = workFlowTemplateActionService;
            _connectionTypeService = connectionTypeService;
            _workFlowTemplateActionConnectionService = workFlowTemplateActionConnectionService;
        }
        
        // POST: api/ActionTypes
        [HttpPost]
        public ActionResult PostActionType(ActionTypeCM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var nameExist = _actionTypeService.GetByName(model.Name);
                if (nameExist != null) return BadRequest("ActionType" + WebConstant.NameExisted);

                ActionType actionType = new ActionType();
                actionType = _mapper.Map<ActionType>(model);
                _actionTypeService.Create(actionType);
                return StatusCode(201, actionType.ID);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/ActionTypes
        [HttpGet]
        public ActionResult<IEnumerable<ActionTypeVM>> GetActionTypes()
        {
            try
            {
                List<ActionTypeVM> result = new List<ActionTypeVM>();
                var data = _actionTypeService.GetAll();
                foreach (var item in data)
                {
                    result.Add(_mapper.Map<ActionTypeVM>(item));
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/ActionTypes/5
        [HttpGet("GetByID")]
        public ActionResult<ActionTypeVM> GetActionType(Guid ID)
        {
            try
            {
                var rs = _actionTypeService.GetByID(ID);
                if (rs == null) return NotFound(WebConstant.NotFound);
                ActionTypeVM result = _mapper.Map<ActionTypeVM>(rs);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // PUT: api/ActionTypes/5
        [HttpPut]
        public IActionResult PutActionType(ActionTypeUM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var actionTypeInDb = _actionTypeService.GetByID(model.ID);
                if (actionTypeInDb == null) return NotFound(WebConstant.NotFound);

                var nameExist = _actionTypeService.GetByName(model.Name);
                if (nameExist != null) return BadRequest("ActionType" + WebConstant.NameExisted);

                _mapper.Map(model, actionTypeInDb);
                _actionTypeService.Save();
                return Ok(WebConstant.Success);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/ActionTypes/5
        [HttpDelete]
        public ActionResult DeleteActionType(Guid ID)
        {
            try
            {
                var actionTypeInDb = _actionTypeService.GetByID(ID);
                if (actionTypeInDb == null) return NotFound(WebConstant.NotFound);
                actionTypeInDb.IsDeleted = true;
                _actionTypeService.Save();
                return Ok(WebConstant.Success);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}