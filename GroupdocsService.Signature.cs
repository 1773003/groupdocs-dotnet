using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using Groupdocs.Api.Contract;
using Groupdocs.Signature.Api;

using Microsoft.Http;
using Groupdocs.Security;

namespace Groupdocs.Sdk
{
    public partial class GroupdocsService
    {
        /// <summary>
        /// Returns a list of all envelopes documents.
        /// </summary>
        public SignatureEnvelopesResponse GetEnvelopes(string statusId, string page, string count, string documentId, string recipientEmail, string date)
        {
            var template = SignatureApiUriTemplates.BuildUriTemplate(SignatureApiUriTemplates.GetEnvelopes);
            var parameters = new NameValueCollection
            {
                { "userId", UserId },
                { "statusId", statusId},
                { "page", page},
                { "count", count},
                { "documentId", documentId },
                { "recipientEmail", recipientEmail },
                { "date", date}
            };

            var response = SubmitRequest<SignatureEnvelopesResponse>(template, parameters);
            return response;
        }

        public SignatureFieldsResponse GetFieldsList(string fieldId)
        {
            var template = SignatureApiUriTemplates.BuildUriTemplate(SignatureApiUriTemplates.GetFields);

            var parameters = new NameValueCollection
                                 {
                { "userId", UserId },
                { "fieldId", fieldId}
            };

            var response = SubmitRequest<SignatureFieldsResponse>(template, parameters);

            return response;
        }

        public SignatureFieldResponse AddSignatureField(SignatureFieldSettings settings)
        {
            var template = SignatureApiUriTemplates.BuildUriTemplate(SignatureApiUriTemplates.CreateField);

            var parameters = new NameValueCollection
                                 {
                { "userId", UserId }
            };

            using (var content = HttpContentNetExtensions.CreateJsonNetDataContract(settings))
            {
                var response = SubmitRequest<SignatureFieldResponse>(template, parameters, "POST", content);
                return response;
            }
        }

        public SignatureFieldResponse UpdateSignatureField(string fieldId, SignatureFieldSettings settings)
        {
            var template = SignatureApiUriTemplates.BuildUriTemplate(SignatureApiUriTemplates.Field);

            var parameters = new NameValueCollection
                                 {
                { "userId", UserId },
                { "fieldId", fieldId}
            };

            using (var content = HttpContentNetExtensions.CreateJsonNetDataContract(settings))
            {
                var response = SubmitRequest<SignatureFieldResponse>(template, parameters, "POST", content);
                return response;
            }
        }

        public SignatureStatusResponse DeleteSignatureField(string fieldId)
        {
            var template = SignatureApiUriTemplates.BuildUriTemplate(SignatureApiUriTemplates.Field);

            var parameters = new NameValueCollection
                                 {
                { "userId", UserId },
                { "fieldId", fieldId }
            };

            var response = SubmitRequest<SignatureStatusResponse>(template, parameters, "DELETE", HttpContent.CreateEmpty());

            return response;
        }

        public SignatureEnvelopesResponse GetRecipientEnvelopes(string statusId)
        {
            var template = SignatureApiUriTemplates.BuildUriTemplate(SignatureApiUriTemplates.GetRecipientEnvelopes);
            var parameters = new NameValueCollection
            {
                { "userId", UserId },
                { "statusId", statusId}
            };

            var response = SubmitRequest<SignatureEnvelopesResponse>(template, parameters);
            return response;
        }

        public SignatureEnvelopeResponse CreateEnvelope(string templateId, string envelopeId, string name)
        {
            var template = SignatureApiUriTemplates.BuildUriTemplate(SignatureApiUriTemplates.CreateEnvelope);
            var parameters = new NameValueCollection
                                 {
                                     {"userId", UserId},
                                     {"name", name},
                                     {"templateId", templateId},
                                     {"envelopeId", envelopeId}
                                 };

            var response = SubmitRequest<SignatureEnvelopeResponse>(template, parameters, "POST", HttpContent.CreateEmpty());
            return response;
        }

        public SignatureEnvelopeResponse SendEnvelope(string envelopeId)
        {
            var template = SignatureApiUriTemplates.BuildUriTemplate(SignatureApiUriTemplates.SendEnvelope);
            var parameters = new NameValueCollection
            {
                { "userId", UserId },
                { "envelopeId", envelopeId }
            };
            
            var response = SubmitRequest<SignatureEnvelopeResponse>(template, parameters, "PUT", HttpContent.CreateEmpty());
            return response;
        }

