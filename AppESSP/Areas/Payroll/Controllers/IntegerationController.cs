using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using AppESSP.App_Start;
using ESSPCORE.Common;
using ESSPCORE.EF;
using ESSPSERVICE.Generic;
using Newtonsoft.Json;

namespace AppESSP.Areas.Payroll.Controllers
{

    public class IntegerationController : Controller
    {
        IEntityService<PayrollPeriod> PayrollPeriodService;
        IDDService DDService;
        IEntityService<VAT_MonthData> MonthDataService;
        public List<string> ToasterMessages = new List<string>();
        public IntegerationController(IEntityService<PayrollPeriod> payrollPeriodService, IDDService dDService, IEntityService<VAT_MonthData> monthDataService)
        {
            PayrollPeriodService = payrollPeriodService;
            DDService = dDService;
            MonthDataService = monthDataService;
        }
        // GET: Payroll/Integeration
        public ActionResult Index()
        {
            ABESSPEntities db = new ABESSPEntities();
            List<PayrollPeriod> list = db.PayrollPeriods.Where(aa => aa.PeriodStageID == "O").ToList();

            return View(list);
        }
        public ActionResult Integrate(int? id)
        {


            ABESSPEntities db = new ABESSPEntities();
            if (id != null)
            {
                List<PayrollPeriod> payrollPeriods = db.PayrollPeriods.Where(aa => aa.PeriodStageID == "O").ToList();
                if (payrollPeriods.Count() > 1)
                {
                    ModelState.AddModelError("PRName", "More then one Payroll Period are open, Please close last period.");
                }
                else
                {
                    List<VAT_MonthData> monthDataList = db.VAT_MonthData.Where(aa => aa.PayrollPeriodID == id).ToList();
                    foreach (var item in monthDataList)
                    {
                        int EarnedHours = 0;
                        if (item.TWorkTime != null || item.TWorkTime > 0)
                        {
                            EarnedHours = (int)(item.TWorkTime) / 60;
                        }
                        else
                        {
                            EarnedHours = 0;
                        }
                        try
                        {
                            using (var http = new HttpClient())
                            {
                                var data = new ModelType
                                {
                                    EMP_NO = Convert.ToInt16(item.OEmpID),
                                    EARNED_DAYS = (float)item.WorkDays,
                                    PERIOD = item.PRName,
                                    ABSENTS = (float)item.AbsentDays,
                                    LWOP = (float)item.WOPLeavesDays,
                                    EARNED_HOURS = EarnedHours
                                };
                                var content = new StringContent(JsonConvert.SerializeObject(data));
                                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                                var request = http.PostAsync("http://10.60.0.41:8080/ords/mashrest/mash_wms/mash_wms/", content);
                                var response = request.Result.Content.ReadAsStringAsync().Result;
                            }
                        }

                        catch (Exception)
                        {

                            throw;
                        }
                    
                       
                    }
                    ToasterMessages.Add("Monthly Data transferd successfully.");
                    Session["ToasterMessages"] = ToasterMessages;
                }
               
            }
       
            return RedirectToAction("Index");
        }
    }
}