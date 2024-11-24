using Microsoft.AspNetCore.Mvc;
using CommonBoilerPlateEight.Domain.Entity;
using CommonBoilerPlateEight.Domain.Helper;
using CommonBoilerPlateEight.Domain.Interfaces;
using CommonBoilerPlateEight.Web.Extensions;
using System.Text.Json;
using System.Transactions;

namespace CommonBoilerPlateEight.Web.Controllers
{
    public class DataMigrationController : Controller
    {
        private readonly IDbContext _db;
        public DataMigrationController(IDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> SeedCountry()
        {
            using var tx = TransactionScopeHelper.GetInstance();
            string jsonFilePath = "wwwroot/Country.json"; // Update with your file path
            List<CountryModel> countries = await ReadJsonFileAsync(jsonFilePath);
            foreach (var country in countries)
            {
                var newCountry = new Country(country.name, country.flag, country.code, country.dial_code);
                await _db.Countries.AddAsync(newCountry);
            }
            await _db.SaveChangesAsync();
            tx.Complete();
            this.NotifySuccess("migration complete");
            return RedirectToAction("Index", "Home");
        }
        private static async Task<List<CountryModel>> ReadJsonFileAsync(string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                return await JsonSerializer.DeserializeAsync<List<CountryModel>>(fs);
            }
        }


    }

    public class CountryModel
    {
        public string name { get; set; }
        public string flag { get; set; }
        public string code { get; set; }
        public string dial_code { get; set; }
    }
}
