using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Common;
using Lykke.Common.ApiLibrary.Extensions;
using Lykke.Service.Resources.Core.Domain.Languages;
using Lykke.Service.Resources.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lykke.Service.Resources.Controllers
{
    [Route("api/[controller]")]
    public class LanguagesController : Controller
    {
        private readonly ILanguagesRepository _repository;

        public LanguagesController(
            ILanguagesRepository repository
            )
        {
            _repository = repository;
        }
        
        [HttpGet]
        [SwaggerOperation("GetAllLanguages")]
        [ProducesResponseType(typeof(IEnumerable<Language>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAllLanguages()
        {
            var languages = await _repository.GetAllAsync();
            
            if (languages == null)
                return NotFound();
            
            return Ok(languages);
        }
        
        [HttpPost("add")]
        [SwaggerOperation("AddLanguage")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddLanguage([FromBody]Language model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorResponse.Create(ModelState.GetErrorMessage()));

            if (!model.Code.IsValidPartitionOrRowKey())
                return BadRequest(ErrorResponse.Create($"Invalid {nameof(model.Code)} value"));
            
            if (!model.Name.IsValidPartitionOrRowKey())
                return BadRequest(ErrorResponse.Create($"Invalid {nameof(model.Name)} value"));
            
            await _repository.AddAsync(model.Code, model.Name);
            return Ok();
        }
        
        [HttpPost("delete")]
        [SwaggerOperation("DeleteLanguage")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteLanguage(string code)
        {
            if (string.IsNullOrEmpty(code))
                return BadRequest(ErrorResponse.Create($"{nameof(code)} can't be empty"));
            
            if (!code.IsValidPartitionOrRowKey())
                return BadRequest(ErrorResponse.Create($"Invalid {nameof(code)} value"));

            await _repository.DeleteAsync(code);
            return Ok();
        }
    }
}
