using EBikeTracker.Models;

namespace EBikeTracker.Services;

/// <summary>
/// Central state store. Pages call LoadAsync on init,
/// then mutate + call SaveAsync. StateChanged fires UI refresh.
/// </summary>
public class BikeStateService(StorageService storage)
{
    public BatteryState Battery { get; private set; } = new();
    public List<Trip> Trips { get; private set; } = [];
    public MaintenanceData Maintenance { get; private set; } = new();
    public BikeInfo BikeInfo { get; private set; } = new();
    public KilometerPrefs KmPrefs { get; private set; } = new();

    public event Action? StateChanged;

    public async Task LoadAsync()
    {
        Battery     = await storage.GetAsync<BatteryState>("ebike_battery") ?? new();
        Trips       = await storage.GetAsync<List<Trip>>("ebike_trips")     ?? [];
        Maintenance = await storage.GetAsync<MaintenanceData>("ebike_maint")  ?? new();
        BikeInfo    = await storage.GetAsync<BikeInfo>("ebike_info")          ?? new();
        KmPrefs     = await storage.GetAsync<KilometerPrefs>("ebike_kmprefs") ?? new();
        StateChanged?.Invoke();
    }

    public async Task SaveBatteryAsync()
    {
        await storage.SetAsync("ebike_battery", Battery);
        StateChanged?.Invoke();
    }

    public async Task SaveTripsAsync()
    {
        await storage.SetAsync("ebike_trips", Trips);
        StateChanged?.Invoke();
    }

    public async Task SaveMaintenanceAsync()
    {
        await storage.SetAsync("ebike_maint", Maintenance);
        StateChanged?.Invoke();
    }

    public async Task SaveBikeInfoAsync()
    {
        await storage.SetAsync("ebike_info", BikeInfo);
        StateChanged?.Invoke();
    }

    public async Task SaveKmPrefsAsync()
    {
        await storage.SetAsync("ebike_kmprefs", KmPrefs);
        StateChanged?.Invoke();
    }

    public async Task ResetAsync()
    {
        await storage.RemoveAsync("ebike_battery");
        await storage.RemoveAsync("ebike_trips");
        await storage.RemoveAsync("ebike_maint");
        await storage.RemoveAsync("ebike_info");
        await storage.RemoveAsync("ebike_kmprefs");
        Battery     = new();
        Trips       = [];
        Maintenance = new();
        BikeInfo    = new();
        KmPrefs     = new();
        StateChanged?.Invoke();
    }

    public void AddTrip(Trip trip)   => Trips.Insert(0, trip);
    public void RemoveTrip(Guid id)  => Trips.RemoveAll(t => t.Id == id);
    public void ClearTrips()         => Trips.Clear();

    // ── Computed stats ──────────────────────────────────────────────────────
    public double TotalKm      => Trips.Sum(t => t.Km);
    public double WeekKm       => Trips.Where(t => t.Date >= StartOfWeek).Sum(t => t.Km);
    public double MonthKm      => Trips.Where(t => t.Date >= StartOfMonth).Sum(t => t.Km);
    public double AvgKm        => Trips.Count > 0 ? TotalKm / Trips.Count : 0;

    private static DateTime StartOfWeek
    {
        get
        {
            var now = DateTime.Today;
            int diff = (7 + (now.DayOfWeek - DayOfWeek.Monday)) % 7;
            return now.AddDays(-diff);
        }
    }
    private static DateTime StartOfMonth => new(DateTime.Today.Year, DateTime.Today.Month, 1);
}
