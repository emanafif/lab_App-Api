using Lab_App.DTO;
using Lab_App.Models;
using Lab_App.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Globalization;

namespace Lab_App.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MainController : ControllerBase
    {
        private readonly myDBcontext db;
        private readonly LicenseService _licenseService;


        public MainController(myDBcontext db, LicenseService licenseService)
        {
            this.db = db;
            _licenseService = licenseService;
            
        }

        //public Labs? GetLabDetails(int labId)
        //{
        //    var lab = db.Labs.FirstOrDefault(x => x.lab_id == labId);
        //    return lab != null ? new Labs
        //    {
        //        Na = lab.Na,
        //        Va = lab.Va,
        //        Qras = lab.Qras
        //    } : (Labs?)null;
        //}

        Labs GetLabDetails(int labId)
        {
            return db.Labs.FirstOrDefault(x => x.lab_id == labId);
        }


        [HttpPost("Postdata")]
        public IActionResult postdata(mainDTO model)
        {

            if (ModelState.IsValid)
            {
                var labDetails = GetLabDetails(model.labId);
                if (labDetails == null)
                {
                    return NotFound("Lab details not found.");
                }
                var bod = model.BOD;
                var qin = model.Qin;
                var mlss = model.MLSS;
                var mlsswas = model.MLSSwas;

                var Na = (double)labDetails.Na;
                var Va = (double)labDetails.Va;
                var Qpumpreturn = labDetails.Qpumpreturn;
                var Qpumpplus = labDetails.Qpumpplus;

                
                double? fmbefore = (bod * qin) / (mlss * 0.75 * Na * Va);
                var fm = Math.Round(fmbefore ?? 0.0, 2);
                var Qras = (double)labDetails.Qras;

                var Tss = model.Tss;
                double? Qrasplus = 0.0;
                double? Qrasminus = 0.0;

                double? QrasTotal = 0.0;
                double? QrasPERSCENT = 0.0;
                double? WantedQrasPERSCENT = 0.0;
                if (fm > 0.4)
                {
                    double? mlssbbefore = (bod * qin) / (labDetails.fm * 0.75 * Na * Va);
                    var mlssb = Math.Round(mlssbbefore ?? 0.0, 0);

                    double? mbbefore = (mlssb * 0.75 * Va * Na);
                    var mb = Math.Round(mbbefore ?? 0.0, 0) ;

                    double? mabefore = (mlss * 0.75 * Va * Na);
                    var ma = Math.Round(mabefore ?? 0.0, 0);


                    double? qrasplus = (mb - ma)/(0.75* mlsswas);
                    Qrasplus = qrasplus;

                    double? qrastotal = Qrasplus + Qras;
                    QrasTotal = qrastotal;

                    double? Qtimeras = QrasTotal / Qpumpreturn;
                    double? qrasPERSCENT = (QrasTotal / Qras) * 100;
                    QrasPERSCENT = qrasPERSCENT;

                    double? wantedQrasPERSCENT = QrasPERSCENT - 100;
                    WantedQrasPERSCENT = wantedQrasPERSCENT;

                    var entity = new mainTable()
                    {
                        BOD = bod,
                        Qin = qin,
                        MLSS = mlss,
                        MLSSwas=mlsswas,
                        Na = Na,
                        Va = Va,
                        FM = fm,
                        Qpumpreturn=Qpumpreturn,
                        Qpumpplus= Qpumpplus,
                        Qtimeras= Math.Round(Qtimeras ?? 0.0, 1),
                        MLSSB = mlssb ,
                        MB = mb,
                        Mx = ma,
                        Qrasplus = Math.Round(Qrasplus ?? 0.0, 0),
                        QrasTotal = Math.Round(QrasTotal ?? 0.0, 0),
                        QrasPERSCENT = Math.Round(QrasPERSCENT ?? 0.0, 0),
                        WantedQrasPERSCENT = Math.Round(WantedQrasPERSCENT ?? 0.0, 0),
                        date = model.date,
                        labId= model.labId

                    };
                    db.Add(entity);
                    db.SaveChanges();

                    return Ok(entity);
                }

                else if (fm < 0.2)
                {
                    double? mlssbbefore = (bod * qin) / (labDetails.fm * 0.75 * Na * Va);
                    var mlssb = Math.Round(mlssbbefore ?? 0.0, 0);

                    double? mbbefore = (mlssb * 0.75 * Va * Na);
                    var mb = Math.Round(mbbefore ?? 0.0, 0);

                    double? mabefore = (mlss * 0.75 * Va * Na);
                    var ma = Math.Round(mabefore ?? 0.0, 0);
                    //var ma = (model.MLSS * 0.75 * Va * Na);


                    double? qrasminus = (ma - mb)/(0.75* mlsswas);
                    Qrasminus = qrasminus;

                    double? qrastotal = Qras - Qrasminus;
                    QrasTotal = qrastotal;
                    double? Qtimeras = QrasTotal / Qpumpreturn;

                    var qrasPERSCENT = (QrasTotal / Qras) * 100;
                    QrasPERSCENT = qrasPERSCENT;

                    var wantedQrasPERSCENT = 100 - QrasPERSCENT;
                    WantedQrasPERSCENT = wantedQrasPERSCENT;

                    var entity = new mainTable()
                    {
                        BOD = bod,
                        Qin = qin,
                        MLSS = mlss,
                        MLSSwas=mlsswas,
                        Na = Na,
                        Va = Va,
                        FM = fm,
                        Qpumpreturn = Qpumpreturn,
                        Qpumpplus = Qpumpplus,
                        Qtimeras = Math.Round(Qtimeras ?? 0.0, 1),
                        MLSSB = mlssb,
                        MB = mb,
                        Mx = ma,
                        //Qrasplus = Qrasplus,
                        Qrasminus = Math.Round(Qrasminus ?? 0.0, 0),
                        QrasTotal = Math.Round(QrasTotal ?? 0.0, 0),
                        QrasPERSCENT = Math.Round(QrasPERSCENT ?? 0.0, 0),
                        WantedQrasPERSCENT = Math.Round(WantedQrasPERSCENT ?? 0.0, 0),
                        date = model.date,
                        labId = model.labId

                    };
                    db.Add(entity);
                    db.SaveChanges();

                    return Ok(entity);
                }


                else
                {
                    double? mlssbbefore = (bod * qin) / (0.3 * 0.75 * Na * Va);
                    var mlssb = Math.Round(mlssbbefore ?? 0.0, 0);

                    double? mbbefore = (mlssb * 0.75 * Va * Na);
                    var mb = Math.Round(mbbefore ?? 0.0, 0);

                    double? mabefore = (mlss * 0.75 * Va * Na);
                    var ma = Math.Round(mabefore ?? 0.0, 0);

                    double? qrasplus = (ma - mb) / (0.75 * mlsswas);
                    Qrasplus = qrasplus;

                    double? qrastotal = Qras - Qrasplus;
                    QrasTotal = qrastotal;
                    double? Qtimeras = QrasTotal / Qpumpreturn;

                    var qrasPERSCENT = (QrasTotal / Qras) * 100;
                    QrasPERSCENT = qrasPERSCENT;

                    var wantedQrasPERSCENT = 100 - QrasPERSCENT;
                    WantedQrasPERSCENT = wantedQrasPERSCENT;

                    var entity = new mainTable()
                    {
                        BOD = bod,
                        Qin = qin,
                        MLSS = mlss,
                        MLSSwas=mlsswas,
                        Na = Na,
                        Va = Va,
                        FM = fm,
                        Qpumpreturn = Qpumpreturn,
                        Qpumpplus = Qpumpplus,
                        Qtimeras = Math.Round(Qtimeras ?? 0.0, 1),
                        MLSSB = mlssb,
                        MB = mb,
                        Mx = ma,
                        Qrasplus = Math.Round(Qrasplus ?? 0.0, 0),
                        QrasTotal = Math.Round(QrasTotal ?? 0.0, 0),
                        QrasPERSCENT = Math.Round(QrasPERSCENT ?? 0.0, 0),
                        WantedQrasPERSCENT = Math.Round(WantedQrasPERSCENT ?? 0.0, 0),
                        date = model.date,
                        labId = model.labId

                    };
                    db.Add(entity);
                    db.SaveChanges();

                    return Ok(entity);
                }
            }

            return BadRequest(ModelState);
        }


        [Authorize(Roles = "ITadmin")]
        [HttpPost("addlab")]
        public IActionResult addLab(LabDTO model)
        {
            if (ModelState.IsValid)
            {
                var lab = new Labs
                {
                    lab_name = model.lab_name,
                    Na = model.Na,
                    Va = model.Va,
                    Qras = model.Qras,
                    Qpumpreturn = model.Qpumpreturn,
                    fm = model.fm,
                };
                db.Add(lab);
                db.SaveChanges();
                return Ok(lab);

            }
            return BadRequest(model);

        }

        //public IActionResult getcurrentlabData(LabDTO model, int id) {
        //    var entity = db.Labs.Find(id);



        //}


        [HttpGet("getlabbyid")]
        public IActionResult GetLabById(int id)
        {
            var entity = db.Labs.Find(id);

            var model = new getLabbyidDTO
            {
                lab_id = entity.lab_id,
                fm = entity.fm,
                Qpumpreturn = entity.Qpumpreturn,
                lab_name = entity.lab_name,
                Va = entity.Va,
                Na = entity.Na,
               Qras = entity.Qras,
            };
            return Ok(model);
        }

        [Authorize(Roles = "admin")]
        [HttpPost("updatelab")]
        public IActionResult Updateabdata(int id, getLabbyidDTO model)
        {

            if (ModelState.IsValid)
            {
                var entity = db.Labs.FirstOrDefault(x => x.lab_id == id);

                if (entity != null)
                {
                    entity.lab_id = id;
                    entity.lab_name = model.lab_name;
                    entity.Qpumpreturn = model.Qpumpreturn;
                    entity.fm = model.fm;
                    entity.Va = model.Va;
                    entity.Na = model.Na;
                    entity.Qras = model.Qras;
                    db.SaveChanges();
                    return StatusCode(204, "datasaved");
                }
            }
            return BadRequest(model);
        }


        //[HttpPost("Postdata")]
        //public IActionResult postdata(mainDTO model)
        //{

        //    if (ModelState.IsValid)
        //    {

        //        var bod = model.BOD;
        //        var qin = model.Qin;
        //        var mlss = model.MLSS;
        //        var Qras = model.Qras;
        //        var Na = model.Na;
        //        var Va = model.Va;
        //        var Tss = model.Tss;
        //        var fm = (bod * qin) / (mlss * 0.75 * Na * Va);
        //        var mlssb = (bod * qin) / (0.3 * 0.75 * Na * Va);
        //        var mb = (mlssb * 0.75 * Va * Na) ;
        //        var ma = (model.MLSS * 0.75 * Va * Na) ;
        //        double? Qrasplus = 0.0;
        //        double? QrasTotal = 0.0;
        //        double? QrasPERSCENT = 0.0;
        //        double? WantedQrasPERSCENT = 0.0;
        //        if (fm > 0.4)
        //        {
        //            double? qrasplus =( mb - ma)/(0.75* mlss);
        //            Qrasplus = qrasplus;

        //            double? qrastotal = Qrasplus + Qras;
        //            QrasTotal = qrastotal;

        //            double? qrasPERSCENT = (QrasTotal / Qras) * 100;
        //            QrasPERSCENT = qrasPERSCENT;

        //            double? wantedQrasPERSCENT = QrasPERSCENT - 100;
        //            WantedQrasPERSCENT = wantedQrasPERSCENT;

        //            var entity = new mainTable()
        //            {
        //                BOD = bod,
        //                Qin = qin,
        //                MLSS = mlss,
        //                Na = Na,
        //                Va = Va,
        //                FM = fm,
        //                MLSSB = mlssb,
        //                MB = mb,
        //                Mx = ma,
        //                Qrasplus = Qrasplus,
        //                QrasTotal = QrasTotal,
        //                QrasPERSCENT = QrasPERSCENT,
        //                WantedQrasPERSCENT = WantedQrasPERSCENT,
        //                date = model.date,
        //                labId=model.labId,


        //            };
        //            db.Add(entity);
        //            db.SaveChanges();

        //            return Ok(entity);
        //        }

        //        else if (fm < 0.2)
        //        {
        //            double? qrasplus =( ma - mb) / (0.75 * mlss);
        //            Qrasplus = qrasplus;

        //            double? qrastotal = Qras - Qrasplus;
        //            QrasTotal = qrastotal;

        //            var qrasPERSCENT = (QrasTotal / Qras) * 100;
        //            QrasPERSCENT = qrasPERSCENT;

        //            var wantedQrasPERSCENT = 100 - QrasPERSCENT;
        //            WantedQrasPERSCENT = wantedQrasPERSCENT;

        //            var entity = new mainTable()
        //            {
        //                BOD = bod,
        //                Qin = qin,
        //                MLSS = mlss,
        //                Na = Na,
        //                Va = Va,
        //                FM = fm,
        //                MLSSB = mlssb,
        //                MB = mb,
        //                Mx = ma,
        //                Qrasplus = Qrasplus,
        //                QrasTotal = QrasTotal,
        //                QrasPERSCENT = QrasPERSCENT,
        //                WantedQrasPERSCENT = WantedQrasPERSCENT,
        //                date = model.date

        //            };
        //            db.Add(entity);
        //            db.SaveChanges();

        //            return Ok(entity);
        //        }


        //        else
        //        {
        //            double? qrasplus = (ma - mb) / (0.75 * mlss);
        //            Qrasplus = qrasplus;

        //            double? qrastotal = Qras - Qrasplus;
        //            QrasTotal = qrastotal;

        //            var qrasPERSCENT = (QrasTotal / Qras) * 100;
        //            QrasPERSCENT = qrasPERSCENT;

        //            var wantedQrasPERSCENT = 100 - QrasPERSCENT;
        //            WantedQrasPERSCENT = wantedQrasPERSCENT;

        //            var entity = new mainTable()
        //            {
        //                BOD = bod,
        //                Qin = qin,
        //                MLSS = mlss,
        //                Na = Na,
        //                Va = Va,
        //                FM = fm,
        //                MLSSB = mlssb,
        //                MB = mb,
        //                Mx = ma,
        //                Qrasplus = Qrasplus,
        //                QrasTotal = QrasTotal,
        //                QrasPERSCENT = QrasPERSCENT,
        //                WantedQrasPERSCENT = WantedQrasPERSCENT,
        //                date = model.date

        //            };
        //            db.Add(entity);
        //            db.SaveChanges();

        //            return Ok(entity);
        //        }
        //    }
        //    return BadRequest(ModelState);
        //}

        [HttpGet("allaExperments/{labid}")]
        public IActionResult showallaExperments(int labid)
        {
            var allaExperments = db.mainTableS.Select(x => new allexpermentsDTO
            {
                Id = x.Id,
                BOD=x.BOD,
                Qin = x.Qin,
                MLSS = x.MLSS,
                MLSSwas=x.MLSSwas,
                Na = x.Na,
                Va = x.Va,
                FM = x.FM,
                Qpumpreturn =x. Qpumpreturn,
                Qpumpplus = x.Qpumpplus,
                Qtimeras =x. Qtimeras,
                MLSSB = x.MLSSB,
                MB = x.MB,
                Mx= x.Mx,
                Qrasplus= x.Qrasplus,
                Qrasminus=x.Qrasminus,
                QrasTotal = x.QrasTotal,
                QrasPERSCENT= x.QrasPERSCENT,
                WantedQrasPERSCENT=x.WantedQrasPERSCENT,
                date = x.date,
                labname = x.Labs.lab_name,
                labId = x.labId,

            }).Where(x=>x.labId==labid).OrderByDescending(m => m.Id);
            return Ok(allaExperments);
        }

        [HttpGet("{id:int}")]
        public IActionResult getEXPbyId(int id)
        {
            var exp = db.mainTableS.Where(x => x.Id == id).Select(x => new allexpermentsDTO
            {
                Id = x.Id,
                BOD = x.BOD,
                Qin = x.Qin,
                MLSS = x.MLSS,
                MLSSwas = x.MLSSwas,
                Na = x.Na,
                Va = x.Va,
                FM = x.FM,
                Qpumpreturn = x.Qpumpreturn,
                Qpumpplus = x.Qpumpplus,
                Qtimeras = x.Qtimeras,
                MLSSB = x.MLSSB,
                MB = x.MB,
                Mx = x.Mx,
                Qrasplus = x.Qrasplus,
                Qrasminus = x.Qrasminus,
                QrasTotal = x.QrasTotal,
                QrasPERSCENT = x.QrasPERSCENT,
                WantedQrasPERSCENT = x.WantedQrasPERSCENT,
                date = x.date
            }
              );
            return Ok( exp );
        }


        [HttpGet("searchbydate/{date1}")]
        public IActionResult search([FromRoute] DateTime date1, int labid)
        {

            var result = db.mainTableS.Where(x => ((x.date == date1) && x.labId==labid)
           ).Select(x => new allexpermentsDTO
           {
               Id = x.Id,
               BOD = x.BOD,
               Qin = x.Qin,
               MLSS = x.MLSS,
               MLSSwas = x.MLSSwas,

               Na = x.Na,
               Va = x.Va,
               FM = x.FM,
               Qpumpreturn = x.Qpumpreturn,
               Qpumpplus = x.Qpumpplus,
               Qtimeras = x.Qtimeras,
               MLSSB = x.MLSSB,
               MB = x.MB,
               Mx = x.Mx,
               Qrasplus = x.Qrasplus,
               QrasTotal = x.QrasTotal,
               QrasPERSCENT = x.QrasPERSCENT,
               WantedQrasPERSCENT = x.WantedQrasPERSCENT,
               date = x.date,
                labname = x.Labs.lab_name,
               labId = x.labId,

           }
           );
            return Ok(result);
        }

        [HttpGet("searchby2date/{date1}/{date2}")]
        public IActionResult searchby2dates([FromRoute] DateTime date1, [FromRoute] DateTime date2, int labid)
        {

            var result = db.mainTableS.Where(x => ((x.date >= date1 && date2.AddDays(1) > x.date && x.labId == labid))
           ).Select(x => new allexpermentsDTO
           {
               Id = x.Id,
               BOD = x.BOD,
               Qin = x.Qin,
               MLSS = x.MLSS,
               MLSSwas = x.MLSSwas,
               Na = x.Na,
               Va = x.Va,
               FM = x.FM,
               Qpumpreturn = x.Qpumpreturn,
               Qpumpplus = x.Qpumpplus,
               Qtimeras = x.Qtimeras,
               MLSSB = x.MLSSB,
               MB = x.MB,
               Mx = x.Mx,
               Qrasplus = x.Qrasplus,
               QrasTotal = x.QrasTotal,
               QrasPERSCENT = x.QrasPERSCENT,
               WantedQrasPERSCENT = x.WantedQrasPERSCENT,
               date = x.date,
               labname = x.Labs.lab_name,
               labId = x.labId,

           }
           );
            return Ok(result);
        }

        // license


        string ConvertToArabicNumbers(string input)
        {
            return input.Replace('0', '٠')
                        .Replace('1', '١')
                        .Replace('2', '٢')
                        .Replace('3', '٣')
                        .Replace('4', '٤')
                        .Replace('5', '٥')
                        .Replace('6', '٦')
                        .Replace('7', '٧')
                        .Replace('8', '٨')
                        .Replace('9', '٩');
        }


        [HttpPost("validate/{licenseKey}")]
        public IActionResult ValidateLicense( string licenseKey)
        {
            if (string.IsNullOrEmpty(licenseKey))
            {
                Console.WriteLine("🚨 مفتاح الترخيص غير موجود في الطلب!");
                return BadRequest(new { message = "مفتاح الترخيص مطلوب." });
            }

            bool isValid = _licenseService.ValidateLicense(licenseKey);
            DateTime expiration = _licenseService.getexpirationdate(licenseKey);

            if (!isValid)
            {             
                Console.WriteLine("🚨 مفتاح الترخيص غير صالح أو منتهي الصلاحية.");
                return Unauthorized(new
                {
                    message = "مفتاح الترخيص  منتهي الصلاحية.",
                    //expiration = $" تاريخ الانتهاء {expiration.ToShortDateString()} "
                    expiration = "  تاريخ الانتهاء  : " + ConvertToArabicNumbers(expiration.ToString("dd -MM-yyyy")) 
                    
                });
            }
            Console.WriteLine("✅ مفتاح الترخيص صالح!");
            return Ok(new { message = "مفتاح الترخيص صالح." });
        }


        


        
    }
}
