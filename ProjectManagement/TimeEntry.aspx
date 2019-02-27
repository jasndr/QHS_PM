<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TimeEntry.aspx.cs" Inherits="ProjectManagement.TimeEntry1" %>

<%@ Register Assembly="DropDownChosen" Namespace="CustomDropDown" TagPrefix="ucc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<script src="//code.jquery.com/ui/1.11.4/jquery-ui.js"></script>--%>
    <script src="Scripts/chosen.jquery.js"></script>
    <link href="Content/chosen.css" rel="stylesheet" />
    <%--<script src="Scripts/bootstrap.min.js"></script>--%>
    <script src="Scripts/bootstrap-datepicker.min.js"></script>
    <script src="Scripts/jquery.validate.min.js"></script>
    <script src="Scripts/WebForms/jquerymousewheel.js"></script>
    <style>
        .validateError {
            color: red;
            font-weight: bold;
        }

        #formIncomplete {
            color: darkred;
        }

        #MainContent_lblWarning {
            font-size: 16pt;
            font-weight: bold;
            color: red;
        }

        #textWarning {
            font-size: 14pt;
            font-weight: bold;
            color: blue;
        }

        #textConclusion {
            font-size: 12pt;
            color: darkolivegreen;
        }
        #MainContent_timeEntryReportType label{
                font-weight: normal;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%--<div id="surveyModal" class="modal fade">
      <div class="modal-dialog">
         <div class="modal-content">
            <div class="modal-header">
               <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
               <h4 class="modal-title">Confirmation</h4>
            </div>
            <div class="modal-body">
               <p>Are you sure you want to send survey to PI?</p>
               <p class="text-warning"><small>The survey can only be sent once.</small></p>
            </div>
            <div class="modal-footer">
               <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
               <asp:Button class="btn btn-primary" ID="btnSendSurvey" runat="server" Text="Submit"></asp:Button>
            </div>
         </div>
      </div>
   </div>--%>

    <div id="rootwizard" class="">
        <div class="panel panel-default">
            <div class="panel-heading">Add Time</div>
            <div class="panel-body">
                <div class="row form-group-md">
                    <div class="col-sm-2 text-left">
                        <label class="control-label" for="txtCCAbbrv">QHS Faculty/Staff:</label></div>
                    <div class="col-sm-3">
                        <asp:DropDownList ID="ddlBioStat" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlBioStat_Changed" AutoPostBack="True">
                        </asp:DropDownList>
                    </div>
                    <label class="col-sm-3 control-label" for="txtAgreementId">PI:</label>
                    <div class="col-sm-4">
                        <ucc:DropDownListChosen ID="ddlBiostatChosen" runat="server"
                            NoResultsText="No results match."
                            DataPlaceHolder="Search PI" AllowSingleDeselect="true">
                        </ucc:DropDownListChosen>
                    </div>
                </div>

                <%--<div class="row">                   
                    <div class="col-xs-6 col-md-3">
                        <asp:DropDownList ID="ddlPI" runat="server" CssClass="chosen-select" OnSelectedIndexChanged="ddlPI_Changed" AutoPostBack="True" AllowSingleDeselect="true" Visible="false">
                        </asp:DropDownList>                      
                    </div>
                    
                </div>--%>
                <br />

                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>

                        <div class="panel panel-primary">
                            <div class="panel-heading">Project Phase</div>
                            <div class="panel-body">

                                <div class="row">
                                    <div class="col-xs-11 col-md-11">
                                        <asp:DropDownList ID="ddlProject" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlProject_Changed" AutoPostBack="True">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-md-1"></div>
                                    <div class="col-md-10">
                                        <table class="table table-striped">
                                            <thead>
                                                <tr>
                                                    <th>Phase</th>
                                                    <th>Desc</th>
                                                    <th>Start Date</th>
                                                    <th>PhD est</th>
                                                    <th>PhD spt</th>
                                                    <th>MS est</th>
                                                    <th>MS spt</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <asp:Repeater ID="rptPhase" runat="server">
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td><%# Eval("Name") %></td>
                                                            <td><%# Eval("Desc") %></td>
                                                            <td><%# Eval("StartDate") %></td>
                                                            <td><%# Eval("PhdHrs") %></td>
                                                            <td><%# Eval("PhdSpt", "{0:0.00}") %></td>
                                                            <td><%# Eval("MsHrs") %></td>
                                                            <td><%# Eval("MsSpt") %></td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>

                                <%-- <div class="modal-body">--%>
                                <div class="row" id="divPhase">
                                    <div class="hidden">
                                        <asp:DropDownList ID="ddlPhaseHdn" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                    <%--<br />
                                    <div class="col-xs-12 col-md-12">
                                        <asp:GridView ID="gvPhase" runat="server" AutoGenerateColumns="False"
                                            ShowFooter="True"
                                            OnRowDataBound="gvPhase_RowDataBound"
                                            OnRowDeleting="gvPhase_RowDeleting"
                                            class="table table-striped table-bordered">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="RadioButton1" runat="server"
                                                            onclick="RadioCheck(this);" />
                                                        <asp:HiddenField ID="HiddenField1" runat="server"
                                                            Value='<%#Eval("Id")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Id" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblId" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Phase" HeaderStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPhase" runat="server" Text='<%# Eval("Name") %>' Visible="false" />
                                                        <asp:DropDownList ID="ddlPhase" runat="server" CssClass="form-control">
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                    <FooterStyle HorizontalAlign="Left" />
                                                    <FooterTemplate>
                                                        <asp:Button ID="btnAddPhase" runat="server" CssClass="btn btn-info"
                                                            Text="Add New" OnClick="btnAddPhase_Click" />
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Start Date" HeaderStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtStartDate" runat="server" class="form-control" Text='<%# Eval("StartDate") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Service Type" HeaderStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblServiceType" runat="server" Text='<%# Eval("ServiceTypeId") %>' Visible="false" />
                                                        <asp:DropDownList ID="ddlServiceType" runat="server" CssClass="form-control">
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Desc" HeaderStyle-Width="18%">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtTitle" runat="server" class="form-control" Text='<%# Eval("Title") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Ms Est" HeaderStyle-Width="8%">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtMsHrs" runat="server" class="form-control" Text='<%# Eval("MsHrs") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Ms Spt" HeaderStyle-Width="8%">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtMsSpt" runat="server" class="form-control" Text='<%# Eval("MsHrs") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Phd Est" HeaderStyle-Width="8%">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtPhdHrs" runat="server" class="form-control" Text='<%# Eval("PhdHrs") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Phd Spt" HeaderStyle-Width="8%">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtPhdSpt" runat="server" class="form-control" Text='<%# Eval("MsHrs") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Del" HeaderStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkDelete" runat="server" Text="Delete" CommandName="Delete"
                                                            ToolTip="Delete" OnClientClick='return confirm("Are you sure you want to delete this entry?");'
                                                            CommandArgument=''><img src="images/icon-delete.png" /></asp:LinkButton>
                                                    </ItemTemplate>

                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>

                                    </div>--%>
                                </div>
                                <%--</div>--%>

                                <%--<div class="panel panel-success">
                                    <div class="panel-heading">
                                        <a data-toggle="collapse" data-target="#collapseTwo"
                                            class="collapsed">Project Requested and Spent Time
                                        </a>
                                    </div>

                                    <div id="collapseTwo" class="panel-collapse collapse">
                                        <div class="panel-body">
                                            <div class="row">
                                                <div class="col-xs-10 col-md-10">
                                                    <asp:GridView ID="GridViewProjectEffort" runat="server" AutoGenerateColumns="False"
                                                        CssClass="table table-striped table-bordered"
                                                        ShowFooter="true">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Invoice Id" HeaderStyle-Width="15%" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblId" runat="server" Text='<%# Eval("InvoiceId") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Req Num" HeaderStyle-Width="15%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblProjectId" runat="server" Text='<%# Eval("ReqNum") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Req Date" HeaderStyle-Width="15%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblReqDate" runat="server" Text='<%# Eval("ReqDate") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="From Date" HeaderStyle-Width="15%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFromDate" runat="server" Text='<%# Eval("FromDate") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="To Date" HeaderStyle-Width="15%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblToDate" runat="server" Text='<%# Eval("ToDate") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Phd Req" HeaderStyle-Width="10%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPhdRequested" runat="server" Text='<%# Eval("PhdReq") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Phd Spt" HeaderStyle-Width="10%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPhdSpent" runat="server" Text='<%# Eval("PhdSpt") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Ms Req" HeaderStyle-Width="10%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblMsRequested" runat="server" Text='<%# Eval("MsReq") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Ms Spt" HeaderStyle-Width="10%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblMsSpent" runat="server" Text='<%# Eval("MsSpt") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-xs-10 col-md-10">

                                                    <div class="panel panel-default">
                                                        <div class="panel-body">
                                                            <div class="row">
                                                                <div class="col-xs-3 col-md-3">
                                                                    <div class='input-group date' id='datetimepicker3'>
                                                                        <span class="input-group-addon">
                                                                            <span class="glyphicon glyphicon-calendar"></span>
                                                                        </span>
                                                                        <input type="text" id="txtFromDate" class="form-control" placeholder="From Date">
                                                                    </div>
                                                                </div>
                                                                <div class="col-xs-3 col-md-3">
                                                                    <div class='input-group date' id='datetimepicker4'>
                                                                        <span class="input-group-addon">
                                                                            <span class="glyphicon glyphicon-calendar"></span>
                                                                        </span>
                                                                        <input type="text" id="txtToDate" class="form-control" placeholder="To Date">
                                                                    </div>
                                                                </div>
                                                                <div class="col-xs-2 col-md-2">
                                                                    <button type="button" class="btn btn-success" id="btnGetTime">Get Time</button>
                                                                </div>
                                                                <div class="col-xs-2 col-md-2">
                                                                    <input type="text" id="txtPhdTime" class="form-control" placeholder="phd time">
                                                                </div>
                                                                <div class="col-xs-2 col-md-2">
                                                                    <input type="text" id="txtMsTime" class="form-control" placeholder="ms time">
                                                                </div>
                                                            </div>

                                                        </div>
                                                    </div>
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>--%>
                            </div>
                        </div>

                        <div class="hidden">
                            <textarea id="textAreaPhase" cols="30" rows="3" runat="server"></textarea>
                        </div>

                        <div class="panel panel-info">
                            <div class="panel-heading">Time Entry</div>
                            <div class="panel-body">
                                <table class="table">
                                    <thead>
                                        <tr class="table-success">
                                            <th>Phase</th>
                                            <th>Service Type</th>
                                            <th>Date</th>
                                            <th>Time</th>
                                            <th>Description</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td class="col-md-2">
                                                <asp:DropDownList ID="ddlPhase" runat="server" CssClass="form-control">
                                                </asp:DropDownList>
                                            </td>
                                            <td class="col-md-2">
                                                <asp:DropDownList ID="ddlServiceType" runat="server" CssClass="form-control">
                                                </asp:DropDownList>
                                            </td>
                                            <td class="col-md-2">
                                                <div class='input-group date' id='datetimepicker1'>
                                                    <span class="input-group-addon">
                                                        <span class="glyphicon glyphicon-calendar"></span>
                                                    </span>
                                                    <asp:TextBox ID="TextBoxSubmitDate" runat="server" class="form-control"></asp:TextBox>
                                                </div>
                                            </td>
                                            <td class="col-md-1">
                                                <asp:TextBox ID="TextBoxTime" runat="server" class="form-control" Text="0.5" EnableViewState="true"></asp:TextBox>
                                                <asp:CustomValidator ID="TextBoxTimeValidator"
                                                    runat="server"
                                                    OnServerValidate="TextBoxTimeValidate"
                                                    ControlToValidate="TextBoxTime"
                                                    ErrorMessage="Over Estimated Hours!!!"
                                                    CssClass="validateError"
                                                    Display="Dynamic">
                                                </asp:CustomValidator>
                                            </td>
                                            <td class="col-md-5">
                                                <asp:TextBox ID="TextBoxDesc" runat="server" class="form-control"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>

                    </ContentTemplate>
                    <Triggers>
                        <%-- <asp:AsyncPostBackTrigger ControlID="rptInvoice" EventName="ItemCommand" />--%>
                        <asp:PostBackTrigger ControlID="btnSubmit" />
                        <asp:AsyncPostBackTrigger ControlID="ddlProject" EventName="SelectedIndexChanged" />
                        <%--<asp:AsyncPostBackTrigger ControlID="btnAddInvoiceItem" />--%>
                        <%--<asp:PostBackTrigger ControlID="btnSave" />--%>
                    </Triggers>
                </asp:UpdatePanel>

                <div class="row hidden">
                    <div class="col-xs-6 col-md-6">
                        Service Type
                        <%--<asp:DropDownList ID="ddlServiceType" runat="server" CssClass="form-control" >
                        </asp:DropDownList>--%>
                    </div>
                </div>

                <div class="row hidden">
                    <div class="col-xs-6 col-md-6">
                        Description                
                        <%--<asp:TextBox ID="TextBoxDesc" runat="server" class="form-control" TextMode="MultiLine" Rows="2"></asp:TextBox>--%>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-6 col-md-10"></div>
                    <div class="col-xs-6 col-md-2">
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="btn btn-info" OnClientClick="return validateControl();" OnClick="btnSubmit_Click" />
                        <button type="button" style="display: none;" id="btnShowWarningModal" class="btn btn-primary btn-lg"
                            data-toggle="modal" data-target="#overspentWarningModal">
                            Warning Modal</button>
                    </div>
                </div>
                <asp:HiddenField ID="HiddenFieldCurrentDate" Value="" runat="server" />

            </div>
        </div>

        <div id="overspentWarningModal" class="modal fade">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                        <img style="height: 100px; width: 100px; display: block; margin-left: auto; margin-right: auto;" src="images/Stop_sign.png" />
                        <br />
                        <h3 id="formIncomplete" class="modal-title">Estimated Hours Exceeded</h3>
                    </div>
                    <div class="modal-body">
                        <asp:Label ID="lblWarning" runat="server">WARNING: Your time entry hours <u><strong>have not</strong></u> been saved!!!</asp:Label><br />
                        <br />
                        <p class="text-warning" id="textWarning"></p>
                        <br />
                        <p class="text-conclusion" id="textConclusion">
                            For additional assistance, including inquiries about adding more 
                                                                   hours to the estimate, please contact Admin.  Mahalo!
                        </p>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="panel panel-info">
            <div class="panel-heading">Time Entry Monthly Report</div>
            <div class="panel-body">
                <div class="row">
                    <div class="col-md-4 col-xs-12">
                            <label>Report Type: </label>
                            <asp:RadioButtonList ID="timeEntryReportType" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                <asp:ListItem Text="Monthly" Value="option_Monthly" Selected="True" />
                                <asp:ListItem Text="Project" Value="option_Project" />
                            </asp:RadioButtonList>
                    </div>
                    <div class="col-md-6 col-xs-12">
                        <asp:Button ID="btnMonthly" runat="server" Text="Get Report" class="btn btn-info" OnClick="btnMonthly_Click" />
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-xs-6 col-md-2">
                        <div class='input-group date dateDropdown'>
                            <span class="input-group-addon">
                                <span class="glyphicon">Year</span>
                            </span>
                            <asp:DropDownList ID="ddlYear" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="col-xs-6 col-md-2">
                        <div class='input-group date dateDropdown'>
                            <span class="input-group-addon">
                                <span class="glyphicon">Month</span>
                            </span>
                            <asp:DropDownList ID="ddlMonth" runat="server" CssClass="form-control" Caption="">
                            </asp:DropDownList>
                        </div>
                    </div>

                    

                </div>
                <br />
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div>
                            <asp:GridView ID="GridViewTimeEntry" runat="server" AutoGenerateColumns="False" class="table table-striped"
                                OnRowDeleting="GridViewTimeEntry_RowDeleting"
                                OnRowCommand="GridViewTimeEntry_RowCommand"
                                OnRowDataBound="GridViewTimeEntry_RowDataBound" ShowFooter="true">
                                <Columns>
                                    <%--<asp:ButtonField CommandName="editRecord"
                                        ControlStyle-CssClass="btn btn-info" ButtonType="Button"
                                        Text="Edit" HeaderText="" />--%>
                                    <asp:TemplateField HeaderText="Id" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblId" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Project Id">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" ID="btnProject" CommandName="editRecord">
                                                <asp:Label ID="lblProjectId" runat="server" Text='<%# Eval("ProjectId") %>'></asp:Label>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Project">
                                        <ItemTemplate>
                                            <asp:Label ID="lblProjectName" runat="server" Text='<%# Eval("Title") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Project Phase">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPhase" runat="server" Text='<%# Eval("Phase") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Service Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lblServiceType" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDesc" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText=" Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDate" runat="server" Text='<%# Convert.ToDateTime(Eval("Date")).ToShortDateString() %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblTotalHour" runat="server" Text="Total Hour:" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText=" Hour">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTime" runat="server" Text='<%# Eval("Duration") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblTotalTime" runat="server" Text="Total Hour" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <%--<asp:LinkButton ID="lnkEdit" runat="server" Text="" CommandName="Edit" ToolTip="Edit"
                                CommandArgument=''><img src="images/icon-edit.gif" /></asp:LinkButton>--%>
                                            <asp:LinkButton ID="lnkDelete" runat="server" Text="Delete" CommandName="Delete"
                                                ToolTip="Delete" OnClientClick='return confirm("Are you sure you want to delete this entry?");'
                                                CommandArgument=''><img src="images/icon-delete.png" /></asp:LinkButton>
                                        </ItemTemplate>
                                        <%--<EditItemTemplate>
                            <asp:LinkButton ID="lnkInsert" runat="server" Text="" ValidationGroup="editGrp" CommandName="Update" ToolTip="Save"
                                CommandArgument=''><img src="images/icon-save.gif" /></asp:LinkButton>
                            <asp:LinkButton ID="lnkCancel" runat="server" Text="" CommandName="Cancel" ToolTip="Cancel"
                                CommandArgument=''><img src="images/icon-cancel.gif" /></asp:LinkButton>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:LinkButton ID="lnkInsert" runat="server" Text="" ValidationGroup="newGrp" CommandName="InsertNew" ToolTip="Add New Entry"
                                CommandArgument=''><img src="images/icon-save.gif" /></asp:LinkButton>
                            <asp:LinkButton ID="lnkCancel" runat="server" Text="" CommandName="CancelNew" ToolTip="Cancel"
                                CommandArgument=''><img src="images/icon-cancel.gif" /></asp:LinkButton>
                        </FooterTemplate>--%>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <asp:Label ID="Label10" runat="server" Text="Label">no time entries</asp:Label>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </div>
                    </ContentTemplate>
                    <%-- <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSubmit" />
                    </Triggers>--%>
                </asp:UpdatePanel>
                <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                    <ProgressTemplate>
                        <img src="" alt="Loading.. Please wait!" />
                    </ProgressTemplate>
                </asp:UpdateProgress>

                <br />
                <%--<asp:Button Text="Export" OnClick="ExportExcel" runat="server" visable="false"/>--%>
            </div>
        </div>

        <div id="editModal" class="modal fade" role="dialog" aria-labelledby="gridSystemModalLabel" aria-hidden="true"
            data-backdrop="static" data-keyboard="false">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 id="editModalLabel">Edit Time Entry</h4>
                    </div>
                    <asp:UpdatePanel ID="upEdit" runat="server">
                        <ContentTemplate>
                            <div class="modal-body">
                                <div class="row">
                                    <div class="col-xs-6 col-md-6">
                                        <asp:Label ID="lblEditId" runat="server" Visible="false"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-xs-6 col-md-6">
                                        <b>Project Id: </b>
                                        <asp:Label ID="lblEditProjectId" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-xs-6 col-md-10">
                                        <b>Project Title: </b>
                                        <asp:Label ID="lblEditProject" runat="server" Text=""></asp:Label>
                                    </div>

                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-xs-2 col-md-2 hidden">
                                        <%--<b>Phase: </b>--%>
                                        <asp:Label ID="lblEditPhase" runat="server" Text=""></asp:Label>
                                    </div>
                                    <div class="col-md-2">
                                        Phase
                                        <asp:DropDownList ID="ddlPhaseEdit" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-xs-6 col-md-6">
                                        Service Type                                       
                                        <asp:DropDownList ID="ddlEditServiceType" runat="server" CssClass="form-control"></asp:DropDownList>
                                    </div>
                                </div>
                                <br />

                                <div class="row">
                                    <div class="col-xs-6 col-md-6">
                                        Description                
                                        <asp:TextBox ID="TextBoxEditDesc" runat="server" class="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                    </div>

                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-xs-6 col-md-3">
                                        Time             
                                        <asp:TextBox ID="TextBoxEditTime" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                    <div class="col-xs-6 col-md-3">
                                        Date
                                        <div class='input-group date' id='datetimepicker2'>
                                            <span class="input-group-addon">
                                                <span class="glyphicon glyphicon-calendar"></span>
                                            </span>
                                            <asp:TextBox ID="TextBoxEditDate" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <br />
                            </div>
                            <div class="modal-footer">
                                <asp:Label ID="lblResult" Visible="false" runat="server"></asp:Label>
                                <asp:Button ID="btnSave" runat="server" Text="Update" CssClass="btn btn-info" OnClick="btnSave_Click" />
                                <button class="btn btn-info" data-dismiss="modal" aria-hidden="true">Close</button>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="GridViewTimeEntry" EventName="RowCommand" />
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>

        <div class="clearfix"></div>

    </div>

    <script type="text/javascript"> 
        function pageLoad(sender, args) {

            // Display or hide Month/Year based on Time Entry Report type.
            var selectedValue = $('#<%= timeEntryReportType.ClientID %> input:checked');
            if (selectedValue == 'option_Project') {
                $('.dateDropdown').hide()
            }

            if ($("#MainContent_timeEntryReportType_0").is(':checked')) {
                    $('.dateDropdown').show();
                } else if ($("#MainContent_timeEntryReportType_1").is(':checked')) {
                    $('.dateDropdown').hide();
                }

            $('#<%= timeEntryReportType.ClientID %>').click(function () {
                if ($("#MainContent_timeEntryReportType_0").is(':checked')) {
                    $('.dateDropdown').show();
                } else if ($("#MainContent_timeEntryReportType_1").is(':checked')) {
                    $('.dateDropdown').hide();
                }
            });

            



            $('#li_timeentry').addClass('selected');

            //$("#MainContent_TextBoxTime").val(0.5);

            //$(document).ready(function () {

            //$(function(){
            //    $("#MainContent_ddlPI").chosen();
            //});

            $(function () {
                $('#datetimepicker1').datepicker({
                    format: "mm/dd/yyyy",
                    autoclose: true,
                    orientation: "top"
                });

                $('#datetimepicker2').datepicker({
                    format: "mm/dd/yyyy",
                    autoclose: true,
                    orientation: "top"
                });

                $('#idTourDateDetails').datepicker({
                    dateFormat: 'dd-mm-yy',
                    minDate: '+5d',
                    changeMonth: false,
                    changeYear: false,
                    altField: "#idTourDateDetailsHidden",
                    altFormat: "yy-mm-dd"
                });

                $('#txtFromDate').datepicker({
                    format: "mm/dd/yyyy",
                    autoclose: true,
                    todayHighlight: true,
                    orientation: "top"
                });

                $('#txtToDate').datepicker({
                    format: "mm/dd/yyyy",
                    autoclose: true,
                    todayHighlight: true,
                    orientation: "top"
                });
            });

            $(function () {
                $("#MainContent_TextBoxTime").bind("mousewheel", function (event, delta) {
                    if (delta > 0) {
                        this.value = parseFloat(this.value) + 0.5;
                    } else {
                        if (parseInt(this.value) > 0) {
                            this.value = parseFloat(this.value) - 0.5;
                        }
                    }
                    return false;
                });

            });

            $("#MainContent_ddlBiostatChosen").change(function (e) {
                $('#MainContent_ddlProject').val('');
                bindProjects();
            });

            //$("#MainContent_ddlProject").change(function(e) {

            //    bindProjects();
            //});

            bindProjects();
        }

        function bindProjects() {
            var biostatId = $("#MainContent_ddlBioStat").val();
            var piId = $("#MainContent_ddlBiostatChosen").val();
            var filterPI = $("#MainContent_ddlBiostatChosen :selected").text();
            var currentProjectId = $("#MainContent_ddlProject").val();

            if (!piId) piId = 0;
            if (biostatId > 0) {
                //$("#MainContent_ddlProject").val(0);

                $("#MainContent_ddlProject > option").each(function () {
                    if (this.text.indexOf(filterPI) > 0 || filterPI.length == 0 || filterPI.indexOf('Search') >= 0) {
                        $("#MainContent_ddlProject").children("option[value=" + this.value + "]").show();
                    }
                    else {
                        $("#MainContent_ddlProject").children("option[value=" + this.value + "]").hide();
                    }

                    //if (this.value == currentProjectId) {
                    //    $("#MainContent_ddlProject").val(this.value);
                    //}
                });

                //$.ajax({
                //    type: "POST",
                //    url: "TimeEntry.aspx/GetProjectList",
                //    data: '{"biostatId":"' + biostatId + '","piId":"' + piId + '"}',
                //    contentType: "application/json; charset=utf-8",
                //    dataType: "json",
                //    async: false,
                //    success: function (rtndata) {
                //        //alert(JSON.stringify(rtndata.d));
                //        bindProjectList(rtndata.d);
                //    },
                //    error: function (xhr, status, err) {
                //        var err = eval("(" + xhr.responseText + ")");
                //        alert(err.Message);

                //    }
                //});
            }
        }

        function bindProjectList(data) {
            //var options = $("#MainContent_ddlProject").html();                                     
            //var text = $("#MainContent_ddlBiostatChosen :selected").text();
            //if (text == "All") return;

            var selectedArray = [];
            for (var i = 0; i < data.length; i++) {
                var projectId = data[i].Id;
                selectedArray.push(parseInt(projectId));
            }

            $("#MainContent_ddlProject").val(0);

            $("#MainContent_ddlProject > option").each(function () {

                if (this.value > 0) {
                    if ($.inArray(parseInt(this.value), selectedArray) != -1) {
                        $("#MainContent_ddlProject").children("option[value=" + this.value + "]").show();
                    }
                    else {
                        $("#MainContent_ddlProject").children("option[value=" + this.value + "]").hide();
                    }
                }
            });
        }

        function ShowWarningModal() {
            $('#btnShowWarningModal').click();
        }

        <%--//https://msdn.microsoft.com/en-us/library/f5db6z8k.aspx
        /// Not being used, but client-side alternative to validation error 
        ///////////////////////////////////////////////////////////////////
        function textBoxTimeValidate(oSrc, args) {

            var projectId = $("#<%=ddlProject.ClientID%>").val();
            var currPhase = $("#<%=ddlPhase.ClientID%> :selected").text();
            // get current user phase type - currUserType
            // get hours of project for current user type - hoursInProject
            // get estimated hours for current user - estimatedHOurs

            // args.IsValid if (args.Value + hoursInProject) <= estimatedHours

            // if args.IsNotValid >> print error message (should already be done)

            args.IsValid = (args.Value < 8);
        }--%>


        $('#btnSubmit').click(function () {
            $("#commentForm").validate().cancelSubmit = true;
            $("#commentForm").submit();
            return false;
        })

        //});

        //$("input[name='MainContent_TextBoxTime']").TouchSpin({
        //    verticalbuttons: true
        //});

        $("#<%=ddlProject.ClientID%>").change(function () {
            //LoadProjectEffort();
            ////getProjectTime();
            //bindProjectHours([0, 0]);

            //divPhaseToggle();

            $("#<%=ddlProject.ClientID%>").append(new Option("option text", "value"));
        });

       <%--function divPhaseToggle() {
            var projectId = $("#<%=ddlProject.ClientID%>").val();
            if (projectId > 0) {  
                $('#divPhase').show();
            }
            else
                $('#divPhase').hide();
        }--%>

        <%--function LoadProjectEffort(){               
                var projectId = $("#<%=ddlProject.ClientID%>").val();
                   if (projectId > 0) { 
                       $.ajax({
                           type: "POST",
                           url: "TimeEntry.aspx/GetInvoiceData",
                           data: "{ \"projectId\":" + projectId + "}",
                           contentType: "application/json; charset=utf-8",
                           dataType: "json",
                           async: false,
                           success: function (rtndata) {                           
                               $("#<%=GridViewProjectEffort.ClientID%>").find("tr:gt(0)").remove();

                           if (rtndata.d.length > 0) { 
                               //$("#<%=GridViewProjectEffort.ClientID%> tr").not(":first").not(":last").remove();
                               var phdReqTotal = 0.0;
                               var phdSptTotal = 0.0;
                               var msReqTotal = 0.0;
                               var msSptTotal = 0.0;
                               for (var i = 0; i < rtndata.d.length; i++) {
                                   $("#<%=GridViewProjectEffort.ClientID%>").append("<tr><td>" + 
                                   rtndata.d[i].ReqNum + "</td><td>" + 
                                   rtndata.d[i].ReqDate + "</td><td>" + 
                                   rtndata.d[i].FromDate + "</td><td>" + 
                                   rtndata.d[i].ToDate + "</td><td>" + 
                                   rtndata.d[i].PhdReq + "</td><td>" + 
                                   rtndata.d[i].PhdSpt + "</td><td>" + 
                                   rtndata.d[i].MsReq + "</td><td>" + 
                                   rtndata.d[i].MsSpt + "</td></tr>");

                                   phdReqTotal += rtndata.d[i].PhdReq;
                                   phdSptTotal += rtndata.d[i].PhdSpt;
                                   msReqTotal += rtndata.d[i].MsReq;
                                   msSptTotal += rtndata.d[i].MsSpt;
                               }

                               $("#<%=GridViewProjectEffort.ClientID%>").append("<tr><td></td><td>Total</td><td></td><td></td><td>" +
                                   phdReqTotal + "</td><td>" +
                                   phdSptTotal + "</td><td>" +
                                   msReqTotal + "</td><td>" +
                                   msSptTotal + "</td></tr>");
                           }
                           else
                           {
                               $("#<%=GridViewProjectEffort.ClientID%>").append("<tr><td>&nbsp;</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
                           }
                       },
                       error: function (xhr, status, err) {
                           var err = eval("(" + xhr.responseText + ")");
                           alert(err.Message);

                       }
                   });               
               }
           }--%>

        <%--$("#btnGetTime").click(function () {
            $("#btnGetTime").val = "Processing......";
            getProjectHours();

            var btn = $(this);
            btn.prop('disabled', true);
            setTimeout(function(){
                btn.prop('disabled', false);
            }, 5000);
        })

        function getProjectHours() {
            var projectId = $("#<%=ddlProject.ClientID%>").val();
            var startDate = $("#txtFromDate").val();
            var endDate = $("#txtToDate").val();
               
            if (projectId > 0 && Date.parse(startDate) && Date.parse(endDate)
                && (new Date(startDate).getTime() <= new Date(endDate).getTime())) {
                var uri = '/api/Project/GetProjectHours/?projectId=' + projectId + '&startDate=' + startDate + '&endDate=' + endDate;
                   
                $.getJSON(uri)
                    .done(function (data) {
                        // On success
                        bindProjectHours(data);  
                    });
            }
        }

        function bindProjectHours(data) {
            if (data.length > 0) {
                $("#txtPhdTime").val(data[0]); 
                $("#txtMsTime").val(data[1]);
            }
        }--%>


        <%--$("#MainContent_ddlProject").change(function () {
               //var piId = $("#MainContent_ddlPI").val();
               var projectId = $("#MainContent_ddlProject").val();
              
               if (projectId > 0) { 
                   $.ajax({
                       type: "POST",
                       url: "TimeEntry.aspx/GetPI",
                       data: "{ \"projectId\":" + projectId + "}",
                       contentType: "application/json; charset=utf-8",
                       dataType: "json",
                       async: false,
                       success: function (rtndata) {
                           alert(JSON.stringify(rtndata.d));
                           $("#<%=ddlPI.ClientID%>").val(rtndata.d);
                           },
                           error: function (xhr, status, err) {
                               var err = eval("(" + xhr.responseText + ")");
                               alert(err.Message);

                           }
                       });
                       }
                       else {
                           $("#<%=ddlPI.ClientID%>").val(1);
               }
           });           --%>



        function ValidateEntry(sender, args) {
            if (document.getElementById("<%=TextBoxSubmitDate.ClientID %>").innerText != "") {
                args.IsValid = true;
            } else {
                args.IsValid = false;
            }
        }



        <%--function RadioCheck(rb) {
               var gv = document.getElementById("<%=gvPhase.ClientID%>");
               var rbs = gv.getElementsByTagName("input");
 
               var row = rb.parentNode.parentNode;
               for (var i = 0; i < rbs.length; i++) {
                   if (rbs[i].type == "checkbox") {
                       if (rbs[i].checked && rbs[i] != rb) {
                           rbs[i].checked = false;
                           break;
                       }
                   }
               }
           }    --%>

        function validateControl() {
            $("#commentForm").validate({
                rules: {
                    <%=ddlBioStat.UniqueID %>: {
                        required:true
                    },
                    <%=ddlProject.UniqueID %>: {
                        required:true
                    },
                    <%=ddlServiceType.UniqueID %>: {
                        required:true
                    },
                    <%=TextBoxSubmitDate.UniqueID %>: {
                        required:true
                    },
                    <%=TextBoxTime.UniqueID %>: {
                    required: true,
                    number: true
                }
                }
            });
        }

    </script>
</asp:Content>
