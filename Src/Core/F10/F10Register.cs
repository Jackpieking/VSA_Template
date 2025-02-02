using F10.BusinessLogic;
using F10.DataAccess;
using FACommon.DependencyInjection;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace F10;

public sealed class F10Register : IServiceRegister
{
    public IServiceCollection Register(IServiceCollection services, IConfiguration configuration)
    {
        var currentAssembly = typeof(F10Register).Assembly;

        #region Filters
        services.RegisterFiltersFromAssembly(currentAssembly);
        #endregion

        #region Validation
        services.AddValidatorsFromAssembly(currentAssembly, ServiceLifetime.Singleton);
        #endregion

        #region Core
        services
            .AddScoped<IF10Repository, F10Repository>()
            .MakeScopedLazy<IF10Repository>()
            .AddScoped<F10Service>();
        #endregion

        return services;
    }
}
