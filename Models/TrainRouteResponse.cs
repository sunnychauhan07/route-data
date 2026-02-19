namespace RouteDataApp.Models;

public sealed class TrainRouteResponse
{
    public string TrainNumber { get; init; } = string.Empty;
    public string TrainName { get; init; } = string.Empty;
    public LiveStatus? LiveStatus { get; init; }
    public IReadOnlyList<RouteStop> Stops { get; init; } = [];
}

public sealed class RouteStop
{
    public int Sequence { get; init; }
    public string StationCode { get; init; } = string.Empty;
    public string StationName { get; init; } = string.Empty;
    public string ArrivalTime { get; init; } = string.Empty;
    public string DepartureTime { get; init; } = string.Empty;
    public string HaltMinutes { get; init; } = string.Empty;
    public string DistanceKm { get; init; } = string.Empty;
    public string Platform { get; init; } = string.Empty;
}

public sealed class LiveStatus
{
    public string CurrentStation { get; init; } = string.Empty;
    public string LastUpdatedAt { get; init; } = string.Empty;
    public string DelayInfo { get; init; } = string.Empty;
    public string RunningStatus { get; init; } = string.Empty;
}