        public SignatureStatusResponse SignEnvelope(string envelopeId, string recipientId)
        {
            var template = SignatureApiUriTemplates.BuildUriTemplate(SignatureApiUriTemplates.SignEnvelope);
            var parameters = new NameValueCollection
            {
                { "userId", UserId },
                { "envelopeId", envelopeId },
                { "recipientId", recipientId }
            };
            
            var response = SubmitRequest<SignatureStatusResponse>(template, parameters, "PUT", HttpContent.CreateEmpty());
            return response;
        }

        public SignatureEnvelopeResponse ModifyEnvelope(string envelopeId, SignatureEnvelopeSettings settings)
        {
            var template = SignatureApiUriTemplates.BuildUriTemplate(SignatureApiUriTemplates.Envelope);
            var parameters = new NameValueCollection
            {
                { "userId", UserId },
                { "envelopeId", envelopeId }
            };

            using (var content = HttpContentNetExtensions.CreateJsonNetDataContract(settings))
            {
                var response = SubmitRequest<SignatureEnvelopeResponse>(template, parameters, "POST", content);
                return response;
            }

        }


        public SignatureEnvelopeResponse RenameEnvelope(string envelopeId, string name)
        {
            var template = SignatureApiUriTemplates.BuildUriTemplate(SignatureApiUriTemplates.RenameEnvelope);
            var parameters = new NameValueCollection
            {
                { "userId", UserId },
                { "envelopeId", envelopeId },
                { "name", name }
            };

            var response = SubmitRequest<SignatureEnvelopeResponse>(template, parameters, "PUT", HttpContent.CreateEmpty());
            return response;
        }

        public SignatureEnvelopeResponse GetEnvelope(string envelopeId)
        {
            var template = SignatureApiUriTemplates.BuildUriTemplate(SignatureApiUriTemplates.GetEnvelope);
            var parameters = new NameValueCollection
            {
                { "userId", UserId },
                { "envelopeId", envelopeId }
            };

            var response = SubmitRequest<SignatureEnvelopeResponse>(template, parameters);
            return response;
        }


        public SignatureEnvelopeRecipientsResponse GetEnvelopeRecipients(string envelopeId)
        {
            var template = SignatureApiUriTemplates.BuildUriTemplate(SignatureApiUriTemplates.GetEnvelopeRecipients);
            var parameters = new NameValueCollection
            {
                { "userId", UserId },
                { "envelopeId", envelopeId }
            };

            var response = SubmitRequest<SignatureEnvelopeRecipientsResponse>(template, parameters);
            return response;
        }

        public SignatureEnvelopeRecipientResponse AddEnvelopeRecipient(string envelopeId, string recipientEmail, string recipientFirstName, string recipientLastName, string roleId, decimal order)
        {
            var template = SignatureApiUriTemplates.BuildUriTemplate(SignatureApiUriTemplates.AddEnvelopeRecipient);
            var parameters = new NameValueCollection
            {
                { "userId", UserId },
                { "envelopeId", envelopeId },
                { "recipientEmail", recipientEmail },
                { "recipientFirstName", recipientFirstName },
                { "recipientLastName", recipientLastName },
                { "roleId", roleId },
                { "order", order.ToString(CultureInfo.InvariantCulture) },
            };

            var response = SubmitRequest<SignatureEnvelopeRecipientResponse>(template, parameters, "POST", HttpContent.CreateEmpty());
            return response;
        }

        public SignatureEnvelopeRecipientResponse ModifyEnvelopeRecipient(string envelopeId, string recipientId, string recipientEmail, string recipientFirstName, string recipientLastName, string roleId, decimal order)
        {
            var template = SignatureApiUriTemplates.BuildUriTemplate(SignatureApiUriTemplates.ModifyEnvelopeRecipient);
            var parameters = new NameValueCollection
            {
                { "userId", UserId },
                { "envelopeId", envelopeId },
                { "recipientId", recipientId },
                { "recipientEmail", recipientEmail },
                { "recipientFirstName", recipientFirstName },
                { "recipientLastName", recipientLastName },
                { "roleId", roleId },
                { "order", order.ToString(CultureInfo.InvariantCulture) },
            };

            var response = SubmitRequest<SignatureEnvelopeRecipientResponse>(template, parameters, "PUT", HttpContent.CreateEmpty());
            return response;
        }

