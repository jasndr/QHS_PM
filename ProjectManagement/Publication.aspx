<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Publication.aspx.cs" Inherits="ProjectManagement.Publication1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="Scripts/bootstrap.min.js"></script>
    <script src="Scripts/bootstrap-datepicker.min.js"></script>
    <script src="Scripts/jquery.validate.min.js"></script>
    <%--  <script src="Scripts/bootstrap-select.min.js"></script>--%>
    <%--<script src="Scripts/jquery.steps.js"></script>--%>
    <script type="text/javascript">         

            $(document).ready(function () {
                var DropdownListPubType = document.getElementById('<%=ddlPubType.ClientID %>');
                var SelectedIndex = DropdownListPubType.selectedIndex;

                if (SelectedIndex == 0) {
                    $('#divAbstract').hide();
                    $('#divAbstract').show();
                }
                else {
                    $('#divAbstract').hide();
                }


            });

            function divAbstractToggle(ddlPubType) {
                var DropdownListPubType = document.getElementById('<%=ddlPubType.ClientID %>');
                var SelectedIndex = DropdownListPubType.selectedIndex;

                if (SelectedIndex == 0) {
                    $('#divAbstract').hide();
                    $('#divAbstract').show();
                }
                else {
                    $('#divAbstract').hide();
                }

            }


            $(function () {
                $('#datetimepicker1').datepicker();
                $('#datetimepicker2').datepicker();
                $('#datetimepicker3').datepicker();
            });


            function SelectSingleRadiobutton(rdbtnid) {
                var rdBtn = document.getElementById(rdbtnid);
                var rdBtnList = document.getElementsByTagName("input");
                for (i = 0; i < rdBtnList.length; i++) {
                    if (rdBtnList[i].type == "radio" && rdBtnList[i].id != rdBtn.id && rdBtnList[i].name != "optionsRadios") {
                        rdBtnList[i].checked = false;
                    }
                }
            }
       

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="rootwizard" class="jumbotron">
        <asp:Label runat="server" ID="lblDebugMsg"></asp:Label>
        <div class="row">
            <div class="col-xs-6 col-md-6">
                <div class="panel panel-default">
                    <div class="panel-heading">Publication Type</div>
                    <div class="panel-body">
                        <asp:DropDownList ID="ddlPubType" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlPubType_Changed" AutoPostBack="True">
                        </asp:DropDownList>
                        <%--<div class="radio">
                     <label>
                     <input type="radio" name="pubRadios" id="optionAbstract" value="Abstract" runat="server"  checked onclick="divAbstractToggle()" />
                     Abstract
                     </label>
                     </div>
                     <div class="radio">
                     <label>
                     <input type="radio" name="pubRadios" id="optionManuscript" value="Manuscript" runat="server"  onclick="divAbstractHide()" />
                     Manuscript
                     </label>
                     </div>     --%>
                    </div>
                </div>
            </div>
        </div>
      
        <%--<div class="row" id="divPI">
         <div class="col-xs-6 col-md-4">
            Principal Investigator:       
            <asp:DropDownList ID="ddlInvestor" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlInvestor_Changed" AutoPostBack="True">
            </asp:DropDownList>
         </div>
         </div>
         <br />--%>
        <div class="row">
            <div class="col-xs-6 col-md-6">
                <div class="panel panel-default">
                    <div class="panel-heading">Project</div>
                    <div class="panel-body">
                        <asp:DropDownList ID="ddlProject" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlProject_Changed" AutoPostBack="True" required>
                        </asp:DropDownList>
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Required!" ControlToValidate="ddlProject"></asp:RequiredFieldValidator>--%>
                    </div>
                </div>
            </div>
        </div>
       
        <div class="row">
            <div class="col-xs-6 col-md-6">
                <div class="panel panel-default">
                    <div class="panel-heading">Publications</div>
                    <div class="panel-body">
                        <%--Choose a submitted publication:--%>
                  <asp:DropDownList ID="ddlPublication" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlPublication_Changed" AutoPostBack="True">
                  </asp:DropDownList>
                       <%-- <br />
                        Choose a sub publication:
                        <asp:DropDownList ID="ddlChildPublication" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlChildPublication_Changed" AutoPostBack="True">
                        </asp:DropDownList>
                        <br />--%>
                        <%--<asp:CheckBox ID="CheckBoxConvert" runat="server" Text="Convert to Manuscript"/>--%>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-6 col-md-6">
                <div class="panel panel-default">
                    <div class="panel-heading">BQHS Faculty and Staff</div>
                    <div class="panel-body">
                        <asp:GridView ID="GridViewBioStat" runat="server" AutoGenerateColumns="False" class="table table-striped table-hover" GridLines="None">
                            <Columns>
                                <asp:TemplateField HeaderText="Select">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkRow" runat="server"></asp:CheckBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Id" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblId" runat="server" Text='<%# Eval("Key") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblName" runat="server" Text='<%# Eval("Value") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <SelectedRowStyle BackColor="LightCyan"
                                ForeColor="DarkBlue"
                                Font-Bold="true" />  
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-6 col-md-6">
                <div class="panel panel-default">
                    <div class="panel-heading">Status</div>
                    <div class="panel-body">
                        <div class="form-group">
                            <%--<label class="col-xs-3 control-label">Date Range</label>--%>
                            <div class="col-xs-9">
                                <div class="radio">
                                    <label>
                                        <input id="optionSubmitted" runat="server" name="statusRadios" value="Submitted" type="radio" checked>
                                        Submitted
                                    </label>
                                </div>
                                <div class="radio">
                                    <label>
                                        <input id="optionAccepted" runat="server" name="statusRadios" value="Accepted" type="radio" disabled>
                                        Accepted</label>
                                </div>
                                <div class="radio">
                                    <label>
                                        <input type="radio" runat="server" name="statusRadios" id="optionPublished" value="Published" disabled>
                                        Published
                                    </label>
                                </div>
                                <div class="radio">
                                    <label>
                                        <input type="radio" runat="server" name="statusRadios" id="optionNotAccepted" value="NotAccepted" disabled>
                                        Not Accepted
                                    </label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <br />
        Date of current status:
      <div class="row">
          <div class="col-xs-6 col-md-6">
                <div class="panel panel-default">
                    <div class="panel-heading">Submit Date</div>
                    <div class="panel-body">
                        <div class='input-group date' id='datetimepicker1'>
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-calendar"></span>
                            </span>
                            <asp:TextBox ID="TextBoxSubmitDate" runat="server" class="form-control" Width="50%" required></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
      </div>
        <br />
        <div class="row">
            <div class="col-xs-6 col-md-6">
                <div class="panel panel-default">
                    <div class="panel-heading">Grants</div>
                    <div class="panel-body">
                        <asp:GridView ID="GridViewGrant" runat="server" AutoGenerateColumns="False" class="table table-bordered table-hover">
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
                                        <asp:Label ID="lblName" runat="server" Text='<%# Eval("GrantAffilName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <%--<div class="panel panel-default">
               <div class="panel-heading">Grants</div>
               <div class="panel-body">
                  <div class="checkbox">
                     <label>
                     <input type="checkbox" id="inlineCheckbox1" value="option1" runat="server">
                     RMATRX
                     </label>
                  </div>
                  <div class="checkbox">
                     <label>
                     <input type="checkbox" id="inlineCheckbox2" value="option2" runat="server">
                     G12 BRIDGES
                     </label>
                  </div>
                  <div class="checkbox">
                     <label>
                     <input type="checkbox" id="inlineCheckbox3" value="option3" runat="server">
                     INBRE
                     </label>
                  </div>
               </div>
               </div>--%>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-6 col-md-6">
                <h5>Authors:</h5>
                <textarea id="TextAreaAuthors" runat="server" class="form-control" rows="3" required></textarea>
            </div>
        </div>
        <br />
        <div class="row">
            <div class="col-xs-6 col-md-6">
                Title:
            <asp:TextBox ID="TextBoxTitle" runat="server" class="form-control" required></asp:TextBox>
            </div>
            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Required!" ControlToValidate="TextBoxTitle"></asp:RequiredFieldValidator>--%>
        </div>
        <br />
        <div id="divAbstract">
            <div class="row">
                <div class="col-xs-6 col-md-6">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            Conference
                        </div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-xs-6 col-md-12">
                                    Choose a Conference:
                           <asp:DropDownList ID="ddlConference" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlConference_Changed" AutoPostBack="True">
                           </asp:DropDownList>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-xs-6 col-md-12">
                                    Name of Conference/Meeting:
                           <asp:TextBox ID="TextBoxName" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-xs-6 col-md-12">
                                    Location of Conference/Meeting:
                           <asp:TextBox ID="TextBoxLocation" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <br />
                            Start Date of Conference:
                     <div class="form-group">
                         <div class='input-group date' id='datetimepicker2'>
                             <span class="input-group-addon">
                                 <span class="glyphicon glyphicon-calendar"></span>
                             </span>
                             <asp:TextBox ID="TextBoxStartDate" runat="server" class="form-control" Width="50%"></asp:TextBox>
                         </div>
                     </div>
                            End Date of Conference:
                     <div class="form-group">
                         <div class='input-group date' id='datetimepicker3'>
                             <span class="input-group-addon">
                                 <span class="glyphicon glyphicon-calendar"></span>
                             </span>
                             <asp:TextBox ID="TextBoxEndDate" runat="server" class="form-control" Width="50%"></asp:TextBox>
                         </div>
                     </div>
                            <div class="row">
                                <div class="col-xs-6 col-md-6">
                                    <div class="radio">
                                        <label>
                                            <input type="radio" name="optionsRadios" id="optionsRadios1" value="option1" checked>
                                            Poster
                                        </label>
                                        <label>
                                            <input type="radio" name="optionsRadios" id="optionsRadios2" value="option2">
                                            Presentation
                                        </label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="divManuscript">
            <div class="row">
                <div class="col-xs-6 col-md-6">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            Journal
                        </div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-xs-6 col-md-12">
                                    Name of Journal:
                           <asp:TextBox ID="TextBoxJournal" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-xs-6 col-md-6">
                                    Volume #:
                           <asp:TextBox ID="TextBoxVolumeNo" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-xs-6 col-md-6">
                                    Issue #:
                           <asp:TextBox ID="TextBoxIssueNo" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-xs-6 col-md-6">
                                    Pages:
                           <asp:TextBox ID="TextBoxPages" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-xs-6 col-md-6">
                                    DOI: (Manuscript only)
                           <asp:TextBox ID="TextBoxDOI" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-xs-6 col-md-6">
                                    Pmid: (Manuscript only)
                           <asp:TextBox ID="TextBoxPmid" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-xs-6 col-md-6">
                                    Pmcid: (Manuscript only)
                           <asp:TextBox ID="TextBoxPmcid" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="form-group">
            <div class="row">
                <div class="col-xs-6 col-md-6">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            File Upload
                        </div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-xs-6 col-md-6">
                                    <input type="file" id="inputFile" runat="server" class="form-control">
                                </div>
                                <div class="col-xs-6 col-md-6">
                                    <%--<p class="help-block">block-level help text here.</p>--%>
                                    <asp:Button ID="ButtonUpload" runat="server" OnClick="ButtonUpload_Click" Text="Upload" class="btn" />
                                </div>
                            </div>
                            <br />
                            <%--<div class="row">
                                <div class="col-xs-6 col-md-6">
                                    <asp:Label runat="server" ID="lblUploadedFile"></asp:Label>
                                </div>
                                <div class="col-xs-6 col-md-6">
                                    <asp:LinkButton ID="linkButtonDownload" runat="server" OnClick="linkButtonDownload_Click">Download</asp:LinkButton>
                                </div>
                            </div>--%>
                            <div class="row">
                                <div class="col-xs-6 col-md-12">
                                    <asp:GridView ID="GridViewFile" runat="server" AutoGenerateColumns="false" EmptyDataText="No files uploaded" class="table table-bordered table-hover">
                                        <Columns>
                                            <asp:BoundField DataField="Text" HeaderText="File Name" />
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDownload" Text="Download" CommandArgument='<%# Eval("Value") %>' runat="server" OnClick="DownloadFile"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDelete" Text="Delete" CommandArgument='<%# Eval("Value") %>' runat="server" OnClick="DeleteFile" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-xs-6 col-md-10">
                </div>
                <div class="col-xs-6 col-md-2">
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" class="btn btn-primary" />
                </div>
            </div>
        </div>
        
        <%--  <p>
           <asp:ValidationSummary ID="ValidationSummary" runat="server"
               HeaderText="You received the following errors:"></asp:ValidationSummary>
       </p>--%>
   
    <asp:HiddenField ID="HiddenFieldCurrentDate" Value="" runat="server" />
    <asp:HiddenField ID="HiddenFieldPubType" Value="Abstract" runat="server" />
</asp:Content>
