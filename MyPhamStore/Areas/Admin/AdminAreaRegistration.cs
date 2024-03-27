using System.Web.Mvc;

namespace MyPhamStore.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { controller = "MyPhams", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "MyPhamStore.Areas.Admin.Controllers" }
                );
        }
    }
}