# MarketPriceAPI

## Опис проєкту
API для отримання актуальних та історичних цін ринкових активів (валюти, акції, індекси) з використанням Fintacharts як провайдера даних.

---

## Особливості
- Підтримка реального часу через WebSocket API.  
- Історичні дані через REST API.  
- Зберігання даних у базі (SQLite / SQL Server).  
- RESTful API з чистою архітектурою.  
- Легко розширюється та конфігурується.

---

## Технічні вимоги
- .NET 7 або .NET 8 SDK  
- SQLite (або SQL Server)

---

## Установка та запуск

### Локальний запуск

1. Клонувати репозиторій:
   ```bash
   git clone <repository-url>
   cd MarketPriceAPI
    ```
2. Налаштувати конфігурацію у `appsettings.json`:
   - Вказати URL Fintacharts API
   - Вказати username та password для доступу до API
	- Вказати realm та clientId
3. Є можливість обрати базу даних, яка буде використовуватися, наприклад SQLite або SQL Server. Для цього потрібно змінити налаштування у `appsettings.json`:
   ```json
   "DatabaseProvider" :  "SQLite",
    "ConnectionStrings": {
    "SqlServer": "Server=localhost;Database=MarketPriceAPIDb;Integrated Security=true;TrustServerCertificate=True;",
    "SQLite": "Data Source=assets.db"
  },

4. Відновити пакети та запустити проєкт:
   ```bash
   dotnet restore
   dotnet run
   
5. API буде доступний за адресою https://localhost:8080

----

## API Ендпоінти
### Отримання усіх активів
GET /api/assets

Параметри(опційно):

provider — фільтр по провайдеру

kind — фільтр по типу активу

#### Приклад запиту:
https://localhost:8080/api/asset?provider=oanda&kind=forex

#### Відповідь:
```json
{
        "id": "054dc5aa-7d4e-45b5-abea-11cb24823ce4",
        "symbol": "CAD/JPY",
        "kind": "forex",
        "description": "CAD/JPY",
        "tickSize": 1E-05,
        "currency": "JPY",
        "baseCurrency": "CAD",
        "mappings": {
            "simulation": {
                "isPresent": true,
                "symbol": "CADJPY",
                "exchange": "",
                "defaultOrderSize": 10000,
                "maxOrderSize": 1000000,
                "tradingHours": {
                    "regularStart": "00:00:00",
                    "regularEnd": "00:00:00",
                    "electronicStart": "00:00:00",
                    "electronicEnd": "00:00:00"
                }
            },
            "oanda": {
                "isPresent": true,
                "symbol": "CAD_JPY",
                "exchange": "",
                "defaultOrderSize": 10000,
                "maxOrderSize": null,
                "tradingHours": {
                    "regularStart": "00:00:00",
                    "regularEnd": "00:00:00",
                    "electronicStart": "00:00:00",
                    "electronicEnd": "00:00:00"
                }
            }
        },
        "profile": {
            "name": "CAD/JPY",
            "gics": null
        }
    },
    ....
]
```

### Реальні ціни

https://localhost:8080/api/realtimeprices
Параметри:
instrumentId — ідентифікатор активу

Використовується WebSocket для отримання реальних цін. Дані зберігаються у базу даних та доступні через REST API.

#### Приклад запиту:
https://localhost:8080/api/realtimeprices?instrumentId=63d4a5b6-351f-41a0-969f-6e5bfcd32fcd
#### Відповідь:
```json
{
    "id": "5a07ac4e-a5c1-465d-ace8-30515250bbcd",
    "instrumentId": "63d4a5b6-351f-41a0-969f-6e5bfcd32fcd",
    "bid": 0.7539,
    "ask": 0.0,
    "last": 0.0,
    "symbol": "CAD/CHF",
    "description": "CAD/CHF",
    "timestamp": "2025-07-08T01:22:48.768578"
}
```

### Історичні ціни
GET /api/historicalPrices/latest-bars — отримати останні історичні бари

Параметри:

instrumentId (обов’язково)

provider, interval, periodicity, barsCount (опційно)

