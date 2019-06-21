<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClientRequestForm.aspx.cs" Inherits="ProjectManagement.Guest.ClientRequestForm" %>

<!DOCTYPE html>

<html xmlns="https://www.w3.org/1999/xhtml">
<head runat="server">
    <title>JABSOM Biostatistics Core Facility Request Form</title>
    <link href="~/Content/bootstrap.min.css" rel="stylesheet" />
    <script src="../Scripts/jquery-1.10.2.min.js"></script>
    <script src="../Scripts/bootstrap-datepicker.min.js"></script>
    <script src="../Scripts/jquery.validate.js"></script>
    <script src="../Scripts/bootstrap.min.js"></script>
    <script src="../Scripts/InputMask.js"></script>
    <%-- Refer to reCaptcha API --%>
    <%--<script src="https://www.google.com/recaptcha/api.js?onload=renderRecaptcha&render=explicit" async="async" defer="defer"></script>--%>
    <script src="https://www.google.com/recaptcha/api.js?render=<%= ConfigurationManager.AppSettings["captchaSiteKey"] %>"></script>
    

    <%-- https://www.c-sharpcorner.com/article/integration-of-google-recaptcha-in-websites/ --%>

    <%--<script src="../Scripts/jquery.cookie.js"></script>--%>

     

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
            color: #ccc; /* old IE */
            background-color: #ccc; /*333*/ /* Modern Browsers */
        }

        .noresize {
            resize: none;
        }

        .table th, .table td {
            /*background-color: white;*/
            border-top: none !important;
        }

        .error {
            color: #a99588;
            font-size: 1em;
        }

        #formIncomplete {
            color: darkred;
        }

        #MainContent_lblWarning {
            font-size: 16pt;
        }

        #textWarning {
            font-size: 12pt;
            font-weight: bold;
            color: red;
        }

        .pageTitle {
            border-bottom: 1px #ccc solid;
            font-family: 'Lucida Sans', sans-serif;
        }

        .contentWords {
            font-family: 'Noto Sans', sans-serif;
            font-size: 12pt;
        }

        .greenLink {
            color: green;
        }

        .instructions {
            margin-left: 2.5%;
            margin-right: 2.5%;
        }

        .twitter-typeahead {
            width: 100%;
        }

        .tt-menu {
            width: 100%;
            background-color: white;
            border: 1px green solid;
        }

        .tt-selectable {
            padding: 3px;
        }

        .tt-selectable:hover,
        .tt-selectable:focus {
            background-color: lightgreen;
        }

        .tooltip2 {
            position: relative;
            display: inline-block;
            border-bottom: 1px dotted black;
            font-weight: bold;
        }

        .tooltip2 .tooltiptext2 {
            visibility: hidden;
            width: 120px;
            background-color: grey;
            color: #FFF;
            text-align: center;
            border-radius: 6px;
            padding: 5px;
            font-weight: normal;
            /* Position the tooltip. */
            position: absolute;
            z-index: 1;
            top: -5px;
            left: 105%;
            /* Fade in tooltip - takes 1 second to go from 0% to 100% opacity. */
            opacity: 0;
            transition: opacity 1s;
        }

        /*.tooltip2 .tooltiptext2::after {
        content: "";
        position: absolute;
        top: 50%;
        right: 100%;
        margin-top: -5px;
        border-width: 5px;
        border-style: solid;
        border-color: transparent black transparent transparent;
        }*/
        .tooltip2:hover .tooltiptext2 {
            visibility: visible;
            opacity: 1;
        }

        .help-block{
            font-weight: normal;
            color: #AAA;
        }

    </style>