        public SignatureStatusResponse DeleteEnvelopeRecipient(string envelopeId, string recipientId)
        {
            var template = SignatureApiUriTemplates.BuildUriTemplate(SignatureApiUriTemplates.EnvelopeRecipient);
            var parameters = new NameValueCollection
            {
                { "userId", UserId },
                { "envelopeId", envelopeId },
                { "recipientId", recipientId },
            };

            var response = SubmitRequest<SignatureStatusResponse>(template, parameters, "DELETE", HttpContent.CreateEmpty());
            return response;
        }


        public SignatureEnvelopeDocumentResponse AddEnvelopeDocument(string envelopeId, string documentId, decimal order)
        {
            var template = SignatureApiUriTemplates.BuildUriTemplate(SignatureApiUriTemplates.AddEnvelopeDocument);
            var parameters = new NameValueCollection
            {
                { "userId", UserId },
                { "envelopeId", envelopeId },
                { "documentId", documentId },
                { "order", order.ToString(CultureInfo.InvariantCulture) },
            };

            var response = SubmitRequest<SignatureEnvelopeDocumentResponse>(template, parameters, "POST", HttpContent.CreateEmpty());
            return response;
        }

        public SignatureStatusResponse DeleteEnvelopeDocument(string envelopeId, string documentId)
        {
            var template = SignatureApiUriTemplates.BuildUriTemplate(SignatureApiUriTemplates.EnvelopeDocument);
            var parameters = new NameValueCollection
            {
                { "userId", UserId },
                { "envelopeId", envelopeId },
                { "documentId", documentId },
            };

            var response = SubmitRequest<SignatureStatusResponse>(template, parameters, "DELETE", HttpContent.CreateEmpty());
            return response;
        }

        public SignatureEnvelopeDocumentsResponse GetEnvelopeDocuments(string envelopeId)
        {
            var template = SignatureApiUriTemplates.BuildUriTemplate(SignatureApiUriTemplates.GetEnvelopeDocuments);
            var parameters = new NameValueCollection
            {
                { "userId", UserId },
                { "envelopeId", envelopeId }
            };

            var response = SubmitRequest<SignatureEnvelopeDocumentsResponse>(template, parameters);
            return response;
        }

        public SignatureEnvelopeDocumentResponse GetEnvelopeDocument(string envelopeId, string documentId)
        {
            var template = SignatureApiUriTemplates.BuildUriTemplate(SignatureApiUriTemplates.EnvelopeDocument);
            var parameters = new NameValueCollection
            {
                { "userId", UserId },
                { "envelopeId", envelopeId },
                { "documentId", documentId },                
            };

            var response = SubmitRequest<SignatureEnvelopeDocumentResponse>(template, parameters);
            return response;
        }


        public SignatureEnvelopeFieldResponse AddEnvelopeField(string envelopeId, string documentId, string recipientId, string fieldId, SignatureEnvelopeFieldSettings fieldSettings)
        {
            var template = SignatureApiUriTemplates.BuildUriTemplate(SignatureApiUriTemplates.AddEnvelopeField);
            var parameters = new NameValueCollection
                                 {
                                     {"userId", UserId},
                                     {"envelopeId", envelopeId},
                                     {"documentId", documentId},
                                     {"recipientId", recipientId},
                                     {"fieldId", fieldId}
                                 };
            var content = HttpContentNetExtensions.CreateJsonNetDataContract(fieldSettings);
            var response = SubmitRequest<SignatureEnvelopeFieldResponse>(template, parameters, "POST", content);
            return response;
        }

        public SignatureEnvelopeFieldResponse UpdateEnvelopeField(string envelopeId, string documentId, string fieldId, SignatureEnvelopeFieldSettings fieldSettings)
        {
            var template = SignatureApiUriTemplates.BuildUriTemplate(SignatureApiUriTemplates.ModifyEnvelopeField);
            var parameters = new NameValueCollection
                                 {
                                     {"userId", UserId},
                                     {"envelopeId", envelopeId},
                                     {"documentId", documentId},
                                     {"fieldId", fieldId}
                                 };
            var content = HttpContentNetExtensions.CreateJsonNetDataContract(fieldSettings);
            var response = SubmitRequest<SignatureEnvelopeFieldResponse>(template, parameters, "PUT", content);
            return response;
        }

