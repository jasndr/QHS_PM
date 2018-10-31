<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProjectForm2.aspx.cs" Inherits="ProjectManagement.ProjectForm2" %>

<%@ Register Assembly="DropDownChosen" Namespace="CustomDropDown" TagPrefix="ucc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="Scripts/jquery.bootstrap.wizard.min.js"></script>
    <script src="Scripts/bootstrap-datepicker.min.js"></script>
    <script src="Scripts/chosen.jquery.js"></script>
    <link href="Content/chosen.css" rel="stylesheet" />
    <style>
        h5 {
            overflow: hidden;
            text-align: center;
            font-weight: bold;
        }

            h5:before,
            h5:after {
                background-color: #000;
                content: "";
                display: inline-block;
                height: 1px;
                position: relative;
                vertical-align: middle;
                width: 50%;
            }

            h5:before {
                right: 0.5em;
                margin-left: -50%;
            }

            h5:after {
                left: 0.5em;
                margin-right: -50%;
            }

        .table-borderless tbody tr td, .table-borderless tbody tr th, .table-borderless thead tr th {
            border: none;
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
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="loader"></div>

    <div class="panel panel-default">
        <%--<div class="panel-heading"><b>Project</b></div>--%>
        <div class="panel-body">
            <div id="rootwizard" class="">

                <ul>
                    <li class="hidden"><a href="#tab1" data-toggle="tab"></a></li>
                    <li class="hidden"><a href="#MainContent_tabAdmin" data-toggle="tab"></a></li>
                </ul>
                <div class="tab-content">
                    <div class="panel panel-info">
                        <div class="panel-heading">
                            <h4 class="text-center"><b>Project Form</b></h4>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-9"></div>
                        <div class="col-md-1">
                            Id:
                            <asp:Label ID="lblProjectId" runat="server" Text=""></asp:Label>
                        </div>
                        <div class="col-md-2">
                            PI:&nbsp;
                    <asp:LinkButton runat="server" ID="lnkPI" CommandName="PI" OnCommand="lnkPI_Command" OnClientClick="var originalTarget = document.forms[0].target; document.forms[0].target = '_blank'; setTimeout(function () { document.forms[0].target = originalTarget; }, 1000);">
                        <asp:Label ID="lblPI" runat="server"></asp:Label>
                    </asp:LinkButton>
                        </div>
                    </div>
                    <br />
                    <div class="tab-pane" id="tab1">
                        <div class="row">
                            <div class="col-sm-1 text-left">
                                <label class="control-label" for="txtTitle">PI:</label>
                            </div>
                            <div class="col-sm-3">
                                <ucc:DropDownListChosen ID="ddlPI" runat="server" Width="200px"
                                    NoResultsText="No results match."
                                    DataPlaceHolder="Search PI" AllowSingleDeselect="true">
                                </ucc:DropDownListChosen>
                            </div>
                            <%-- <div class="col-sm-1">
                        <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnRest_Click" class="btn btn-info"/>
                    </div>--%>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-sm-1 text-left">
                                <label class="control-label" for="txtFirstName">Project:</label>
                            </div>
                            <div class="col-sm-6">
                                <asp:DropDownList ID="ddlProject" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlProject_Changed" AutoPostBack="True">
                                </asp:DropDownList>
                            </div>

                            <div class="col-sm-1 offset1">
                                <asp:CheckBox ID="chkBiostat" runat="server" Text="Biostat"></asp:CheckBox>
                            </div>
                            <div class="col-sm-1 offset1">
                                <asp:CheckBox ID="chkBioinfo" runat="server" Text="Bioinfo"></asp:CheckBox>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-sm-1 text-left">
                                <label class="control-label" for="txtTitle">Title:</label>
                            </div>
                            <div class="col-sm-9">
                                <input class="form-control" type="text" name="txtTitle" id="txtTitle" runat="Server" />
                            </div>
                            <div class="col-sm-1 hidden">
                                <asp:DropDownList ID="ddlProjectHdn" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-sm-1 text-left">
                                <label class="control-label" for="txtSummary">Summary:</label>
                            </div>
                            <div class="col-sm-9">
                                <textarea class="form-control noresize" rows="3" name="txtSummary" id="txtSummary" runat="Server"></textarea>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-sm-1">
                                <label class="control-label" for="txtInitialDate">Initial date:</label>
                            </div>
                            <div class="col-sm-2">
                                <div class='input-group date' id='dtpInitialDate'>
                                    <span class="input-group-addon">
                                        <span class="glyphicon glyphicon-calendar"></span>
                                    </span>
                                    <asp:TextBox ID="txtInitialDate" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-2 text-right">
                                <label class="control-label" for="txtDeadline">Deadline:</label>
                            </div>
                            <div class="col-sm-2">
                                <div class='input-group date' id='dtpDeadline'>
                                    <span class="input-group-addon">
                                        <span class="glyphicon glyphicon-calendar"></span>
                                    </span>
                                    <asp:TextBox ID="txtDeadline" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>

                            <%--<div class="col-sm-3 text-left">
                        <label class="control-label" for="txtDeadline">Request received date:</label></div>
                    <div class="col-sm-2">
                        <div class='input-group date' id='dtpRequestRcvDate'>
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-calendar"></span>
                            </span>
                            <asp:TextBox ID="txtRequestRcvdDate" runat="server" class="form-control"></asp:TextBox>
                        </div>
                    </div>--%>
                        </div>
                        <br />
                        <h5>QHS Faculty/Staff</h5>
                        <div class="row">
                            <div class="col-sm-2 text-left">
                                <label class="control-label" for="txtFirstName">Lead member:</label>
                            </div>
                            <div class="col-sm-3">
                                <asp:DropDownList ID="ddlLeadBiostat" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-sm-6 text-left">
                                <label class="control-label">Other member(s): check all that apply, or indicate N/A</label>
                            </div>
                        </div>
                        <%--<br />--%>
                        <table class="table table-hover table-borderless" id="tblBiostat">
                            <tbody>
                                <asp:Repeater ID="rptBiostat" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="FirstchkId" runat="server"></asp:CheckBox>
                                                <%--<asp:HiddenField ID="Id1" Value='<%#Eval("Id1")%>' runat="server" />--%>
                                                <asp:HiddenField ID="FirstBitValue" Value='<%#Eval("BitValue1")%>' runat="server" />
                                                <%# Eval("Name1") %>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="SecondchkId" runat="server" Visible='<%# (int)Eval("Id2") > 0 %>'></asp:CheckBox>
                                                <%--<asp:HiddenField ID="Id2" Value='<%#Eval("Id2")%>' runat="server" />--%>
                                                <asp:HiddenField ID="SecondBitValue" Value='<%#Eval("BitValue2")%>' runat="server" />
                                                <%# Eval("Name2") %>
                                            </td>
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <td>
                                            <asp:CheckBox ID="FirstchkId" runat="server"></asp:CheckBox>
                                            <%--<asp:HiddenField ID="Id1" Value='<%#Eval("Id1")%>' runat="server" />--%>
                                            <asp:HiddenField ID="FirstBitValue" Value='<%#Eval("BitValue1")%>' runat="server" />
                                            <%# Eval("Name1") %>
                                        </td>
                                        <td>
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
                            <div class="col-sm-2">
                                <input class="form-control hidden" type="text" name="txtOtherMemberBitSum" id="txtOtherMemberBitSum" runat="Server" />
                            </div>
                        </div>

                        <h5>Study Area</h5>
                        <div class="row">
                            <div class="col-sm-6 text-left">
                                <label class="control-label">Check all that apply</label>
                            </div>
                        </div>
                        <%--<br />--%>
                        <table class="table table-hover table-borderless" id="tblStudyArea">
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
                            </tbody>
                        </table>
                        <div class="row">
                            <div class="col-sm-6">
                                <input class="form-control" type="text" name="txtStudyAreaOther" id="txtStudyAreaOther" runat="Server" />
                            </div>
                            <div class="col-sm-2">
                                <input class="form-control hidden" type="text" name="txtStudyAreaBitSum" id="txtStudyAreaBitSum" runat="Server" />
                            </div>
                        </div>

                        <h5>Health Data</h5>
                        <div class="row">
                            <div class="col-sm-6 text-left">
                                <label class="control-label">Check all that apply, or indicate N/A</label>
                            </div>
                        </div>
                        <%-- <br />--%>
                        <table class="table table-hover table-borderless" id="tblHealthData">
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
                                <input class="form-control" type="text" name="txtHealthDataOther" id="txtHealthDataOther" runat="Server" />
                            </div>
                            <div class="col-sm-2">
                                <input class="form-control hidden" type="text" name="txtHealthDataBitSum" id="txtHealthDataBitSum" runat="Server" />
                            </div>
                        </div>

                        <h5>Study Type</h5>
                        <div class="row">
                            <div class="col-sm-6 text-left">
                                <label class="control-label">Check all that apply</label>
                            </div>
                        </div>
                        <%--<br />--%>
                        <table class="table table-hover table-borderless" id="tblStudyType">
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
                                <input class="form-control" type="text" name="txtStudyTypeOther" id="txtStudyTypeOther" runat="Server" />
                            </div>
                            <div class="col-sm-2">
                                <input class="form-control hidden" type="text" name="txtStudyTypeBitSum" id="txtStudyTypeBitSum" runat="Server" />
                            </div>
                        </div>

                        <h5>Study Population</h5>
                        <div class="row">
                            <div class="col-sm-6 text-left">
                                <label class="control-label">Check all that apply, or indicate N/A</label>
                            </div>
                        </div>
                        <%-- <br />--%>
                        <table class="table table-hover table-borderless" id="tblStudyPopulation">
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
                            <div class="col-sm-1"></div>
                            <div class="col-sm-6">
                                <input class="form-control" type="text" name="txtStudyPopulationOther" id="txtStudyPopulationOther" runat="Server" />
                            </div>
                            <div class="col-sm-1">
                                <input class="form-control hidden" type="text" name="txtStudyPopulationBitSum" id="txtStudyPopulationBitSum" runat="Server" />
                            </div>
                        </div>

                        <h5>Service</h5>
                        <div class="row">
                            <div class="col-sm-6 text-left">
                                <label class="control-label">Check all that apply</label>
                            </div>
                        </div>
                        <%-- <br />--%>
                        <table class="table table-hover table-borderless" id="tblService">
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
                        </table>
                        <div class="row">
                            <div class="col-sm-6" id="divLetterOfSupport">
                                <div class="col-sm-3">
                                    <label for="chkLetterOfSupportYes">Letter of Support only?</label>
                                </div>
                                <div class="col-sm-3">
                                    <asp:CheckBox ID="chkLetterOfSupportYes" runat="server" Text="Yes"></asp:CheckBox>
                                </div>
                                <div class="col-sm-3">
                                    <asp:CheckBox ID="chkLetterOfSupportNo" runat="server" Text="No"></asp:CheckBox>
                                </div>
                            </div>
                            <div class="col-sm-4">
                                <input class="form-control" type="text" name="txtServiceOther" id="txtServiceOther" runat="Server" />
                            </div>
                            <div class="col-sm-2">
                                <input class="form-control hidden" type="text" name="txtServiceBitSum" id="txtServiceBitSum" runat="Server" />
                            </div>
                        </div>
                        <br />
                        <h5>Credit To</h5>
                        <div class="row">
                            <div class="col-sm-6 text-left">
                                <label class="control-label">Credit to sister cores</label>
                            </div>
                        </div>
                        <div class="row offset2">
                            <div class="col-sm-2">
                                <asp:CheckBox ID="chkCreditToBiostat" runat="server" Text="Biostat Only"></asp:CheckBox>
                            </div>
                            <div class="col-sm-2">
                                <asp:CheckBox ID="chkCreditToBioinfo" runat="server" Text="Bioinfo Only"></asp:CheckBox>
                            </div>
                            <div class="col-sm-2">
                                <asp:CheckBox ID="chkCreditToBoth" runat="server" Text="Biostat and Bioinfo"></asp:CheckBox>
                            </div>
                        </div>

                        <h5>Other Description</h5>
                        <br />
                        <div class="row">
                            <div class="col-sm-2 text-left">
                                <label class="control-label">
                                    Other project description:
                                </label>
                            </div>
                            <div class="col-sm-6">
                                <table class="table" id="tblDesc">
                                    <tbody>
                                        <tr>
                                            <td>
                                                <label>
                                                    Is PI a junior investigator?
                                                </label>
                                            </td>
                                            <td>
                                                <input type="checkbox" id="chkJuniorPIYes" value="1" runat="server" />Yes
                                        &nbsp;
                                        <input type="checkbox" id="chkJuniorPINo" value="0" runat="server" />No
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label for="chkMentorYes">
                                                    Does PI have mentor?
                                                </label>
                                            </td>
                                            <td>
                                                <input type="checkbox" id="chkMentorYes" value="1" runat="server" />Yes
                                        &nbsp;
                                        <input type="checkbox" id="chkMentorNo" value="0" runat="server" />No
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label>
                                                    Is project an internal study?
                                                </label>
                                            </td>
                                            <td>
                                                <input type="checkbox" id="chkInternalYes" value="1" runat="server" />Yes
                                        &nbsp;
                                        <input type="checkbox" id="chkInternalNo" value="0" runat="server" />No
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label>
                                                    Is project an infrastructure grant pilot study?
                                                </label>
                                            </td>
                                            <td>
                                                <input type="checkbox" id="chkPilotYes" value="1" runat="server" />Yes
                                        &nbsp;
                                        <input type="checkbox" id="chkPilotNo" value="0" runat="server" />No
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label>
                                                    Is this a paying project?
                                                </label>
                                            </td>
                                            <td>
                                                <input type="checkbox" id="chkPayingYes" value="1" runat="server" />Yes
                                        &nbsp;
                                        <input type="checkbox" id="chkPayingNo" value="0" runat="server" />No
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
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
                        <div class="row" id="divPayProject">
                            <div class="col-sm-3 text-left">
                                <label class="control-label" for="txtPayProject">If paying project, type of payment:</label>
                            </div>
                            <div class="col-sm-6">
                                <input class="form-control" type="text" name="txtPayProject" id="txtPayProject" runat="Server" />
                            </div>
                        </div>
                        <br />

                        <h5>Funding Source</h5>
                        <%--<br />--%>
                        <div class="row">
                            <div class="col-sm-6 text-left">
                                <label class="control-label" style="text-align: left;">
                                    Check all that apply, or indicate N/A.
                                    <br />
                                    <em style="font-weight: normal;">(This section is used to mark
                                    funding sources, <u>not</u> grants acknowledged.)</em><br />
                                </label>
                            </div>
                        </div>
                        <%--<br />--%>
                        <table class="table table-hover table-borderless" id="tblGrant">
                            <tbody>
                                <asp:Repeater ID="rptGrant" runat="server">
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
                        <div id="divDeptFund">
                            <div class="row">
                                <div class="col-sm-3 col-sm-offset-1">
                                    <label class="control-label" for="ddlDepartmentFunding">Department Funding</label>
                                    <asp:DropDownList ID="ddlDepartmentFunding" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-sm-1"></div>
                                <div class="col-sm-3">
                                    <label class="control-label" for="txtDeptFundOth">Department Funding - Other</label>
                                    <input class="form-control" type="text" name="txtDeptFundOth" id="txtDeptFundOth" runat="Server" />
                                </div>
                                <div class="col-sm-1"></div>
                                <div class="col-sm-3">
                                    <label class="control-label" for="txtGrantOther">Other:</label>
                                    <input class="form-control" type="text" name="txtGrantOther" id="txtGrantOther" runat="Server" />
                                </div>
                                <div class="col-sm-2">
                                    <input class="form-control hidden" type="text" name="txtGrantBitSum" id="txtGrantBitSum" runat="Server" />
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div id="divDeptFundMou">
                                    <div class="col-sm-1"></div>
                                    <div class="col-sm-6">
                                        <div class="col-sm-4">
                                            <label for="chkDeptFundMouYes">Is this project supported by an MOU?</label>
                                        </div>
                                        <div class="col-sm-1">
                                            <asp:CheckBox ID="chkDeptFundMouYes" runat="server" Text="Yes"></asp:CheckBox>
                                        </div>
                                        <div class="col-sm-1">
                                            <asp:CheckBox ID="chkDeptFundMouNo" runat="server" Text="No"></asp:CheckBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <br />

                        <h5>Acknowledgements</h5>
                        <%--<br />--%>
                        <div class="row">
                            <div class="col-sm-11 text-left">
                                <label class="control-label" style="text-align: left;">
                                    Check all that apply, or indicate N/A.
                                    <br />
                                    <em style="font-weight: normal;">(This section is used to mark
                                    entities to acknowledge, which may / may not be in addition to the source of funding.  Funding sources
                                    have been checked by default; please check/uncheck necessary acknowledgements.)</em><br />
                                </label>
                            </div>
                        </div>
                        <%--<br />--%>
                        <table class="table table-hover table-borderless" id="tblAkn">
                            <tbody>
                                <asp:Repeater ID="rptAkn" runat="server">
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
                        <div class="row" id="divAknDeptFund">
                            <div class="col-sm-3 col-sm-offset-1">
                                <label class="control-label" for="ddlAknDepartmentFunding">Department Funding</label>
                                <asp:DropDownList ID="ddlAknDepartmentFunding" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="col-sm-1"></div>
                            <div class="col-sm-3">
                                <label class="control-label" for="txtAknDeptFundOth">Department Funding - Other</label>
                                <input class="form-control" type="text" name="txtAknDeptFundOth" id="txtAknDeptFundOth" runat="Server" />
                            </div>
                            <div class="col-sm-1"></div>
                            <div class="col-sm-3">
                                <label class="control-label" for="txtAknOther">Other:</label>
                                <input class="form-control" type="text" name="txtAknOther" id="txtAknOther" runat="Server" />
                            </div>
                            <div class="col-sm-2">
                                <input class="form-control hidden" type="text" name="txtAknBitSum" id="txtAknBitSum" runat="Server" />
                            </div>
                        </div>
                        <br />

                        <h5>Phase</h5>
                        <br />
                        <asp:UpdatePanel ID="upPhase" runat="server">
                            <ContentTemplate>
                                <div>
                                    <asp:GridView ID="gvPhase" runat="server" AutoGenerateColumns="False"
                                        ShowFooter="True"
                                        OnRowDataBound="gvPhase_RowDataBound"
                                        OnRowDeleting="gvPhase_RowDeleting"
                                        class="table">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Id" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblId" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Phase" HeaderStyle-Width="8%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPhase" runat="server" Text='<%# Eval("Name") %>' />
                                                </ItemTemplate>
                                                <FooterStyle HorizontalAlign="Left" />
                                                <FooterTemplate>
                                                    <asp:Button ID="btnAddPhase" runat="server" CssClass="btn btn-info"
                                                        Text="Add New" OnClick="btnAddPhase_Click" OnClientClick="AddPhase()" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Agreement" HeaderStyle-Width="12%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAgmtId" runat="server" Text='<%# Eval("AgmtId") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Start Date (mm/dd/yyyy)" HeaderStyle-Width="15%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtStartDate" runat="server" class="form-control" Text='<%# Eval("StartDate") %>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Completion Date (mm/dd/yyyy)" HeaderStyle-Width="15%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtCompletionDate" runat="server" class="form-control" Text='<%# Eval("CompletionDate") %>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Title" HeaderStyle-Width="25%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtTitle" runat="server" class="form-control" Text='<%# Eval("Title") %>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ms Hrs" HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtMsHrs" runat="server" class="form-control" Text='<%# Eval("MsHrs") %>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Phd Hrs" HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtPhdHrs" runat="server" class="form-control" Text='<%# Eval("PhdHrs") %>'></asp:TextBox>
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
                                </div>

                            </ContentTemplate>
                            <%--<Triggers></Triggers>--%>
                        </asp:UpdatePanel>

                        <%--<table class="table table-bordered table-hover">
                    <thead class="thead-inverse">
                        <tr>
                            <td>Id</td>
                            <td>Phase</td>
                            <td>Start Date(mm/dd/yyyy)</td>
                            <td>Title</td>
                            <td>Ms Hrs</td>
                            <td>Phd Hrs</td>
                            <td>Del</td>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="rptPhase" runat="server"
                            OnItemCommand="cpRepeater_ItemCommand"
                            OnItemDataBound="cpRepeater_ItemDataBound">
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkDelete" runat="server" />
                                        <asp:Label ID="lblPhaseId" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlPhase" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtStartDate" runat="server" class="form-control" Text='<%# Eval("StartDate") %>'></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTitle" runat="server" class="form-control" Text='<%# Eval("Title") %>'></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMsHrs" runat="server" class="form-control" Text='<%# Eval("MsHrs") %>'></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox1" runat="server" class="form-control" Text='<%# Eval("PhdHrs") %>'></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="lnkDelete" runat="server" Text="Delete" CommandName="Delete"
                                            ToolTip="Delete" OnClientClick='return confirm("Are you sure you want to delete this entry?");'
                                            CommandArgument=''><img src="images/icon-delete.png" />
                                        </asp:LinkButton>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                <tr style="background-color: #15880a">
                                    <td colspan="5">
                                        <asp:Button ID="btnAddPhase" runat="server" CssClass="btn btn-info"
                                            Text="Add New" OnClick="btnAddPhase_Click" />
                                    </td>
                                </tr>
                            </FooterTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>--%>

                        <div class="row">
                            <div class="col-sm-5">
                                <input type="checkbox" id="chkApproved" value="1" runat="server" />&nbsp;<b>Admin reviewed</b>
                            </div>
                        </div>
                        <hr />
                        <div class="row">
                            <div class="col-sm-6">
                            </div>
                            <div class="col-sm-2">
                                <asp:Button ID="btnSubmit1" runat="server" Text="Submit" OnClick="btnSubmit1_Click" class="btn btn-primary submitBtn" OnClientClick="ClientSideClick(this)" UseSubmitBehavior="False" />
                            </div>
                        </div>

                    </div>

                    <div class="tab-pane" id="tabAdmin" runat="server">
                        <h5>Admin</h5>
                        <br />

                        <div class="row">
                            <div class="col-sm-3">
                                <input type="checkbox" id="chkIsRmatrix" value="1" runat="server" />&nbsp;RMATRIX-II request for resources
                            </div>
                            <div class="col-sm-9">
                                <div class="row" id="divRmatrixRequest">
                                    <div class="col-sm-3 text-right">
                                        <label class="control-label" for="txtTitle">Request number:</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <input class="form-control" type="text" name="txtRmatrixNum" id="txtRmatrixNum" runat="Server" />
                                    </div>
                                    <div class="col-sm-3 text-right">
                                        <label class="control-label" for="txtRmatrixSubDate">Submission date:</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <div class='input-group date' id='dtpRmatrixSubDate'>
                                            <span class="input-group-addon">
                                                <span class="glyphicon glyphicon-calendar"></span>
                                            </span>
                                            <asp:TextBox ID="txtRmatrixSubDate" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <%--<div class="col-sm-5">
                                <input type="checkbox" id="chkRmatrixReport" value="1" runat="server" />&nbsp;Do not report to RMATRIX
                            </div>--%>
                            <div class="col-sm-5">
                                <input type="checkbox" id="chkReportToRmatrix" value="1" runat="server" />&nbsp;Report to RMATRIX
                            </div>
                            <div class="col-sm-5">
                                <input type="checkbox" id="chkReportToOlaHawaii" value="1" runat="server" />&nbsp;Report to Ola Hawaii
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-sm-3">
                                <input type="checkbox" id="chkIsOlaHawaii" value="1" runat="server" />&nbsp;Ola Hawaii request for resources
                            </div>
                            <div class="col-sm-9">

                                <div id="divOlaHawaiiRequest">
                                    <div class="row">
                                        <div class="col-sm-3 text-right">
                                            <label class="control-label" for="txtTitle">Request number:</label>
                                        </div>
                                        <div class="col-sm-3">
                                            <input class="form-control" type="text" name="txtOlaHawaiiNum" id="txtOlaHawaiiNum" runat="Server" />
                                        </div>
                                        <div class="col-sm-3 text-right">
                                            <label class="control-label" for="txtOlaHawaiiSubDate">Submission date:</label>
                                        </div>
                                        <div class="col-sm-3">
                                            <div class='input-group date' id='dtpOlaHawaiiSubDate'>
                                                <span class="input-group-addon">
                                                    <span class="glyphicon glyphicon-calendar"></span>
                                                </span>
                                                <asp:TextBox ID="txtOlaHawaiiSubDate" runat="server" class="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div>
                                        <div class="row offset2">
                                            <div class="col-sm-6 text-right">
                                                <label class="control-label">Request Type:</label>
                                            </div>
                                            <div class="col-sm-6"></div>
                                        </div>
                                        <div class="row offset2">
                                            <div class="col-sm-6"></div>
                                            <div class="col-sm-2">
                                                <asp:CheckBox ID="chkRequestTypeRfunded" runat="server" Text="R-funded"></asp:CheckBox>
                                            </div>
                                            <div class="col-sm-2">
                                                <asp:CheckBox ID="chkRequestTypePilotPI" runat="server" Text="Pilot PI"></asp:CheckBox>
                                            </div>
                                            <div class="col-sm-2">
                                                <asp:CheckBox ID="chkRequestTypeOther" runat="server" Text="Other"></asp:CheckBox>
                                            </div>

                                        </div>

                                    </div>
                                </div>

                            </div>
                        </div>
                        <br />

                        <%--<b>Phase Completion:</b>
                <asp:Repeater ID="rptPhaseCompletion" runat="server">
                    <HeaderTemplate>
                        <table class="table table-bordered table-hover" id="tblPhaseCompletion">
                            <tr>
                                <th>Phase</th>
                                <th>Ms Hrs (completed)</th>
                                <th>Ms Hrs (estimated)</th>
                                <th>Phd Hrs (completed)</th>
                                <th>Phd Hrs (estimated)</th>
                                <th>Completion Date</th>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td><%# Eval("Name") %></td>
                            <td><%# Eval("MsHrsT") %></td>
                            <td><%# Eval("MsHrs") %></td>
                            <td><%# Eval("PhdHrsT") %></td>
                            <td><%# Eval("PhdHrs") %></td>
                            <td>
                                <%# Eval("CompletionDate") %>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
                <br />--%>
                        <div class="row">
                            <div class="col-sm-3">
                                <label class="control-label" for="txtCompletionDate">Project completion date:</label>
                            </div>
                            <div class="col-sm-2">
                                <div class='input-group date' id='dtpCompletionDate'>
                                    <span class="input-group-addon">
                                        <span class="glyphicon glyphicon-calendar"></span>
                                    </span>
                                    <asp:TextBox ID="txtCompletionDate" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-2">
                                <%--<button type="button" ID="btnSurvey" OnClick="ShowSurveyModal()"  class="btn btn-primary">Survey</button>--%>
                                <asp:Button class="btn btn-primary" ID="btnSurvey" runat="server" Text="Send Client Survey" OnClick="btnSurvey_Click"></asp:Button>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-sm-2">
                                <label class="control-label" for="txtProjectStatus">Project status:</label>
                            </div>
                            <div class="col-sm-5">
                                <input class="form-control" type="text" name="txtProjectStatus" id="txtProjectStatus" runat="Server" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Button class="btn btn-primary" ID="btnAddGrant" runat="server" Text="Add Fund" OnClick="btnAddGrant_Click"></asp:Button>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-sm-1 text-left">
                                <label class="control-label" for="txtSummary">Comments:</label>
                            </div>
                            <div class="col-sm-6">
                                <textarea class="form-control noresize" rows="3" name="txtComments" id="txtComments" runat="Server"></textarea>
                            </div>
                            <div class="col-sm-2">
                                <asp:Button ID="btnAdminSubmit" runat="server" Text="Submit" OnClick="btnSubmit1_Click" class="btn btn-primary submitBtn" OnClientClick="ClientSideClick(this);" UseSubmitBehavior="False" />
                            </div>
                        </div>
                        <br />
                        <hr />

                    </div>

                    <ul class="pager wizard">
                        <li class="previous first" style="display: none;"><a href="#">First</a></li>
                        <li class="previous"><a href="#">Previous</a></li>
                        <li class="next last" style="display: none;"><a href="#">Last</a></li>
                        <li class="next"><a href="#">Next</a></li>
                    </ul>
                </div>
            </div>

        </div>
    </div>

    <div id="surveyModal" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title">Confirmation</h4>
                </div>
                <asp:UpdatePanel ID="upSurvey" runat="server">
                    <ContentTemplate>
                        <div class="modal-body">
                            <asp:Label ID="lblSurveyMsg" runat="server"></asp:Label><br />
                            <div style="margin-left: .5in" id="divProjectInfo" runat="server">
                                Project title:
                                <asp:Label ID="lblProjectTitle" runat="server"></asp:Label><br />
                                Faculty/Staff:
                                <asp:Label ID="lblBiostats" runat="server"></asp:Label><br />
                                Project period:
                                <asp:Label ID="lblProjectPeriod" runat="server"></asp:Label><br />
                                <%--Service hours: <asp:Label ID="lblServiceHours" runat="server"></asp:Label><br />--%>
                            </div>
                            <br />
                            <p class="text-warning">The survey can only be sent twice.</p>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                            <asp:Button class="btn btn-primary" ID="btnSendSurvey" runat="server" Text="Submit" OnClick="btnSendSurvey_Click" OnClientClick="ClientSideClick(this)" UseSubmitBehavior="False"></asp:Button>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSurvey" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnSendSurvey" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <%--<div id="warningModal2" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h3 id="formIncomplete2" class="modal-title">Form Incomplete</h3>
                </div>
                <div class="modal-body">
                    <asp:Label ID="lblWarning2" runat="server">Please review the following <u>incomplete</u> form items and try to submit again:</asp:Label><br />
                    <br />
                    <p class="text-warning" id="textWarning2"></p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>--%>

    <div id="warningModal" class="modal" tabindex="-1" role="dialog">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h3 id="formIncomplete" class="modal-title">Form Incomplete</h3>
                </div>
                <asp:UpdatePanel ID="upWarning" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="modal-body">
                            <label id="lblWarning">Please review the following <u>incomplete</u> form items and try to submit again:</label>
                            <br />
                            <p class="text-warning" id="textWarning"></p>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <div class="clearfix"></div>

    <script type="text/javascript">
        $(document).ready(function () {
            $(".loader").fadeOut("slow");

            $('#rootwizard').bootstrapWizard({
                onTabShow: function (tab, navigation, index) {
                    //$('html, body').animate({scrollTop: $("#rootwizard").offset().top}, 0);
                    $('#rootwizard').scrollTop(0);
                }
            });

            bindProjects();
            $('#MainContent_ddlPI').change(function (e) {
                $('#MainContent_ddlProject').val('');   //find('option:first').attr('selected', 'selected');
                bindProjects();

            });

            // Makes "RMATRIX" and "Ola Hawaii" checkboxes checked by default
            // if there is no ID already tied to the project form.
            var projectId = $("#MainContent_lblProjectId").text();
            if (projectId == null || projectId == 0) {
                $('#tblAkn').find('td').each(function () {
                    var _aknCheckBox = $(this).find(":input[name$='chkId']"),
                        //_aknBitValue = $(this).find(":input[name$='BitValue']").val(),
                        _aknName = $(this).eq(0).text().trim();

                    if (_aknName == 'RMATRIX' || _aknName == 'Ola Hawaii') {
                        _aknCheckBox.prop("checked", true);
                    }


                });
            }




            // Initializes date for date fields.
            var initDate = new biostatNS.DatePicker('dtpInitialDate');
            initDate.init();

            var dueDate = new biostatNS.DatePicker('dtpDeadline');
            dueDate.init();

            var requestRcvDate = new biostatNS.DatePicker('dtpRequestRcvDate');
            requestRcvDate.init();

            var rmatrixSubDate = new biostatNS.DatePicker('dtpRmatrixSubDate');
            rmatrixSubDate.init();

            var olaHawaiiSubDate = new biostatNS.DatePicker('dtpOlaHawaiiSubDate');
            olaHawaiiSubDate.init();

            var completionDate = new biostatNS.DatePicker('dtpCompletionDate');
            completionDate.init();

            //TextBoxOtherToggle(tblHealthData);
            //$('#tblHealthData').on( 'click', 'input[type="checkbox"]', function() {
            //    //$( this ).closest( 'tr' ).toggleClass( 'green' );
            //    TextBoxOtherToggle(this.closest('table'));
            //})

            // ------  Intializes Other/Bitsum functionality, ----------- \\
            // ------  points to grid of checkboxes for specific section -- \\
            var tBiostat = new biostatNS.TableToggle(tblBiostat);
            tBiostat.Click(tblBiostat);
            $('#tblBiostat').on('click',
                'input[type="checkbox"]',
                function () {
                    tBiostat.Click(this.closest('table'));
                });

            var tStudyArea = new biostatNS.TableToggle(tblStudyArea);
            tStudyArea.Click(tblStudyArea);
            $('#tblStudyArea').on('click',
                'input[type="checkbox"]',
                function () {
                    tStudyArea.Click(this.closest('table'));
                });

            var tHealthData = new biostatNS.TableToggle(tblHealthData);
            tHealthData.Click(tblHealthData);
            $('#tblHealthData').on('click',
                'input[type="checkbox"]',
                function () {
                    tHealthData.Click(this.closest('table'));
                });

            var tStudyType = new biostatNS.TableToggle(tblStudyType);
            tStudyType.Click(tblStudyType);
            $('#tblStudyType').on('click',
                'input[type="checkbox"]',
                function () {
                    tStudyType.Click(this.closest('table'));
                });

            var tStudyPopulation = new biostatNS.TableToggle(tblStudyPopulation);
            tStudyPopulation.Click(tblStudyPopulation);
            $('#tblStudyPopulation').on('click',
                'input[type="checkbox"]',
                function () {
                    tStudyPopulation.Click(this.closest('table'));
                });

            var tService = new biostatNS.TableToggle(tblService);
            tService.Click(tblService);
            $('#tblService').on('click',
                'input[type="checkbox"]',
                function () {
                    tService.Click(this.closest('table'));
                });

            var tGrant = new biostatNS.TableToggle(tblGrant);
            tGrant.Click(tblGrant);
            $('#tblGrant').on('click',
                'input[type="checkbox"]',
                function () {
                    tGrant.Click(this.closest('table'));
                });

            var tAkn = new biostatNS.TableToggle(tblAkn);
            tAkn.Click(tblAkn);
            $('#tblAkn').on('click',
                'input[type="checkbox"]',
                function () {
                    tAkn.Click(this.closest('table'));
                });

            // -- Reveals hidden sections if certain choices are made -- \\
            ToggleDiv($('#MainContent_chkMentorYes'), $('#divMentor'));
            ToggleDiv($('#MainContent_chkPayingYes'), $('#divPayProject'));
            ToggleDiv($('#MainContent_chkIsRmatrix'), $('#divRmatrixRequest'));
            ToggleDiv($('#MainContent_chkIsOlaHawaii'), $('#divOlaHawaiiRequest'));
            //ToggleDiv($('#MainContent_rptGrant_SecondchkId_6'), $('#divDeptFund'));

            ToggleDiv4($('#MainContent_rptStudyPopulation_chkId_0'),
                $('#MainContent_rptStudyPopulation_chkId_1'),
                $('#MainContent_rptStudyPopulation_chkId_2'),
                $('#MainContent_rptStudyPopulation_chkId_3'),
                $('#divHealthDisparity'));

            ToggleDiv($('#MainContent_rptService_chkId_3'), $('#divLetterOfSupport'));


            // -- Hides/shows certain sections if certain selections are made -- \\
            $('#MainContent_chkIsRmatrix').change(function () {
                ToggleDiv($(this), $('#divRmatrixRequest'));
            });

            $('#MainContent_chkIsOlaHawaii').change(function () {
                ToggleDiv($(this), $('#divOlaHawaiiRequest'));
            });

            //If Department funding is checked, show dropdown list.
            //$('#MainContent_rptGrant_SecondchkId_6').change(function () {
            //    ToggleDiv($(this), $('#divDeptFund'));
            //});

            $('#tblDesc').on('click',
                'input[type="checkbox"]',
                function () {
                    if ($(this).is($('#MainContent_chkMentorYes'))) {
                        ToggleDiv($(this), $('#divMentor'));
                    }

                    if ($(this).is($('#MainContent_chkPayingYes'))) {
                        ToggleDiv($(this), $('#divPayProject'));
                    }

                    if ($(this).is(":checked")) {
                        if ($(this).val() == 1)
                            $(this).next().removeAttr("checked");
                        if ($(this).val() == 0)
                            $(this).prev().removeAttr("checked");

                        if ($(this).is($('#MainContent_chkMentorNo')))
                            $('#divMentor').hide();

                        if ($(this).is($('#MainContent_chkPayingNo')))
                            $('#divPayProject').hide();
                    }
                });

            $('#tblStudyPopulation').on('click',
                'input[type="checkbox"]',
                function () {
                    if ($(this).is($('#MainContent_rptStudyPopulation_chkId_0')) // Native Hawaiians,
                        //--- Pacific Islanders,
                        //--- and Filipinos
                        || $(this).is($('#MainContent_rptStudyPopulation_chkId_1')) // Hawaii Populations
                        || $(this).is($('#MainContent_rptStudyPopulation_chkId_2')) // U.S. Populations
                        || $(this).is($('#MainContent_rptStudyPopulation_chkId_3')) // International Populations
                    ) {
                        ToggleDiv($(this), $('#divHealthDisparity'));
                    }
                });

            $('#tblService').on('click',
                'input[type="checkbox"]',
                function () {
                    if ($(this).is($('#MainContent_rptService_chkId_3'))) {
                        ToggleDiv($(this), $('#divLetterOfSupport'));
                    }
                });

            // -- If exisitng project, adds ability to add grant, invoice, or survey. -- \\
            var projectId = $("#MainContent_lblProjectId").text();
            if (projectId > 0) {
                $("#MainContent_btnAddInvoice").prop("disabled", false);
                $("#MainContent_btnAddGrant").prop("disabled", false);
                $("#MainContent_btnSurvey").prop("disabled", false);
            }
            else {
                $("#MainContent_btnAddInvoice").prop("disabled", true);
                $("#MainContent_btnAddGrant").prop("disabled", true);
                $("#MainContent_btnSurvey").prop("disabled", true);
            }

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

            //$("#MainContent_ddlLeadBiostat").change(function () {
            //    if ($("#MainContent_ddlLeadBiostat").val() > 0) {
            //        var biostatSelected = $("#MainContent_ddlLeadBiostat option:selected").text();


            //    }
            //});


        });

        var biostatNS = biostatNS || {};

        // TableToggle - Makes "other" appear if "other" checkboxes exists.
        //               Also creates and adds bitsum for each checkbox checked.
        biostatNS.TableToggle = function (tableId) {
            var _table = $(tableId),
                _textOther = _table.next().find(":input[name$='Other']"),
                _textBitSum = _table.next().find(":input[name$='BitSum']"),
                _id = _table.attr('id');


            // If the "Other" checkbox is checked, then then the "other" field is displayed.
            // The bit sum total is added from the bit value of the checkbox.
            return {
                Click: function (e) {
                    var _bitSum = 0;

                    _table.find('td').each(function () {
                        var _checkBox = $(this).find(":input[name$='chkId']"),
                            _bitValue = $(this).find(":input[name$='BitValue']").val(),
                            _name = $(this).eq(0).text().trim();

                        if (_name == 'Other' || _name == 'International Populations') {

                            //alert(_id + ' is the table id');

                            if (_checkBox.is(':checked')) {
                                _textOther.show();

                                if (_id == 'tblGrant' || _id == 'tblAkn') {
                                    _textOther.parent().find('label').show();
                                }
                            }
                            else {
                                _textOther.hide();
                                $(_textOther).val('');

                                if (_id == 'tblGrant' || _id == 'tblAkn') {
                                    _textOther.parent().find('label').hide();
                                }

                            }
                        }

                        // check equivalent checkbox in "funding source (grant)" section to "acknowledgements" section.
                        if (_id == 'tblGrant') {
                            // find tblAkn
                            var _tableAkn = $('#tblAkn'),
                                _aknBitSum = 0;
                            // if _checkbox is checked
                            _checkBox.change(function () {
                                if (_checkBox.is(':checked')) {
                                    // find checkbox where _name is tblAkn.checkbox.name
                                    _tableAkn.find('td').each(function () {
                                        var _aknCheckBox = $(this).find(":input[name$='chkId']"),
                                            _aknBitValue = $(this).find(":input[name$='BitValue']").val(),
                                            _aknName = $(this).eq(0).text().trim();
                                        if (_name == _aknName) {
                                            _aknCheckBox.prop('checked', true);

                                            if (_aknName == 'Department Funding') {
                                                $('#MainContent_ddlAknDepartmentFunding').show();
                                                $('#MainContent_ddlAknDepartmentFunding').parent().find('label').show();

                                            }
                                            if (_aknName == 'Other') {
                                                $('#MainContent_txtAknOther').show();
                                                $('#MainContent_txtAknOther').parent().find('label').show();
                                            }

                                        }
                                        // update bitsum for calculation
                                        if (_aknCheckBox.is(':checked'))
                                            _aknBitSum += parseInt(_aknBitValue, 10);

                                        if (_aknName == 'N/A' && _aknCheckBox.is(':checked')) {
                                            ToggleTable(_tableAkn, true);
                                        }
                                        else if (_aknBitSum == 0) {
                                            ToggleTable(_tableAkn, false);
                                        }

                                    });
                                }
                            });


                            // for each checkbox, if _checkbox is checked and if tblGrant.Name = tblAkn.name
                        }

                        if (_name == 'Department Funding' && _id == 'tblGrant') {


                            if (_checkBox.is(':checked')) {
                                //ddldropdown show
                                $('#MainContent_ddlDepartmentFunding').show();
                                $('#MainContent_ddlDepartmentFunding').parent().find('label').show();


                                $('#MainContent_ddlDepartmentFunding').change(function () {
                                    var selectedVal = this.value;

                                    if (selectedVal == 96 /*(Other)*/) {
                                        $('#MainContent_txtDeptFundOth').show();
                                        $('#MainContent_txtDeptFundOth').parent().find('label').show();

                                        $('#MainContent_chkDeptFundMouYes').prop('checked', false);
                                        $('#MainContent_chkDeptFundMouNo').prop('checked', false);
                                        $('#divDeptFundMou').hide();
                                    }
                                    else if (selectedVal == 62 /*(School of Nursing & Dental Hygiene)*/) {
                                        $('#divDeptFundMou').show();

                                        $('#MainContent_txtDeptFundOth').val('');
                                        $('#MainContent_txtDeptFundOth').hide();
                                        $('#MainContent_txtDeptFundOth').parent().find('label').hide();
                                    }
                                    else {
                                        $('#MainContent_txtDeptFundOth').val('');
                                        $('#MainContent_txtDeptFundOth').hide();
                                        $('#MainContent_txtDeptFundOth').parent().find('label').hide();

                                        $('#MainContent_chkDeptFundMouYes').prop('checked', false);
                                        $('#MainContent_chkDeptFundMouNo').prop('checked', false);
                                        $('#divDeptFundMou').hide();
                                    }
                                });

                                if ($('#MainContent_ddlDepartmentFunding').val() == 96 /*(Other)*/) {
                                    $('#MainContent_txtDeptFundOth').show();
                                    $('#MainContent_txtDeptFundOth').parent().find('label').show();
                                }
                                else if ($('#MainContent_ddlDepartmentFunding').val() == 62 /*(School of Nursing & Dental Hygiene)*/) {
                                    $('#divDeptFundMou').show();

                                    $('#MainContent_txtDeptFundOth').val('');
                                    $('#MainContent_txtDeptFundOth').hide();
                                    $('#MainContent_txtDeptFundOth').parent().find('label').hide();

                                }
                                else {
                                    $('#MainContent_txtDeptFundOth').val('');
                                    $('#MainContent_txtDeptFundOth').hide();
                                    $('#MainContent_txtDeptFundOth').parent().find('label').hide();

                                    $('#MainContent_chkDeptFundMouYes').prop('checked', false);
                                    $('#MainContent_chkDeptFundMouNo').prop('checked', false);
                                    $('#divDeptFundMou').hide();
                                }


                            }
                            else {
                                //ddldropdown hide
                                $('#MainContent_ddlDepartmentFunding').val('');
                                $('#MainContent_ddlDepartmentFunding').hide();
                                $('#MainContent_ddlDepartmentFunding').parent().find('label').hide();

                                $('#MainContent_txtDeptFundOth').val('');
                                $('#MainContent_txtDeptFundOth').hide();
                                $('#MainContent_txtDeptFundOth').parent().find('label').hide();

                                $('#MainContent_chkDeptFundMouYes').prop('checked', false);
                                $('#MainContent_chkDeptFundMouNo').prop('checked', false);
                                $('#divDeptFundMou').hide();
                            }

                        }

                        if (_name == 'Department Funding' && _id == 'tblAkn') {


                            if (_checkBox.is(':checked')) {
                                //ddldropdown show
                                $('#MainContent_ddlAknDepartmentFunding').show();
                                $('#MainContent_ddlAknDepartmentFunding').parent().find('label').show();


                                $('#MainContent_ddlAknDepartmentFunding').change(function () {
                                    var selectedVal = this.value;

                                    if (selectedVal == 96) {
                                        $('#MainContent_txtAknDeptFundOth').show();
                                        $('#MainContent_txtAknDeptFundOth').parent().find('label').show();
                                    }
                                    else {
                                        $('#MainContent_txtAknDeptFundOth').val('');
                                        $('#MainContent_txtAknDeptFundOth').hide();
                                        $('#MainContent_txtAknDeptFundOth').parent().find('label').hide();
                                    }
                                });

                                if ($('#MainContent_ddlAknDepartmentFunding').val() == 96) {
                                    $('#MainContent_txtAknDeptFundOth').show();
                                    $('#MainContent_txtAknDeptFundOth').parent().find('label').show();
                                } else {
                                    $('#MainContent_txtAknDeptFundOth').val('');
                                    $('#MainContent_txtAknDeptFundOth').hide();
                                    $('#MainContent_txtAknDeptFundOth').parent().find('label').hide();
                                }


                            }
                            else {
                                //ddldropdown hide
                                $('#MainContent_ddlAknDepartmentFunding').val('');
                                $('#MainContent_ddlAknDepartmentFunding').hide();
                                $('#MainContent_ddlAknDepartmentFunding').parent().find('label').hide();

                                $('#MainContent_txtAknDeptFundOth').val('');
                                $('#MainContent_txtAknDeptFundOth').hide();
                                $('#MainContent_txtAknDeptFundOth').parent().find('label').hide();
                            }

                        }


                        if (_checkBox.is(':checked'))
                            _bitSum += parseInt(_bitValue, 10);

                        if (_name == 'N/A' && _checkBox.is(':checked')) {
                            ToggleTable(_table, true);
                        }
                        else if (_bitSum == 0) {
                            ToggleTable(_table, false);
                        }
                    });

                    $(_textBitSum).val(_bitSum);


                }
            }
        }
        // Disables all checkboxes if checkbox choice "N/A" is selected.
        function ToggleTable(tbl, isNA) {
            tbl.find('td').each(function () {
                var _checkBox = $(this).find(":input[name$='chkId']"),
                    _bitValue = $(this).find(":input[name$='BitValue']").val(),
                    _name = $(this).eq(0).text().trim();

                if (isNA) {
                    if (_name != 'N/A') {
                        _checkBox.prop("checked", false);
                        _checkBox.prop("disabled", true);
                    }
                }
                else {
                    _checkBox.prop("disabled", false);
                }
            });
        }

        // DatePicker - initializes date for date fields.
        biostatNS.DatePicker = function (ctrlId) {
            var ctl = ctrlId;

            return {
                init: function (e) {
                    $('#' + ctl).datepicker({
                        todayHighlight: true,
                        format: "mm/dd/yyyy",
                        autoclose: true,
                        orientation: "top"
                    })
                }
                , onchange: function (e) {
                    $('#' + ctl).on('changeDate', function (e) {
                        //revalidate
                        if ($('#' + ctl).validate()) {
                            $('#' + ctl).closest('.row').removeClass('has-error');
                        }
                    });
                }
            }
        }

        // -- Initializes view of projects -- \\
        function bindProjects() {
            //var piId = $("#MainContent_ddlPI").val();
            var filterPI = $("#MainContent_ddlPI :selected").text();
            //var currentProjectId = $("#MainContent_ddlProjectHdn").val();

            $('#MainContent_lblPI').text(filterPI);
            $("#MainContent_ddlProjectHdn > option").not(":first").each(function () {
                if (this.text.indexOf(filterPI) > 0 || filterPI.length == 0 || filterPI.indexOf('Search') >= 0) {
                    $("#MainContent_ddlProjectHdn").children("option[value=" + this.value + "]").show();
                    $("#MainContent_ddlProject").children("option[value=" + this.value + "]").show();
                }
                else {
                    $("#MainContent_ddlProjectHdn").children("option[value=" + this.value + "]").hide();
                    $("#MainContent_ddlProject").children("option[value=" + this.value + "]").hide();
                }

                //if (this.value == currentProjectId) {
                //    $("#MainContent_ddlProject").val(this.value);
                //}
            });

            if (filterPI.length == 0 || filterPI.indexOf('Search') >= 0) {
                $('#MainContent_lblProjectId').text('');
                $('#MainContent_txtTitle').val('');
                $('#MainContent_txtSummary').val('');
                $('#MainContent_txtInitialDate').val('');
                $('#MainContent_txtDeadline').val('');
                $('#MainContent_ddlLeadBiostat').val('');

                $('#tblBiostat').find('td').each(function () {
                    $(this).find(":input[name$='chkId']").prop('checked', false);
                });

                $('#tblStudyArea').find('td').each(function () {
                    $(this).find(":input[name$='chkId']").prop('checked', false);
                });
                $('#MainContent_txtStudyAreaOther').val('');
                $('#MainContent_txtStudyAreaOther').hide();

                $('#tblHealthData').find('td').each(function () {
                    $(this).find(":input[name$='chkId']").prop('checked', false);
                });
                $('#MainContent_txtHealthDataOther').val('');
                $('#MainContent_txtHealthDataOther').hide();

                $('#tblStudyType').find('td').each(function () {
                    $(this).find(":input[name$='chkId']").prop('checked', false);
                });
                $('#MainContent_txtStudyTypeOther').val('');
                $('#MainContent_txtStudyTypeOther').hide();

                $('#tblStudyPopulation').find('td').each(function () {
                    $(this).find(":input[name$='chkId']").prop('checked', false);
                });
                $('#MainContent_txtStudyPopulationOther').val('');
                $('#MainContent_txtStudyPopulationOther').hide();

                $('#tblService').find('td').each(function () {
                    $(this).find(":input[name$='chkId']").prop('checked', false);
                });
                $('#MainContent_txtServiceOther').val('');
                $('#MainContent_txtServiceOther').hide();

                $('#tblGrant').find('td').each(function () {
                    $(this).find(":input[name$='chkId']").prop('checked', false);
                });
                $('#MainContent_txtGrantOther').val('');
                $('#MainContent_txtGrantOther').hide();
                $('#MainContent_txtGrantOther').parent().find('label').hide();

                $('#MainContent_ddlDepartmentFunding').val('');
                $('#MainContent_ddlDepartmentFunding').hide();
                $('#MainContent_ddlDepartmentFunding').parent().find('label').hide();

                $('#MainContent_txtDeptFundOth').val('');
                $('#MainContent_txtDeptFundOth').hide();
                $('#MainContent_txtDeptFundOth').parent().find('label').hide();

                $('#MainContent_chkDeptFundMouYes').prop('checked', false);
                $('#MainContent_chkDeptFundMouNo').prop('checked', false);
                $('#divDeptFundMou').hide();

                $('#tblAkn').find('td').each(function () {
                    $(this).find(":input[name$='chkId']").prop('checked', false);
                });
                $('#MainContent_txtAknOther').val('');
                $('#MainContent_txtAknOther').hide();
                $('#MainContent_txtAknOther').parent().find('label').hide();

                $('#MainContent_ddlAknDepartmentFunding').val('');
                $('#MainContent_ddlAknDepartmentFunding').hide();
                $('#MainContent_ddlAknDepartmentFunding').parent().find('label').hide();

                $('#MainContent_txtAknDeptFundOth').val('');
                $('#MainContent_txtAknDeptFundOth').hide();
                $('#MainContent_txtAknDeptFundOth').parent().find('label').hide();


                <%=ClientScript.GetPostBackEventReference(upPhase, "")%>

                //$('#MainContent_txtRequestRcvdDate').val('');                

                $('#MainContent_chkApproved').prop('checked', false);
                //$('#MainContent_chkJuniorPIYes').prop('checked', false);

                $('#tblDesc > tbody  > tr').each(function () {
                    var chkBox = $(this).find('input[type="checkbox"]');
                    if (chkBox.is(':checked')) {
                        chkBox.prop('checked', false);
                    }
                });

                $('#divHealthDisparity > div').each(function () {
                    var chkBox = $(this).find('input[type="checkbox"]');
                    if (chkBox.is(':checked')) {
                        chkbox.prop('checked', false);
                    }
                });


                $('#divLetterOfSupport > div').each(function () {
                    var chkBox = $(this).find('input[type="checkbox"]');
                    if (chkBox.is(':checked')) {
                        chkBox.prop('checked', false);
                    }
                });

                $('#MainContent_txtMentorFirstName').val('');
                $('#MainContent_txtMentorLastName').val('');
                $('#MainContent_txtMentorEmail').val('');
                $('#divMentor').hide();

                $('#MainContent_chkIsRmatrix').prop('checked', false);
                //$('#MainContent_chkRmatrixReport').prop('checked', false);
                $('#MainContent_chkReportToRmatrix').prop('checked', false);
                $('#MainContent_chkReportToOlaHawaii').prop('checked', false);

                $('#MainContent_txtRmatrixNum').val('');
                $('#MainContent_txtRmatrixSubDate').val('');

                $('#MainContent_chkIsOlaHawaii').prop('checked', false);

                $('#MainContent_txtOlaHawaiiNum').val('');
                $('#MainContent_txtOlaHawaiiSubDate').val('');

                $('#MainContent_txtCompletionDate').val('');
                $('#MainContent_txtProjectStatus').val('');
                $('#MainContent_txtComments').val('');
            }

        }

        // ToggleDiv - shows section if check; otherwise remain hidden.
        function ToggleDiv(checkBox, theDiv) {
            if (checkBox.is(":checked"))
                theDiv.show();
            else
                theDiv.hide();
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

        //function MakeSubmitClickableAgain() {
        //    var thisButton = $(".submitBtn[value='Processing......']");

        //    thisButton.prop("disabled", false);
        //    thisButton.prop("value", "Submit");
        //}


        // -- Unchecks other choices if choice is selected in specific section -- \\
        $("#MainContent_chkBiostat").change(function () {
            if (this.checked) {
                $('#MainContent_chkBioinfo').prop('checked', false);
            }
            else
                $('#MainContent_chkBioinfo').prop('checked', true);
        });

        $("#MainContent_chkBioinfo").change(function () {
            if (this.checked) {
                $('#MainContent_chkBiostat').prop('checked', false);
            }
            else
                $('#MainContent_chkBiostat').prop('checked', true);
        });

        //------------------------------------------------------------------
        $("#MainContent_chkRequestTypeRfunded").change(function () {
            if (this.checked) {
                $('#MainContent_chkRequestTypePilotPI').prop('checked', false);
                $('#MainContent_chkRequestTypeOther').prop('checked', false);
            }
            //else
            //    $('#MainContent_chkCreditToBioinfo').prop('checked', true);
        });

        $("#MainContent_chkRequestTypePilotPI").change(function () {
            if (this.checked) {
                $('#MainContent_chkRequestTypeRfunded').prop('checked', false);
                $('#MainContent_chkRequestTypeOther').prop('checked', false);
            }
        });

        $("#MainContent_chkRequestTypeOther").change(function () {
            if (this.checked) {
                $('#MainContent_chkRequestTypeRfunded').prop('checked', false);
                $('#MainContent_chkRequestTypePilotPI').prop('checked', false);
            }
        });

        //-----------------------------------------------------------------
        $("#MainContent_chkHealthDisparityYes").change(function () {
            if (this.checked) {
                $('#MainContent_chkHealthDisparityNo').prop('checked', false);
                $('#MainContent_chkHealthDisparityNA').prop('checked', false);
            }
        });

        $("#MainContent_chkHealthDisparityNo").change(function () {
            if (this.checked) {
                $('#MainContent_chkHealthDisparityYes').prop('checked', false);
                $('#MainContent_chkHealthDisparityNA').prop('checked', false);
            }
        });

        $("#MainContent_chkHealthDisparityNA").change(function () {
            if (this.checked) {
                $('#MainContent_chkHealthDisparityYes').prop('checked', false);
                $('#MainContent_chkHealthDisparityNo').prop('checked', false);
            }
        });

        //-----------------------------------------------------------------
        $("#MainContent_chkLetterOfSupportYes").change(function () {
            if (this.checked) {
                $('#MainContent_chkLetterOfSupportNo').prop('checked', false);
                $('#MainContent_chkLetterOfSupportNA').prop('checked', false);
            }
        });

        $("#MainContent_chkLetterOfSupportNo").change(function () {
            if (this.checked) {
                $('#MainContent_chkLetterOfSupportYes').prop('checked', false);
                $('#MainContent_chkLetterOfSupportNA').prop('checked', false);
            }
        });

        $("#MainContent_chkLetterOfSupportNA").change(function () {
            if (this.checked) {
                $('#MainContent_chkLetterOfSupportYes').prop('checked', false);
                $('#MainContent_chkLetterOfSupportNo').prop('checked', false);
            }
        });

        //-----------------------------------------------------------------
        $("#MainContent_chkDeptFundMouYes").change(function () {
            if (this.checked) {
                $('#MainContent_chkDeptFundMouNo').prop('checked', false);
            }
        });

        $("#MainContent_chkDeptFundMouNo").change(function () {
            if (this.checked) {
                $('#MainContent_chkDeptFundMouYes').prop('checked', false);
            }
        });

        //-----------------------------------------------------------------

        $("#MainContent_chkCreditToBiostat").change(function () {
            if (this.checked) {
                $('#MainContent_chkCreditToBioinfo').prop('checked', false);
                $('#MainContent_chkCreditToBoth').prop('checked', false);
            }
            //else
            //    $('#MainContent_chkCreditToBioinfo').prop('checked', true);
        });

        $("#MainContent_chkCreditToBioinfo").change(function () {
            if (this.checked) {
                $('#MainContent_chkCreditToBiostat').prop('checked', false);
                $('#MainContent_chkCreditToBoth').prop('checked', false);
            }
        });

        $("#MainContent_chkCreditToBoth").change(function () {
            if (this.checked) {
                $('#MainContent_chkCreditToBiostat').prop('checked', false);
                $('#MainContent_chkCreditToBioinfo').prop('checked', false);
            }
        });

        //------------------------------------------------------------------


    </script>
</asp:Content>
