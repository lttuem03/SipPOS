namespace SipPOS.Models;

public class Category : BaseModel
{
    public string? Name { get; set; }
    public string? Desc { get; set; }
    public string? Status { get; set; }
    public IList<string> ImageUrls { get; set; } = new List<string>();

}
