//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;

//namespace Identity_And_Access_Management.Controllers
//{
//    [ApiExplorerSettings(IgnoreApi = true)]
//    public class OpenIddictApplicationsController : Controller
//    {

//        private readonly IApplicationApiService _service;
//        public OpenIddictApplicationsController(
//          IApplicationApiService service
//        )
//        {
//            _service = service;
//        }

//        [HttpGet]
//        //[ProducesResponseType(typeof(IEnumerable<ApplicationViewModel>), StatusCodes.Status200OK)]
//        public async Task<IActionResult> GetAllApps()
//        {
//            var result = await _service.GetApplicationsAsync();

//            return View(result);
//        }

//        [HttpGet("{id}")]
//        [ActionName(name: "GetRegisteredAppbyAppId")]
//        //[ProducesResponseType(typeof(ApplicationViewModel), StatusCodes.Status200OK)]
//        public async Task<IActionResult> GetAppbyId(string Scopeid)
//        {
//            if (Scopeid == null) return BadRequest();

//            var result = await _service.GetAsync(Scopeid);
//            if (result == null) return NotFound();

//            return View(result);
//        }
//        [HttpGet]
//        //[ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
//        public async Task<IActionResult> Create()
//        {
//            ApplicationViewModel m = new ApplicationViewModel();

//            return View(m);
//        }
//        [HttpPost]
//        //[ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
//        public async Task<IActionResult> Create([FromBody] ApplicationViewModel model)
//        {
//            if (model == null) return BadRequest();
//            if (!ModelState.IsValid) return BadRequest(ModelState);

//            var result = await _service.CreateAsync(model);

//            return Created($"clients/{result}", result);
//        }

//        [HttpPut]
//        //[ProducesResponseType(StatusCodes.Status204NoContent)]
//        public async Task<IActionResult> Update([FromBody] ApplicationViewModel model)
//        {
//            if (model == null) return BadRequest();
//            if (!ModelState.IsValid) return BadRequest(ModelState);

//            await _service.UpdateAsync(model);

//            return NoContent();
//        }

//        [HttpDelete("{id}")]
//        //[ProducesResponseType(StatusCodes.Status204NoContent)]
//        public async Task<IActionResult> Delete(string id)
//        {
//            if (id == null) return BadRequest();

//            await _service.DeleteAsync(id);

//            return NoContent();
//        }

//        [HttpGet("options")]
//        [AllowAnonymous]
//        //[ProducesResponseType(typeof(ApplicationOptionsViewModel), StatusCodes.Status200OK)]
//        public async Task<IActionResult> GetAllOptions()
//        {
//            var result = await _service.GetOptionsAsync();

//            return Ok(result);
//        }

//    }
//}
