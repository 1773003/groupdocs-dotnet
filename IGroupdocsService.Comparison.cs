using Groupdocs.Api.Comparison.Contract;

namespace Groupdocs.Sdk
{
    public partial interface IGroupdocsService
    {
        CompareResponse Compare(string sourceFileId, string targetFileId);

        ChangesResponse GetChanges(string resultFileId);

        DocumentDetailsResponse GetDocumentDetails(string guid);
    }
}