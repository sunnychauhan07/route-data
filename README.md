# RouteDataApp (.NET Razor Pages)

This project is a C#/.NET web app that includes:
- Route lookup by entering a train number (NTES API integration-ready)
- Live status display for the selected train
- Excel export of full route data including stations, timings, halt, distance, and platform

## Run locally

```bash
dotnet restore
dotnet run
```

> If `NtesApi:BaseUrl` is empty, the app returns demo data.

## Configure NTES API

Set these values in `appsettings.json`:

```json
"NtesApi": {
  "BaseUrl": "https://your-ntes-host",
  "RoutePath": "/api/train/{trainNumber}/route"
}
```

The `{trainNumber}` placeholder in `RoutePath` is replaced automatically.

## Expected API response (JSON)

```json
{
  "trainNumber": "12301",
  "trainName": "Rajdhani Express",
  "liveStatus": {
    "currentStation": "Kanpur Central",
    "lastUpdatedAt": "17-Feb-2026 20:10",
    "delayInfo": "10 min",
    "runningStatus": "Running Late"
  },
  "stops": [
    {
      "sequence": 1,
      "stationCode": "NDLS",
      "stationName": "New Delhi",
      "arrivalTime": "Source",
      "departureTime": "16:55",
      "haltMinutes": "-",
      "distanceKm": "0",
      "platform": "3"
    }
  ]
}
```
