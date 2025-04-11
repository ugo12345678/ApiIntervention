using DataAccess.Abstraction.Repositories;
using DataAccess.Repositories;
using Business.Abstraction.Manager;
using DataAccess.Abstraction;
using DataAccess;
using Business.Manager;
using FluentValidation;
using Business.Abstraction.Models;
using Business.Validators.Version;

namespace Api.Extensions
{
    /// <summary>
    /// Extension class used to configure Swagger
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services, IConfiguration configuration)
        {
            // IOC - Data Access
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IInterventionRepository, InterventionRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            // IOC - Business
            services.AddScoped<IInterventionManager, InterventionManager>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentLanguageManager, CurrentLanguageManager>();
            services.AddScoped<IMessageLocalizerManager, MessageLocalizerManager>();

            // Validator
            services.AddScoped<IValidator<InterventionModel>, InterventionValidator>();
            // Other
            services.AddScoped<IUnitOfWork, EfCoreUnitOfWork<ApplicationDbContext>>(provider =>
            {
                ApplicationDbContext dbContext = provider.GetRequiredService<ApplicationDbContext>();
                EfCoreUnitOfWork<ApplicationDbContext> unitOfWork = new(dbContext);
                return unitOfWork;
            });

            services.AddMemoryCache();

            return services;
        }
    }
}
