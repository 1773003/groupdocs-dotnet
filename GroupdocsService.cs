using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization.Json;
using Microsoft.Http;


namespace Groupdocs.Sdk
{
    using Groupdocs.Api.Contract;
    using Groupdocs.Api.Contract.Documents;
    using Groupdocs.Api.Contract.UriTemplates;
    using Groupdocs.Auxiliary;
    using Groupdocs.Common;
    using Groupdocs.Security;

    public partial class GroupdocsService : IGroupdocsService
    {
        protected HttpClient _client;

        static GroupdocsService()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.ServerCertificateValidationCallback =
                delegate(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                };
        }

        /// <summary>
        /// Initializes a new instance of the Saaspose.Api.Client.GroupdocsService class with the specified service base address, user ID and private key
        /// </summary>
        /// <param name="baseAddress">URL address of the SaaSpose API</param>
        /// <param name="userId">User identifier consuming the service</param>
        /// <param name="privateKey">User private key to access the service</param>
        public GroupdocsService(string baseAddress, string userId, string privateKey = null)
        {
            if (!Uri.IsWellFormedUriString(baseAddress, UriKind.Absolute))
            {
                throw new ArgumentException("The parameter is not a valid absolute URL.", "baseAddress");
            }

            BaseAddress = baseAddress;
            UserId = userId;
            PrivateKey = privateKey;

            _client = new HttpClient(BaseAddress);
            //_client.DefaultHeaders.ContentType = "application/json";
            _client.TransportSettings.ReadWriteTimeout = new TimeSpan(0, 10, 0);
            _client.TransportSettings.ConnectionTimeout = new TimeSpan(0, 5, 0);
        }

