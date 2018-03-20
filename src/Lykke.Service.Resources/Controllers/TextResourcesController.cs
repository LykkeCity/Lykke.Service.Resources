using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Common;
using Lykke.Common.ApiLibrary.Extensions;
using Lykke.Service.Resources.Core.Domain.TextResources;
using Lykke.Service.Resources.Core.Services;
using Lykke.Service.Resources.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lykke.Service.Resources.Controllers
{
    [Route("api/[controller]")]
    public class TextResourcesController : Controller
    {
        private readonly ITextResourcesService _textResourcesService;
        private readonly ILanguagesService _languagesService;

        public TextResourcesController(
            ITextResourcesService textResourcesService,
            ILanguagesService languagesService
            )
        {
            _textResourcesService = textResourcesService;
            _languagesService = languagesService;
        }
        
        [HttpGet]
        [SwaggerOperation("GetAllTextResources")]
        [ProducesResponseType(typeof(IEnumerable<TextResource>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NotFound)]
        public IActionResult GetAllResources()
        {
            var resource = _textResourcesService.GetAll();
            
            if (resource == null)
                return NotFound();
            
            return Ok(resource);
        }
        
        [HttpGet("{lang}/{name}")]
        [SwaggerOperation("GetTextResource")]
        [ProducesResponseType(typeof(TextResource), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NotFound)]
        public IActionResult GetResource(string lang, string name)
        {
            var resource = _textResourcesService.Get(lang, name);
            
            if (resource == null)
                return NotFound();
            
            return Ok(resource);
        }
        
        [HttpGet("all/{lang}/{name}")]
        [SwaggerOperation("GetTextResources")]
        [ProducesResponseType(typeof(IEnumerable<TextResource>), (int)HttpStatusCode.OK)]
        public IActionResult GetAll(string lang, string name)
        {
            var resources = _textResourcesService.GetSection(lang, name);
            return Ok(resources);
        }
        
        [HttpPost("add")]
        [SwaggerOperation("AddTextResource")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddResource([FromBody]TextResourceModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorResponse.Create(ModelState.GetErrorMessage()));

            if (!model.Lang.IsValidPartitionOrRowKey())
                return BadRequest(ErrorResponse.Create($"Invalid {nameof(model.Lang)} value"));
            
            if (!model.Name.IsValidPartitionOrRowKey())
                return BadRequest(ErrorResponse.Create($"Invalid {nameof(model.Name)} value"));

            var language = _languagesService.Get(model.Lang);

            if (language == null)
                return BadRequest(ErrorResponse.Create($"Language with code '{model.Lang}' not found"));
            
            await _textResourcesService.AddAsync(model.Lang, model.Name, model.Value);
            return Ok();
        }
        
        [HttpPost("delete")]
        [SwaggerOperation("DeleteTextResource")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteResource([FromBody]DeleteTextResourceModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorResponse.Create(ModelState.GetErrorMessage()));

            if (!model.Lang.IsValidPartitionOrRowKey())
                return BadRequest(ErrorResponse.Create($"Invalid {nameof(model.Lang)} value"));
            
            if (!model.Name.IsValidPartitionOrRowKey())
                return BadRequest(ErrorResponse.Create($"Invalid {nameof(model.Name)} value"));

            var res = _textResourcesService.Get(model.Lang, model.Name);

            if (res == null)
                return BadRequest($"Text resource with language '{model.Lang}' and name '{model.Name}' not found");
            
            await _textResourcesService.DeleteAsync(model.Lang, model.Name);
            return Ok();
        }
    }
}
