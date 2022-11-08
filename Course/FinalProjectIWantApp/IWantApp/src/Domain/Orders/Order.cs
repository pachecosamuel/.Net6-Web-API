using Flunt.Validations;
using System.Diagnostics.Contracts;

namespace IWantApp.Domain.Orders;

public class Order : Entity
{
    public string CustomerId { get; private set; }
    public List<Product> Products { get; private set; }
    public decimal Total { get; private set; }
    public string DeliveryAdress { get; private set; }

    public Order()
    {
    }

    public Order(string customerId, string clientName, List<Product> products, string deliveryAdress)
    {
        CustomerId=customerId;
        Products=products;
        DeliveryAdress=deliveryAdress;

        CreatedBy = clientName;
        EditedBy = clientName;
        CreatedOn = DateTime.Now;
        EditedOn = DateTime.Now;

        Total = 0;
        foreach(var item in Products)
        {
            Total += item.Price;
        }

        Validate();
    }

    private void Validate()
    {
        var contract = new Contract<Order>()
            .IsNotNull(CustomerId, "Customer")
            .IsNotNull(Products, "Products");
        AddNotifications(contract);
    }
}
