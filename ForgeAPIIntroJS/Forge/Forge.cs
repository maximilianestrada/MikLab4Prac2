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
using System.Text;
using System.Net;           // for HttpStatusCode 
using System.Configuration; // for Configuration. Add reference to System.Configuration.
using System.Diagnostics;   // for Debug writing 
// Added for RestSharp 
using RestSharp;
using RestSharp.Deserializers;
using RestSharp.Serializers;

///=============================================================
/// Welcome to the Forge API. 
/// 
/// Forge class defines REST API call to Forge Web services.
/// 
/// You can find the API documentation at the following site:
/// https://developer.autodesk.com/
/// 
/// We are using a library RestSharp in this lab for 
/// simplicity and to focus on Forge specific API. 
///=============================================================
namespace ForgeAPIIntro
{
    class Forge
    {
        // Set values specific to your environments.

        // To Do: set your own configuration in App.config file.
        private static string baseApiUrl = ConfigurationManager.AppSettings["baseApiUrl"];
        private static string clientID = ConfigurationManager.AppSettings["clientID"];
        private static string clientSecret = ConfigurationManager.AppSettings["clientSecret"];

        // Member variables 
        // Save the last response. This is for learning purposes. 
        // Not required to make actual REST call. 
        public static IRestResponse m_lastResponse = null;

        ///=============================================================
        /// POST authenticate 
        /// Obtain a 2-legged access token.
        ///  
        /// URL
        /// https://developer.api.autodesk.com/authentication/v1/authenticate
        /// Method: POST
        /// Doc: 
        /// https://developer.autodesk.com/en/docs/oauth/v2/reference/http/authenticate-POST/
        /// 
        /// Sample Response (JSON)
        /// {
        ///   "token_type":"Bearer",
        ///   "expires_in":3599,
        ///   "access_token":"aPJ8Ibj34KgDj8tkuWQ...Fjo4hzs5sN" 
        /// }
        ///=============================================================

        public static string Authenticate(string scope)
        {
            // (1) Build request 
            var client = new RestClient();
            client.BaseUrl = new System.Uri(baseApiUrl);

            // Set resource/end point
            var request = new RestRequest();
            request.Resource = "authentication/v1/authenticate";
            request.Method = Method.POST;

            // Set required parameters 
            request.AddParameter("client_id", clientID);
            request.AddParameter("client_secret", clientSecret);
            request.AddParameter("grant_type", "client_credentials");
            request.AddParameter("scope", scope);

            Debug.WriteLine("Calling POST authentication/v1/authenticate ...");

            // (2) Execute request and get response
            IRestResponse response = client.Execute(request);

            // Save response. This is to see the response for our learning.
            m_lastResponse = response;

            Debug.WriteLine("StatusCode = " + response.StatusCode);

            // (3) Parse the response and get the access token. 
            string accessToken = "";
            if (response.StatusCode == HttpStatusCode.OK)
            {
                JsonDeserializer deserial = new JsonDeserializer();
                AuthenticateResponse loginResponse =
                    deserial.Deserialize<AuthenticateResponse>(response);
                accessToken = loginResponse.access_token;
            }

            return accessToken;
        }

        ///==============================================================
        /// POST buckets
        /// Create a bucket. A "bucket" is a container that holds data. 
        /// 
        /// URL
        /// https://developer.api.autodesk.com/oss/v2/buckets
        /// Methods: POST 
        /// Doc
        /// https://developer.autodesk.com/en/docs/data/v2/reference/http/buckets-POST/
        ///==============================================================

        public static bool CreateBucket(string accessToken, string bucketKey, string policyKey)
        {
            // (1) Build request 
            var client = new RestClient();
            client.BaseUrl = new System.Uri(baseApiUrl);

            // Set resource/end point
            var request = new RestRequest();
            request.Resource = "oss/v2/buckets";
            request.Method = Method.POST;

            // Add headers  
            request.AddHeader("Authorization", "Bearer " + accessToken);
            request.AddHeader("Content-Type", "application/json");

            // Add JSON body. in the simplest form. 
            request.AddJsonBody(new { bucketKey = bucketKey, policyKey = policyKey });

            Debug.WriteLine("Calling POST oss/v1/buckets ...");

            // (2) Execute request and get response
            IRestResponse response = client.Execute(request);

            // Save response. This is to see the response for our learning.
            m_lastResponse = response;

            Debug.WriteLine("StatusCode = " + response.StatusCode);

            // (3) Parse the response. Return true if succeed, false otherwise. 
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }

            return false;
        }

        ///==============================================================
        /// GET buckets/:bucketKey/details
        /// Get bucket details. 
        /// This gives you infomration about your bucket. 
        /// 
        /// URL
        /// https://developer.api.autodesk.com/oss/v2/buckets/:bucketKey/details
        /// Methods: GET 
        /// Doc
        /// https://developer.autodesk.com/en/docs/data/v2/reference/http/buckets-:bucketKey-details-GET/
        ///==============================================================

