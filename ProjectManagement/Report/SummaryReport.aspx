<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SummaryReport.aspx.cs" Inherits="ProjectManagement.Report.SummaryReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .innerContainer {
            max-height: 75vh;
            overflow: auto;
        }

        table thead {
            background: white;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel panel-default">
        <div class="panel-heading">
            <b>RMATRIX / Ola HAWAII <span id="collabCtrSectionTitle" runat="server">/ Collaboration Center </span>Summary Report</b>
        </div>

        <div class="panel-body">
            <%--<br />--%>
            <div class="row">
                <div class="col-md-1">
                    <label>Report Type: </label>
                </div>
                <%--&nbsp;--%>
                <div id="grantButtonSection" class="col-md-1" runat="server">
                    <label class="radio-inline">
                        <input type="radio" name="optionsRadios" id="optionsRadios1" value="option1" class="radio-inline" checked>
                        Grant
                    </label>
                </div>
                <div id="collabCtrButtonSection" class="col-md-2" runat="server">
                    <label class="radio-inline">
                        <input type="radio" name="optionsRadios" id="optionsRadios2" value="option2" class="radio-inline">
                        Collaboration Center
                    </label>
                </div>
            </div>
            <br />
            <div>
                <div class="row" id="divGrantDropDown">
                    <div class="col-md-1 text-right">
                        <label class="control-label">Grant:</label>
                    </div>
                    <div class="col-md-2">
                        <asp:DropDownList ID="ddlGrantType" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                </div>
                <div class="row" id="divCollabCtrDropDown">
                    <div class="col-md-1 text-right">
                        <label class="control-label">Collaboration Center:</label>
                    </div>
                    <div class="col-md-3">
                        <asp:DropDownList ID="ddlCollabCenter" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-md-1 text-right">
                        <label class="control-label">From Date:</label>
                    </div>
                    <div class="col-md-2">
                        <div class="input-group date" id="dtpFromDate">
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-calendar"></span>
                            </span>
                            <asp:TextBox ID="txtFromDate" runat="server" class="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-1 text-right">
                        <label class="control-label">To Date:</label>
                    </div>
                    <div class="col-md-2">
                        <div class="input-group date" id="dtpToDate">
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-calendar"></span>
                            </span>
                            <asp:TextBox ID="txtToDate" runat="server" class="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-1 text-right">
                        <label class="control-label">Report Type:</label>
                    </div>
                    <div class="col-md-2">
                        <asp:DropDownList ID="ddlReport" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                    <div class="col-md-1 text-right">
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-info" OnClick="btnSubmit_Click" OnClientClick="ClientSideClick(this)" UseSubmitBehavior="False" />
                    </div>
                </div>
            </div>
            <hr />
            <div class="row" id="divProject">
                <div class="col-md-12">
                    <div class="innerContainer">
                        <table class="table table-striped table-hover table-bordered" id="rmatrixSummary" style="width: 30% !important">
                            <thead>
                                <tr>
                                    <th colspan="16"><% =GrantName %> <% =ReportType %> Report from <% =FromDate %> to <% =ToDate %></th>
                                </tr>
                                <tr>
                                    <th class="col-sm-1">ProjectId</th>
                                    <th class="col-sm-1">First Name</th>
                                    <th class="col-sm-1">Last Name</th>
                                    <th class="col-sm-1">Affiliation</th>
                                    <th class="col-sm-2">Project Title</th>
                                    <th class="col-sm-1">Initial Date</th>
                                    <th class="col-sm-1">Completion Date</th>
                                    <th class="col-sm-1">Service Category</th>
                                    <th class="col-sm-1">Study Area</th>
                                    <th class="col-sm-1">Study Type</th>
                                    <th class="col-sm-1">Study Population</th>
                                    <th>Pilot PI</th>
                                    <th>New PI</th>
                                    <th>New Project</th>
                                    <th>Project</th>
                                    <th>Total Hours</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rptSummaryProject" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td><%# Eval("ProjectId") %></td>
                                            <td><%# Eval("FirstName") %></td>
                                            <td><%# Eval("LastName") %></td>
                                            <td><%# Eval("Affiliation") %></td>
                                            <td><%# Eval("ProjectTitle") %></td>
                                            <td><%# Eval("InitialDate") %></td>
                                            <td><%# Eval("CompletionDate") %></td>
                                            <td><%# Eval("Service") %></td>
                                            <td><%# Eval("StudyArea") %></td>
                                            <td><%# Eval("StudyType") %></td>
                                            <td><%# Eval("StudyPopulation") %></td>
                                            <td><%# Eval("IsPilotPI") %></td>
                                            <td><%# Eval("IsNewPI") %></td>
                                            <td><%# Eval("IsNewProject") %></td>
                                            <td><%# Eval("IsProject") %></td>
                                            <td><%# Eval("TotalHours") %></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
            <div class="row" id="divProjectOla">
                <div class="col-md-12">
                    <div class="innerContainer">
                        <table class="table table-striped table-hover table-bordered" id="olaHawaiiSummary" style="width: 30% !important">
                            <thead>
                                <tr>
                                    <th colspan="20"><% =GrantName %> <% =ReportType %> Report from <% =FromDate %> to <% =ToDate %></th>
                                </tr>
                                <tr>
                                    <th class="col-sm-1">ProjectId</th>
                                    <th class="col-sm-1">First Name</th>
                                    <th class="col-sm-1">Last Name</th>
                                    <th class="col-sm-1">Affiliation</th>
                                    <th class="col-sm-2">Project Title</th>
                                    <th class="col-sm-1">Initial Date</th>
                                    <th class="col-sm-1">Completion Date</th>
                                    <th class="col-sm-1">Service Category</th>
                                    <th class="col-sm-1">Study Area</th>
                                    <th class="col-sm-1">Study Type</th>
                                    <th class="col-sm-1">Study Population</th>
                                    <th>Ola Hawaii Request</th>
                                    <th>Ola Project</th>
                                    <th>Internal Project</th>
                                    <th>Pilot/Junior Investigator</th>
                                    <th>Health Disparity</th>
                                    <th>New PI</th>
                                    <th>New Project</th>
                                    <th>Project</th>
                                    <th>Total Hours</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rptSummaryProjectOla" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td><%# Eval("ProjectId") %></td>
                                            <td><%# Eval("FirstName") %></td>
                                            <td><%# Eval("LastName") %></td>
                                            <td><%# Eval("Affiliation") %></td>
                                            <td><%# Eval("ProjectTitle") %></td>
                                            <td><%# Eval("InitialDate") %></td>
                                            <td><%# Eval("CompletionDate") %></td>
                                            <td><%# Eval("Service") %></td>
                                            <td><%# Eval("StudyArea") %></td>
                                            <td><%# Eval("StudyType") %></td>
                                            <td><%# Eval("StudyPopulation") %></td>
                                            <td><%# Eval("IsOlaHawaiiRequest") %></td>
                                            <td><%# Eval("IsOlaSubmitted") %></td>
                                            <td><%# Eval("IsOlaReported") %></td>
                                            <td><%# Eval("IsPilotPI") %></td>
                                            <td><%# Eval("IsHealthDisparity") %></td>
                                            <td><%# Eval("IsNewPI") %></td>
                                            <td><%# Eval("IsNewProject") %></td>
                                            <td><%# Eval("IsProject") %></td>
                                            <td><%# Eval("TotalHours") %></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
            <div class="row" id="divPub">
                <div class="col-md-12">
                    <div class="innerContainer">
                        <table class="table table-striped table-hover table-bordered" id="summaryPub">
                            <thead>
                                <tr>
                                    <th colspan="15"><% =GrantName %> <% =ReportType %> Report from <% =FromDate %> to <% =ToDate %></th>
                                </tr>
                                <tr>
                                    <th>Paper Id</th>
                                    <th>Project Id</th>
                                    <th>Status</th>
                                    <th>Date</th>
                                    <th>Authors</th>
                                    <th>Title</th>
                                    <th>HealthData</th>
                                    <th>Journal</th>
                                    <th>Pub Year</th>
                                    <th>Volume</th>
                                    <th>Issue</th>
                                    <th>Pages</th>
                                    <th>DOI</th>
                                    <th>PMID</th>
                                    <th>PMCID</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rptSummaryPub" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td><%# Eval("Id") %></td>
                                            <td><%# Eval("ProjectId") %></td>
                                            <td><%# Eval("PubStatus") %></td>
                                            <td><%# Eval("StatusDate") %></td>
                                            <td><%# Eval("Author") %></td>
                                            <td><%# Eval("Title") %></td>
                                            <td><%# Eval("HealthData") %></td>
                                            <td><%# Eval("JournalName") %></td>
                                            <td><%# Eval("PubYear") %></td>
                                            <td><%# Eval("Volume") %></td>
                                            <td><%# Eval("Issue") %></td>
                                            <td><%# Eval("Page") %></td>
                                            <td><%# Eval("DOI") %></td>
                                            <td><%# Eval("PMID") %></td>
                                            <td><%# Eval("PMCID") %></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
            <div class="row" id="divPubOla">
                <div class="col-md-12">
                    <div class="innerContainer">
                        <table class="table table-striped table-hover table-bordered" id="summaryPubOla">
                            <thead>
                                <tr>
                                    <th colspan="18"><% =GrantName %> <% =ReportType %> Report from <% =FromDate %> to <% =ToDate %></th>
                                </tr>
                                <tr>
                                    <th>Paper Id</th>
                                    <th>Project Id</th>
                                    <th>Status</th>
                                    <th>Date</th>
                                    <th>Authors</th>
                                    <th>Title</th>
                                    <th>HealthData</th>
                                    <th>Journal</th>
                                    <th>Pub Year</th>
                                    <th>Volume</th>
                                    <th>Issue</th>
                                    <th>Pages</th>
                                    <th>DOI</th>
                                    <th>PMID</th>
                                    <th>PMCID</th>
                                    <th>Ola HAWAII Funded</th>
                                    <th>Ola HAWAII Acknowledged</th>
                                    <th>Ola Hawaii Request for Resources</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rptSummaryPubOla" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td><%# Eval("Id") %></td>
                                            <td><%# Eval("ProjectId") %></td>
                                            <td><%# Eval("PubStatus") %></td>
                                            <td><%# Eval("StatusDate") %></td>
                                            <td><%# Eval("Author") %></td>
                                            <td><%# Eval("Title") %></td>
                                            <td><%# Eval("HealthData") %></td>
                                            <td><%# Eval("JournalName") %></td>
                                            <td><%# Eval("PubYear") %></td>
                                            <td><%# Eval("Volume") %></td>
                                            <td><%# Eval("Issue") %></td>
                                            <td><%# Eval("Page") %></td>
                                            <td><%# Eval("DOI") %></td>
                                            <td><%# Eval("PMID") %></td>
                                            <td><%# Eval("PMCID") %></td>
                                            <td><%# Eval("IsOlaFunded") %></td>
                                            <td><%# Eval("IsOlaAcknowledged") %></td>
                                            <td><%# Eval("IsOlaRequest") %></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
            <div class="row" id="divAbstract">
                <div class="col-md-12">
                    <div class="innerContainer">
                        <table class="table table-striped table-hover table-bordered" id="summaryAbstract">
                            <thead>
                                <tr>
                                    <th colspan="12"><% =GrantName %> <% =ReportType %> Report from <% =FromDate %> to <% =ToDate %></th>
                                </tr>
                                <tr>
                                    <th>Paper Id</th>
                                    <th>Project Id</th>
                                    <th>Status</th>
                                    <th>Date</th>
                                    <th>Authors</th>
                                    <th>Title</th>
                                    <th>Conference</th>
                                    <th>Location</th>
                                    <th>Start Date</th>
                                    <th>End Date</th>
                                    <th>Type</th>
                                    <th>Grants</th>
                                </tr>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rptSummaryAbstract" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td><%# Eval("Id") %></td>
                                            <td><%# Eval("ProjectId") %></td>
                                            <td><%# Eval("PubStatus") %></td>
                                            <td><%# Eval("StatusDate") %></td>
                                            <td><%# Eval("Author") %></td>
                                            <td><%# Eval("Title") %></td>
                                            <td><%# Eval("conf") %></td>
                                            <td><%# Eval("ConfLoc") %></td>
                                            <td><%# Eval("StartDate") %></td>
                                            <td><%# Eval("EndDate") %></td>
                                            <td><%# Eval("pp") %></td>
                                            <td><%# Eval("GrantAffilName") %></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
            <div class="row" id="divAbstractOla">
                <div class="col-md-12">
                    <div class="innerContainer">
                        <table class="table table-striped table-hover table-bordered" id="summaryAbstractOla">
                            <thead>
                                <tr>
                                    <th colspan="15"><% =GrantName %> <% =ReportType %> Report from <% =FromDate %> to <% =ToDate %></th>
                                </tr>
                                <tr>
                                    <th>Paper Id</th>
                                    <th>Project Id</th>
                                    <th>Status</th>
                                    <th>Date</th>
                                    <th>Authors</th>
                                    <th>Title</th>
                                    <th>Conference</th>
                                    <th>Location</th>
                                    <th>Start Date</th>
                                    <th>End Date</th>
                                    <th>Type</th>
                                    <th>Grants</th>
                                    <th>Ola HAWAII Funded</th>
                                    <th>Ola HAWAII Acknowledged</th>
                                    <th>Ola Hawaii Request for Resources</th>
                                </tr>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rptSummaryAbstractOla" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td><%# Eval("Id") %></td>
                                            <td><%# Eval("ProjectId") %></td>
                                            <td><%# Eval("PubStatus") %></td>
                                            <td><%# Eval("StatusDate") %></td>
                                            <td><%# Eval("Author") %></td>
                                            <td><%# Eval("Title") %></td>
                                            <td><%# Eval("conf") %></td>
                                            <td><%# Eval("ConfLoc") %></td>
                                            <td><%# Eval("StartDate") %></td>
                                            <td><%# Eval("EndDate") %></td>
                                            <td><%# Eval("pp") %></td>
                                            <td><%# Eval("GrantAffilName") %></td>
                                            <td><%# Eval("IsOlaFunded") %></td>
                                            <td><%# Eval("IsOlaAcknowledged") %></td>
                                            <td><%# Eval("IsOlaRequest") %></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
            <div class="row" id="divAcademic">
                <div class="col-sm-12">
                    <div class="innerContainer">
                        <table class="table table-striped table-hover table-bordered" id="summaryAcademic">
                            <thead>
                                <tr>
                                    <th colspan="12"><% =GrantName %> <% =ReportType %> Report from <% =FromDate %> to <% =ToDate %></th>
                                </tr>
                                <tr>
                                    <th>Id</th>
                                    <th>Type</th>
                                    <th>Biostat</th>
                                    <th>Title</th>
                                    <th>Organization</th>
                                    <th>Course Number</th>
                                    <th>Number of Credits</th>
                                    <th>Semester</th>
                                    <th>Start Date</th>
                                    <th>End Date</th>
                                    <th>Number of Attendees</th>
                                    <th>Comments</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rptSummaryAcademic" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td><%# Eval("Id") %></td>
                                            <td><%# Eval("AcademicType") %></td>
                                            <td><%# Eval("Biostat") %></td>
                                            <td><%# Eval("Title") %></td>
                                            <td><%# Eval("Organization") %></td>
                                            <td><%# Eval("CourseNumber") %></td>
                                            <td><%# Eval("NumOfCredits") %></td>
                                            <td><%# Eval("Semester") %></td>
                                            <td><%# Eval("StartDate") %></td>
                                            <td><%# Eval("EndDate") %></td>
                                            <td><%# Eval("NumOfAttendees") %></td>
                                            <td><%# Eval("Comments") %></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-md-2">
                    <asp:Button ID="btnExportExcel" runat="server" Text="Download" CssClass="btn btn-info" OnClick="btnExportExcel_Click" OnClientClick="blockUIForDownload(this)" UseSubmitBehavior="False" />
                </div>
            </div>
        </div>
    </div>
    <input type="hidden" id="hdnRowCount" runat="server" />
    <input type="hidden" id="download_token" runat="server" />

    <script src="../Scripts/bootstrap-datepicker.min.js"></script>
    <script src="../Scripts/jquery.cookie.js"></script>
    <script src="../Scripts/jquery.floatThead.js"></script>
    <script type="text/javascript">

        function pageLoad(sender, args) {

            //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\\

            if (document.getElementById('optionsRadios1').checked) {
                $('#divCollabCtrDropDown').hide();
            }
            else if (document.getElementById('optionsRadios2').checked) {
                $('#divGrantDropDown').hide();
            }

            $('input[type=radio][name=optionsRadios]').change(function () {
                if (this.value == 'option1') {
                    $('#divGrantDropDown').show();
                    $('#divCollabCtrDropDown').hide();
                }
                else if (this.value == 'option2') {
                    $('#divGrantDropDown').hide();
                    $('#divCollabCtrDropDown').show();
                }
            });
            //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\\


        }



        $(document).ready(function () {
            //if ($("#MainContent_txtReportDate").val() == '') {
            //    var today = new Date();
            //    var mm = today.getMonth() + 1;
            //    if (mm < 10) mm = '0' + mm;
            //    var yy = today.getFullYear();
            //    $("#MainContent_txtReportDate").val(mm + '/' + yy);
            //}

            //var reportDate = new DatePicker('dtpReportDate');
            //reportDate.init();

            var fromDate = new DatePicker('dtpFromDate');
            fromDate.init();

            var toDate = new DatePicker('dtpToDate');
            toDate.init();

            if ($('#MainContent_hdnRowCount').val() > 0)
                $('#MainContent_btnExportExcel').prop("disabled", false);
            else {
                $('#MainContent_btnExportExcel').prop("disabled", true);
            }

            divToggle();

            /**___Fixed Header Scrollable Table____ */
            var $table = $('table');
            $table.floatThead({
                scrollContainer: function ($table) {
                    return $table.closest('.innerContainer');
                }
            });
            /**___Fixed Header Scrollable Table____ */
        });

        var DatePicker = function (ctrlId) {
            var ctl = ctrlId;

            return {
                init: function (e) {
                    $('#' + ctl).datepicker({
                        //starView: "year",
                        //viewMode: "months",
                        //minViewMode: "months",
                        format: "mm/dd/yyyy",
                        autoclose: true,
                        orientation: "top",
                        todayHighlight: true
                    });
                }
            }
        }



        function divToggle() {
            if ($('#MainContent_ddlReport').val() == '') {
                $('#divProject').hide();
                $('#divProjectOla').hide();
                $('#divPub').hide();
                $('#divAbstract').hide();
                $('#divPubOla').hide();
                $('#divAbstractOla').hide();
                $('#divAcademic').hide();
            }
            else if ($('#MainContent_ddlReport').val() >= 11 && $('#MainContent_ddlReport').val() <= 13) {

                if ($('#MainContent_ddlGrantType').val() != 2) {
                    $('#divProject').show();
                    $('#divProjectOla').hide();
                }
                else {
                    $('#divProject').hide();
                    $('#divProjectOla').show();
                }
                $('#divPub').hide();
                $('#divAbstract').hide();
                $('#divAcademic').hide();
                $('#divPubOla').hide();
                $('#divAbstractOla').hide();
            }
            else if ($('#MainContent_ddlReport').val() == 14) {

                if ($('#MainContent_ddlGrantType').val() != 2) {
                    $('#divPub').show();
                    $('#divPubOla').hide();
                }
                else {
                    $('#divPub').hide();
                    $('#divPubOla').show();
                }

                $('#divProject').hide();
                $('#divProjectOla').hide();
                //$('#divPub').show();
                $('#divAbstract').hide();
                $('#divAcademic').hide();
                //$('#divPubOla').hide();
                $('#divAbstractOla').hide();
            }
            else if ($('#MainContent_ddlReport').val() == 15) {

                if ($('#MainContent_ddlGrantType').val() != 2) {
                    $('#divAbstract').show();
                    $('#divAbstractOla').hide();
                }
                else {
                    $('#divAbstract').hide();
                    $('#divAbstractOla').show();
                }

                $('#divProject').hide();
                $('#divProjectOla').hide();
                $('#divPub').hide();
                //$('#divAbstract').show();
                $('#divAcademic').hide();
                $('#divPubOla').hide();
                //$('#divAbstractOla').hide();
            }
            else if ($('#MainContent_ddlReport').val() == 16) {
                $('#divProject').hide();
                $('#divProjectOla').hide();
                $('#divPub').hide();
                $('#divAbstract').hide();
                $('#divAcademic').show();
                $('#divPubOla').hide();
                $('#divAbstractOla').hide();
            }

        }

        function ClientSideClick(myButton) {
            //make sure the button is not of type "submit" but "button"
            if (myButton.getAttribute('type') == 'button') {
                // disable the button                
                myButton.disabled = true;
                //myButton.className = "btn-inactive";
                myButton.value = "Processing......";
            }
            return true;

            divToggle();
        }

        var fileDownloadCheckTimer;
        function blockUIForDownload(button) {
            var token = new Date().getTime(); //use the current timestamp as the token value
            $('#MainContent_download_token').val(token);
            $('#MainContent_btnExportExcel').prop("disabled", true);
            $('#MainContent_btnExportExcel').val("Downloading");
            //$(".pdsa-submit-progress").removeClass("hidden");
            fileDownloadCheckTimer = window.setInterval(function () {
                var cookieValue = $.cookie('fileDownloadToken');
                if (cookieValue <= token) {

                    finishDownload();
                }
            }, 3000);
        }

        function finishDownload() {
            window.clearInterval(fileDownloadCheckTimer);
            $.cookie('fileDownloadToken', null); //clears this cookie value
            //$(".pdsa-submit-progress").addClass("hidden");
            $('#MainContent_btnExportExcel').prop("disabled", false);
            $('#MainContent_btnExportExcel').val("Download");
        }

    </script>
</asp:Content>
