using System.Collections.Specialized;
using System.IO;
using Groupdocs.Api.Comparison.Contract;
using Groupdocs.Api.Contract;
using Groupdocs.Api.Contract.UriTemplates;
using Groupdocs.Auxiliary;

namespace Groupdocs.Sdk
{
    public partial class GroupdocsService
    {
        public CompareResponse Compare(string sourceFileId, string targetFileId)
        {
            Throw.IfNull(sourceFileId, "sourceFileId");
            Throw.IfNull(targetFileId, "targetFileId");

            var template = ComparisonApiUriTemplates.BuildUriTemplate(ComparisonApiUriTemplates.Compare);
            var parameters = new NameValueCollection
                                 {
                                     {"userId", UserId},
                                     {"sourceFileId", sourceFileId},
                                     {"targetFileId", targetFileId}
                                 };

            var response = SubmitRequest<CompareResponse>(template, parameters);
            return response;
        }

        public ChangesResponse GetChanges(string resultFileId)
        {
            Throw.IfNull(resultFileId, "resultFileId");

            var template = ComparisonApiUriTemplates.BuildUriTemplate(ComparisonApiUriTemplates.GetChanges);
            var parameters = new NameValueCollection
                                 {
                                     {"userId", UserId},
                                     {"resultFileId", resultFileId}
                                 };

            var response = SubmitRequest<ChangesResponse>(template, parameters);
            return response;
        }

        public DocumentDetailsResponse GetDocumentDetails(string guid)
        {
            Throw.IfNull(guid, "guid");

            var template = ComparisonApiUriTemplates.BuildUriTemplate(ComparisonApiUriTemplates.GetDocumentDetails);
            var parameters = new NameValueCollection
                                 {
                                     {"userId", UserId},
                                     {"guid", guid}
                                 };

            var response = SubmitRequest<DocumentDetailsResponse>(template, parameters);

            return response;
        }


    }
}
