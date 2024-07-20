
# MeDirect_Currency_Exchange_API 

This is a project to simulate a currency exchange api. It uses Fixer.io as a currency provider connecting via API.
It stores the providers information into the Cache for 30min (configurable) and the same client can only do 10 trades in the last hour(configurable too).



## Tech Stack

**API:** .Net Core 8, RESTful

**Database:** SQLite

**Logging:** Serilog


## API Currency Exchange Request Workflow

![App Screenshot](https://gcdnb.pbrd.co/images/KYcHSp9jru3k.png?o=1)

## API Currency Exchange Database
![App Screenshot](https://gcdnb.pbrd.co/images/uKhTokBZLCXo.png?o=1)

# API Reference

# Client Controller

## Add Client

```http
  POST /api/Client/AddClient
```

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `clientRequest` | `ClientRequest` | **Required**. Client Request structure |

#### ClientRequest

| Name      | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `ID`      | `int`    | **Required**. Client Identification number |
| `Name`    | `string` | **Required**. Client Name |

## Get Client

```http
  GET /api/Client/GetClient
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `id_Client`| `int`   | **Required**. Id of the Client    |

### Return

| Name      | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `Client`| `client`   | Client Structure                  |

#### Client Variable

| Name      | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `ID`      | `int`    | **Required**. Client Identification number |
| `Name`    | `string` | **Required**. Client Name |
| `DT_Create`    | `DateTime` | **Required**. Date and time the Client was created |

# Currency Exchange Controller

## Get Rate

```http
  GET /api/CurrencyExchange/GetRate
```

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `fromCurrency` | `string` | **Required**.The currency code to exchange from (e.g., "EUR").  |
| `toCurrency` | `string` | **Required**. The currency code to exchange to (e.g., "USD"). |

### Return

| Type     | Description                       |
| :------- | :-------------------------------- |
| `decimal`   | rate of the exchange                  |

## ExchangeTrade

```http
  GET /api/CurrencyExchange/ExchangeTrade
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `tradeRequest`      | `TradeRequest` | **Required**. Trade request structure |

#### TradeRequest

| Name      | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `ID_Client`      | `int`    | **Required**. Client Identification number |
| `FromCurrency`    | `string` | **Required**. The currency code to exchange from (e.g., "EUR"). |
| `ToCurrency` | `string` | **Required**. The currency code to exchange to (e.g., "USD"). |
| `Amount` | `decimal` | **Required**. The Amount for the exchange. |

### Return OK (200)

| Name      | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `ID`      | `int`    | Trade ID |
| `ID_Client`    | `int` | Client ID. |
| `FromCurrency`    | `string` | The currency code to exchange from (e.g., "EUR"). |
| `ToCurrency` | `string` | The currency code to exchange to (e.g., "USD"). |
| `Amount` | `decimal` | The Amount for the exchange. |
| `Rate` | `decimal` | Rate used at that time. |
| `ExchangedAmount` | `decimal` | Amount exchanged. |

### Return Error

| Name      | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `type`| `string`   | type of return  example "Error"              |
| `title`| `string`   | title of the return example "Not Found"                  |
| `status`| `int`   | status code                  |
| `Errors`| `list<string>`   | list of error codes.                  |
