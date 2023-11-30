# LibaryM

Bu proje, Kişisel olarak yaptığım, Kütüphane kitap ödünç verme süreci içindir.

## Gereksinimler

- .NET 6.0 SDK
- AutoMapper v12.0.1
- Microsoft.EntityFrameworkCore v7.0.14
- Serilog.Sinks.File v5.0.0
- Serilog.Sinks.MSSqlServer v6.4.0

## Kurulum

1. Repoyu klonlayın: `git clone https://github.com/bahriyeliiix/LibaryM.git`
2. appsettings.json dosyasında MsSql ConnectionString girin: `"DevConnection": "Server=yourServer;Database=yourDb;User Id=userID;Password=userPassword;TrustServerCertificate=true"`
3. PMConsole'a açın ve çalıştırın : `update-database`
4. Proje dizinine gidin: `cd LibaryM`
5. Uygulamayı başlatın: `dotnet run`

## Kullanım

Uygulamayı başlattıktan sonra Menü kısmında 'Books' menüsüne tıklayarak devam edebilirsiniz.

## Proje Yapısı

Bu proje, aşağıdaki alt projeleri içermektedir:

- Common
- DAL (Data Access Layer)
- Services

## Kullanılan Paketler ve Versiyonları

- AutoMapper v12.0.1
- Microsoft.EntityFrameworkCore v7.0.14
- Serilog.Sinks.File v5.0.0
- Serilog.Sinks.MSSqlServer v6.4.0

## İletişim

Eğer bir sorunuz veya öneriniz varsa, lütfen offical.furkanaydin@gmail.com adresinden bize ulaşın.
