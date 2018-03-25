using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Common;
using Lykke.Common.ApiLibrary.Extensions;
using Lykke.Service.Resources.Core.Services;
using Lykke.Service.Resources.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lykke.Service.Resources.Controllers
{
    [Route("api/languages")]
    public class LanguagesController : Controller
    {
        private readonly ILanguagesService _service;

        public LanguagesController(
            ILanguagesService languagesService
            )
        {
            _service = languagesService;
        }
        
        /// <summary>
        /// Gets all languages
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation("GetAllLanguages")]
        [ProducesResponseType(typeof(IEnumerable<Language>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NotFound)]
        public IActionResult GetAllLanguages()
        {
            var languages = _service.GetAll();
            
            if (languages == null)
                return NotFound();
            
            return Ok(languages);
        }
        
        /// <summary>
        /// Adds language
        /// </summary>
        /// <param name="model">language model</param>
        /// <returns></returns>
        [HttpPost]
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
            
            await _service.AddAsync(model.Code, model.Name);
            return Ok();
        }
        
        /// <summary>
        /// Deletes language by code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpDelete("{code}")]
        [SwaggerOperation("DeleteLanguage")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteLanguage(string code)
        {
            if (string.IsNullOrEmpty(code))
                return BadRequest(ErrorResponse.Create($"{nameof(code)} can't be empty"));
            
            if (!code.IsValidPartitionOrRowKey())
                return BadRequest(ErrorResponse.Create($"Invalid {nameof(code)} value"));

            var language = _service.Get(code);

            if (language == null)
                return BadRequest(ErrorResponse.Create($"Language with code '{code}' not found"));
            
            await _service.DeleteAsync(code);
            return Ok();
        }
    }
}
