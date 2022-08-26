using DataCollector.Core;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DataCollector.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreatorsController : ControllerBase
    {
        private ICreatorListService _creatorListService;

        public CreatorsController(ICreatorListService creatorListService)
        {
            _creatorListService = creatorListService;
        }

        // GET: api/<CreatorsController>
        /*[HttpGet]
        public async Task<IEnumerable<CreatorDto>> GetAllCreators()
        {
            await _creatorListService.FillDtoListByDatabase();
            return _creatorListService.GetD;
        }*/

        // POST api/<CreatorsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<CreatorsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CreatorsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
