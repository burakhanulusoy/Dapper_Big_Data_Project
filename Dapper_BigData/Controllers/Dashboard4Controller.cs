using Dapper;
using Dapper_BigData.Models;
using Kaira.WebUI.Context;
using Microsoft.AspNetCore.Mvc;

namespace Dapper_BigData.Controllers
{
    public class Dashboard4Controller(AppDbContext appDbContext) : Controller
    {
        public async Task<IActionResult> Index()
        {
            using var connection = appDbContext.CreateConnection();

            // Şehir ve Ülke bazında müşteri sayılarını çekiyoruz
            string query = @"
                SELECT 
                    City, 
                    Country, 
                    COUNT(CustomerId) as CustomerCount 
                FROM Customers
                WHERE City IS NOT NULL AND City <> ''
                GROUP BY City, Country
                ORDER BY CustomerCount DESC";

            var cityData = (await connection.QueryAsync<CityLocationViewModel>(query)).ToList();

            ViewBag.CityLocations = cityData;

            return View();
        }
    }
}