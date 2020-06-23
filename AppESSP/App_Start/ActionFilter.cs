using ESSPCORE.Common;
using ESSPCORE.EF;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace AppESSP.App_Start
{
    public class CustomActionFilter : FilterAttribute, IActionFilter
    {
        void IActionFilter.OnActionExecuted(ActionExecutedContext filterContext)
        {
            filterContext.Controller.ViewBag.OnActionExecuted = "IActionFilter.OnActionExecuted filter called";
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {

        }

        //void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        //{

        //    HttpSessionStateBase session = filterContext.HttpContext.Session;
        //    var user = session["LoggedInUser"] as VMLoggedUser;

        //    try
        //    {
        //        if (filterContext.HttpContext.Request.HttpMethod == "POST")
        //        {
        //            using (var stream = new MemoryStream())
        //            {
        //                filterContext.HttpContext.Request.InputStream.Seek(0, SeekOrigin.Begin);
        //                filterContext.HttpContext.Request.InputStream.CopyTo(stream);
        //                string requestBody = Encoding.UTF8.GetString(stream.ToArray());
        //            }

        //            //var stream = filterContext.HttpContext.Request.InputStream;
        //            //var data = new byte[stream.Length];
        //            //stream.Read(data, 0, data.Length);
        //            //string val = (Encoding.UTF8.GetString(data));

        //            //var ser = new JavaScriptSerializer();

        //            //// you can read the json data from here
        //            //var jsonDictionary = ser.Deserialize<Dictionary<string, string>>(val);

        //            var parameters = filterContext.HttpContext.Request.QueryString;
        //            var ActionInfo = filterContext.ActionDescriptor;
        //            var pars = ActionInfo.GetParameters();
        //            foreach (var p in pars)
        //            {

        //                var type = p.ParameterType; //get type expected
        //            }
        //            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
        //            string actionName = filterContext.ActionDescriptor.ActionName;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}
        //private string GetJsonContents(System.Web.HttpRequestBase Request)
        //{
        //    string JsonContents = string.Empty;
        //    try
        //    {
        //        using (Stream receiveStream = Request.InputStream)
        //        {
        //            using (StreamReader readStream = new StreamReader(receiveStream))
        //            {
        //                receiveStream.Seek(0, System.IO.SeekOrigin.Begin);
        //                JsonContents = readStream.ReadToEnd();
        //            }
        //        }
        //    }
        //    catch
        //    {
        //    }
        //    return JsonContents;
        //}
    }
    public static class AuditLogging
    {
        public static void SaveAuditLog(object NewObj,int LoggedInuserID, EnumControllerNames enumControllerName, EnumActionNames enumActionName)
        {
            using (var db = new ABESSPEntities())
            {
                Type t = NewObj.GetType();
                PropertyInfo[] props = t.GetProperties();

                foreach (var prop in props)
                    if (prop.GetIndexParameters().Length == 0)
                    {
                        string PValue = "";
                        string PName = prop.Name;
                        string PType = prop.PropertyType.Name;
                        if (prop.GetValue(NewObj) != null)
                            PValue = prop.GetValue(NewObj).ToString();
                    }
                    else
                    {

                    } 
            }
        }
    }
    public enum EnumControllerNames
    {
        Payroll,
        FinancialYear,
        Reader
    }
    public enum EnumActionNames
    {
        Create,
        Edit,
        Delete
    }
}