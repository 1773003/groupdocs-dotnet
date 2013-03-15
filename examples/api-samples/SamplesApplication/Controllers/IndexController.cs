using System;
using System.Configuration;
using System.Web.Mvc;
using Groupdocs.Sdk;

namespace SamplesApp.Controllers
{
    public class SamplesController : Controller
    {
        public ActionResult Index(String result)
        {
            // Pass GUID of the document as model.
            return View("Index", null, result);
        }
        
        public ActionResult Sample01()
        {
            
            if (Request.HttpMethod == "POST")
            {
                System.Collections.Hashtable result = new System.Collections.Hashtable();
                String userId = Request.Form["client_id"];
                String private_key = Request.Form["private_key"];
                result.Add("client_id", userId);
                result.Add("private_key", private_key);
                Groupdocs.Api.Contract.UserInfoResult userInfo = null;
                String message = null;

                if (userId == null || private_key == null)
                {
                    message = "Please enter all parameters";
                    result.Add("error", message);

                    return View("Sample01", null, result);
                }
                else
                {
                    // Create service for uploading file to Groupdocs account
                    GroupdocsService service = new GroupdocsService("https://api.groupdocs.com/v2.0", userId, private_key);
                    userInfo = service.GetUserProfile();
                    result.Add("FirstName", userInfo.User.FirstName);
                    result.Add("LastName", userInfo.User.LastName);
                    result.Add("NickName", userInfo.User.NickName);
                    result.Add("PrimaryEmail", userInfo.User.PrimaryEmail);
                    return View("Sample01", null, result);
                }
            }
            else
            {
                return View("Sample01");
            }
        }

        public ActionResult Sample02()
        {
            if (Request.HttpMethod == "POST")
            {
                System.Collections.Hashtable result = new System.Collections.Hashtable();
                String userId = Request.Form["client_id"];
                String private_key = Request.Form["private_key"];
                result.Add("client_id", userId);
                result.Add("private_key", private_key);
                Groupdocs.Api.Contract.ListEntitiesResult files = null;
                String message = null;

                if (userId == null || private_key == null)
                {
                    message = "Please enter all parameters";
                    result.Add("error", message);

                    return View("Sample02", null, result);
                }
                else
                {
                    GroupdocsService service = new GroupdocsService("https://api.groupdocs.com/v2.0", userId, private_key);
                    files = service.GetFileSystemEntities("", 0, -1, null, true, null, null);

                    if (files.Files != null)
                    {
                        String name = null;
                        TagBuilder tag = new TagBuilder("br");


                        for (int i = 0; i < files.Files.Length; i++)
                        {
                            name += files.Files[i].Name + tag;

                        }

                        MvcHtmlString names = MvcHtmlString.Create(name);
                        result.Add("Names", names);
                        return View("Sample02", null, result);
                    }
                    else
                    {
                        message = "GetFileSystemEntities returns error";
                        result.Add("error", message);
                        return View("Sample02", null, result);
                    }
                }
            }
            else
            {
                return View("Sample02");
            }
        }

        public ActionResult Sample03()
        {
            if (Request.HttpMethod == "POST")
            {
                System.Collections.Hashtable result = new System.Collections.Hashtable();
                String userId = Request.Form["client_id"];
                String private_key = Request.Form["private_key"];
                result.Add("client_id", userId);
                result.Add("private_key", private_key);
                Groupdocs.Api.Contract.UploadRequestResult upload = null;
                String message = null;

                if (userId == null || private_key == null || Request.Files == null)
                {
                    message = "Please enter all parameters";
                    result.Add("error", message);

                    return View("Sample03", null, result);
                }
                else
                {
                    GroupdocsService service = new GroupdocsService("https://api.groupdocs.com/v2.0", userId, private_key);
                    foreach (string inputTagName in Request.Files)
                    {
                        var file = Request.Files[inputTagName];
                        // Check that file is not fake.
                        if (file.ContentLength > 0)
                        {
                            // Upload file with empty description.
                            upload = service.UploadFile(file.FileName, String.Empty, file.InputStream);

                            if (upload.Guid != null)
                            {
                                result.Add("guid", upload.Guid);
                                // Redirect to viewer with received GUID.
                            }
                            else
                            {
                                message = "UploadFile returns error";
                                result.Add("error", message);
                                return View("Sample03", null, result);
                            }
                        }
                    }

                    return View("Sample03", null, result);
                }
            }
            else
            {
                return View("Sample03");
            }
        }

