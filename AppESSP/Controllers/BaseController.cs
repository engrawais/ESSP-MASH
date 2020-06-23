using AppESSP.Helper;
using ESSPCORE.Common;
using ESSPSERVICE.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AppESSP.Controllers
{
    public class BaseController : Controller, IActionFilter
    {
        //protected override void OnException(ExceptionContext filterContext)
        //{
        //    var controllerName = filterContext.RouteData.Values["controller"];
        //    var actionName = filterContext.RouteData.Values["action"];
        //    var model = new HandleErrorInfo(filterContext.Exception, controllerName.ToString(), actionName.ToString());
        //    string Exception = filterContext.Exception.Message;
        //    string InnerException = filterContext.Exception.InnerException != null ? filterContext.Exception.InnerException.Message : "";
        //    //AppAssistant.WriteToExceptionLogFile(controllerName + " | " + actionName + " | " + Exception + " | " + InnerException);
        //    filterContext.Result = new ViewResult
        //    {
        //        ViewName = "~/Views/Home/ErrorPage.cshtml",
        //        ViewData = new ViewDataDictionary<HandleErrorInfo>(model),
        //        TempData = filterContext.Controller.TempData
        //    };
        //    filterContext.ExceptionHandled = true;
        //}
        void IActionFilter.OnActionExecuted(ActionExecutedContext filterContext)
        {
            //filterContext.Controller.ViewBag.OnActionExecuted = "IActionFilter.OnActionExecuted filter called";
        }
        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            bool HavePermission = true;
            try
            {
                HttpSessionStateBase session = filterContext.HttpContext.Session;
                
                if (HavePermission == false)
                {
                    //filterContext.Result = new HttpUnauthorizedResult();
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Index" }));
                    filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);
                }
            }
            catch (Exception ex)
            {
                //filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Index" }));
                //filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);
            }

        }

        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            HttpSessionStateBase session = filterContext.HttpContext.Session;
            VMLoggedUser LoggedInUser = Session["LoggedInUser"] as VMLoggedUser;
            bool HavePermission = false;
            try
            {
                switch (filterContext.ActionDescriptor.ActionName)
                {
                    case "Create":
                        if (LoggedInUser.TMSAdd == true)
                            HavePermission = true;
                        break;
                    case "Create1":
                        if (LoggedInUser.TMSAdd == true)
                            HavePermission = true;
                        break;
                    case "Create2":
                        if (LoggedInUser.TMSAdd == true)
                            HavePermission = true;
                        break;
                    case "Create3":
                        if (LoggedInUser.TMSAdd==true)
                            HavePermission = true;
                        break;
                    case "Edit":
                        if(LoggedInUser.TMSEdit== true)
                            HavePermission = true;
                        break;
                    case "Details":
                        if(LoggedInUser.TMSView == true)
                            HavePermission = true;
                        break;
                    case "Delete":
                        if(LoggedInUser.TMSDelete == true)
                            HavePermission = true;
                        break;
                    default:
                        HavePermission = true;
                        break;
                }
                if (HavePermission == false)
                {
                    //filterContext.Result = new HttpUnauthorizedResult();
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, action = "Index" }));
                    filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);

                }
            }
            catch (Exception ex)
            {
                //filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Index" }));
                //filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);
            }
        }
    }
}