        public static string GetBucketDetails(string accessToken, string bucketKey)
        {
            // (1) Build request 
            var client = new RestClient();
            client.BaseUrl = new System.Uri(baseApiUrl);

            // Set resource/end point
            var request = new RestRequest();
            request.Resource = "oss/v2/buckets/{bucketKey}/details";
            request.Method = Method.GET;

            // Add headers  
            request.AddHeader("Authorization", "Bearer " + accessToken);
            request.AddHeader("Content-Type", "application/json");

            // Set UrlSegment 
            request.AddUrlSegment("bucketKey", bucketKey);

            Debug.WriteLine("Calling GET oss/v2/buckets/{bucketKey}/details ...");

            // (2) Execute request and get response
            IRestResponse response = client.Execute(request);

            // Save response. This is to see the response for our learning.
            m_lastResponse = response;

            Debug.WriteLine("StatusCode = " + response.StatusCode);

            // (3) Parse the response. Return true if succeed, false otherwise. 
            string detail = "";    
            if (response.StatusCode == HttpStatusCode.OK)
            {
                detail = response.Content; 
            }

            return detail;  
        }

        ///==============================================================
        /// PUT buckets/:bucketKey/objects/:objectName
        /// Upload a file
        /// 
        /// URL
        /// https://developer.api.autodesk.com/oss/v2/buckets/:bucketKey/objects/:objectName
        /// Methods: PUT 
        /// Doc
        /// https://developer.autodesk.com/en/docs/data/v2/reference/http/buckets-:bucketKey-objects-:objectName-PUT/
        /// 
        /// ==============================================================

        public static string Upload(string accessToken, string bucketKey, string objectName, byte[] fileData)
        {
            // (1) Build request 
            var client = new RestClient();
            client.BaseUrl = new System.Uri(baseApiUrl);

            // Set resource/end point
            var request = new RestRequest();
            request.Resource = "oss/v2/buckets/{bucketKey}/objects/{objectName}";
            request.Method = Method.PUT;

            // Set UrlSegment 
            request.AddUrlSegment("bucketKey", bucketKey);
            request.AddUrlSegment("objectName", objectName);

            // Add header 
            request.AddHeader("Authorization", "Bearer " + accessToken);
            request.AddHeader("Content-Type", "application/octet-stream");

            // Add parameter 
            request.AddParameter("requestBody", fileData, ParameterType.RequestBody);

            Debug.WriteLine("Calling PUT oss/v2/buckets/{bucketKey}/objects/{objectName} ...");

            // (2) Execute request and get response
            IRestResponse response = client.Execute(request);

            // Save response. This is to see the response for our learning.
            m_lastResponse = response;

            Debug.WriteLine("StatusCode = " + response.StatusCode);

            // (3) Parse the response. Get objectId. 

            // Get the objectId. objectId looks like this: objectId = "urn:xxx" 
            string objectId = "";
            if (response.StatusCode == HttpStatusCode.OK)
            {
                JsonDeserializer deserial = new JsonDeserializer();
                OssBucketsObjectsObjectNameResponse bucketsObjectsResponse =
                    deserial.Deserialize<OssBucketsObjectsObjectNameResponse>(response);
                objectId = bucketsObjectsResponse.objectId;
            }

            return objectId; // "urn:xxx" 
        }

        ///==============================================================
        /// GET buckets/:bucketKey/objects
        /// Get a list of objects in the given bucket.
        /// 
        /// URL
        /// https://developer.api.autodesk.com/oss/v2/buckets/:bucketKey/objects
        /// Methods: GET 
        /// Doc
        /// https://developer.autodesk.com/en/docs/data/v2/reference/http/buckets-:bucketKey-objects-GET/
        ///==============================================================
        public static string GetBucketObjects(string accessToken, string bucketKey)
        {
            // (1) Build request 
            var client = new RestClient();
            client.BaseUrl = new System.Uri(baseApiUrl);

            // Set resource/end point
            var request = new RestRequest();
            request.Resource = "oss/v2/buckets/{bucketKey}/objects";
            request.Method = Method.GET;

            // Add headers  
            request.AddHeader("Authorization", "Bearer " + accessToken);
            request.AddHeader("Content-Type", "application/json");

            // Set UrlSegment 
            request.AddUrlSegment("bucketKey", bucketKey);

            Debug.WriteLine("Calling GET oss/v2/buckets/{bucketKey}/objects ...");

            // (2) Execute request and get response
            IRestResponse response = client.Execute(request);

            // Save response. This is to see the response for our learning.
            m_lastResponse = response;

            Debug.WriteLine("StatusCode = " + response.StatusCode);

            // (3) Parse the response. Return true if succeed, false otherwise.
            string bucketObjects = "";    
            if (response.StatusCode == HttpStatusCode.OK)
            {
                bucketObjects = response.Content; 
            }

            return bucketObjects; 
        }

