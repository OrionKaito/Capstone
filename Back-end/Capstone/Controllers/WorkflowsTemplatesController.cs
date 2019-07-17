using AutoMapper;
using Capstone.Helper;
using Capstone.Model;
using Capstone.Service;
using Capstone.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Capstone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkflowsTemplatesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IWorkFlowTemplateActionService _workFlowTemplateActionService;
        private readonly IWorkFlowTemplateActionConnectionService _workFlowTemplateActionConnectionService;
        private readonly IConnectionTypeService _connectionTypeService;
        private readonly IWorkFlowTemplateService _workFlowService;
        private readonly IPermissionService _permissionService;
        private readonly IWorkFlowTemplateActionService _workFlowTemplateActionService;
        private readonly IWorkFlowTemplateActionConnectionService _workFlowTemplateActionConnectionService;

        public WorkflowsTemplatesController(IMapper mapper
            , IWorkFlowTemplateService workFlowService
            , IPermissionService permissionService
            , IWorkFlowTemplateActionService workFlowTemplateActionService
            , IWorkFlowTemplateActionConnectionService workFlowTemplateActionConnectionService)
        {
            _mapper = mapper;
            _workFlowService = workFlowService;
            _permissionService = permissionService;
            _workFlowTemplateActionService = workFlowTemplateActionService;
            _workFlowTemplateActionConnectionService = workFlowTemplateActionConnectionService;
        }

        // GET: api/Workflows
        [HttpGet]
        public ActionResult<IEnumerable<WorkFlowTemplateVM>> GetWorkflowsTemplates()
        {
            try
            {
                List<WorkFlowTemplateVM> result = new List<WorkFlowTemplateVM>();
                var workFlow = new WorkFlowTemplateVM();
                var data = _workFlowService.GetAll();
                foreach (var item in data)
                {
                    result.Add(_mapper.Map<WorkFlowTemplateVM>(item));
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/Workflows/5
        [HttpGet("GetByID")]
        public ActionResult<WorkFlowTemplateVM> GetWorkflowTemplate(Guid ID)
        {
            try
            {
                var data = _workFlowService.GetByID(ID);
                if (data == null) return BadRequest(WebConstant.NotFound);
                WorkFlowTemplateVM result = _mapper.Map<WorkFlowTemplateVM>(data);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("GetWorkflowToUse")]
        public ActionResult<IEnumerable<WorkFlowTemplateVM>> GetWorkflowToUse()
        {
            try
            {
                var userID = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
                var permissionsOfUser = _permissionService.GetByUserID(userID);

                List<WorkFlowTemplateVM> workFlowTemplates = new List<WorkFlowTemplateVM>();

                foreach (var item in permissionsOfUser)
                {
                    var workflows = _workFlowService.GetByPermissionToUse(item.ID);

                    foreach (var workflow in workflows)
                    {
                        workFlowTemplates.Add(_mapper.Map<WorkFlowTemplateVM>(workflow));
                    }
                }

                return Ok(workFlowTemplates);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("GetWorkflowToEdit")]
        public ActionResult<IEnumerable<WorkFlowTemplateVM>> GetWorkflowToEdit()
        {
            try
            {
                var userID = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
                var permissionsOfUser = _permissionService.GetByUserID(userID);

                List<WorkFlowTemplateVM> workFlowTemplates = new List<WorkFlowTemplateVM>();

                foreach (var item in permissionsOfUser)
                {
                    var workflows = _workFlowService.GetByPermissionToUse(item.ID);

                    foreach (var workflow in workflows)
                    {
                        workFlowTemplates.Add(_mapper.Map<WorkFlowTemplateVM>(workflow));
                    }
                }

                return Ok(workFlowTemplates);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public ActionResult PutWorkflowTemplate(WorkFlowTemplateUM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var workFlowInDb = _workFlowService.GetByID(model.ID);
                if (workFlowInDb == null) return BadRequest(WebConstant.NotFound);

                _mapper.Map<WorkFlowTemplate>(model);
                _workFlowService.Save();
                return Ok(WebConstant.Success);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPut("drafJson")]
        public ActionResult PutDrafJson(WorkFlowTemplateJSON model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var workFlowInDb = _workFlowService.GetByID(model.ID);
                if (workFlowInDb == null) return BadRequest(WebConstant.NotFound);

                var userID = HttpContext.User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value;
                if (workFlowInDb.OwnerID != userID) return BadRequest(WebConstant.AccessDined);

                workFlowInDb.Data = model.Data;
                _workFlowService.Save();
                return Ok(WebConstant.Success);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPut("saveJson")]
        public ActionResult PutSaveJson(FullWorkFlowTem model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var saveWFTAId = new Guid[model.action.Length];
                int saveIndex = 0;
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
                    thisModel.ActionTypeID = new Guid("bd363bff-11f5-49eb-67ac-08d709b08fff");
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
                    for (int i = 0; i < model.action.Length; i++)
                    {
                        if (arrowAction.idDiv[0].Equals(model.action[i].id))
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

        // POST: api/Workflows
        [HttpPost]
        public ActionResult PostWorkflowTemplate(WorkFlowTemplateCM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                WorkFlowTemplate workFlow = new WorkFlowTemplate();
                if (_workFlowService.GetByName(model.Name) != null) return BadRequest("Workflow" + WebConstant.NameExisted);

                var userID = HttpContext.User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value;

                workFlow = _mapper.Map<WorkFlowTemplate>(model);
                workFlow.OwnerID = userID;
                _workFlowService.Create(workFlow);

                return StatusCode(201, workFlow.ID);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/Workflows/5
        [HttpDelete("{id}")]
        public ActionResult DeleteWorkflowTemplate(Guid ID)
        {
            try
            {
                var workFlowInDb = _workFlowService.GetByID(ID);
                if (workFlowInDb == null) return BadRequest(WebConstant.NotFound);

                workFlowInDb.IsDeleted = true;
                _workFlowService.Save();
                return Ok(WebConstant.Success);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("ToggleEnable")]
        public ActionResult ToggleEnable(Guid ID)
        {
            var workFlowInDb = _workFlowService.GetByID(ID);
            if (workFlowInDb == null) return BadRequest(WebConstant.NotFound);

            try
            {
                if (workFlowInDb.IsEnabled == true)
                {
                    workFlowInDb.IsEnabled = false;
                }
                else
                {
                    workFlowInDb.IsEnabled = true;
                }
                _workFlowService.Save();
                return Ok(WebConstant.Success);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("SaveWorkflow")]
        public ActionResult SaveWorkflow(SaveWowkFlowTemplateUM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                _workFlowService.BeginTransaction();
                var workFlowInDB = _workFlowService.GetByID(model.WorkFlowTemplateID);
                workFlowInDB.Data = model.Data;

                foreach (var action in model.Actions)
                {
                    var workFlowTemplateAction = _mapper.Map<WorkFlowTemplateAction>(action);
                    workFlowTemplateAction.WorkFlowTemplateID = model.WorkFlowTemplateID;
                    _workFlowTemplateActionService.Create(workFlowTemplateAction);
                }

                foreach (var connection in model.Connections)
                {
                    var workflowConnection = _mapper.Map<WorkFlowTemplateActionConnection>(connection);
                    _workFlowTemplateActionConnectionService.Create(workflowConnection);
                }

                _workFlowService.CommitTransaction();

                return Ok(WebConstant.Success);
            }
            catch (Exception e)
            {
                _workFlowService.RollBack();
                return BadRequest(e.Message);
            }
        }

        [HttpPut("SaveCraft")]
        public ActionResult SaveCraft(SaveCraftTemplateUM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var workFlowInDb = _workFlowService.GetByID(model.ID);
                if (workFlowInDb == null) return BadRequest(WebConstant.NotFound);

                workFlowInDb.Data = model.Data;
                _workFlowService.Save();
                return Ok(WebConstant.Success);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
