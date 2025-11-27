namespace GT.Application.DTOs;

public class AuthenticateResponse
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }
}
