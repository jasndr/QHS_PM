<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdhocReport.aspx.cs" Inherits="ProjectManagement.Report.AdhocReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<script src="../Scripts/bootstrap.min.js"></script>--%>    
    <script src="../Scripts/bootstrap-datepicker.min.js"></script>
    <script src="../Scripts/jquery.cookie.js"></script>
    <script src="../Scripts/typeahead.jquery.min.js"></script> 
    
     <style>
        .twitter-typeahead{
             width: 100%;
        }
        .tt-menu{
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%--<div class="jumbotron">--%>
    <br />
    <div>
    <div class="panel panel-default">
        <div class="panel-heading">
            <div>
                <label>Report Type: </label>&nbsp;
                <label class="radio-inline">
                    <input type="radio" name="optionsRadios" id="optionsRadios1" value="option1"  class="radio-inline" checked>
                    Time Entry
                </label>
                <label class="radio-inline">
                    <input type="radio" name="optionsRadios" id="optionsRadios2" value="option2"  class="radio-inline">
                    Project
                </label>
                <label class="radio-inline">
                    <input type="radio" name="optionsRadios" id="optionsRadios3" value="option3"  class="radio-inline">
                    Paper
                </label>
                <label class="radio-inline">
                    <input type="radio" name="optionsRadios" id="optionsRadios4" value="option4"  class="radio-inline">
                    Academic
                </label>                  
            </div> 
        </div>

        <div class="panel-body">
            <div class="row">
                <div class="col-md-1">
                     Biostats:  
                </div>
                <div class="col-md-5">      
                        <asp:DropDownList ID="ddlBiostat" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                </div>
                <div class="col-md-3">
                    <asp:Button ID="btnSubmit" runat="server" Text="Get Report" class="btn btn-info" OnClick="btnSubmit_Click" OnClientClick="ClientSideClick(this)" UseSubmitBehavior="False" />                   
                </div>
                <div class="hidden">
                    <textarea id="textAreaAffil" rows="3" runat="server"></textarea>
                </div>    
            </div>
            <hr />
            <div id="divAcademic">
                <div class="row">
                   <%-- <div class="col-xs-6 col-md-1">
                        <label class="col-sm-2 control-label">From</label>    
                    </div> --%>             
                    <div class="col-md-3">From Date:
                        <div class='input-group date' id='dtAcademicFrom'>
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-calendar"></span>
                            </span>
                            <asp:TextBox ID="txtAcademicFrom" runat="server" class="form-control" ></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-3">To Date:
                        <div class='input-group date' id='dtAcademicTo'>
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-calendar"></span>
                            </span>
                            <asp:TextBox ID="txtAcademicTo" runat="server" class="form-control" ></asp:TextBox>
                        </div>
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-md-3">Academic Type:
                        <asp:DropDownList ID="ddlAcademicType" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                </div>
            </div>

            <div id="divTimeEntry">
                <div class="row">
                   <%-- <div class="col-xs-6 col-md-1">
                        <label class="col-sm-2 control-label">From</label>    
                    </div> --%>             
                    <div class="col-md-3">From Date:
                        <div class='input-group date' id='datetimepicker1'>
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-calendar"></span>
                            </span>
                            <asp:TextBox ID="TextBoxStartDate" runat="server" class="form-control" ></asp:TextBox>
                        </div>
                    </div>
                  <%--  <div class="col-xs-6 col-md-1">
                        <label class="col-sm-2 control-label">To</label>     
                    </div>--%>
                    <div class="col-md-3">To Date:
                        <div class='input-group date' id='datetimepicker2'>
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-calendar"></span>
                            </span>
                            <asp:TextBox ID="TextBoxEndDate" runat="server" class="form-control" ></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-6">Time Entry Report:
                        <asp:DropDownList ID="ddlReport" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>           
                </div>
                <br />
                <div class="row">
                    <div class="col-md-6">Project:
                        <asp:DropDownList ID="ddlProject" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                     <div class="col-md-6">Internal Projects Included:
                         <div class="row">
                            <div class="col-md-1">
                                <asp:CheckBox ID="chkInternalTimeEntry" runat="server" ></asp:CheckBox>
                            </div>
                         </div>
                    </div>                    
                </div>
            </div>
            
            <div id="divProject">
                <%--<label>Project Report </label>&nbsp;--%>
                <div class="row">         
                    <div class="col-md-3">Initial Date:
                        <div class='input-group date' id='datepickerInitial'>
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-calendar"></span>
                            </span>
                            <asp:TextBox ID="TextBoxInitialDate" runat="server" class="form-control" ></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-3">Complete Date:
                        <div class='input-group date' id='datepickerComplete'>
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-calendar"></span>
                            </span>
                            <asp:TextBox ID="TextBoxCompleteDate" runat="server" class="form-control" ></asp:TextBox>
                        </div>
                    </div>                    
                    <div class="col-xs-6 col-md-2">
                        Scope:
                        <asp:DropDownList ID="ddlServiceType" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                    <div class="col-xs-6 col-md-2">
                        Threshold:
                        <asp:DropDownList ID="ddlThreshold" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>   
                    <div class="col-md-2">Member:
                        <asp:DropDownList ID="ddlMember" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>         
                </div>
                <br />
                <div class="row">
                    <div class="col-md-6">Affiliation:                                   
                        <asp:TextBox ID="txtPIAffil" runat="server" placeholder="" class="form-control typeahead"></asp:TextBox>
                    </div>
                    <div class="col-md-1">Ongoing:
                        <asp:CheckBox ID="chkOngoing" runat="server" ></asp:CheckBox>
                    </div>
                    <div class="col-md-1">Internal:
                        <asp:CheckBox ID="chkInternal" runat="server" ></asp:CheckBox>
                    </div>
                    <div class="col-md-1">PilotGrant:
                        <asp:CheckBox ID="chkPilotGrant" runat="server" OnClick="IsPilotGrant(this);" ></asp:CheckBox>
                    </div>
                    <div id="divPilotGrant" class="col-md-3">Pilot Grant:                                   
                        <asp:TextBox ID="txtPilotGrant" runat="server" placeholder="semicolon separator" class="form-control"></asp:TextBox>
                    </div>                    
                </div>
                <br />
                <div class="row hidden">
                    <div class="col-md-6">Grant:                                   
                        <asp:TextBox ID="txtGrant" runat="server" placeholder="semicolon separator" class="form-control"></asp:TextBox>
                    </div>
                     <div class="col-xs-6 col-md-2">RMATRIX:
                        <select class="form-control" id="ddlRmatrix" runat="server">
                            <option value="Monthly">Monthly</option>
                            <option value="Summary">Summary</option>
                        </select>
                    </div>
                    <div class="col-md-3" style="padding-top: 20px">
                        <asp:Button ID="btnRmatrixMonthly" runat="server" Text="Download" class="btn btn-info" OnClick="btnRmatrixMonthly_Click" OnClientClick="blockUIForDownload(this)" UseSubmitBehavior="False" />                   
                    </div>                                       
                </div>
            </div>
            
            <div id="divPaper">
                <div>Type:&nbsp;
                    <label class="radio-inline">
                        <input type="radio" name="optionsPubs" id="optionsPubs1" value="paper"  class="radio-inline" checked>
                        Paper
                    </label>
                    <label class="radio-inline">
                        <input type="radio" name="optionsPubs" id="optionsPubs2" value="abstract"  class="radio-inline">
                        Abstract
                    </label>            
                </div> 
                <br />
                <div class="row">
                    <div class="col-md-6">
                        Grant:&nbsp;                   
                        <asp:CheckBox ID="chkRMATRIX" runat="server" class="checkbox-inline" Text="RMATRIX"></asp:CheckBox>
                        <asp:CheckBox ID="chkG12" runat="server" class="checkbox-inline" Text="G12 BRIDGES"></asp:CheckBox>
                        <asp:CheckBox ID="chkINBRE" runat="server" class="checkbox-inline" Text="INBRE"></asp:CheckBox>
                        <asp:CheckBox ID="chkMWest" runat="server" class="checkbox-inline" Text="Mountain West"></asp:CheckBox>
                    </div>
                    <div class="col-md-2">
                        Has PMCID:
                        <asp:CheckBox ID="chkPMCID" runat="server"></asp:CheckBox>
                    </div>
                    <div class="col-md-2">
                        No PMCID:
                        <asp:CheckBox ID="chkNoPMCID" runat="server"></asp:CheckBox>
                    </div>
                     <div class="col-md-2">
                        Citation Format:
                        <asp:CheckBox ID="chkCitation" runat="server"></asp:CheckBox>
                    </div>
                </div>
                <br />
                 <div class="row">
                    <div class="col-md-6" id="the-basics">Author Affiliation:                                   
                        <asp:TextBox ID="txtAuthorAffil" runat="server" class="form-control typeahead" ></asp:TextBox>
                    </div>
                </div>   
                <br />
                <div class="row"> 
                    <div class="col-md-3">Status:&nbsp;
                        <asp:DropDownList ID="ddlPaperStatus" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                    <div class="col-md-3">From Date:
                        <div class='input-group date' id='datepickerPaperFrom'>
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-calendar"></span>
                            </span>
                            <asp:TextBox ID="TextBoxPaperFromDate" runat="server" class="form-control" ></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-3">To Date:
                        <div class='input-group date' id='datepickerPaperTo'>
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-calendar"></span>
                            </span>
                            <asp:TextBox ID="TextBoxPaperToDate" runat="server" class="form-control" ></asp:TextBox>
                        </div>
                    </div>                                        
                </div>
                           
            </div>   

        </div>
    </div>

    <div class="panel panel-default">
        <div class="panel-heading">Report</div>
        <div class="panel-body">                   
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt" Height="500" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="100%">
            <%--<LocalReport ReportEmbeddedResource="ProjectManagement.Report.Report1.rdlc">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DataSet1" />
                </DataSources>
            </LocalReport>--%>
            </rsweb:ReportViewer>
            <%--<asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetData" TypeName="DSIndividualTableAdapters.Rpt_IndividuaL_TotalTableAdapter"></asp:ObjectDataSource>--%>

             
        </div>
    </div>
    
    </div>
    <asp:HiddenField ID="HiddenFieldCurrentDate" Value="" runat="server" />
    <input type="hidden" id="download_token" runat="server"/>
    
    <script type="text/javascript">
        function pageLoad(sender, args) {
            $('#li_report').addClass('selected');

            $(function () {
                $('#datetimepicker1').datepicker({
                    format: "mm/dd/yyyy",
                    autoclose: true,
                    orientation: "top",
                    todayHighlight: true
                });

                $('#datetimepicker2').datepicker({
                    format: "mm/dd/yyyy",
                    autoclose: true,
                    orientation: "top",
                    todayHighlight: true,
                });

                $('#datepickerInitial').datepicker({
                    format: "mm/dd/yyyy",
                    autoclose: true,
                    orientation: "top",
                    todayHighlight: true
                });

                $('#datepickerComplete').datepicker({
                    format: "mm/dd/yyyy",
                    autoclose: true,
                    orientation: "top",
                    todayHighlight: true,
                });

                $('#datepickerPaperFrom').datepicker({
                    format: "mm/dd/yyyy",
                    autoclose: true,
                    orientation: "top",
                    todayHighlight: true,
                });

                $('#datepickerPaperTo').datepicker({
                    format: "mm/dd/yyyy",
                    autoclose: true,
                    orientation: "top",
                    todayHighlight: true,
                });

                $('#dtAcademicFrom').datepicker({
                    format: "mm/dd/yyyy",
                    autoclose: true,
                    orientation: "top",
                    todayHighlight: true
                });

                $('#dtAcademicTo').datepicker({
                    format: "mm/dd/yyyy",
                    autoclose: true,
                    orientation: "top",
                    todayHighlight: true,
                });
            });

            if (document.getElementById('optionsRadios1').checked) {
                $('#divProject').hide();
                $('#divPaper').hide();
                $('#divAcademic').hide();
            }
            else if (document.getElementById('optionsRadios2').checked) {
                $('#divTimeEntry').hide();
                $('#divPaper').hide();
                $('#divAcademic').hide();
            }
            else if (document.getElementById('optionsRadios3').checked) {
                $('#divTimeEntry').hide();
                $('#divProject').hide();
                $('#divAcademic').hide();
            }
            else if (document.getElementById('optionsRadios4').checked) {
                $('#divTimeEntry').hide();
                $('#divProject').hide();
                $('#divPaper').hide();
            }

            $('input[type=radio][name=optionsRadios]').change(function () {
                if (this.value == 'option1') {
                    $('#divTimeEntry').show();
                    $('#divProject').hide();
                    $('#divPaper').hide();
                    $('#divAcademic').hide();
                }
                else if (this.value == 'option2') {
                    $('#divTimeEntry').hide();
                    $('#divProject').show();
                    $('#divPaper').hide();
                    $('#divAcademic').hide();
                }
                else if (this.value == 'option3') {
                    $('#divTimeEntry').hide();
                    $('#divProject').hide();
                    $('#divAcademic').hide();
                    $('#divPaper').show();
                }
                else if (this.value == 'option4') {
                    $('#divTimeEntry').hide();
                    $('#divProject').hide();
                    $('#divPaper').hide();
                    $('#divAcademic').show();
                }

                $("#<%=ReportViewer1.ClientID%>").hide();
            });

            if (document.getElementById('MainContent_chkPilotGrant').checked) {
                $('#divPilotGrant').show();
            }
            else {
                $('#divPilotGrant').hide();
            }

        }

        function IsPilotGrant(e) {
            if (e.id == 'MainContent_chkPilotGrant') {
                if (e.checked)
                    $('#divPilotGrant').show();
                else {
                    //$('#MainContent_GridViewGrant_txtGrantNameNew').val('');
                    $('#divPilotGrant').hide();
                }
            }
        }

        function ClientSideClick(myButton) {
            // Client side validation
            //if (typeof (Page_ClientValidate) == 'function') {
            //    if (Page_ClientValidate() == false)
            //    { return false; }
            //}

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
            $('#MainContent_btnRmatrixMonthly').prop("disabled", true);
            $('#MainContent_btnRmatrixMonthly').val("Downloading");
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
            $('#MainContent_btnRmatrixMonthly').prop("disabled", false);
            $('#MainContent_btnRmatrixMonthly').val("Download");
        }

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

        var data = JSON.parse($('#MainContent_textAreaAffil').val());

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
        }).on('typeahead:selected', function (event, selection) {
            $(this).val(map[selection].Name);

            // clearing the selection requires a typeahead method
            //$(this).typeahead('setQuery', '');
        });


    </script>
</asp:Content>