        public ActionResult Sample04()
        {
            if (Request.HttpMethod == "POST")
            {
                System.Collections.Hashtable result = new System.Collections.Hashtable();
                String userId = Request.Form["client_id"];
                String private_key = Request.Form["private_key"];
                String fileId = Request.Form["fileId"];
                result.Add("client_id", userId);
                result.Add("private_key", private_key);
                result.Add("fileId", fileId);
             
                String message = null;

                if (userId == null || private_key == null || fileId == null)
                {
                    message = "Please enter all parameters";
                    result.Add("error", message);

                    return View("Sample04", null, result);
                }
                else
                {
                    GroupdocsService service = new GroupdocsService("https://api.groupdocs.com/v2.0", userId, private_key);
                    Groupdocs.Api.Contract.ListEntitiesResult files = service.GetFileSystemEntities("", 0, -1, null, true, null, null);
                    String name = null;

                    if (files.Files != null)
                    {
                        
                        for (int i = 0; i < files.Files.Length; i++)
                        {
                            if (files.Files[i].Guid == fileId)
                            {
                                name = files.Files[i].Name;
                            }

                        }
                    }
                    String LocalPath = AppDomain.CurrentDomain.BaseDirectory + "downloads/";

                    bool file = service.DownloadFile(fileId, LocalPath + name);
                    
                    if (file != false)
                    {
                        result.Add("path", LocalPath);
                        result.Add("name", name);
                        return View("Sample04", null, result);
                    }
                    else
                    {
                        message = "DownloadFile returns error";
                        result.Add("error", message);
                        return View("Sample04", null, result);
                    }
                    
                }

            }
            else
            {
                return View("Sample04");
            }
        }

        public ActionResult Sample05()
        {
            if (Request.HttpMethod == "POST")
            {
                System.Collections.Hashtable result = new System.Collections.Hashtable();
                String userId = Request.Form["client_id"];
                String private_key = Request.Form["private_key"];
                String fileId = Request.Form["srcPath"];
                String destPath = Request.Form["destPath"];
                String action = Request.Form["copy"];

                result.Add("client_id", userId);
                result.Add("private_key", private_key);
                result.Add("fileId", fileId);
                result.Add("destPath", destPath);

                String message = null;

                if (userId == "" || private_key == "" || fileId == "" || destPath == "")
                {
                    message = "Please enter all parameters";
                    result.Add("error", message);

                    return View("Sample05", null, result);
                }
                else
                {
                    GroupdocsService service = new GroupdocsService("https://api.groupdocs.com/v2.0", userId, private_key);
                    Groupdocs.Api.Contract.ListEntitiesResult files = service.GetFileSystemEntities("", 0, -1, null, true, null, null);
                    String name = null;
                    decimal id = new decimal();

                    if (files.Files != null)
                    {

                        for (int i = 0; i < files.Files.Length; i++)
                        {
                            if (files.Files[i].Guid == fileId)
                            {
                                name = files.Files[i].Name;
                                id = files.Files[i].Id;
                            }

                        }

                        if (name == null || id == 0)
                        {
                            result.Add("error", "File was not found");
                           return View("Sample05", null, result);
                        }

                        String path = destPath + "/" + name;
                        Groupdocs.Api.Contract.FileMoveResponse transfer = new Groupdocs.Api.Contract.FileMoveResponse();
                        if (action == "Copy")
                        {
                            Groupdocs.Common.OverrideMode overide = new Groupdocs.Common.OverrideMode();
                            transfer = service.CopyFile(id, path, overide);
                            result.Add("button", "Copied");

                        }
                        else
                        {
                            Groupdocs.Common.OverrideMode overide = new Groupdocs.Common.OverrideMode();
                            transfer = service.MoveFile(id, path, overide);
                            result.Add("button", "Moved");
                        }

                        if (transfer.Status.Equals("Ok"))
                        {
                            result.Add("path", path);
                        }
                        else
                        {
                            result.Add("error", transfer.ErrorMessage);
                        }

                    }

                   return View("Sample05", null, result);         
                }

            }
            else
            {
                return View("Sample05");
            }
                        
        }

