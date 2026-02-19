using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RouteDataApp.Models;
using RouteDataApp.Services;

namespace RouteDataApp.Pages;

public sealed class IndexModel(INtesApiClient ntesApiClient, IExcelExportService excelExportService) : PageModel
{
    [BindProperty]
    public string TrainNumber { get; set; } = string.Empty;

    public TrainRouteResponse? RouteData { get; private set; }

    public string? ErrorMessage { get; private set; }

    public async Task OnGetAsync(string? trainNumber, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(trainNumber))
        {
            TrainNumber = trainNumber;
            await LoadRouteAsync(cancellationToken);
        }
    }

    public async Task<IActionResult> OnPostSearchAsync(CancellationToken cancellationToken)
    {
        await LoadRouteAsync(cancellationToken);
        return Page();
    }

    public async Task<IActionResult> OnPostExportAsync(CancellationToken cancellationToken)
    {
        await LoadRouteAsync(cancellationToken);
        if (RouteData is null)
        {
            return Page();
        }

        var file = excelExportService.BuildRouteWorkbook(RouteData);
        var safeTrainNumber = string.IsNullOrWhiteSpace(TrainNumber) ? "train" : TrainNumber;
        return File(file,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"{safeTrainNumber}-route.xlsx");
    }

    private async Task LoadRouteAsync(CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(TrainNumber))
        {
            ErrorMessage = "Please enter a valid train number.";
            RouteData = null;
            return;
        }

        try
        {
            RouteData = await ntesApiClient.GetTrainRouteAsync(TrainNumber.Trim(), cancellationToken);
            ErrorMessage = null;
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            RouteData = null;
        }
    }
}
