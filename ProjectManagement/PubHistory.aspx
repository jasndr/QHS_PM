<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PubHistory.aspx.cs" Inherits="ProjectManagement.PubHistory1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="Scripts/bootstrap.min.js"></script>    
    <script src="Scripts/bootstrap-datepicker.min.js"></script> 
    <script src="Scripts/jquery.bootstrap.wizard.min.js"></script>
    <script src="Scripts/jquery.validate.min.js"></script> 
    <script type="text/javascript">
        $(document).ready(function () {

        });

        $(function () {
            $('#datetimepicker1').datepicker();
            $('#datetimepicker2').datepicker();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="rootwizard" class="jumbotron">
        <div class="panel panel-default">
            <div class="panel-heading"> </div>
            <div class="panel-body">
                <div class="row">
                    <div class="col-xs-6 col-md-6">
                        BQHS Faculty and Staff
                        <asp:DropDownList ID="ddlBiostat" runat="server" CssClass="form-control" Caption="test" OnSelectedIndexChanged="ddlBiostat_Changed" AutoPostBack="True" >
                        </asp:DropDownList>
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-xs-6 col-md-6">
                        Publications
                        <asp:DropDownList ID="ddlPublication" runat="server" CssClass="form-control" Caption="test" OnSelectedIndexChanged="ddlPublication_Changed" AutoPostBack="True" required>
                        </asp:DropDownList>
                    </div>
                </div>
                <br />
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
                                        <input id="optionSubmitted" runat="server" name="statusRadios" value="Submitted" type="radio" disabled>
                                        Submitted
                                    </label>
                                </div>
                                <div class="radio">
                                    <label>
                                        <input id="optionAccepted" runat="server" name="statusRadios" value="Accepted" type="radio" checked>
                                        Accepted</label>
                                </div>
                                <div class="radio">
                                    <label>
                                        <input type="radio" runat="server" name="statusRadios" id="optionPublished" value="Published" >
                                        Published
                                    </label>
                                </div>
                                <div class="radio">
                                    <label>
                                        <input type="radio" runat="server" name="statusRadios" id="optionNotAccepted" value="NotAccepted" >
                                        Not Accepted
                                    </label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-6 col-md-3">
                Start Date
                <div class='input-group date' id='datetimepicker1'>
                    <span class="input-group-addon">
                        <span class="glyphicon glyphicon-calendar"></span>
                    </span>
                    <asp:TextBox ID="TextBoxStartDate" runat="server" class="form-control" required></asp:TextBox>                       
                </div>
            </div>
        </div>
        <br />
        <div class="row">
            <div class="col-xs-6 col-md-3">
                End Date
                <div class='input-group date' id='datetimepicker2'>
                    <span class="input-group-addon">
                        <span class="glyphicon glyphicon-calendar"></span>
                    </span>
                    <asp:TextBox ID="TextBoxEndDate" runat="server" class="form-control" required></asp:TextBox>                       
                </div>
            </div>
        </div>
        <br />
                <div class="row">
            <div class="col-xs-6 col-md-10"></div>
            <div class="col-xs-6 col-md-2">
                <asp:Button ID="btnSubmit" runat="server" Text="Submit"  class="btn btn-primary" OnClick="btnSubmit_Click" />
            </div>
        </div>
            </div>
        </div> 
        
        <div class="panel panel-default">
        <div class="panel-heading">History</div>
        <div class="panel-body">
            
            <asp:GridView ID="GridViewHistory" runat="server" AutoGenerateColumns="False" class="table table-bordered table-hover"
                OnRowDeleting="GridViewHistory_RowDeleting"                
                EmptyDataText="No history record">
                <Columns>
                    <asp:TemplateField HeaderText="Id" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblId" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Start Date">
                        <ItemTemplate>
                            <asp:Label ID="lblStartDate" runat="server" Text='<%# Eval("StartDate") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="End Date">
                        <ItemTemplate>
                            <asp:Label ID="lblEndDate" runat="server" Text='<%# Eval("EndDate") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Creator">
                        <ItemTemplate>
                            <asp:Label ID="lblCreator" runat="server" Text='<%# Eval("Creator") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <%--<asp:LinkButton ID="lnkEdit" runat="server" Text="" CommandName="Edit" ToolTip="Edit"
                                CommandArgument=''><img src="images/icon-edit.gif" /></asp:LinkButton>--%>
                            <asp:LinkButton ID="lnkDelete" runat="server" Text="Delete" CommandName="Delete"
                        ToolTip="Delete" OnClientClick='return confirm("Are you sure you want to delete this record?");'
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
            </asp:GridView>
        </div>
        </div>
          
    </div>
     <asp:HiddenField ID="HiddenFieldCurrentDate" Value="" runat="server" />
</asp:Content>
