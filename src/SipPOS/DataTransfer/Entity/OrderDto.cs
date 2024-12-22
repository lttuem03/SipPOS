using System.Collections.ObjectModel;

namespace SipPOS.DataTransfer.Entity;

public class OrderDto
{
    public long Id { get; set; }

    public long StaffId { get; set; }

    public DateTime CreatedAt { get; set; }

    public ObservableCollection<OrderItemDto> Items { get; set; }

    public double? OrderBasedVAT { get; set; }

    public OrderDto()
    {
        Id = -1;
        StaffId = -1;
        CreatedAt = DateTime.MinValue;
        Items = new ObservableCollection<OrderItemDto>();
        OrderBasedVAT = null;
    }
}