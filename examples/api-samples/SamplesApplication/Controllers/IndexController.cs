using System;
using System.Configuration;
using System.Web.Mvc;
using Groupdocs.Sdk;

namespace SamplesApp.Controllers
{
    public class SamplesController : Controller
    {
        //### Default Index controller
        public ActionResult Index(String result)
        {
            // Pass GUID of the document as model.
            return View("Index", null, result);
        }

        //### Docs controller
       //  public ActionResult Docs(String result)
        //{
            // Pass GUID of the document as model.

         //   return View("/samples/docs/controllers/indexcontroller.html");
        //}

        //### <i>This sample will show how to use <b>Signer object</b> to be authorized at GroupDocs and how to get GroupDocs user infromation using .NET SDK</i>
        public ActionResult Sample01()
        {
            // Check is data posted    
            if (Request.HttpMethod == "POST")
            {
                //### Set variables and get POST data
                System.Collections.Hashtable result = new System.Collections.Hashtable();
                String userId = Request.Form["client_id"];
                String private_key = Request.Form["private_key"];
                result.Add("client_id", userId);
                result.Add("private_key", private_key);
                Groupdocs.Api.Contract.UserInfoResult userInfo = null;
                String message = null;
                // Check is all needed fields are entered
                if (userId == null || private_key == null)
                {
                    //If not all fields entered send error message
                    message = "Please enter all parameters";
                    result.Add("error", message);
                    // Transfer error message to template
                    return View("Sample01", null, result);
                }
                else
                {
                    // Create service for Groupdocs account
                    GroupdocsService service = new GroupdocsService("https://api.groupdocs.com/v2.0", userId, private_key);
                    // Get info about user account
                    userInfo = service.GetUserProfile();
                    //### Put user info to Hashtable
                    result.Add("FirstName", userInfo.User.FirstName);
                    result.Add("LastName", userInfo.User.LastName);
                    result.Add("NickName", userInfo.User.NickName);
                    result.Add("PrimaryEmail", userInfo.User.PrimaryEmail);
                    // Return Hashtable with results to the template
                    return View("Sample01", null, result);
                }
            }
            // If data not posted return to template for filling of necessary fields
            else
            {
                return View("Sample01");
            }
        }
        //### <i>This sample will show how to use <b>GetFileSystemEntities</b> method from Storage  API  to list files within GroupDocs Storage</i>
        public ActionResult Sample02()
        {
            // Check is data posted   
            if (Request.HttpMethod == "POST")
            {
                //### Set variables and get POST data
                System.Collections.Hashtable result = new System.Collections.Hashtable();
                String userId = Request.Form["client_id"];
                String private_key = Request.Form["private_key"];
                result.Add("client_id", userId);
                result.Add("private_key", private_key);
                Groupdocs.Api.Contract.ListEntitiesResult files = null;
                String message = null;
                // Check is all needed fields are entered
                if (userId == null || private_key == null)
                {
                    // If not all fields entered send error message
                    message = "Please enter all parameters";
                    result.Add("error", message);
                    // Transfer error message to template
                    return View("Sample02", null, result);
                }
                else
                {
                    // Create service for Groupdocs account
                    GroupdocsService service = new GroupdocsService("https://api.groupdocs.com/v2.0", userId, private_key);
                    //Get all files and folders from account. 
                    files = service.GetFileSystemEntities("", 0, -1, null, true, null, null);
                    //Check request result
                    if (files.Files != null)
                    {
                        // Create empty variable for results
                        String name = null;
                        // Create TagBuilder for adding HTML tag to the string
                        TagBuilder tag = new TagBuilder("br");

                        // Obtaining all files names
                        for (int i = 0; i < files.Files.Length; i++)
                        {
                            // Creating a string with files names and HTML tag
                            name += files.Files[i].Name + tag;

                        }
                        // Encoding string to the HTML string
                        MvcHtmlString names = MvcHtmlString.Create(name);
                        // Put results to result list
                        result.Add("Names", names);
                        // Transfer result to the template
                        return View("Sample02", null, result);
                    }
                    // If files in request result's is empty return error message
                    else
                    {
                        message = "GetFileSystemEntities returns error";
                        result.Add("error", message);
                        return View("Sample02", null, result);
                    }
                }
            }
            // If data not posted return to template for filling of necessary fields
            else
            {
                return View("Sample02");
            }
        }
        //### <i>This sample will show how to use <b>UploadFile</b> method from Storage Api to upload file to GroupDocs Storage </i>
        public ActionResult Sample03()
        {
            // Check is data posted   
            if (Request.HttpMethod == "POST")
            {
                //### Set variables and get POST data
                System.Collections.Hashtable result = new System.Collections.Hashtable();
                String userId = Request.Form["client_id"];
                String private_key = Request.Form["private_key"];
                result.Add("client_id", userId);
                result.Add("private_key", private_key);
                Groupdocs.Api.Contract.UploadRequestResult upload = null;
                String message = null;
                // Check is all needed fields are entered
                if (userId == null || private_key == null || Request.Files == null)
                {
                    // If not all fields entered send error message
                    message = "Please enter all parameters";
                    result.Add("error", message);
                    // Transfer error message to template
                    return View("Sample03", null, result);
                }
                else
                {
                    // Create service for Groupdocs account
                    GroupdocsService service = new GroupdocsService("https://api.groupdocs.com/v2.0", userId, private_key);
                    //### Upload file
                    // Get tag's names from form
                    foreach (string inputTagName in Request.Files)
                    {
                        var file = Request.Files[inputTagName];
                        // Check that file is not fake.
                        if (file.ContentLength > 0)
                        {
                            // Upload file with empty description.
                            upload = service.UploadFile(file.FileName, String.Empty, file.InputStream);
                            // Check is upload successful
                            if (upload.Guid != null)
                            {
                                // Put uploaded file GuId to the result's list
                                result.Add("guid", upload.Guid);
                                
                            }
                            // If upload was failed return error
                            else
                            {
                                message = "UploadFile returns error";
                                result.Add("error", message);
                                return View("Sample03", null, result);
                            }
                        }
                    }
                    // Redirect to viewer with received GUID.
                    return View("Sample03", null, result);
                }
            }
            // If data not posted return to template for filling of necessary fields
            else
            {
                return View("Sample03");
            }
        }
        //### <i>This sample will show how to use <b>DownloadFile</b> method from Storage Api to download a file from GroupDocs Storage</i>
        public ActionResult Sample04()
        {
            // Check is data posted 
            if (Request.HttpMethod == "POST")
            {
                //### Set variables and get POST data
                System.Collections.Hashtable result = new System.Collections.Hashtable();
                String userId = Request.Form["client_id"];
                String private_key = Request.Form["private_key"];
                String fileId = Request.Form["fileId"];
                result.Add("client_id", userId);
                result.Add("private_key", private_key);
                result.Add("fileId", fileId);
                String message = null;
                // Check is all needed fields are entered
                if (userId == null || private_key == null || fileId == null)
                {
                    // If not all fields entered send error message
                    message = "Please enter all parameters";
                    result.Add("error", message);
                    return View("Sample04", null, result);
                }
                else
                {
                    // Create service for Groupdocs account
                    GroupdocsService service = new GroupdocsService("https://api.groupdocs.com/v2.0", userId, private_key);
                    //### Get all files and folders from account. 
                    Groupdocs.Api.Contract.ListEntitiesResult files = service.GetFileSystemEntities("", 0, -1, null, true, null, null);
                    //Create empty variable for file name
                    String name = null;
                    // Check is files is not null
                    if (files.Files != null)
                    {
                        // Obtaining file name for entered file Id
                        for (int i = 0; i < files.Files.Length; i++)
                        {
                            if (files.Files[i].Guid == fileId)
                            {
                                name = files.Files[i].Name;
                            }

                        }
                    }
                    
                    // Definition of folder where to download file
                    String LocalPath = AppDomain.CurrentDomain.BaseDirectory + "downloads/";
                    //### Make a request to Storage Api for dowloading file
                    // Download file
                    bool file = service.DownloadFile(fileId, LocalPath + name);
                    // If file downloaded successful
                    if (file != false)
                    {
                        // Put file info to the result's list
                        result.Add("path", LocalPath);
                        result.Add("name", name);
                        // Return to the template 
                        return View("Sample04", null, result);
                    }
                    // If file download failed
                    else
                    {
                        // Return error to the template
                        message = "DownloadFile returns error";
                        result.Add("error", message);
                        return View("Sample04", null, result);
                    }
                    
                }

            }
            // If data not posted return to template for filling of necessary fields
            else
            {
                return View("Sample04");
            }
        }
        //### <i>This sample will show how to use <b>MoveFile/copyFile</b> method's from Storage Api to copy/move a file in GroupDocs Storage </i>
        public ActionResult Sample05()
        {
            // Check is data posted 
            if (Request.HttpMethod == "POST")
            {
                //### Set variables and get POST data
                System.Collections.Hashtable result = new System.Collections.Hashtable();
                String userId = Request.Form["client_id"];
                String private_key = Request.Form["private_key"];
                String fileId = Request.Form["srcPath"];
                String destPath = Request.Form["destPath"];
                String action = Request.Form["copy"];
                // Set entered data to the results list
                result.Add("client_id", userId);
                result.Add("private_key", private_key);
                result.Add("fileId", fileId);
                result.Add("destPath", destPath);
                String message = null;
                // Check is all needed fields are entered
                if (userId == "" || private_key == "" || fileId == "" || destPath == "")
                {
                    // If not all fields entered send error message
                    message = "Please enter all parameters";
                    result.Add("error", message);
                    return View("Sample05", null, result);
                }
                else
                {
                    // Create service for Groupdocs account
                    GroupdocsService service = new GroupdocsService("https://api.groupdocs.com/v2.0", userId, private_key);
                    //### Get all files and folders from account. 
                    Groupdocs.Api.Contract.ListEntitiesResult files = service.GetFileSystemEntities("", 0, -1, null, true, null, null);
                    // Create empty variable for file name and id
                    String name = null;
                    decimal id = new decimal();
                    // Check is files is not null
                    if (files.Files != null)
                    {
                        // Obtaining file name and id for entered file GuId
                        for (int i = 0; i < files.Files.Length; i++)
                        {
                            if (files.Files[i].Guid == fileId)
                            {
                                name = files.Files[i].Name;
                                id = files.Files[i].Id;
                            }

                        }
                        // Check is entered file GuId exists
                        if (name == null || id == 0)
                        {
                           result.Add("error", "File was not found");
                           return View("Sample05", null, result);
                        }
                        // Create path to where copy/move file
                        String path = destPath + "/" + name;
                        Groupdocs.Api.Contract.FileMoveResponse transfer = new Groupdocs.Api.Contract.FileMoveResponse();
                        // Copy file if user choose copy
                        if (action == "Copy")
                        {
                            // Create empty Overide mode
                            Groupdocs.Common.OverrideMode overide = new Groupdocs.Common.OverrideMode();
                            // Make request to the Api to copy file
                            transfer = service.CopyFile(id, path, overide);
                            // Put message to result's list
                            result.Add("button", "Copied");

                        }
                        // If user choose Move file
                        else
                        {
                            Groupdocs.Common.OverrideMode overide = new Groupdocs.Common.OverrideMode();
                            transfer = service.MoveFile(id, path, overide);
                            result.Add("button", "Moved");
                        }
                        // Check is copy/move is successful
                        if (transfer.Status.Equals("Ok"))
                        {
                            // If successful put path to the copy/moved file to the results list
                            result.Add("path", path);
                        }
                        else
                        {
                            // If failed put error message
                            result.Add("error", transfer.ErrorMessage);
                        }

                    }
                    // Return results to the template
                   return View("Sample05", null, result);         
                }

            }
            // If data not posted return to template for filling of necessary fields
            else
            {
                return View("Sample05");
            }
                        
        }
        //### <i>This sample will show how to use <b>SignDocument</b> method from Signature Api to Sign Document and upload it to user storage</i>
        public ActionResult Sample06()
        {
            // Check is data posted 
            if (Request.HttpMethod == "POST")
            {
                //### Set variables and get POST data
                System.Collections.Hashtable result = new System.Collections.Hashtable();
                String userId = Request.Form["client_id"];
                String private_key = Request.Form["private_key"];
                var documetToSign = Request.Files["fi_document"];
                var signature = Request.Files["fi_signature"];
                result.Add("client_id", userId);
                result.Add("private_key", private_key);
                Groupdocs.Api.Contract.UploadRequestResult upload = null;
                String message = null;
                // Check is all needed fields are entered
                if (userId == null || private_key == null || Request.Files.Count == 0)
                {
                    // If not all fields entered send error message
                    message = "Please enter all parameters";
                    result.Add("error", message);
                    return View("Sample06", null, result);
                }
                else
                {
                    // Create service for Groupdocs account
                    GroupdocsService service = new GroupdocsService("https://api.groupdocs.com/v2.0", userId, private_key);
                    // Upload document for sign to the storage
                    upload = service.UploadFile(documetToSign.FileName, String.Empty, documetToSign.InputStream);
                    // If file uploaded successfuly
                    if (upload.Guid != null)
                    {
                        // Read binary data from InputStream
                        System.IO.BinaryReader reader = new System.IO.BinaryReader(signature.InputStream);
                        // Convert to byte array
                        byte[] bytedata = reader.ReadBytes((Int32)signature.ContentLength);
                        // Convert from byte array to the Base64 string
                        String data = "data:image/gif;base64," + Convert.ToBase64String(bytedata);
                        //### Create Signer settings object
                        Groupdocs.Api.Contract.SignatureSignDocumentSignerSettings signerSettings = new Groupdocs.Api.Contract.SignatureSignDocumentSignerSettings();
                        // Activate signature place
                        signerSettings.PlaceSignatureOn = "";
                        // Set signer name
                        signerSettings.Name = "GroupDocs";
                        // Transfer signature image
                        signerSettings.Data =  data;
                        // Set Heifht for signature
                        signerSettings.Height = new decimal(40d);
                        // Set width for signature
                        signerSettings.Width = new decimal(100d);
                        // Set top coordinate for signature
                        signerSettings.Top = new decimal(0.83319);
                        // Set left coordinate for signature
                        signerSettings.Left = new decimal(0.72171);
                        // Make request to sig document
                        Groupdocs.Api.Contract.SignatureSignDocumentResponse sign = service.SignDocument(upload.Guid, signerSettings);
                        // If document signed  
                        if (sign.Status.Equals("Ok"))
                        {
                            // Put to the result's list received GUID.
                            result.Add("guid", sign.Result.DocumentId);
                            
                        }
                        // If request returns error
                        else
                        {
                            // Redirect to viewer with error.
                            message = sign.ErrorMessage;
                            result.Add("error", message);
                            return View("Sample06", null, result);
                        }
                    }

                }
                // Redirect to viewer with received result's.
                return View("Sample06", null, result);
                
            }
            // If data not posted return to template for filling of necessary fields
            else
            {
                return View("Sample06");
            }
        }
        //### <i>This sample will show how to use <b>GetFileSystemEntities</b> method from Storage Api to create a list of thumbnails for a document</i>
        public ActionResult Sample07()
        {
            // Check is data posted 
            if (Request.HttpMethod == "POST")
            {
                //### Set variables and get POST data
                System.Collections.Hashtable result = new System.Collections.Hashtable();
                String userId = Request.Form["client_id"];
                String private_key = Request.Form["private_key"];
                result.Add("client_id", userId);
                result.Add("private_key", private_key);
                Groupdocs.Api.Contract.ListEntitiesResult files = null;
                String message = null;
                // Check is all needed fields are entered
                if (userId == null || private_key == null)
                {
                    // If not all fields entered send error message
                    message = "Please enter all parameters";
                    result.Add("error", message);
                    return View("Sample07", null, result);
                }
                else
                {
                    // Create service for Groupdocs account
                    GroupdocsService service = new GroupdocsService("https://api.groupdocs.com/v2.0", userId, private_key);
                    //### Get all files and folders from account. Set last argument to True to get thumbnails.
                    files = service.GetFileSystemEntities("", 0, -1, null, false, null, null, true);
                    // If request was successful
                    if (files.Files != null)
                    {
                        // Create empty variables for file name and image tag
                        String name = "";
                        String image = "";
                        // Create tag builders for img and br tag's
                        TagBuilder tagImg = new TagBuilder("img");
                        TagBuilder tagBr = new TagBuilder("br");
                        // Check is file have thumbnail
                        for (int i = 0; i < files.Files.Length; i++)
                        {
                            if (files.Files[i].Thumbnail != null)
                            {
                                // Delete extension from file name
                                int nameLength = files.Files[i].Name.Length;
                                name = files.Files[i].Name.Substring(0, nameLength - 4);
                                // Set local path for thumbnails
                                String LocalPath = AppDomain.CurrentDomain.BaseDirectory + "downloads/" + name + ".jpg";
                                // Convert thumbnail Base64 data to Base64 string
                                String data = Convert.ToBase64String(files.Files[i].Thumbnail);
                                // Convert from Base64 string to byte array
                                byte[] imageBytes = Convert.FromBase64String(data);
                                // Create Memory stream from byte array
                                System.IO.MemoryStream ms = new System.IO.MemoryStream(imageBytes, 0, imageBytes.Length);
                                // Write memory stream to the file
                                ms.Write(imageBytes, 0, imageBytes.Length);
                                // Create image file from stream
                                System.Drawing.Image img = System.Drawing.Image.FromStream(ms, true);
                                // Save image file
                                img.Save(LocalPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                                img.Dispose();
                                ms.Close();
                                //### Create HTML string with images and names
                                // Set attributes for img tag
                                tagImg.MergeAttribute("src", "/../downloads/" + name + ".jpg", true);
                                tagImg.MergeAttribute("width", "40px");
                                tagImg.MergeAttribute("height", "40px");
                                // Create string from img, file name and br tag
                                image += tagImg.ToString(TagRenderMode.SelfClosing) + files.Files[i].Name + tagBr;
                                name = null;                              
                            }

                        }
                        // Encode string to HTML string
                        MvcHtmlString images = MvcHtmlString.Create(image);
                        result.Add("images", images);
                        return View("Sample07", null, result);
                    }
                    // If request returns error
                    else
                    {
                        // Redirect to viewer with error.
                        message = "GetFileSystemEntities returns error";
                        result.Add("error", message);
                        return View("Sample07", null, result);
                    }
                }
            }
            // If data not posted return to template for filling of necessary fields
            else
            {
                return View("Sample07");
            }
        }
        //### <i>This sample will show how to use <b>GetDocumentPagesImageUrls</b> method from Doc Api to return a URL representing a single page of a Document</i>
        public ActionResult Sample08()
        {
            // Check is data posted 
            if (Request.HttpMethod == "POST")
            {
                //### Set variables and get POST data
                System.Collections.Hashtable result = new System.Collections.Hashtable();
                String userId = Request.Form["client_id"];
                String private_key = Request.Form["private_key"];
                String fileId = Request.Form["fileId"];
                String pageNumber = Request.Form["pageNumber"];
                result.Add("client_id", userId);
                result.Add("private_key", private_key);
                result.Add("fileId", fileId);
                String message = null;
                // Check is all needed fields are entered
                if (userId == null || private_key == null || fileId == null)
                {
                    // If not all fields entered send error message
                    message = "Please enter all parameters";
                    result.Add("error", message);
                    return View("Sample08", null, result);
                }
                else
                {
                    // Create service for Groupdocs account
                    GroupdocsService service = new GroupdocsService("https://api.groupdocs.com/v2.0", userId, private_key);
                    // Make request to get page image url
                    String[] url = service.GetDocumentPagesImageUrls(fileId, "600x750", String.Empty, Int32.Parse(pageNumber), 1, 100, false);
                    // Check if url is not null
                    if (url != null)
                    {
                        // Redirect to template with receive URL
                        result.Add("url", url[0]);
                        return View("Sample08", null, result);
                    }
                    // Else return error to the template
                    else
                    {
                        message = "Something is wrong with your data";
                        result.Add("error", message);
                        return View("Sample08", null, result);
                    }

                }

            }
            // If data not posted return to template for filling of necessary fields
            else
            {
                return View("Sample08");
            }
        }
        //### <i>This sample will show how to use <b>GuId</b> of file to generate an embedded Viewer URL for a Document</i>
        public ActionResult Sample09()
        {
            // Check is data posted 
            if (Request.HttpMethod == "POST")
            {
                //### Set variables and get POST data
                System.Collections.Hashtable result = new System.Collections.Hashtable();
                String width = Request.Form["width"];
                String height = Request.Form["height"];
                String fileId = Request.Form["fileId"];
                result.Add("width", width);
                result.Add("height", height);
                result.Add("fileId", fileId);
                String message = null;
                // If entered file id is null
                if (fileId == null)
                {
                    // Return error message
                    message = "Please enter file id";
                    result.Add("error", message);

                    return View("Sample09", null, result);
                }
                else
                {
                    // Generate Embed viewer url with entered file id
                     String iframe = "https://apps.groupdocs.com/document-viewer/embed/" + fileId + " frameborder=0 width=" + width + " height=" + height;
                     result.Add("iframe", iframe);
                     return View("Sample09", null, result);
                    
                }

            }
            // If data not posted return to template for filling of necessary fields
            else
            {
                return View("Sample09");
            }
        }
        //### <i>This sample will show how to use <b>ShareDocument</b> method from Doc Api to share a document to other users</i>
        public ActionResult Sample10()
        {
            // Check is data posted 
            if (Request.HttpMethod == "POST")
            {
                //### Set variables and get POST data
                System.Collections.Hashtable result = new System.Collections.Hashtable();
                String userId = Request.Form["client_id"];
                String private_key = Request.Form["private_key"];
                String fileId = Request.Form["fileId"];
                String email = Request.Form["email"];
                // Set entered data to the results list
                result.Add("client_id", userId);
                result.Add("private_key", private_key);
                result.Add("fileId", fileId);
                result.Add("email", email);
                String message = null;
                // Check is all needed fields are entered
                if (userId == null || private_key == null || fileId == null || email == null)
                {
                    // If not all fields entered send error message
                    message = "Please enter all parameters";
                    result.Add("error", message);
                    return View("Sample10", null, result);
                }
                else
                {
                    // Create string array with emails
                    String[] sharers = new String[1];
                    sharers[0] = email;
                    // Create service for Groupdocs account
                    GroupdocsService service = new GroupdocsService("https://api.groupdocs.com/v2.0", userId, private_key);
                    // Get all files from storage
                    Groupdocs.Api.Contract.ListEntitiesResult files = service.GetFileSystemEntities("", 0, -1, null, true, null, null);
                    decimal id = new decimal();
                    // Get file id by entered file GuId
                    for (int i = 0; i < files.Files.Length; i++) 
                    {
                        if (files.Files[i].Guid == fileId)
                        {
                            id = files.Files[i].Id;
                        }
                        
                    }
                    // If file wasn't found return error
                    if (id == null)
                    {
                        
                        message = "File GuId you are entered is wrong";
                        result.Add("error", message);
                        return View("Sample10", null, result);
                        
                    }
                    // Make request to share document
                    Groupdocs.Api.Contract.SharedUsersResult share = service.ShareDocument(id, sharers);
                    // Check is request return data
                    if (share != null)
                    {
                        // Return primary email to the template
                        result.Add("sharer", share.SharedUsers[0].PrimaryEmail);
                        return View("Sample10", null, result);
                    }
                    // If request return's null return error to the template
                    else
                    {
                        message = "Something is wrong with your data";
                        result.Add("error", message);
                        return View("Sample10", null, result);
                    }

                }

            }
            // If data not posted return to template for filling of necessary fields
            else
            {
                return View("Sample10");
            }
        }

        //### <i>This sample will show how to use <b>ShareDocument</b> method from Doc Api to share a document to other users</i>
        public ActionResult Sample18()
        {
            // Check is data posted 
            if (Request.HttpMethod == "POST")
            {
                //### Set variables and get POST data
                System.Collections.Hashtable result = new System.Collections.Hashtable();
                String userId = Request.Form["client_id"];
                String private_key = Request.Form["private_key"];
                String fileId = Request.Form["fileId"];
                String type = Request.Form["convert_type"];
                String callback = Request.Form["callback"];
                // Set entered data to the results list
                result.Add("client_id", userId);
                result.Add("private_key", private_key);
                result.Add("fileId", fileId);
                result.Add("email", type);
                String message = null;
                // Check is all needed fields are entered
                if (userId == null || private_key == null || fileId == null || type == null)
                {
                    // If not all fields entered send error message
                    message = "Please enter all parameters";
                    result.Add("error", message);
                    return View("Sample18", null, result);
                }
                else
                {
                    
                    // Create service for Groupdocs account
                    GroupdocsService service = new GroupdocsService("https://api.groupdocs.com/v2.0", userId, private_key);
                    // Get all files from storage
                    decimal jobId = service.ConvertFile(fileId, type, "", false, false, callback);
                    
                    if (jobId != null)
                    {
                        System.Threading.Thread.Sleep(5000);
                        Groupdocs.Api.Contract.GetJobDocumentsResult job = service.GetJobDocuments(jobId);
                        // Return primary email to the template
                        if (job.Inputs[0].Outputs[0].Guid != "")
                        {
                            result.Add("guid", job.Inputs[0].Outputs[0].Guid);
                            return View("Sample18", null, result);
                        }
                        else
                        {
                            result.Add("error", "File GuId is empty");
                            return View("Sample18", null, result);
                        }
                    }
                    // If request return's null return error to the template
                    else
                    {
                        result.Add("error", "Convert is faile");
                        return View("Sample18", null, result);
                    }

                }

            }
            // If data not posted return to template for filling of necessary fields
            else
            {
                return View("Sample18");
            }
        }

    }
}
