<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TimeEntry.aspx.cs" Inherits="ProjectManagement.TimeEntry1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   <script src="Scripts/bootstrap.min.js"></script>    
   <script src="Scripts/bootstrap-datepicker.min.js"></script> 
   <script src="Scripts/jquery.bootstrap.wizard.min.js"></script>
   <script src="Scripts/jquery.validate.min.js"></script> 
   <script type="text/javascript">
       $(document).ready(function () {
           $('#btnSubmit').click(function () {
               $("#commentForm").validate().cancelSubmit = true;
               $("#commentForm").submit();
               return false;
           })
       });
       
       

       $(function () {
           $('#datetimepicker1').datepicker();
           $('#datetimepicker2').datepicker();
          
           $('#idTourDateDetails').datepicker({
               dateFormat: 'dd-mm-yy',
               minDate: '+5d',
               changeMonth: false,
               changeYear: false,
               altField: "#idTourDateDetailsHidden",
               altFormat: "yy-mm-dd"
           });
       });

       function ValidateEntry(sender, args) {
           if (document.getElementById("<%=TextBoxSubmitDate.ClientID %>").innerText != "") {
                args.IsValid = true;
            } else {
                args.IsValid = false;
            }
       }

       function validateControl() {
           $("#commentForm").validate({
               rules: {
                   <%=ddlPI.UniqueID %>: {
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
                       required:true,
                       number:true
                   }
               }
           });
       }
       
   </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <div id="rootwizard" class="jumbotron">
        <div class="panel panel-default">
            <div class="panel-heading">Add Time</div>
            <div class="panel-body">
                <div class="row">
                    <div class="col-xs-6 col-md-3">
                        Biostatisticians
                <asp:DropDownList ID="ddlBioStat" runat="server" CssClass="form-control" Caption="test" OnSelectedIndexChanged="ddlBioStat_Changed" AutoPostBack="True" >
                </asp:DropDownList>
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-xs-6 col-md-3">
                        PI
                <asp:DropDownList ID="ddlPI" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlPI_Changed" AutoPostBack="True" >
                </asp:DropDownList>
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-xs-6 col-md-6">
                        Projects
                <asp:DropDownList ID="ddlProject" runat="server" CssClass="form-control" >
                </asp:DropDownList>
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-xs-6 col-md-6">
                        Service Type
                <asp:DropDownList ID="ddlServiceType" runat="server" CssClass="form-control" >
                </asp:DropDownList>
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-xs-6 col-md-3">
                        Date
                        <div class='input-group date' id='datetimepicker1'>
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-calendar"></span>
                            </span>
                            <asp:TextBox ID="TextBoxSubmitDate" runat="server" class="form-control" ></asp:TextBox>
                        </div>
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-xs-6 col-md-3">
                        Time                
                <asp:TextBox ID="TextBoxTime" runat="server" class="form-control" ></asp:TextBox>
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-xs-6 col-md-10"></div>
                    <div class="col-xs-6 col-md-2">
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="btn btn-info" OnClientClick="return validateControl();" OnClick="btnSubmit_Click"/>
                    </div>
                </div>
                <asp:HiddenField ID="HiddenFieldCurrentDate" Value="" runat="server" />

            </div>
        </div>

        <div class="panel panel-default">
            <div class="panel-heading">Time Entry</div>
            <div class="panel-body">
                <div class="row">
                    <div class="col-xs-6 col-md-3">
                        <div class='input-group date'>
                            <span class="input-group-addon">
                                <span class="glyphicon">Month</span>
                            </span>
                            <asp:DropDownList ID="ddlMonth" runat="server" CssClass="form-control" Caption="test" OnSelectedIndexChanged="ddlMonth_Changed" AutoPostBack="True">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <br />
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div>
                            <asp:GridView ID="GridViewTimeEntry" runat="server" AutoGenerateColumns="False" class="table table-bordered table-hover"
                                OnRowDeleting="GridViewTimeEntry_RowDeleting"
                                OnRowCommand="GridViewTimeEntry_RowCommand">
                                <Columns>
                                    <asp:ButtonField CommandName="editRecord"
                                        ControlStyle-CssClass="btn btn-info" ButtonType="Button"
                                        Text="Edit" HeaderText="Edit" />
                                    <asp:TemplateField HeaderText="Id" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblId" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Id">
                                        <ItemTemplate>
                                            <asp:Label ID="lblProjectId" runat="server" Text='<%# Eval("ProjectId") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Project">
                                        <ItemTemplate>
                                            <asp:Label ID="lblProjectName" runat="server" Text='<%# Eval("Title") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Service Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lblServiceType" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText=" Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDate" runat="server" Text='<%# Convert.ToDateTime(Eval("Date")).ToShortDateString() %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText=" Time">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTime" runat="server" Text='<%# Eval("Duration") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <%--<asp:LinkButton ID="lnkEdit" runat="server" Text="" CommandName="Edit" ToolTip="Edit"
                                CommandArgument=''><img src="images/icon-edit.gif" /></asp:LinkButton>--%>
                                            <asp:LinkButton ID="lnkDelete" runat="server" Text="Delete" CommandName="Delete"
                                                ToolTip="Delete" OnClientClick='return confirm("Are you sure you want to delete this entry?");'
                                                CommandArgument=''></asp:LinkButton>
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
                                    <div class="col-xs-6 col-md-4">
                                        <asp:Label ID="lblEditId" runat="server" Visible="false"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-xs-6 col-md-4">Project Id: 
                                        <asp:Label ID="lblEditProjectId" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-xs-6 col-md-12">Project Title: 
                                        <asp:Label ID="lblEditProject" runat="server" Text=""></asp:Label>
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-xs-6 col-md-4">Service Type                                        
                                        <asp:DropDownList ID="ddlEditServiceType" runat="server" CssClass="form-control"></asp:DropDownList>
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-xs-6 col-md-4">
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
                                <div class="row">
                                    <div class="col-xs-6 col-md-4">
                                        Time                
                                <asp:TextBox ID="TextBoxEditTime" runat="server" class="form-control"></asp:TextBox>
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
    </div>
</asp:Content>
