using AutoMapper;
using Capstone.Helper;
using Capstone.Service;
using Capstone.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;

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
        [HttpPost, DisableRequestSizeLimit]
        public ActionResult PostRequestFile()
        {
            try
            {
                List<string> listPath = new List<string>();
                var files = Request.Form.Files;
                var folderName = WebConstant.Resources;
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName); // đường dẫn tuyệt đối tới folder

                if (!Directory.Exists(pathToSave)) // kiểm tra folder có tồn tại
                {
                    Directory.CreateDirectory(pathToSave);
                }
                if (files.Count > 0 && files != null)
                {
                    foreach (IFormFile file in files)
                    {
                        if (file.Length > 0)
                        {
                            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                            var fullPath = Path.Combine(pathToSave, fileName); // đường dẫn tuyệt đối file
                            var dbPath = Path.Combine(folderName, fileName); // đường tương đối file

                            using (var stream = new FileStream(fullPath, FileMode.Create))
                            {
                                file.CopyTo(stream);
                            }
                            listPath.Add(dbPath);
                        }
                        else
                        {
                            return BadRequest();
                        }
                    }
                }
                else
                {
                    return BadRequest();
                }

                return Ok(listPath);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/RequestFiles
        [HttpGet]
        public ActionResult<IEnumerable<RequestFileVM>> GetRequestFiles()
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
        public ActionResult<RequestFileVM> GetRequestFile(Guid ID)
        {
            try
            {
                var rs = _requestFileService.GetByID(ID);
                if (rs == null) return BadRequest(WebConstant.NotFound);
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
                if (requestFileInDb == null) return BadRequest(WebConstant.NotFound);
                _mapper.Map(model, requestFileInDb);
                _requestFileService.Save();
                return Ok(WebConstant.Success);
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
                if (requestFileInDb == null) return BadRequest(WebConstant.NotFound);
                requestFileInDb.IsDeleted = true;
                _requestFileService.Save();
                return Ok(WebConstant.Success);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
