using System;
using System.IO;
using Groupdocs.Api.Contract.Documents;

namespace Groupdocs.Sdk
{
    using Groupdocs.Common;
    using Groupdocs.Api.Contract;

    public partial interface IGroupdocsService
    {
        /// <summary>
        /// Removes file from user account
        /// </summary>
        /// <param name="fileId">File identifier to remove</param>
        /// <returns>True if the file is removed successfully, false otherwise</returns>
        bool DeleteFile(string fileId);

        /// <summary>
        /// Removes file or folder from user account at a specified path
        /// </summary>
        /// <param name="path">File path to remove</param>
        /// <returns>True if the file is removed successfully, false otherwise</returns>
        bool DeleteFileSystemEntity(string path);

        /// <summary>
        /// Moves a file or folder to a trash folder
        /// </summary>
        /// <param name="path">File or folder path to move.</param>
        /// <returns>True if the file/folder is moved successfully, false otherwise</returns>
        bool MoveToTrash(string path);

        /// <summary>
        /// Restores a removed file or folder from the trash
        /// </summary>
        /// <param name="path">File of folder path to restore</param>
        /// <returns>True if the file/folder is restored, false otherwise</returns>
        bool RestoreFileSystemEntity(string path);

        /// <summary>
        /// Download file from user's account to a local file
        /// </summary>
        /// <param name="fileId">File identifier to be downloaded</param>
		/// <param name="filePath">Local file path with file name and extension</param>
        /// <returns>True if the file was downloaded successfully, false otherwise</returns>
        bool DownloadFile(decimal fileId, string filePath);

    	/// <summary>
    	/// Uploads a local file to user account
    	/// </summary>
    	/// <param name="remotePath">Local file path to upload</param>
    	/// <param name="description">File description</param>
    	/// <param name="localPath">Path to a local file to upload.</param>
        /// <param name="system">A flag indicating whether the file is system or not.</param>
    	/// <returns>Newly uploaded file identifier</returns>
        UploadRequestResult UploadFile(string remotePath, string description, string localPath, bool system = false);

    	/// <summary>
    	/// Uploads a local file to user account
    	/// </summary>
		/// <param name="remotePath">File name being uploaded</param>
    	/// <param name="description">File description</param>
    	/// <param name="stream">File stream to upload</param>
    	/// <param name="adjustedName">File name received from the service</param>
        /// <param name="system">A flag indicating whether the file is system or not.</param>
    	/// <returns>Newly uploaded file identifier</returns>
        UploadRequestResult UploadFile(string remotePath, string description, Stream stream, bool system = false);

        /// <summary>
        /// Uploads a web file to user account
        /// </summary>
        /// <param name="name">Url being uploaded</param>
        /// <returns>Newly uploaded file global unique identifier</returns>
        string UploadUrl(string url);

        /// <summary>
        /// Schedules a file conversion job
        /// </summary>
        /// <param name="fileId">Previously uploaded file identifier to be converted</param>
        /// <param name="type">Semicolon-separated string of file types an original document will be converted to</param>
        /// <param name="description"></param>
        /// <param name="emailResults">Indicates whether converted files should be emailed back to the user</param>
        /// <returns>Newly created conversion job identifier</returns>
        decimal ConvertFile(string fileId, string type, string description, bool emailResults = false, bool printScript = false);

        /// <summary>
        /// Compress existing file in user account
        /// </summary>
        /// <param name="fileId">File identifier to compress</param>
        /// <param name="type">Archive type to compress file to</param>
        /// <returns>Compressed file identifier</returns>
        decimal CompressFile(decimal fileId, ArchiveType type);

        /// <summary>
        /// Moves file
        /// </summary>
        /// <param name="fileId">File identifier to move</param>
        /// <param name="toPath">Path to move to</param>
        /// <param name="mode">Override mode - interrupt/skip/rename/override</param>
        /// <returns>Operation metadata</returns>
        FileMoveResponse MoveFile(decimal fileId, string toPath, OverrideMode mode);

        /// <summary>
        /// Moves folder
        /// </summary>
        /// <param name="path">Folder path to move</param>
        /// <param name="toPath">Path to move to</param>
        /// <param name="mode">Override mode - interrupt/skip/rename/override</param>
        /// <returns>Operation metadata</returns>
        FolderMoveResponse MoveFolder(string path, string toPath, OverrideMode mode);

        /// <summary>
        /// Copies file
        /// </summary>
        /// <param name="fileId">File identifier to move</param>
        /// <param name="toPath">Path to move to</param>
        /// <param name="mode">Override mode - interrupt/skip/rename/override</param>
        /// <returns>Operation metadata</returns>
        FileMoveResponse CopyFile(decimal fileId, string toPath, OverrideMode mode);

