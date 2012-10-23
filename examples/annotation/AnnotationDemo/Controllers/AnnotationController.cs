using System;
using System.Configuration;
using System.Web.Mvc;
using Groupdocs.Sdk;
using Groupdocs.Api.Contract;

namespace AnnotationDemo.Controllers
{
    public class AnnotationController : Controller
    {
        public ActionResult Index(String guid)
        {
            // Pass GUID of the document as model.
            return View("Index", null, guid);
        }

        [HttpPost]
        public ActionResult Upload()
        {
            // Read settings from the configuration file.
            var baseAddress = ConfigurationManager.AppSettings["baseAddress"];
            var userId = ConfigurationManager.AppSettings["userId"];
            var privateKey = ConfigurationManager.AppSettings["privateKey"];

            // Create service for uploading file to Groupdocs account
            var service = new GroupdocsService(baseAddress, userId, privateKey);

            // Iterate through the uploaded files. Right now we have just one uploaded file at a time.
            foreach (string inputTagName in Request.Files)
            {
                var file = Request.Files[inputTagName];
                // Check that file is not fake.
                if (file.ContentLength > 0)
                {
                    // Upload file with empty description.
                    var result = service.UploadFile(file.FileName, String.Empty, file.InputStream);
                    // Redirect to Annotation viewer with received GUID.
                    return RedirectToAction("Index", new {guid = result.Guid});
                }
            }

            // If file was not uploaded redirect to the current page.
            return RedirectToAction("Index");
        }
        [HttpGet]
        public JsonResult Poll(String guid)
        {
            // Read settings from the configuration file.
            var baseAddress = ConfigurationManager.AppSettings["baseAddress"];
            var userId = ConfigurationManager.AppSettings["userId"];
            var privateKey = ConfigurationManager.AppSettings["privateKey"];

            var service = new GroupdocsService(baseAddress, userId, privateKey);
            try
            {
                var response = service.ListAnnotations(guid);
                var output = "";
                foreach (AnnotationInfo annotation in response.Annotations)
                {
                    var replies = "";
                    foreach (AnnotationReplyInfo reply in annotation.Replies)
                    {
                        replies += reply.UserName + ": " + reply.Message;
                    }
                    output += "Annotation Type: " + annotation.Type + " -- Replies: " + replies + "<br/>";
                }
                return Json(output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                
                return Json("Server error or no annotations");
            }
        }
    }
}