        public SignatureStatusResponse DeleteEnvelopeField(string envelopeId, string fieldId)
        {
            var template = SignatureApiUriTemplates.BuildUriTemplate(SignatureApiUriTemplates.EnvelopeField);
            var parameters = new NameValueCollection
                                 {
                                     {"userId", UserId},
                                     {"envelopeId", envelopeId},
                                     {"fieldId", fieldId}
                                 };
            var response = SubmitRequest<SignatureStatusResponse>(template, parameters, "DELETE", HttpContent.CreateEmpty());
            return response;
        }

        public SignatureEnvelopeFieldsResponse GetEnvelopeFields(string envelopeId, string documentId, string recipientId)
        {
            var template = SignatureApiUriTemplates.BuildUriTemplate(SignatureApiUriTemplates.GetEnvelopeFields);
            var parameters = new NameValueCollection
                                 {
                                     {"userId", UserId},
                                     {"envelopeId", envelopeId},
                                     {"documentId", documentId },
                                     {"recipientId", recipientId },
                                 };
            var response = SubmitRequest<SignatureEnvelopeFieldsResponse>(template, parameters);
            return response;
        }

        public SignatureEnvelopeFieldResponse FillEnvelopeField(string envelopeId, string documentId, string recipientId, string fieldId, string data, string signatureId)
        {
            var template = SignatureApiUriTemplates.BuildUriTemplate(SignatureApiUriTemplates.FillEnvelopeField);
            var parameters = new NameValueCollection
                                 {
                                     {"userId", UserId},
                                     {"envelopeId", envelopeId},
                                     {"documentId", documentId },
                                     {"recipientId", recipientId },
                                     {"fieldId", fieldId},
                                     {"signatureId", signatureId}
                                 };


            var response = SubmitRequest<SignatureEnvelopeFieldResponse>(template, parameters, "PUT", HttpContent.Create(data ?? String.Empty));
            return response;
        }

        public SignatureEnvelopeFieldResponse ModifyEnvelopeFieldLocation(string envelopeId, string documentId, string recipientId, string fieldId, string locationId, SignatureEnvelopeFieldLocationSettings settings)
        {
            var template = SignatureApiUriTemplates.BuildUriTemplate(SignatureApiUriTemplates.EnvelopeFieldLocationModify);
            var parameters = new NameValueCollection
                                 {
                                     {"userId", UserId},
                                     {"envelopeId", envelopeId},
                                     {"documentId", documentId},
                                     {"recipientId", recipientId},
                                     {"fieldId", fieldId},
                                     {"locationId", locationId}
                                 };
            var content = HttpContentNetExtensions.CreateJsonNetDataContract(settings);
            var response = SubmitRequest<SignatureEnvelopeFieldResponse>(template, parameters, "PUT", content);
            return response;
        }

        public SignatureStatusResponse DeleteEnvelopeFieldLocation(string envelopeId, string fieldId, string locationId)
        {
            var template = SignatureApiUriTemplates.BuildUriTemplate(SignatureApiUriTemplates.EnvelopeFieldLocation);
            var parameters = new NameValueCollection
                                 {
                                     {"envelopeId", envelopeId},
                                     {"fieldId", fieldId},
                                     {"locationId", locationId},
                                     {"userId", UserId}
                                 };
            var response = SubmitRequest<SignatureStatusResponse>(template, parameters, "DELETE", HttpContent.CreateEmpty());
            return response;
        }


        public ViewDocumentResponse ViewEnvelopeDocument(string fileId, string envelopeId)
        {
            var template = SignatureApiUriTemplates.BuildUriTemplate(SignatureApiUriTemplates.SignatureEnvelopeDocumentThumbnails);
            var parameters = new NameValueCollection
                                 {
                                     {"userId", UserId},
                                     {"fileId", fileId},
                                     {"envelopeId", envelopeId}
                                 };

            var response = SubmitRequest<ViewDocumentResponse>(template, parameters, "POST", HttpContent.CreateEmpty());
            return response;
        }

