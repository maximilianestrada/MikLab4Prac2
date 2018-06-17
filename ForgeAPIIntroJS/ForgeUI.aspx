<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgeUI.aspx.cs" Inherits="ForgeAPIIntro.ForgeUI" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Forge API Web Intro</title>
    <meta charset="utf-8" />
    <style type="text/css">
        #form1
        {
            height: 171px;
            width: 732px;
        }
        body
        {
            background-color:#d4ebda; 
        }
        h1
        {
            color:#201e58;  
            text-align: center;
        }
        #viewer
        {
            height: 300px;
            width: 300px;
            background-color:#f1f0f0; 
        }
        .group {
            border-color: azure;
            border-width: thin; 
        }
        .indent {
            padding-left: 30px;
        }
    </style>

    <!-- Added for Lab4 -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
    <!-- Autodesk Viewer CSS -->
    <link rel="stylesheet" href="https://developer.api.autodesk.com/modelderivative/v2/viewers/style.min.css" type="text/css"/>
    <!--  -->

</head>

<body> 
    <form id="form1" runat="server">
        <h1>My First Forge API Web Intro</h1> 
    <div>
        <!-- Token -->  
        <fieldset class="group">
            <legend>Authentication</legend>
            <div class="indent">
            <asp:Label ID="LabelToken" runat="server" Text="Token" Width="60px"></asp:Label>  
         
            <asp:TextBox ID="TextBoxToken" runat="server" Width="400px"></asp:TextBox>
    &nbsp;
            <asp:Button ID="ButtonToken" runat="server" OnClick="ButtonToken_Click" Text="Get Token" Width="90px" />
            </div>
        </fieldset>
        <br />
        <!-- OSS -->
        <fieldset class="group">
            <legend>OSS</legend>
            <div class="indent">
            <asp:Label ID="LabelBucket" runat="server" Text="Bucket" Width="60px"></asp:Label>

            <asp:TextBox ID="TextBoxBucketKey" runat="server" Width="400px"></asp:TextBox>
            &nbsp
            <asp:Button ID="ButtonBucketCreate" runat="server" Text="Create" OnClick="ButtonBucketCreate_Click" Width="90px" />
            &nbsp;
            <asp:Button ID="ButtonBucketDetails" runat="server" OnClick="ButtonBucketDetails_Click" Text="Details" Width="90px" />
            <br />
            <br />
            <asp:Label ID="LabelFile" runat="server" Text="File" Width="60px"></asp:Label>

            <asp:FileUpload ID="FileUpload1" runat="server" Width="400px"></asp:FileUpload>
            &nbsp;&nbsp;
            <asp:Button ID="ButtonUpload" runat="server" Text="Upload" OnClick="ButtonUpload_Click" Width="90px" />

            &nbsp;
            <asp:Button ID="ButtonObjects" runat="server" OnClick="ButtonObjects_Click" Text="Objects" Width="90px" />
            </div>
        </fieldset>

        <br />
        <!-- Derivative -->
        <fieldset class="group">
            <legend>Derivative</legend>
            <div class="indent">
            <asp:Label ID="LabelObjectId" runat="server" Text="Object Id" Width="60px"></asp:Label>

            <asp:TextBox ID="TextBoxObjectId" runat="server" Width="400px"></asp:TextBox>
            &nbsp;
            <asp:Button ID="ButtonToUrn64" runat="server" OnClick="ButtonToUrn64_Click" Text="To urn64" Width="90px" />
            <br />
            <br />
            <asp:Label ID="LabelUrn64" runat="server" Text="urn64" Width="60px"></asp:Label>

            <asp:TextBox ID="TextBoxUrn64" runat="server" Width="400px"></asp:TextBox>
            &nbsp;
            <asp:Button ID="ButtonTranslate" runat="server" Text="Translate" OnClick="ButtonTranslate_Click" Width="90px" />
            &nbsp;
            <asp:Button ID="ButtonStatus" runat="server" OnClick="ButtonStatus_Click" Text="Status" Width="90px" />
            </div>
        </fieldset>

        <br />
        <!-- Request and Response --> 
        <asp:Label ID="LabelRequest" runat="server" Text="Request"></asp:Label>
        <br />
        <asp:TextBox ID="TextBoxRequest" runat="server" Height="50px" TextMode="MultiLine" Width="580px"></asp:TextBox>
        <br />

        <asp:Label ID="LabelResponse" runat="server" Text="Response"></asp:Label>
        &nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="LabelStatus" runat="server"></asp:Label>
        <br />
        <asp:TextBox ID="TextBoxResponse" runat="server" Height="70px" TextMode="MultiLine" Width="580px" ValidateRequestMode="Disabled"></asp:TextBox>
        <br /><br />

        <!-- Viewer Lab 4 --> 
        <input id="ButtonView" type="button" value="View" onclick="buttonViewClicked()" /><br />
        <!-- The Viewer will be instantiated here -->
        <div id="MyViewerDiv"></div>

    </div> 
    </form>

    <!-- Viewer Lab 4--> 
    <!-- Autodesk Viewer JS--> 
    <script src="https://developer.api.autodesk.com/modelderivative/v2/viewers/three.min.js"></script>
    <script src="https://developer.api.autodesk.com/modelderivative/v2/viewers/viewer3D.min.js"></script>
    <!-- Forge API Intro Code --> 
    <script src="Scripts/viewerBasic.js"></script>

    <script>
        // Call our basic viewer with a valid access token, urn
        // and id of <div/> where the actual viewer goes. 
        // For our Intro lab project, we simply pass the value from 
        // the token text box. In real world application, you will 
        // get it from your server. 
        function buttonViewClicked() {
            var token = $('#TextBoxToken').val();
            var urn = $('#TextBoxUrn64').val();
            viewByTokenAndUrn(token, urn, "MyViewerDiv");
        }
    </script>

</body>
</html>