        /// <summary>
        /// Copies folder
        /// </summary>
        /// <param name="path">Folder path to move</param>
        /// <param name="toPath">Path to move to</param>
        /// <param name="mode">Override mode - interrupt/skip/rename/override</param>
        /// <returns>Operation metadata</returns>
        FolderMoveResponse CopyFolder(string path, string toPath, OverrideMode mode);

        /// <summary>
        /// Creates a new folder in user's storage.
        /// </summary>
        /// <param name="path">A relative path to the folder.</param>
        /// <returns>True if the folder has been created, false otherwise.</returns>
        decimal CreateFolder(string path);

        /// <summary>
        /// Archives files and folders at the specified paths to a zip package.
        /// </summary>
        /// <param name="packageName">The name of the package to be created.</param>
        /// <param name="paths">File/folder paths to be packed.</param>
        /// <returns>A package url.</returns>
        string CreatePackage(string packageName, string[] paths);

        /// <summary>
        /// Returns user storage information: available space, total space, document credits, available document credits etc.
        /// </summary>
        /// <returns>A structure containing user's storage details.</returns>
        StorageInfoResult GetStorageInfo();

        /// <summary>
        /// Creates a new conversion job
        /// </summary>
        /// <param name="actions">Bit mask of job actions to be performed</param>
        /// <param name="formats">Semicolon-separated list of job output documents formats.</param>
        /// <param name="urlOnly">If true, indicates that converted documents will not be attached to the notification email. The parameter is not used when emailResults is set to falst.</param>
        /// <param name="emailResults">Indicates that conversion notification email must be sent.</param>
        /// <returns>Created job identifier</returns>
        decimal CreateJob(JobActions actions, string formats, bool urlOnly, bool emailResults = false);

        /// <summary>
        /// Assings a document to an existing job for further processing
        /// </summary>
        /// <param name="jobId">Job identifier to assign document to.</param>
        /// <param name="fileId">File identifier to assign.</param>
        /// <param name="formats">Optional semicolon-delimited list of output formats to convert document to. If specified, ovverides job file formats.</param>
        /// <returns>True if the document is added successfully, false otherwise</returns>
        bool AddJobDocument(decimal jobId, string fileGuid, string formats);

        /// <summary>
        /// Assings a document URL to an existing job for further processing
        /// </summary>
        /// <param name="jobId">Job identifier to assign document to.</param>
        /// <param name="absoluteUrl">WEB page or file URL.</param>
        /// <param name="formats">Optional semicolon-delimited list of output formats to convert document to. If specified, ovverides job file formats.</param>
        /// <returns>Created document identifier</returns>
        decimal AddJobDocumentUrl(decimal jobId, string absoluteUrl, string formats);

        /// <summary>
        /// Schedules job for execution.
        /// </summary>
        /// <param name="jobId">Job identifier to schedule.</param>
        /// <returns>True if the job is scheduled successfully, false otherwise.</returns>
        bool ScheduleJob(decimal jobId);

        /// <summary>
        /// Update job details and/or status.
        /// </summary>
        /// <param name="job">Job to update.</param>
        /// <returns>True if the job is updated successfully, false otherwise.</returns>
        bool UpdateJob(JobInfo job);

        /// <summary>
        /// Returns job information
        /// </summary>
        /// <param name="jobId">Job identifier to return status for.</param>
        /// <returns>Job status value.</returns>
        JobInfo GetJob(decimal jobId);

        /// <summary>
        /// Return a list of job documents.
        /// </summary>
        /// <param name="jobId">Job identifier to return documents for.</param>
        /// <returns>A structure containing job status and its documents.</returns>
        GetJobDocumentsResult GetJobDocuments(decimal jobId);

        /// <summary>
        /// Returns recent user's jobs
        /// </summary>
        /// <param name="pageIndex">Page index to return jobs from.</param>
        /// <param name="pageSize">Number of jobs to return.</param>
        /// <param name="actions"></param>
        /// <param name="excludedActions"></param>
        /// <returns>Structure containing information about jobs and theirs documents</returns>
        GetJobsResult GetJobs(int pageIndex, int pageSize, JobActions actions = JobActions.None, JobActions excludedActions = JobActions.None);

