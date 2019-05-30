using AutoMapper;
using Capstone.Model;
using Capstone.Service;
using Capstone.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class RequestFilesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRequestFileService _requestFileService;

        public RequestFilesController(IMapper mapper, IRequestFileService requestFileService)
        {
            _mapper = mapper;
            _requestFileService = requestFileService;
        }

        // POST: api/RequestFiles
        [HttpPost]
        public ActionResult<RequestFile> PostRequestFile(RequestFileCM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                RequestFile requestFile = new RequestFile();
                requestFile = _mapper.Map<RequestFile>(model);
                _requestFileService.Create(requestFile);
                _requestFileService.Save();
                return StatusCode(201, "RequestFile Type Created!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/RequestFiles
        [HttpGet]
        public ActionResult<IEnumerable<RequestFile>> GetRequestFiles()
        {
            try
            {
                List<RequestFileVM> result = new List<RequestFileVM>();
                var data = _requestFileService.GetAll();
                foreach (var item in data)
                {
                    result.Add(_mapper.Map<RequestFileVM>(item));
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/RequestFiles/GetByID
        [HttpGet("GetByID")]
        public ActionResult<RequestFile> GetRequestFile(Guid ID)
        {
            try
            {
                var rs = _requestFileService.GetByID(ID);
                if (rs == null) return BadRequest("ID not found!");
                RequestFileVM result = _mapper.Map<RequestFileVM>(rs);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // PUT: api/RequestFiles
        [HttpPut]
        public IActionResult PutRequestFile(RequestFileUM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var requestFileInDb = _requestFileService.GetByID(model.ID);
                if (requestFileInDb == null) return BadRequest("ID not found!");
                _mapper.Map(model, requestFileInDb);
                _requestFileService.Save();
                return Ok("success");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/RequestFiles
        [HttpDelete]
        public ActionResult DeleteRequestFile(Guid ID)
        {
            try
            {
                var requestFileInDb = _requestFileService.GetByID(ID);
                if (requestFileInDb == null) return BadRequest("ID not found!");
                requestFileInDb.isEnable = false;
                _requestFileService.Save();
                return Ok("success");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
