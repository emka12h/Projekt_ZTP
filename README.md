# 💒 Wedding Planner - System Zarządzania Ślubem

Kompleksowy system klasy zarządzania (Management System) ułatwiający organizację ślubu i wesela. Projekt budowany jest w architekturze **mikroserwisów**, co pozwala na niezależne skalowanie i rozwijanie poszczególnych modułów (np. listy gości, budżetu, harmonogramu).

Obecnie rozwijany jest moduł **Budget Service** (Zarządzanie Budżetem).

## 🚀 Technologie i Architektura
Projekt stawia na nowoczesne, komercyjne standardy:
* **Backend:** C#, .NET 8 (lub nowszy), ASP.NET Core Web API
* **Architektura:** Microservices, API Gateway (Wzorzec Gateway Routing)
* **Baza danych:** PostgreSQL (Relacyjna baza danych)
* **ORM:** Entity Framework Core (Code-First)
* **Dokumentacja API:** Swagger / OpenAPI
* **Infrastruktura & DevOps:** Docker, Docker Compose, konteneryzacja środowiska deweloperskiego

## 🗺️ Roadmapa Projektu (Trello)

Projekt został podzielony na logiczne fazy i zadania (Karty), aby symulować pracę w metodyce zwinnej (Agile/Scrum).

### Faza 1: Architektura i Infrastruktura (Zakończona ✅)
* [x] **Karta 1:** Inicjalizacja repozytorium i głównej solucji .NET.
* [x] **Karta 2:** Konfiguracja środowiska kontenerowego (Docker Compose).
* [x] **Karta 3:** Utworzenie i konfiguracja Bramy API (`ApiGateway`).
* [x] **Karta 4:** Utworzenie szkieletu mikroserwisu `GuestService`.

### Faza 2: Baza danych i Zarządzanie Gośćmi (Zakończona ✅)
* [x] **Karta 5:** Baza danych dla Gości (Integracja PostgreSQL + EF Core, automatyczne migracje). Zabezpieczenie przed Race Condition (`EnableRetryOnFailure`).
* [x] **Karta 6:** API Zarządzania Gośćmi (CRUD). Utworzenie kontrolerów do pobierania, dodawania i zmiany statusu RSVP. Stabilizacja portów w środowisku Docker.

### Faza 3: Kolejne Mikroserwisy (W planach ⏳)
* [ ] **Karta 7:** Moduł Budżetu (`BudgetService`) – śledzenie wydatków i opłaconych zaliczek.
* [ ] **Karta 8:** Moduł Harmonogramu (`ScheduleService`) – plan dnia ślubu.
* [ ] **Karta 9:** Autoryzacja i Uwierzytelnianie (`IdentityService`) – logowanie dla Pary Młodej za pomocą tokenów JWT.

### Faza 4: Frontend (W planach ⏳)
* [ ] **Karta 10:** Inicjalizacja aplikacji klienckiej (Interfejs Użytkownika UI).
* [ ] **Karta 11:** Ekran zarządzania listą gości (Połączenie z `GuestService`).
* [ ] **Karta 12:** Interaktywny Dashboard podsumowujący status organizacji.

---
