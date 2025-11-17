using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using Produkty.Data;

namespace Produkty;

public partial class MainPage : ContentPage
{
    private readonly ProductDatabase _database;
    private ObservableCollection<Product> produkty = new ObservableCollection<Product>();
    private Product wybranyProdukt = null;
    private string plikCsv = Path.Combine(FileSystem.AppDataDirectory, "produkty.csv");

    public MainPage(ProductDatabase database)
    {
        InitializeComponent();
        _database = database;
        ListaProduktow.ItemsSource = produkty;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await WczytajProduktyZBazy();
        AktualizujStatus();
    }

    private void AktualizujStatus()
    {
        StatusLabel.Text = "Liczba produktów: " + produkty.Count;
    }

    private void ListaProduktow_Selected(object sender, SelectedItemChangedEventArgs e)
    {
        wybranyProdukt = e.SelectedItem as Product;
    }

    private async void Dodaj_Clicked(object sender, EventArgs e)
    {
        string nazwa = await DisplayPromptAsync("Nowy produkt", "Podaj nazwę:");
        if (string.IsNullOrWhiteSpace(nazwa)) return;

        string cenaTekst = await DisplayPromptAsync("Cena produktu", "Podaj cenę:");
        if (!double.TryParse(cenaTekst?.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out double cena) || cena <= 0)
        {
            await DisplayAlert("Błąd", "Niepoprawna cena!", "OK");
            return;
        }

        string iloscTekst = await DisplayPromptAsync("Ilość produktu", "Podaj ilość:");
        if (!int.TryParse(iloscTekst, out int ilosc) || ilosc <= 0)
        {
            await DisplayAlert("Błąd", "Niepoprawna ilość!", "OK");
            return;
        }

        var produkt = new Product { Nazwa = nazwa, Cena = cena, Ilosc = ilosc };

        await _database.SaveProductAsync(produkt);
        await WczytajProduktyZBazy();
        AktualizujStatus();
    }

    private async void Edytuj_Clicked(object sender, EventArgs e)
    {
        if (wybranyProdukt == null)
        {
            await DisplayAlert("Uwaga", "Najpierw wybierz produkt do edycji.", "OK");
            return;
        }

        string nowaNazwa = await DisplayPromptAsync("Edycja produktu", "Nowa nazwa:", initialValue: wybranyProdukt.Nazwa);
        if (string.IsNullOrWhiteSpace(nowaNazwa)) return;

        string cenaTekst = await DisplayPromptAsync("Edycja produktu", "Nowa cena:", initialValue: wybranyProdukt.Cena.ToString("F2"));
        if (!double.TryParse(cenaTekst, NumberStyles.Any, CultureInfo.InvariantCulture, out double nowaCena) || nowaCena <= 0)
        {
            await DisplayAlert("Błąd", "Niepoprawna cena!", "OK");
            return;
        }

        string iloscTekst = await DisplayPromptAsync("Edycja produktu", "Nowa ilość:", initialValue: wybranyProdukt.Ilosc.ToString());
        if (!int.TryParse(iloscTekst, out int nowaIlosc) || nowaIlosc <= 0)
        {
            await DisplayAlert("Błąd", "Niepoprawna ilość!", "OK");
            return;
        }

        wybranyProdukt.Nazwa = nowaNazwa;
        wybranyProdukt.Cena = nowaCena;
        wybranyProdukt.Ilosc = nowaIlosc;

        await _database.SaveProductAsync(wybranyProdukt);
        await WczytajProduktyZBazy();
        AktualizujStatus();
    }

    private async void Usun_Clicked(object sender, EventArgs e)
    {
        if (wybranyProdukt == null)
        {
            await DisplayAlert("Uwaga", "Najpierw wybierz produkt do usunięcia.", "OK");
            return;
        }

        bool potwierdzenie = await DisplayAlert("Usuń produkt", $"Czy chcesz usunąć: {wybranyProdukt.Nazwa}?", "Tak", "Nie");
        if (potwierdzenie)
        {
            await _database.DeleteProductAsync(wybranyProdukt);
            await WczytajProduktyZBazy();
            wybranyProdukt = null;
            AktualizujStatus();
        }
    }

    private async void Zapisz_Clicked(object sender, EventArgs e)
    {
        try
        {
            StringBuilder csvTekst = new StringBuilder();
            csvTekst.AppendLine("Id,Nazwa,Cena,Ilosc");

            foreach (Product produkt in produkty)
            {
                string linia = $"{produkt.Id},{produkt.Nazwa},{produkt.Cena.ToString(CultureInfo.InvariantCulture)},{produkt.Ilosc}";
                csvTekst.AppendLine(linia);
            }

            File.WriteAllText(plikCsv, csvTekst.ToString(), Encoding.UTF8);
            
            await DisplayAlert("Lokalizacja bazy", FileSystem.AppDataDirectory, "OK");
        }
        catch (Exception blad)
        {
            await DisplayAlert("Błąd zapisu", blad.Message, "OK");
        }
    }

    private async Task WczytajProduktyZBazy()
    {
        produkty.Clear();
        var lista = await _database.GetProductsAsync();
        foreach (var p in lista)
            produkty.Add(p);
    }
}