        /// <summary>
        /// Returns a list of all jobs documents.
        /// </summary>
        /// <param name="pageIndex">Page index to return documents from.</param>
        /// <param name="pageSize">Number of documents to return.</param>
        /// <param name="actions">Job actions to include into the result set.</param>
        /// <param name="excludedActions">Job actions to exclude from the result set.</param>
        /// <returns></returns>
        GetJobsDocumentsResult GetJobsDocuments(int pageIndex, int pageSize, JobActions actions = JobActions.None, JobActions excludedActions = JobActions.None, string orderBy = null, bool orderAsc = true);

        /// <summary>
        /// Enumerates file system entities those belong to user account
        /// </summary>
        /// <param name="path">Relative path to enumerate entities for.</param>
        /// <returns>Array of file system entity records.</returns>
        ListEntitiesResult GetFileSystemEntities(string path, int pageIndex = 0, int pageSize = -1, string orderBy = null, bool orderAsc = true, string filter = null, FileType[] fileTypes = null);

        /// <summary>
        /// Returns a list of file types a document may be converted to
        /// </summary>
        /// <param name="fileId">File identifier to get file types for.</param>
        /// <returns>Array of file types</returns>
        string[] GetDocumentFormats(decimal fileId);

        /// <summary>
        /// Returns a list of template fields a document contains.
        /// </summary>
        /// <param name="fileId">Previously uploaded file identifier to be processed</param>
        /// <param name="includeGeometry">The flag indicates whether fields location and size will be included into the response.</param>
        /// <returns>Template fields data as TemplateFieldsResult</returns>
        TemplateFieldsResult GetTemplateFields(string fileId, bool includeGeometry = true);

        /// <summary>
        /// Assings a data source to an existing job's document for further processing. Data source should be an xml stream and comply to common xsd schema
        /// </summary>
        /// <param name="jobId">Job identifier to assign to.</param>
        /// <param name="fileId">Doc identifier to assign to.</param>
        /// <param name="dataSource">Data source xml stream to assign.</param>
        /// <returns>Created data source identifier</returns>
        decimal AddJobDocumentDataSource(decimal jobId, decimal fileId, decimal datasourceId);

        /// <summary>
        /// Assings a data source to an existing job's document for further processing. Data source is passed as PlainDataSource type
        /// </summary>
        /// <param name="jobId">Job identifier to assign to.</param>
        /// <param name="fileId">Doc identifier to assign to.</param>
        /// <param name="dataSource">Data source as PlainDataSource to assign.</param>
        /// <returns>Created data source identifier</returns>
        decimal AddJobDocumentDataSource(decimal jobId, decimal fileId, DatasourceField[] fields);

        /// <summary>
        /// Schedules a data source binding job.
        /// </summary>
        /// <param name="fileId">Previously uploaded file identifier to be processed.</param>
        /// <param name="datasourceId">Previously created data source identifier to be bound.</param>
        /// <param name="targetType">A file type of the generated document</param>
        /// <param name="emailResults">Indicates whether binded files should be emailed back to the user</param>
        /// <returns>Newly created job identifier</returns>
        decimal MergeTemplate(string fileId, decimal datasourceId, string targetType = null, bool emailResults = false);

        /// <summary>
        /// Schedules a data source binding job.
        /// </summary>
        /// <param name="fileId">Previously uploaded file identifier to be processed</param>
        /// <param name="fields">An array of data source fields to be bound.</param>
        /// <param name="targetType">A file type of the generated document.</param>
        /// <param name="emailResults">Indicates whether binded files should be emailed back to the user</param>
        /// <returns>Newly created job identifier</returns>
        decimal MergeTemplate(string fileId, DatasourceField[] fields, string targetType = null, bool emailResults = false, string assemblyName = null);

        /// <summary>
        /// Returns a collection of pageCount or all page images for a document, starting from startPageIndex
        /// </summary>
        /// <param name="fileId">File id of document</param>
        /// <param name="pageNumber">The first index of page to be viewed</param>
        /// <param name="pageCount">The total number of pages to be viewed. All pages are viewed if null</param>
        /// <param name="quality">The quality of the thumnails in range from 1 to 100. The default value is 50.</param>
        /// <param name="usePdf">The flag indicates whether the document will be converted to a PDF before generating thumnails.</param>
        /// <returns>Collection of images</returns>
        ViewDocumentResult ViewDocument(string fileId, int pageNumber = 0, int? pageCount = null, int? quality = null, bool usePdf = true);

        /// <summary>
        /// Returns an image Url for a document page
        /// </summary>
        /// <param name="fileId">File id of document</param>
        /// <param name="dimension">The dimension of the page image.</param>
        /// <param name="pageNumber">The index of page to be viewed</param>
        /// <param name="quality">The quality of the image in range from 1 to 100. The default value is 50.</param>
        /// <param name="usePdf">The flag indicates whether the document will be converted to a PDF before generating thumnails.</param>
        /// <returns>Image url</returns>
        string GetImageUrl(string fileId, string dimension, int pageNumber = 0, int? quality = null, bool usePdf = true);


