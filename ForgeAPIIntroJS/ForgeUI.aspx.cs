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
using System.Web;
using System.Configuration; // for Configuration.Add reference to System.Configuration.
// Added for RestSharp. 
using RestSharp;

namespace ForgeAPIIntro
{
    /// <summary>
    /// Minimum UI to upload a model to for viewing.
    /// Workflow:  
    /// get token >> create a bucket >> upload a file >>
    /// translate a file for viewing (>> viewer comes in
    /// the next lab 4).
    /// </summary>

    public partial class ForgeUI : System.Web.UI.Page
    {
        // To Do: set your own configuration in App.config file.
        // This is an optional one for convenience to save typing.
        private static string bucketKey = ConfigurationManager.AppSettings["bucketKey"];

        protected void Page_Load(object sender, EventArgs e)
        {
            // An initial bucket name. 
            if(!IsPostBack)
            {
                TextBoxBucketKey.Text = bucketKey;
            }
        }

        //=========================================================
        //  Authentication
        //=========================================================
        protected void ButtonToken_Click(object sender, EventArgs e)
        {
            string scope = "bucket:create bucket:read data:read data:write";

            // Here is the main part that we call Forge authenticate. 
            string authToken = Forge.Authenticate(scope);

            bool authenticated = !string.IsNullOrEmpty(authToken);
            if (authenticated)
            {
                // Save authToken for this session
                Session["authToken"] = authToken;

                // Show the obtained token in the UI.
                // Note: in real world applications, you don't expose token. 
                // This is for our learning and testing.
                TextBoxToken.Text = authToken; 
            }

            // Show the request and response in the form. 
            // This is for learning purpose. 
            ShowRequestResponse(); 
        }

        //==========================================================
        // Helper functions 
        //==========================================================
        // Displays request and response in the text boxes.
        // This is for learning purpose.
        // Note: With this web service version, we don't update with 
        // init calls. We want to avoid an extra network trip and 
        // to keep it simple for the purpose of this lab's exercise. 
        private void InitRequestResponse() // Not used with web version. 
        {
            // initialize the request and response text in the form.
            InitRequestResponse("Request comes here",
                "Response comes here. This may take seconds. Please wait...");
        }
        private void InitRequestResponse(string req, string res)
        {
            // initialize the request and response text in the form.
            TextBoxRequest.Text = req;
            LabelStatus.Text = "";
            TextBoxResponse.Text = res;
        }
        private void ShowRequestResponse()
        {
            // show the request and response in the form. 
            IRestResponse response = Forge.m_lastResponse;
            TextBoxRequest.Text = response.ResponseUri.AbsoluteUri;
            LabelStatus.Text = "Status: " + response.StatusCode.ToString();
            TextBoxResponse.Text = response.Content;
        }

        //==========================================================
        // Bucket 
        //==========================================================
        protected void ButtonBucketCreate_Click(object sender, EventArgs e)
        {
            string accessToken = HttpContext.Current.Session["authToken"] as string;
            string bucketKey = TextBoxBucketKey.Text;
            string policyKey = "transient"; // transient(24h)/temporary(30days)/persistent 

            // Here is the main part that we call Forge API bucket creation.
            // Return true if success, false otherwise.  
            bool result = Forge.CreateBucket(accessToken, bucketKey, policyKey);

            // For our learning, 
            // show the request and response in the form. 
            ShowRequestResponse(); 
        }

        protected void ButtonBucketDetails_Click(object sender, EventArgs e)
        {
            string accessToken = HttpContext.Current.Session["authToken"] as string;
            string bucketKey = TextBoxBucketKey.Text;

            // Here is the main part that we call Forge API bucket details.
            // Return value is bucket detail.   
            string details = Forge.GetBucketDetails(accessToken, bucketKey);

            // For our learning, 
            // show the request and response in the form. 
            ShowRequestResponse();
        }

        //==========================================================
        // Upload 
        //==========================================================
        protected void ButtonUpload_Click(object sender, EventArgs e)
        {
            string accessToken = Session["authToken"] as string;
            string bucketKey = TextBoxBucketKey.Text;
            string objectName = FileUpload1.FileName; 
            byte[] fileData = FileUpload1.FileBytes; 

            // Here is the main part that we call Forge API upload  
            string objectId = Forge.Upload(accessToken, bucketKey, objectName, fileData);

            // show the objectId = urn:xxx on the page. 
            TextBoxObjectId.Text = objectId; // "urn:xxx"

            // For our learning, 
            // show the request and response in the form. 
            ShowRequestResponse(); 
        }

        // Shows a list of objects in the bucket. 
        protected void ButtonObjects_Click(object sender, EventArgs e)
        {
            string accessToken = Session["authToken"] as string;
            string bucketKey = TextBoxBucketKey.Text;

            // Here is the main part that we call Forge API bucket objects.
            // Returns a list of bucket objects. 
            string bucketObjects = Forge.GetBucketObjects(accessToken, bucketKey);

            // For our learning, 
            // show the request and response in the form. 
            ShowRequestResponse();
        }

        //==========================================================
        // Translation
        //==========================================================

        // Convert objectId (or source urn) to Base64 encoded urn. 
        // This is an utility. Not using Forge API itself. 
        protected void ButtonToUrn64_Click(object sender, EventArgs e)
        {
            // Convert objectId (=source urn) to base64 encoded urn. 
            string objectId = TextBoxObjectId.Text;
            string urn64 = Forge.ObjectIdToUrn64(objectId);

            // Show the urn64 in the form. 
            TextBoxUrn64.Text = urn64;

            // This is not a REST call. We only clear the field with a message. 
            InitRequestResponse("Convert ObjectId to Base64 encoded urn.", "");
        }
        protected void ButtonTranslate_Click(object sender, EventArgs e)
        {
            string accessToken = Session["authToken"] as string;
            string urn64 = TextBoxUrn64.Text;

            // Here is the main part that we call Forge API to translate
            // the file for viewing. 
            bool result = Forge.Translate(accessToken, urn64);

            // For our learning, 
            // show the request and response in the form. 
            ShowRequestResponse(); 
        }

        protected void ButtonStatus_Click(object sender, EventArgs e)
        {
            string accessToken = Session["authToken"] as string;
            string urn64 = TextBoxUrn64.Text;

            // Here is the main part that we call Forge API to check 
            // the status of translation.
            // Possible value of status: pending/success/inprogress/failed/timeout.    
            string status = Forge.GetManifest(accessToken, urn64);

            // For our learning, 
            // show the request and response in the form. 
            ShowRequestResponse();
        }
    }
}