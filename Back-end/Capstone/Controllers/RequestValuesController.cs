using AutoMapper;
using Capstone.Model;
using Capstone.Service;
using Capstone.Service.Helper;
using Capstone.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Capstone.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class RequestValuesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRequestValueService _requestValueService;

        public RequestValuesController(IMapper mapper, IRequestValueService requestValueService)
        {
            _mapper = mapper;
            _requestValueService = requestValueService;
        }

        // POST: api/requestValues
        [HttpPost]
        public ActionResult PostRequestValue(RequestValueCM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                RequestValue requestValue = new RequestValue();
                requestValue = _mapper.Map<RequestValue>(model);
                _requestValueService.Create(requestValue);
                _requestValueService.Save();
                return StatusCode(201, requestValue.ID);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/RequestValues
        [HttpGet]
        public ActionResult<IEnumerable<RequestValueVM>> GetRequestValues()
        {
            try
            {
                List<RequestValueVM> result = new List<RequestValueVM>();
                var data = _requestValueService.GetAll();
                foreach (var item in data)
                {
                    result.Add(_mapper.Map<RequestValueVM>(item));
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/RequestValues/GetByID
        [HttpGet("GetByID")]
        public ActionResult<RequestValueVM> GetRequestValue(Guid ID)
        {
            try
            {
                var rs = _requestValueService.GetByID(ID);
                if (rs == null) return BadRequest(WebConstant.NotFound);
                RequestValueVM result = _mapper.Map<RequestValueVM>(rs);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // PUT: api/RequestValues
        [HttpPut]
        public IActionResult PutRequestValue(RequestValueUM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var requestValueInDb = _requestValueService.GetByID(model.ID);
                if (requestValueInDb == null) return BadRequest(WebConstant.NotFound);
                _mapper.Map(model, requestValueInDb);
                _requestValueService.Save();
                return Ok(WebConstant.Success);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