        /// <summary>
        /// Returns a document metadata - e.g. page count (sheet count etc.)
        /// </summary>        
        /// <param name="fileId">File identifier</param>
        /// <returns>GetDocumentInfoResult metadata</returns>
        GetDocumentInfoResult GetDocumentInfo(decimal fileId);

        /// <summary>
        /// Returns a document viewing history
        /// </summary>
        /// <param name="startIndex">The index of first historical entry</param>
        /// <param name="pageSize">The number of historical entries</param>
        /// <returns>DocumentViewsResult metadata</returns>
        DocumentViewsResult GetDocumentViews(int startIndex, int pageSize);

        /// <summary>
        /// Shares a document with users
        /// </summary>
        /// <param name="sharers">Identifiers/emails of users to share document with</param>
        /// <param name="fileId">File identifier</param>
        /// <returns>SharedUsersResult metadata</returns>
        SharedUsersResult ShareDocument(decimal fileId, string[] sharers);

        /// <summary>
        /// Unshares a document
        /// </summary>
        /// <param name="fileId">File identifier</param>
        /// <returns>SharedUsersResult metadata</returns>
        SharedUsersResult UnshareDocument(decimal fileId);

        /// <summary>
        /// Shares a folder with users
        /// </summary>
        /// <param name="sharers">Emails of users to share document with.</param>
        /// <param name="folderId">Folder identifier to share.</param>
        /// <returns>A list of users a folder is shared with.</returns>
        SharedUsersResult ShareFolder(decimal folderId, string[] sharers);

        /// <summary>
        /// Unshares a folder
        /// </summary>
        /// <param name="folderId">Folder identifier to unshare.</param>
        /// <returns>True if the folder is unshared, false otherwise.</returns>
        bool UnshareFolder(decimal folderId);

        /// <summary>
        /// Returns a list of users a folder is shared with.
        /// </summary>
        /// <param name="folderId">Folder identifier.</param>
        /// <returns>An array of users.</returns>
        UserInfo[] GetFolderSharers(decimal folderId);

        /// <summary>
        /// Returns a document access data - e.g. document access mode, owhership and sharing data
        /// </summary>
        /// <param name="fileId">File identifier</param>
        /// <returns>DocumentAccessInfoResult metadata</returns>
        DocumentAccessInfoResult GetDocumentAccessInfo(decimal fileId);

        /// <summary>
        /// Sets a document access mode
        /// </summary>
        /// <param name="fileId">File identifier</param>
        /// <param name="mode">Access mode</param>
        /// <returns>DocumentAccessInfoResult metadata</returns>
        DocumentAccessInfoResult SetDocumentAccessMode(decimal fileId, DocumentAccess mode);

        /// <summary>
        /// Gets details for documents which were shared by the user and/or shared with the user.
        /// </summary>        
        /// <param name="sharesTypes">Shared by or shared with mode</param>
        /// <returns>SharedDocumentsResult metadata</returns>
        SharedDocumentsResult GetSharedDocuments(SharesTypes sharesTypes, int pageIndex = 0, int pageSize = -1, string orderBy = null, bool orderAsc = true);


        /// <summary>
        /// Used to accept/reject invitation
        /// </summary>
        /// <param name="fileId">File to be shared</param>
        /// <param name="status">Required status</param>
        /// <returns>DocumentUserStatus metadata</returns>
        DocumentUserStatusResult SetDocumentUserStatus(decimal fileId, DocumentUserStatus status);


        /// <summary>
        /// Returns questionnaire data.
        /// </summary>
        /// <param name="questionnaireId">Questionnaire identifier.</param>
        /// <returns>A questionnaire data structure.</returns>
        QuestionnaireInfo GetQuestionnaire(decimal questionnaireId);

        /// <summary>
        /// Returns user's questionnaires data.
        /// </summary>
        /// <returns>An array of questionnaire data structures.</returns>
        QuestionnaireInfo[] GetQuestionnaires();

        /// <summary>
        /// Creates a new questionnaire
        /// </summary>
        /// <param name="questionnaire">A structure describing questionnaire.</param>
        /// <returns>A newly created questionnaire identifier.</returns>
        decimal AddQuestionnaire(QuestionnaireInfo questionnaire);

