# 🚀 BigData Analytics Dashboard with .NET 9 & Dapper

<div align="center">

![.NET 9](https://img.shields.io/badge/.NET-9.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Dapper](https://img.shields.io/badge/ORM-Dapper-EA2027?style=for-the-badge)
![MSSQL](https://img.shields.io/badge/Database-MSSQL-A4B0BE?style=for-the-badge&logo=microsoft-sql-server&logoColor=black)
![Python](https://img.shields.io/badge/Data_Gen-Python-3776AB?style=for-the-badge&logo=python&logoColor=white)
![Big Data](https://img.shields.io/badge/Data-1.5M%20Rows-1B1464?style=for-the-badge)

<br />

**1.5 Milyon satırlık devasa bir e-ticaret veri setini analiz eden, yöneten ve görselleştiren yüksek performanslı Dashboard uygulaması.**

</div>

---

## 📖 Proje Hakkında

Bu proje, gerçek hayat senaryolarına (Business Logic) uygun olarak **Python** ile simüle edilmiş, **MSSQL** veritabanına aktarılmış ve **.NET 9.0 + Dapper** mimarisi kullanılarak milisaniyeler içinde sorgulanmış bir Big Data çözümüdür.

### 🎯 Temel Özellikler

| Özellik | Açıklama |
| :--- | :--- |
| **⚡ Yüksek Performans** | Klasik EF Core yerine **Dapper mikro-ORM** kullanılarak maksimum sorgu hızı ve minimum bellek kullanımı. |
| **📊 İleri Düzey Görselleştirme** | Veri akışı, satış analizleri ve müşteri performanslarının **ApexCharts/Chart.js** ile dinamik sunumu. |
| **🤖 AI Destekli Veri** | Google Colab üzerinde çalışan özel Python scripti ile üretilmiş **1.5 Milyon** satırlık tutarlı ve ilişkisel veri seti. |
| **📦 Modüler Mimari** | **.NET 9.0 MVC**, Repository Pattern ve SOLID prensiplerine uygun temiz kod yapısı. |

---

## 📊 Ekran Görüntüleri

<table>
  <tr>
    <td align="center">
      <img src="https://via.placeholder.com/800x450?text=Dashboard+Genel+Bakis" alt="Dashboard" width="100%" />
      <br /><em>Dashboard Genel Bakış</em>
    </td>
  </tr>
  <tr>
    <td align="center">
      <img src="https://via.placeholder.com/800x450?text=Veri+Analiz+Grafikleri" alt="Charts" width="100%" />
      <br /><em>Detaylı Veri Analiz Grafikleri</em>
    </td>
  </tr>
</table>

---

## 💡 Neden Dapper?

1.5 Milyon satırlık bir tabloda Entity Framework Core (EF Core) ile yapılan sorgular, özellikle karmaşık JOIN işlemlerinde, Aggregation (SUM, COUNT) sorgularında ve "Read-Only" senaryolarda bellek yönetimi (Change Tracking) nedeniyle yavaş kalabilmektedir.

Bu projede **Dapper** kullanarak şunları sağladık:

> * ✅ **Raw SQL Gücü:** SQL sorgularını doğrudan ve optimize edilmiş şekilde çalıştırdık.
> * ✅ **Düşük Maliyet:** Object-Relational Mapping (ORM) maliyetini minimuma indirdik.
> * ✅ **Anlık Raporlama:** Büyük veri setlerinde dashboard performansını maksimize ettik.

---









## 🛠️ Kurulum ve Veri Seti Oluşturma (Adım Adım)

Bu proje veritabanı odaklıdır. Uygulamayı ayağa kaldırmadan önce aşağıdaki adımları takip ederek veri setini oluşturmanız gerekmektedir.

### Adım 1: Veri Setinin Oluşturulması (Python)

Veri seti rastgele "lorem ipsum" verilerinden değil, belirli kategori, marka ve fiyat kurallarına göre oluşturulmuştur.

Aşağıdaki Python kodunu **Google Colab** veya lokal Python ortamınızda çalıştırarak `.csv` dosyalarını üretin.
> **Not:** Bu script `Final_Products.csv` ve 15 parça halinde `Final_Orders_Part_X.csv` dosyaları üretecektir.

```python
import csv
import random
from datetime import datetime, timedelta
import time
# Google Colab kullanıyorsanız bu satırı açın:
# from google.colab import files

# --- KESİN AYARLAR ---
PRODUCT_COUNT = 1000       # 1.000 Çeşit Kaliteli Ürün
ORDER_PARTS = 15           # 15 Dosya
ORDERS_PER_PART = 100000   # Her dosyada 100.000 Sipariş (Toplam 1.5 Milyon)
CUSTOMER_COUNT = 1000      # Müşteri ID: 1 ile 1000 arası

# --- KATEGORİ VE ÜRÜN KURALLARI ---
category_rules = {
    1: {"items": [("Apple", "MacBook Air M2", 35000, 50000), ("Samsung", "Galaxy S23", 40000, 60000)], "desc": "Teknoloji"},
    2: {"items": [("Mavi", "Black Pro Jeans", 800, 1500), ("Zara", "T-Shirt", 300, 800)], "desc": "Giyim"},
    3: {"items": [("Karaca", "Tencere Seti", 2000, 5000), ("IKEA", "Kitaplık", 1500, 4000)], "desc": "Ev"},
    # ... (Diğer kategoriler scriptin tamamında mevcuttur)
    15: {"items": [("Omron", "Tansiyon Aleti", 800, 2000), ("Oral-B", "Diş Fırçası", 600, 1500)], "desc": "Sağlık"}
}

statuses = ["Teslim Edildi", "Kargoda", "Hazırlanıyor", "İptal Edildi"]

def generate_perfect_data():
    print("🛠️ ADIM 1: Ürünler (Products) oluşturuluyor...")
    # ... (Ürün oluşturma mantığı) ...
    # Tam kod 'DataGenerator.py' dosyasındadır.

    print("🚀 ADIM 2: Siparişler (Orders) oluşturuluyor...")
    # ... (Sipariş oluşturma mantığı - 1.5 Milyon döngü) ...
            
    print(f"📦 Dosyalar oluşturuldu.")

if __name__ == "__main__":
    generate_perfect_data()
