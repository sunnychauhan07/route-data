using RouteDataApp.Models;

namespace RouteDataApp.Services;

public interface IExcelExportService
{
    byte[] BuildRouteWorkbook(TrainRouteResponse routeData);
}
