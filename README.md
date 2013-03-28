groupdocs-dotnet
================

GroupDocs .NET SDK
####################################

Requirements
************

-  SDK requires .NET 4.0 (or later).

Installation
************

You can use this repository to download and install SDK manually. Also GroupDocs SDK is now
in `NUGET`.

Nuget package: https://nuget.org/packages/groupdocs-dotnet/

Last SDK version: 2.3.0.

Usage Example
*************

    // Create service for Groupdocs account
    GroupdocsService service = new GroupdocsService("https://api.groupdocs.com/v2.0", userId, private_key);
    // Get info about user account
    userInfo = service.GetUserProfile();

###[Sign, Manage, Annotate, Assemble, Compare and Convert Documents with GroupDocs](http://groupdocs.com)
1. [Sign documents online with GroupDocs Signature](http://groupdocs.com/apps/signature)
2. [PDF, Word and Image Annotation with GroupDocs Annotation](http://groupdocs.com/apps/annotation)
3. [Online DOC, DOCX, PPT Document Comparison with GroupDocs Comparison](http://groupdocs.com/apps/comparison)
4. [Online Document Management with GroupDocs Dashboard](http://groupdocs.com/apps/dashboard)
5. [Doc to PDF, Doc to Docx, PPT to PDF, and other Document Conversions with GroupDocs Viewer](http://groupdocs.com/apps/viewer)
6. [Online Document Automation with GroupDocs Assembly](http://groupdocs.com/apps/assembly)

