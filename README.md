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

<div align="center">
  <img src="https://github.com/user-attachments/assets/6a874ba4-2c16-4f16-84df-e0fbac217a58" width="100%" alt="Dashboard Genel Bakış" />
  <p><em>🚀 BigData Analytics Dashboard - Genel Bakış</em></p>
</div>

<br />

<details>
<summary>
  <h2 style="display:inline-block">✨ 📸 Detaylı Analizler, Grafikler ve Tabloları Görmek İçin Tıklayın ✨</h2>
</summary>
<br>

<div align="center">
  <h3>📉 Siparişlere Göre İstatistikler</h3>
  <img src="https://github.com/user-attachments/assets/82798c16-b301-4520-ad1f-f37141722c1b" width="100%" />
  <br /><br />
  <img src="https://github.com/user-attachments/assets/4f075d33-f429-470f-a77b-d4c84eb729f9" width="100%" />
</div>

<hr />

<div align="center">
  <h3>📊 Kategorilere Göre İstatistikler</h3>
  <img src="https://github.com/user-attachments/assets/9b0a079c-080b-4a1a-afcc-e7b965d7a320" width="100%" />
  <br /><br />
  <img src="https://github.com/user-attachments/assets/84896625-9646-48bb-a174-221ca49921ad" width="100%" />
</div>

<hr />

<div align="center">
  <h3>📦 Ürünlere ve Müşterilere Göre İstatistikler</h3>
  <img src="https://github.com/user-attachments/assets/03d0cd96-a54e-4290-8f77-99a81df2c1dd" width="100%" />
  <br /><br />
  <img src="https://github.com/user-attachments/assets/1c265009-a408-4244-b54a-afee035de7d3" width="100%" />
</div>

<hr />

<div align="center">
  <h3>📋 Detaylı Listeler ve Raporlar</h3>
  <img src="https://github.com/user-attachments/assets/213a31f0-1678-40a0-a39e-20d90766b933" width="100%" />
  <br /><br />
  <img src="https://github.com/user-attachments/assets/07eade7c-54cf-4a25-843d-b1bf39968fbd" width="100%" />
</div>

<hr />

<div align="center">
  <h3></h3>
  <img src="https://github.com/user-attachments/assets/96455b31-3647-4f31-8735-15513c60f014" width="100%" />
  <br /><br />
  <img src="https://github.com/user-attachments/assets/3170af54-901a-463b-a77f-1c8fb33e86f9" width="100%" />
</div>

<hr />

<div align="center">
  <h3>💻 Kod Yapısı ve Mimari</h3>
  <img src="https://github.com/user-attachments/assets/cfefa8ae-6727-41fd-bc33-5c72adc3a191" width="100%" />
  <br /><br />
  <img src="https://github.com/user-attachments/assets/6fc715d6-20f3-4b38-a530-9400c4e0ae2d" width="100%" />
  <br /><br />
  <img src="https://github.com/user-attachments/assets/4e47872d-4959-486c-839a-df7c03fdbe18" width="100%" />
</div>

<br />
</details>

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
