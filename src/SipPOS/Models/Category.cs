namespace SipPOS.Models;

public class Category: Model
{
    public string Name
    {
        get; set;
    }

    public string Desc
    {
        get; set;
    }

    public List<Product> Products
    {
        get; set;
    }
}
