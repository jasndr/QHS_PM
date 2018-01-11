<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MonthlyReport.aspx.cs" Inherits="ProjectManagement.MonthlyReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="rootwizard" class="jumbotron">
        <div class="panel panel-default">
            <div class="panel-heading">
                Select a Month
            <div class="panel-body">
                <div class="row">
                    <div class="col-xs-6 col-md-3">
                        Month
                <asp:DropDownList ID="ddlMonth" runat="server" CssClass="form-control" Caption="test" OnSelectedIndexChanged="ddlMonth_Changed" AutoPostBack="True" required>
                </asp:DropDownList>
                    </div>
                </div>
            </div>
            </div>
        </div>

        <div class="panel panel-default">
            <div class="panel-heading">Time Entry</div>
            <div class="panel-body">
                <asp:GridView ID="GridViewTimeEntry" runat="server" AutoGenerateColumns="False" class="table table-bordered table-hover">
                    <Columns>
                        <asp:TemplateField HeaderText="Id" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblId" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText=" Project">
                            <ItemTemplate>
                                <asp:Label ID="lblProjectName" runat="server" Text='<%# Eval("Title") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText=" Category">
                            <ItemTemplate>
                                <asp:Label ID="lblCategoryName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText=" Date">
                            <ItemTemplate>
                                <asp:Label ID="lblDate" runat="server" Text='<%# Eval("Date") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText=" Time">
                            <ItemTemplate>
                                <asp:Label ID="lblTime" runat="server" Text='<%# Eval("Duration") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>


    </div>
</asp:Content>