        /// <summary>
        /// Updates an existing questionnaire
        /// </summary>
        /// <param name="questionnaireId">Questionnaire identifier.</param>
        /// <param name="questionnaire">Questionnaire data.</param>
        /// <returns>True if the questionnaire has been updated.</returns>
        bool UpdateQuestionnaire(decimal questionnaireId, QuestionnaireInfo questionnaire);

        /// <summary>
        /// Creates a new questionnaire and attaches it to an existing document.
        /// </summary>
        /// <param name="documentId">Document identifier to attach a questionnaire to.</param>
        /// <param name="questionnaire">Questionnaire data.</param>
        /// <returns>A newly created questionnaire identifier.</returns>
        decimal AddDocumentQuestionnaire(string documentId, QuestionnaireInfo questionnaire);

        /// <summary>
        /// Attaches an existing questionnaire to a document
        /// </summary>
        /// <param name="documentId">Document identifier to attach a questionnaire to.</param>
        /// <param name="questionnaireId">Questionnaire identifier to attach.</param>
        /// <returns></returns>
        bool AddDocumentQuestionnaire(string documentId, decimal questionnaireId);

        /// <summary>
        /// Returns a list of questionnaires assigned to a given document.
        /// </summary>
        /// <param name="documentId">Document identifier to returns questionnaires for.</param>
        /// <returns>A list of questionnaires assigned to a document.</returns>
        QuestionnaireInfo[] GetDocumentQuestionnaires(string documentId);

        /// <summary>
        /// Creates a new data source to be used by document assembly operations.
        /// </summary>
        /// <param name="datasource">A data source to create.</param>
        /// <returns>A newly created data source identifier or -1 if an error occurred.</returns>
        decimal AddDataSource(Datasource datasource);

        /// <summary>
        /// Returns existing data source fields.
        /// </summary>
        /// <param name="datasourceId">Data source identifier.</param>
        /// <param name="fields">Field names to return.</param>
        /// <returns>Data source object.</returns>
        Datasource GetDatasource(decimal datasourceId, params string[] fields);

        /// <summary>
        /// Returns a list of data sources those were created using a specified questionnaire.
        /// </summary>
        /// <param name="questionnaireId">Questionnaire identifier used to create data sources.</param>
        /// <param name="includeFields">A flag indicating whether data source fields will be included into the response.</param>
        /// <returns>An array of data sources created using a questionnaire.</returns>
        Datasource[] GetQuestionnaireDatasources(decimal questionnaireId, bool includeFields = false);

        /// <summary>
        /// Updates specified data source fields; other fields remains unchanged.
        /// </summary>
        /// <param name="datasourceId">Data source identifier to update fields for.</param>
        /// <param name="datasource">Data source object.</param>
        /// <returns>True if the data source fields are update, false otherwise.</returns>
        bool UpdateDatasourceFields(decimal datasourceId, Datasource datasource);


        /// <summary>
        /// Submits a new questionnaire execution to a user.
        /// </summary>
        /// <param name="questionnaireId">Questionnaire identifier to be executed.</param>
        /// <param name="executiveEmail">User email address to submit execution to.</param>
        /// <param name="approverEmail">User email address to make a review of the exectuion.</param>
        /// <returns></returns>
        decimal AddQuestionnaireExecution(decimal questionnaireId, string executiveEmail, string approverEmail = null);


        /// <summary>
        /// Returns a list of user's questionnaire executions.
        /// </summary>
        /// <returns>An array of QuestionnaireExecutionInfo objects containing exections details.</returns>
        QuestionnaireExecutionInfo[] GetQuestionnaireExecutions();

        /// <summary>
        /// Updates existing questionnaire execution fields.
        /// </summary>
        /// <param name="executionId">Questionnaire execution identifier to update.</param>
        /// <param name="execution">Questionnaire execution structure.</param>
        /// <returns>True if the questionnaire execution is updated, false otherwise.</returns>
        bool UpdateQuestionnaireExecution(decimal executionId, QuestionnaireExecutionInfo execution);

        /// <summary>
        /// Updates an existing questionnaire execution status.
        /// </summary>
        /// <param name="executionId">Questionnaire execution identifier to update.</param>
        /// <param name="status">A new questionnaire execution status.</param>
        /// <returns>True if the questionnaire execution status is updated, false otherwise.</returns>
        bool UpdateQuestionnaireExecutionStatus(decimal executionId, QuestionnaireExecutionStatus status);

        /// <summary>
        /// User identifier consuming the service
        /// </summary>
        string UserId { get; set; }

        /// <summary>
        /// User private key to access the service
        /// </summary>
        string PrivateKey { get; set; }
    }
}
