<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RmatrixMonthly.aspx.cs" Inherits="ProjectManagement.Report.RmatrixMonthly" %>

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
            <b>RMATRIX Monthly Report</b>
        </div>

        <div class="panel-body">
            <br />
            <div>
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
                    <div class="col-md-2 offset1">
                        <asp:CheckBox ID="chkRmatrix" runat="server" Text="RMATRIX request"></asp:CheckBox>
                    </div>
                    <div class="col-md-1 text-right">
                        <asp:Button ID="btnSumbit" runat="server" Text="Submit" CssClass="btn btn-info" OnClick="btnSumbit_Click" OnClientClick="ClientSideClick(this)" UseSubmitBehavior="False" />
                    </div>
                </div>
                <%--<div class="row">
                    <div class="col-md-1">
                        <label class="control-label" for="txtReportDate">Report Month</label>
                    </div>
                    <div class="col-md-2">
                        <div class='input-group date' id='dtpReportDate'>
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-calendar"></span>
                            </span>
                            <asp:TextBox ID="txtReportDate" runat="server" class="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>--%>
            </div>
            <hr />

            <div class="row" id="divReport" runat="server" visible="false">
                <div class="col-md-12">
                    <div class="innerContainer">
                        <table class="table table-striped table-hover table-bordered" id="rmatrixMonthly">
                            <thead>
                                <tr>
                                    <th class="col-sm-1">Id</th>
                                    <th>PI Name</th>
                                    <th>Title</th>
                                    <th>Grant</th>
                                    <th>Lead</th>
                                    <th>Member</th>
                                    <th>Service Category</th>
                                    <th>Initial Date</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rptRmatrixMonthly" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td><%# Eval("Id") %></td>
                                            <td><%# Eval("PIName") %></td>
                                            <td><%# Eval("Title") %></td>
                                            <td><%# Eval("GrantName") %></td>
                                            <td><%# Eval("LeadBio") %></td>
                                            <td><%# Eval("Member") %></td>
                                            <td><%# Eval("ServiceCategory") %></td>
                                            <td><%# Eval("Initialdate") %></td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <tr>
                                            <td>Total</td>
                                            <td>
                                                <asp:Label ID="lblTotal" runat="server" /></td>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                    </FooterTemplate>
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
    <script type="text/javascript" src="../Scripts/jquery.floatThead.js"></script>
    <script type="text/javascript">
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

            var $table = $('table');
            $table.floatThead({
                scrollContainer: function ($table) {
                    return $table.closest('.innerContainer');
                }
            });

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

        function ClientSideClick(myButton) {
            //make sure the button is not of type "submit" but "button"
            if (myButton.getAttribute('type') == 'button') {
                // disable the button                
                myButton.disabled = true;
                //myButton.className = "btn-inactive";
                myButton.value = "Processing......";
            }
            return true;
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
