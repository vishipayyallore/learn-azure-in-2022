using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Web;
using System.Net.Http.Headers;
using System.Text.Json;

namespace AADDemo1.Pages
{
    public class CallApiModel : PageModel
    {
        public string Json = string.Empty;
        private readonly ITokenAcquisition _tokenAcquisition;
        public string content;

        public CallApiModel(ITokenAcquisition tokenAcquisition)
        {
            _tokenAcquisition = tokenAcquisition;
        }

        public async Task OnGet()
        {
            string[] scope = new string[] { "api://29d520e2-320b-4f91-93a8-ce19478a2499/WebApiAccess" };
            string accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(scope);


            string apiURL = "https://localhost:7062/api/UseIdentity";
            HttpClient client = new();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            HttpResponseMessage responseMessage = await client.GetAsync(apiURL);
            content = await responseMessage.Content.ReadAsStringAsync();

            var parsed = JsonDocument.Parse(content);
            var formatted = JsonSerializer.Serialize(parsed, new JsonSerializerOptions { WriteIndented = true });

            Json = formatted;
        }
    }
}
