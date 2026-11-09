# ⚡ E-Bike Tracker
Demo: https://devbuzzker.github.io/BikeApp/

Blazor WebAssembly PWA zum Verfolgen von Akku, Kilometern und Wartung deines E-Bikes. Entstanden in drei Tagen mit Claude Code.

## Features

- 🔋 **Fahrrad** — Akkustand eingeben, Ladezeit berechnen (1% = 1 min 22 sek), Zielladung
- 📍 **Kilometer** — Fahrten protokollieren, Woche/Monat/Gesamt-Statistiken
- 🔧 **Wartung** — Ketten-, Reifen-, Inspektions-Intervalle mit Statusampel
- 📱 **PWA** — auf dem Handy installierbar
- 💾 Alle Daten lokal im Browser (localStorage), kein Server nötig

## Lokale Entwicklung

```bash
# Voraussetzung: .NET 8 SDK
dotnet restore
dotnet run
# → https://localhost:5001
```

Deployment: Bei jedem Push auf `main` baut GitHub Actions die App und veröffentlicht sie auf GitHub Pages (`.github/workflows/deploy.yml`).

## Auf dem Handy installieren

- Safari/Chrome → die Demo-URL öffnen
- **Teilen → Zum Home-Bildschirm** (iOS) oder **Installieren** (Android)

## Projektstruktur

```
EBikeTracker/
├── Models/
│   └── Models.cs          # BatteryState, Trip, MaintenanceData, BikeInfo
├── Services/
│   ├── StorageService.cs  # localStorage via JS Interop
│   └── BikeStateService.cs # zentraler State + berechnete Werte
├── Pages/
│   ├── Fahrrad.razor      # Akku-Seite
│   ├── Kilometer.razor    # Fahrten-Seite
│   └── Wartung.razor      # Wartungs-Seite
├── Shared/
│   ├── MainLayout.razor   # Shell + Bottom-Navigation
│   ├── BatteryRing.razor  # SVG-Donut-Komponente
│   ├── WartungRow.razor   # Status-Zeile mit Badge
│   ├── Toast.razor        # Benachrichtigungs-Toast
│   └── Onboarding.razor   # Einführung beim ersten Start
└── wwwroot/
    ├── index.html
    ├── manifest.json      # PWA Manifest
    ├── service-worker.js
    └── css/app.css
```

## Ladezeit-Berechnung

```
1% Akku = 82 Sekunden Ladezeit
Formel: (Ziel% - Aktuell%) × 82 Sekunden
```