        ///==============================================================
        /// POST Job
        /// Translate your data to another format. In our case, svf for viewable.
        /// 
        /// URL
        /// https://developer.api.autodesk.com/modelderivative/v2/designdata/job
        /// Methods: POST 
        /// Doc
        /// https://developer.autodesk.com/en/docs/model-derivative/v2/reference/http/job-POST/
        /// 
        /// ==============================================================
        public static bool Translate(string accessToken, string urn64)
        {
            // (1) Build request 
            var client = new RestClient();
            client.BaseUrl = new System.Uri(baseApiUrl);

            // Set resource/end point
            var request = new RestRequest();
            request.Resource = "modelderivative/v2/designdata/job";
            request.Method = Method.POST;

            // Add header 
            request.AddHeader("Authorization", "Bearer " + accessToken);
            request.AddHeader("Content-Type", "application/json");

            // Constract a request body 
            var views = new List<string>() { "2d", "3d" };
            JobBody body = new JobBody(urn64, "svf", views);
            JsonSerializer serializer = new JsonSerializer();
            string jsonbody = serializer.Serialize(body);

            request.AddParameter("application/json", jsonbody, ParameterType.RequestBody);

            Debug.WriteLine("Calling POST modelderivative/v2/designdata/job ...");

            // (2) Execute request and get response
            IRestResponse response = client.Execute(request);

            // Save response. This is to see the response for our learning.
            m_lastResponse = response;

            Debug.WriteLine("StatusCode = " + response.StatusCode);

            // (3) Parse the response. 
            // The job will start. You will need to check the status 
            // using GET :urn manifest after this.
             
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }

            return false;
        }

        /// ==============================================================
        /// Helper function. 
        /// Convert objectId (or source URN) to Base64-encoded URN
        /// Not using Forge API. 
        /// 
        /// cf. Tools to encode to base 64. 
        /// http://www.base64encode.org/
        /// cf. 
        /// https://stackoverflow.com/questions/26353710/how-to-achieve-base64-url-safe-encoding-in-c
        /// ==============================================================
        /// 
        static readonly char[] padding = { '=' };
        public static string ObjectIdToUrn64(string objectId)
        {
            // padded version
            byte[] bytes = Encoding.UTF8.GetBytes(objectId);
            string urn64 = Convert.ToBase64String(bytes);

            // unpadded version
            urn64 = urn64.TrimEnd(padding).Replace('+', '-').Replace('/', '_'); 

            return urn64;
        }

        ///==============================================================
        /// GET :urn/manifest
        /// Get information about derivatives, including the status of translation job. 
        ///   
        /// URL
        /// https://developer.api.autodesk.com/modelderivative/v2/designdata/:urn/manifest
        /// Methods: GET  
        /// Doc
        /// https://developer.autodesk.com/en/docs/model-derivative/v2/reference/http/urn-manifest-GET/
        ///==============================================================
        public static string GetManifest(string accessToken, string urn)
        {
            // (1) Build request 
            var client = new RestClient();
            client.BaseUrl = new System.Uri(baseApiUrl);

            // Set resource/end point
            var request = new RestRequest();
            request.Resource = "modelderivative/v2/designdata/{urn}/manifest";
            request.Method = Method.GET;

            // Add header 
            request.AddHeader("Authorization", "Bearer " + accessToken);
            //request.AddHeader("Content-Type", "application/json");

            // Set UrlSegment 
            request.AddUrlSegment("urn", urn);

            Debug.WriteLine("Calling GET modelderivative/v2/designdata/{urn}/manifest ...");

            // (2) Execute request and get response
            IRestResponse response = client.Execute(request);

            // Save response. This is to see the response for our learning.
            m_lastResponse = response;

            Debug.WriteLine("StatusCode = " + response.StatusCode);

            // (3) Parse the response. Return translation status. 
            string status = "";
            if (response.StatusCode == HttpStatusCode.OK)
            {
                JsonDeserializer deserial = new JsonDeserializer();
                UrnManifestResponse bucketsObjectsResponse =
                    deserial.Deserialize<UrnManifestResponse>(response);
                status = bucketsObjectsResponse.status;
            }

            return status;
        }
    }

    ///==============================================================
    /// Helper class to construct JSON body to call POST Job 
    /// 
    /// Doc 
    /// https://developer.autodesk.com/en/docs/model-derivative/v2/reference/http/job-POST/
    /// 
    /// Note: this is not generic solution. For the purpose of the lab, 
    /// we only considered making viewables. 
    ///==============================================================
    public class JobBody
    {
        // default constructor 
        public JobBody(string urn, string type, List<string> views)
        {
            input = new Input() { urn = urn };
            output = new Output()
            {
                formats = new List<Format>()
                {
                    new Format()
                    {
                        type = type,
                        views = views
                    }
                }
            };
        }

        public Input input { get; set; }
        public Output output { get; set; }
        public class Input
        {
            public string urn { get; set; }
        }

        public class Output
        {
            public List<Format> formats { get; set; }
        }
        public class Format
        {
            public string type { get; set; }
            public List<string> views { get; set; }
        }
    }
    //----------------------------------

}