        public ActionResult Sample06()
        {
            if (Request.HttpMethod == "POST")
            {
                System.Collections.Hashtable result = new System.Collections.Hashtable();
                String userId = Request.Form["client_id"];
                String private_key = Request.Form["private_key"];
                var documetToSign = Request.Files["fi_document"];
                var signature = Request.Files["fi_signature"];
                result.Add("client_id", userId);
                result.Add("private_key", private_key);
                Groupdocs.Api.Contract.UploadRequestResult upload = null;
                String message = null;

                if (userId == null || private_key == null || Request.Files.Count == 0)
                {
                    message = "Please enter all parameters";
                    result.Add("error", message);

                    return View("Sample06", null, result);
                }
                else
                {
                    GroupdocsService service = new GroupdocsService("https://api.groupdocs.com/v2.0", userId, private_key);
                    upload = service.UploadFile(documetToSign.FileName, String.Empty, documetToSign.InputStream);

                    if (upload.Guid != null)
                    {

                        System.IO.BinaryReader reader = new System.IO.BinaryReader(signature.InputStream);
                        byte[] bytedata = reader.ReadBytes((Int32)signature.ContentLength);
                        String data = "data:image/gif;base64," + Convert.ToBase64String(bytedata);
                        Groupdocs.Api.Contract.SignatureSignDocumentSignerSettings signerSettings = new Groupdocs.Api.Contract.SignatureSignDocumentSignerSettings();
                        signerSettings.PlaceSignatureOn = "";
                        signerSettings.Name = "GroupDocs";
                        signerSettings.Data =  data;
                        signerSettings.Height = new decimal(40d);
                        signerSettings.Width = new decimal(100d);
                        signerSettings.Top = new decimal(0.83319);
                        signerSettings.Left = new decimal(0.72171);
                        Groupdocs.Api.Contract.SignatureSignDocumentResponse sign = service.SignDocument(upload.Guid, signerSettings);
                            
                        if (sign.Status.Equals("Ok"))
                        {
                            result.Add("guid", sign.Result.DocumentId);
                            // Redirect to viewer with received GUID.
                        }
                        else
                        {
                            message = sign.ErrorMessage;
                            result.Add("error", message);
                            return View("Sample06", null, result);
                        }
                    }

                }
                   
                return View("Sample06", null, result);
                
            }
            else
            {
                return View("Sample06");
            }
        }

