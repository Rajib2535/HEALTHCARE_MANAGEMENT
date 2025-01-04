using COMMON_SERVICE.Interface;
using COMMON_SERVICE.Service;
using CORPORATE_DISBURSEMENT_UTILITY;
using DATA.Interface;
using DATA.Repository;
using SERVICE.Interface;
using SERVICE.Manager;

namespace WEB_APP.Extensions
{
    /// <summary>
    /// service extension for normalizing dependency injection
    /// </summary>
    public static class ServiceExtension
    {
        public static IServiceCollection AddActionModuleServices(this IServiceCollection services)
        {

            services.AddScoped<ICommonRepository, CommonRepository>();
            services.AddScoped<ICommonService, CommonService>();

            services.AddScoped<IHttpClientExtensions, HttpClientExtensions>();

            services.AddScoped<IDashboardManager, DashboardManager>();
            services.AddScoped<IDashboardRepository, DashboardRepository>();



            services.AddScoped<ICommonDropdownRepository, CommonDropdownRepository>();
            services.AddScoped<ICommonDropdownManager, CommonDropdownManager>();


            services.AddScoped<ICommonManager, CommonManager>();
            services.AddScoped<ICredentialRepository, CredentialRepository>();
            services.AddScoped<ITokenManager, TokenManager>();
            services.AddScoped<ITokenHandler, TokenHandler>();
            services.AddScoped<IDocterManager, DoctorManager>();
            return services;
        }
        public static IServiceCollection AddUserLoginAndPermissionModuleServices(this IServiceCollection services)
        {
            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IMenuManager, MenuManager>();
            services.AddScoped<IMenuRepository, MenuRepository>();
            services.AddScoped<IPermissionManager, PermissionManager>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IUserRoleManager, UserRoleManager>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddScoped<IRoleManager, RoleManager>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IRolePermissionManager, RolePermissionManager>();
            services.AddScoped<IRolePermissionRepository, RolePermissionRepository>();
            services.AddScoped<IDoctorRepository, DoctorRepository>();

            return services;
        }


    }
}
