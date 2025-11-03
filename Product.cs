namespace Produkty;

public class Product
{
    public int Id { get; set; }
    public string Nazwa { get; set; }
    public double Cena { get; set; }
    public int Ilosc { get; set; }

    public override string ToString()
    {
        return $"{Id}. {Nazwa} - {Cena} zł ({Ilosc} szt.)";
    }
}