using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace PayKorea.Net.AspNetCore;

public sealed class PayKoreaOptions
{
	public string Provider { get; set; } = "Mock"; // 추후 Toss/Inicis 등으로 확장
}

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddPayKorea(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddOptions<PayKoreaOptions>()
			.Bind(configuration.GetSection("PayKorea"))
			.ValidateDataAnnotations()
			.ValidateOnStart();

		services.TryAddSingleton<IPaymentProvider>(sp =>
		{
			var opts = sp.GetRequiredService<IOptions<PayKoreaOptions>>().Value;
			// 옵션에 따라 Provider 선택 (현재는 Mock만)
			return new MockPaymentProvider();
		});

		services.TryAddSingleton<PayKoreaClient>();
		return services;
	}
}
