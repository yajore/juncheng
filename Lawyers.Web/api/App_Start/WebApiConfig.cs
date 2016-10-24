using System.Web.Http;

namespace Lawyers.Web
{
    public static class WebApiConfig
    {
        //请求频次
        //static string reqLimit = ConfigurationManager.AppSettings["reqLimit"];

        public static void Register(HttpConfiguration config)
        {

            //config.MessageHandlers.Add(new ThrottlingHandler(
            //                        new InMemoryThrottleStore(),
            //                        id => int.Parse(reqLimit),
            //                        TimeSpan.FromMinutes(1)
            //                        ));
            ////异常筛选
            //config.Filters.Add(new WebApiExceptionFilter());
            // Web API 配置和服务
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { controller = "baseapi", id = "NoMethod" }
            );
        }
    }
}
