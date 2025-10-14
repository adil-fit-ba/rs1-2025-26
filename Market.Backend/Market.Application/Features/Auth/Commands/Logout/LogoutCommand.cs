namespace Market.Application.Features.Auth.Commands.Logout;

/// <summary>
/// Command za odjavu korisnika i poni�tavanje (revokaciju) refresh tokena.
/// </summary>
public sealed class LogoutCommand : IRequest
{
    /// <summary>
    /// Refresh token koji se poni�tava.
    /// </summary>
    public required string RefreshToken { get; init; }
}
