<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConfirmRequest.aspx.cs" Inherits="ProjectManagement.Guest.ConfirmRequest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>JABSOM Biostatistics Core Facility Collaboration Request & Request form - Confirm</title>
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

        img {
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
            <img src="../images/Banner_Print3_QHS-01.png" class="img-rounded" />
            <br />
            <br />
            <div class="panel panel-warning">
                <div class="panel-heading">
                    <h4 class="text-center"><strong>Please review your submission below before proceeding.</strong></h4>
                </div>
            </div>

            <div class="text-center">
                <div class="panel panel-info">
                    <div class="panel-heading">
                        <h4><strong>Investigator Info</strong></h4>
                    </div>
                </div>
                <br />
                <div class="row form-group-md">
                    <%--<label class="col-sm-2 control-label" for="lblFirstName">First name</label>--%>
                    <div class="col-sm-4">
                        <strong>First name:</strong>
                        <asp:Label ID="lblFirstName" runat="server"></asp:Label>
                        <%--<input class="form-control disbaled" type="text" Id="lblFirstName" runat="server" />--%>
                    </div>
                    <div class="col-sm-4">
                        <strong>Last name:</strong>
                        <asp:Label ID="lblLastName" runat="server"></asp:Label>
                    </div>
                    <div class="col-sm-4">
                        <strong>Degree:</strong>
                        <asp:Label ID="lblDegree" runat="server"></asp:Label>
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-sm-4">
                        <strong>Email:</strong>
                        <asp:Label ID="lblEmail" runat="server"></asp:Label>
                    </div>
                    <div class="col-sm-4">
                        <strong>Phone number:</strong>
                        <asp:Label ID="lblPhone" runat="server"></asp:Label>
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-sm-6">
                        <strong>Organization/Department:</strong>
                        <asp:Label ID="lblDept" runat="server"></asp:Label>
                    </div>
                    <div class="col-sm-6">
                        <strong>Investigator status:</strong>
                        <asp:Label ID="lblInvestStatus" runat="server"></asp:Label>
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-sm-4">
                        <strong>Is PI a junior investigator?</strong>
                        <asp:Label ID="lblJuniorPI" runat="server"></asp:Label>
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-sm-4">
                        <strong>Does PI have mentor?</strong>
                        <asp:Label ID="lblMentor" runat="server"></asp:Label>
                    </div>
                </div>
                <br />


                <hr />
                <%--<h4 style="background-color: cornflowerblue">Project Info</h4>--%>
                <div class="panel panel-info">
                    <div class="panel-heading">
                        <h4><strong>Project Info</strong></h4>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-12">
                        <strong>Project Title:</strong>
                        <asp:Label ID="lblProjectTitle" runat="server"></asp:Label>
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-sm-12">
                        <strong>Project Summary:</strong>
                        <asp:Label ID="lblProjectSummary" runat="server"></asp:Label>
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-sm-6">
                        <strong>Study Area:</strong>
                        <asp:Label ID="lblStudyArea" runat="server"></asp:Label>
                    </div>
                    <div class="col-sm-6">
                        <strong>Health Database(s) Utilized:</strong>
                        <asp:Label ID="lblHealthData" runat="server"></asp:Label>
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-sm-6">
                        <strong>Study Type:</strong>
                        <asp:Label ID="lblStudyType" runat="server"></asp:Label>
                    </div>
                    <div class="col-sm-6">
                        <strong>Study Population:</strong>
                        <asp:Label ID="lblStudyPopulation" runat="server"></asp:Label>
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-sm-12">
                        <strong>Type of support:</strong>
                        <asp:Label ID="lblService" runat="server"></asp:Label>
                    </div>
                </div>
                <br />

                <div class="row">
                    <div class="col-sm-6">
                        <strong>Is project a funded infrastructure grant pilot study?</strong>
                        <asp:Label ID="lblPilot" runat="server"></asp:Label>
                    </div>
                </div>
                <br />


                <div class="row">
                    <div class="col-sm-6">
                        <strong>Is this project for a grant proposal?</strong>
                        <asp:Label ID="lblProposal" runat="server"></asp:Label>
                    </div>
                </div>
                <br />

                <div class="row">
                    <div class="col-sm-11 col-sm-offset-1">
                        <strong>Is this application to a pilot program of a UH infrastructure grant?</strong>
                        <asp:Label ID="lblUHPilotGrant" runat="server"></asp:Label>
                    </div>
                    <div class="col-sm-11 col-sm-offset-1">
                        <strong>What is the grant / funding agency?</strong>
                        <asp:Label ID="lblPilotGrantName" runat="server"></asp:Label>
                    </div>
                </div>
                <br />

                <div class="row">
                    <div class="col-sm-12">
                        <strong>Funding Source:</strong>
                        <asp:Label ID="lblGrant" runat="server"></asp:Label>
                    </div>
                </div>
                <br />


                <div class="row">
                    <div class="col-sm-12">
                        <strong>Important deadlines:</strong>
                        <asp:Label ID="lblDueDate" runat="server"></asp:Label>
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-sm-12">
                        <strong>QHS faculty/staff preference (if any):</strong>
                        <asp:Label ID="lblBiostat" runat="server"></asp:Label>
                    </div>
                </div>
            </div>

            <hr />

            <div class="text-center">
                <p class="contentWords"><strong>Would you like to proceed?</strong></p>
                <div class="row" style="width: 50%;margin-left: 25%;">
                    <div class="col-xs-6">
                        <asp:Button ID="btnReturn" runat="server" CssClass="btn btn-danger" Text="No (Go back and edit)" OnClick="btnReturn_Click" />
                    </div>
                    <div class="col-xs-6">
                        <asp:Button runat="server" CssClass="btn btn-info" Text="Yes (Submit)" />
                    </div>
                </div>
                <br /><br />
            </div>
        </div>
    </form>
</body>
</html>
