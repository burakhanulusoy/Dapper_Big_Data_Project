namespace Dapper_BigData.Models
{
    public class MapMarkerViewModel
    {
        public string Name { get; set; }     // Ülke Adı (Örn: Türkiye)
        public double[] Coords { get; set; } // Koordinatlar [Enlem, Boylam]
        public int Count { get; set; }       // Kişi Sayısı
    }
}
