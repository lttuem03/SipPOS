using System.Collections.ObjectModel;
using SipPOS.DataTransfer.Entity;

namespace SipPOS.Models.Entity;

public class Order
{
    public long Id
    {
        get;
    }

    public long StaffId
    {
        get;
    }

    public DateTime CreatedAt
    {
        get;
    }

    public ObservableCollection<OrderItem> Items
    {
        get;
    }

    public double? OrderBasedVAT
    {
        get;
    }

    public Order(long id, OrderDto dto)
    {
        Id = id;
        StaffId = dto.StaffId;
        CreatedAt = dto.CreatedAt;
        Items = new();
        OrderBasedVAT = dto.OrderBasedVAT;

        long orderItemId = 0;

        foreach (var itemDto in dto.Items)
        {
            Items.Add(new OrderItem
            (
                id: orderItemId,
                dto: itemDto
            ));

            orderItemId++;
        }
    }
}