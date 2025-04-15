//yhis class handles simple Basic Authentication for the GameStoreAPI.
//it reads the "Authorization" header, decodes it (base64), checks credentials (hardcoded for demo),
//and authenticates the user. this is useful to restrict access to endpoints using [Authorize].

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace GameStoreAPI.Authentication
{
    public class AuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        //ISystemClock is marked obsolete but still required by ASP.NET Core base class in .NET 8
#pragma warning disable CS0618
        public AuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock) { }
#pragma warning restore CS0618

        //main logic that handles authentication
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // 1. check if the request contains the Authorization header
            if (!Request.Headers.ContainsKey("Authorization"))
                return Task.FromResult(AuthenticateResult.Fail("Missing Authorization Header"));

            try
            {
                var rawHeader = Request.Headers["Authorization"].ToString();

                // 2. reject empty Authorization headers
                if (string.IsNullOrEmpty(rawHeader))
                    return Task.FromResult(AuthenticateResult.Fail("Empty Authorization Header"));

                // 3. parse the header ("Basic base64encodedstring")
                var authHeader = AuthenticationHeaderValue.Parse(rawHeader);

                // 4. decode the Base64-encoded string
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter ?? "");
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':', 2);

                // 5. validate the format (should be username:password)
                if (credentials.Length != 2)
                    return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Format"));

                var username = credentials[0];
                var password = credentials[1];

                // 6. hardcoded credential check (for demo/testing only)
                if (username != "admin" || password != "password")
                    return Task.FromResult(AuthenticateResult.Fail("Invalid Username or Password"));

                // 7. create authentication ticket and return success
                var claims = new[] { new Claim(ClaimTypes.Name, username) };
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return Task.FromResult(AuthenticateResult.Success(ticket));
            }
            catch (FormatException)
            {
                return Task.FromResult(AuthenticateResult.Fail("Authorization Header Format Error"));
            }
            catch (Exception ex)
            {
                return Task.FromResult(AuthenticateResult.Fail($"Auth failed: {ex.Message}"));
            }
        }
    }
}