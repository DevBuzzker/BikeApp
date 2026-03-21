# ⚡ E-Bike Tracker

Blazor WebAssembly PWA zum Verfolgen von Akku, Kilometern und Wartung deines E-Bikes.

## Features

- 🔋 **Fahrrad** — Akkustand eingeben, Ladezeit berechnen (1% = 1 min 22 sek), Zielladung
- 📍 **Kilometer** — Fahrten protokollieren, Woche/Monat/Gesamt-Statistiken
- 🔧 **Wartung** — Ketten-, Reifen-, Inspektions-Intervalle mit Statusampel
- 📱 **PWA** — auf dem Handy installierbar, funktioniert offline
- 💾 Alle Daten lokal im Browser (localStorage), kein Server nötig

## Lokale Entwicklung

```bash
# Voraussetzung: .NET 8 SDK
dotnet restore
dotnet run
# → https://localhost:5001
```

## Deploy auf GitHub Pages

### 1. Repo erstellen & pushen
```bash
git init
git add .
git commit -m "Initial commit"
git remote add origin https://github.com/DEIN_NAME/DEIN_REPO.git
git push -u origin main
```

### 2. GitHub Pages aktivieren
- Gehe zu **Settings → Pages**
- Source: **GitHub Actions**
- Fertig — der Workflow baut und deployed automatisch!

### 3. App-URL
```
https://DEIN_NAME.github.io/DEIN_REPO/
```

### 4. App auf iPhone/Android installieren
- Safari/Chrome → die URL öffnen
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
│   └── Toast.razor        # Benachrichtigungs-Toast
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
