using RouteDataApp.Models;

namespace RouteDataApp.Services;

public interface INtesApiClient
{
    Task<TrainRouteResponse> GetTrainRouteAsync(string trainNumber, CancellationToken cancellationToken = default);
}
