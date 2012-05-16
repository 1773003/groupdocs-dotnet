using System;
using Groupdocs.Api.Contract.Annotation;
using Groupdocs.Api.Contract.Data;

namespace Groupdocs.Sdk
{
    using Groupdocs.Common;
    using Groupdocs.Api.Contract;

	public partial interface IGroupdocsService 
	{
        CreateAnnotationResult CreateAnnotation(string documentGuid, AnnotationType type, Rectangle box, Point? annotationPosition, 
                                                Range? textRange = null, string svgPath = null, string message = null);
        ListAnnotationsResult ListAnnotations(string documentGuid);
        DeleteAnnotationResult DeleteAnnotation(string annotationGuid);

        AddReplyResult AddAnnotationReply(string annotationGuid, string message);
        ListRepliesResult ListAnnotationReplies(string annotationGuid, DateTime? after = null);

        SetCollaboratorsResult SetAnnotationCollaborators(string documentGuid, string[] collaborators);
        GetCollaboratorsResult GetAnnotationCollaborators(string documentGuid);
	    MoveAnnotationResult MoveAnnotation(string annotationGuid, Point position);
	}
}
