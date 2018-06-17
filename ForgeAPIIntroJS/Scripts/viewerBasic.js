//
// Copyright (C) 2017-2018 by Autodesk, Inc.
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
// Written by: M.Harada. August 2016.  
// Last updated 1/21/2018

// The basic viewer
// 
// This is based on the instruction, and modified to fit our Intro lab: 
// https://developer.autodesk.com/en/docs/viewer/v2/tutorials/basic-viewer/
// (Note: the documentation page changes constantly. It may not the latest.) 
// 
// Note: If you want to view pdf., use basic application.
// https://developer.autodesk.com/en/docs/viewer/v2/tutorials/basic-application/
// 
// Usage e.g.: 
//      viewByTokenAndUrn(token, urn, "MyViewerDiv"); 
//

function viewByTokenAndUrn(token, urn, divId) {

    var viewer;
    var viewerDivId;

    //==================================================
    // Various callback functions
    //==================================================

    /**
    * Autodesk.Viewing.Document.load() success callback.
    * Proceeds with model initialization.
    */
    function onDocumentLoadSuccess(doc) {

        // A document contains references to 3D and 2D viewables.
        var viewables = Autodesk.Viewing.Document.getSubItemsWithProperties(doc.getRootItem(), { 'type': 'geometry' }, true);

        if (viewables.length === 0) {
            console.error('Document contains no viewables.');
            return;
        }

        // Choose any of the avialble viewables
        var initialViewable = viewables[0];
        var svfUrl = doc.getViewablePath(initialViewable);
        var modelOptions = {
            sharedPropertyDbPath: doc.getPropertyDbPath()
        };

        //var viewerDiv = document.getElementById('MyViewerDiv');
        var viewerDiv = document.getElementById(viewerDivId);
        viewer = new Autodesk.Viewing.Private.GuiViewer3D(viewerDiv);
        viewer.start(svfUrl, modelOptions, onLoadModelSuccess, onLoadModelError);
    }

    /**
    * Autodesk.Viewing.Document.load() failuire callback.
    */
    function onDocumentLoadFailure(viewerErrorCode) {
        console.error('onDocumentLoadFailure() - errorCode:' + viewerErrorCode);
    }

    /**
    * viewer.loadModel() success callback.
    * Invoked after the model's SVF has been initially loaded.
    * It may trigger before any geometry has been downloaded and displayed on-screen.
    */
    function onLoadModelSuccess(model) {
        console.log('onLoadModelSuccess()!');
        console.log('Validate model loaded: ' + (viewer.model === model));
        console.log(model);
    }

    /**
    * viewer.loadModel() failure callback.
    * Invoked when there's an error fetching the SVF file.
    */
    function onLoadModelError(viewerErrorCode) {
        console.error('onLoadModelError() - errorCode:' + viewerErrorCode);
    }

    //======================================================================
    // Main 
    //======================================================================

    var options = {
        env: 'AutodeskProduction',
        accessToken: token //'<YOUR_APPLICATION_TOKEN>'
    };

    var documentId = 'urn:' + urn; // 'urn:<YOUR_URN_ID>';

    viewerDivId = divId; // This is where your viewer goes. e.g., <div id="MyViewerDiv"/>

    // This is from basic viewer.
    Autodesk.Viewing.Initializer(options, function onInitialized() {
        Autodesk.Viewing.Document.load(documentId, onDocumentLoadSuccess, onDocumentLoadFailure);
    });

}