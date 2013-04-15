using System;
using System.Configuration;
using System.Web.Mvc;
using Groupdocs.Sdk;
using Groupdocs.Api;
using System.IO;
using System.Linq;

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
                String basePath = Request.Form["server_type"];
                String url = Request.Form["url"];
                result.Add("client_id", userId);
                result.Add("private_key", private_key);
                result.Add("url", url);
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
                    if (basePath == "")
                    {
                        basePath = "https://api.groupdocs.com/v2.0";
                    }
                    // Create service for Groupdocs account
                    GroupdocsService service = new GroupdocsService(basePath, userId, private_key);
                    result.Add("basePath", basePath);
                    if (url == "")
                    {
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
                    }
                    else
                    {
                        //Make request to upload file from entered Url
                        String guid = service.UploadUrl(url);
                        if (guid != null)
                        {
                            //If file uploaded return his GuId to template
                            result.Add("guid", guid);
                            return View("Sample03", null, result);
                        }
                        //If file wasn't uploaded return error
                        else
                        {
                            result.Add("error", "Something wrong with entered data");
                            return View("Sample03", null, result);
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

        //### <i>This sample will show how programmatically create and post an annotation into document</i>
        public ActionResult Sample11()
        {
            // Check is data posted 
            if (Request.HttpMethod == "POST")
            {
                //### Set variables and get POST data
                System.Collections.Hashtable result = new System.Collections.Hashtable();
                String userId = Request.Form["client_id"];
                String private_key = Request.Form["private_key"];
                String fileId = Request.Form["fileId"];
                String annotation_type = Request.Form["annotation_type"];
                String box_x = Request.Form["box_x"];
                String box_y = Request.Form["box_y"];
                String box_width = Request.Form["box_width"];
                String box_height = Request.Form["box_height"];
                String annotationPosition_x = Request.Form["annotationPosition_x"];
                String annotationPosition_y = Request.Form["annotationPosition_y"];
                String range_position = Request.Form["range_position"];
                String range_length = Request.Form["range_length"];
                String text = Request.Form["text"];
             
                // Set entered data to the results list
                result.Add("client_id", userId);
                result.Add("private_key", private_key);
                result.Add("fileId", fileId);
                result.Add("type", annotation_type);
                String message = null;
                // Check is all needed fields are entered
                if (userId == null || private_key == null || fileId == null || annotation_type == null)
                {
                    // If not all fields entered send error message
                    message = "Please enter all parameters";
                    result.Add("error", message);
                    return View("Sample11", null, result);
                }
                else
                {

                    // Create service for Groupdocs account
                    GroupdocsService service = new GroupdocsService("https://api.groupdocs.com/v2.0", userId, private_key);
                    //Make request to api for convert file
                   //###Create AnnotationType
                   Groupdocs.Common.AnnotationType type = new Groupdocs.Common.AnnotationType();
                   //Create Rectangle object
                   Groupdocs.Api.Contract.Rectangle rectangle = new Groupdocs.Api.Contract.Rectangle();
                   //Create Point object
                   Groupdocs.Api.Contract.Data.Point point = new Groupdocs.Api.Contract.Data.Point();
                   //Create Range object
                   Groupdocs.Api.Contract.Range range = new Groupdocs.Api.Contract.Range();
                   //Set annotation parameters if annotation type is point
                   if (annotation_type == "point") 
                   {
                       type = Groupdocs.Common.AnnotationType.Point;
                       rectangle.X = float.Parse(box_x);
                       rectangle.Y = float.Parse(box_y);
                       rectangle.Width = 0;
                       rectangle.Height = 0;
                       point.X = 0;
                       point.Y = 0;

                   }
                   //Set annotation parameters if annotation type is text
                   else if (annotation_type == "text")
                   {
                       type = Groupdocs.Common.AnnotationType.Text;
                       point.X = double.Parse(annotationPosition_x);
                       point.Y = double.Parse(annotationPosition_y);
                       range.Length = Int32.Parse(range_length);
                       range.Position = Int32.Parse(range_position);
                   }
                   //Set annotation parameters if annotation type is area
                   else
                   {
                       type = Groupdocs.Common.AnnotationType.Area;
                       point.X = 0;
                       point.Y = 0;
                       rectangle.X = float.Parse(box_x);
                       rectangle.Y = float.Parse(box_y);
                       rectangle.Width = float.Parse(box_width);
                       rectangle.Height = float.Parse(box_height);
                   }
                   //### Make request to Api to create Annotation
                   Groupdocs.Api.Contract.Annotation.CreateAnnotationResult annotation = service.CreateAnnotation(fileId, type, rectangle, point, range, null, null);
                   //Check if GuId of document with added annotation is not empty
                    if (annotation.DocumentGuid != "")
                   {
                        //Set data for template
                       result.Add("guid", annotation.DocumentGuid);
                       return View("Sample11", null, result);
                   }
                   //If GuId is empty return error
                   else
                   {
                       result.Add("error", "Annotation is fail");
                       return View("Sample11", null, result);
                   }
                    
                }

            }
            // If data not posted return to template for filling of necessary fields
            else
            {
                return View("Sample11");
            }
        }

        //### <i>This sample will show how to list all annotations from document</i>
        public ActionResult Sample12()
        {
            // Check is data posted 
            if (Request.HttpMethod == "POST")
            {
                //### Set variables and get POST data
                System.Collections.Hashtable result = new System.Collections.Hashtable();
                String userId = Request.Form["client_id"];
                String private_key = Request.Form["private_key"];
                String fileId = Request.Form["fileId"];
               
                // Set entered data to the results list
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
                    return View("Sample12", null, result);
                }
                else
                {
                    // Create service for Groupdocs account
                    GroupdocsService service = new GroupdocsService("https://api.groupdocs.com/v2.0", userId, private_key);
                    // Get all annotations from document
                    Groupdocs.Api.Contract.Annotation.ListAnnotationsResult annotations = service.ListAnnotations(fileId);
                    // If annotations wasn't found return error
                    if (annotations.Annotations.Length == 0)
                    {

                        message = "File GuId you are entered is wrong";
                        result.Add("error", message);
                        return View("Sample12", null, result);

                    }
                    // Create String variables for creating HTMl div with results
                    String annotation = "";
                    String replies = "";
                    String block = "";
                    //Get annotations data
                    for (int i = 0; i < annotations.Annotations.Length; i++)
                    {
                        //Get Annotation access and type
                        annotation = "Access: " + annotations.Annotations[i].Access + "<br />" + "Type: " +
                                            annotations.Annotations[i].Type + "<br />";
                        //Check if Annotation have replies
                        if (annotations.Annotations[i].Replies.Length == 0)
                        {
                            replies = "";
                           
                        }
                        else
                        {
                            //Get all replies from annotation
                            for (int n = i; n < annotations.Annotations[i].Replies.Length; n++)
                            {
                                //Get replies data
                                replies += "<li>" + annotations.Annotations[i].Replies[n].UserName +
                                                    " : " + annotations.Annotations[i].Replies[n].Message + "</li>";
                            }
                        }
                        //Create block with annotation data for template
                        block += "<div>" + annotation + "Replies: " + "<ul>" + replies + "</ul>" + "<hr />" + "</div>";
                    }
                    //Convert from string to HTML string
                    MvcHtmlString div = MvcHtmlString.Create(block);
                    //Set data for template
                    result.Add("div", div);
                    return View("Sample12", null, result);
                }

            }
            // If data not posted return to template for filling of necessary fields
            else
            {
                return View("Sample12");
            }
        }
        //### <i>This sample will show how to add collaborator to doc with annotations</i>
        public ActionResult Sample13()
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
                    return View("Sample13", null, result);
                }
                else
                {
                    // Create string array with emails
                    String[] collaborators = new String[1];
                    collaborators[0] = email;
                    // Create service for Groupdocs account
                    GroupdocsService service = new GroupdocsService("https://api.groupdocs.com/v2.0", userId, private_key);
                    // Make request to set annotation collaborators
                    Groupdocs.Api.Contract.Annotation.SetCollaboratorsResult collaborate = service.SetAnnotationCollaborators(fileId, collaborators);
                    // Check is request return data
                    if (collaborate != null)
                    {
                        // Return primary email to the template
                        result.Add("collaborator", collaborate.Collaborators[0].PrimaryEmail);
                        return View("Sample13", null, result);
                    }
                    // If request return's null return error to the template
                    else
                    {
                        message = "Something is wrong with your data";
                        result.Add("error", message);
                        return View("Sample13", null, result);
                    }

                }

            }
            // If data not posted return to template for filling of necessary fields
            else
            {
                return View("Sample13");
            }
        }
        //### <i>This sample will show how  to check the list of shares for a folder</i>
        public ActionResult Sample14()
        {
            // Check is data posted 
            if (Request.HttpMethod == "POST")
            {
                //### Set variables and get POST data
                System.Collections.Hashtable result = new System.Collections.Hashtable();
                String userId = Request.Form["client_id"];
                String private_key = Request.Form["private_key"];
                String path = Request.Form["path"];
                // Set entered data to the results list
                result.Add("client_id", userId);
                result.Add("private_key", private_key);
                result.Add("path", path);
                String message = null;
                // Check is all needed fields are entered
                if (userId == null || private_key == null || path == null)
                {
                    // If not all fields entered send error message
                    message = "Please enter all parameters";
                    result.Add("error", message);
                    return View("Sample14", null, result);
                }
                else
                {
                    //### Create a proper path from entered path
                    //Reaplace dashlet's with proper dashlet "/"
                    String properPath = path.Replace("(\\/)", "/");
                    //Convert string to array
                    String[] pathArray = properPath.Split('/');
                    String lastFolder = "";
                    String newPath = "";
                    //Check if array content more than one element
                    if (pathArray.Length > 1)
                    {
                        //Create string with proper path from array
                        lastFolder = pathArray[pathArray.Length - 1];
                        newPath = String.Join("/", pathArray);
                    }
                    else
                    {
                        lastFolder = pathArray[0];
                    }
                    // Create service for Groupdocs account
                    GroupdocsService service = new GroupdocsService("https://api.groupdocs.com/v2.0", userId, private_key);
                    // Get all folders from account
                    Groupdocs.Api.Contract.ListEntitiesResult folders = service.GetFileSystemEntities("", 0, -1, null, true, null, null);
                    decimal folderId = new decimal();
                    //Get folder Id
                    if (folders.Folders != null)
                    {
                        for (int i = 0; i < folders.Folders.Length; i++)
                        {
                            if (folders.Folders[i].Name == lastFolder)
                            {
                                folderId = folders.Folders[i].Id;
                            }
                        }
                    }
                    //Get all sharers for entered folder
                    Groupdocs.Api.Contract.UserInfo[] shares = service.GetFolderSharers(folderId);
                    // Check is request return data
                    if (shares.Length != 0)
                    {
                        String emails = "";
                        //Get all emails of sharers
                        for (int n = 0; n < shares.Length; n++)
                        {
                            emails += shares[n].PrimaryEmail + "<br />";
                        }
                        //Convert string to HTML string
                         MvcHtmlString email = MvcHtmlString.Create(emails);
                        // Return primary email to the template
                        result.Add("shares", email);
                        return View("Sample14", null, result);
                    }
                    // If request return's null return error to the template
                    else
                    {
                        message = "Something is wrong with your data";
                        result.Add("error", message);
                        return View("Sample14", null, result);
                    }

                }

            }
            // If data not posted return to template for filling of necessary fields
            else
            {
                return View("Sample14");
            }
        }
        //### <i>This sample will show how  to check the number of document's views using .NET SDK</i>
        public ActionResult Sample15()
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
                Groupdocs.Api.Contract.DocumentViewsResult userInfo = null;
                String message = null;
                // Check is all needed fields are entered
                if (userId == null || private_key == null)
                {
                    //If not all fields entered send error message
                    message = "Please enter all parameters";
                    result.Add("error", message);
                    // Transfer error message to template
                    return View("Sample15", null, result);
                }
                else
                {
                    // Create service for Groupdocs account
                    GroupdocsService service = new GroupdocsService("https://api.groupdocs.com/v2.0", userId, private_key);
                    // Get info about documents view
                    userInfo = service.GetDocumentViews(0);
                    //Return amount of elements which will be a views amount
                    result.Add("views", userInfo.Views.Length);
                    // Return Hashtable with results to the template
                    return View("Sample15", null, result);
                }
            }
            // If data not posted return to template for filling of necessary fields
            else
            {
                return View("Sample15");
            }
        }
        //### <i>This sample will show how to insert Assembly questionary into webpage using .NET SDK</i>
        public ActionResult Sample16()
        {
            // Check is data posted    
            if (Request.HttpMethod == "POST")
            {
                //### Set variables and get POST data
                System.Collections.Hashtable result = new System.Collections.Hashtable();
                String fileId = Request.Form["fileId"];
                String message = "";
                if (fileId == null)
                {
                    //If not all fields entered send error message
                    message = "Please enter File Id";
                    result.Add("error", message);
                    // Transfer error message to template
                    return View("Sample16", null, result);
                }
                else
                {
                    result.Add("guid", fileId);
                   
                }
                // Transfer error message to template
                return View("Sample16", null, result);
            }
            // If data not posted return to template for filling of necessary fields
            else
            {
                return View("Sample16");
            }
        }
        //### <i>This sample will show how to upload a file into the storage and compress it into zip archive </i>
        public ActionResult Sample17()
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
                    return View("Sample17", null, result);
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
                            if (upload.Id != null)
                            {
                                //Compress uploaded file into "zip" archive
                                decimal Id = service.CompressFile(upload.Id, Groupdocs.Common.ArchiveType.Zip);
                               if (Id != null)
                               {
                                   //Get all files from account
                                   Groupdocs.Api.Contract.ListEntitiesResult files = service.GetFileSystemEntities("", 0, -1, null, true, null, null);
                                   String name = "";
                                   //Check if request return data
                                   if (files.Files.Length != 0)
                                   {
                                       //Get Name and Id of compresed file
                                       for (int i = 0; i < files.Files.Length; i++)
                                       {
                                           if (files.Files[i].Id == Id)
                                           {
                                               name = files.Files[i].Name;
                                           }
                                       }
                                       //If file uploaded and compresed return message with file name to the template
                                       result.Add("message", "Archive created and saved successfully as " + name);
                                       return View("Sample17", null, result);
                                   }
                                   else
                                   {
                                       //If file GuId is empty return error
                                       result.Add("error", "File Name is empty");
                                       return View("Sample17", null, result);
                                   }
                               }
                            }
                            // If upload was failed return error
                            else
                            {
                                message = "UploadFile returns error";
                                result.Add("error", message);
                                return View("Sample17", null, result);
                            }
                        }
                    }
                    // Redirect to viewer with received data
                    return View("Sample17", null, result);
                }
            }
            // If data not posted return to template for filling of necessary fields
            else
            {
                return View("Sample17");
            }
        }
        //### <i>This sample will show how to use <b>ConvertFile</b> method from to convert a document to other type</i>
        public ActionResult Sample18()
        {
            // Check is data posted 
            if (Request.HttpMethod == "POST")
            {
                //### Set variables and get POST data
                System.Collections.Hashtable result = new System.Collections.Hashtable();
                String userId = Request.Form["client_id"];
                String privateKey = Request.Form["private_key"];
                String fileId = Request.Form["fileId"];
                String type = Request.Form["convert_type"];
                String url = Request.Form["url"];
                var file = Request.Files["file"];
                String callback = Request.Form["callback"];
                // Set entered data to the results list
                result.Add("client_id", userId);
                result.Add("private_key", privateKey);
                result.Add("fileId", fileId);
                result.Add("type", type);

                if (!String.IsNullOrEmpty(callback))
                {
                    result.Add("callback", callback);
                }

                String message = null;
                // Check is all required fields were provided
                if (String.IsNullOrEmpty(userId) || String.IsNullOrEmpty(privateKey) || String.IsNullOrEmpty(type)
                    || (String.IsNullOrEmpty(fileId) && (String.IsNullOrEmpty(url)) && (file.ContentLength == 0)))
                {
                    // If not all required fields were provided - send error message
                    message = "Please enter all parameters";
                    result.Add("error", message);
                    return View("Sample18", null, result);
                }
                else
                {
                    //path to settings file - temporary save userId and apiKey like to property file
                    String infoFile = AppDomain.CurrentDomain.BaseDirectory + "user_info.txt";
                    
                    //open file in rewrite mode
                    FileStream fcreate = System.IO.File.Open(infoFile, FileMode.Create); // will create the file or overwrite it if it already exists
                    //String filePath = AppDomain.CurrentDomain.BaseDirectory + "user_info.txt";
                    System.IO.StreamWriter infoFileStreamWriter = new StreamWriter(fcreate);
                    //w = System.IO.File.CreateText(filePath);
                    infoFileStreamWriter.WriteLine(userId); //save userId
                    infoFileStreamWriter.WriteLine(privateKey); //save privateKey
                    infoFileStreamWriter.Flush();
                    infoFileStreamWriter.Close();

                    String downloadFolder = AppDomain.CurrentDomain.BaseDirectory + "downloads/";
                    //check if Downloads folder exists and remove it to clean all old files
                    if (Directory.Exists(downloadFolder))
                    {
                        Directory.Delete(downloadFolder, true);
                    }

                    //Create service (API Client object) linked to provided Groupdocs account
                    GroupdocsService service = new GroupdocsService("https://api.groupdocs.com/v2.0", userId, privateKey);

                    //if URL to web file was provided - upload the file from Web and get it's GUID
                    if (url != "")
                    {
                        //Make request to upload file from entered Url
                        String guid = service.UploadUrl(url);
                        if (guid != null)
                        {
                            //If file uploaded return his GuId
                            fileId = guid;

                        }
                        //If file wasn't uploaded return error
                        else
                        {
                            result.Add("error", "Something wrong with entered data");
                            return View("Sample18", null, result);
                        }
                    }

                    //if file was uploaded locally - upload the file and get it's GUID
                    if (file.FileName != "")
                    {
                        //Upload local file 
                        Groupdocs.Api.Contract.UploadRequestResult upload = service.UploadFile(file.FileName, String.Empty, file.InputStream);
                        if (upload.Guid != null)
                        {
                            //If file uploaded return his guid
                            fileId = upload.Guid;
                        }
                        else
                        {
                            //If file wasn't uploaded return error
                            result.Add("error", "Something wrong with entered data");
                            return View("Sample18", null, result);
                        }
                    }


                    decimal jobId = 0;
                    try
                    {
                        //Make request to api for convert file.
                        //@fileId - GUID. Represents the provided file - via guid, web url or local file.
                        //@type - File type of the result file
                        //@callback - callback URL
                        jobId = service.ConvertFile(fileId, type, "", false, false, callback);
                    }
                    catch (Exception e)
                    {

                        result.Add("error", e.ToString());
                        return View("Sample18", null, result);
                    }

                    /*
                     * The approack bellow is not good one to get Job Results directly in the code after ConvertFile method.
                     * We use this approach to show the result file in embedded iframe on the same sample page.
                     * In production it's better to use Callback approach - this approach allso implemented in this sample. 
                     */

                    //Delay is required to be shure that the file was processed
                    System.Threading.Thread.Sleep(5000);
                    //Make request to api to get document info by job id
                    Groupdocs.Api.Contract.GetJobDocumentsResult job = service.GetJobDocuments(jobId);

                    if (job.Inputs[0].Outputs[0].Guid != "")
                    {
                        //Return file guid to the template
                        result.Add("guid", job.Inputs[0].Outputs[0].Guid);
                        return View("Sample18", null, result);
                    }
                    else
                    {
                        //If file GuId is empty return error
                        result.Add("error", "File GuId is empty");
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
        //### <i>This sample will show how to Compare documents</i>
        public ActionResult Sample19()
        {
            // Check is data posted 
            if (Request.HttpMethod == "POST")
            {
                //### Set variables and get POST data
                System.Collections.Hashtable result = new System.Collections.Hashtable();
                String userId = Request.Form["client_id"];
                String private_key = Request.Form["private_key"];
                String sourceFileId = Request.Form["sourceFileId"];
                String targetFileId = Request.Form["targetFileId"];
                String callback = Request.Form["callback"];
                String basePath = Request.Form["server_type"];
                // Set entered data to the results list
                result.Add("client_id", userId);
                result.Add("private_key", private_key);
                result.Add("sourceFileId", sourceFileId);
                result.Add("targetFileId", targetFileId);
                result.Add("callback", callback);
                result.Add("basePath", basePath);
                String message = null;
                // Check is all needed fields are entered
                if (userId == null || private_key == null || sourceFileId == null || targetFileId == null)
                {
                    // If not all fields entered send error message
                    message = "Please enter all parameters";
                    result.Add("error", message);
                    return View("Sample19", null, result);
                }
                else
                {

                    // Create service for Groupdocs account
                    GroupdocsService service = new GroupdocsService(basePath, userId, private_key);
                    //Compare two documents and setting collback Url
                    Groupdocs.Api.Comparison.Contract.CompareResponse compare = service.Compare(sourceFileId, targetFileId, callback);

                    if (compare.Status != null)
                    {
                        //Delay necessary that the inquiry would manage to be processed
                        System.Threading.Thread.Sleep(5000);
                        //Make request to api for get document info by job id
                        Groupdocs.Api.Contract.GetJobDocumentsResult job = service.GetJobDocuments(compare.Result.JobId);

                        if (job.Outputs[0].Guid != null)
                        {
                            //Return file guid to the template
                            result.Add("guid", job.Outputs[0].Guid);
                            return View("Sample19", null, result);
                        }
                        else
                        {
                            //If file GuId is empty return error
                            result.Add("error", "File GuId is empty");
                            return View("Sample19", null, result);
                        }
                    }
                    // If request return's null return error to the template
                    else
                    {
                        result.Add("error", compare.ErrorMessage);
                        return View("Sample19", null, result);
                    }

                }

            }
            // If data not posted return to template for filling of necessary fields
            else
            {
                return View("Sample19");
            }
        }

        //### <i>This sample will show how to Get Compare Change list for document</i>
        public ActionResult Sample20()
        {
            // Check is data posted 
            if (Request.HttpMethod == "POST")
            {
                //### Set variables and get POST data
                System.Collections.Hashtable result = new System.Collections.Hashtable();
                String userId = Request.Form["client_id"];
                String private_key = Request.Form["private_key"];
                String resultFileId = Request.Form["resultFileId"];
                // Set entered data to the results list
                result.Add("client_id", userId);
                result.Add("private_key", private_key);
                result.Add("sourceFileId", resultFileId);
                String message = null;
                // Check is all needed fields are entered
                if (userId == null || private_key == null || resultFileId == null)
                {
                    // If not all fields entered send error message
                    message = "Please enter all parameters";
                    result.Add("error", message);
                    return View("Sample20", null, result);
                }
                else
                {

                    // Create service for Groupdocs account
                    GroupdocsService service = new GroupdocsService("https://api.groupdocs.com/v2.0", userId, private_key);
                    //###Make request to ComparisonApi using user id
                    //Get changes list for document
                    Groupdocs.Api.Comparison.Contract.ChangesResponse info = service.GetChanges(resultFileId);

                    if (info.Status != null)
                    {
                       
		                String action = null;
		                String width = null;
                        String height = null;
		                String text = null;
		                String type = null;
                        //###Create table with changes for template
                        String table = "<table class='border'>";
                        table += "<tr><td><font color='green'>Change Name</font></td><td><font color='green'>Change</font></td></tr>";
                        //Count of iterations                                
						for (int i = 0; i < info.Result.Changes.Length; i++) {
                            //###Check received data
							if (info.Result.Changes[i].Action == null) {
								action = "";
							} else {
								action = info.Result.Changes[i].Action;
							}
							
							if (info.Result.Changes[i].Page == null) {
								width = "";
							} else {
								width = Convert.ToString(info.Result.Changes[i].Page.Width);
							}

                            if (info.Result.Changes[i].Page == null) {
								height = "";
							} else {
								height = Convert.ToString(info.Result.Changes[i].Page.Height);
							}
							
							if ( info.Result.Changes[i].Text == null) {
								text = "";
							} else {
								text = info.Result.Changes[i].Text;
							}
							
							if ( info.Result.Changes[i].Type == null) {
								type = "";
							} else {
								type = info.Result.Changes[i].Type;
							}
                            //Formation of a body of the table
							table += "<tr><td>Id</td><td>" + info.Result.Changes[i].Id + "</td></tr>";
							table += "<tr><td>Action</td><td>" + action + "</td></tr>";
							table += "<tr><td>Text</td><td>" + text + "</td></tr>";
							table += "<tr><td>Type</td><td>" + type + "</td></tr>";
							table += "<tr><td>Width</td><td>" + width + "</td></tr>";
                            table += "<tr><td>Height</td><td>" + height + "</td></tr>";
							table += "<tr bgcolor='#808080'><td></td><td></td></tr>";
                        }

                        table += "</table>";
                        //Convert string to HTML string
                        MvcHtmlString infoTable = MvcHtmlString.Create(table);
                        //Return table to the template
                        result.Add("info", infoTable);
                        return View("SAmple20", null, result);
                    }
                    // If request return's null return error to the template
                    else
                    {
                        result.Add("error", info.ErrorMessage);
                        return View("Sample20", null, result);
                    }

                }

            }
            // If data not posted return to template for filling of necessary fields
            else
            {
                return View("Sample20");
            }
        }

        //### <i>This sample will show how to use StorageApi to Create and Upload Envelop to GroupDocs account</i>
        public ActionResult Sample21()
        {
            // Check is data posted 
            if (Request.HttpMethod == "POST")
            {
                //### Set variables and get POST data
                System.Collections.Hashtable result = new System.Collections.Hashtable();
                String userId = Request.Form["client_id"];
                String private_key = Request.Form["private_key"];
                String email = Request.Form["email"];
                String name = Request.Form["name"];
                String lastName = Request.Form["lastName"];
                String callback = Request.Form["callbackUrl"];
                var documet = Request.Files["file"];
                String basePath = Request.Form["server_type"];
                // Set entered data to the results list
                result.Add("client_id", userId);
                result.Add("private_key", private_key);
                result.Add("email", email);
                result.Add("firstName", name);
                result.Add("lastName", lastName);
                result.Add("callback", callback);
                result.Add("basePath", basePath);
                String message = null;
                // Check is all needed fields are entered
                if (userId == null || private_key == null || email == null || lastName == null || name == null || documet == null)
                {
                    // If not all fields entered send error message
                    message = "Please enter all parameters";
                    result.Add("error", message);
                    return View("Sample21", null, result);
                }
                else
                {

                    // Create service for Groupdocs account
                    GroupdocsService service = new GroupdocsService(basePath, userId, private_key);
                    //Upload file for envelop
                    Groupdocs.Api.Contract.UploadRequestResult upload = service.UploadFile(documet.FileName, String.Empty, documet.InputStream);
                    //Check is file uploaded
                    if (upload.Guid != null)
                    {
                        //Create envilope using user id and entered by user name
                        Groupdocs.Api.Contract.SignatureEnvelopeResponse envelop = service.CreateEnvelope("", "", documet.FileName, upload.Guid, false);
                        if (envelop.Status.Equals("Ok"))
                        {
                            decimal dec = new decimal();
                            //Get role list for curent user
                            Groupdocs.Api.Contract.SignatureRolesResponse roles = service.GetSignatureRoles();
                            String roleId = "";
                            //Get id of role which can sign
                            for (int i = 0; i < roles.Result.Roles.Length; i++)
                            {
                                if (roles.Result.Roles[i].Name.Equals("Signer"))
                                {
                                    roleId = roles.Result.Roles[i].Id;
                                    break;
                                }
                            }
                            //Add recipient to envelope
                            Groupdocs.Api.Contract.SignatureEnvelopeRecipientResponse addRecipient = service.AddEnvelopeRecipient(envelop.Result.Envelope.Id, email, name, lastName, roleId, dec);
                            //Send envelop with callback url
                            Groupdocs.Api.Contract.SignatureEnvelopeSendResponse send = service.SendEnvelope(envelop.Result.Envelope.Id, callback);
                            //Check is envelope send status
                            if (send.Status.Equals("Ok"))
                            {
                                //Set data for template
                                result.Add("envelop", envelop.Result.Envelope.Id);
                                result.Add("recipient", addRecipient.Result.Recipient.Id);
                                return View("Sample21", null, result);
                            }
                            //If status failed set error for template
                            else
                            {
                                result.Add("error", send.ErrorMessage);
                                return View("Sample21", null, result);
                            }
                           
                        }
                        //If envelope wasn't created send error
                        else
                        {
                            result.Add("error", envelop.ErrorMessage);
                            return View("Sample21", null, result);
                        }
                    }
                    // If request return's null return error to the template
                    else
                    {
                        result.Add("error", "Upload is failed");
                        return View("Sample21", null, result);
                    }

                }

            }
            // If data not posted return to template for filling of necessary fields
            else
            {
                return View("Sample21");
            }
        }

        //### <i>This sample will show how create or update user and add him to collaborators</i>
        public ActionResult Sample22()
        {
            // Check is data posted 
            if (Request.HttpMethod == "POST")
            {
                //### Set variables and get POST data
                System.Collections.Hashtable result = new System.Collections.Hashtable();
                String userId = Request.Form["client_id"];
                String private_key = Request.Form["private_key"];
                String email = Request.Form["email"];
                String firstName = Request.Form["first_name"];
                String lastName = Request.Form["last_name"];
                String fileId = Request.Form["fileId"];
                String callback = Request.Form["callbackUrl"];
                String basePath = Request.Form["server_type"];
                // Set entered data to the results list
                result.Add("client_id", userId);
                result.Add("private_key", private_key);
                result.Add("email", email);
                result.Add("firstName", firstName);
                result.Add("lastName", lastName);
                result.Add("fileId", fileId);
                result.Add("callback", callback);
                result.Add("basePath", basePath);
                String message = null;
                // Check is all needed fields are entered
                if (userId == null || private_key == null || email == null || lastName == null || firstName == null || fileId == null)
                {
                    // If not all fields entered send error message
                    message = "Please enter all parameters";
                    result.Add("error", message);
                    return View("Sample22", null, result);
                }
                else
                {

                    // Create service for Groupdocs account
                    GroupdocsService service = new GroupdocsService(basePath, userId, private_key);
                    //Create User info object
                    Groupdocs.Api.Contract.UserInfo user = new Groupdocs.Api.Contract.UserInfo();
                    //Create Role info object
                    Groupdocs.Api.Contract.RoleInfo roleInfo = new Groupdocs.Api.Contract.RoleInfo();
                    //Create array of roles.
                    Groupdocs.Api.Contract.RoleInfo[] roleList = new Groupdocs.Api.Contract.RoleInfo[1];
                    //Set user role Id. Can be: 1 -  SysAdmin, 2 - Admin, 3 - User, 4 - Guest
                    roleInfo.Id = 3;
                    //Set user role name. Can be: SysAdmin, Admin, User, Guest
                    roleInfo.Name = "User";
                    roleList[0] = roleInfo;
                    //Set nick name as entered first name
                    user.NickName = firstName;
                    //Set first name as entered first name
                    user.FirstName = firstName;
                    //Set last name as entered last name
                    user.LastName = lastName;
                    user.Roles = roleList;
                    //Set email as entered email
                    user.PrimaryEmail = email;
                    //Creating of new User user - object with new user info
                    Groupdocs.Api.Contract.UserIdentity newUser = service.UpdateAccountUser(user);
                    // If request return's null return error to the template
                    if (newUser.Guid != null)
                    {
                        //Create array with entered email for SetAnnotationCollaborators method 
                        String[] emails = new String[1];
                        emails[0] = email;
                        //Make request to Ant api for set new user as annotation collaborator
                        Groupdocs.Api.Contract.Annotation.SetCollaboratorsResult addCollaborator = service.SetAnnotationCollaborators(fileId, emails, "2.0");
                        if (addCollaborator.Collaborators != null)
                        {
                            //Set reviewers rights for new user.
                            Groupdocs.Api.Contract.Annotation.SetReviewerRightsResult setReviewer = service.SetReviewerRights(fileId, addCollaborator.Collaborators);
                            //Make request to Annotation api to set CallBack session
                            Groupdocs.Api.Contract.Annotation.SetSessionCallbackUrlResult setCallback = service.SetSessionCallbackUrl(fileId, callback);
                            //Return GuId of new User to the template
                            result.Add("userId", newUser.Guid);
                            return View("Sample22", null, result);
                        }
                        //If Collaborators is empty return error
                        else
                        {
                            result.Add("error", "Collaborators is empty");
                            return View("Sample22", null, result);
                        }
                    }
                    //If upload file returns faile, return error to the template
                    else
                    {
                        result.Add("error", "Upload is failed");
                        return View("Sample22", null, result);
                    }

                }

            }
            // If data not posted return to template for filling of necessary fields
            else
            {
                return View("Sample22");
            }
        }

        //### <i>This sample will show how to use <b>GuId</b> of file to generate an embedded Viewer URL for a Document</i>
        public ActionResult Sample23()
        {
            // Check is data posted 
            if (Request.HttpMethod == "POST")
            {
                //### Set variables and get POST data
                System.Collections.Hashtable result = new System.Collections.Hashtable();
                String userId = Request.Form["client_id"];
                String private_key = Request.Form["private_key"];
                String fileId = Request.Form["fileId"];
                String basePath = Request.Form["server_type"];
                // Set entered data to the results list
                result.Add("client_id", userId);
                result.Add("private_key", private_key);
                result.Add("basePath", basePath);
                String message = null;
                // Check is all needed fields are entered
                if (userId == null || private_key == null || fileId == null)
                {
                    // If not all fields entered send error message
                    message = "Please enter all parameters";
                    result.Add("error", message);
                    return View("Sample23", null, result);
                }
                else
                {

                    // Create service for Groupdocs account
                    GroupdocsService service = new GroupdocsService(basePath, userId, private_key);
                    //Make request to get document pages as images
                    Groupdocs.Api.Contract.ViewDocumentResult pageImage = service.ViewDocument(fileId, 0, null, null, 100, false);
                    if (pageImage.Guid != null)
                    {
                        //Return page images GuId to the template
                        result.Add("fileId", pageImage.Guid);
                        return View("Sample23", null, result);
                    }
                    //If request return empty GuId set error for template
                    else
                    {
                        result.Add("error", "Something wrong with entered data");
                        return View("Sample23", null, result);
                    }

                }

            }
            // If data not posted return to template for filling of necessary fields
            else
            {
                return View("Sample23");
            }
        }

        //### <i>This sample will show how to use <b>Upload</b> method from Storage Api to upload file to GroupDocs Storage </i>
        public ActionResult Sample24()
        {
            // Check is data posted 
            if (Request.HttpMethod == "POST")
            {
                //### Set variables and get POST data
                System.Collections.Hashtable result = new System.Collections.Hashtable();
                String userId = Request.Form["client_id"];
                String private_key = Request.Form["private_key"];
                String url = Request.Form["url"];
                // Set entered data to the results list
                result.Add("client_id", userId);
                result.Add("private_key", private_key);
                result.Add("url", url);
                String message = null;
                // Check is all needed fields are entered
                if (userId == null || private_key == null || url == null)
                {
                    // If not all fields entered send error message
                    message = "Please enter all parameters";
                    result.Add("error", message);
                    return View("Sample24", null, result);
                }
                else
                {

                    // Create service for Groupdocs account
                    GroupdocsService service = new GroupdocsService("https://api.groupdocs.com/v2.0", userId, private_key);
                    //Make request to upload file from entered Url
                    String guid = service.UploadUrl(url);
                    if (guid != null)
                    {
                        //If file uploaded return his GuId to template
                        result.Add("fileId", guid);
                        return View("Sample24", null, result);
                    }
                    //If file wasn't uploaded return error
                    else
                    {
                        result.Add("error", "Something wrong with entered data");
                        return View("Sample24", null, result);
                    }

                }

            }
            // If data not posted return to template for filling of necessary fields
            else
            {
                return View("Sample24");
            }
        }
        //### Callback check for Sample18
        public ActionResult convert_callback()
        {

            String infoFile = AppDomain.CurrentDomain.BaseDirectory + "user_info.txt";
            //String fileInfo = AppDomain.CurrentDomain.BaseDirectory + "user_info.txt";

            //String userInfo = System.IO.File.ReadLines(infoFile);
            System.IO.StreamReader userInfoFile = new System.IO.StreamReader(infoFile);
            String userId = userInfoFile.ReadLine();
            String privateKey = userInfoFile.ReadLine();
            userInfoFile.Close();

            // Check is data posted 
            if (Request.HttpMethod == "POST")
            {     
                System.IO.StreamReader reader = new System.IO.StreamReader(Request.InputStream);
                String jsonString = reader.ReadToEnd();

                var data = System.Web.Helpers.Json.Decode(jsonString);
               
                String jobId = data.SourceId;
                jobId = jobId.Replace(" ", "");
                String fileId = "";
                 // Create service for Groupdocs account
                GroupdocsService service = new GroupdocsService("https://api.groupdocs.com/v2.0", userId, privateKey);
                //Make request to api for get document info by job id
                Groupdocs.Api.Contract.GetJobDocumentsResult job = service.GetJobDocuments(jobId);
                 String name = "";
                if (job.Inputs[0].Outputs[0].Guid != "")
                {
                    //Return file guid to the template
                   fileId = job.Inputs[0].Outputs[0].Guid;
                   name = job.Inputs[0].Outputs[0].Name;
                }
                

                // Definition of folder where to download file
                String downloadFolder = AppDomain.CurrentDomain.BaseDirectory + "downloads/";

                if (!Directory.Exists(downloadFolder))
                {
                    DirectoryInfo di = Directory.CreateDirectory(downloadFolder);
                }

                //### Make a request to Storage Api for dowloading file
                // Download file
                bool file = service.DownloadFile(fileId, downloadFolder + name);

                return View("convert_callback");
            }
            // If data not posted return to template for filling of necessary fields
            else
            {
                String jobId = userId + ":" + privateKey;
                //String jobId = System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "downloads/convert_callback.txt");
                return View("convert_callback", null, jobId);

            }
        }
       
        public String check_file()
        {
            // Check is data posted 
                String result = "";
                byte counter = 0; //counter to not wait forever
                String LocalPath = AppDomain.CurrentDomain.BaseDirectory + "downloads/";
                        do
                        {
                            if (counter >= 10)
                            {
                                result = "Error";
                                break;
                            }
                            System.Threading.Thread.Sleep(5000);

                            if (!Directory.Exists(LocalPath))
                            {
                                DirectoryInfo di = Directory.CreateDirectory(LocalPath);
                            }
                            string[] filePaths = Directory.GetFiles(LocalPath).Select(Path.GetFileName).ToArray();

                            if (filePaths.Length == 0)
                            {
                                counter++;
                                continue;
                            }
                            else
                            {
                                result = filePaths[0]; //get the name of the fist file in the directory
                                break;
                            }
                           

                        } while (true);

                        if (result.Equals("Error")) {
                            return "File was not found. Looks like something went wrong."; //exit and return error
                        }

                        //form the link to result file
                        result = "<a href='/downloads/" + result + "'>Converted file</a>";
                        return result;
            }

        //### Callback check for Sample19
        public ActionResult compare_callback()
        {
            // Check is data posted 
            if (Request.HttpMethod == "POST")
            {
                //Create txt file and write all resived from server data to this file
                String filePath = AppDomain.CurrentDomain.BaseDirectory + "downloads/compare_callback.txt";
                System.IO.StreamWriter w;
                w = System.IO.File.CreateText(filePath);
                w.WriteLine(Request.Params);
                w.Flush();
                w.Close();
                //Read all strings from file for template
                String r = System.IO.File.ReadAllText(filePath);
                return View("compare_callback", null, r);
            }
            // If data not posted return to template for filling of necessary fields
            else
            {
                String filePath = AppDomain.CurrentDomain.BaseDirectory + "downloads/compare_callback.txt";
                String r = System.IO.File.ReadAllText(filePath);
                return View("compare_callback", null, r);
                
            }
        }
        //### Callback check for Sample21
        public ActionResult signature_callback()
        {
            // Check is data posted 
            if (Request.HttpMethod == "POST")
            {
                //Create txt file and write all resived from server data to this file
                String filePath = AppDomain.CurrentDomain.BaseDirectory + "downloads/signature_callback.txt";
                System.IO.StreamWriter w;
                w = System.IO.File.CreateText(filePath);
                w.WriteLine(Request.Params);
                w.Flush();
                w.Close();
                //Read all strings from file for template
                String r = System.IO.File.ReadAllText(filePath);
                return View("signature_callback", null, r);
            }
            // If data not posted return to template for filling of necessary fields
            else
            {
                String filePath = AppDomain.CurrentDomain.BaseDirectory + "downloads/signature_callback.txt";
                String r = System.IO.File.ReadAllText(filePath);
                return View("signature_callback");
            }
        }
        //### Callback check for Sample22
        public ActionResult annotation_callback()
        {
            // Check is data posted 
            if (Request.HttpMethod == "POST")
            {
                //Create txt file and write all resived from server data to this file
                String filePath = AppDomain.CurrentDomain.BaseDirectory + "downloads/annotation_callback.txt";
                System.IO.StreamWriter w;
                w = System.IO.File.CreateText(filePath);
                w.WriteLine(Request.Params);
                w.Flush();
                w.Close();
                //Read all strings from file for template
                String r = System.IO.File.ReadAllText(filePath);
                return View("annotation_callback", null, r);
            }
            // If data not posted return to template for filling of necessary fields
            else
            {
                String filePath = AppDomain.CurrentDomain.BaseDirectory + "downloads/annotation_callback.txt";
                String r = System.IO.File.ReadAllText(filePath);
                return View("annotation_callback");
            }
        }

    }
}
