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
        [HttpPost("fullWorkflow")]
        public ActionResult PostActionType(FullWorkFlowTem model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var saveWFTAId = new Guid[model.action.Length] ;
                int saveIndex=0;
                foreach (var actionItem in model.action)
                {
                    WorkFlowTemplateActionCM thisModel = new WorkFlowTemplateActionCM();
                    thisModel.WorkFlowTemplateID = model.WorkFlowID;
                    thisModel.Name = actionItem.name;
                    thisModel.Description = actionItem.description;
                    thisModel.IsApprovedByLineManager = actionItem.isApprovedByLineManager;
                    thisModel.IsStart = actionItem.start;
                    thisModel.IsEnd = actionItem.end;
                    thisModel.PermissionToUseID = actionItem.permissionToUseID;
                    thisModel.ActionTypeID = new Guid("7e4cb4e7-ca19-40d5-3568-08d6f8a554a6");
                    WorkFlowTemplateAction workFlowTemplateAction = new WorkFlowTemplateAction();
                    // trùng tên kệ cha nó chứ :)))
                    //if (_workFlowTemplateActionService.GetByName(thisModel.Name) != null) return BadRequest("WorkflowTemplateAction "
                    //  + WebConstant.NameExisted);
                    workFlowTemplateAction = _mapper.Map<WorkFlowTemplateAction>(thisModel);
                    _workFlowTemplateActionService.Create(workFlowTemplateAction);
                    
                    saveWFTAId[saveIndex] = workFlowTemplateAction.ID;
                    saveIndex++;


                }

                foreach (var arrowAction in model.arrow)
                {
                    ConnectionTypeCM thisModel = new ConnectionTypeCM();
                    thisModel.Name = arrowAction.name;

                    ConnectionType connectionType = new ConnectionType();
                    connectionType = _mapper.Map<ConnectionType>(thisModel);
                    _connectionTypeService.Create(connectionType);

                    WorkFlowTemplateActionConnectionCM thisConModel = new WorkFlowTemplateActionConnectionCM();
                    thisConModel.ConnectionTypeID = connectionType.ID;
                    for(int i=0;i< model.action.Length; i++)
                    {
                        if(arrowAction.idDiv[0].Equals(model.action[i].id))
                        {
                            thisConModel.FromWorkFlowTemplateActionID = saveWFTAId[i];
                        }
                        if (arrowAction.idDiv[1].Equals(model.action[i].id))
                        {
                            thisConModel.ToWorkFlowTemplateActionID = saveWFTAId[i];
                        }
                    }
                    WorkFlowTemplateActionConnection workFlowTemplateActionConnection = new WorkFlowTemplateActionConnection();
                    workFlowTemplateActionConnection = _mapper.Map<WorkFlowTemplateActionConnection>(thisConModel);
                    _workFlowTemplateActionConnectionService.Create(workFlowTemplateActionConnection);
                }
                return StatusCode(201);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
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