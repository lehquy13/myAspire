using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using LibraryManagement.WebApi.Mappings;

namespace LibraryManagement.WebApi
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddCors();
            services.AddMappings();
            
            //Handle validators
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            //lower url
            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
            });

            return services;
        }
    }
}

