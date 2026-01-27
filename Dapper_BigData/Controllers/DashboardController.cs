using Dapper;
using Dapper_BigData.Models;
using Kaira.WebUI.Context;
using Microsoft.AspNetCore.Mvc;

namespace Dapper_BigData.Controllers
{
    public class DashboardController(AppDbContext appDbContext) : Controller
    {
       public async Task<IActionResult> Index()
        {
            // 1. SQL Sorgusu: Ülkeye göre grupla ve say
            // NOT: Tablo adını kendi tablona göre düzelt (Örn: Tbl_Customer, Users vb.)
            string query = @"
                SELECT 
                    Country as CountryName, 
                    COUNT(*) as Count 
                FROM Customers 
                GROUP BY Country 
                ORDER BY Count DESC"; 

            // 2. Bağlantıyı oluştur ve veriyi çek
            using var connection = appDbContext.CreateConnection();
            var chartData = await connection.QueryAsync<CountryChartViewModel>(query);

            // 3. Verileri ayırıp View tarafına gönderiyoruz
            // Chart bizden iki ayrı dizi (array) istiyor: biri sayılar, biri isimler.
            ViewBag.SeriesData = chartData.Select(x => x.Count).ToList();
            ViewBag.LabelsData = chartData.Select(x => x.CountryName).ToList();

            return View();
        }
    }
}
