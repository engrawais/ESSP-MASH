using System.Web.Mvc;

namespace AppESSP.Areas.ESSP
{
    public class ESSPAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ESSP";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ESSP_default",
                "ESSP/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}