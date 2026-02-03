using Dapper;
using Dapper_BigData.Models;
using Kaira.WebUI.Context;
using Microsoft.AspNetCore.Mvc;

namespace Dapper_BigData.Controllers
{
    public class Dashboard3Controller(AppDbContext appDbContext) : Controller
    {
        public async Task<IActionResult> Index()
        {
            using var connection = appDbContext.CreateConnection();

            // =============================================================
            // 1. CHART: AYLIK SATIŞ PERFORMANSI (Total, Max, Min)
            // =============================================================
            string query = @"
                WITH MonthlyStats AS (
                    SELECT 
                        MONTH(o.OrderDate) as Month,
                        p.ProductName,
                        COUNT(o.Id) as OrderCount
                    FROM Orders o
                    JOIN Products p ON o.ProductId = p.ProductId
                    GROUP BY MONTH(o.OrderDate), p.ProductName
                ),
                RankedStats AS (
                    SELECT 
                        Month,
                        ProductName,
                        OrderCount,
                        SUM(OrderCount) OVER (PARTITION BY Month) as MonthTotal,
                        ROW_NUMBER() OVER (PARTITION BY Month ORDER BY OrderCount DESC) as RnMax,
                        ROW_NUMBER() OVER (PARTITION BY Month ORDER BY OrderCount ASC) as RnMin
                    FROM MonthlyStats
                )
                SELECT 
                    Month,
                    MAX(MonthTotal) as TotalSales,
                    MAX(CASE WHEN RnMax = 1 THEN OrderCount END) as MaxSalesCount,
                    MAX(CASE WHEN RnMax = 1 THEN ProductName END) as MaxProductName,
                    MAX(CASE WHEN RnMin = 1 THEN OrderCount END) as MinSalesCount,
                    MAX(CASE WHEN RnMin = 1 THEN ProductName END) as MinProductName
                FROM RankedStats
                GROUP BY Month
                ORDER BY Month";

            var data = (await connection.QueryAsync<MonthlySalesStatsViewModel>(query)).ToList();

            var totalSales = new int[12];
            var maxSales = new int[12];
            var minSales = new int[12];
            var maxNames = new string[12];
            var minNames = new string[12];

            foreach (var item in data)
            {
                int index = item.Month - 1;
                if (index >= 0 && index < 12)
                {
                    totalSales[index] = item.TotalSales;
                    maxSales[index] = item.MaxSalesCount;
                    minSales[index] = item.MinSalesCount;
                    maxNames[index] = item.MaxProductName;
                    minNames[index] = item.MinProductName;
                }
            }

            ViewBag.SeriesTotal = totalSales;
            ViewBag.SeriesMax = maxSales;
            ViewBag.SeriesMin = minSales;
            ViewBag.NamesMax = maxNames;
            ViewBag.NamesMin = minNames;


            // =============================================================
            // 2. CHART: ÜRÜN ÇEŞİTLİLİĞİ & ÖDEME YÖNTEMİ
            // =============================================================
            string query3 = @"
                WITH ProductStats AS (
                    -- 1. Her ay kaç farklı ürün satıldı?
                    SELECT 
                        MONTH(OrderDate) as Month,
                        COUNT(DISTINCT ProductId) as UniqueProductCount
                    FROM Orders
                    GROUP BY MONTH(OrderDate)
                ),
                PaymentStatsRaw AS (
                    -- 2. Her ay hangi ödeme yöntemi kaç kez kullanıldı?
                    SELECT 
                        MONTH(OrderDate) as Month,
                        PaymentMethod,
                        COUNT(*) as UsageCount
                    FROM Orders
                    GROUP BY MONTH(OrderDate), PaymentMethod
                ),
                PaymentStatsRanked AS (
                    -- 3. Ödeme yöntemlerini sırala
                    SELECT 
                        Month,
                        PaymentMethod,
                        ROW_NUMBER() OVER(PARTITION BY Month ORDER BY UsageCount DESC) as Rn
                    FROM PaymentStatsRaw
                )
                -- 4. Birleştir
                SELECT 
                    ps.Month,
                    ps.UniqueProductCount,
                    pay.PaymentMethod as TopPaymentMethod
                FROM ProductStats ps
                LEFT JOIN PaymentStatsRanked pay ON ps.Month = pay.Month AND pay.Rn = 1
                ORDER BY ps.Month";

            // --- HATA BURADAYDI: query yerine query3 Yazdık ---
            var data3 = (await connection.QueryAsync<MonthlyProductStatsViewModel>(query3)).ToList();

            var productCounts = new int[12];
            var paymentMethods = new string[12];

            foreach (var item in data3)
            {
                int index = item.Month - 1;
                if (index >= 0 && index < 12)
                {
                    productCounts[index] = item.UniqueProductCount;
                    paymentMethods[index] = item.TopPaymentMethod;
                }
            }

            ViewBag.ProductCounts = productCounts;
            ViewBag.PaymentMethods = paymentMethods;




            // SQL Notu: DATEPART(WEEKDAY, OrderDate) varsayılan ayarlarda 1=Pazar, 7=Cumartesi döner.
            string queryRadar = @"
    SELECT 
        DATEPART(WEEKDAY, OrderDate) as DayNumber, 
        COUNT(Id) as OrderCount
    FROM Orders
    GROUP BY DATEPART(WEEKDAY, OrderDate)
    ORDER BY DayNumber";

            var radarRawData = (await connection.QueryAsync<DayOfWeekStatsViewModel>(queryRadar)).ToList();

            // Chart için 7 günlük boş dizi (Pazar'dan Cumartesi'ye)
            var radarData = new int[7];

            foreach (var item in radarRawData)
            {
                // SQL'de 1=Pazar gelir, Dizide 0. index Pazar olsun istiyoruz.
                // Bu yüzden (DayNumber - 1) yapıyoruz.
                int index = item.DayNumber - 1;

                if (index >= 0 && index < 7)
                {
                    radarData[index] = item.OrderCount;
                }
            }

            ViewBag.RadarData = radarData;

            // 1. Kutu (Box) Verileri: Her kategori için fiyat dağılımı
            string queryBox = @"
        SELECT DISTINCT
            c.CategoryName,
            MIN(p.Price) OVER (PARTITION BY c.CategoryName) as MinPrice,
            PERCENTILE_CONT(0.25) WITHIN GROUP (ORDER BY p.Price) OVER (PARTITION BY c.CategoryName) as Q1,
            PERCENTILE_CONT(0.50) WITHIN GROUP (ORDER BY p.Price) OVER (PARTITION BY c.CategoryName) as Median,
            PERCENTILE_CONT(0.75) WITHIN GROUP (ORDER BY p.Price) OVER (PARTITION BY c.CategoryName) as Q3,
            MAX(p.Price) OVER (PARTITION BY c.CategoryName) as MaxPrice
        FROM Products p
        JOIN Categories c ON p.CategoryId = c.CategoryId
        WHERE p.Price > 0"; // Fiyatı 0 olanları eleyelim

            var boxDataRaw = (await connection.QueryAsync<CategoryBoxPlotViewModel>(queryBox)).ToList();

            // 2. Aykırı Değerler (Outliers):
            // Her kategorinin en pahalı 3 ürününü 'Aykırı' olarak işaretleyelim ki grafikte nokta olarak görünsün.
            string queryOutliers = @"
        SELECT * FROM (
            SELECT 
                c.CategoryName, 
                p.Price,
                ROW_NUMBER() OVER(PARTITION BY c.CategoryName ORDER BY p.Price DESC) as rn
            FROM Products p
            JOIN Categories c ON p.CategoryId = c.CategoryId
        ) t
        WHERE t.rn <= 3"; // Her kategoriden en pahalı 3 ürün

            var outlierRaw = (await connection.QueryAsync<CategoryOutlierViewModel>(queryOutliers)).ToList();

            // --- CHART İÇİN VERİ HAZIRLIĞI ---
            var boxPlotSeries = new List<object>();
            var outlierSeries = new List<object>();

            // Kategorileri tek tek gezerek chart formatına çevir
            foreach (var item in boxDataRaw)
            {
                // Kutu Verisi
                boxPlotSeries.Add(new
                {
                    x = item.CategoryName,
                    y = new[] { (int)item.MinPrice, (int)item.Q1, (int)item.Median, (int)item.Q3, (int)item.MaxPrice }
                });

                // O kategoriye ait outlier'ları bul
                var outliers = outlierRaw.Where(o => o.CategoryName == item.CategoryName).ToList();
                foreach (var o in outliers)
                {
                    // Eğer outlier değeri, Q3'ten çok uzaksa çizelim (Grafik karışmasın diye filtreliyoruz)
                    if (o.Price > item.Q3 * 1.2m)
                    {
                        outlierSeries.Add(new
                        {
                            x = item.CategoryName,
                            y = (int)o.Price
                        });
                    }
                }
            }

            ViewBag.BoxPlotData = boxPlotSeries;
            ViewBag.OutlierData = outlierSeries;

            string queryTreemap = @"
    SELECT 
        Country,
        COUNT(CustomerId) as CustomerCount
    FROM Customers
    WHERE Country IS NOT NULL AND Country <> ''
    GROUP BY Country
    ORDER BY CustomerCount DESC";

            var countryData = (await connection.QueryAsync<CountryStatsViewModel>(queryTreemap)).ToList();

            // ApexCharts Treemap formatı: { x: "İsim", y: Değer }
            var treemapSeries = new List<object>();

            foreach (var item in countryData)
            {
                treemapSeries.Add(new
                {
                    x = item.Country,
                    y = item.CustomerCount
                });
            }

            // Veriyi ViewBag ile gönderiyoruz
            ViewBag.CountryTreemapData = treemapSeries;



            string queryPerformance = @"
    WITH MonthlyCustomerStats AS (
        SELECT 
            MONTH(o.OrderDate) as Month,
            c.CustomerName + ' ' + c.CustomerSurname as FullName,
            SUM(o.Quantity) as TotalQty
        FROM Orders o
        JOIN Customers c ON o.CustomerId = c.CustomerId
        GROUP BY MONTH(o.OrderDate), c.CustomerName, c.CustomerSurname
    ),
    RankedStats AS (
        SELECT 
            Month, FullName, TotalQty,
            ROW_NUMBER() OVER (PARTITION BY Month ORDER BY TotalQty DESC) as RnMax,
            ROW_NUMBER() OVER (PARTITION BY Month ORDER BY TotalQty ASC) as RnMin
        FROM MonthlyCustomerStats
    )
    SELECT 
        Month,
        MAX(CASE WHEN RnMax = 1 THEN TotalQty END) as MaxQuantity,
        MAX(CASE WHEN RnMax = 1 THEN FullName END) as MaxCustomerName,
        MAX(CASE WHEN RnMin = 1 THEN TotalQty END) as MinQuantity,
        MAX(CASE WHEN RnMin = 1 THEN FullName END) as MinCustomerName
    FROM RankedStats
    GROUP BY Month ORDER BY Month";

            var performanceData = (await connection.QueryAsync<CustomerPerformanceViewModel>(queryPerformance)).ToList();

            // Chart için 12 aylık hazırlık
            var rangeSeries = new List<object>();
            var maxLineSeries = new List<object>();
            var minLineSeries = new List<object>();
            var maxCustomerNames = new string[12];
            var minCustomerNames = new string[12];

            for (int i = 1; i <= 12; i++)
            {
                var dataPoint = performanceData.FirstOrDefault(x => x.Month == i);
                string monthName = System.Globalization.CultureInfo.GetCultureInfo("tr-TR").DateTimeFormat.GetAbbreviatedMonthName(i);

                if (dataPoint != null)
                {
                    // Range Area: [Min, Max]
                    rangeSeries.Add(new { x = monthName, y = new[] { dataPoint.MinQuantity, dataPoint.MaxQuantity } });
                    // Çizgiler için tekil değerler
                    maxLineSeries.Add(new { x = monthName, y = dataPoint.MaxQuantity });
                    minLineSeries.Add(new { x = monthName, y = dataPoint.MinQuantity });
                    // İsimler (Tooltip için)
                    maxCustomerNames[i - 1] = dataPoint.MaxCustomerName;
                    minCustomerNames[i - 1] = dataPoint.MinCustomerName;
                }
            }

            ViewBag.RangeData = rangeSeries;
            ViewBag.MaxLineData = maxLineSeries;
            ViewBag.MinLineData = minLineSeries;
            ViewBag.MaxNames = maxCustomerNames;
            ViewBag.MinNames = minCustomerNames;

            string queryStats = @"
    SELECT 
        (SELECT COUNT(*) FROM Customers) as TotalCustomers,
        (SELECT COUNT(*) FROM Orders) as TotalOrders,
        (SELECT COUNT(*) FROM Orders WHERE Status = 'Teslim Edildi') as DeliveredOrders,
        (SELECT COUNT(*) FROM Orders WHERE Status <> 'Teslim Edildi') as NotDeliveredOrders";

            // Tek satır geleceği için QueryFirstOrDefaultAsync kullanıyoruz
            var stats = await connection.QueryFirstOrDefaultAsync<DashboardStatsViewModel>(queryStats);

            // Eğer veri gelmezse hata vermemesi için new'leyelim
            if (stats == null) stats = new DashboardStatsViewModel();

            ViewBag.Stats = stats;


            return View();
        }
    }
}