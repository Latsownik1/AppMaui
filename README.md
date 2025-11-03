# Produkty - Aplikacja do zarządzania produktami

Prosta aplikacja napisana w **.NET MAUI**, służąca do zarządzania produktami. Umożliwia dodawanie, edytowanie, usuwanie oraz zapisywanie produktów do pliku CSV.

## Funkcje
- Wyświetlanie listy produktów
- Dodawanie nowego produktu (nazwa, cena, ilość)
- Edycja istniejącego produktu
- Usuwanie produktu
- Zapis danych do pliku CSV
- Wczytywanie danych z pliku CSV przy uruchomieniu aplikacji
- Informacja o liczbie produktów

## Struktura projektu
- `MainPage.xaml` - UI aplikacji
- `MainPage.xaml.cs` - logika aplikacji
- `Product.cs` - klasa reprezentująca produkt
- `produkty.csv` - plik CSV przechowujący dane produktów (tworzony automatycznie w katalogu aplikacji)

## Wymagania
- [.NET MAUI](https://learn.microsoft.com/en-us/dotnet/maui/what-is-maui)
- Visual Studio 2022 lub nowsze z obsługą .NET MAUI
- System Windows lub macOS

## Instalacja i uruchomienie
1. Sklonuj repozytorium:
   ```bash
   git clone https://github.com/TWOJE-NAZWISKO/produkty.git
