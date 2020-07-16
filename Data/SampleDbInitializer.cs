using ODataBatchResponseSample.Entities;

namespace ODataBatchResponseSample.Data
{
    public class SampleDbInitializer
    {
        public static void Initialize(SampleDbContext db)
        {
            db.Database.EnsureDeleted();

            db.Data.Add(new Datum { Name = "A" });
            db.Data.Add(new Datum { Name = "B" });
            db.Data.Add(new Datum { Name = "C" });

            db.SaveChanges();
        }
    }
}
