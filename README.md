# 📦 Produkty – Aplikacja .NET MAUI z SQLite

Aplikacja służy do zarządzania produktami. Umożliwia dodawanie, edytowanie, usuwanie oraz zapisywanie danych do pliku CSV.  
Dane przechowywane są w **lokalnej bazie SQLite**, tworzonej automatycznie przy pierwszym uruchomieniu aplikacji.

---

## 🚀 Funkcjonalność

- 📋 Wyświetlanie listy produktów z bazy SQLite  
- ➕ Dodawanie nowego produktu (nazwa, cena, ilość)  
- ✏️ Edycja istniejącego produktu  
- 🗑️ Usuwanie produktu  
- 💾 Eksport danych do pliku **CSV**  
- 🔢 Wyświetlanie liczby produktów  
- 🗃️ Automatyczne tworzenie bazy SQLite i tabeli `Product`

---

## 🗂️ Struktura projektu

Produkty/
│
├── MainPage.xaml → UI aplikacji
├── MainPage.xaml.cs → logika aplikacji (CRUD, CSV)
├── Product.cs → model danych
│
├── Data/
│ └── ProductDatabase.cs → obsługa bazy SQLite
│
├── MauiProgram.cs → konfiguracja MAUI + rejestracja SQLite
├── produkty.db3 → baza SQLite tworzona automatycznie
└── produkty.csv → eksport CSV tworzony ręcznie


---

## 🧰 Wymagania

- Visual Studio 2022 (z obsługą **.NET MAUI**)  
- .NET SDK 7 lub 8  
- Workload MAUI:

bash
dotnet workload install maui


## Platforma:

Windows

macOS

Android (emulator)

## 📥 Instalacja i uruchomienie
1️⃣ Pobierz projekt
git clone https://github.com/Latsownik1/AppMaui.git

2️⃣ Otwórz rozwiązanie

Otwórz w Visual Studio:

Produkty.sln

3️⃣ Uruchom aplikację

Wybierz platformę (Windows Machine / Android Emulator)

Kliknij Run

Aplikacja automatycznie utworzy lokalną bazę danych:

FileSystem.AppDataDirectory/produkty.db3

## 💾 Przechowywanie danych
🔹 Baza SQLite

Plik:

produkty.db3


Tworzy się automatycznie w:

FileSystem.AppDataDirectory

🔹 Eksport CSV

Po kliknięciu „Zapisz CSV” powstaje:

produkty.csv


również w AppDataDirectory.
