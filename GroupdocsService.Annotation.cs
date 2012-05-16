using System;
using System.Collections.Specialized;
using Groupdocs.Api.Contract.Annotation;
using Groupdocs.Api.Contract.Data;
using Microsoft.Http;

namespace Groupdocs.Sdk
{
    using Groupdocs.Auxiliary;
    using Groupdocs.Common;
    using Groupdocs.Api.Contract;
    using Groupdocs.Api.Contract.UriTemplates;

    public partial class GroupdocsService
    {
        public CreateAnnotationResult CreateAnnotation(string documentGuid, AnnotationType type, Rectangle box,
                                                       Point? annotationPosition,
                                                       Range? textRange = null, 
                                                       string svgPath = null, string message = null)
        {
            Throw.IfNull<string>(documentGuid, "documentGuid");

            var template = AnnotationApiUriTemplates.BuildUriTemplate(AnnotationApiUriTemplates.CreateAnnotation);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "fileId", documentGuid }
            };

            var annotation = new AnnotationInfo { Type = type, Box = box, AnnotationPosition = annotationPosition, TextRange = textRange, SvgPath = svgPath };

            if (!String.IsNullOrWhiteSpace(message))
            {
                annotation.Replies = new AnnotationReplyInfo[] { new AnnotationReplyInfo { Message = message ?? "" } };
            }

            using (var content = HttpContentNetExtensions.CreateJsonNetDataContract<AnnotationInfo>(annotation))
            {
                var response = SubmitRequest<CreateAnnotationResponse>(template, parameters, "POST", content);
                if (response.Code != ResponseCode.Ok)
                {
                    throw new GroupdocsServiceException(response.ErrorMessage);
                }

                return response.Result;
            }
        }

        public ListAnnotationsResult ListAnnotations(string documentGuid)
        {
            Throw.IfNull<string>(documentGuid, "documentGuid");

            var template = AnnotationApiUriTemplates.BuildUriTemplate(AnnotationApiUriTemplates.ListAnnotations);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "fileId", documentGuid }
            };

            var response = SubmitRequest<ListAnnotationsResponse>(template, parameters);
            if (response.Code != ResponseCode.Ok)
            {
                throw new GroupdocsServiceException(response.ErrorMessage);
            }

            return response.Result;
        }

        public DeleteAnnotationResult DeleteAnnotation(string annotationGuid)
        {
            Throw.IfNull<string>(annotationGuid, "annotationGuid");

            var template = AnnotationApiUriTemplates.BuildUriTemplate(AnnotationApiUriTemplates.DeleteAnnotation);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "annotationId", annotationGuid }
            };

            var response = SubmitRequest<DeleteAnnotationResponse>(template, parameters, "DELETE", HttpContent.CreateEmpty());
            if (response.Code != ResponseCode.Ok)
            {
                throw new GroupdocsServiceException(response.ErrorMessage);
            }

            return response.Result;
        }

        public AddReplyResult AddAnnotationReply(string annotationGuid, string message)
        {
            Throw.IfNull<string>(annotationGuid, "annotationGuid");
            Throw.IfNull<string>(message, "message");

            var template = AnnotationApiUriTemplates.BuildUriTemplate(AnnotationApiUriTemplates.CreateReply);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "annotationId", annotationGuid }
            };

            using (var content = HttpContentNetExtensions.CreateJsonNetDataContract<string>(message))
            {
                var response = SubmitRequest<AddReplyResponse>(template, parameters, "POST", content);
                if (response.Code != ResponseCode.Ok)
                {
                    throw new GroupdocsServiceException(response.ErrorMessage);
                }

                return response.Result;
            }
        }

        public EditReplyResult EditAnnotationReply(string replyGuid, string message)
        {
            Throw.IfNull<string>(replyGuid, "replyGuid");
            Throw.IfNull<string>(message, "message");

            var template = AnnotationApiUriTemplates.BuildUriTemplate(AnnotationApiUriTemplates.EditAnnotationReply);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "replyGuid", replyGuid }
            };

            using (var content = HttpContentNetExtensions.CreateJsonNetDataContract<string>(message))
            {
                var response = SubmitRequest<EditReplyResponse>(template, parameters, "PUT", content);
                if (response.Code != ResponseCode.Ok)
                {
                    throw new GroupdocsServiceException(response.ErrorMessage);
                }

                return response.Result;
            }
        }

        public ListRepliesResult ListAnnotationReplies(string annotationGuid, DateTime? after = null)
        {
            Throw.IfNull<string>(annotationGuid, "annotationGuid");

            var template = AnnotationApiUriTemplates.BuildUriTemplate(AnnotationApiUriTemplates.ListReplies);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "annotationId", annotationGuid },
                { "after", (after == null ? "0" : ((long) (after.Value.ToUniversalTime() - DateTimeConstants.Epoch).TotalMilliseconds).ToString()) }
            };

            var response = SubmitRequest<ListRepliesResponse>(template, parameters);
            if (response.Code != ResponseCode.Ok)
            {
                throw new GroupdocsServiceException(response.ErrorMessage);
            }

            return response.Result;
        }

        public SetCollaboratorsResult SetAnnotationCollaborators(string documentGuid, string[] collaborators)
        {
            Throw.IfNull<string>(documentGuid, "documentGuid");
            Throw.IfNull<string[]>(collaborators, "collaborators");

            var template = AnnotationApiUriTemplates.BuildUriTemplate(AnnotationApiUriTemplates.SetCollaborators);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "fileId", documentGuid }
            };

            using (var content = HttpContentNetExtensions.CreateJsonNetDataContract<string[]>(collaborators))
            {
                var response = SubmitRequest<SetCollaboratorsResponse>(template, parameters, "PUT", content);
                if (response.Code != ResponseCode.Ok)
                {
                    throw new GroupdocsServiceException(response.ErrorMessage);
                }

                return response.Result;
            }
        }

        public GetCollaboratorsResult GetAnnotationCollaborators(string documentGuid)
        {
            Throw.IfNull<string>(documentGuid, "documentGuid");

            var template = AnnotationApiUriTemplates.BuildUriTemplate(AnnotationApiUriTemplates.GetCollaborators);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "fileId", documentGuid }
            };

            var response = SubmitRequest<GetCollaboratorsResponse>(template, parameters);
            if (response.Code != ResponseCode.Ok)
            {
                throw new GroupdocsServiceException(response.ErrorMessage);
            }

            return response.Result;
        }


        public MoveAnnotationResult MoveAnnotation(string annotationGuid, Point position)
        {
            Throw.IfNull<string>(annotationGuid, "annotationGuid");

            var template = AnnotationApiUriTemplates.BuildUriTemplate(AnnotationApiUriTemplates.MoveAnnotation);
            var parameters = new NameValueCollection()
            {
                { "userId", UserId },
                { "annotationId", annotationGuid }
            };

            using (var content = HttpContentNetExtensions.CreateJsonNetDataContract<Point>(position))
            {
                var response = SubmitRequest<MoveAnnotationResponse>(template, parameters, "PUT", content);
                if (response.Code != ResponseCode.Ok)
                {
                    throw new GroupdocsServiceException(response.ErrorMessage);
                }

                return response.Result;
            }
        }

    }
}
