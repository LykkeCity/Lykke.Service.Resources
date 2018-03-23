using System.Collections.Generic;
using System.Linq;
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
    [Route("api/textresources")]
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
        
        /// <summary>
        /// Gets all text resources
        /// </summary>
        /// <returns></returns>
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
        
        /// <summary>
        /// Gets text resource by language and name
        /// </summary>
        /// <param name="lang">languages</param>
        /// <param name="name">full name of the resource</param>
        /// <returns></returns>
        [HttpGet("{lang}/{name}/resource")]
        [SwaggerOperation("GetTextResource")]
        [ProducesResponseType(typeof(TextResource), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NotFound)]
        public IActionResult GetResource(string lang, string name)
        {
            if (!lang.IsValidPartitionOrRowKey())
                return BadRequest(ErrorResponse.Create($"Invalid {nameof(lang)} value"));
            
            if (!name.IsValidPartitionOrRowKey())
                return BadRequest(ErrorResponse.Create($"Invalid {nameof(name)} value"));

            var language = _languagesService.Get(lang);

            if (language == null)
                return BadRequest(ErrorResponse.Create($"Language with code '{lang}' not found"));
            
            var resource = _textResourcesService.Get(lang, name);
            
            if (resource == null)
                return NotFound();
            
            return Ok(resource);
        }
        
        /// <summary>
        /// Gets section of text resources
        /// </summary>
        /// <remarks>Returns text resources in the specified section, for example: lykke.ios will return all text resources under this section (lykke.ios.text, lykke.ios.title etc.)</remarks>
        /// <param name="lang">language</param>
        /// <param name="name">full name of the section</param>
        /// <returns></returns>
        [HttpGet("{lang}/{name}/section")]
        [SwaggerOperation("GetTextResourceSection")]
        [ProducesResponseType(typeof(IEnumerable<TextResource>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NotFound)]
        public IActionResult GetTextResourceSection(string lang, string name)
        {
            if (!lang.IsValidPartitionOrRowKey())
                return BadRequest(ErrorResponse.Create($"Invalid {nameof(lang)} value"));
            
            if (!name.IsValidPartitionOrRowKey())
                return BadRequest(ErrorResponse.Create($"Invalid {nameof(name)} value"));

            var language = _languagesService.Get(lang);

            if (language == null)
                return BadRequest(ErrorResponse.Create($"Language with code '{lang}' not found"));
            
            var resources = _textResourcesService.GetSection(lang, name).ToList();

            if (resources.Count == 0)
                return NotFound();
            
            return Ok(resources);
        }
        
        /// <summary>
        /// Adds text resource
        /// </summary>
        /// <param name="model">text resource model</param>
        /// <returns></returns>
        [HttpPost]
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
        
        /// <summary>
        /// Deletes text reosource
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpDelete]
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