        public ViewDocumentResponse ViewTemplateDocument(string fileId, string templateId)
        {
            var template = SignatureApiUriTemplates.BuildUriTemplate(SignatureApiUriTemplates.SignatureTemplateDocumentThumbnails);
            var parameters = new NameValueCollection
                                 {
                                     {"userId", UserId},
                                     {"fileId", fileId},
                                     {"templateId", templateId}
                                 };

            var response = SubmitRequest<ViewDocumentResponse>(template, parameters, "POST", HttpContent.CreateEmpty());
            return response;
        }

        public Stream GetSignedDocuments(string envelopeId)
        {
            var uriTemplate = SignatureApiUriTemplates.BuildUriTemplate(SignatureApiUriTemplates.GetSignedEnvelopeDocuments);
            var parameters = new NameValueCollection
                                 {
                                     {"userId", UserId},
                                     {"envelopeId", envelopeId}
                                 };

            var template = new UriTemplate(uriTemplate);

            Uri url = template.BindByName(new Uri(BaseAddress), parameters);
            string signedUrl = UrlSignature.Sign(url.AbsoluteUri, PrivateKey);

            var request = new HttpRequestMessage { Method = "GET", Uri = new Uri(signedUrl) };

            HttpContent content = request.Content;
            _client.DefaultHeaders.ContentLength = (content != null && content.HasLength() ? new long?(content.GetLength()) : null);

            OnSendingRequest(request);

            return _client.Send(request).Content.ReadAsStream();
        }









        public SignatureTemplatesResponse GetTemplates(string page, string count)
        {
            var template = SignatureApiUriTemplates.BuildUriTemplate(SignatureApiUriTemplates.GetTemplates);
            var parameters = new NameValueCollection
            {
                { "userId", UserId },
                { "page", page},
                { "count", count}
            };

            var response = SubmitRequest<SignatureTemplatesResponse>(template, parameters);
            return response;
        }

        public SignatureTemplateResponse GetTemplate(string templateId)
        {
            var template = SignatureApiUriTemplates.BuildUriTemplate(SignatureApiUriTemplates.GetTemplate);
            var parameters = new NameValueCollection
            {
                { "userId", UserId },
                { "templateId", templateId }
            };

            var response = SubmitRequest<SignatureTemplateResponse>(template, parameters);
            return response;
        }



        public SignatureTemplateResponse CreateTemplate(string templateId, string envelopeId, string name)
        {
            var template = SignatureApiUriTemplates.BuildUriTemplate(SignatureApiUriTemplates.CreateTemplate);
            var parameters = new NameValueCollection
                                 {
                                     {"userId", UserId},
                                     {"name", name},
                                     {"templateId", templateId},
                                     {"envelopeId", envelopeId}
                                 };

            var response = SubmitRequest<SignatureTemplateResponse>(template, parameters, "POST", HttpContent.CreateEmpty());
            return response;
        }

        public SignatureTemplateResponse RenameTemplate(string templateId, string name)
        {
            var template = SignatureApiUriTemplates.BuildUriTemplate(SignatureApiUriTemplates.RenameTemplate);
            var parameters = new NameValueCollection
            {
                { "userId", UserId },
                { "templateId", templateId },
                { "name", name }
            };

            var response = SubmitRequest<SignatureTemplateResponse>(template, parameters, "PUT", HttpContent.CreateEmpty());
            return response;
        }

        public SignatureTemplateResponse ModifyTemplate(string templateId, SignatureTemplateSettings settings)
        {
            var template = SignatureApiUriTemplates.BuildUriTemplate(SignatureApiUriTemplates.Template);
            var parameters = new NameValueCollection
            {
                { "userId", UserId },
                { "templateId", templateId }
            };

            using (var content = HttpContentNetExtensions.CreateJsonNetDataContract(settings))
            {
                var response = SubmitRequest<SignatureTemplateResponse>(template, parameters, "POST", content);
                return response;
            }

        }

        public SignatureTemplateRecipientsResponse GetTemplateRecipients(string templateId)
        {
            var template = SignatureApiUriTemplates.BuildUriTemplate(SignatureApiUriTemplates.GetTemplateRecipients);
            var parameters = new NameValueCollection
            {
                { "userId", UserId },
                { "templateId", templateId }
            };

            var response = SubmitRequest<SignatureTemplateRecipientsResponse>(template, parameters);
            return response;
        }

