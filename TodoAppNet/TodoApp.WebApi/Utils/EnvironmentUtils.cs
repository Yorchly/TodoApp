using TodoApp.Application.Common.Enums;

namespace TodoApp.WebApi.Utils
{
    public static class EnvironmentUtils
    {
        public static EnvironmentEnum GetEnvironment(string? env) => env switch
        {
            "Development" => EnvironmentEnum.Development,
            "Production" => EnvironmentEnum.Production,
            _ => EnvironmentEnum.Production
        };
    }
}
