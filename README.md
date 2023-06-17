# Identity Service

The identity service is responsible for handling anything between-user interactions, including the searching of users and establishing a first contact by providing various public encryption keys as per the Signal Procotol.

> More information regarding the Signal Procotol exchange keys can be found [here](https://signal.org/docs/specifications/x3dh/#publishing-keys).

This service is part of the greater UIMA project communicated between an API gateway by desktop clients and has a direct connection to an event bus. This service  has its own database for storing keys and user data:

![Individual  UIMA C4 Model](https://github.com/UIMA-Messaging/identity-service/assets/56337726/29aba09b-839a-4ab4-bbc2-6b5a0c8adf9d)

## Configuration
You can configure environment variable in appsettings.json file in the project. This file should look like:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "Users": "POSTGRES_CONNECTION_STRING",
    "Keys": "POSTGRES_CONNECTION_STRING"
  },
  "Bugsnag": {
    "ApiKey": "BUGSNAG_API_KEY"
  },
  "RabbitMQ": {
    "Uri": "RABBITMQ_HOST",
    "Exchange": "THIS_EXCHANGE",
    "UserRegistrations": {
      "Exchange": "REGISTRATION_EXCHANGE",
      "RoutingKey": "REGISTRATION_ROUTING_KEY"
    }
  }
}
```

## Endpoints

The identity service has two controllers: one for handling the searching of users, and another, for the handling of exchange keys.

`GET users/username/{username}`

This endpoint returns one user given a `username`. If there are no exect match found, a 404 response is returned.

Return body:
```json
{
  "id": "string",
  "jid": "string",
  "displayName": "string",
  "username": "string",
  "image": "string",
  "joinedAt": "2023-06-17T12:15:55.707Z"
}
```

`GET users/search/{query}`

This endpoint returns a paginated list of users given a `query` fuzzy search. The search is performed in both the user's username as well as display names. An paginated result with an empty list is returned should no match be found.

|       **Parameter** | **Description**           | **Required** |
| ------------------: | ------------------------- | :----------: |
| `count` int         | Number of results to be returned, max 100, default 10.                |     false    |
| `offset` int        | 0 indexed page number, default 0                  |     false    |

Return body:
```json
{
  "previousPage": "string",
  "nextPage": "string",
  "results": [
    {
      "id": "string",
      "jid": "string",
      "displayName": "string",
      "username": "string",
      "image": "string",
      "joinedAt": "2023-06-17T12:21:03.042Z"
    }
  ]
}
```






