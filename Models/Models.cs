namespace EBikeTracker.Models;

public class BatteryState
{
    public int? Percent { get; set; }
    public double? CapacityWh { get; set; }
    public double? RangeKm { get; set; } = 42;
    public DateTime? LastCharged { get; set; }

    // 1% = 1 min 22 sec = 82 seconds (editable)
    public int SecondsPerPercent { get; set; } = 82;
    public int TargetPercent { get; set; } = 100;

    public TimeSpan ChargeTimeToFull =>
        Percent.HasValue
            ? TimeSpan.FromSeconds((100 - Percent.Value) * SecondsPerPercent)
            : TimeSpan.Zero;

    public TimeSpan ChargeTimeTo(int targetPercent)
    {
        int from = Percent ?? 0;
        int diff = Math.Max(0, targetPercent - from);
        return TimeSpan.FromSeconds(diff * SecondsPerPercent);
    }

    public double? CurrentRangeKm =>
        Percent.HasValue && RangeKm.HasValue
            ? Math.Round(RangeKm.Value * Percent.Value / 100.0, 1)
            : null;

    public double? CurrentWh =>
        Percent.HasValue && CapacityWh.HasValue
            ? Math.Round(CapacityWh.Value * Percent.Value / 100.0, 0)
            : null;

    public string StatusColor => Percent switch
    {
        <= 20 => "#f85149",
        <= 50 => "#f0883e",
        _     => "#39d353"
    };
}

public class Trip
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public double Km { get; set; }
    public string Name { get; set; } = "";
    public DateTime Date { get; set; } = DateTime.Now;
    public int? BatteryUsedPercent { get; set; }
}

public class MaintenanceData
{
    public double CurrentKm { get; set; }
    public double? ChainChangeAtKm { get; set; }
    public double? InspectionAtKm { get; set; }
    public double? TireChangeAtKm { get; set; }
    public double? BatteryReplaceAtKm { get; set; }

    public MaintenanceStatus GetStatus(double? atKm)
    {
        if (!atKm.HasValue) return MaintenanceStatus.Unknown;
        double diff = atKm.Value - CurrentKm;
        if (diff <= 0) return MaintenanceStatus.Due;
        if (diff < 200) return MaintenanceStatus.Soon;
        return MaintenanceStatus.Ok;
    }
}

public enum MaintenanceStatus { Unknown, Ok, Soon, Due }

public class KilometerPrefs
{
    public double? LastKm { get; set; }
    public string LastName { get; set; } = "";
    public Dictionary<string, int> TagUsage { get; set; } = new();
    public List<string> Tags { get; set; } = ["Arbeit", "Einkaufen", "Spazierfahrt", "Sport", "Ausflug"];
}

public class BikeInfo
{
    public string Model { get; set; } = "";
    public DateTime? PurchaseDate { get; set; }
    public string SerialNumber { get; set; } = "";
}
