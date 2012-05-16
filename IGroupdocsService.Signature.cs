using System.IO;
using Groupdocs.Api.Contract;

namespace Groupdocs.Sdk
{
    public partial interface IGroupdocsService
    {
        /// <summary>
        /// Returns a list of all envelopes documents.
        /// </summary>
        SignatureEnvelopesResponse GetEnvelopes(string statusId, string page, string count, string documentId, string recipientEmail, string date);

        SignatureFieldsResponse GetFieldsList(string fieldId);
        SignatureFieldResponse AddSignatureField(SignatureFieldSettings settings);
        SignatureFieldResponse UpdateSignatureField(string fieldId, SignatureFieldSettings settings);
        SignatureStatusResponse DeleteSignatureField(string fieldId);

        SignatureEnvelopesResponse GetRecipientEnvelopes(string statusId);
        SignatureEnvelopeResponse CreateEnvelope(string templateId, string envelopeId, string name);
        SignatureEnvelopeResponse SendEnvelope(string envelopeId);
        SignatureStatusResponse SignEnvelope(string envelopeId, string recipientId);
        SignatureEnvelopeResponse ModifyEnvelope(string envelopeId, SignatureEnvelopeSettings settings);
        SignatureEnvelopeResponse RenameEnvelope(string envelopeId, string name);
        SignatureEnvelopeResponse GetEnvelope(string envelopeId);
        SignatureEnvelopeRecipientsResponse GetEnvelopeRecipients(string envelopeId);

        SignatureEnvelopeRecipientResponse AddEnvelopeRecipient(string envelopeId, string recipientEmail,
            string recipientFirstName, string recipientLastName, string roleId, decimal order);

        SignatureStatusResponse DeleteEnvelopeRecipient(string envelopeId, string recipientId);

        SignatureEnvelopeDocumentResponse AddEnvelopeDocument(string envelopeId, string documentId,
            decimal order);

        SignatureStatusResponse DeleteEnvelopeDocument(string envelopeId, string documentId);
        SignatureEnvelopeDocumentsResponse GetEnvelopeDocuments(string envelopeId);

        SignatureEnvelopeFieldResponse AddEnvelopeField(string envelopeId, string documentId, string recipientId, string fieldId, SignatureEnvelopeFieldSettings fieldSettings);
        SignatureStatusResponse DeleteEnvelopeField(string envelopeId, string fieldId);
        SignatureEnvelopeFieldsResponse GetEnvelopeFields(string envelopeId, string documentId, string recipientId);
        SignatureEnvelopeFieldResponse FillEnvelopeField(string envelopeId, string documentId, string recipientId, string fieldId, string data, string dataFrom);

        SignatureEnvelopeFieldResponse ModifyEnvelopeFieldLocation(string envelopeId, string documentId, string recipientId, string fieldId, string locationId, SignatureEnvelopeFieldLocationSettings settings);
        SignatureStatusResponse DeleteEnvelopeFieldLocation(string envelopeId, string fieldId, string locationId);

        ViewDocumentResponse ViewEnvelopeDocument(string fileId, string envelopeId);
        ViewDocumentResponse ViewTemplateDocument(string fileId, string templateId);
        Stream GetSignedDocuments(string envelopeId);

        SignatureTemplatesResponse GetTemplates(string page, string count);
        SignatureTemplateResponse CreateTemplate(string templateId, string envelopeId, string name);
        SignatureTemplateResponse GetTemplate(string templateId);
        SignatureTemplateResponse RenameTemplate(string templateId, string name);
        SignatureTemplateResponse ModifyTemplate(string templateId, SignatureTemplateSettings settings);
        SignatureTemplateRecipientsResponse GetTemplateRecipients(string templateId);
        SignatureTemplateRecipientResponse AddTemplateRecipient(string templateId, string nickname, string roleId, decimal order);
        SignatureStatusResponse DeleteTemplateRecipient(string templateId, string recipientId);

        SignatureTemplateDocumentResponse AddTemplateDocument(string templateId, string documentId, decimal order);

        SignatureStatusResponse DeleteTemplateDocument(string templateId, string documentId);
        SignatureTemplateDocumentsResponse GetTemplateDocuments(string templateId);

        SignatureTemplateFieldResponse AddSignatureTemplateField(string templateId, string documentId, string recipientId, string fieldId, SignatureTemplateFieldSettings fieldSettings);
        SignatureStatusResponse DeleteSignatureTemplateField(string templateId, string fieldId);
        SignatureTemplateFieldsResponse GetSignatureTemplateFields(string templateId, string documentId, string recipientId);
        
        SignatureTemplateFieldResponse ModifySignatureTemplateFieldLocation(string templateId, string documentId, string recipientId, string fieldId, string locationId, SignatureTemplateFieldLocationSettings settings);
        SignatureStatusResponse DeleteSignatureTemplateFieldLocation(string templateId, string fieldId, string locationId);
    }
}
