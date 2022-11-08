namespace IWantApp.Endpoints.Order;

public record OrderRequest(List<Guid> ProductIds, string DeliveryAdress);
