#region Copyright
//
// Copyright (C) 2015-2018 by Autodesk, Inc.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
//
// Written by M.Harada.  
// 
#endregion // Copyright

using System;
using System.Collections.Generic;

namespace ForgeAPIIntro
{
    /// Response for
    /// POST authenticate
    /// 2-legged authentication
    /// 
    /// POST 
    /// https://developer.api.autodesk.com/authentication/v1/authenticate
    /// Doc
    /// https://developer.autodesk.com/en/docs/oauth/v2/reference/http/authenticate-POST/
    /// 
    [Serializable]
    public class AuthenticateResponse
    {
        public string token_type { get; set; }
        public string expires_in { get; set; } // expiry time in seconds. (60 min) 
        public string access_token { get; set; }
    }

    /// Response for
    /// PUT buckets/:bucketKey/objects/:objectName
    /// this is for uploading a file to a bucket
    /// 
    /// PUT
    /// /oss/v2/buckets/:bucketKey/objects/:objectName
    /// Doc
    /// https://developer.autodesk.com/en/docs/data/v2/reference/http/buckets-:bucketKey-objects-:objectName-PUT/
    /// 
    [Serializable]
    public class OssBucketsObjectsObjectNameResponse
    {
        public string bucketKey { get; set; }
        //public List<OssBucketsObject> objects { get; set; }
        public string objectId { get; set; }
        public string objectKey { get; set; }
        public string sha1 { get; set; }
        public int size { get; set; }
        public string contentType { get; set; }
        public string location { get; set; }
    }

    /// 
    /// Response for 
    /// GET :urn/manifest
    /// 
    /// You can used this to check the status of derivatives, such as progress of translation. 
    /// 
    /// GET
    /// https://developer.api.autodesk.com/modelderivative/v2/designdata/:urn/manifest
    /// Doc: 
    /// https://developer.autodesk.com/en/docs/model-derivative/v2/reference/http/urn-manifest-GET/
    /// 
    /// cf. I used the following tool to generate C# class from a sample JSON. 
    /// http://json2csharp.com/
    /// 
    /// for more info. about the tool: 
    /// https://fieldofviewblog.wordpress.com/2015/04/23/json-to-c-sharp-class-generator/
    /// 
    [Serializable]
    public class UrnManifestResponse
    {
        public class Child
        {
            public string guid { get; set; }
            public string type { get; set; }
            public string role { get; set; }
            public string name { get; set; }
            public string status { get; set; }
            public string progress { get; set; }
            public string hasThumbnail { get; set; }
            public string mime { get; set; }
            public string urn { get; set; }
            public List<double> resolution { get; set; }

            public List<Child> children { get; set; }
        }
        public class Derivative
        {
            public string name { get; set; }
            public bool hasThumbnail { get; set; }
            public string role { get; set; }
            public string status { get; set; }
            public string progress { get; set; }
            public string outputType { get; set; }
            public List<Child> children { get; set; }
        }

        public string type { get; set; }
        public string hasThumbnail { get; set; }
        public string status { get; set; }
        public string progress { get; set; }
        public string region { get; set; }
        public string urn { get; set; }
        public List<Derivative> derivatives { get; set; }
    }

}
