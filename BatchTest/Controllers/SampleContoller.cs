using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using ODataBatchResponseSample.Data;
using ODataBatchResponseSample.Entities;
using System.Threading.Tasks;

namespace ODataBatchResponseSample.Controllers
{
    public class SampleContoller : ODataController
    {
        private readonly SampleDbContext _db;
        public SampleContoller(SampleDbContext db)
        {
            _db = db;
        }

        [EnableQuery]
        [ODataRoute("Data")]
        public IActionResult GetData()
        {
            return Ok(_db.Data);
        }

        [EnableQuery]
        [ODataRoute("Data({Id})")]
        public async Task<IActionResult> GetData(int Id)
        {
            var entity = await _db.Data.FindAsync(Id);

            if (entity == null)
                return NotFound();

            return Ok(entity);
        }

        [ODataRoute("Data")]
        public async Task<IActionResult> PostData([FromBody]Datum entity)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);

            _db.Data.Add(entity);

            await _db.SaveChangesAsync();

            return Created(entity);
        }
    }
}
