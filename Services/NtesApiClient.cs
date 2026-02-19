using System.Net.Http.Json;
using RouteDataApp.Models;

namespace RouteDataApp.Services;

public sealed class NtesApiClient(HttpClient httpClient, IConfiguration configuration, ILogger<NtesApiClient> logger) : INtesApiClient
{
    private readonly Uri? _baseUri = TryCreateUri(configuration["NtesApi:BaseUrl"]);
    private readonly string _routePath = configuration["NtesApi:RoutePath"] ?? "/api/train/{trainNumber}/route";

    public async Task<TrainRouteResponse> GetTrainRouteAsync(string trainNumber, CancellationToken cancellationToken = default)
    {
        if (_baseUri is null)
        {
            logger.LogWarning("NtesApi:BaseUrl is missing. Returning demo data for train {TrainNumber}.", trainNumber);
            return BuildDemoData(trainNumber);
        }

        var routeUrl = BuildRouteUrl(trainNumber);
        using var response = await httpClient.GetAsync(routeUrl, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new HttpRequestException($"NTES API failed with {(int)response.StatusCode}: {body}");
        }

        var data = await response.Content.ReadFromJsonAsync<TrainRouteResponse>(cancellationToken: cancellationToken);
        return data ?? throw new InvalidOperationException("NTES API returned an empty payload.");
    }

    private string BuildRouteUrl(string trainNumber)
    {
        var path = _routePath.Replace("{trainNumber}", Uri.EscapeDataString(trainNumber), StringComparison.OrdinalIgnoreCase);
        var uri = new Uri(_baseUri!, path);
        return uri.ToString();
    }

    private static Uri? TryCreateUri(string? baseUrl) =>
        Uri.TryCreate(baseUrl, UriKind.Absolute, out var uri) ? uri : null;

    private static TrainRouteResponse BuildDemoData(string trainNumber) =>
        new()
        {
            TrainNumber = trainNumber,
            TrainName = "Rajdhani Express (Demo)",
            LiveStatus = new LiveStatus
            {
                CurrentStation = "Kanpur Central",
                RunningStatus = "Running Right Time",
                DelayInfo = "0 minutes",
                LastUpdatedAt = DateTimeOffset.Now.ToString("dd-MMM-yyyy HH:mm")
            },
            Stops =
            [
                new RouteStop { Sequence = 1, StationCode = "NDLS", StationName = "New Delhi", ArrivalTime = "Source", DepartureTime = "16:55", HaltMinutes = "-", DistanceKm = "0", Platform = "3" },
                new RouteStop { Sequence = 2, StationCode = "CNB", StationName = "Kanpur Central", ArrivalTime = "21:25", DepartureTime = "21:30", HaltMinutes = "5", DistanceKm = "440", Platform = "1" },
                new RouteStop { Sequence = 3, StationCode = "PRYJ", StationName = "Prayagraj Junction", ArrivalTime = "23:40", DepartureTime = "23:45", HaltMinutes = "5", DistanceKm = "635", Platform = "4" },
                new RouteStop { Sequence = 4, StationCode = "DDU", StationName = "Pt. Deen Dayal Upadhyaya Jn", ArrivalTime = "02:05", DepartureTime = "02:15", HaltMinutes = "10", DistanceKm = "788", Platform = "2" },
                new RouteStop { Sequence = 5, StationCode = "HWH", StationName = "Howrah Junction", ArrivalTime = "10:10", DepartureTime = "Destination", HaltMinutes = "-", DistanceKm = "1450", Platform = "9" }
            ]
        };
}
