# RiotDotNet Projektdokumentation

## Projektübersicht
RiotDotNet ist ein komplexes System zur Interaktion mit dem League of Legends Client und der Riot Games API. Es verwendet eine Event-Sourcing-Architektur basierend auf BE.CQRS und bietet verschiedene Clients für unterschiedliche Anwendungsfälle.

## Projektstruktur

### BE.Riot.Events
- **Beschreibung**: Event-Definitions-Projekt für das BE.CQRS Event-Sourcing-System
- **Hauptfunktionen**:
  - Definition von Domain Events
  - Serialisierungsunterstützung für BE.CQRS
- **Wichtige Klassen**:
  - `PlayerMatchHistoryCreated`
  - `SummonerMatchesRead`

### BE.Riot.EventSource
- **Beschreibung**: Kern-Implementierung des Event-Sourcing-Systems
- **Hauptkomponenten**:
  - Bootstrap-Mechanismen für CQRS
  - Denormalizer für Match-Historien
  - Domain-Objekt-Definitionen
- **Untermodule**:
  - Bootstrap: System-Initialisierung
  - Denormalizer: Event-Projektion
  - Domain Objects: Aggregat-Definitionen

### BE.Riot.Domain
- **Beschreibung**: Zentrale Domain-Logik und Geschäftsregeln
- **Hauptbereiche**:
  - HTTP-Schnittstellen zur Riot Games API
  - Match-History-Verwaltung
  - Summoner-Verwaltung
- **Wichtige Interfaces**:
  - `IRiotClient`
  - `ICompletedGameDetailCache`
  - `ISummonerMatchHistoryIdsGateway`

### BE.Riot.LeagueDesktop.Client
- **Beschreibung**: Client für die Interaktion mit dem lokalen League of Legends Client
- **Hauptfunktionen**:
  - Event-Polling vom League Client
  - Realtime-Daten-Integration
- **Komponenten**:
  - Events: Event-Definitionen und Basis-Klassen
  - Infrastructure: Hilfsklassen für Client-Interaktion
  - Models: DTOs für League Client Daten

### BE.Riot.Console
- **Beschreibung**: Kommandozeilen-Interface für System-Interaktionen
- **Funktionen**:
  - Match-History-Updates triggern
  - System-Verwaltung
- **Technologien**:
  - Spectre.Console für CLI
  - System.CommandLine für Kommando-Parsing

### LeagueConnectClient
- **Beschreibung**: Beispiel-Implementierung für League Client Integration
- **Hauptfunktion**:
  - Demonstration der Event-Verarbeitung
  - Beispiel für Client-Integration

### BE.Riot.Mongo (Gateway)
- **Beschreibung**: MongoDB-Implementierung für Datenpersistenz
- **Funktionen**:
  - Cache für Spiel-Details
  - Summoner Match-History Speicherung

## Technologie-Stack
- **.NET 9.0**: Basis-Framework
- **BE.CQRS**: Event-Sourcing-Framework
- **MongoDB**: Datenpersistenz
- **Riot Games API**: Externe Datenquelle
- **League Client API**: Lokale Client-Integration

## Wichtige Hinweise für die Entwicklung
- Event-Sourcing-Patterns beachten
- Separation of Concerns zwischen Domain und Infrastructure
- Asynchrone Verarbeitung für API-Calls
- Proper Exception Handling für Client-Kommunikation

## Abhängigkeiten
- BE.CQRS (https://github.com/BoasE/BE.CQRS)
- Riot Games API
- League of Legends Client

---
*Diese Dokumentation wird kontinuierlich erweitert und aktualisiert.*