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
            // 1. SQL Sorgusu
            string query = @"
                SELECT 
                    Country as CountryName, 
                    COUNT(*) as Count 
                FROM Customers 
                GROUP BY Country 
                ORDER BY Count DESC";

            using var connection = appDbContext.CreateConnection();
            var chartData = (await connection.QueryAsync<CountryChartViewModel>(query)).ToList();

            var totalUserCount = chartData.Sum(x => x.Count);

            // --- GENEL VERİLER ---
            ViewBag.SeriesData = chartData.Select(x => x.Count).ToList();
            ViewBag.LabelsData = chartData.Select(x => x.CountryName).ToList();
            ViewBag.TotalUserCount = totalUserCount;

            // ---------------------------------------------------------------------
            // --- 1. EN ÇOK OLAN 4 ÜLKE (TOP COUNTRIES) ---
            // ---------------------------------------------------------------------
            var topCountries = chartData.Take(4).Select(x =>
            {
                string iso = GetCountryIsoCode(x.CountryName);
                return new
                {
                    CountryName = x.CountryName,
                    Count = x.Count,
                    FlagUrl = iso != "XX" ? $"https://flagcdn.com/w40/{iso.ToLower()}.png" : "assets/images/default.png",
                    // DÜZELTME BURADA: İSMİNİ 'Ratio' YAPTIK
                    Ratio = totalUserCount > 0 ? ((double)x.Count / totalUserCount) * 100 : 0
                };
            }).ToList();

            ViewBag.TopCountries = topCountries;


            // ---------------------------------------------------------------------
            // --- 2. EN AZ OLAN 4 ÜLKE (LEAST COUNTRIES) ---
            // ---------------------------------------------------------------------
            var leastCountries = chartData.OrderBy(x => x.Count).Take(4).Select(x =>
            {
                string iso = GetCountryIsoCode(x.CountryName);
                return new
                {
                    CountryName = x.CountryName,
                    Count = x.Count,
                    FlagUrl = iso != "XX" ? $"https://flagcdn.com/w40/{iso.ToLower()}.png" : "assets/images/default.png",
                    // DÜZELTME BURADA: İSMİNİ 'Ratio' YAPTIK
                    Ratio = totalUserCount > 0 ? ((double)x.Count / totalUserCount) * 100 : 0
                };
            }).ToList();

            ViewBag.LeastCountries = leastCountries;


            // --- 3. HARİTA İÇİN MARKER VERİSİ ---
            var markersList = new List<object>();
            foreach (var item in chartData)
            {
                string isoCode = GetCountryIsoCode(item.CountryName);
                double[] coords = GetCountryCoordinates(isoCode);
                if (coords != null)
                {
                    markersList.Add(new { name = item.CountryName, coords = coords, count = item.Count });
                }
            }
            ViewBag.MapMarkers = markersList;




            // SQL Sorgusunu güncelliyoruz: TotalPrice'ı da topluyoruz.
            string orderQuery = @"
    SELECT 
        MONTH(OrderDate) as Month, 
        COUNT(Id) as TotalOrders, 
        SUM(Quantity) as TotalQuantity,
        SUM(TotalPrice) as TotalRevenue
    FROM Orders
    GROUP BY MONTH(OrderDate)
    ORDER BY Month";

            var orderData = (await connection.QueryAsync(orderQuery)).ToList();

            // 12 Aylık diziler
            var monthlyOrders = new int[12];
            var monthlyQuantities = new int[12];
            var monthlyRevenue = new decimal[12]; // Para olduğu için decimal

            foreach (var item in orderData)
            {
                int monthIndex = item.Month - 1;
                if (monthIndex >= 0 && monthIndex < 12)
                {
                    monthlyOrders[monthIndex] = (int)item.TotalOrders;
                    monthlyQuantities[monthIndex] = (int)item.TotalQuantity;
                    monthlyRevenue[monthIndex] = (decimal)item.TotalRevenue; // Toplam Ciro
                }
            }

            // View'a Gönderim
            ViewBag.MonthlyOrders = monthlyOrders;     // Sipariş Sayısı (Sütun Grafik için)
            ViewBag.MonthlyQuantities = monthlyQuantities; // (Önceki chart için)
            ViewBag.MonthlyRevenue = monthlyRevenue;   // Satış/Ciro (Çizgi Grafik için)

            // ==========================================================


            string topCustomersQuery = @"
    SELECT TOP 8
        c.CustomerId,
        c.CustomerName,
        c.CustomerSurname,
        c.CustomerImageUrl,
        c.City,     -- Şehir eklendi
        c.Country,  -- Ülke eklendi
        c.Email,
        SUM(o.TotalPrice) as TotalSpending
    FROM Orders o
    INNER JOIN Customers c ON o.CustomerId = c.CustomerId
    GROUP BY c.CustomerId, c.CustomerName, c.CustomerSurname, c.CustomerImageUrl, c.City, c.Country, c.Email
    ORDER BY TotalSpending DESC";

            var topCustomers = (await connection.QueryAsync<TopCustomerViewModel>(topCustomersQuery)).ToList();

            ViewBag.TopCustomers = topCustomers;



            string lowBudgetQuery = @"
    SELECT TOP 6
        c.CustomerId,
        c.CustomerName,
        c.CustomerSurname,
        c.CustomerImageUrl,
        c.Email,
        o.PaymentMethod,
        o.Status,
        o.TotalPrice
    FROM Orders o
    INNER JOIN Customers c ON o.CustomerId = c.CustomerId
    ORDER BY o.TotalPrice ASC"; // En düşükten yükseğe

            var lowBudgetCustomers = (await connection.QueryAsync<LowBudgetCustomerViewModel>(lowBudgetQuery)).ToList();

            ViewBag.LowBudgetCustomers = lowBudgetCustomers;
            // ==========================================================




            // 1. En Yüksek Sipariş Tutarı
            string maxOrderQuery = "SELECT MAX(TotalPrice) FROM Orders";
            var maxOrderPrice = await connection.ExecuteScalarAsync<decimal>(maxOrderQuery);

            // 2. En Çok Satan Ürün (Adı ve Kategorisi)
            // Not: Products ve Categories tablolarınızın olduğunu varsayıyoruz. 
            // Eğer yoksa sadece Orders'tan ProductId çekebiliriz.
            string bestProductQuery = @"
    SELECT TOP 1 
        p.ProductName, 
        c.CategoryName
    FROM Orders o
    INNER JOIN Products p ON o.ProductId = p.ProductId
    INNER JOIN Categories c ON p.CategoryId = c.CategoryId
    GROUP BY p.ProductName, c.CategoryName
    ORDER BY SUM(o.Quantity) DESC";

            var bestProduct = await connection.QueryFirstOrDefaultAsync(bestProductQuery);

            // 3. En Çok Satan Kategori
            string bestCategoryQuery = @"
    SELECT TOP 1 
        c.CategoryName
    FROM Orders o
    INNER JOIN Products p ON o.ProductId = p.ProductId
    INNER JOIN Categories c ON p.CategoryId = c.CategoryId
    GROUP BY c.CategoryName
    ORDER BY SUM(o.Quantity) DESC";

            string bestCategory = await connection.ExecuteScalarAsync<string>(bestCategoryQuery);

            // 4. En Çok Kullanılan Ödeme Yöntemi
            string topPaymentQuery = @"
    SELECT TOP 1 PaymentMethod 
    FROM Orders 
    GROUP BY PaymentMethod 
    ORDER BY COUNT(*) DESC";

            string topPaymentMethod = await connection.ExecuteScalarAsync<string>(topPaymentQuery);

            // Verileri ViewBag'e atıyoruz
            ViewBag.Stats_MaxOrderPrice = maxOrderPrice;
            ViewBag.Stats_BestProduct = bestProduct; // { ProductName, CategoryName } döner
            ViewBag.Stats_BestCategory = bestCategory;
            ViewBag.Stats_PaymentMethod = topPaymentMethod;


            return View();
        }

        // --- YARDIMCI METOTLAR (AYNEN KALACAK) ---
        private string GetCountryIsoCode(string countryName)
        {
            if (string.IsNullOrEmpty(countryName)) return "XX";
            countryName = countryName.Trim();
            var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "Türkiye", "TR" }, { "Turkey", "TR" }, { "Turkiye", "TR" },
                { "Almanya", "DE" }, { "Germany", "DE" },
                { "İngiltere", "GB" }, { "United Kingdom", "GB" },
                { "Fransa", "FR" }, { "France", "FR" },
                { "İspanya", "ES" }, { "Spain", "ES" },
                { "Hollanda", "NL" }, { "Netherlands", "NL" },
                { "Belçika", "BE" }, { "Belgium", "BE" },
                { "Avusturya", "AT" }, { "Austria", "AT" },
                { "Danimarka", "DK" }, { "Denmark", "DK" },
                { "İsveç", "SE" }, { "Sweden", "SE" },
                { "Macaristan", "HU" }, { "Hungary", "HU" },
                { "Bulgaristan", "BG" }, { "bullgarisatn", "BG" },
                { "ABD", "US" }, { "United States", "US" },
                { "Rusya", "RU" }, { "Russia", "RU" },
                { "Brezilya", "BR" }, { "Brazil", "BR" },
                { "Çin", "CN" }, { "China", "CN" }
            };
            return map.ContainsKey(countryName) ? map[countryName] : "XX";
        }

        private double[] GetCountryCoordinates(string isoCode)
        {
            var coords = new Dictionary<string, double[]>
            {
                { "TR", new double[] { 38.9637, 35.2433 } },
                { "DE", new double[] { 51.1657, 10.4515 } },
                { "GB", new double[] { 55.3781, -3.4360 } },
                { "FR", new double[] { 46.2276, 2.2137 } },
                { "ES", new double[] { 40.4637, -3.7492 } },
                { "NL", new double[] { 52.1326, 5.2913 } },
                { "BE", new double[] { 50.5039, 4.4699 } },
                { "AT", new double[] { 47.5162, 14.5501 } },
                { "DK", new double[] { 56.2639, 9.5018 } },
                { "SE", new double[] { 60.1282, 18.6435 } },
                { "HU", new double[] { 47.1625, 19.5033 } },
                { "BG", new double[] { 42.7339, 25.4858 } },
                { "US", new double[] { 37.0902, -95.7129 } },
                { "RU", new double[] { 61.5240, 105.3188 } },
                { "BR", new double[] { -14.2350, -51.9253 } },
                { "CN", new double[] { 35.8617, 104.1954 } }
            };
            return coords.ContainsKey(isoCode) ? coords[isoCode] : null;
        }
    }
}