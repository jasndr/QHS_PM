<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ImportRequest.aspx.cs" Inherits="ProjectManagement.ImportRequest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<script src="Scripts/jquery-1.10.2.min.js"></script>--%>
    <script src="Scripts/jquery.bootpag.min.js"></script>
    <script src="Scripts/jquery.bootstrap.wizard.min.js"></script>
    <script src="Scripts/bootstrap-datepicker.min.js"></script>
    <script src="Scripts/fileinput.js"></script>
    <link href="Content/fileinput.css" rel="stylesheet" />
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

        #p_text {
            font-size: 11pt;
            font-weight: 300;
        }

        .file-preview-frame {
            height: /*482px*/ 30% !important;
            width: 100% !important;
        }

        .fileinput-upload-button {
            display: none !important;
        }

        .file-footer-buttons {
            display: none !important;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">

        <div class="panel panel-default">
            <div class="panel-heading"><strong>Import RMATRIX/Ola Hawaii Request Form</strong></div>
            <div class="panel-body">
                <div>
                    <p id="p_text">
                        This form will be used to generate a Principal Investigator and Project record into the tracking system
                        from the PDF form of the RMATRIX or Ola Hawaii Request form so users will no longer have to enter the
                        information into the system manually.  Please upload the file below and confirm that the information
                        is correct before submitting.  Mahalo.
                    </p>
                </div>
                <br />
                <div>
                    <div class="fileinput fileinput-new" data-provides="fileinput">
                        <span class="btn btn-default btn-file"><span>Upload PDF of RMATRIX/Ola Hawaii Request Form here</span>
                            <input type="file" class="file" id="fileUpload" name="fileUpload" runat="server" />
                        </span>
                    </div>
                </div>
                <br />
                <%--<div>
                    Uploaded file:
                    <asp:LinkButton ID="lnkFile" runat="server"></asp:LinkButton>
                    OnClick="DownloadFile"
                </div>--%>
                <div>
                    <asp:Button ID="btnImport" runat="server" Text="Import" CssClass="btn btn-info" OnClientClick="return validateControl(); return false;" OnClick="btnImport_Click" /> <%--OnClientClick="return validateControl(); return false;"--%>
                </div>
            </div>
        </div>


        <div id="editModal" class="modal fade" role="dialog" aria-labelledby="gridSystemModalLabel" aria-hidden="true"
            data-backdrop="static" data-keyboard="false">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">

                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <%--<h5 style="font-weight: normal;"><strong><em>Please edit/review the following PI/Project pages before importing.
                        </em></strong></h5>--%>
                        <p style="font-size: 11pt; font-weight: 500">
                            <em>Instructions: Please review the information extracted from the uploaded request form for both the PI
                                             and Project Information and edit/add fields as necessary.  When ready to import to the database,
                                             please click "Upload" to submit.
                            </em>
                        </p>
                    </div>
                    <asp:UpdatePanel ID="upEdit" runat="server">
                        <ContentTemplate>
                            <div class="loader"></div>
                            <div class="modal-body">
                                <div id="rootwizard">
                                    <ul>
                                        <li class="hidden"><a href="#pi_tab" data-toggle="tab"></a></li>
                                        <li class="hidden"><a href="#project_tab" data-toggle="tab"></a></li>
                                    </ul>
                                    <div class="tab-content">
                                        <div class="tab-pane" id="pi_tab">
                                            <h4 id="editModalLabel">PI Form</h4>
                                            <hr />
                                            <div class="row">
                                                <asp:Label ID="lblInvestId" runat="server" Visible="false"></asp:Label>
                                            </div>
                                            <div class="row">
                                                <div class="col-xs-6 col-md-6">
                                                    <label for="TextBoxFirstName">First Name</label>
                                                    <asp:TextBox ID="TextBoxFirstName" runat="server" placeholder="First Name" class="form-control"></asp:TextBox>
                                                </div>
                                                <div class="col-xs-6 col-md-6">
                                                    <label for="TextBoxLastName">Last Name</label>
                                                    <asp:TextBox ID="TextBoxLastName" runat="server" placeholder="Last Name" class="form-control"></asp:TextBox>
                                                </div>
                                            </div>
                                            <br />
                                            <div class="row">
                                                <div class="col-xs-6 col-md-6">
                                                    <label for="TextBoxEmail">Email</label>
                                                    <asp:TextBox ID="TextBoxEmail" runat="server" placeholder="Email" class="form-control"></asp:TextBox>
                                                </div>
                                                <div class="col-xs-2 col-md-2">
                                                    <label for="chkPilot">Pilot Investigator</label>
                                                    <asp:CheckBox ID="chkPilot" runat="server"></asp:CheckBox>
                                                </div>
                                            </div>
                                            <br />
                                            <div class="row">
                                                <div class="col-xs-6 col-md-6">
                                                    <label for="TextBoxPhone">Phone</label>
                                                    <asp:TextBox ID="TextBoxPhone" runat="server" placeholder="(___) ___-____" class="form-control"></asp:TextBox>
                                                </div>
                                            </div>
                                            <br />
                                            <div class="row">
                                                <div class="col-xs-6 col-md-6">
                                                    <label for="ddlStatus">Status</label>
                                                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control" onchange="divNonHawaiiToggle();"></asp:DropDownList>
                                                </div>
                                                <div class="col-xs-6 col-md-6" id="divNonHawaii">
                                                    <label for="TextBoxNonHawaii" id="lblOtherDegree">Other Degree</label>
                                                    <asp:TextBox ID="TextBoxNonHawaii" runat="server" class="form-control"></asp:TextBox>
                                                </div>
                                                <div class="col-xs-6 col-md-6" id="divUHFaculty">
                                                    <label for="ddlUHFaculty">UH Faculty</label>
                                                    <asp:DropDownList ID="ddlUHFaculty" runat="server" CssClass="form-control"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <br />
                                            <div class="row">
                                                <div class="col-xs-6 col-md-6">
                                                    <div class="panel panel-default">
                                                        <div class="panel-heading">Degree</div>
                                                        <div class="panel-body">
                                                            <asp:GridView ID="GridViewDegree" runat="server" AutoGenerateColumns="False" class="table table-bordered table-hover" ShowFooter="true">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Select" HeaderStyle-Width="20%">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkRow" runat="server" OnClick="GridViewDegreeToggle(this);"></asp:CheckBox>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Id" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblId" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Name" HeaderStyle-Width="80%">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:TextBox ID="txtDegreeOther" runat="server" class="form-control"></asp:TextBox>
                                                                        </FooterTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <hr />
                                            <span>Affiliations</span>
                                            <div class="row">
                                                <div class="col-xs-6 col-md-6">
                                                    <div class="panel panel-default">
                                                        <div class="panel-heading">Department </div>
                                                        <div class="panel-body">
                                                            <asp:GridView ID="GridViewDept" runat="server" AutoGenerateColumns="False" class="table table-bordered table-hover">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Select">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkRow" runat="server"></asp:CheckBox>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Id" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblId" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Name">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-xs-6 col-md-6">
                                                    <div class="panel panel-default">
                                                        <div class="panel-heading">Office </div>
                                                        <div class="panel-body">
                                                            <asp:GridView ID="GridViewOffice" runat="server" AutoGenerateColumns="False" class="table table-bordered table-hover">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Select">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkRow" runat="server"></asp:CheckBox>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Id" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblId" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Name">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-xs-6 col-md-6">
                                                    <div class="panel panel-default">
                                                        <div class="panel-heading">College </div>
                                                        <div class="panel-body">
                                                            <asp:DropDownList ID="ddlJabsomOther" runat="server" CssClass="form-control" onchange="gridviewUHDeptToggle();">
                                                            </asp:DropDownList>
                                                            <br />
                                                            <div id="divGridViewUHDep">
                                                                <asp:GridView ID="GridViewUHDept" runat="server" AutoGenerateColumns="False" class="table table-bordered table-hover" Style="display: none">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Select">
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chkRow" runat="server"></asp:CheckBox>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Id" Visible="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblId" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Name">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </div>
                                                            <div id="divGridViewCommunityCollege">
                                                                <asp:GridView ID="GridViewCommunityCollege" runat="server" AutoGenerateColumns="False" class="table table-bordered table-hover" Style="display: none">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Select">
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chkRow" runat="server"></asp:CheckBox>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Id" Visible="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblId" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Name">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </div>
                                                            <div id="divGridViewCommunityPartner">
                                                                <asp:GridView ID="GridViewCommunityPartner" runat="server" AutoGenerateColumns="False" class="table table-bordered table-hover" Style="display: none" ShowFooter="true">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Select">
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chkRow" runat="server" OnClick="GridViewCommunityPartnerToggle(this)"></asp:CheckBox>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Id" Visible="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblId" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Name">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:TextBox ID="txtCommunityPartnerOther" runat="server" class="form-control"></asp:TextBox>
                                                                            </FooterTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-xs-6 col-md-6">
                                                    <div class="panel panel-default">
                                                        <div class="panel-heading">Non-UH Hawaii Client</div>
                                                        <%--<div class="panel-body">
                                            <asp:DropDownList ID="ddlHospital" runat="server" CssClass="form-control" onchange="gridviewUHDeptToggle();">
                                            </asp:DropDownList>
                                            <br />
                                            <div id="divGridViewHph">
                                                <asp:GridView ID="GridViewHph" runat="server" AutoGenerateColumns="False" class="table table-bordered table-hover" Caption="Hawaii Pacific Health Hospitals (On Oahu):" Style="display: none">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Select">
                                                            <ItemTemplate>
                                                                <asp:RadioButton ID="chkRow" runat="server" OnClick="SelectSingleRadiobutton(this.id)" GroupName="hospitalMenu"></asp:RadioButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Id" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblId" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Name">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>--%>
                                                        <div class="panel-body">
                                                            <asp:GridView ID="GridViewNonUH" runat="server" AutoGenerateColumns="False" class="table table-bordered table-hover" ShowFooter="true">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Select">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkRow" runat="server" OnClick="GridViewNonUHToggle(this);"></asp:CheckBox>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Id" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblId" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Name">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:TextBox ID="txtNonUHOther" runat="server" class="form-control"></asp:TextBox>
                                                                        </FooterTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>


                                        </div>

                                        <div class="tab-pane" id="project_tab">

                                            <h4 id="editModalLabel2">Project Form</h4>
                                            <hr />
                                            <div class="row">
                                                <div class="col-sm-1 text-left">
                                                    <label class="control-label" for="TxtPI">PI:</label>
                                                </div>
                                                <div class="col-sm-3">
                                                    <%--<input class="control-label" type="text" name="TxtPI" id="TxtPI" runat="Server" />--%>
                                                    <asp:TextBox ID="TxtPI" runat="Server" CssClass="form-control"></asp:TextBox>
                                                </div>
                                            </div>
                                            <br />
                                            <div class="row">
                                                <div class="col-sm-1 text-left">
                                                    <label class="control-label" for="TxtProject">Project:</label>
                                                </div>
                                                <div class="col-sm-6">
                                                    <%--<input class="control-label" type="text" name="TxtProject" id="TxtProject" runat="server" />--%>
                                                    <asp:TextBox ID="TxtProject" runat="Server" CssClass="form-control"></asp:TextBox>
                                                </div>
                                                <div class="col-sm-1 offset1">
                                                    <asp:CheckBox ID="chkBiostat" runat="server" Text="Biostat" />
                                                </div>
                                                <div class="col-sm-1 offset1">
                                                    <asp:CheckBox ID="chkBioinfo" runat="server" Text="Bioinfo" />
                                                </div>
                                            </div>
                                            <br />
                                            <div class="row">
                                                <div class="col-sm-1 text-left">
                                                    <label class="control-label" for="TxtTitle">Title:</label>
                                                </div>
                                                <div class="col-sm-9">
                                                    <%--                                                <input class="control-label" type="text" name="txtTitle7" id="txtTitle7" runat="server" />--%>
                                                    <asp:TextBox ID="TxtTitle" runat="server" CssClass="form-control"></asp:TextBox>
                                                </div>
                                                <div class="col-sm-1 hidden">
                                                    <asp:DropDownList ID="ddlProjectHdn" runat="server" CssClass="form-control"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <br />
                                            <div class="row">
                                                <div class="col-sm-1 text-left">
                                                    <label class="control-label" for="TxtSummary">Summary:</label>
                                                </div>
                                                <div class="col-sm-9">
                                                    <textarea class="form-control noresize" rows="3" name="TxtSummary" id="TxtSummary" runat="server"></textarea>
                                                </div>
                                            </div>
                                            <br />
                                            <div class="row">
                                                <div class="col-sm-1">
                                                    <label class="control-label" for="txtInitialDate">Initial date:</label>
                                                </div>
                                                <div class="col-sm-2">
                                                    <div class="input-group date" id='dtpInitialDate'>
                                                        <span class="input-group-addon">
                                                            <span class="glyphicon glyphicon-calender"></span>
                                                        </span>
                                                        <asp:TextBox ID="txtInitialDate" runat="server" CssClass="form-control">
                                                        </asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-sm-2 text-right">
                                                    <label class="control-label" for="txtDeadline">Deadline:</label>
                                                </div>
                                                <div class="col-sm-2">
                                                    <div class="input-group date" id="dtpDeadline">
                                                        <span class="input-group-addon">
                                                            <span class="glyphicon glyphicon-calendar"></span>
                                                        </span>
                                                        <asp:TextBox ID="txtDeadline" runat="server" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <br />
                                            <h5>QHS Faculty/Staff</h5>
                                            <div class="row">
                                                <div class="col-sm-2 text-left">
                                                    <label class="control-label" for="ddlLeadMember">Lead member:</label>
                                                </div>
                                                <div class="col-sm-3">
                                                    <asp:DropDownList ID="ddlLeadBiostat" runat="server" CssClass="form-control"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <br />
                                            <div class="row">
                                                <div class="col-sm-6 text-left">
                                                    <label class="control-label">Other member(s): check all that apply, or indicate N/A</label>
                                                </div>
                                            </div>
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
                                            <div class="col-sm-6 text-left">
                                                <label class="control-label">Check all that apply</label>
                                            </div>
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
                                                    <input class="form-control" type="text" name="txtStudyAreaOther" id="txtStiduAreaOther" runat="server" />
                                                </div>
                                                <div class="col-sm-2">
                                                    <input class="form-control hidden" type="text" name="txtStudyAreaBitSum" id="txtStudyAreaBitSum" runat="server" />
                                                </div>
                                            </div>

                                            <h5>Health Data</h5>
                                            <div class="row">
                                                <div class="col-sm-6 text-left">
                                                    <label class="control-label">Check all that apply, or indicate N/A</label>
                                                </div>
                                            </div>
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
                                                    <input class="form-control hidden" type="text" name="txtHealthDataBitSum" id="txtHealthDataBitSum" runat="server" />
                                                </div>
                                            </div>

                                            <h5>Study Type</h5>
                                            <div class="row">
                                                <div class="col-sm-6 text-left">
                                                    <label class="control-label">Check all that apply</label>
                                                </div>
                                            </div>
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
                                                    <input class="form-control" type="text" name="txtStudyTypeOther" id="txtStudyTypeOther" runat="server" />
                                                </div>
                                                <div class="col-sm-2">
                                                    <input class="form-control hidden" type="text" name="txtStudyTypeBitSum" runat="server" />
                                                </div>
                                            </div>

                                            <h5>Study Population</h5>
                                            <div class="row">
                                                <div class="col-sm-6 text-left">
                                                    <label class="control-label">Check all that apply, or indicate N/A</label>
                                                </div>
                                            </div>
                                            <table class="table-striped table-hover table-borderless" id="tblStudyPopulation">
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
                                                        <asp:CheckBox ID="chkHealthDisparityYes" runat="server" Text="Yes" />
                                                    </div>
                                                    <div class="col-sm-3">
                                                        <asp:CheckBox ID="chkHealthDisparityNo" runat="server" Text="Yes" />
                                                    </div>
                                                    <div class="col-sm-3">
                                                        <asp:CheckBox ID="chkHealthDisparityNA" runat="server" Text="N/A" />
                                                    </div>
                                                </div>
                                                <div class="col-sm-1"></div>
                                                <div class="col-sm-6">
                                                    <input class="form-control" type="text" name="txtStudyPopulationOther" id="txtStudyPopulationOther" runat="Server" />
                                                </div>
                                                <div class="col-sm-1">
                                                    <input class="form-control hidden" type="text" name="txtStudyPopulationBitSum" id="txtStudyPopulationBitSum" runat="server" />
                                                </div>
                                            </div>

                                            <h5>Service</h5>
                                            <div class="row">
                                                <div class="col-sm-6 text-left">
                                                    <label class="control-label">Check all that apply</label>
                                                </div>
                                            </div>
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
                                                        <asp:CheckBox ID="chkLetterOfSupportYes" runat="server" Text="Yes" />
                                                    </div>
                                                    <div class="col-sm-3">
                                                        <asp:CheckBox ID="chkLetterOfSupportNo" runat="server" Text="No" />
                                                    </div>
                                                </div>
                                                <div class="col-sm-4">
                                                    <input class="form-control" type="text" name="txtServiceOther" id="txtServiceOther" runat="Server" />
                                                </div>
                                                <div class="col-sm-2">
                                                    <input class="form-control hidden" type="text" name="txtServiceBitSum" id="txtServiceBitSum" runat="server" />
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
                                                    <asp:CheckBox ID="chkCreditToBiostat" runat="server" Text="Biostat Only" />
                                                </div>
                                                <div class="col-sm-2">
                                                    <asp:CheckBox ID="chkCreditToBioinfo" runat="server" Text="Bioinfo Only" />
                                                </div>
                                                <div class="col-sm-3">
                                                    <asp:CheckBox ID="chkCreditToBoth" runat="server" Text="Biostat and Bioinfo" />
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
                                                    <label class="control-label" for="txtMentorFirstName">Mentor first name:</label>
                                                </div>
                                                <div class="col-sm-2">
                                                    <input class="form-control" type="text" name="txtMentorFirstName" id="txtMentorFirstName" runat="Server" />
                                                </div>
                                                <div class="col-sm-2 text-right">
                                                    <label class="control-label" for="txtMentorLastName">Mentor last name:</label>
                                                </div>
                                                <div class="col-sm-2">
                                                    <input class="form-control" type="text" name="txtMentorLastName" id="txtMentorLastName" runat="Server" />
                                                </div>
                                                <div class="col-sm-1 text-right">
                                                    <label class="control-label" for="txtMentorEmail">Email</label>
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
                                            <div class="row">
                                                <div class="col-sm-6 text-left">
                                                    <label class="control-label" style="text-align: left;">
                                                        Check all that apply, or indicate N/A.
                                                        <br />
                                                        <em style="font-weight: normal;">(This section is used to mark funding sources, <u>not</u> grants acknowledged.)</em><br />
                                                    </label>
                                                </div>
                                            </div>
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
                                                        <asp:DropDownList ID="ddlDepartmentFunding" runat="server" CssClass="form-control"></asp:DropDownList>
                                                    </div>
                                                    <div class="col-sm-1"></div>
                                                    <div class="col-sm-3">
                                                        <label class="control-label" for="textDeptFundOth">Department Funding - Other</label>
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
                                                                <asp:CheckBox ID="chkDeptFundMouYes" runat="server" Text="Yes" />
                                                            </div>
                                                            <div class="col-sm-1">
                                                                <asp:CheckBox ID="chkDeptFundMouNo" runat="server" Text="No" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <br />

                                            <h5>Acknowledgements</h5>
                                            <div class="row">
                                                <div class="col-sm-11 text-left">
                                                    <label class="control-label" style="text-align: left;">
                                                        Check all that apply, or indicate N/A.
                                                        <br />
                                                        <em style="font-weight: normal;">(This section is used to mark entities to acknowledge, which 
                                                         may or may not be in addition to the source of funding. Funding soures have been checked
                                                         by default; please check/uncheck necessary acknowledgements.)</em><br />
                                                    </label>
                                                </div>
                                            </div>
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
                                                    <asp:DropDownList ID="ddlAknDepartmentFunding" runat="server" CssClass="form-control"></asp:DropDownList>
                                                </div>
                                                <div class="col-sm-1"></div>
                                                <div class="col-sm-3">
                                                    <label class="control-label" for="txtAknDeptFundOther">Department Funding - Other</label>
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


                                            <div class="modal-footer">
                                                <asp:Label ID="lblResult" Visible="false" runat="server"></asp:Label>
                                                <asp:Button ID="btnSave" runat="server" Text="Upload" CssClass="btn btn-info" OnClick="btnUpload_Click" OnClientClick="return ClientSideClick(this);" />
                                                <%--OnClick="btnSave_Click"--%>
                                                <button class="btn btn-info" data-dismiss="modal" aria-hidden="true">Close</button>
                                            </div>
                                        </div>

                                        <ul class="pager wizard">
                                            <li class="previous first" style="display: none;"><a href="#">First</a></li>
                                            <li class="previous"><a href="#">Previous</a></li>
                                            <li class="next last" style="display: none;"><a href="#"></a></li>
                                            <li class="next"><a href="#">Next</a></li>
                                        </ul>

                                    </div>
                                    <%-- --%>
                                </div>
                            </div>

                            <%--<div class="modal-footer">
                                <asp:Label ID="lblResult" Visible="false" runat="server"></asp:Label>
                                <asp:Button ID="btnSave" runat="server" Text="Update" CssClass="btn btn-info" OnClientClick="return validateControl();" />
                                --OnClick="btnSave_Click"--
                            <button class="btn btn-info" data-dismiss="modal" aria-hidden="true">Close</button>
                            </div>--%>
                        </ContentTemplate>
                        <%--<Triggers>
                            <asp:AsyncPostBackTrigger ControlID="rptPIProject" EventName="ItemCommand" /> 
                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        </Triggers>--%>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">

        $(document).ready(function () {

            $('.loader').fadeOut("slow");

            $('#rootwizard').bootstrapWizard({
                onTabShow: function (tab, navigation, index) {
                    $('#rootwizard').scrollTop(0);
                }
            });

            $('.next').click(function () {
                $('#editModal').scrollTop(0); //.animate({ scrollTop: 0 }, 'slow');
                return false;
            });

            bindProjects();

            // Makes "RMATRIX" and "Ola Hawaii" checkboxes checked by default
            // if there is no ID already tied to the project form.
            var projectId = $("#MainContent_lblProjectId").text();
            if (projectId === null || projectId === 0) {
                $('#tblAkn').find('td').each(function () {
                    var _aknCheckBox = $(this).find(":input[name$='chkId']"),
                        //_aknBitValue = $(this).find(":input[name$='BitValue']").val(),
                        _aknName = $(this).eq(0).text().trim();

                    if (_aknName === 'RMATRIX' || _aknName === 'Ola Hawaii') {
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
                        if ($(this).val() === 1)
                            $(this).next().removeAttr("checked");
                        if ($(this).val() === 0)
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
            var projectId2 = $("#MainContent_lblProjectId").text();
            if (projectId2 > 0) {
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
                    if (e.keyCode === 13) { //if this is enter key
                        e.preventDefault();
                        return false;
                    }
                    else
                        return true;
                });
            });

        });

        // TableToggle - Makes "other" appear if "other" checkboxes exists.
        //               Also creates and adds bitsum for each checkbox checked.


        var biostatNS = biostatNS || {};

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

                        if (_name === 'Other' || _name === 'International Populations') {

                            //alert(_id + ' is the table id');

                            if (_checkBox.is(':checked')) {
                                _textOther.show();

                                if (_id === 'tblGrant' || _id === 'tblAkn') {
                                    _textOther.parent().find('label').show();
                                }
                            }
                            else {
                                _textOther.hide();
                                $(_textOther).val('');

                                if (_id === 'tblGrant' || _id === 'tblAkn') {
                                    _textOther.parent().find('label').hide();
                                }

                            }
                        }

                        // check equivalent checkbox in "funding source (grant)" section to "acknowledgements" section.
                        if (_id === 'tblGrant') {
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
                                        if (_name === _aknName) {
                                            _aknCheckBox.prop('checked', true);

                                            if (_aknName === 'Department Funding') {
                                                $('#MainContent_ddlAknDepartmentFunding').show();
                                                $('#MainContent_ddlAknDepartmentFunding').parent().find('label').show();

                                            }
                                            if (_aknName === 'Other') {
                                                $('#MainContent_txtAknOther').show();
                                                $('#MainContent_txtAknOther').parent().find('label').show();
                                            }

                                        }
                                        // update bitsum for calculation
                                        if (_aknCheckBox.is(':checked'))
                                            _aknBitSum += parseInt(_aknBitValue, 10);

                                        if (_aknName === 'N/A' && _aknCheckBox.is(':checked')) {
                                            ToggleTable(_tableAkn, true);
                                        }
                                        else if (_aknBitSum === 0) {
                                            ToggleTable(_tableAkn, false);
                                        }

                                    });
                                }
                            });


                            // for each checkbox, if _checkbox is checked and if tblGrant.Name = tblAkn.name
                        }

                        if (_name === 'Department Funding' && _id === 'tblGrant') {


                            if (_checkBox.is(':checked')) {
                                //ddldropdown show
                                $('#MainContent_ddlDepartmentFunding').show();
                                $('#MainContent_ddlDepartmentFunding').parent().find('label').show();


                                $('#MainContent_ddlDepartmentFunding').change(function () {
                                    var selectedVal = this.value;

                                    if (selectedVal === 96 /*(Other)*/) {
                                        $('#MainContent_txtDeptFundOth').show();
                                        $('#MainContent_txtDeptFundOth').parent().find('label').show();

                                        $('#MainContent_chkDeptFundMouYes').prop('checked', false);
                                        $('#MainContent_chkDeptFundMouNo').prop('checked', false);
                                        $('#divDeptFundMou').hide();
                                    }
                                    else if (selectedVal === 62 /*(School of Nursing & Dental Hygiene)*/) {
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

                                if ($('#MainContent_ddlDepartmentFunding').val() === 96 /*(Other)*/) {
                                    $('#MainContent_txtDeptFundOth').show();
                                    $('#MainContent_txtDeptFundOth').parent().find('label').show();
                                    $('#divDeptFundMou').hide();
                                }
                                else if ($('#MainContent_ddlDepartmentFunding').val() === 62 /*(School of Nursing & Dental Hygiene)*/) {
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

                        if (_name === 'Department Funding' && _id === 'tblAkn') {


                            if (_checkBox.is(':checked')) {
                                //ddldropdown show
                                $('#MainContent_ddlAknDepartmentFunding').show();
                                $('#MainContent_ddlAknDepartmentFunding').parent().find('label').show();


                                $('#MainContent_ddlAknDepartmentFunding').change(function () {
                                    var selectedVal = this.value;

                                    if (selectedVal === 96) {
                                        $('#MainContent_txtAknDeptFundOth').show();
                                        $('#MainContent_txtAknDeptFundOth').parent().find('label').show();
                                    }
                                    else {
                                        $('#MainContent_txtAknDeptFundOth').val('');
                                        $('#MainContent_txtAknDeptFundOth').hide();
                                        $('#MainContent_txtAknDeptFundOth').parent().find('label').hide();
                                    }
                                });

                                if ($('#MainContent_ddlAknDepartmentFunding').val() === 96) {
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

                        if (_name === 'N/A' && _checkBox.is(':checked')) {
                            ToggleTable(_table, true);
                        }
                        else if (_bitSum === 0) {
                            ToggleTable(_table, false);
                        }
                    });

                    $(_textBitSum).val(_bitSum);


                }
            };
        };

        // Disables all checkboxes if checkbox choice "N/A" is selected.
        function ToggleTable(tbl, isNA) {
            tbl.find('td').each(function () {
                var _checkBox = $(this).find(":input[name$='chkId']"),
                    _bitValue = $(this).find(":input[name$='BitValue']").val(),
                    _name = $(this).eq(0).text().trim();

                if (isNA) {
                    if (_name !== 'N/A') {
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
                    });
                }
                , onchange: function (e) {
                    $('#' + ctl).on('changeDate', function (e) {
                        //revalidate
                        if ($('#' + ctl).validate()) {
                            $('#' + ctl).closest('.row').removeClass('has-error');
                        }
                    });
                }
            };
        };


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
                    });
                }
                , onchange: function (e) {
                    $('#' + ctl).on('changeDate', function (e) {
                        //revalidate
                        if ($('#' + ctl).validate()) {
                            $('#' + ctl).closest('.row').removeClass('has-error');
                        }
                    });
                }
            };
        };

        // -- Initializes view of projects -- \\
        function bindProjects() {

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

            <%--= ClientScript.GetPostBackEventReference(upPhase, "")--%>


            $('#MainContent_chkApproved').prop('checked', false);

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
            // make sure the button is not type "submit" but "button
            if (myButton.getAttribute('type') === 'button') {
                // disable the button
                myButton.disabled = true;
                myButton.value = "Processing......";
            }
            return true;
        }

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

        function pageLoad(sender, args) {

            $('#editModal').on('shown.bs.modal', function () {
                $('#editModal').scrollTop(0);
            });

            gridviewUHDeptToggle();

            bindDivNonHawaii();

            if (document.getElementById('MainContent_GridViewNonUH_chkRow_8').checked) {
                $('#MainContent_GridViewNonUH_txtNonUHOther').show();
            }
            else {
                $('#MainContent_GridViewNonUH_txtNonUHOther').hide();
            }

            if (document.getElementById('MainContent_GridViewDegree_chkRow_10').checked) {
                $('#MainContent_GridViewDegree_txtDegreeOther').show();
            }
            else {
                $('#MainContent_GridViewDegree_txtDegreeOther').hide();
            }

            if (document.getElementById('MainContent_GridViewCommunityPartner_chkRow_4').checked) {
                $('#MainContent_GridViewCommunityPartner_txtCommunityPartnerOther').show();
            } else {
                $('#MainContent_GridViewCommunityPartner_txtCommunityPartnerOther').hide();
            }

        }
        

        function divNonHawaiiToggle() {
            bindDivNonHawaii();
            $("#MainContent_TextBoxNonHawaii").val('');
        }

        function bindDivNonHawaii() {
            var DropdownListStatus = document.getElementById('<%=ddlStatus.ClientID %>');
            var SelectedIndex = DropdownListStatus.selectedIndex;

            var SelectedText = DropdownListStatus.options[DropdownListStatus.selectedIndex].text;

            if (SelectedText.indexOf("Non-Hawaii") > -1 || SelectedText.indexOf("UH Student") > -1) {
                <%--document.getElementById('<%=GridViewNonUH.ClientID %>').style.display = "inherit";--%>
                document.getElementById('divNonHawaii').style.display = "inherit";

                if (SelectedText.indexOf("UH Student") > -1)
                    $("#lblOtherDegree").text("Student Degree");
                else
                    $("#lblOtherDegree").text("Institution/Organization");
            }
            else
                document.getElementById('divNonHawaii').style.display = "none";

            if (SelectedText.indexOf("UH Faculty") > -1)
                document.getElementById('divUHFaculty').style.display = "inherit";
            else
                document.getElementById('divUHFaculty').style.display = "none";

            //if (SelectedText.indexOf("UH Student") > -1)
            //    document.getElementById('divStudent').style.display = "inherit";
            //else
            //    document.getElementById('divStudent').style.display = "none";
            //$('#MainContent_TextBoxNonHawaii').val('');

        }

        function GridViewDegreeToggle(e) {
            if (e.id === "MainContent_GridViewDegree_chkRow_10") {
                if (e.checked)
                    $('#MainContent_GridViewDegree_txtDegreeOther').show();
                else {
                    $('#MainContent_GridViewDegree_txtDegreeOther').val('');
                    $('#MainContent_GridViewDegree_txtDegreeOther').hide();
                }
            }
        }

        function GridViewCommunityPartnerToggle(e) {
            if (e.id === 'MainContent_GridViewCommunityPartner_chkRow_4') {
                if (e.checked)
                    $('#MainContent_GridViewCommunityPartner_txtCommunityPartnerOther').show();
                else {
                    $('#MainContent_GridViewCommunityPartner_txtCommunityPartnerOther').val('');
                    $('#MainContent_GridViewCommunityPartner_txtCommunityPartnerOther').hide();
                }

            }
        }

        function gridviewUHDeptToggle() {
            var DropdownListJabsomOther = document.getElementById('<%=ddlJabsomOther.ClientID%>');
            var SelectedIndexJabsomOther = DropdownListJabsomOther.selectedIndex;

            var SelectedTextJabsomOther = DropdownListJabsomOther.options[DropdownListJabsomOther.selectedIndex].text;

            if (SelectedTextJabsomOther.indexOf("UH School") > -1)
                document.getElementById('<%=GridViewUHDept.ClientID %>').style.display = "inherit";
            else
                document.getElementById('<%=GridViewUHDept.ClientID %>').style.display = "none";

            if (SelectedTextJabsomOther.indexOf("UH Community College") > -1)
                document.getElementById('<%=GridViewCommunityCollege.ClientID %>').style.display = "inherit";
            else
                document.getElementById('<%=GridViewCommunityCollege.ClientID %>').style.display = "none";

            if (SelectedTextJabsomOther.indexOf("Community Partner") > -1)
                document.getElementById('<%=GridViewCommunityPartner.ClientID %>').style.display = "inherit";
            else
                document.getElementById('<%=GridViewCommunityPartner.ClientID %>').style.display = "none";
        }

        function GridViewNonUHToggle(e) {
            if (e.id === 'MainContent_GridViewNonUH_chkRow_8') {
                if (e.checked)
                    $('#MainContent_GridViewNonUH_txtNonUHOther').show();
                else {
                    $('#MainContent_GridViewNonUH_txtNonUHOther').val('');
                    $('#MainContent_GridViewNonUH_txtNonUHOther').hide();
                }
            }
        }

    </script>
</asp:Content>


