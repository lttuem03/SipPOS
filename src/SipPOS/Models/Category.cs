namespace SipPOS.Models;

public class Category: BaseModel
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
