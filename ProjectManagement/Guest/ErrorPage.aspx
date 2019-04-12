<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ErrorPage.aspx.cs" Inherits="ProjectManagement.Guest.ErrorPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>JABSOM Biostatistics Core Facility - ERROR</title>    
    <script src="../Scripts/jquery-1.10.2.min.js"></script>
    <script src="../Scripts/bootstrap.min.js"></script>
    <script src="../Scripts/bootstrap-datepicker.min.js"></script>
    <link href="~/Content/bootstrap.min.css" rel="stylesheet" />
    <style>
        /*h3 {
            overflow: hidden;
            text-align: center;
            font-weight: bold;
        }
        h3:before,
        h3:after {
            background-color: #000;
            content: "";
            display: inline-block;
            height: 1px;
            position: relative;
            vertical-align: middle;
            width: 50%;
        }
        h3:before {
            right: 0.5em;
            margin-left: -50%;
        }
        h3:after {
            left: 0.5em;
            margin-right: -50%;
        }*/
        body {
            /*padding-top: 0.5em;
            padding-left: 20em;
            padding-right: 20em;
            padding-bottom: 1.0em;*/
        }

        /* Wrapping element */
        /* Set some basic padding to keep content from hitting the edges */
        .body-content { 
            padding-top: 1.0em;           
            /*background-color: 	#f1f3f8;*/
            background-color: whitesmoke;
        }
        img
        {   
            width: 100%;
        }
        hr {
            border: none;
            height: 2px;
            /* Set the hr color */
            color: #333; /* old IE */
            background-color: #333; /* Modern Browsers */
        }
        .noresize {
            resize: none; 
        }

        .table th, .table td { 
            background-color: white;
            border-top: none !important; 
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
     <div class="container body-content">
        <img src="../images/Banner_Print3.jpg" class="img-rounded" />
        <br />
        <br />
            <div class="panel panel-warning">
                <div class="panel-heading">              
                    <h4 class="text-center"><b>Error - Something went wrong with your form.</b></h4>
                </div>
            </div>

            <div class="text-center">
                <h2></h2>
                <br />
                <h3><b>Please try again.<br /><br /> For additional assistance or more information about the JABSOM Biostatistics Core Facility, feel free to contact us:</b></h3>
                <br />
                <p style="font-size:120%;">
                    <%--<h4>Office of Biostatistics & Quantitative Health Sciences (BQHS)</h4>
                <h4>University of Hawaii John A. Burns School of Medicine</h4>
                <h4>651 Ilalo Street, Biosciences Building, Suite 211</h4>
                <h4>Honolulu, HI 96813</h4>
                <h4>Phone: (808) 692-1840</h4>
                <h4>Fax: (808) 692-1966</h4>
                <h4>E-mail: biostat@hawaii.edu</h4>--%>
                    JABSOM Biostatistics Core Facility<br />
                    Department of Quantitative Health Sciences (QHS)<br />
                    University of Hawaii John A. Burns School of Medicine<br />
                    651 Ilalo Street, Medical Education Building, Suite 411<br />
                    Honolulu, HI 96813<br />
                    Phone: (808) 692-1840<br />
                    E-mail: qhs@hawaii.edu<br />
                    <a href="http://biostat.jabsom.hawaii.edu/" target="_blank">http://biostat.jabsom.hawaii.edu/</a>
                </p>
            </div>
        </div>
    </form>
</body>
</html>
