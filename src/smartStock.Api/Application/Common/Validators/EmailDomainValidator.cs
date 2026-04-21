using System.Net;

namespace smartStock.Api.Application.Common.Validators;

internal static class EmailDomainValidator
{
    internal static async Task<bool> DominioExisteAsync(string email, CancellationToken ct)
    {
        try
        {
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            cts.CancelAfter(TimeSpan.FromSeconds(3));
            var dominio     = email.Split('@').Last();
            var direcciones = await Dns.GetHostAddressesAsync(dominio, cts.Token);
            return direcciones.Length > 0;
        }
        catch
        {
            return false;
        }
    }
}
