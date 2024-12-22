using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SipPOS.DataTransfer.Entity;

public class OrderItemDto
{
    public long? Id { get; set; }

    public long OrderId { get; set; }

    public long ProductId { get; set; }

    public int Quantity { get; set; }

    public double TotalValue { get; set; }

    public string? Option { get; set; }

    public string? Note { get; set; }

    public double? ProductBasedVAT { get; set; }

    public OrderItemDto()
    {
        Id = -1;
        OrderId = -1;
        ProductId = -1;
        Quantity = 0;
        TotalValue = 0;

        Option = null;
        Note = null;
        ProductBasedVAT = null;
    }
}