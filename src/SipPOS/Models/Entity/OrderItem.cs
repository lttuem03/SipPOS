namespace SipPOS.Models.Entity;

using SipPOS.DataTransfer.Entity;

public class OrderItem
{
    public long Id
    {
        get;
    }

    public long OrderId
    {
        get;
    }

    public long ProductId
    {
        get;
    }

    public int Quantity
    {
        get;
    }

    public double TotalValue
    {
        get;
    }

    public string? Option
    {
        get;
    }

    public string? Note
    {
        get;
    }

    public double? ProductBasedVAT
    {
        get;
    }

    public OrderItem(long id, OrderItemDto dto)
    {
        Id = id;
        OrderId = dto.OrderId;
        ProductId = dto.ProductId;
        Quantity = dto.Quantity;
        TotalValue = dto.TotalValue;

        Option = dto.Option;
        Note = dto.Note;
        ProductBasedVAT = dto.ProductBasedVAT;
    }
}