        public ActionResult Sample07()
        {
            if (Request.HttpMethod == "POST")
            {
                System.Collections.Hashtable result = new System.Collections.Hashtable();
                String userId = Request.Form["client_id"];
                String private_key = Request.Form["private_key"];
                result.Add("client_id", userId);
                result.Add("private_key", private_key);
                Groupdocs.Api.Contract.ListEntitiesResult files = null;
                String message = null;

                if (userId == null || private_key == null)
                {
                    message = "Please enter all parameters";
                    result.Add("error", message);

                    return View("Sample07", null, result);
                }
                else
                {
                    GroupdocsService service = new GroupdocsService("https://api.groupdocs.com/v2.0", userId, private_key);
                    files = service.GetFileSystemEntities("", 0, -1, null, false, null, null, true);
                    
                    if (files.Files != null)
                    {
                        String name = "";
                        String image = "";
                        TagBuilder tagImg = new TagBuilder("img");
                        TagBuilder tagBr = new TagBuilder("br");

                        for (int i = 0; i < files.Files.Length; i++)
                        {
                            if (files.Files[i].Thumbnail != null)
                            {

                                int nameLength = files.Files[i].Name.Length;
                                name = files.Files[i].Name.Substring(0, nameLength - 4);
                                String LocalPath = AppDomain.CurrentDomain.BaseDirectory + "downloads/" + name + ".jpg";
                                //System.IO.StreamWriter sw = System.IO.File.CreateText(LocalPath);
                                              
                                String data = Convert.ToBase64String(files.Files[i].Thumbnail);
                                byte[] imageBytes = Convert.FromBase64String(data);
                                System.IO.MemoryStream ms = new System.IO.MemoryStream(imageBytes, 0, imageBytes.Length);
                                ms.Write(imageBytes, 0, imageBytes.Length);
                                System.Drawing.Image img = System.Drawing.Image.FromStream(ms, true);
                                img.Save(LocalPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                                img.Dispose();
                                ms.Close();
                                tagImg.MergeAttribute("src", "/../downloads/" + name + ".jpg", true);
                                tagImg.MergeAttribute("width", "40px");
                                tagImg.MergeAttribute("height", "40px");
                                image += tagImg.ToString(TagRenderMode.SelfClosing) + files.Files[i].Name + tagBr;
                                name = null;                              
                            }

                        }

                        MvcHtmlString images = MvcHtmlString.Create(image);
                        result.Add("images", images);
                        return View("Sample07", null, result);
                    }
                    else
                    {
                        message = "GetFileSystemEntities returns error";
                        result.Add("error", message);
                        return View("Sample07", null, result);
                    }
                }
            }
            else
            {
                return View("Sample07");
            }
        }

        public ActionResult Sample08()
        {
            if (Request.HttpMethod == "POST")
            {
                System.Collections.Hashtable result = new System.Collections.Hashtable();
                String userId = Request.Form["client_id"];
                String private_key = Request.Form["private_key"];
                String fileId = Request.Form["fileId"];
                String pageNumber = Request.Form["pageNumber"];
                result.Add("client_id", userId);
                result.Add("private_key", private_key);
                result.Add("fileId", fileId);

                String message = null;

                if (userId == null || private_key == null || fileId == null)
                {
                    message = "Please enter all parameters";
                    result.Add("error", message);

                    return View("Sample08", null, result);
                }
                else
                {
                    GroupdocsService service = new GroupdocsService("https://api.groupdocs.com/v2.0", userId, private_key);
                    String[] url = service.GetDocumentPagesImageUrls(fileId, "600x750", String.Empty, Int32.Parse(pageNumber), 1, 100, false);
                    
                    if (url != null)
                    {
                        result.Add("url", url[0]);
                        return View("Sample08", null, result);
                    }
                    else
                    {
                        message = "Something is wrong with your data";
                        result.Add("error", message);
                        return View("Sample08", null, result);
                    }

                }

            }
            else
            {
                return View("Sample08");
            }
        }

        public ActionResult Sample09()
        {
            if (Request.HttpMethod == "POST")
            {
                System.Collections.Hashtable result = new System.Collections.Hashtable();
                String width = Request.Form["width"];
                String height = Request.Form["height"];
                String fileId = Request.Form["fileId"];
                result.Add("width", width);
                result.Add("height", height);
                result.Add("fileId", fileId);

                String message = null;

                if (fileId == null)
                {
                    message = "Please enter file id";
                    result.Add("error", message);

                    return View("Sample09", null, result);
                }
                else
                {
                     String iframe = "https://apps.groupdocs.com/document-viewer/embed/" + fileId + " frameborder=0 width=" + width + " height=" + height;
                     result.Add("iframe", iframe);
                     return View("Sample09", null, result);
                    
                }

            }
            else
            {
                return View("Sample09");
            }
        }

        public ActionResult Sample10()
        {
            if (Request.HttpMethod == "POST")
            {
                System.Collections.Hashtable result = new System.Collections.Hashtable();
                String userId = Request.Form["client_id"];
                String private_key = Request.Form["private_key"];
                String fileId = Request.Form["fileId"];
                String email = Request.Form["email"];
                result.Add("client_id", userId);
                result.Add("private_key", private_key);
                result.Add("fileId", fileId);
                result.Add("email", email);
                String[] sharers = new String[1];
                sharers[0] = email;
                String message = null;

                if (userId == null || private_key == null || fileId == null || email == null)
                {
                    message = "Please enter all parameters";
                    result.Add("error", message);

                    return View("Sample10", null, result);
                }
                else
                {
                    GroupdocsService service = new GroupdocsService("https://api.groupdocs.com/v2.0", userId, private_key);
                    Groupdocs.Api.Contract.ListEntitiesResult files = service.GetFileSystemEntities("", 0, -1, null, true, null, null);
                    decimal id = new decimal();

                    for (int i = 0; i < files.Files.Length; i++) 
                    {
                        if (files.Files[i].Guid == fileId)
                        {
                            id = files.Files[i].Id;
                        }
                        
                    }

                    if (id == null)
                    {
                        
                        message = "File GuId you are entered is wrong";
                        result.Add("error", message);
                        return View("Sample10", null, result);
                        
                    }

                    Groupdocs.Api.Contract.SharedUsersResult share = service.ShareDocument(id, sharers);

                    if (share != null)
                    {
                        result.Add("sharer", share.SharedUsers[0].PrimaryEmail);
                        return View("Sample10", null, result);
                    }
                    else
                    {
                        message = "Something is wrong with your data";
                        result.Add("error", message);
                        return View("Sample10", null, result);
                    }

                }

            }
            else
            {
                return View("Sample10");
            }
        }

    }
}
