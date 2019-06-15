﻿using AutoMapper;
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
    public class RequestActionsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRequestActionService _requestActionService;

        public RequestActionsController(IMapper mapper, IRequestActionService requestActionService)
        {
            _mapper = mapper;
            _requestActionService = requestActionService;
        }

        // POST: api/RequestActions
        [HttpPost]
        public ActionResult<RequestAction> PostRequestAction(RequestActionCM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                RequestAction requestAction = new RequestAction();
                requestAction = _mapper.Map<RequestAction>(model);
                requestAction.CreateDate = DateTime.Now;

                _requestActionService.Create(requestAction);
                return StatusCode(201, requestAction.ID);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/RequestActions
        [HttpGet]
        public ActionResult<IEnumerable<RequestAction>> GetRequestActions()
        {
            try
            {
                List<RequestActionVM> result = new List<RequestActionVM>();
                var data = _requestActionService.GetAll();
                foreach (var item in data)
                {
                    result.Add(_mapper.Map<RequestActionVM>(item));
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/RequestActions/GetByID
        [HttpGet("GetByID")]
        public ActionResult<RequestAction> GetRequestAction(Guid ID)
        {
            try
            {
                var rs = _requestActionService.GetByID(ID);
                if (rs == null) return BadRequest("ID not found!");
                RequestActionVM result = _mapper.Map<RequestActionVM>(rs);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // PUT: api/RequestActions
        // Người phê duyệt không có quyền update
        //[HttpPut]
        //public IActionResult PutRequestAction(RequestActionUM model)
        //{
        //    if (!ModelState.IsValid) return BadRequest(ModelState);
        //    try
        //    {
        //        var requestActionInDb = _requestActionService.GetByID(model.ID);
        //        if (requestActionInDb == null) return BadRequest("ID not found!");
        //        _mapper.Map(model, requestActionInDb);
        //        _requestActionService.Save();
        //        return Ok("success");
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.Message);
        //    }
        //}

        // DELETE: api/RequestActions/5
        [HttpDelete("{id}")]
        public ActionResult DeleteRequestActions(Guid ID)
        {
            try
            {
                var requestActionInDb = _requestActionService.GetByID(ID);
                if (requestActionInDb == null) return BadRequest(WebConstant.NotFound);

                requestActionInDb.IsDeleted = true;
                _requestActionService.Save();
                return Ok(WebConstant.Success);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
