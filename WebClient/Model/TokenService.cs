
using Azure.Core;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebClient.ViewModel;
namespace WebClient.Model
{
    public class TokenService : ITokenService
    {
        string ITokenService.AccessToken { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        string ITokenService.UserDetails { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        string ITokenService.AccessToken2 { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    }
}
