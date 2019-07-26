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
        private readonly IRoleService _roleService;

        public WorkflowsTemplatesController(IMapper mapper
            , IWorkFlowTemplateService workFlowService
            , IPermissionService permissionService
            , IWorkFlowTemplateActionService workFlowTemplateActionService
            , IWorkFlowTemplateActionConnectionService workFlowTemplateActionConnectionService
            , IRoleService roleService)
        {
            _mapper = mapper;
            _workFlowService = workFlowService;
            _permissionService = permissionService;
            _workFlowTemplateActionService = workFlowTemplateActionService;
            _workFlowTemplateActionConnectionService = workFlowTemplateActionConnectionService;
            _roleService = roleService;
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
                var role = _roleService.GetByUserID(userID);

                List<WorkFlowTemplateVM> workFlowTemplates = new List<WorkFlowTemplateVM>();
                if (role.Name.Equals(WebConstant.Staff))
                {
                    var workflows = _workFlowService.GetAll();

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
                bool checkUpdate = false;
                var workFlowInDB = _workFlowService.GetByID(model.WorkFlowTemplateID);

                var actionsInDb = _workFlowTemplateActionService.GetByWorkFlowID(model.WorkFlowTemplateID);
                if (actionsInDb.Any()) //Kiểm tra wokrflow có action nào không
                {
                    checkUpdate = true;
                    //Disable workflow cũ
                    var workFlowInDb = _workFlowService.GetByID(model.WorkFlowTemplateID);
                    workFlowInDb.IsDeleted = true;
                    workFlowInDb.IsEnabled = false;
                    _workFlowService.Save();

                    //Tạo workflow mới
                    WorkFlowTemplate workFlow = new WorkFlowTemplate
                    {
                        Data = model.Data,
                        Description = workFlowInDB.Description,
                        Name = workFlowInDB.Name,
                        OwnerID = workFlowInDB.OwnerID,
                        PermissionToUseID = workFlowInDB.PermissionToUseID,
                    };
                    _workFlowService.Create(workFlow);

                    model.WorkFlowTemplateID = workFlow.ID; //cập nhật id là của workflow mới
                }
                else
                {
                    workFlowInDB.Data = model.Data;
                }

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

                if (checkUpdate)
                {
                    return StatusCode(201, "Update " + WebConstant.Success);
                }
                else
                {
                    return Ok(WebConstant.Success);

                }
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
