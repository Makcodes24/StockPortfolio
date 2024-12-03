namespace WebClient.ViewModel
{
    public interface ITokenService
    {
        string? AccessToken { get; set; }
        string? UserDetails { get; set; }

        string AccessToken2 { get; set; }


    }
}
