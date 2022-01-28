//using Microsoft.AspNetCore.Mvc;

//namespace Identity_And_Access_Management.Controllers
//{
//    [ApiExplorerSettings(IgnoreApi = true)]
//    public class OpenIddictScopesController : Controller
//    {
//        private readonly IScopeApiService _service;

//        public OpenIddictScopesController(IScopeApiService service)
//        {
//            _service = service;
//        }

//        [HttpGet]
//        //[ProducesResponseType(typeof(IEnumerable<ScopeViewModel>), StatusCodes.Status200OK)]
//        public async Task<IActionResult> GetAllScopes()
//        {
//            var result = await _service.GetScopesAsync();

//            return View(result);
//        }

//        [HttpGet("names")]
//        //[ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
//        public async Task<IActionResult> GetScopeNames()
//        {
//            var result = await _service.GetScopeNamesAsync();

//            return Ok(result);
//        }

//        [HttpGet("{id}")]
//        [ActionName(name:"GetScopebyScopeId")]
//        //[ProducesResponseType(typeof(ScopeViewModel), StatusCodes.Status200OK)]
//        public async Task<IActionResult> GetScopeById(string id)
//        {
//            if (id == null) return BadRequest();

//            var result = await _service.GetAsync(id);
//            if (result == null) return NotFound();

//            return Ok(result);
//        }

//        [HttpPost]
//        //[ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
//        public async Task<IActionResult> Create([FromBody] ScopeViewModel model)
//        {
//            if (model == null) return BadRequest();
//            if (!ModelState.IsValid) return BadRequest(ModelState);

//            var result = await _service.CreateAsync(model);

//            return Created($"scopes/{result}", result);
//        }

//        [HttpPut]
//        //[ProducesResponseType(StatusCodes.Status204NoContent)]
//        public async Task<IActionResult> Update([FromBody] ScopeViewModel model)
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

//    }
//}
