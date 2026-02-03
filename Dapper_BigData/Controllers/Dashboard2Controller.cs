using Dapper;
using Dapper_BigData.Models;
using Kaira.WebUI.Context; // Context'inizin namespace'i
using Microsoft.AspNetCore.Mvc;

namespace Dapper_BigData.Controllers
{
    public class Dashboard2Controller(AppDbContext appDbContext) : Controller
    {
        public async Task<IActionResult> Index()
        {
            // 1. Veritabanı Bağlantısı
            using var connection = appDbContext.CreateConnection();

            // 2. SQL Sorgusu: Kategori Adı, Toplam Ciro, Toplam Sipariş Sayısı
            // Not: Products ve Categories tablolarının adlarını veritabanınıza göre kontrol edin.
            string query = @"
                SELECT 
                    c.CategoryName, 
                    SUM(o.TotalPrice) as TotalRevenue, 
                    COUNT(o.Id) as OrderCount
                FROM Orders o
                INNER JOIN Products p ON o.ProductId = p.ProductId
                INNER JOIN Categories c ON p.CategoryId = c.CategoryId
                GROUP BY c.CategoryName
                ORDER BY TotalRevenue DESC"; // Ciroya göre çoktan aza sırala

            var data = (await connection.QueryAsync(query)).ToList();

            // 3. Verileri Chart İçin Ayıklama (Dizilere Çevirme)
            ViewBag.Categories = data.Select(x => x.CategoryName).ToList();
            ViewBag.Revenues = data.Select(x => x.TotalRevenue).ToList();
            ViewBag.OrderCounts = data.Select(x => x.OrderCount).ToList();


            string query2 = @"
                SELECT 
                    c.CategoryName as name, 
                    COUNT(o.Id) as value
                FROM Orders o
                INNER JOIN Products p ON o.ProductId = p.ProductId
                INNER JOIN Categories c ON p.CategoryId = c.CategoryId
                GROUP BY c.CategoryName
                ORDER BY value DESC"; // En çok satılan en büyük dilim olsun

            var pieData = (await connection.QueryAsync(query2)).ToList();

            // View'a gönderiyoruz
            ViewBag.CategoryPieData = pieData;


            // 1. En Çok Sipariş Alan Kategori (Adı ve Sipariş Sayısı)
            string mostOrderedCatQuery = @"
    SELECT TOP 1 
        c.CategoryName, 
        COUNT(o.Id) as OrderCount
    FROM Orders o
    JOIN Products p ON o.ProductId = p.ProductId
    JOIN Categories c ON p.CategoryId = c.CategoryId
    GROUP BY c.CategoryName
    ORDER BY OrderCount DESC";
            var mostOrderedCat = await connection.QueryFirstOrDefaultAsync(mostOrderedCatQuery);

            // 2. En Az Sipariş Alan Kategori (Adı ve Sipariş Sayısı)
            string leastOrderedCatQuery = @"
    SELECT TOP 1 
        c.CategoryName, 
        COUNT(o.Id) as OrderCount
    FROM Orders o
    JOIN Products p ON o.ProductId = p.ProductId
    JOIN Categories c ON p.CategoryId = c.CategoryId
    GROUP BY c.CategoryName
    ORDER BY OrderCount ASC";
            var leastOrderedCat = await connection.QueryFirstOrDefaultAsync(leastOrderedCatQuery);

            // 3. En Yüksek Fiyatlı Ürün (Adı ve Fiyatı)
            string expensiveProductQuery = @"
    SELECT TOP 1 ProductName, Price 
    FROM Products 
    ORDER BY Price DESC";
            var expensiveProduct = await connection.QueryFirstOrDefaultAsync(expensiveProductQuery);

            // 4. En Ucuz Ürün (Adı ve Fiyatı)
            string cheapestProductQuery = @"
    SELECT TOP 1 ProductName, Price 
    FROM Products 
    ORDER BY Price ASC";
            var cheapestProduct = await connection.QueryFirstOrDefaultAsync(cheapestProductQuery);

            // Verileri ViewBag'e Atıyoruz
            ViewBag.Card_MostOrdered = mostOrderedCat;
            ViewBag.Card_LeastOrdered = leastOrderedCat;
            ViewBag.Card_Expensive = expensiveProduct;
            ViewBag.Card_Cheapest = cheapestProduct;




            string query3 = @"
    SELECT DISTINCT
        c.CategoryName,
        MAX(p.Price) OVER (PARTITION BY c.CategoryId) as MaxPrice,
        FIRST_VALUE(p.ProductName) OVER (PARTITION BY c.CategoryId ORDER BY p.Price DESC) as MaxProductName,
        
        MIN(p.Price) OVER (PARTITION BY c.CategoryId) as MinPrice,
        FIRST_VALUE(p.ProductName) OVER (PARTITION BY c.CategoryId ORDER BY p.Price ASC) as MinProductName,
        
        AVG(p.Price) OVER (PARTITION BY c.CategoryId) as AvgPrice
    FROM Products p
    JOIN Categories c ON p.CategoryId = c.CategoryId";

            // Ham veriyi alıyoruz
            var rawData = (await connection.QueryAsync<CategoryPriceStatsViewModel>(query3)).ToList();

            // ========================================================================
            // KRİTİK NOKTA: GRAFİĞİN DÜZGÜN DURMASI İÇİN SIRALAMA YAPIYORUZ
            // ========================================================================
            // En pahalı kategoriden en ucuza doğru sırala (MaxPrice'a göre)
            var data2 = rawData.OrderByDescending(x => x.MaxPrice).ToList();


            // --- CHART İÇİN AYRI AYRI DİZİLER OLUŞTURUYORUZ (Artık sıralı) ---

            // X Ekseni
            ViewBag.CatNames = data2.Select(x => x.CategoryName).ToList();

            // Seriler (Sayısal Veriler)
            ViewBag.MaxPrices = data2.Select(x => x.MaxPrice).ToList();
            ViewBag.MinPrices = data2.Select(x => x.MinPrice).ToList();
            ViewBag.AvgPrices = data2.Select(x => Math.Round(x.AvgPrice, 2)).ToList();

            // Tooltip İçin İsim Verileri
            ViewBag.MaxProductNames = data2.Select(x => x.MaxProductName).ToList();
            ViewBag.MinProductNames = data2.Select(x => x.MinProductName).ToList();


            string topCategoriesQuery = @"
    SELECT TOP 6
        c.CategoryId,
        c.CategoryName,
        COUNT(DISTINCT p.ProductId) as ProductCount,
        COUNT(o.Id) as OrderCount,
        ISNULL(SUM(o.TotalPrice), 0) as TotalRevenue
    FROM Categories c
    LEFT JOIN Products p ON c.CategoryId = p.CategoryId
    LEFT JOIN Orders o ON p.ProductId = o.ProductId
    GROUP BY c.CategoryId, c.CategoryName
    ORDER BY OrderCount DESC"; // Sipariş sayısına göre sırala

            var topCategories = (await connection.QueryAsync<CategoryStatsViewModel>(topCategoriesQuery)).ToList();

            ViewBag.TopCategoriesTable = topCategories;









            return View();
        }
    }
}