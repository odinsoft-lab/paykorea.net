using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace PayKorea.Net.Toss;

public sealed class TossPaymentsClient
{
    private readonly HttpClient _http;
    private readonly TossPaymentsOptions _opts;

    public TossPaymentsClient(HttpClient httpClient, TossPaymentsOptions options)
    {
        _http = httpClient;
        _opts = options;
        _http.BaseAddress = new Uri(options.BaseUrl);

        // Basic Auth with SecretKey:
        var basic = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_opts.SecretKey}:"));
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basic);
        _http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<TossPaymentResponse> ConfirmAsync(TossConfirmRequest req, CancellationToken ct = default)
    {
        using var message = new HttpRequestMessage(HttpMethod.Post, "/v1/payments/confirm")
        {
            Content = JsonContent.Create(req)
        };
        if (!string.IsNullOrWhiteSpace(_opts.TestCode))
        {
            message.Headers.Add("TossPayments-Test-Code", _opts.TestCode);
        }
        var resp = await _http.SendAsync(message, ct);
        if (resp.IsSuccessStatusCode)
        {
            var ok = await resp.Content.ReadFromJsonAsync<TossPaymentResponse>(cancellationToken: ct);
            if (ok is null) throw new InvalidOperationException("Empty response");
            return ok;
        }
        var err = await resp.Content.ReadFromJsonAsync<TossErrorResponse>(cancellationToken: ct);
        throw new HttpRequestException($"Toss error: {err?.code} {err?.message}");
    }

    public async Task<TossPaymentResponse> CancelAsync(string paymentKey, TossCancelRequest req, CancellationToken ct = default)
    {
        using var message = new HttpRequestMessage(HttpMethod.Post, $"/v1/payments/{paymentKey}/cancel")
        {
            Content = JsonContent.Create(req)
        };
        if (!string.IsNullOrWhiteSpace(_opts.TestCode))
        {
            message.Headers.Add("TossPayments-Test-Code", _opts.TestCode);
        }
        var resp = await _http.SendAsync(message, ct);
        if (resp.IsSuccessStatusCode)
        {
            var ok = await resp.Content.ReadFromJsonAsync<TossPaymentResponse>(cancellationToken: ct);
            if (ok is null) throw new InvalidOperationException("Empty response");
            return ok;
        }
        var err = await resp.Content.ReadFromJsonAsync<TossErrorResponse>(cancellationToken: ct);
        throw new HttpRequestException($"Toss error: {err?.code} {err?.message}");
    }
}
