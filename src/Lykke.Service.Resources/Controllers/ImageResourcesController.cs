using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Common;
using Lykke.Common.ApiLibrary.Extensions;
using Lykke.Service.Resources.Core.Domain.ImageResources;
using Lykke.Service.Resources.Core.Services;
using Lykke.Service.Resources.Models;
using Lykke.Service.Resources.Settings.ServiceSettings;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lykke.Service.Resources.Controllers
{
    [Route("api/[controller]")]
    public class ImageResourcesController : Controller
    {
        private readonly IImageResourcesService _imageResourcesService;
        private readonly ResourcesSettings _settings;

        public ImageResourcesController(
            IImageResourcesService imageResourcesService,
            ResourcesSettings settings
            )
        {
            _imageResourcesService = imageResourcesService;
            _settings = settings;
        }
        
        [HttpGet]
        [SwaggerOperation("GetAllImageResources")]
        [ProducesResponseType(typeof(IEnumerable<ImageResource>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NotFound)]
        public IActionResult GetAllResources()
        {
            var resource = _imageResourcesService.GetAll();
            
            if (resource == null)
                return NotFound();
            
            return Ok(resource);
        }
        
        [HttpGet("{name}")]
        [SwaggerOperation("GetImageResource")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NotFound)]
        public IActionResult GetResource(string name)
        {
            var resource = _imageResourcesService.Get(name);
            
            if (resource == null)
                return NotFound();
            
            return Ok(resource);
        }
        
        [HttpPost("add")]
        [SwaggerOperation("AddImageResource")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddResource([FromBody]ImageResourceModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorResponse.Create(ModelState.GetErrorMessage()));

            if (model.Data.Length == 0)
                return BadRequest(ErrorResponse.Create($"Invalid {nameof(model.Data)} value"));
            
            if (model.Data.Length > _settings.MaxFileSizeInMb * 1024 * 1024)
                return BadRequest(ErrorResponse.Create($"File size must be below {_settings.MaxFileSizeInMb} Mb"));

            if (await _imageResourcesService.IsFileExistsAsync(model.Name))
                return BadRequest(ErrorResponse.Create($"File with name {model.Name} already exists"));
            
            await _imageResourcesService.AddAsync(model.Name, model.Data);
            return Ok();
        }
        
        [HttpPost("delete")]
        [SwaggerOperation("DeleteImageResource")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteResource(string name)
        {
            if (string.IsNullOrEmpty(name))
                return BadRequest(ErrorResponse.Create($"{nameof(name)} can't be empty"));
            
            if (!name.IsValidPartitionOrRowKey())
                return BadRequest(ErrorResponse.Create($"Invalid {nameof(name)} value"));

            await _imageResourcesService.DeleteAsync(name);
            return Ok();
        }
    }
}
