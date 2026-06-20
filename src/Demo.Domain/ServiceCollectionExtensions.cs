using Microsoft.Extensions.DependencyInjection;

namespace Demo.Domain;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services) => services;
}