        /// <summary>
        /// Download file from user's account to a local file
        /// </summary>
        /// <param name="fileId">File identifier to be downloaded</param>
        /// <param name="localPath">Local file path with file name and extension</param>
        /// <returns>True if the file was downloaded successfully, false otherwise</returns>
        public bool DownloadFile(decimal fileId, string localPath)
        {
            Throw.IfNotGreater<decimal>(fileId, 0, "fileId");
            Throw.IfNull<string>(localPath, "localPath");

            var template = new UriTemplate(
                StorageApiUriTemplates.BuildUriTemplate(StorageApiUriTemplates.GetFile));
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "fileId", fileId.ToString() }
            };

            Uri url = template.BindByName(new Uri(BaseAddress), parameters);
            string signedUrl = UrlSignature.Sign(url.AbsoluteUri, PrivateKey);

            try
            {
                HttpResponseMessage response = _client.Get(signedUrl);
                response.EnsureStatusIs(HttpStatusCode.OK);

                using (Stream stream = response.Content.ReadAsStream())
                {
                    Groupdocs.Auxiliary.StreamDownloader.DownloadToFile(stream, localPath);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

    	/// <summary>
    	/// Uploads a local file to user account
    	/// </summary>
    	/// <param name="remotePath">Remote file path to upload</param>
    	/// <param name="description">File description</param>
    	/// <param name="localPath">Local file path (on a disk) to upload</param>
        /// <param name="system">A flag indicating whether the file is system or not.</param>
    	/// <returns>Newly uploaded file identifier</returns>
        public UploadRequestResult UploadFile(string remotePath, string description, string localPath, bool system = false)
        {
            using (Stream stream = File.OpenRead(localPath))
            {
                return UploadFile(remotePath, description, stream);
            }
        }

    	/// <summary>
    	/// Uploads a local file to user account
    	/// </summary>
		/// <param name="remotePath">File name being uploaded</param>
    	/// <param name="description">File description</param>
    	/// <param name="stream">File stream to upload</param>
    	/// <param name="fileName">File name assigned by the server</param>
        /// <param name="system">A flag indicating whether the file is system or not.</param>
    	/// <returns>Newly uploaded file identifier</returns>
        public UploadRequestResult UploadFile(string remotePath, string description, Stream stream, bool system = false)
        {
            // validate parameters
            Throw.IfNull<string>(remotePath, "remotePath");

            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            // prepare and submit request
            var template = StorageApiUriTemplates.BuildUriTemplate(StorageApiUriTemplates.Upload);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "path", remotePath.Trim('\\', '/') },
                { "description", description }
            };

            var message = new HttpRequestMessage { Method = "POST", Content = HttpContent.Create(stream, "application/octet-stream", null) };
            message.Headers.Add(HttpHeaders.SystemFile, String.Empty);

            UploadResponse response = SubmitRequest<UploadResponse>(template, parameters, message);
            if (response.Code != ResponseCode.Ok)
            {
                throw new GroupdocsServiceException(response.ErrorMessage);
            }

            return response.Result;
        }

        /// <summary>
        /// Uploads a web file to user account
        /// </summary>
        /// <param name="name">Url being uploaded</param>
        /// <returns>Newly uploaded file global unique identifier</returns>
        public string UploadUrl(string url)
        {
            // validate parameters
            Throw.IfNull<string>(url, "url");

            // prepare and submit request
            var template = StorageApiUriTemplates.BuildUriTemplate(StorageApiUriTemplates.UploadWeb);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "url", url },
            };

            UploadResponse response = SubmitRequest<UploadResponse>(template, parameters, "POST", HttpContent.CreateEmpty());
            return response.Result.Guid;
        }

        /// <summary>
        /// Schedules a file conversion job
        /// </summary>
        /// <param name="fileId">Previously uploaded file identifier to be converted</param>
        /// <param name="type">Semicolon-separated string of file types an original document will be converted to</param>
        /// <param name="description"></param>
        /// <param name="emailResults">Indicates whether converted files should be emailed back to the user</param>
        /// <returns>Newly created conversion job identifier</returns>
        public decimal ConvertFile(string fileId, string type, string description, bool emailResults = false, bool printScript = false)
        {
            Throw.IfNull<string>(fileId, "fileId");
            Throw.IfNull<string>(type, "type");

            var template = ApiUriTemplates.BuildUriTemplate(ApiUriTemplates.Convert);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "fileId", fileId },
                { "targetType", type },
                { "description", description },
                { "emailResults", emailResults.ToString() },
                { "printScript", printScript.ToString() }
            };

            ConvertResponse response = SubmitRequest<ConvertResponse>(template, parameters, "POST", HttpContent.CreateEmpty());
            return response.Result.JobId;
        }

        /// <summary>
        /// Compress existing file in user account
        /// </summary>
        /// <param name="fileId">File identifier to compress</param>
        /// <param name="type">Archive type to compress file to</param>
        /// <returns>Compressed file identifier</returns>
        public decimal CompressFile(decimal fileId, ArchiveType type)
        {
            Throw.IfNotGreater<decimal>(fileId, 0, "fileId");

            var template = StorageApiUriTemplates.BuildUriTemplate(StorageApiUriTemplates.Compress);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "fileId", fileId.ToString() },
                { "archiveType", type.ToString() }
            };

            CompressResponse response = SubmitRequest<CompressResponse>(template, parameters, "POST", HttpContent.CreateEmpty());
            return response.Result.Id;
        }

        public FileMoveResponse MoveFile(decimal fileId, string toPath, OverrideMode mode)
        {
            Throw.IfNotGreater<decimal>(fileId, 0, "fileId");

            var template = StorageApiUriTemplates.BuildUriTemplate(StorageApiUriTemplates.MoveFile);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "path", toPath },
                { "mode", mode.ToString()}
            };

            var message = new HttpRequestMessage { Method = "PUT", Content = HttpContent.CreateEmpty() };
            message.Headers.Add(HttpHeaders.FileMove, fileId.ToString());

            var response = SubmitRequest<FileMoveResponse>(template, parameters, message);
            return response;
        }

        public FileMoveResponse CopyFile(decimal fileId, string toPath, OverrideMode mode)
        {
            Throw.IfNotGreater<decimal>(fileId, 0, "fileId");

            var template = StorageApiUriTemplates.BuildUriTemplate(StorageApiUriTemplates.MoveFile);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "path", toPath },
                { "mode", mode.ToString()}
            };

            var message = new HttpRequestMessage { Method = "PUT", Content = HttpContent.CreateEmpty() };
            message.Headers.Add(HttpHeaders.FileCopy, fileId.ToString());

            var response = SubmitRequest<FileMoveResponse>(template, parameters, message);
            return response;
        }


        public FolderMoveResponse MoveFolder(string path, string toPath, OverrideMode mode)
        {
            var template = StorageApiUriTemplates.BuildUriTemplate(StorageApiUriTemplates.MoveFolder);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "path", toPath },
                { "mode", mode.ToString()}
            };

            using (var message = new HttpRequestMessage { Method = "PUT", Content = HttpContent.CreateEmpty() })
            {
                message.Headers.Add(HttpHeaders.FileMove, path.Trim('/', '\\'));

                var response = SubmitRequest<FolderMoveResponse>(template, parameters, message);
                return response;
            }
        }

        public FolderMoveResponse CopyFolder(string path, string toPath, OverrideMode mode)
        {
            var template = StorageApiUriTemplates.BuildUriTemplate(StorageApiUriTemplates.MoveFolder);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "path", toPath },
                { "mode", mode.ToString()}
            };

            using (var message = new HttpRequestMessage { Method = "PUT", Content = HttpContent.CreateEmpty() })
            {
                message.Headers.Add(HttpHeaders.FileCopy, path.Trim('/', '\\'));

                var response = SubmitRequest<FolderMoveResponse>(template, parameters, message);
                return response;
            }
        }

        /// <summary>
        /// Creates a new folder in user's storage.
        /// </summary>
        /// <param name="path">A relative path to the folder.</param>
        /// <returns>True if the folder has been created, false otherwise.</returns>
        public decimal CreateFolder(string path)
        {
            var template = StorageApiUriTemplates.BuildUriTemplate(StorageApiUriTemplates.CreateFolder);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "path", Uri.EscapeUriString(path.Trim('/', '\\')) }
            };

            var response = SubmitRequest<CreateFolderResponse>(template, parameters, "POST", HttpContent.CreateEmpty());
            return (response != null && response.Code == ResponseCode.Ok ? response.Result.Id : -1);
        }

        /// <summary>
        /// Archives files and folders at the specified paths to a zip package.
        /// </summary>
        /// <param name="packageName">The name of the package to be created.</param>
        /// <param name="paths">File/folder paths to be packed.</param>
        /// <returns>A package url.</returns>
        public string CreatePackage(string packageName, string[] paths)
        {
            var template = StorageApiUriTemplates.BuildUriTemplate(StorageApiUriTemplates.CreatePackage);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "packageName", packageName }
            };

            Uri url = new UriTemplate(template).BindByName(new Uri(BaseAddress), parameters);
            string signedUrl = UrlSignature.Sign(url.AbsoluteUri, PrivateKey);

            using (var content = HttpContentNetExtensions.CreateJsonNetDataContract<string[]>(paths))
            {
                var response = SubmitRequest<CreatePackageResponse>(template, parameters, "POST", content);
                return (response != null && response.Code == ResponseCode.Ok ? response.Result.Url : String.Empty);
            }
        }

        public StorageInfoResult GetStorageInfo()
        {
            var template = StorageApiUriTemplates.BuildUriTemplate(StorageApiUriTemplates.GetInfo);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId }
            };

            var response = SubmitRequest<StorageInfoResponse>(template, parameters);
            return (response.Code == ResponseCode.Ok ? response.Result : null);
        }

        /// <summary>
        /// Removes file from user account
        /// </summary>
        /// <param name="fileId">File identifier to remove</param>
        /// <returns>True if the file is removed successfully, false otherwise</returns>
        public bool DeleteFile(string fileId)
        {
            Throw.IfNull<string>(fileId, "fileId");

            var template = StorageApiUriTemplates.BuildUriTemplate(StorageApiUriTemplates.Delete);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "fileId", fileId }
            };

            DeleteResponse response = SubmitRequest<DeleteResponse>(template, parameters, "DELETE");
            return (response.Code == ResponseCode.Ok);
        }

        /// <summary>
        /// Removes file or folder from user account at a specified path
        /// </summary>
        /// <param name="path">File path to remove</param>
        /// <returns>True if the file is removed successfully, false otherwise</returns>
        public bool DeleteFileSystemEntity(string path)
        {
            var template = StorageApiUriTemplates.BuildUriTemplate(StorageApiUriTemplates.DeleteFromFolder);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "path", path.Trim('/', '\\') }
            };

            DeleteResponse response = SubmitRequest<DeleteResponse>(template, parameters, "DELETE");
            return (response.Code == ResponseCode.Ok);
        }

        /// <summary>
        /// Moves a file or folder to a trash folder
        /// </summary>
        /// <param name="path">File or folder path to move.</param>
        /// <returns>True if the file/folder is moved successfully, false otherwise</returns>
        public bool MoveToTrash(string path)
        {
            var template = StorageApiUriTemplates.BuildUriTemplate(StorageApiUriTemplates.MoveToTrash);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "path", path.Trim('/', '\\') }
            };

            var response = SubmitRequest<FolderMoveResponse>(template, parameters, "PUT", HttpContent.CreateEmpty());
            return (response.Code == ResponseCode.Ok);
        }


        /// <summary>
        /// Restores a removed file or folder from the trash
        /// </summary>
        /// <param name="path">File of folder path to restore</param>
        /// <returns>True if the file/folder is restored, false otherwise</returns>
        public bool RestoreFileSystemEntity(string path)
        {
            var template = StorageApiUriTemplates.BuildUriTemplate(StorageApiUriTemplates.RestoreFromTrash);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "path", path.Trim('/', '\\') }
            };

            DeleteResponse response = SubmitRequest<DeleteResponse>(template, parameters, "DELETE");
            return (response.Code == ResponseCode.Ok);
        }

        /// <summary>
        /// Creates a new conversion job
        /// </summary>
        /// <param name="actions">Bit mask of job actions to be performed</param>
        /// <param name="formats">Semicolon-separated list of job output documents formats.</param>
        /// <param name="urlOnly">If true, indicates that converted documents will not be attached to the notification email. The parameter is not used when emailResults is set to falst.</param>
        /// <param name="emailResults">Indicates that conversion notification email must be sent.</param>
        /// <returns>Created job identifier</returns>
        public decimal CreateJob(JobActions actions, string formats, bool urlOnly, bool emailResults = false)
        {
            if (actions == JobActions.None)
            {
                throw new ArgumentOutOfRangeException("actions", "Job actions should contain at least one action.");
            }

            var template = ApiUriTemplates.BuildUriTemplate(ApiUriTemplates.CreateJob);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId }
            };

            var job = new JobInfo { Actions = actions, UrlOnly = urlOnly, EmailResults = emailResults, OutputFormats = formats };
            var content = HttpContentNetExtensions.CreateJsonNetDataContract<JobInfo>(job);
            var response = SubmitRequest<CreateJobResponse>(template, parameters, "POST", content);

            return response.Result.JobId;
        }

        /// <summary>
        /// Assings a document to an existing job for further processing
        /// </summary>
        /// <param name="jobId">Job identifier to assign document to.</param>
        /// <param name="fileId">File identifier to assign.</param>
        /// <param name="formats">Optional semicolon-delimited list of output formats to convert document to. If specified, ovverides job file formats.</param>
        /// <returns>True if the document is added successfully, false otherwise</returns>
        public bool AddJobDocument(decimal jobId, string fileGuid, string formats)
        {
            Throw.IfNotGreater<decimal>(jobId, 0, "jobId");
            Throw.IfNull<string>(fileGuid, "fileGuid");

            var template = ApiUriTemplates.BuildUriTemplate(ApiUriTemplates.AddJobDocument);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "jobId", jobId.ToString() },
                { "fileId", fileGuid },
                { "formats", formats }
            };

            AddJobDocumentResponse response = SubmitRequest<AddJobDocumentResponse>(template, parameters, "PUT", HttpContent.CreateEmpty());
            return (response.Code == ResponseCode.Ok);
        }

        /// <summary>
        /// Assings a document URL to an existing job for further processing
        /// </summary>
        /// <param name="jobId">Job identifier to assign document to.</param>
        /// <param name="absoluteUrl">WEB page or file URL.</param>
        /// <param name="formats">Optional semicolon-delimited list of output formats to convert document to. If specified, ovverides job file formats.</param>
        /// <returns>Created document identifier</returns>
        public decimal AddJobDocumentUrl(decimal jobId, string absoluteUrl, string formats)
        {
            Throw.IfNotGreater<decimal>(jobId, 0, "jobId");
            Throw.IfNull<string>(absoluteUrl, "absoluteUrl");

            var template = ApiUriTemplates.BuildUriTemplate(ApiUriTemplates.AddJobDocumentUrl);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "jobId", jobId.ToString() },
                { "absoluteUrl", absoluteUrl },
                { "formats", formats }
            };

            AddJobDocumentResponse response = SubmitRequest<AddJobDocumentResponse>(template, parameters, "PUT", HttpContent.CreateEmpty());
            return response.Result.DocumentId;
        }

        /// <summary>
        /// Schedules job for execution.
        /// </summary>
        /// <param name="jobId">Job identifier to schedule.</param>
        /// <returns>True if the job is scheduled successfully, false otherwise.</returns>
        public bool ScheduleJob(decimal jobId)
        {
            return UpdateJob(new JobInfo { Id = jobId, Status = JobStatus.Pending });
        }

        /// <summary>
        /// Update job details and/or status.
        /// </summary>
        /// <param name="job">Job to update.</param>
        /// <returns>True if the job is updated successfully, false otherwise.</returns>
        public bool UpdateJob(JobInfo job)
        {
            Throw.IfNull<JobInfo>(job, "job");
            Throw.IfNotGreater<decimal>(job.Id, 0, "job.Id");

            var template = ApiUriTemplates.BuildUriTemplate(ApiUriTemplates.UpdateJob);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "jobId", job.Id.ToString() }
            };

            var content = HttpContentNetExtensions.CreateJsonNetDataContract<JobInfo>(job);
            var response = SubmitRequest<UpdateJobResponse>(template, parameters, "PUT", content);

            return (response.Code == ResponseCode.Ok);
        }

        /// <summary>
        /// Returns job information
        /// </summary>
        /// <param name="jobId">Job identifier to return status for.</param>
        /// <returns>Job status value.</returns>
        public JobInfo GetJob(decimal jobId)
        {
            Throw.IfNotGreater<decimal>(jobId, 0, "jobId");

            var template = ApiUriTemplates.BuildUriTemplate(ApiUriTemplates.GetJobJson);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "jobId", jobId.ToString() }
            };

            GetJobResponse response = SubmitRequest<GetJobResponse>(template, parameters);
            return response.Result;
        }

        /// <summary>
        /// Return a list of job documents.
        /// </summary>
        /// <param name="jobId">Job identifier to return documents for.</param>
        /// <returns>A structure containing job status and its documents.</returns>
        public GetJobDocumentsResult GetJobDocuments(decimal jobId)
        {
            Throw.IfNotGreater<decimal>(jobId, 0, "jobId");

            var template = ApiUriTemplates.BuildUriTemplate(ApiUriTemplates.GetJobDocuments);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "jobId", jobId.ToString() }
            };

            GetJobDocumentsResponse response = SubmitRequest<GetJobDocumentsResponse>(template, parameters);
            return response.Result;
        }

        /// <summary>
        /// Returns recent user's jobs
        /// </summary>
        /// <param name="count">Number of jobs to return.</param>
        /// <returns>Structure containing information about jobs and theirs documents</returns>
        public GetJobsResult GetJobs(int pageIndex, int pageSize, JobActions actions = JobActions.None, JobActions excludedActions = JobActions.None)
        {
            var template = ApiUriTemplates.BuildUriTemplate(ApiUriTemplates.GetJobs);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "pageIndex", pageIndex.ToString() },
                { "pageSize", pageSize.ToString() },
                { "actions", actions.ToString("d") },
                { "excludedActions", excludedActions.ToString("d") }
            };

            GetJobsResponse response = SubmitRequest<GetJobsResponse>(template, parameters);
            return response.Result;
        }

        /// <summary>
        /// Returns a list of all jobs documents.
        /// </summary>
        /// <param name="pageIndex">Page index to return documents from.</param>
        /// <param name="pageSize">Number of documents to return.</param>
        /// <param name="actions">Job actions to include into the result set.</param>
        /// <param name="excludedActions">Job actions to exclude from the result set.</param>
        /// <returns></returns>
        public GetJobsDocumentsResult GetJobsDocuments(int pageIndex, int pageSize, JobActions actions = JobActions.None, JobActions excludedActions = JobActions.None, string orderBy = null, bool orderAsc = true)
        {
            var template = ApiUriTemplates.BuildUriTemplate(ApiUriTemplates.GetJobsDocuments);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "pageIndex", pageIndex.ToString() },
                { "pageSize", pageSize.ToString() },
                { "actions", actions.ToString("d") },
                { "excludedActions", excludedActions.ToString("d") },
                { "orderBy", (orderBy ?? String.Empty) },
                { "orderAsc", orderAsc.ToString() }
            };

            GetJobsDocumentsResponse response = SubmitRequest<GetJobsDocumentsResponse>(template, parameters);
            return response.Result;
        }

        /// <summary>
        /// Enumerates file system entities those belong to user account
        /// </summary>
        /// <param name="path">Relative path to enumerate entities for.</param>
        /// <returns>Array of file system entity records.</returns>
        public ListEntitiesResult GetFileSystemEntities(string path, int pageIndex = 0, int pageSize = -1, string orderBy = null, bool orderAsc = true, string filter = null, string fileTypes = null, bool extended = false)
        {
            /*if (Path.IsPathRooted(path))
            {
                throw new ArithmeticException("Path must be related.");
            }*/

            var template = StorageApiUriTemplates.BuildUriTemplate(StorageApiUriTemplates.ListEntities);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "path", path.Trim('/', '\\') },
                { "pageIndex", pageIndex.ToString() },
                { "pageSize", pageSize.ToString() },
                { "orderBy", (orderBy ?? String.Empty) },
                { "orderAsc", orderAsc.ToString() },
                { "filter", filter },
                { "fileTypes", fileTypes },
                { "extended", extended.ToString() }
            };

            ListEntitiesResponse response = SubmitRequest<ListEntitiesResponse>(template, parameters);
            return response.Result;
        }

        /// <summary>
        /// Enumerates file system entities those belong to user account
        /// </summary>
        /// <param name="path">Relative path to enumerate entities for.</param>
        /// <returns>Array of file system entity records.</returns>
        public ListEntitiesResult GetFileSystemEntities(string path, int pageIndex = 0, int pageSize = -1, string orderBy = null, bool orderAsc = true, string filter = null, FileType[] fileTypes = null)
        {
            var types = fileTypes != null ? String.Join(",", fileTypes.Select(ft => ft.ToString().ToLower())) : null;
            return GetFileSystemEntities(path, pageIndex, pageSize, orderBy, orderAsc, filter, types);
        }

        /// <summary>
        /// Returns a list of file types a document may be converted to
        /// </summary>
        /// <param name="fileId">File identifier to get file types for.</param>
        /// <returns>Array of file types</returns>
        public string[] GetDocumentFormats(decimal fileId)
        {
            Throw.IfNotGreater<decimal>(fileId, 0, "fileId");

            var template = DocumentApiUriTemplates.BuildUriTemplate(DocumentApiUriTemplates.GetFormats);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "fileId", fileId.ToString() }
            };

            GetDocumentForeignTypesResponse response = SubmitRequest<GetDocumentForeignTypesResponse>(template, parameters);
            return (response.Result != null && !string.IsNullOrEmpty(response.Result.Types) ?
                response.Result.Types.Split(';') : new string[0]);
        }

        /// <summary>
        /// Assings a data source to an existing job's document for further processing. Data source should be an xml stream and comply to common xsd schema
        /// </summary>
        /// <param name="jobId">Job identifier to assign to.</param>
        /// <param name="fileId">Doc identifier to assign to.</param>
        /// <param name="dataSource">Data source xml stream to assign.</param>
        /// <returns>Created data source identifier</returns>
        public decimal AddJobDocumentDataSource(decimal jobId, decimal fileId, decimal datasourceId)
        {
            Throw.IfNotGreater<decimal>(fileId, 0, "fileId");
            Throw.IfNotGreater<decimal>(jobId, 0, "jobId");
            Throw.IfNotGreater<decimal>(datasourceId, 0, "datasourceId");

            var template = MergeApiUriTemplates.BuildUriTemplate(MergeApiUriTemplates.AddJobDocumentDataSource);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "fileId", fileId.ToString() },
                { "jobId", jobId.ToString() },
                { "datasourceId", datasourceId.ToString() }
            };

            AddDocumentDataSourceResponse response = SubmitRequest<AddDocumentDataSourceResponse>(template, parameters, "PUT", HttpContent.CreateEmpty());
            return (response.Code == ResponseCode.Ok ? response.Result.DataSourceId : -1);
        }

        /// <summary>
        /// Assings a data source to an existing job's document for further processing. Data source is passed as PlainDataSource type
        /// </summary>
        /// <param name="jobId">Job identifier to assign to.</param>
        /// <param name="fileId">Doc identifier to assign to.</param>
        /// <param name="dataSource">Data source as PlainDataSource to assign.</param>
        /// <returns>Created data source identifier</returns>
        public decimal AddJobDocumentDataSource(decimal jobId, decimal fileId, DatasourceField[] fields)
        {
            Throw.IfNotGreater<decimal>(fileId, 0, "fileId");
            Throw.IfNotGreater<decimal>(jobId, 0, "jobId");
            Throw.IfNull(fields, "fields");

            var template = MergeApiUriTemplates.BuildUriTemplate(MergeApiUriTemplates.AddJobDocumentDataSourceFields);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "fileId", fileId.ToString() },
                { "jobId", jobId.ToString() }
            };

            var content = HttpContentNetExtensions.CreateJsonNetDataContract<DatasourceField[]>(fields);
            var response = SubmitRequest<AddDocumentDataSourceResponse>(template, parameters, "PUT", content);
            return (response.Code == ResponseCode.Ok ? response.Result.DataSourceId : -1);
        }

        /// <summary>
        /// Schedules a data source binding job.
        /// </summary>
        /// <param name="fileId">Previously uploaded file identifier to be processed.</param>
        /// <param name="datasourceId">Previously created data source identifier to be bound.</param>
        /// <param name="targetType">A file type of the generated document</param>
        /// <param name="emailResults">Indicates whether binded files should be emailed back to the user</param>
        /// <returns>Newly created job identifier</returns>
        public decimal MergeTemplate(string fileId, decimal datasourceId, string targetType = null, bool emailResults = false)
        {
            Throw.IfNull<string>(fileId, "fileId");
            Throw.IfNotGreater<decimal>(datasourceId, 0, "datasourceId");

            var template = MergeApiUriTemplates.BuildUriTemplate(MergeApiUriTemplates.MergeDatasource);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "fileId", fileId.ToString() },
                { "datasourceId", datasourceId.ToString() },
                { "targetType", targetType },                
                { "emailResults", emailResults.ToString() }
            };

            MergeTemplateResponse response =  SubmitRequest<MergeTemplateResponse>(template, parameters, "POST", HttpContent.CreateEmpty());
            return (response.Code == ResponseCode.Ok ? response.Result.JobId : -1);
        }

        /// <summary>
        /// Schedules a data source binding job.
        /// </summary>
        /// <param name="fileId">Previously uploaded file identifier to be processed</param>
        /// <param name="fields">An array of data source fields to be bound.</param>
        /// <param name="targetType">A file type of the generated document.</param>
        /// <param name="emailResults">Indicates whether binded files should be emailed back to the user</param>
        /// <returns>Newly created job identifier</returns>
        public decimal MergeTemplate(string fileId, DatasourceField[] fields, string targetType = null, bool emailResults = false, string assemblyName = null)
        {
            Throw.IfNull<string>(fileId, "fileId");
            Throw.IfNull<DatasourceField[]>(fields, "fields");

            var template = MergeApiUriTemplates.BuildUriTemplate(MergeApiUriTemplates.MergeDatasourceFields);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "fileId", fileId.ToString() },
                { "targetType", targetType },                
                { "emailResults", emailResults.ToString() },
                { "assemblyName", assemblyName }
            };

            var content = HttpContentNetExtensions.CreateJsonNetDataContract<DatasourceField[]>(fields);
            var response = SubmitRequest<MergeTemplateResponse>(template, parameters, "POST", content);
            return (response.Code == ResponseCode.Ok ? response.Result.JobId : -1);
        }

        /// <summary>
        /// Returns a list of template fields a document contains.
        /// </summary>
        /// <param name="fileId">Previously uploaded file identifier to be processed</param>
        /// <param name="includeGeometry">The flag indicates whether fields location and size will be included into the response.</param>
        /// <returns>Template fields data as TemplateFieldsResult</returns>
        public TemplateFieldsResult GetTemplateFields(String fileId, bool includeGeometry = true)
        {
            Throw.IfNull<string>(fileId, "fileId");

            var template = DocumentApiUriTemplates.BuildUriTemplate(DocumentApiUriTemplates.GetFields);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "fileId", fileId },
                { "includeGeometry", includeGeometry.ToString() }
            };

            var response = SubmitRequest<TemplateFieldsResponse>(template, parameters);
            return response == null || response.Code != ResponseCode.Ok ? null : response.Result;
        }

        /// <summary>
        /// Returns a collection of pageCount page images for a document, starting from startPageIndex
        /// </summary>
        /// <param name="fileId">File id of document</param>
        /// <param name="startPageIndex">The first index of page to be viewed</param>
        /// <param name="pageCount">The total number of pages to be viewed</param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="keepAspectRatio"></param>
        /// <returns>Collection of images</returns>
        public ViewDocumentResult ViewDocument(string fileId, int pageNumber = 0, int? pageCount = null, int? quality = null, bool usePdf = true)
        {
            Throw.IfNull<string>(fileId, "fileId");
            Throw.IfNotGreater<decimal>(pageCount ?? 1, 0, "pageCount");

            var template = DocumentApiUriTemplates.BuildUriTemplate(DocumentApiUriTemplates.CreateThumbnails);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "fileId", fileId },
                { "pageNumber", pageNumber.ToString() },
                { "pageCount", pageCount.HasValue ? pageCount.Value.ToString() : null },
                { "quality", quality.HasValue ? quality.Value.ToString() : null },
                { "usePdf", usePdf.ToString() }
            };

            var response = SubmitRequest<ViewDocumentResponse>(template, parameters, "POST", HttpContent.CreateEmpty());
            return response == null  || response.Code != ResponseCode.Ok ? null : response.Result;
        }

        /// <summary>
        /// Returns a xml for a document(convert document to xml)
        /// </summary>
        /// <param name="guid">File identifier to be converted</param>
        public Stream GetXml(string guid)
        {
            Throw.IfNull<string>(guid, "guid");

            var template = new UriTemplate(SharedApiUriTemplates.BuildUriTemplate(SharedApiUriTemplates.GetXml));
            var parameters = new NameValueCollection()
            {
                {"guid", guid}
            };
            Uri url = template.BindByName(new Uri(BaseAddress), parameters);

            try
            {
                HttpResponseMessage response = _client.Get(url.ToString());
                response.EnsureStatusIs(HttpStatusCode.OK);

                return response.Content.ReadAsStream();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Returns a collection of page images for a embed document
        /// </summary>
        /// <param name="guid">Guid id of document</param>
        /// <param name="quality">Document quality</param>
        /// <param name="usePdf">Should generate Xml from Pdf</param>
        /// <returns>Collection of images</returns>
        public ViewDocumentResult ViewEmbedDocument(string guid, int pageNumber = 0, int? pageCount = null, int? quality = null, bool usePdf = true)
        {
            Throw.IfNull<string>(guid, "guid");

            var template = SharedApiUriTemplates.BuildUriTemplate(SharedApiUriTemplates.CreateThumbnails);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "guid", guid },
                { "pageNumber", pageNumber.ToString() },
                { "pageCount", pageCount.HasValue ? pageCount.Value.ToString() : null },
                { "quality", quality.HasValue ? quality.Value.ToString() : null },
                { "usePdf", usePdf.ToString() }
            };

            var response = SubmitRequest<ViewDocumentResponse>(template, parameters, "POST", HttpContent.CreateEmpty());
            return response == null  || response.Code != ResponseCode.Ok ? null : response.Result;
        }

        /// <summary>
        /// Returns a document viewing history
        /// </summary>
        /// <param name="startIndex">The index of first historical entry</param>
        /// <param name="pageSize">The number of historical entries</param>
        /// <returns>DocumentViewsResult metadata</returns>
        public DocumentViewsResult GetDocumentViews(int startIndex, int pageSize = -1)
        {                     
            Throw.IfNotGreater<int>(startIndex, -1, "startIndex");
            Throw.IfNotGreater<int>(pageSize == -1 ? 1 : pageSize, 0, "pageSize");

            var template = DocumentApiUriTemplates.BuildUriTemplate(DocumentApiUriTemplates.GetViews);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },                
                { "startIndex", startIndex.ToString() },
                { "pageSize", pageSize.ToString() }
            };

            var response = SubmitRequest<DocumentViewsResponse>(template, parameters);
            return response == null || response.Code != ResponseCode.Ok ? null : response.Result;
        }

        /// <summary>
        /// Shares a document with users
        /// </summary>
        /// <param name="sharers">Identifiers/emails of users to share document with</param>
        /// <param name="fileId">File identifier</param>
        /// <returns>SharedUsersResult metadata</returns>
        public SharedUsersResult ShareDocument(decimal fileId, string[] sharers)
        {
            Throw.IfNotGreater<decimal>(fileId, 0, "fileId");
            Throw.IfNull<string[]>(sharers, "sharers");

            var template = DocumentApiUriTemplates.BuildUriTemplate(DocumentApiUriTemplates.SetSharers);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "fileId", fileId.ToString() }
            };

            var content = HttpContentNetExtensions.CreateJsonNetDataContract<string[]>(sharers);
            var response = SubmitRequest<SharedUsersResponse>(template, parameters, "PUT", content);
            return response == null || response.Code != ResponseCode.Ok ? null : response.Result;
        }

        /// <summary>
        /// Unshares a document
        /// </summary>
        /// <param name="fileId">File identifier</param>
        /// <returns>SharedUsersResult metadata</returns>
        public SharedUsersResult UnshareDocument(decimal fileId)
        {
            Throw.IfNotGreater<decimal>(fileId, 0, "fileId");

            var template = DocumentApiUriTemplates.BuildUriTemplate(DocumentApiUriTemplates.RemoveSharers);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "fileId", fileId.ToString() }
            };

            var response = SubmitRequest<SharedUsersResponse>(template, parameters, "DELETE", HttpContent.CreateEmpty());
            return response == null || response.Code != ResponseCode.Ok  ? null : response.Result;
        }

        /// <summary>
        /// Shares a folder with users
        /// </summary>
        /// <param name="sharers">Emails of users to share document with.</param>
        /// <param name="folderId">Folder identifier to share.</param>
        /// <returns>A list of users a folder is shared with.</returns>
        public SharedUsersResult ShareFolder(decimal folderId, string[] sharers)
        {
            Throw.IfNotGreater<decimal>(folderId, 0, "folderId");
            Throw.IfNull<string[]>(sharers, "sharers");

            var template = DocumentApiUriTemplates.BuildUriTemplate(DocumentApiUriTemplates.SetFolderSharers);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "folderId", folderId.ToString() }
            };

            using (var content = HttpContentNetExtensions.CreateJsonNetDataContract<string[]>(sharers))
            {
                var response = SubmitRequest<SharedUsersResponse>(template, parameters, "PUT", content);
                return (response != null && response.Code == ResponseCode.Ok ? response.Result : null);
            }
        }

        /// <summary>
        /// Unshares a folder
        /// </summary>
        /// <param name="folderId">Folder identifier to unshare.</param>
        /// <returns>True if the folder is unshared, false otherwise.</returns>
        public bool UnshareFolder(decimal folderId)
        {
            Throw.IfNotGreater<decimal>(folderId, 0, "folderId");

            var template = DocumentApiUriTemplates.BuildUriTemplate(DocumentApiUriTemplates.RemoveFolderSharers);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "folderId", folderId.ToString() }
            };

            var response = SubmitRequest<SharedUsersResponse>(template, parameters, "DELETE", HttpContent.CreateEmpty());
            return (response != null && response.Code == ResponseCode.Ok);
        }

        /// <summary>
        /// Returns a list of users a folder is shared with.
        /// </summary>
        /// <param name="folderId">Folder identifier.</param>
        /// <returns>An array of users.</returns>
        public UserInfo[] GetFolderSharers(decimal folderId)
        {
            Throw.IfNotGreater<decimal>(folderId, 0, "folderId");

            var template = DocumentApiUriTemplates.BuildUriTemplate(DocumentApiUriTemplates.GetFolderSharers);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "folderId", folderId.ToString() }              
            };

            var response = SubmitRequest<SharedUsersResponse>(template, parameters);
            return (response == null || response.Code != ResponseCode.Ok ? null : response.Result.SharedUsers);
        }

        /// <summary>
        /// Returns a document access data - e.g. document access mode, owhership and sharing data
        /// </summary>
        /// <param name="fileId">File identifier</param>
        /// <returns>DocumentAccessInfoResult metadata</returns>
        public DocumentAccessInfoResult GetDocumentAccessInfo(decimal fileId)
        {
            Throw.IfNotGreater<decimal>(fileId, 0, "fileId");

            var template = DocumentApiUriTemplates.BuildUriTemplate(DocumentApiUriTemplates.GetDocumentAccessInfo);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "fileId", fileId.ToString() }
            };

            var response = SubmitRequest<DocumentAccessInfoResponse>(template, parameters);
            return response == null || response.Code != ResponseCode.Ok ? null : response.Result;
        }

        /// <summary>
        /// Sets a document access mode
        /// </summary>
        /// <param name="fileId">File identifier</param>
        /// <param name="mode">Access mode</param>
        /// <returns>DocumentAccessInfoResult metadata</returns>
        public DocumentAccessInfoResult SetDocumentAccessMode(decimal fileId, DocumentAccess mode)
        {
            Throw.IfNotGreater<decimal>(fileId, 0, "fileId");

            var template = DocumentApiUriTemplates.BuildUriTemplate(DocumentApiUriTemplates.SetDocumentAccessMode);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "fileId", fileId.ToString( )},
                { "mode", mode.ToString("d") }
            };

            var response =
                SubmitRequest<DocumentAccessInfoResponse>(template, parameters, "PUT", HttpContent.CreateEmpty());
            return response == null || response.Code != ResponseCode.Ok  ? null : response.Result;
        }

        /// <summary>
        /// Gets details for documents which were shared by the user and/or shared with the user.
        /// </summary>        
        /// <param name="mode">Shared by or shared with mode</param>
        /// <returns>SharedDocumentsResult metadata</returns>
        public SharedDocumentsResult GetSharedDocuments(SharesTypes types, int pageIndex = 0, int pageSize = -1, string orderBy = null, bool orderAsc = true)
        {
            var template = DocumentApiUriTemplates.BuildUriTemplate(DocumentApiUriTemplates.GetShares);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "sharesTypes", types.ToString().ToLower() },
                { "pageIndex", pageIndex.ToString() },
                { "pageSize", pageSize.ToString() },
                { "orderBy", (orderBy ?? String.Empty) },
                { "orderAsc", orderAsc.ToString() }
            };

            var response = SubmitRequest<SharedDocumentsResponse>(template, parameters);
            return response == null || response.Code != ResponseCode.Ok ? null : response.Result;
        }

        /// <summary>
        /// Used to accept/reject invitation
        /// </summary>
        /// <param name="fileId">File to be shared</param>
        /// <param name="status">Required status</param>
        /// <returns>DocumentUserStatus metadata</returns>
        public DocumentUserStatusResult SetDocumentUserStatus(decimal fileId, DocumentUserStatus status)
        {
            var template = DocumentApiUriTemplates.BuildUriTemplate(DocumentApiUriTemplates.SetSharerStatus);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "fileId", fileId.ToString() }
            };

            var content = HttpContentNetExtensions.CreateJsonNetDataContract<DocumentUserStatus>(status);
            var response = SubmitRequest<DocumentUserStatusResponse>(template, parameters, "PUT", content);
            return response == null || response.Code != ResponseCode.Ok ? null : response.Result;
        }

        /// <summary>
        /// Return a document metadata - e.g. page count (sheet count etc.)
        /// </summary>        
        /// <param name="fileId">File identifier</param>
        /// <returns>GetDocumentInfoResult metadata</returns>
        public GetDocumentInfoResult GetDocumentInfo(decimal fileId)
        {
            Throw.IfNotGreater<decimal>(fileId, 0, "fileId");

            var template = DocumentApiUriTemplates.BuildUriTemplate(DocumentApiUriTemplates.GetMetadata);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "fileId", fileId.ToString() }
            };

            var response = SubmitRequest<GetDocumentInfoResponse>(template, parameters);
            return response == null || response.Code != ResponseCode.Ok ? null : response.Result;
        }

        public QuestionnaireInfo GetQuestionnaire(decimal questionnaireId)
        {
            Throw.IfNotGreater<decimal>(questionnaireId, 0, "questionnaireId");

            var template = MergeApiUriTemplates.BuildUriTemplate(MergeApiUriTemplates.GetQuestionnaire);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "questionnaireId", questionnaireId.ToString() }
            };

            var response = SubmitRequest<GetQuestionnaireResponse>(template, parameters);
            return (response != null && response.Code == ResponseCode.Ok ? response.Result.Questionnaire : null);
        }

        public QuestionnaireInfo[] GetQuestionnaires()
        {
            var template = MergeApiUriTemplates.BuildUriTemplate(MergeApiUriTemplates.GetQuestionnaires);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId }
            };

            var response = SubmitRequest<GetQuestionnairesResponse>(template, parameters);
            return (response != null && response.Code == ResponseCode.Ok ? response.Result.Questionnaires : null);
        }

        /// <summary>
        /// Creates a new questionnaire
        /// </summary>
        /// <param name="questionnaire">A structure describing questionnaire.</param>
        /// <returns>A newly created questionnaire identifier.</returns>
        public decimal AddQuestionnaire(QuestionnaireInfo questionnaire)
        {
            Throw.IfNull<QuestionnaireInfo>(questionnaire, "questionnaire");

            var template = MergeApiUriTemplates.BuildUriTemplate(MergeApiUriTemplates.CreateQuestionnaire);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId }
            };

            var content = HttpContentNetExtensions.CreateJsonNetDataContract<QuestionnaireInfo>(questionnaire);
            var response = SubmitRequest<CreateQuestionnaireResponse>(template, parameters, "POST", content);
            return (response == null || response.Code != ResponseCode.Ok ? -1 : response.Result.QuestionnaireId);
        }

        /// <summary>
        /// Updates an existing questionnaire
        /// </summary>
        /// <param name="questionnaireId">Questionnaire identifier.</param>
        /// <param name="questionnaire">Questionnaire data.</param>
        /// <returns>True if the questionnaire has been updated.</returns>
        public bool UpdateQuestionnaire(decimal questionnaireId, QuestionnaireInfo questionnaire)
        {
            Throw.IfNotGreater<decimal>(questionnaireId, 0, "questionnaireId");
            Throw.IfNull<QuestionnaireInfo>(questionnaire, "questionnaire");

            var template = MergeApiUriTemplates.BuildUriTemplate(MergeApiUriTemplates.UpdateQuestionnaire);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "questionnaireId", questionnaireId.ToString() }
            };

            var content = HttpContentNetExtensions.CreateJsonNetDataContract<QuestionnaireInfo>(questionnaire);
            var response = SubmitRequest<UpdateQuestionnaireResponse>(template, parameters, "PUT", content);
            return (response != null && response.Code == ResponseCode.Ok);
        }

        public decimal AddDocumentQuestionnaire(string documentId, QuestionnaireInfo questionnaire)
        {
            Throw.IfNull<string>(documentId, "documentId");
            Throw.IfNull<QuestionnaireInfo>(questionnaire, "questionnaire");

            var template = MergeApiUriTemplates.BuildUriTemplate(MergeApiUriTemplates.CreateDocumentQuestionnaire);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "fileId", documentId.ToString() }
            };

            var content = HttpContentNetExtensions.CreateJsonNetDataContract<QuestionnaireInfo>(questionnaire);
            var response = SubmitRequest<AddDocumentQuestionnaireResponse>(template, parameters, "POST", content);
            return (response != null && response.Code == ResponseCode.Ok ? response.Result.QuestionnaireId : -1);
        }

        public bool AddDocumentQuestionnaire(string documentId, decimal questionnaireId)
        {
            Throw.IfNull<string>(documentId, "documentId");
            Throw.IfNotGreater<decimal>(questionnaireId, 0, "questionnaireId");

            var template = MergeApiUriTemplates.BuildUriTemplate(MergeApiUriTemplates.AddDocumentQuestionnaire);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "fileId", documentId.ToString() },
                { "questionnaireId", questionnaireId.ToString() }
            };

            var response = SubmitRequest<AddDocumentQuestionnaireResponse>(template, parameters, "PUT", HttpContent.CreateEmpty());
            return (response != null && response.Code == ResponseCode.Ok);
        }

        public QuestionnaireInfo[] GetDocumentQuestionnaires(string documentId)
        {
            Throw.IfNull<string>(documentId, "documentId");

            var template = MergeApiUriTemplates.BuildUriTemplate(MergeApiUriTemplates.GetDocumentQuestionnaires);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "fileId", documentId.ToString() }
            };

            var response = SubmitRequest<GetDocumentQuestionnairesResponse>(template, parameters);
            return (response != null && response.Code == ResponseCode.Ok ? response.Result.Questionnaires : new QuestionnaireInfo[0]);
        }

        public decimal AddDataSource(Datasource datasource)
        {
            Throw.IfNull<Datasource>(datasource, "datasource");

            var template = MergeApiUriTemplates.BuildUriTemplate(MergeApiUriTemplates.AddDataSource);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId }
            };

            using (var content = HttpContentNetExtensions.CreateJsonNetDataContract<Datasource>(datasource))
            {
                var response = SubmitRequest<AddDatasourceResponse>(template, parameters, "POST", content);
                return (response != null && response.Code == ResponseCode.Ok ? response.Result.DatasourceId : -1);
            }
        }


        public Datasource GetDatasource(decimal datasourceId, params string[] fields)
        {
            Throw.IfNotGreater<decimal>(datasourceId, 0, "datasourceId");

            var template = MergeApiUriTemplates.BuildUriTemplate(MergeApiUriTemplates.GetDataSource);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "datasourceId", datasourceId.ToString() },
                { "fields", (fields != null ? String.Join(",", fields) : String.Empty) }
            };

            var response = SubmitRequest<GetDatasourceResponse>(template, parameters);
            return response == null || response.Code != ResponseCode.Ok ? null : response.Result.Datasource;
        }

        /// <summary>
        /// Returns a list of data sources those were created using a specified questionnaire.
        /// </summary>
        /// <param name="questionnaireId">Questionnaire identifier used to create data sources.</param>
        /// <param name="includeFields">A flag indicating whether data source fields will be included into the response.</param>
        /// <returns>An array of data sources created using a questionnaire.</returns>
        public Datasource[] GetQuestionnaireDatasources(decimal questionnaireId, bool includeFields = false)
        {
            Throw.IfNotGreater<decimal>(questionnaireId, 0, "questionnaireId");

            var template = MergeApiUriTemplates.BuildUriTemplate(MergeApiUriTemplates.GetQuestionnaireDataSources);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "questionnaireId", questionnaireId.ToString() },
                { "includeFields", includeFields.ToString() }
            };

            var response = SubmitRequest<GetDatasourcesResponse>(template, parameters);
            return response == null || response.Code != ResponseCode.Ok ? null : response.Result.Datasources;
        }

        /// <summary>
        /// Updates specified data source fields; other fields remains unchanged.
        /// </summary>
        /// <param name="datasourceId">Data source identifier to update fields for.</param>
        /// <param name="datasource">Data source object.</param>
        /// <returns>True if the data source fields are update, false otherwise.</returns>
        public bool UpdateDatasourceFields(decimal datasourceId, Datasource datasource)
        {
            Throw.IfNotGreater<decimal>(datasourceId, 0, "datasourceId");
            Throw.IfNull<Datasource>(datasource, "datasource");

            var template = MergeApiUriTemplates.BuildUriTemplate(MergeApiUriTemplates.UpdateDataSourceFields);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "datasourceId", datasourceId.ToString() }
            };

            using (var content = HttpContentNetExtensions.CreateJsonNetDataContract<Datasource>(datasource))
            {
                var response = SubmitRequest<AddDatasourceResponse>(template, parameters, "PUT", content);
                return (response != null && response.Code == ResponseCode.Ok);
            }
        }

        public Stream GetImage(string guid, int page, int? quality = null, bool usePdf = true)
        {
            var template = new UriTemplate(
                SharedApiUriTemplates.BuildUriTemplate(SharedApiUriTemplates.GetImage));
            var parameters = new NameValueCollection()
            {
                { "guid", guid },
                { "folio", page.ToString() },
                { "dimension", "480x" },
                { "quality", quality.HasValue ? quality.Value.ToString() : null },
                { "usePdf", usePdf.ToString() }
            };

            Uri url = template.BindByName(new Uri(BaseAddress), parameters);

            HttpResponseMessage response = _client.Get(url);
            response.EnsureStatusIs(HttpStatusCode.OK);
            return response.Content.ReadAsStream();
        }

        public string GetImageUrl(string fileId, string dimension, int pageNumber = 0, int? quality = null, bool usePdf = true)
        {
            var template = new UriTemplate(SharedApiUriTemplates.BuildUriTemplate(SharedApiUriTemplates.GetImageUrl));
            var parameters = new NameValueCollection()
            {
                { "guid", fileId },
                { "folio", pageNumber.ToString() },
                { "dimension", dimension },
                { "quality", quality.HasValue ? quality.Value.ToString() : null },
                { "usePdf", usePdf.ToString().ToLower() }
            };

            Uri url = template.BindByName(new Uri(BaseAddress), parameters);

            try
            {
                HttpResponseMessage response = _client.Get(url.ToString());
                response.EnsureStatusIs(HttpStatusCode.OK);
                var json = response.Content.ReadAsJsonNetDataContract<GetUrlResponse>();

                return json.Result.Url;
            }
            catch
            {
                return null;
            }
        }

        public decimal AddQuestionnaireExecution(decimal questionnaireId, string executiveEmail, string approverEmail = null)
        {
            Throw.IfNotGreater<decimal>(questionnaireId, 0, "questionnaireId");
            Throw.IfNull<string>(executiveEmail, "executiveEmail");

            var template = MergeApiUriTemplates.BuildUriTemplate(MergeApiUriTemplates.AddQuestionnaireExecution);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "questionnaireId", questionnaireId.ToString() }
            };

            var execution = new QuestionnaireExecutionInfo
            {
                Executive = new UserIdentity { PrimaryEmail = executiveEmail },
                Approver = new UserIdentity { PrimaryEmail = approverEmail }
            };

            using (var content = HttpContentNetExtensions.CreateJsonNetDataContract<QuestionnaireExecutionInfo>(execution))
            {
                var response = SubmitRequest<AddQuestionnaireExecutionResponse>(template, parameters, "POST", content);
                return (response != null && response.Code == ResponseCode.Ok ? response.Result.ExecutionId : -1);
            }
        }

        public QuestionnaireExecutionInfo[] GetQuestionnaireExecutions()
        {
            var template = MergeApiUriTemplates.BuildUriTemplate(MergeApiUriTemplates.GetQuestionnaireExecutions);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId }
            };

            var response = SubmitRequest<GetQuestionnaireExecutionsResponse>(template, parameters);
            return (response != null && response.Code == ResponseCode.Ok ? response.Result.Executions : new QuestionnaireExecutionInfo[0]);
        }

        public bool UpdateQuestionnaireExecution(decimal executionId, QuestionnaireExecutionInfo execution)
        {
            Throw.IfNotGreater<decimal>(executionId, 0, "executionId");
            Throw.IfNull<QuestionnaireExecutionInfo>(execution, "execution");

            var template = MergeApiUriTemplates.BuildUriTemplate(MergeApiUriTemplates.UpdateQuestionnaireExecution);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "executionId", executionId.ToString() }
            };

            using (var content = HttpContentNetExtensions.CreateJsonNetDataContract<QuestionnaireExecutionInfo>(execution))
            {
                var response = SubmitRequest<UpdateQuestionnaireExecutionResponse>(template, parameters, "PUT", content);
                return (response != null && response.Code == ResponseCode.Ok);
            }
        }

        /// <summary>
        /// Updates an existing questionnaire execution status.
        /// </summary>
        /// <param name="executionId">Questionnaire execution identifier to update.</param>
        /// <param name="status">A new questionnaire execution status.</param>
        /// <returns>True if the questionnaire execution status is updated, false otherwise.</returns>
        public bool UpdateQuestionnaireExecutionStatus(decimal executionId, QuestionnaireExecutionStatus status)
        {
            Throw.IfNotGreater<decimal>(executionId, 0, "executionId");

            var template = MergeApiUriTemplates.BuildUriTemplate(MergeApiUriTemplates.UpdateQuestionnaireExecutionStatus);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "executionId", executionId.ToString() }
            };

            using (var content = HttpContentNetExtensions.CreateJsonNetDataContract<QuestionnaireExecutionStatus>(status))
            {
                var response = SubmitRequest<UpdateQuestionnaireExecutionResponse>(template, parameters, "PUT", content);
                return (response != null && response.Code == ResponseCode.Ok);
            }
        }

        /// <summary>
        /// Get subscription plans
        /// </summary>
        public GetSubscriptionPlanResult GetSubscriptionPlans()
        {
            var template = SystemApiUriTemplates.BuildUriTemplate(SystemApiUriTemplates.GetSubscriptionPlans);
            var parameters = new NameValueCollection()
            {
                { "callerId", UserId }
            };

            var response = SubmitRequest<GetSubscriptionPlansResponse>(template, parameters);
            return response == null || response.Code != ResponseCode.Ok ? null : response.Result;
        }

        /// <summary>
        /// Get internal user subscription plan
        /// </summary>
        public GetPlanResult GetUserPlan()
        {
            var template = SystemApiUriTemplates.BuildUriTemplate(SystemApiUriTemplates.GetUserPlan);
            var parameters = new NameValueCollection()
            {
                { "callerId", UserId },
                { "userId", UserId }
            };

            var response = SubmitRequest<GetPlanResponse>(template, parameters);
            return response == null || response.Code != ResponseCode.Ok ? null : response.Result;
        }

        /// <summary>
        /// Get external user subscription plan
        /// </summary>
        public GetUserSubscriptionPlanResult GetUserSubscription()
        {
            var template = SystemApiUriTemplates.BuildUriTemplate(SystemApiUriTemplates.GetUserSubscriptionPlan);
            var parameters = new NameValueCollection()
            {
                { "callerId", UserId },
                { "userId", UserId }
            };

            var response = SubmitRequest<GetUserSubscriptionPlanResponse>(template, parameters);
            return response == null || response.Code != ResponseCode.Ok ? null : response.Result;
        }

        /// <summary>
        /// Create/Upgrade user subscription
        /// </summary>
        public bool SetSubscription(string userId, int productId, SubscriptionPlanInfo subscriptionPlan)
        {
            var template = SystemApiUriTemplates.BuildUriTemplate(SystemApiUriTemplates.SetSubscriptionPlan);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "productId", productId.ToString() }
            };

            using (var content = HttpContentNetExtensions.CreateJsonNetDataContract<SubscriptionPlanInfo>(subscriptionPlan))
            {
                var response = SubmitRequest<SetUserSubscriptionPlanResponse>(template, parameters, "PUT", content);
                return (response != null && response.Code == ResponseCode.Ok);
            }
        }


        protected virtual void OnSendingRequest(HttpRequestMessage request)
        { }

        protected TResponse SubmitRequest<TResponse>(string uriTemplate, NameValueCollection uriParams, string method = "GET", HttpContent content = null)
            where TResponse : class, new()
        {
            HttpRequestMessage message = new HttpRequestMessage { Method = method, Content = content };
            return SubmitRequest<TResponse>(uriTemplate, uriParams, message);
        }

        protected TResponse SubmitRequest<TResponse>(string uriTemplate, NameValueCollection uriParams, HttpRequestMessage request)
            where TResponse : class, new()
        {
            ValidateRequestParams(uriTemplate, uriParams, request);

            var template = new UriTemplate(uriTemplate);

            Uri url = template.BindByName(new Uri(BaseAddress), uriParams);
            string signedUrl = UrlSignature.Sign(url.AbsoluteUri, PrivateKey);

            request.Uri = new Uri(signedUrl);

            HttpContent content = request.Content;
            _client.DefaultHeaders.ContentLength = (content != null && content.HasLength() ? new long?(content.GetLength()) : null);

            OnSendingRequest(request);

            return SendRequest<TResponse>(request);
        }

        public virtual TResponse SendRequest<TResponse>(HttpRequestMessage request)
        {
            using (HttpResponseMessage response = _client.Send(request))
            {
                response.EnsureStatusIs(HttpStatusCode.OK);

                TResponse json = response.Content.ReadAsJsonNetDataContract<TResponse>();
                return json;
            }
        }

        private void ValidateRequestParams(string uriTemplate, NameValueCollection uriParams, HttpRequestMessage request)
        {
            Throw.IfNull<string>(uriTemplate, "uriTemplate");
            Throw.IfNull<NameValueCollection>(uriParams, "uriParams");
            Throw.IfNull<HttpRequestMessage>(request, "request");
        }

        private UserInfoResult GetUserProfileByToken(string token, string uriTemplate)
        {
            var parameters = new NameValueCollection() { { "callerId", this.UserId }, { "token", System.Uri.EscapeUriString(token) } };
            var template = UserManagementUriTemplates.BuildUriTemplate(uriTemplate);
            var response = SubmitRequest<UserInfoResponse>(template, parameters);

            if (response.Code != ResponseCode.Ok)
            {
                throw new GroupdocsServiceException(response.ErrorMessage);
            }

            return response.Result;
        }

        #region Properties
        /// <summary>
        /// SaaSpose API server address
        /// </summary>
        public string BaseAddress { get; private set; }

        /// <summary>
        /// User identifier consuming the service
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// User private key to access the service
        /// </summary>
        public string PrivateKey { get; set; }
        #endregion Properties
    }

    internal static class HttpContentNetExtensions
    {
        public static HttpContent CreateJsonNetDataContract<T>(T value)
        {
            var serializer = new Newtonsoft.Json.JsonSerializer();

            using (System.IO.MemoryStream ms = new MemoryStream())
            using (System.IO.StreamWriter writer = new StreamWriter(ms))
            {
                serializer.Serialize(writer, value);
                writer.Flush();
                ms.Position = 0;
                return HttpContent.Create(ms.ToArray(), "application/json");
            }
        }

        public static T ReadAsJsonNetDataContract<T>(this HttpContent content)
        {
            var serializer = new Newtonsoft.Json.JsonSerializer();

            //var s = content.ReadAsString();
            using (Stream stream = content.ReadAsStream())
            {
                using (StreamReader sr = new StreamReader(stream))
                {
                    return (T) serializer.Deserialize(sr, typeof(T));
                }
            }
        }
    }
}