</head>
<body>


    <form id="requestForm" runat="server" class="form-horizontal">

        <asp:ScriptManager runat="server">
            <Scripts>
                <%--To learn more about bundling scripts in ScriptManager see https://go.microsoft.com/fwlink/?LinkID=301884 --%>
                <%--Framework Scripts--%>
                <asp:ScriptReference Name="MsAjaxBundle" />
                <%--Site Scripts--%>
            </Scripts>
        </asp:ScriptManager>

        <div class="container body-content">
            <%--<div class="jumbotron">--%>
            <%--<img src="../images/Banner_Print3.jpg" class="img-rounded" />--%>
            <img src="<%=Page.ResolveUrl("~/images/Banner_Print3_QHS-01.png")%>" class="img-rounded" />
            <br />
            <br />
            <div id="divRequest" visible="false">
                <div class="text-center">
                    <h3 class="text-center pageTitle"><strong>JABSOM BIOSTATISTICS CORE FACILITY REQUEST FORM</strong></h3>
                </div>
                <br />
                <div class="instructions">
                    <%-- <h4><b>Mahalo for contacting the Office of Biostatistics & Quantitative Health Sciences (BQHS)! To request our quantitative health sciences support, please complete and submit the form below.</b></h4>--%>
                    <p class="contentWords">
                        <strong>Purpose:</strong> To help us determine your needs and prioritize our resoures.
                    </p>
                    <br />
                    <p class="contentWords">
                        <strong>Timeline:</strong> All <strong>grant</strong> requests must be submitted at least four (4) weeks before any required project 
                           completion deadline (e.g., 4 weeks prior to a grant submission deadline). All other requested must be submitted at least two (2) weeks before
                           any required project completion deadline.
                    </p>
                    <br />
                    <p class="contentWords">
                        <strong>Authorship:</strong> It is our policy that for biostatistical <%--and bioinformatics --%>service requests which result in manuscript(s),
                           the biostatistician <%--or bioinformatician --%>contribution should be recognized by manuscript co-authorship. Investigators submitting requests for
                           statistical assistance with research grant preparation are expected to include a budget for statistical effort in the grant.
                    </p>
                    <br />
                    <p class="contentWords">
                        Please review the <a href="https://qhs.jabsom.hawaii.edu/wp-content/uploads/sites/31/2019/06/20190507-Biostat-Core-Policies_Final.pdf" target="_blank" class="greenLink"><strong>Biostatistics Core Facility Policies</strong></a> <strong>prior</strong> to submitting this request.
                    </p>
                    <br />
                    <p class="contentWords">
                        If funding from <strong>Ola HAWAII</strong> will be used to support this request, please go <a href="https://redcap.jabsom.hawaii.edu/redcap/surveys/?s=8HD7K4EMWJ" class="greenLink"><strong>here</strong></a>. 
                    </p>
                    <br />
                    <p class="contentWords">
                        Please complete the fields below with your contact information.
                    </p>
                </div>
                <hr />
                <div class="panel panel-success">
                    <div class="panel-heading">
                        <h4 class="text-center"><b>Investigator Information</b></h4>
                    </div>
                </div>

                <%--<div class="form-group form-group-lg">
            <label class="col-sm-2 control-label" for="formGroupInputLarge">Large label</label>
            <div class="col-sm-10">
                <input class="form-control" type="text" id="formGroupInputLarge" placeholder="Large input" />
            </div>
        </div>--%>
                <div class="row form-group-md">
                    <label class="col-sm-2 control-label" for="txtFirstName">First name</label>
                    <div class="col-sm-2">
                        <input class="form-control" type="text" name="txtFirstName" id="txtFirstName" placeholder="First name" runat="server" />
                    </div>
                    <label class="col-sm-2 control-label" for="txtLastName">Last name</label>
                    <div class="col-sm-2">
                        <input class="form-control" type="text" name="txtLastName" id="txtLastName" placeholder="Last name" runat="server" />
                    </div>
                    <label class="col-sm-1 control-label" for="ddlDegree">Degree</label>
                    <div class="col-sm-3">
                        <%--<input class="form-control" type="text" name="txtDegree" id="txtDegree" placeholder="Degree" runat="server" />--%>
                        <asp:DropDownList ID="ddlDegree" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                </div>
                <div class="row form-group-md">
                    <div class="col-sm-9"></div>
                    <div class="col-sm-3">
                        <label class="control-label" for="txtDegreeOther">Other:</label>
                        <input class="form-control" type="text" name="txtDegreeOther" id="txtDegreeOther" placeholder="Degree - Other" runat="Server" />
                    </div>
                </div>
                <br />
                <div class="row form-group-md">
                    <label class="col-sm-2 control-label" for="txtEmail">Email</label>
                    <div class="col-sm-2">
                        <input class="form-control" type="text" name="txtEmail" id="txtEmail" placeholder="Email" runat="server" />
                    </div>
                    <label class="col-sm-2 control-label" for="txtPhone">
                        Phone number
                <%--<p class="help-block">(only enter numbers)</p>--%>
                    </label>
                    <div class="col-sm-2">
                        <input class="form-control phoneNum" type="text" name="txtPhone" id="txtPhone" placeholder="Phone" runat="server" />
                    </div>
                    <%--<div class="col-xs-2 col-md-2">
                        <label for="chkPilot">Pilot Investigator</label>
                        <asp:CheckBox ID="chkPilot" runat="server"></asp:CheckBox>
                    </div>--%>
                </div>
                <br />
                <div class="row form-group-md">
                    <label class="col-sm-2 control-label" for="txtDept">Organization/Department</label>
                    <div class="col-sm-5">
                        <input class="form-control typeahead" type="text" name="txtDept" id="txtDept" placeholder="Organization/Department" runat="server" onchange="updateId(this)" />
                    </div>
                    <label class="col-sm-2 control-label">Investigator status</label>
                    <div class="col-sm-3">
                        <%--<select class="form-control" name="ddlPIStatus">
                            <option value="UH Faculty">UH Faculty</option>
                            <option value="UH Fellow/Resident">UH Fellow/Resident</option>
                            <option value="UH Student">UH Student</option>
                            <option value="Other Research Staff">Other Research Staff</option>
                            <option value="Community Researcher">Community Researcher</option>
                            <option value="Other">Other</option>
                            <option value="Unknown">Unknown</option>
                        </select>--%>
                        <asp:DropDownList ID="ddlPIStatus" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                </div>
                <%--<div class="row form-group-md">
                    <div class="col-sm-9"></div>
                    <div class="col-sm-3">
                        <label class="control-label" for="txtPIStatusOther">Other:</label>
                        <input class="form-control" type="text" name="txtPIStatusOther" id="txtPIStatusOther" placeholder="Investigator status - Other" runat="Server" />
                    </div>
                </div>--%>
                <div class="row offset2 hidden">
                    <div class="col-md-2 text-right">
                        <label class="control-label">Organization Affiliation Id:</label>
                    </div>
                    <div class="col-md-1">
                        <input class="form-control" type="text" name="txtDeptAffilId" id="txtDeptAffilId" runat="Server" />
                    </div>
                </div>
                <br />
                <div class="row" id="divJuniorPiQ">
                    <div>
                        <div class="col-sm-4">
                            <label for="chkJuniorPIYes">Is PI a </label>
                            <div class="tooltip2">junior investigator<span class="tooltiptext2">Assistant Professor, Researcher, Specialist or below</span>?</div>
                            <%--<div class="tooltip">Help!<span class="tooltiptext">Hello</span></div>--%>
                        </div>
                        <div class="col-sm-2">
                            <asp:CheckBox ID="chkJuniorPIYes" value="1" runat="server" Text="Yes"></asp:CheckBox>
                        </div>
                        <div class="col-sm-2">
                            <asp:CheckBox ID="chkJuniorPINo" value="0" runat="server" Text="No"></asp:CheckBox>
                        </div>
                    </div>
                </div>
                <br />
                <div class="row" id="divMentorQ">
                    <div>
                        <div class="col-sm-4">
                            <label for="chkMentorYes">Does PI have mentor?</label>
                        </div>
                        <div class="col-sm-2">
                            <asp:CheckBox ID="chkMentorYes" value="1" runat="server" Text="Yes"></asp:CheckBox>
                        </div>
                        <div class="col-sm-2">
                            <asp:CheckBox ID="chkMentorNo" value="0" runat="server" Text="No"></asp:CheckBox>
                        </div>
                    </div>
                </div>
                <br />

                <div class="row" id="divMentor">
                    <div class="col-sm-2 text-left">
                        <label class="control-label" for="txtTitle">Mentor first name:</label>
                    </div>
                    <div class="col-sm-2">
                        <input class="form-control" type="text" name="txtMentorFirstName" id="txtMentorFirstName" runat="Server" />
                    </div>
                    <div class="col-sm-2 text-right">
                        <label class="control-label" for="txtTitle">Mentor last name:</label>
                    </div>
                    <div class="col-sm-2">
                        <input class="form-control" type="text" name="txtMentorLastName" id="txtMentorLastName" runat="Server" />
                    </div>
                    <div class="col-sm-1 text-right">
                        <label class="control-label" for="txtTitle">Email:</label>
                    </div>
                    <div class="col-sm-3">
                        <input class="form-control" type="text" name="txtMentorEmail" id="txtMentorEmail" runat="Server" />
                    </div>
                </div>
                <br />

                <div class="panel panel-success">
                    <div class="panel-heading">
                        <h4 class="text-center"><b>Project Information</b></h4>
                    </div>
                </div>
                <div class="row form-group-md">
                    <label class="col-sm-2 control-label" for="txtProjectTitle">Project title</label>
                    <div class="col-sm-10">
                        <input class="form-control" type="text" name="txtProjectTitle" id="txtProjectTitle" placeholder="Project title" runat="server" />
                    </div>
                </div>
                <br />
                <div class="row">
                    <label class="col-sm-2 control-label" for="txtProjectSummary">
                        Summary of project and QHS need
                    </label>
                    <div class="col-sm-10">
                        <textarea class="form-control noresize" rows="5" id="txtProjectSummary" name="txtProjectSummary" runat="server"></textarea>
                        <p class="help-block">Please provide a short summary of your project: description of data, outcomes, and a short statement of your service needs. It will be helpful for QHS to evaluate your need ahead of our initial consultation. (4000 character limit)</p>

                    </div>
                </div>

                <hr />

                <div class="row">
                    <label class="col-sm-2 control-label">
                        Study Area(s)<span class="help-block">(check all that apply)</span>
                    </label>
                    <div class="col-sm-10">
                        <table class="table" id="tblStudyArea">
                            <%--runat="server"--%>
                            <tbody>
                                <asp:Repeater ID="rptStudyArea" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td style="width: 50%">
                                                <asp:CheckBox ID="chkId" runat="server"></asp:CheckBox>
                                                <asp:HiddenField ID="Id" Value='<%#Eval("Id")%>' runat="server" />
                                                <asp:HiddenField ID="BitValue" Value='<%#Eval("BitValue")%>' runat="server" />
                                                <%# Eval("Name") %>
                                            </td>
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <td style="width: 50%">
                                            <asp:CheckBox ID="chkId" runat="server"></asp:CheckBox>
                                            <asp:HiddenField ID="Id" Value='<%#Eval("Id")%>' runat="server" />
                                            <asp:HiddenField ID="BitValue" Value='<%#Eval("BitValue")%>' runat="server" />
                                            <%# Eval("Name") %>
                                        </td>
                                        </tr>
                                    </AlternatingItemTemplate>
                                </asp:Repeater>
                                <%--<tr>
                                    <td>
                                        <label>
                                            <input type="checkbox" value="1" runat="server" />
                                            Aging & Chronic Disease Prevention/Management
                                        </label>
                                    </td>
                                    <td>
                                        <label>
                                            <input type="checkbox" value="4" runat="server" />
                                            Nutrition & Metabolic Health
                                        </label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            <input type="checkbox" value="2" runat="server" />
                                            Cardiovascular Health
                                        </label>
                                    </td>
                                    <td>
                                        <label>
                                            <input type="checkbox" value="5" runat="server" />
                                            Growth, Development, & Reproductive Health
                                        </label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            <input type="checkbox" value="3" runat="server" />
                                            Cancer Prevention & Health Recovery
                                        </label>
                                    </td>
                                    <td>
                                        <label>
                                            <input type="checkbox" value="6" runat="server" />
                                            Respiratory Health
                                        </label>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <label>
                                            <input type="checkbox" value="99" runat="server" />
                                            Other
                                        </label>
                                    </td>
                                </tr>--%>
                            </tbody>
                        </table>
                        <div class="row">
                            <div class="col-sm-6">
                                <label class="control-label" for="txtStudyAreaOther">Other:</label>
                                <input class="form-control" type="text" name="txtStudyAreaOther" id="txtStudyAreaOther" placeholder="Study Area(s) - Other" runat="Server" />
                                <br />
                            </div>
                            <div class="col-sm-2">
                                <input class="form-control hidden" type="text" name="txtStudyAreaBitSum" id="txtStudyAreaBitSum" runat="Server" />
                            </div>
                        </div>
                    </div>
                </div>
                <%--<div class="row form-group-md">
                    <div class="col-sm-9"></div>
                    <div class="col-sm-3">
                        <label class="control-label" for="txtStudyAreaOther">Other:</label>
                        <input class="form-control" type="text" name="txtPIStatusOther" id="Text1" placeholder="Investigator status - Other" runat="Server" />
                    </div>
                </div>--%>

                <hr />

                <div class="row">
                    <label class="col-sm-2 control-label">
                        Health Database(s) Utilized<span class="help-block">(check all that apply)</span>
                    </label>
                    <div class="col-sm-10">
                        <table class="table" id="tblHealthData">
                            <%--runat="server"--%>
                            <tbody>
                                <asp:Repeater ID="rptHealthData" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td style="width: 50%">
                                                <asp:CheckBox ID="chkId" runat="server"></asp:CheckBox>
                                                <asp:HiddenField ID="Id" Value='<%#Eval("Id")%>' runat="server" />
                                                <asp:HiddenField ID="BitValue" Value='<%#Eval("BitValue")%>' runat="server" />
                                                <%# Eval("Name") %>
                                            </td>
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <td style="width: 50%">
                                            <asp:CheckBox ID="chkId" runat="server"></asp:CheckBox>
                                            <asp:HiddenField ID="Id" Value='<%#Eval("Id")%>' runat="server" />
                                            <asp:HiddenField ID="BitValue" Value='<%#Eval("BitValue")%>' runat="server" />
                                            <%# Eval("Name") %>
                                        </td>
                                        </tr>
                                    </AlternatingItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                        <div class="row">
                            <div class="col-sm-6">
                                <label class="control-label" for="txtHealthDataOther">Other:</label>
                                <input class="form-control" type="text" name="txtHealthDataOther" id="txtHealthDataOther" placeholder="Health Database(s) Utilized - Other" runat="Server" />
                                <br />
                            </div>
                            <div class="col-sm-2">
                                <input class="form-control hidden" type="text" name="txtHealthDataBitSum" id="txtHealthDataBitSum" runat="Server" />
                            </div>
                        </div>
                    </div>
                </div>

                <hr />

                <div class="row">
                    <label class="col-sm-2 control-label">
                        Study Type<span class="help-block">(check all that apply)</span>
                    </label>
                    <div class="col-sm-10">
                        <table class="table" id="tblStudyType">
                            <%--runat="server"--%>
                            <tbody>
                                <asp:Repeater ID="rptStudyType" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td style="width: 50%">
                                                <asp:CheckBox ID="chkId" runat="server"></asp:CheckBox>
                                                <asp:HiddenField ID="Id" Value='<%#Eval("Id")%>' runat="server" />
                                                <asp:HiddenField ID="BitValue" Value='<%#Eval("BitValue")%>' runat="server" />
                                                <%# Eval("Name") %>
                                            </td>
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <td style="width: 50%">
                                            <asp:CheckBox ID="chkId" runat="server"></asp:CheckBox>
                                            <asp:HiddenField ID="Id" Value='<%#Eval("Id")%>' runat="server" />
                                            <asp:HiddenField ID="BitValue" Value='<%#Eval("BitValue")%>' runat="server" />
                                            <%# Eval("Name") %>
                                        </td>
                                        </tr>
                                    </AlternatingItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                        <div class="row">
                            <div class="col-sm-6">
                                <label class="control-label" for="txtStudyTypeOther">Other:</label>
                                <input class="form-control" type="text" name="txtStudyTypeOther" id="txtStudyTypeOther" placeholder="Study Type - Other" runat="Server" />
                                <br />
                            </div>
                            <div class="col-sm-2">
                                <input class="form-control hidden" type="text" name="txtStudyTypeBitSum" id="txtStudyTypeBitSum" runat="Server" />
                            </div>
                        </div>
                    </div>
                </div>

                <hr />

                <div class="row">
                    <label class="col-sm-2 control-label">
                        Study Population<span class="help-block">(check all that apply)</span>
                    </label>
                    <div class="col-sm-10">
                        <table class="table" id="tblStudyPopulation">
                            <%--runat="server"--%>
                            <tbody>
                                <asp:Repeater ID="rptStudyPopulation" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td style="width: 50%">
                                                <asp:CheckBox ID="chkId" runat="server"></asp:CheckBox>
                                                <asp:HiddenField ID="Id" Value='<%#Eval("Id")%>' runat="server" />
                                                <asp:HiddenField ID="BitValue" Value='<%#Eval("BitValue")%>' runat="server" />
                                                <%# Eval("Name") %>
                                            </td>
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <td style="width: 50%">
                                            <asp:CheckBox ID="chkId" runat="server"></asp:CheckBox>
                                            <asp:HiddenField ID="Id" Value='<%#Eval("Id")%>' runat="server" />
                                            <asp:HiddenField ID="BitValue" Value='<%#Eval("BitValue")%>' runat="server" />
                                            <%# Eval("Name") %>
                                        </td>
                                        </tr>
                                    </AlternatingItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                        <div class="row">
                            <div class="col-sm-4" id="divHealthDisparity">
                                <div class="col-sm-3">
                                    <label for="chkHealthDisparityYes">Health disparity?</label>
                                </div>
                                <div class="col-sm-3">
                                    <asp:CheckBox ID="chkHealthDisparityYes" runat="server" Text="Yes"></asp:CheckBox>
                                </div>
                                <div class="col-sm-3">
                                    <asp:CheckBox ID="chkHealthDisparityNo" runat="server" Text="No"></asp:CheckBox>
                                </div>
                                <div class="col-sm-3">
                                    <asp:CheckBox ID="chkHealthDisparityNA" runat="server" Text="N/A"></asp:CheckBox>
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <label class="control-label" for="txtStudyPopulationOther">Specify:</label>
                                <input class="form-control" type="text" name="txtStudyPopulationOther" id="txtStudyPopulationOther" placeholder="International Populations - Specify" runat="Server" />
                                <br />
                            </div>
                            <div class="col-sm-2">
                                <input class="form-control hidden" type="text" name="txtStudyPopulationBitSum" id="txtStudyPopulationBitSum" runat="Server" />
                            </div>
                        </div>
                    </div>
                </div>

                <hr />

                <div class="row">
                    <div class="col-sm-2">
                        <label class="control-label">
                            Type(s) of support needed
                <span class="help-block">(check all that apply)</span>
                        </label>
                    </div>
                    <div class="col-sm-10">
                        <table class="table" id="tblService">
                            <%--runat="server"--%>
                            <tbody>
                                <asp:Repeater ID="rptService" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td style="width: 50%">
                                                <asp:CheckBox ID="chkId" runat="server"></asp:CheckBox>
                                                <asp:HiddenField ID="Id" Value='<%#Eval("Id")%>' runat="server" />
                                                <asp:HiddenField ID="BitValue" Value='<%#Eval("BitValue")%>' runat="server" />
                                                <%# Eval("Name") %>
                                            </td>
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <td style="width: 50%">
                                            <asp:CheckBox ID="chkId" runat="server"></asp:CheckBox>
                                            <asp:HiddenField ID="Id" Value='<%#Eval("Id")%>' runat="server" />
                                            <asp:HiddenField ID="BitValue" Value='<%#Eval("BitValue")%>' runat="server" />
                                            <%# Eval("Name") %>
                                        </td>
                                        </tr>
                                    </AlternatingItemTemplate>
                                </asp:Repeater>
                            </tbody>
                            <%--<tbody>
                                <tr>
                                    <td>
                                        <label>
                                            <input type="checkbox" value="1" runat="server" />
                                            Study Design
                                        </label>
                                    </td>
                                    <td>
                                        <label>
                                            <input type="checkbox" value="4" runat="server" />
                                            Grant Proposal Development
                                        </label>
                                    </td>
                                    <td>
                                        <label>
                                            <input type="checkbox" value="7" runat="server" />
                                            Education
                                        </label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            <input type="checkbox" value="2" runat="server" />
                                            Database Design & Management
                                        </label>
                                    </td>
                                    <td>
                                        <label>
                                            <input type="checkbox" value="5" runat="server" />
                                            Bioinformatics Analysis
                                        </label>
                                    </td>
                                    <td>
                                        <label>
                                            <input type="checkbox" value="9" runat="server" />
                                            Other
                                        </label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            <input type="checkbox" value="3" runat="server" />
                                            Biostatistical Data Analysis
                                        </label>
                                    </td>
                                    <td>
                                        <label>
                                            <input type="checkbox" value="6" runat="server" />
                                            Dissemination
                                        </label>
                                    </td>
                                    <td></td>
                                </tr>
                            </tbody>--%>
                        </table>
                        <div class="row">
                            <div class="col-sm-6">
                                <label class="control-label" for="txtServiceOther">Other:</label>
                                <input class="form-control" type="text" name="txtServiceOther" id="txtServiceOther" placeholder="Type(s) of support needed - Other" runat="Server" />
                                <br />
                            </div>
                            <div class="col-sm-2">
                                <input class="form-control hidden" type="text" name="txtServiceBitSum" id="txtServiceBitSum" runat="Server" />
                            </div>
                        </div>
                    </div>
                </div>

                <hr />

                <div class="row" id="divPilotQ">
                    <div>
                        <div class="col-sm-4">
                            <label for="chkPilotYes">Is project a funded infrastructure grant pilot study?</label>
                        </div>
                        <div class="col-sm-2">
                            <asp:CheckBox ID="chkPilotYes" value="1" runat="server" Text="Yes"></asp:CheckBox>
                        </div>
                        <div class="col-sm-2">
                            <asp:CheckBox ID="chkPilotNo" value="0" runat="server" Text="No"></asp:CheckBox>
                        </div>
                    </div>
                </div>
                <br />

                <div class="row" id="divProposalQ">
                    <div>
                        <div class="col-sm-4">
                            <label for="chkProposalYes">Is this project for a grant proposal?</label>
                        </div>
                        <div class="col-sm-2">
                            <asp:CheckBox ID="chkProposalYes" value="1" runat="server" Text="Yes"></asp:CheckBox>
                        </div>
                        <div class="col-sm-2">
                            <asp:CheckBox ID="chkProposalNo" value="0" runat="server" Text="No"></asp:CheckBox>
                        </div>
                    </div>
                </div>
                <br />

                <div class="row divGrantProposal" id="divGrantProposal">
                    <div class="col-sm-6">
                        <div class="col-sm-9 text-left">
                            <label class="control-label" for="txt">Is this application to a pilot program of a UH infrastructure grant?</label>
                        </div>
                        <div class="col-sm-3">
                            <input type="checkbox" id="chkIsUHPilotGrantYes" value="1" runat="server" />Yes
                                        &nbsp;
                                    <input type="checkbox" id="chkIsUHPilotGrantNo" value="0" runat="server" />No
                        </div>
                    </div>
                    <div class="col-sm-6" id="divGrantProposalFundingAgency">
                        <div class="col-sm-4 text-left">
                            <label class="control-label" for="ddlUHGrant">What is the grant name?</label>
                        </div>
                        <div class="col-sm-6">
                            <input class="form-control" type="text" name="txtGrantProposalFundingAgency"
                                id="txtGrantProposalFundingAgency" runat="Server" />
                        </div>
                    </div>
                </div>
                <br />


                <hr />


                <div class="row">
                    <label class="col-sm-2 control-label">
                        What is the funding source to support this request?<span class="help-block">(check all that apply)</span> <%--Is your project affiliated with any of the following grants?--%>
                    </label>
                    <div class="col-sm-10">
                        <table class="table" id="tblFunding">
                            <%--runat="server"--%>
                            <tbody>
                                <asp:Repeater ID="rptFunding" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td style="width: 25%">
                                                <asp:CheckBox ID="FirstchkId" runat="server"></asp:CheckBox>
                                                <%--<asp:HiddenField ID="Id1" Value='<%#Eval("Id1")%>' runat="server" />--%>
                                                <asp:HiddenField ID="FirstBitValue" Value='<%#Eval("BitValue1")%>' runat="server" />
                                                <%# Eval("Name1") %>
                                            </td>
                                            <td style="width: 25%">
                                                <asp:CheckBox ID="SecondchkId" runat="server" Visible='<%# (int)Eval("Id2") > 0 %>'></asp:CheckBox>
                                                <%--<asp:HiddenField ID="Id2" Value='<%#Eval("Id2")%>' runat="server" />--%>
                                                <asp:HiddenField ID="SecondBitValue" Value='<%#Eval("BitValue2")%>' runat="server" />
                                                <%# Eval("Name2") %>
                                            </td>
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <td style="width: 25%">
                                            <asp:CheckBox ID="FirstchkId" runat="server"></asp:CheckBox>
                                            <%--<asp:HiddenField ID="Id1" Value='<%#Eval("Id1")%>' runat="server" />--%>
                                            <asp:HiddenField ID="FirstBitValue" Value='<%#Eval("BitValue1")%>' runat="server" />
                                            <%# Eval("Name1") %>
                                        </td>
                                        <td style="width: 25%">
                                            <asp:CheckBox ID="SecondchkId" runat="server" Visible='<%# (int)Eval("Id2") > 0 %>'></asp:CheckBox>
                                            <%--<asp:HiddenField ID="Id2" Value='<%#Eval("Id2")%>' runat="server" />--%>
                                            <asp:HiddenField ID="SecondBitValue" Value='<%#Eval("BitValue2")%>' runat="server" />
                                            <%# Eval("Name2") %>
                                        </td>
                                        </tr>
                                    </AlternatingItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                        <div class="row">
                            <div class="col-sm-4">
                                <label class="control-label" for="ddlDepartmentFunding">Department Funding</label>
                                <asp:DropDownList ID="ddlDepartmentFunding" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="col-sm-1"></div>
                            <div class="col-sm-3">
                                <label class="control-label" for="txtDeptFundOth">Other</label>
                                <input class="form-control" type="text" name="txtDeptFundOth" id="txtDeptFundOth" placeholder="Department Funding - Other" runat="Server" />
                            </div>
                            <div class="col-md-1"></div>
                            <div class="col-sm-3">
                                <label class="control-label" for="txtFundingOther">Other:</label>
                                <input class="form-control" type="text" name="txtFundingOther" id="txtFundingOther" placeholder="Funding Source - Other" runat="Server" />
                                <br />
                            </div>
                            <div class="col-sm-2">
                                <input class="form-control hidden" type="text" name="txtFundingBitSum" id="txtFundingBitSum" runat="Server" />
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div id="divDeptFundMou">
                                <div class="col-sm-6">
                                    <div class="col-sm-4">
                                        <label for="chkDeptFundMouYes">Is this project supported by an MOU?</label>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:CheckBox ID="chkDeptFundMouYes" runat="server" Text="Yes" />
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:CheckBox ID="chkDeptFundMouNo" runat="server" Text="No" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <br />
                    </div>
                </div>

                <hr />

                <div class="row">
                    <div class="col-sm-2">
                        <label class="control-label">Important deadlines</label>
                    </div>
                    <div class="col-sm-2">
                        <div class='input-group date' id='dtpDueDate'>
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-calendar"></span>
                            </span>
                            <asp:TextBox ID="txtDueDate" runat="server" class="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-sm-3">
                        <label class="control-label">Biostatistics faculty/staff preference (if any)</label>
                    </div>
                    <div class="col-sm-3">
                        <asp:DropDownList ID="ddlBiostat" runat="server" CssClass="form-control"></asp:DropDownList>
                        <%--<input class="form-control nameahead" type="text" name="txtPreferBiostat"  onchange="updateId(this)"  />--%>
                        <%--<asp:TextBox ID="txtPreferBiostat" runat="server" class="form-control"></asp:TextBox>--%>
                    </div>
                </div>
                <div class="row offset2 hidden">
                    <div class="col-md-2 text-right">
                        <label class="control-label">QHS Faculty Staff Preference Id:</label>
                    </div>
                    <div class="col-md-1">
                        <input class="form-control" type="text" name="txtPreferBiostatId" id="txtPreferBiostatId" runat="Server" />
                    </div>
                </div>
                <br />

                <%--[Not including bioinformatics for now]--%>
                <%--<div class="row" id="divProjectTypeQ">
                    <div>
                        <div class="col-sm-4">
                            <label for="chkProjectTypeBiostat">Send request to:</label>
                        </div>
                        <div class="col-sm-2">
                            <asp:CheckBox ID="chkProjectTypeBiostat" runat="server" Text="Biostat"></asp:CheckBox>
                        </div>
                        <div class="col-sm-2">
                            <asp:CheckBox ID="chkProjectTypeBioinfo" runat="server" Text="Bioinfo"></asp:CheckBox>
                        </div>
                        <div class="col-sm-2">
                            <asp:CheckBox ID="chkProjectTypeBoth" runat="server" Text="Both"></asp:CheckBox>
                        </div>
                    </div>
                </div>
                <br />--%>

                <div class="row" style="display: none;">
                    <div class="col-sm-2">
                        <label class="control-label">Google Captcha</label>
                    </div>
                    <div class="col-sm-2">
                        <%--<input class="form-control" type="text" name="txtCaptchaCode" id="txtCaptchaCode" />--%>
                        <%--<asp:TextBox ID="txtPreferBiostat" runat="server" class="form-control"></asp:TextBox>--%>

                        <div id="ReCaptchaContainer"></div>
                        <label id="lblRecaptchaMessage" runat="server" clientmode="static"></label>

                        <%--<input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response" />--%>
                        <asp:HiddenField ID="GcaptchaResponse" runat="server" />
                        <input type="hidden" name="action" value="validate_captcha" />


                    </div>
                </div>
                <br />

                <asp:UpdatePanel ID="UP1" runat="server">
                    <ContentTemplate>
                        <%--<div class="row form-group-md"">
                        <div class="col-sm-2">
                            <label class="control-label">Captcha code</label>
                        </div>
                        <div class="col-sm-2">
                            <asp:Image ID="imgCaptcha" runat="server" class="img-responsive" />                            
                        </div>
                        <div class="col-sm-1">
                            <button id="btnRefresh" runat="server" onserverclick="btnRefresh_Click" class="btn btn-info">
                                <span class="glyphicon glyphicon-refresh"></span> Refresh</button>
                        </div>
                    </div>--%>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <br />


                <div class="row">
                    <div class="col-sm-12">
                        <asp:Button ID="btnSubmit" name="btnSubmit" runat="server" Text="Submit" class="btn center-block" OnClick="btnSubmit_Click" PnClientClick="ClientSideClick(this)" UseSubmitBehavior="false" />
                        <button type="button" style="display: none" id="btnShowWarningModal" class="btn btn-primary btn-lg"
                            data-toggle="modal" data-target="#warningModal">
                            Warning Modal</button>
                    </div>
                </div>
                <div class="hidden">
                    <textarea id="textAreaDeptAffil" rows="3" runat="server"></textarea>
                    <%--<textarea id="textAreaMembers" rows="3" runat="server"></textarea>--%>
                </div>
                <br />




            </div>

            <div id="warningModal" class="modal fade">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                            <img style="height: 100px; width: 100px; display: block; margin-left: auto; margin-right: auto;"
                                src="../images/Stop_sign.png" />
                            <br />
                            <div id="formIncomplete" class="modal-title">
                                <h1>Form Incomplete</h1>
                                <h4 style="font-family: 'Times New Roman'"><em>Form has <u><strong>not</strong></u> been saved!!!</em></h4>
                            </div>
                        </div>
                        <div class="modal-body">
                            <asp:Label ID="lblWarning" runat="server">Please review the following <u>incomplete</u> fields and resubmit:</asp:Label><br />
                            <br />
                            <p class="txt-warning" id="textWarning"></p>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </form>

    <script src="../Scripts/typeahead.jquery.min.js"></script>
    <script type="text/javascript">

        grecaptcha.ready(function () {
            grecaptcha.execute('<%= ConfigurationManager.AppSettings["captchaSiteKey"] %>', { action: 'validate_captcha' })
                .then(function (token) {
                    $('#GcaptchaResponse').val(token);
                })
        });

        $(document).ready(function () {

            bindForm();

            $(function () {
                $('#dtpDueDate').datepicker({
                    todayHighlight: true,
                    format: "mm/dd/yyyy",
                    autoclose: true,
                    orientation: "top"
                });
            });

            $(function () {
                $(':text').bind('keydown', function (e) {
                    if (e.keyCode == 13) { //if this is enter key
                        e.preventDefault();
                        return false;
                    }
                    else
                        return true;
                });
            });

            //Automatically put parentheses and dash for phone number fields
            $(function () {
                var phones = [{ "mask": "(###) ###-####" }, { "mask": "(###) ###-##############" }];
                $('.phoneNum').inputmask({
                    mask: phones,
                    greedy: false,
                    definitions: { '#': { validator: "[0-9]", cardinality: 1 } }
                });
            });



        });

        // -- Prepares form jQuery/javascript functionality -- \\
        function bindForm() {

            // --> Shows "Other" field if dropdown selection is "Other".
            showHideOtherField($('#ddlDegree'), $('#txtDegreeOther'));
            //showHideOtherField($('#ddlPIStatus'), $('#txtPIStatusOther'));


            // --> Initialize Other/Bitsum functionality.
            var tStudyArea = new checkboxTable.TableToggle(tblStudyArea);
            initializeCheckboxTable(tStudyArea, tblStudyArea);

            var tHealthData = new checkboxTable.TableToggle(tblHealthData);
            initializeCheckboxTable(tHealthData, tblHealthData);

            var tStudyType = new checkboxTable.TableToggle(tblStudyType);
            initializeCheckboxTable(tStudyType, tblStudyType);

            var tStudyPopulation = new checkboxTable.TableToggle(tblStudyPopulation);
            initializeCheckboxTable(tStudyPopulation, tblStudyPopulation);

            var tService = new checkboxTable.TableToggle(tblService);
            initializeCheckboxTable(tService, tblService);

            var tFunding = new checkboxTable.TableToggle(tblFunding);
            initializeCheckboxTable(tFunding, tblFunding);


            // --> Toggles Yes/No checkboxes to make them single choice.

            $('#chkJuniorPIYes').change(function () {
                $('#chkJuniorPINo').prop('checked', false);
            });

            $('#chkJuniorPINo').change(function () {
                $('#chkJuniorPIYes').prop('checked', false);
            });


            $('#chkDeptFundMouYes').change(function () {
                $('#chkDeptFundMouNo').prop('checked', false);
            });

            $('#chkDeptFundMouNo').change(function () {
                $('#chkDeptFundMouYes').prop('checked', false);
            });

            ToggleDiv($('#chkMentorYes'), $('#divMentor'));

            $('#chkMentorYes').change(function () {
                ToggleDiv($(this), $('#divMentor'));
                $('#chkMentorNo').prop('checked', false);
            });

            $('#chkMentorNo').change(function () {
                $('#chkMentorYes').prop('checked', false);
                $('#divMentor').hide();
            });

            $('#chkPilotYes').change(function () {
                $('#chkPilotNo').prop('checked', false);
            });

            $('#chkPilotNo').change(function () {
                $('#chkPilotYes').prop('checked', false);
            });

            ToggleDiv4($('#rptStudyPopulation_chkId_0'),
                $('#rptStudyPopulation_chkId_1'),
                $('#rptStudyPopulation_chkId_2'),
                $('#rptStudyPopulation_chkId_3'),
                $('#divHealthDisparity'));



            $("#chkHealthDisparityYes").change(function () {
            if (this.checked) {
                $('#chkHealthDisparityNo').prop('checked', false);
                $('#chkHealthDisparityNA').prop('checked', false);
            }
            });

            $("#chkHealthDisparityNo").change(function () {
                if (this.checked) {
                    $('#chkHealthDisparityYes').prop('checked', false);
                    $('#chkHealthDisparityNA').prop('checked', false);
                }
            });

            $("#chkHealthDisparityNA").change(function () {
                if (this.checked) {
                    $('#chkHealthDisparityYes').prop('checked', false);
                    $('#chkHealthDisparityNo').prop('checked', false);
                }
            });

            ToggleDiv($('#chkProposalYes'), $('#divGrantProposal'));

            $('#chkProposalYes').change(function () {
                ToggleDiv($(this), $('#divGrantProposal'));
                $('#chkProposalNo').prop('checked', false);
            });

            $('#chkProposalNo').change(function () {
                //ToggleDiv($(this), $('#divGrantProposal'));
                $('#chkProposalYes').prop('checked', false);
                $('#divGrantProposal').hide();
            });

            ToggleDiv($('#chkIsUHPilotGrantYes'), $('#divGrantProposalFundingAgency'));
            ToggleDiv($('#chkIsUHPilotGrantNo'), $('#divGrantProposalFundingAgency'));

            $('#divGrantProposal').on('click',
                'input[type="checkbox"]',
                function () {

                    if ($(this).is($('#chkIsUHPilotGrantYes'))) {
                        ToggleDiv($(this), $('#divGrantProposalFundingAgency'));
                    }

                    if ($(this).is($('#chkIsUHPilotGrantNo'))) {
                        ToggleDiv($(this), $('#divGrantProposalFundingAgency'));
                    }

                    if ($(this).is(":checked")) {
                        if ($(this).val() == 1)
                            $(this).next().removeAttr("checked");
                        if ($(this).val() == 0) {
                            $(this).prev().removeAttr("checked");
                            //$('#ddlUHGrant').val('');
                        }


                        /*if ($(this).is($('#chkIsUHPilotGrantNo')))
                            $('#divUHGrant').hide();
                        if ($(this).is($('#chkIsUHPilotGrantYes')))
                            $('#divGrantProposalFundingAgency').hide();*/
                    } /*else {
                        $('#ddlUHGrant').val('');
                    }*/

                });

                $('#tblStudyPopulation').on('click',
                    'input[type="checkbox"]',
                    function () {
                        if ($(this).is($('#rptStudyPopulation_chkId_0')) // Native Hawaiians,
                            //--- Pacific Islanders,
                            //--- and Filipinos
                            || $(this).is($('#rptStudyPopulation_chkId_1')) // Hawaii Populations
                            || $(this).is($('#rptStudyPopulation_chkId_2')) // U.S. Populations
                            || $(this).is($('#rptStudyPopulation_chkId_3')) // International Populations
                        ) {
                            ToggleDiv($(this), $('#divHealthDisparity'));
                        }
                    });

            /// [Not including bioinformatics for now]
            //$("#chkProjectTypeBiostat").change(function () {
            //    if (this.checked) {
            //        $('#chkProjectTypeBioinfo').prop('checked', false);
            //        $('#chkProjectTypeBoth').prop('checked', false);
            //    }
            //});

            //$('#chkProjectTypeBioinfo').change(function () {
            //    if (this.checked) {
            //        $("#chkProjectTypeBiostat").prop('checked', false);
            //        $('#chkProjectTypeBoth').prop('checked', false);
            //    }
            //});

            //$('#chkProjectTypeBoth').change(function () {
            //    if (this.checked) {
            //        $('#chkProjectTypeBioinfo').prop('checked', false);
            //        $("#chkProjectTypeBiostat").prop('checked', false);
            //    }
            //});



        }


        // -- Given dropdown trigger, shows or hides the text "Other" field if "Other" is selected in the dropdown.-- \\
        function showHideOtherField(dropdown, otherField) {
            otherField.val('');
            otherField.hide();
            otherField.parent().find('label').hide()

            dropdown.on('change', function () {
                var selectedText = $("option:selected", this).text();

                if (selectedText == "Other") {
                    otherField.show();
                    otherField.parent().find('label').show();
                }
                else {
                    otherField.val('');
                    otherField.hide();
                    otherField.parent().find('label').hide();
                }
            });
        }

        // -- Initializes Checkbox Table and bitsum fields to determine selections.
        function initializeCheckboxTable(t, tableId) {

            t.Click(tableId);

            var _table = $(tableId),
                _textOther = _table.next().find(":input[name$='Other']"),
                _textBitSum = _table.next().find(":input[name$='BitSum']"),
                _id = _table.attr('id');

            _table.on('click', 'input[type="checkbox"]', function () {
                t.Click(this.closest('table'));
            });

            _table.find('td').each(function () {
                $(this).find(":input[name$='chkId']").prop('checked', false);
            })

            _textOther.val('');
            _textOther.parent().closest('div').hide();

        }

        // -- Initializes and creates functionality for checkbox tables
        var checkboxTable = checkboxTable || {};

        checkboxTable.TableToggle = function (tableId) {
            var _table = $(tableId),
                _textOther = _table.next().find(":input[name$='Other']"),
                _textBitSum = _table.next().find(":input[name$='BitSum']"),
                _id = _table.attr('id');


            // --> If "Other" checkbox is checked, then the "other" field is displayed.
            //     The BitSum total is added from the bit value of the checkbox.
            return {
                Click: function (e) {
                    var _bitSum = 0;

                    _table.find('td').each(function () {
                        var _checkBox = $(this).find(":input[name$='chkId']"),
                            _bitValue = $(this).find(":input[name$='BitValue']").val(),
                            _name = $(this).eq(0).text().trim();



                        if (_name == 'Other' || _name == 'International Populations') {

                            if (_checkBox.is(':checked')) {
                                _textOther.parent().closest('div').show();
                            }
                            else {
                                _textOther.val('');
                                _textOther.parent().closest('div').hide();
                            }


                        }


                        // --> Show "Department Funding" dropdown options. ||||||
                        if (_name == 'Department Funding' && _id == 'tblFunding') {

                            if (_checkBox.is(':checked')) {
                                $('#ddlDepartmentFunding').show();
                                $('#ddlDepartmentFunding').parent().find('label').show();


                                $('#ddlDepartmentFunding').change(function () {
                                    var selectedText = $("option:selected", this).text();

                                    if (selectedText == "Other") {
                                        $("#txtDeptFundOth").show();
                                        $("#txtDeptFundOth").parent().find('label').show();

                                        $('#chkDeptFundMouYes').prop('checked', false);
                                        $('#chkDeptFundMouNo').prop('checked', false);
                                        $('#divDeptFundMou').hide();

                                    } else if (selectedText == "School of Nursing & Dental Hygiene") {
                                        $('#divDeptFundMou').show();

                                        $('#txtDeptFundOth').val('');
                                        $('#txtDeptFundOth').hide();
                                        $('#txtDeptFundOth').parent().find('label').hide();
                                    } else {
                                        $('#txtDeptFundOth').val('');
                                        $('#txtDeptFundOth').hide();
                                        $('#txtDeptFundOth').parent().find('label').hide();

                                        $('#chkDeptFundMouYes').prop('checked', false);
                                        $('#chkDeptFundMouNo').prop('checked', false);
                                        $('#divDeptFundMou').hide();
                                    }

                                });

                                if ($("option:selected", $('#ddlDepartmentFunding')).text() == "Other") {
                                    $("#txtDeptFundOth").show();
                                    $("#txtDeptFundOth").parent().find('label').show();
                                } else if ($("option:selected", $("#ddlDepartmentFunding")).text() == "School of Nursing & Dental Hygiene") {
                                    $("#divDeptFundMou").show();

                                    $("#txtDeptFundOth").val('');
                                    $("#txtDeptFundOth").hide();
                                    $("#txtDeptFundOth").parent().find('label').hide();
                                } else {
                                    $("#txtDeptFundOth").val('');
                                    $("#txtDeptFundOth").hide();
                                    $("#txtDeptFundOth").parent().find('label').hide();

                                    $("#chkDeptFundMouYes").prop('checked', false);
                                    $("#chkDeptFundMouNo").prop('checked', false);
                                    $("#divDeptFundMou").hide();
                                }

                            }
                            else {
                                $("#ddlDepartmentFunding").val('');
                                $("#ddlDepartmentFunding").hide();
                                $("#ddlDepartmentFunding").parent().find('label').hide();

                                $("#txtDeptFundOth").val('');
                                $("#txtDeptFundOth").hide();
                                $("#txtDeptFundOth").parent().find('label').hide();

                                $('#chkDeptFundMouYes').prop('checked', false);
                                $('#chkDeptFundMouNo').prop('checked', false);
                                $('#divDeptFundMou').hide();

                            }

                        }
                        //                                                 ||||||

                        // --> Adds bitsum of current checkbox field
                        if (_checkBox.is(':checked')) {
                            _bitSum += parseInt(_bitValue, 10);
                        }

                        // --> Deselects all other checkboxes in current checkbox
                        //     if N/A is selected.
                        if (_name == 'N/A' && _checkBox.is(':checked')) {
                            ToggleTable(_table, true);
                        }
                        else if (_bitSum == 0) {
                            ToggleTable(_table, false);
                        }

                    });

                    //--> Puts BitSum total into hidden BitSum field.
                    $(_textBitSum).val(_bitSum);


                }
            }
        }

        // -- Disables all checkboxes if checkbox choice "N/A" is selected.
        function ToggleTable(tbl, isNA) {
            tbl.find('td').each(function () {
                var _checkBox = $(this).find(":input[name$='chkId']"),
                    _bitValue = $(this).find(":input[name$='BitValue']"),
                    _name = $(this).eq(0).text().trim();

                if (isNA) {
                    if (_name != "N/A") {
                        _checkBox.prop("checked", false);
                        _checkBox.prop("disabled", true);
                    }
                } else {
                    _checkBox.prop("disabled", false);
                }

            });
        }

        // ToggleDiv - shows section if check; otherwise remain hidden.
        function ToggleDiv(checkBox, theDiv) {
            if (checkBox.is(":checked")) {
                theDiv.show();
            }
            else {
                theDiv.hide();
            }
        }

        // ToggleDiv4 - Same functionality as 'ToggleDiv' with ability to handle four checkboxes.
        function ToggleDiv4(checkBox1, checkBox2, checkBox3, checkBox4, theDiv) {
            if (checkBox1.is(":checked")
                || checkBox2.is(":checked")
                || checkBox3.is(":checked")
                || checkBox4.is(":checked"))
                theDiv.show();
            else
                theDiv.hide();
        }


        $('form').validate({
            //onkeyup: function (element) { //turn off onkeyup validation for ajax checking fields
            //    var element_id = jQuery(element).attr('id');
            //    if (this.settings.rules[element_id].onkeyup !== false) {
            //        jQuery.validator.defaults.onkeyup.apply(this, arguments);
            //    }
            //},
            rules: {
                txtFirstName: {
                    required: true
                },
                txtLastName: {
                    required: true
                },
                ddlDegree: {
                    required: true
                },
                txtEmail: {
                    required: true,
                    email: true
                },
                txtPhone: {
                    required: true,
                    //digits: true
                },
                txtDept: {
                    required: true
                },
                ddlPIStatus: {
                    required: true
                },
                txtMentorFirstName: {
                    required: true
                },
                txtMentorLastName: {
                    required: true
                },
                txtMentorEmail: {
                    required: true,
                    email: true
                },
                txtProjectTitle: {
                    required: true
                },
                ddlUHGrant: {
                    required: true
                },
                txtGrantProposalFundingAgency:{
                    required: true
                }
                //,
                //txtCaptchaCode: {
                //    required: true,
                //    remote: function () {
                //        return {
                //            async: false,
                //            url: "ClientRequestForm.aspx/IsCaptchaCodeValid",
                //            type: "POST",
                //            contentType: "application/json; charset=utf-8",
                //            dataType: "json",
                //            data: JSON.stringify({ captchaCode: $('#txtCaptchaCode').val() }),
                //            dataFilter: function (data) {
                //                var msg = JSON.parse(data);
                //                if (msg.hasOwnProperty('d'))
                //                    return msg.d;
                //                else
                //                    return msg;
                //            }
                //        }
                //    }
                //}
            },
            messages: {
                txtCaptchaCode: {
                    remote: "Invalid Code"
                }
            },
            //submitHandler: function (form) {
            //    //alert('ajax being called...');            
            //},
            invalidHandler: function (event, validator) {
                //alert('number of invalid fields: ' + validator.numberOfInvalids());
            },
            highlight: function (element, errorClass) {
                $(element).closest('.form-group').addClass('has-error');
            },
            unhighlight: function (element, errorClass) {
                $(element).closest('.form-group').removeClass('has-error');
            },
            onkeyup: false,
            onblur: true,
            onfocusout: function (element) {
                if (element.id != "txtCaptchaCode")
                    $(element).valid();
            }
        });

        //function GetCheckResult() {
        //    var captchaCode = $("#txtCaptchaCode").val();

        //    $.ajax({
        //        type: "POST",
        //        url: "ClientRequestForm.aspx/CheckCaptchaCode",
        //        data: '{"captchaCode":"' + captchaCode + '"}',
        //        contentType: "application/json; charset=utf-8",
        //        dataType: "json",
        //        success: function (rtndata) {
        //            $("#txtCaptchaCode").addClass('has-error');
        //            if (JSON.stringify(rtndata.d) == 'Valid Captcha Code') {
        //                this.submit();
        //            }
        //            else {
        //                //$("#txtCaptchaCode").closest('.form-group').addClass('has-error');
        //                return captchaCode;
        //            }
        //        },
        //        error: function (xhr, status, err) {
        //            var err = eval("(" + xhr.responseText + ")");
        //            alert(err.Message);

        //        }
        //    });
        //}

        var site_key = '<%= ConfigurationManager.AppSettings["captchaSiteKey"]%>';
        var renderRecaptcha = function () {
            grecaptcha.render('ReCaptchaContainer', {
                'sitekey': site_key,
                'callback': reCaptchaCallback,
                theme: 'light',
                type: 'image',
                size: 'normal'
            });
        };

        var reCaptchaCallback = function (response) {
            if (response !== '') {
                $('#lblRecaptchaMessage').css('color', 'green').html('Success');
            }
        };

        $('button[type="button"]').click(function (e) {
            var message = 'Please check the checkbox';
            if (typeof (grecaptcha) != 'undefined') {
                //var response = grecaptcha.getResponse();
                (grecaptcha.getResponse().length === 0) ? (message = 'Captcha verification failed') : (message = 'Success!');
            }
            $('#lblrecaptchaMessage').html(message);
            $('#lblrecaptchaMessage').css('color', (message.toLowerCase() == 'success!') ? "green" : "red");
        });

        //function ShowWarningModal() {
        //    $('#btnShowWarningModal').click();
        //}

        function ClientSideClick(myButton) {
            if (myButton.getAttribute('type') == 'button') {
                myButton.disabled = true;
                myButton.value = "Processing......";
            }
            return true;
        }

        function updateId(txtbox) {
            if (txtbox.Value == '') {
                if (txtbox.id == 'MainContent_txtDept') {
                    $('#MainContent_txtDeptAffilId').val('');
                } else if (txtbox.id == 'MainContent_txtPreferBiostat') {
                    $('#MainContent_txtPreferBiostatId').val('');
                }
            }
        }

        //-----------------

        var substringMatcher = function (strs) {
            return function findMatches(q, cb) {
                // an array that will be populated with substring matches
                var matches = [];

                // regex used to determine if a string contains the substring `q`
                var substrRegex = new RegExp(q, 'i');

                // iterate through the pool of strings and for any string that
                // contains the substring `q`, add it to the `matches` array
                $.each(strs, function (i, str) {
                    if (substrRegex.test(str)) {
                        matches.push(str);
                    }
                });

                cb(matches);
            };
        };

        var names = [];
        var map = {};

        var data = JSON.parse($('#textAreaDeptAffil').val());

        $.each(data, function (i, name) {
            map[name.Name] = name;
            names.push(name.Name);
        });

        //for (var step = 0; step < 3; step++) {
        //    alert(pinames[step]);
        //}

        $('.typeahead').typeahead({
            hint: true,
            highlight: true,
            minLength: 1
        },
            {
                name: 'name',
                limit: 10,
                source: substringMatcher(names)
                //,selected: function (item) {
                //    //selectedAffil = map[item.Id]; alert(selectedAffil);
                //    $('#txtSelectedAffil').val(map[item].Id);
                //    return item;
                //}
                , updater: function (item) {
                    alert(map[item].Name);
                }
            }).on('typeahead:selected', function (event, selection) {
                //$(this).val(map[selection].Name);
                $('#txtDeptAffilId').val(map[selection].Id);

                // clearing the selection requires a typeahead method
                //$(this).typeahead('setQuery', '');

            });


        // [Converting QHS Faculty/Staff to dropdown - Typeable dropdown no longer necessary]
        //var memberNames = [];
        //var memberMap = {};

        //var memberData = JSON.parse($('#textAreaMembers').val());

        //$.each(memberData, function (i, name) {
        //    memberMap[name.Name] = name;
        //    memberNames.push(name.Name);
        //});

        ////for (var step = 0; step < 3; step++) {
        ////    alert(pinames[step]);
        ////}

        //$('.nameahead').typeahead({
        //    hint: true,
        //    highlight: true,
        //    minLength: 1
        //},
        //    {
        //        name: 'name',
        //        limit: 10,
        //        source: substringMatcher(memberNames)
        //        //,selected: function (item) {
        //        //    //selectedAffil = map[item.Id]; alert(selectedAffil);
        //        //    $('#txtSelectedAffil').val(map[item].Id);
        //        //    return item;
        //        //}
        //        //, updater: function (item) {
        //        //    alert(memberMap[item].Name);
        //        //}
        //    }).on('typeahead:selected', function (obj, datum) {
        //        //$(this).val(map[selection].Name);
        //        $('#txtPreferBiostatId').val(memberMap[datum].Id);

        //        // clearing the selection requires a typeahead method
        //        //$(this).typeahead('setQuery', '');

        //    });


    </script>
</body>
</html>
