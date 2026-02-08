# ToDo API - Onion Architecture & JWT Security

Bu proje, modern yazılım prensipleri ve **Onion Architecture** (Soğan Mimarisi) kullanılarak geliştirilmiş kapsamlı bir ToDo yönetim sistemidir.

## 🛠 Teknik Özellikler
- **Mimari:** Onion Architecture (Domain, Application, Infrastructure, WebAPI)
- **Güvenlik:** JWT Authentication, BCrypt Password Hashing
- **Veri Erişimi:** Entity Framework Core & PostgreSQL
- **Hata Yönetimi:** Global Exception Handling (Middleware)
- **Validasyon:** FluentValidation
- **Mapping:** AutoMapper

## 🚀 Öne Çıkan Özellikler
- **Kullanıcı İzolasyonu:** Her kullanıcı yalnızca kendi verilerine erişebilir.
- **Merkezi Hata Yakalama:** Tüm hatalar tek bir merkezden yönetilir ve standart JSON döner.
- **Clean Code:** SOLID prensiplerine uygun, okunabilir ve geliştirilebilir kod yapısı.

## 📦 Kurulum
1. `git clone https://github.com/serhad-e/ToDoList.git`
2. `appsettings.json` içindeki bağlantı dizesini güncelleyin.
3. `dotnet ef database update` ile veritabanını oluşturun.
4. `dotnet run` ile çalıştırın.
