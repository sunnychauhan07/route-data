using ClosedXML.Excel;
using RouteDataApp.Models;

namespace RouteDataApp.Services;

public sealed class ExcelExportService : IExcelExportService
{
    public byte[] BuildRouteWorkbook(TrainRouteResponse routeData)
    {
        using var workbook = new XLWorkbook();
        var sheet = workbook.Worksheets.Add("Route");

        sheet.Cell(1, 1).Value = "Train Number";
        sheet.Cell(1, 2).Value = routeData.TrainNumber;
        sheet.Cell(2, 1).Value = "Train Name";
        sheet.Cell(2, 2).Value = routeData.TrainName;

        sheet.Cell(4, 1).Value = "Seq";
        sheet.Cell(4, 2).Value = "Station Code";
        sheet.Cell(4, 3).Value = "Station Name";
        sheet.Cell(4, 4).Value = "Arrival";
        sheet.Cell(4, 5).Value = "Departure";
        sheet.Cell(4, 6).Value = "Halt (min)";
        sheet.Cell(4, 7).Value = "Distance (km)";
        sheet.Cell(4, 8).Value = "Platform";

        var header = sheet.Range(4, 1, 4, 8);
        header.Style.Font.Bold = true;
        header.Style.Fill.BackgroundColor = XLColor.LightGray;

        var row = 5;
        foreach (var stop in routeData.Stops)
        {
            sheet.Cell(row, 1).Value = stop.Sequence;
            sheet.Cell(row, 2).Value = stop.StationCode;
            sheet.Cell(row, 3).Value = stop.StationName;
            sheet.Cell(row, 4).Value = stop.ArrivalTime;
            sheet.Cell(row, 5).Value = stop.DepartureTime;
            sheet.Cell(row, 6).Value = stop.HaltMinutes;
            sheet.Cell(row, 7).Value = stop.DistanceKm;
            sheet.Cell(row, 8).Value = stop.Platform;
            row++;
        }

        if (routeData.LiveStatus is not null)
        {
            sheet.Cell(1, 4).Value = "Live Status";
            sheet.Cell(1, 5).Value = routeData.LiveStatus.RunningStatus;
            sheet.Cell(2, 4).Value = "Current Station";
            sheet.Cell(2, 5).Value = routeData.LiveStatus.CurrentStation;
            sheet.Cell(3, 4).Value = "Delay";
            sheet.Cell(3, 5).Value = routeData.LiveStatus.DelayInfo;
            sheet.Cell(3, 6).Value = "Updated";
            sheet.Cell(3, 7).Value = routeData.LiveStatus.LastUpdatedAt;
        }

        sheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}
