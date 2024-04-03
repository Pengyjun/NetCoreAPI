using CH.Simple.SqlSuger;

namespace CH.Simple.APP.API
{
    public static class ProgramExtensions
    {
        /// <summary>
        /// 注入SqlSugar
        /// </summary>
        /// <param name="builder"></param>
        public static void AddSqlSugar(this WebApplicationBuilder builder)
        {
            builder.Services.AddSqlSugarContext(builder.Configuration.GetConnectionString("MySQL"));
        }
    }
}