#### Приклад запиту:
https://localhost:8080/api/historicalPrices/latest-bars?instrumentId=63e9ee30-8391-48f6-b135-bcd651515f8a&barsCount=3&provider=simulation&interval=1&periodicity=day
#### Відповідь:
```json
[
    {
        "t": "2025-07-06T03:00:00+03:00",
        "o": 150.84,
        "h": 153.78,
        "l": 140.76,
        "c": 142.08,
        "v": 31989453
    },
    {
        "t": "2025-07-07T03:00:00+03:00",
        "o": 142.03,
        "h": 144.36,
        "l": 138.07,
        "c": 140.43,
        "v": 32028402
    },
    {
        "t": "2025-07-08T03:00:00+03:00",
        "o": 140.46,
        "h": 142.51,
        "l": 140.17,
        "c": 141.56,
        "v": 1847813
    }
]
```

### Історичні ціни за датами
GET /api/historicalPrices/date-range

Параметри:

instrumentId (обов’язково)

startDate, endDate

provider, interval, periodicity (опційно)

#### Приклад запиту:
https://localhost:8080/api/historicalPrices/date-range?instrumentId=ad9e5345-4c3b-41fc-9437-1d253f62db52&startDate=2025-01-01&provider=simulation&interval=1&periodicity=minute
#### Відповідь:
```json
[
    {
        "t": "2025-07-01T04:03:00+03:00",
        "o": 0.8957,
        "h": 0.8962,
        "l": 0.8957,
        "c": 0.8962,
        "v": 108652
    },
    {
        "t": "2025-07-01T04:04:00+03:00",
        "o": 0.8962,
        "h": 0.8963,
        "l": 0.8961,
        "c": 0.8961,
        "v": 186536
    },
    {
        "t": "2025-07-01T04:05:00+03:00",
        "o": 0.8962,
        "h": 0.8963,
        "l": 0.8957,
        "c": 0.8959,
        "v": 125993
    },
    {
        "t": "2025-07-01T04:06:00+03:00",
        "o": 0.8958,
        "h": 0.8962,
        "l": 0.8957,
        "c": 0.8957,
        "v": 119514
    },
    {
        "t": "2025-07-01T04:07:00+03:00",
        "o": 0.8957,
        "h": 0.8957,
        "l": 0.8952,
        "c": 0.8955,
        "v": 122903
    },
    ....
```

### Історичні ціни з часом назад
GET /api/historicalPrices/time-back

Параметри:

instrumentId (обов’язково)

timeBack (TimeSpan, напр. 00:30:00 для 30 хвилин)

provider, interval, periodicity (опційно)


#### Приклад запиту:
https://localhost:8080/api/historicalPrices/date-range?instrumentId=ad9e5345-4c3b-41fc-9437-1d253f62db52&provider=simulation&interval=1&periodicity=hour&timeBack=10.12:24:02
#### Відповідь:
```json
[
    {
        "t": "2025-06-08T04:00:00+03:00",
        "o": 0.8862,
        "h": 0.8890,
        "l": 0.8862,
        "c": 0.8881,
        "v": 7585320
    },
    {
        "t": "2025-06-08T05:00:00+03:00",
        "o": 0.8882,
        "h": 0.8920,
        "l": 0.8876,
        "c": 0.8893,
        "v": 7655685
    },
    {
        "t": "2025-06-08T06:00:00+03:00",
        "o": 0.8893,
        "h": 0.8930,
        "l": 0.8888,
        "c": 0.8926,
        "v": 7615586
    },
    {
        "t": "2025-06-08T07:00:00+03:00",
        "o": 0.8926,
        "h": 0.8937,
        "l": 0.8898,
        "c": 0.8920,
        "v": 7539940
    },
    {
        "t": "2025-06-08T08:00:00+03:00",
        "o": 0.8919,
        "h": 0.8940,
        "l": 0.8912,
        "c": 0.8930,
        "v": 7679951
    },
    {
        "t": "2025-06-08T09:00:00+03:00",
        "o": 0.8930,
        "h": 0.8945,
        "l": 0.8913,
        "c": 0.8923,
        "v": 7476686
    },
```

---
## Конфігурація
DatabaseProvider — вибір провайдера бази (SQLite або SqlServer)

ConnectionStrings — рядки підключення