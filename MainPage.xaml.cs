using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace Produkty;

public partial class MainPage : ContentPage
{
    private ObservableCollection<Product> produkty = new ObservableCollection<Product>();
    private Product wybranyProdukt = null;
    private string plikCsv = Path.Combine(FileSystem.AppDataDirectory, "produkty.csv");

    public MainPage()
    {
        InitializeComponent();
        ListaProduktow.ItemsSource = produkty;
        WczytajProdukty();
        AktualizujStatus();
    }

    private void AktualizujStatus()
    {
        StatusLabel.Text = "Liczba produktów: " + produkty.Count;
    }

    private void ListaProduktow_Selected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem is Product zaznaczony)
        {
            wybranyProdukt = zaznaczony;
        }
        else
        {
            wybranyProdukt = null;
        }
    }

    private async void Dodaj_Clicked(object sender, EventArgs e)
    {
        string nazwa = await DisplayPromptAsync("Nowy produkt", "Podaj nazwę:");
        if (string.IsNullOrWhiteSpace(nazwa)) return;

        string cenaTekst = await DisplayPromptAsync("Cena produktu", "Podaj cenę:");
        double cena;
        if (cenaTekst != null)
        {
            string cenaZKropka = cenaTekst.Replace(',', '.');
            bool czyCenaPoprawna = double.TryParse(cenaZKropka, NumberStyles.Any, CultureInfo.InvariantCulture, out cena);
            if (!czyCenaPoprawna || cena <= 0)
            {
                await DisplayAlert("Błąd", "Niepoprawna cena!", "OK");
                return;
            }
        }
        else
        {
            await DisplayAlert("Błąd", "Nie podano ceny!", "OK");
            return;
        }

        string iloscTekst = await DisplayPromptAsync("Ilość produktu", "Podaj ilość:");
        int ilosc;
        bool czyIloscPoprawna = int.TryParse(iloscTekst, out ilosc);
        if (!czyIloscPoprawna || ilosc <= 0)
        {
            await DisplayAlert("Błąd", "Niepoprawna ilość!", "OK");
            return;
        }

        int noweId;
        if (produkty.Count == 0)
        {
            noweId = 1;
        }
        else
        {
            noweId = produkty.Max(p => p.Id) + 1;
        }

        Product nowyProdukt = new Product();
        nowyProdukt.Id = noweId;
        nowyProdukt.Nazwa = nazwa;
        nowyProdukt.Cena = cena;
        nowyProdukt.Ilosc = ilosc;

        produkty.Add(nowyProdukt);
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
        double nowaCena;
        bool czyCenaPoprawna = double.TryParse(cenaTekst, NumberStyles.Any, CultureInfo.InvariantCulture, out nowaCena);
        if (!czyCenaPoprawna || nowaCena <= 0)
        {
            await DisplayAlert("Błąd", "Niepoprawna cena!", "OK");
            return;
        }

        string iloscTekst = await DisplayPromptAsync("Edycja produktu", "Nowa ilość:", initialValue: wybranyProdukt.Ilosc.ToString());
        int nowaIlosc;
        bool czyIloscPoprawna = int.TryParse(iloscTekst, out nowaIlosc);
        if (!czyIloscPoprawna || nowaIlosc <= 0)
        {
            await DisplayAlert("Błąd", "Niepoprawna ilość!", "OK");
            return;
        }

        wybranyProdukt.Nazwa = nowaNazwa;
        wybranyProdukt.Cena = nowaCena;
        wybranyProdukt.Ilosc = nowaIlosc;

        ListaProduktow.ItemsSource = null;
        ListaProduktow.ItemsSource = produkty;
    }

    private async void Usun_Clicked(object sender, EventArgs e)
    {
        if (wybranyProdukt == null)
        {
            await DisplayAlert("Uwaga", "Najpierw wybierz produkt do usunięcia.", "OK");
            return;
        }

        bool potwierdzenie = await DisplayAlert("Usuń produkt", "Czy chcesz usunąć: " + wybranyProdukt.Nazwa + "?", "Tak", "Nie");
        if (potwierdzenie)
        {
            produkty.Remove(wybranyProdukt);
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
                string linia = produkt.Id + "," + produkt.Nazwa + "," + produkt.Cena.ToString(CultureInfo.InvariantCulture) + "," + produkt.Ilosc;
                csvTekst.AppendLine(linia);
            }

            File.WriteAllText(plikCsv, csvTekst.ToString(), Encoding.UTF8);
            await DisplayAlert("Sukces", "Dane zapisano do pliku: " + plikCsv, "OK");
        }
        catch (Exception blad)
        {
            await DisplayAlert("Błąd zapisu", blad.Message, "OK");
        }
    }

    private void WczytajProdukty()
    {
        if (!File.Exists(plikCsv)) return;

        string[] linie = File.ReadAllLines(plikCsv);

        for (int i = 1; i < linie.Length; i++)
        {
            string[] czesci = linie[i].Split(',');

            if (czesci.Length < 4) continue;

            Product produkt = new Product();
            produkt.Id = int.Parse(czesci[0]);
            produkt.Nazwa = czesci[1];
            produkt.Cena = double.Parse(czesci[2], CultureInfo.InvariantCulture);
            produkt.Ilosc = int.Parse(czesci[3]);

            produkty.Add(produkt);
        }
    }
}
