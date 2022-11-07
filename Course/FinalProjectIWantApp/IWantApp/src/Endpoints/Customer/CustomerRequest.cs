namespace IWantApp.Endpoints.Customer;

public record CustomerRequest(string Email, string Password, string Name, string Cpf);