        public SignatureTemplateRecipientResponse AddTemplateRecipient(string templateId, string nickname, string roleId, decimal order)
        {
            var template = SignatureApiUriTemplates.BuildUriTemplate(SignatureApiUriTemplates.AddTemplateRecipient);
            var parameters = new NameValueCollection
            {
                { "userId", UserId },
                { "templateId", templateId },
                { "nickname", nickname },
                { "roleId", roleId },
                { "order", order.ToString(CultureInfo.InvariantCulture) },
            };

            var response = SubmitRequest<SignatureTemplateRecipientResponse>(template, parameters, "POST", HttpContent.CreateEmpty());
            return response;
        }

        public SignatureTemplateRecipientResponse UpdateTemplateRecipient(string templateId, string recipientId, string nickname, string roleId, decimal order)
        {
            var template = SignatureApiUriTemplates.BuildUriTemplate(SignatureApiUriTemplates.ModifyTemplateRecipient);
            var parameters = new NameValueCollection
            {
                { "userId", UserId },
                { "templateId", templateId },
                { "recipientId", recipientId },
                { "nickname", nickname },
                { "roleId", roleId },
                { "order", order.ToString(CultureInfo.InvariantCulture) },
            };

            var response = SubmitRequest<SignatureTemplateRecipientResponse>(template, parameters, "PUT", HttpContent.CreateEmpty());
            return response;
        }

        public SignatureStatusResponse DeleteTemplateRecipient(string templateId, string recipientId)
        {
            var template = SignatureApiUriTemplates.BuildUriTemplate(SignatureApiUriTemplates.TemplateRecipient);
            var parameters = new NameValueCollection
            {
                { "userId", UserId },
                { "templateId", templateId },
                { "recipientId", recipientId },
            };

            var response = SubmitRequest<SignatureStatusResponse>(template, parameters, "DELETE", HttpContent.CreateEmpty());
            return response;
        }


        public SignatureTemplateDocumentResponse AddTemplateDocument(string templateId, string documentId, decimal order)
        {
            var template = SignatureApiUriTemplates.BuildUriTemplate(SignatureApiUriTemplates.AddTemplateDocument);
            var parameters = new NameValueCollection
            {
                { "userId", UserId },
                { "templateId", templateId },
                { "documentId", documentId },
                { "order", order.ToString(CultureInfo.InvariantCulture) },
            };

            var response = SubmitRequest<SignatureTemplateDocumentResponse>(template, parameters, "POST", HttpContent.CreateEmpty());
            return response;
        }

        public SignatureStatusResponse DeleteTemplateDocument(string templateId, string documentId)
        {
            var template = SignatureApiUriTemplates.BuildUriTemplate(SignatureApiUriTemplates.TemplateDocument);
            var parameters = new NameValueCollection
            {
                { "userId", UserId },
                { "templateId", templateId },
                { "documentId", documentId },
            };

            var response = SubmitRequest<SignatureStatusResponse>(template, parameters, "DELETE", HttpContent.CreateEmpty());
            return response;
        }

        public SignatureTemplateDocumentsResponse GetTemplateDocuments(string templateId)
        {
            var template = SignatureApiUriTemplates.BuildUriTemplate(SignatureApiUriTemplates.GetTemplateDocuments);
            var parameters = new NameValueCollection
            {
                { "userId", UserId },
                { "templateId", templateId }
            };

            var response = SubmitRequest<SignatureTemplateDocumentsResponse>(template, parameters);
            return response;
        }

        public SignatureTemplateFieldResponse AddSignatureTemplateField(string templateId, string documentId, string recipientId, string fieldId, SignatureTemplateFieldSettings fieldSettings)
        {
            var template = SignatureApiUriTemplates.BuildUriTemplate(SignatureApiUriTemplates.AddTemplateField);
            var parameters = new NameValueCollection
                                 {
                                     {"userId", UserId},
                                     {"templateId", templateId},
                                     {"documentId", documentId},
                                     {"recipientId", recipientId},
                                     {"fieldId", fieldId}
                                 };
            var content = HttpContentNetExtensions.CreateJsonNetDataContract(fieldSettings);
            var response = SubmitRequest<SignatureTemplateFieldResponse>(template, parameters, "POST", content);
            return response;
        }

