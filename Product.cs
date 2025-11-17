using SQLite;

namespace Produkty;

public class Product
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [MaxLength(100)]
    public string Nazwa { get; set; }

    public double Cena { get; set; }
    public int Ilosc { get; set; }

    public override string ToString()
    {
        return $"{Id}. {Nazwa} - {Cena} zł ({Ilosc} szt.)";
    }
}