        public SignatureEnvelopeFieldResponse UpdateSignatureTemplateField(string templateId, string documentId, string fieldId, SignatureEnvelopeFieldSettings fieldSettings)
        {
            var template = SignatureApiUriTemplates.BuildUriTemplate(SignatureApiUriTemplates.ModifyTemplateField);
            var parameters = new NameValueCollection
                                 {
                                     {"userId", UserId},
                                     {"templateId", templateId},
                                     {"documentId", documentId},
                                     {"fieldId", fieldId}
                                 };
            var content = HttpContentNetExtensions.CreateJsonNetDataContract(fieldSettings);
            var response = SubmitRequest<SignatureEnvelopeFieldResponse>(template, parameters, "PUT", content);
            return response;
        }

        public SignatureStatusResponse DeleteSignatureTemplateField(string templateId, string fieldId)
        {
            var template = SignatureApiUriTemplates.BuildUriTemplate(SignatureApiUriTemplates.TemplateField);
            var parameters = new NameValueCollection
                                 {
                                     {"userId", UserId},
                                     {"templateId", templateId},
                                     {"fieldId", fieldId}
                                 };
            var response = SubmitRequest<SignatureStatusResponse>(template, parameters, "DELETE", HttpContent.CreateEmpty());
            return response;
        }

        public SignatureTemplateFieldsResponse GetSignatureTemplateFields(string templateId, string documentId, string recipientId)
        {
            var template = SignatureApiUriTemplates.BuildUriTemplate(SignatureApiUriTemplates.GetTemplateFields);
            var parameters = new NameValueCollection
                                 {
                                     {"userId", UserId},
                                     {"templateId", templateId},
                                     {"documentId", documentId },
                                     {"recipientId", recipientId },
                                 };
            var response = SubmitRequest<SignatureTemplateFieldsResponse>(template, parameters);
            return response;
        }

        public SignatureTemplateFieldResponse ModifySignatureTemplateFieldLocation(string templateId, string documentId, string recipientId, string fieldId, string locationId, SignatureTemplateFieldLocationSettings settings)
        {
            var template = SignatureApiUriTemplates.BuildUriTemplate(SignatureApiUriTemplates.TemplateFieldLocationModify);
            var parameters = new NameValueCollection
                                 {
                                     {"userId", UserId},
                                     {"templateId", templateId},
                                     {"documentId", documentId},
                                     {"recipientId", recipientId},
                                     {"fieldId", fieldId},
                                     {"locationId", locationId}
                                 };
            var content = HttpContentNetExtensions.CreateJsonNetDataContract(settings);
            var response = SubmitRequest<SignatureTemplateFieldResponse>(template, parameters, "PUT", content);
            return response;
        }

        public SignatureStatusResponse DeleteSignatureTemplateFieldLocation(string templateId, string fieldId, string locationId)
        {
            var template = SignatureApiUriTemplates.BuildUriTemplate(SignatureApiUriTemplates.TemplateFieldLocation);
            var parameters = new NameValueCollection
                                 {
                                     {"templateId", templateId},
                                     {"fieldId", fieldId},
                                     {"locationId", locationId},
                                     {"userId", UserId}
                                 };
            var response = SubmitRequest<SignatureStatusResponse>(template, parameters, "DELETE", HttpContent.CreateEmpty());
            return response;
        }

        public SignatureContactsResponse GetContacts(string page, string count, string firstName, string lastName, string email)
        {
            var template = SignatureApiUriTemplates.BuildUriTemplate(SignatureApiUriTemplates.GetContacts);
            var parameters = new NameValueCollection
            {
                { "userId", UserId },
                { "page", page},
                { "count", count},
                { "firstName", firstName },
                { "lastName", lastName },
                { "email", email }
            };
            var response = SubmitRequest<SignatureContactsResponse>(template, parameters);
            return response;
        }

        public SignatureContactResponse AddContact(SignatureContactSettings settings)
        {
            var template = SignatureApiUriTemplates.BuildUriTemplate(SignatureApiUriTemplates.AddContact);
            var parameters = new NameValueCollection
            {
                { "userId", UserId }
            };

            using (var content = HttpContentNetExtensions.CreateJsonNetDataContract(settings))
            {
                var response = SubmitRequest<SignatureContactResponse>(template, parameters, "POST", content);
                return response;
            }
        }

        public SignatureContactResponse UpdateContact(string contactId, SignatureContactSettings settings)
        {
            var template = SignatureApiUriTemplates.BuildUriTemplate(SignatureApiUriTemplates.Contact);
            var parameters = new NameValueCollection
            {
                { "userId", UserId },
                { "contactId", contactId }
            };
            using (var content = HttpContentNetExtensions.CreateJsonNetDataContract(settings))
            {
                var response = SubmitRequest<SignatureContactResponse>(template, parameters, "POST", content);
                return response;
            }
        }

        public SignatureContactResponse DeleteContact(string contactId)
        {
            var template = SignatureApiUriTemplates.BuildUriTemplate(SignatureApiUriTemplates.DeleteContact);
            var parameters = new NameValueCollection
            {
                { "userId", UserId },
                { "contactId", contactId }
            };
            var response = SubmitRequest<SignatureContactResponse>(template, parameters, "DELETE", HttpContent.CreateEmpty());
            return response;
        }

        public SignatureContactsImportResponse ImportContacts(SignatureContactSettings[] settings)
        {
            var template = SignatureApiUriTemplates.BuildUriTemplate(SignatureApiUriTemplates.ImportContacts);
            var parameters = new NameValueCollection
            {
                { "userId", UserId }
            };

            using (var content = HttpContentNetExtensions.CreateJsonNetDataContract(settings))
            {
                var response = SubmitRequest<SignatureContactsImportResponse>(template, parameters, "POST", content);
                return response;
            }
        }

        public SignatureSignatureResponse CreateSignature(string envelopeName, SignatureSignatureSettings settings)
        {
            var template = SignatureApiUriTemplates.BuildUriTemplate(SignatureApiUriTemplates.CreateSignature);
            var parameters = new NameValueCollection
            {
                { "userId", UserId },
                {"name", envelopeName}
            };

            using (var content = HttpContentNetExtensions.CreateJsonNetDataContract(settings))
            {
                var response = SubmitRequest<SignatureSignatureResponse>(template, parameters, "POST", content);
                return response;
            }
        }

        public SignatureSignatureResponse CreateSignatureAddimage(string signatureId, string dataFrom, string type)
        {
            var template = SignatureApiUriTemplates.BuildUriTemplate(SignatureApiUriTemplates.AddSignatureImage);
            var parameters = new NameValueCollection
                                 {
                                     {"userId", UserId},
                                     {"signatureId", signatureId},
                                     {"dataFrom", dataFrom},
                                     {"type", type}
                                 };
            var response = SubmitRequest<SignatureSignatureResponse>(template, parameters, "POST", HttpContent.CreateEmpty());
            return response;
        }

        public SignatureSignaturesResponse GetSignatures()
        {
            var template = SignatureApiUriTemplates.BuildUriTemplate(SignatureApiUriTemplates.GetSignatures);
            var parameters = new NameValueCollection
                                 {
                                     {"userId", UserId}
                                 };
            var response = SubmitRequest<SignatureSignaturesResponse>(template, parameters);
            return response;
        }

        public SignatureSignatureResponse ModifySignature(string signatureId, SignatureSignatureSettings settings)
        {
            var template = SignatureApiUriTemplates.BuildUriTemplate(SignatureApiUriTemplates.ModifySignature);
            var parameters = new NameValueCollection
                                 {
                                     {"userId", UserId},
                                     {"signatureId", signatureId}
                                 };
            using (var content = HttpContentNetExtensions.CreateJsonNetDataContract(settings))
            {
                var response = SubmitRequest<SignatureSignatureResponse>(template, parameters, "POST", content);
                return response;
            }
        }

        public SignatureSignatureResponse DeleteSignature(string signatureId)
        {
            var template = SignatureApiUriTemplates.BuildUriTemplate(SignatureApiUriTemplates.DeleteSignature);
            var parameters = new NameValueCollection
            {
                { "userId", UserId },
                { "signatureId", signatureId }
            };
            var response = SubmitRequest<SignatureSignatureResponse>(template, parameters, "DELETE", HttpContent.CreateEmpty());
            return response;
        }

        public SignatureEnvelopeResourcesResponse GetEnvelopeResources()
        {
            var template = SignatureApiUriTemplates.BuildUriTemplate(SignatureApiUriTemplates.GetEnvelopeRecources);
            var parameters = new NameValueCollection
                                 {
                                     {"userId", UserId}
                                 };
            var response = SubmitRequest<SignatureEnvelopeResourcesResponse>(template, parameters);
            return response;
        }

    }
}
