<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Investor.aspx.cs" Inherits="ProjectManagement.Investor"  MaintainScrollPositionOnPostback="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    PI Name : <asp:TextBox ID="txtSrchInvestorName" runat="server"></asp:TextBox>
    &emsp;   
    <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
    <br />   
    <hr />    
   
    <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
    <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" 
        GridLines="None" AutoGenerateColumns="False"
        OnRowCommand="GridView1_RowCommand"
        OnRowEditing="GridView1_RowEditing"
        OnRowCancelingEdit="GridView1_RowCancelingEdit"
        onrowdatabound="GridView1_RowDataBound"
        OnRowUpdating="GridView1_RowUpdating" ShowFooter="True"
        class="table table-bordered table-hover"
        >
        <Columns>
            <asp:TemplateField HeaderText="">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkEdit" runat="server" Text="" CommandName="Edit" ToolTip="Edit"
                        CommandArgument=''><img src="images/icon-edit.gif" /></asp:LinkButton>
                    <%--<asp:LinkButton ID="lnkDelete" runat="server" Text="Delete" CommandName="Delete"
                        ToolTip="Delete" OnClientClick='return confirm("Are you sure you want to delete this entry?");'
                        CommandArgument=''><img src="../Images/icon_delete.png" /></asp:LinkButton>--%>
                </ItemTemplate>
                <EditItemTemplate>
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
                </FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Id">
                <ItemTemplate>
                    <asp:Label ID="lblId" runat="server" Text='<%# Bind("Id") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>             
            <asp:TemplateField HeaderText="First Name">
                <EditItemTemplate>
                    <asp:TextBox ID="txtFirstName" runat="server" Text='<%# Bind("FirstName") %>' CssClass="" MaxLength="30"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="valFirstName" runat="server" ControlToValidate="txtFirstName"
                        Display="Dynamic" ErrorMessage="First Name is required." ForeColor="Red" SetFocusOnError="True"
                        ValidationGroup="editGrp">*</asp:RequiredFieldValidator>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblFirstName" runat="server" Text='<%# Bind("FirstName") %>'></asp:Label>
                </ItemTemplate>
                <FooterTemplate>
                    <asp:TextBox ID="txtFirstNameNew" runat="server" CssClass="" MaxLength="30"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="valFirstNameNew" runat="server" ControlToValidate="txtFirstNameNew"
                        Display="Dynamic" ErrorMessage="First Name is required." ForeColor="Red" SetFocusOnError="True"
                        ValidationGroup="newGrp">*</asp:RequiredFieldValidator>
                </FooterTemplate>
            </asp:TemplateField>       
            <asp:TemplateField HeaderText="Last Name">
                <EditItemTemplate>
                    <asp:TextBox ID="txtLastName" runat="server" Text='<%# Bind("LastName") %>' CssClass="" MaxLength="30"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="valLastName" runat="server" ControlToValidate="txtLastName"
                        Display="Dynamic" ErrorMessage="Last Name is required." ForeColor="Red" SetFocusOnError="True"
                        ValidationGroup="editGrp">*</asp:RequiredFieldValidator>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblLastName" runat="server" Text='<%# Bind("LastName") %>'></asp:Label>
                </ItemTemplate>
                <FooterTemplate>
                    <asp:TextBox ID="txtLastNameNew" runat="server" CssClass="" MaxLength="30"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="valLastNameNew" runat="server" ControlToValidate="txtLastNameNew"
                        Display="Dynamic" ErrorMessage="Last Name is required." ForeColor="Red" SetFocusOnError="True"
                        ValidationGroup="newGrp">*</asp:RequiredFieldValidator>
                </FooterTemplate>
            </asp:TemplateField>   
            <asp:TemplateField HeaderText="Email">
                <EditItemTemplate>
                    <asp:TextBox ID="txtEmail" runat="server" Text='<%# Bind("Email") %>' CssClass="" MaxLength="30"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="valEmail" runat="server" ControlToValidate="txtEmail"
                        Display="Dynamic" ErrorMessage="Email is required." ForeColor="Red" SetFocusOnError="True"
                        ValidationGroup="editGrp">*</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="valRegEmail" runat="server" ErrorMessage="Invalid Email" ValidationGroup="editGrp"
                        SetFocusOnError="true" Display="Dynamic" ControlToValidate="txtEmail" ForeColor="Red"
                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">*</asp:RegularExpressionValidator>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblEmail" runat="server" Text='<%# Bind("Email") %>'></asp:Label>
                </ItemTemplate>
                <FooterTemplate>
                    <asp:TextBox ID="txtEmailNew" runat="server" CssClass="" MaxLength="30"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="valEmailNew" runat="server" ControlToValidate="txtEmailNew"
                        Display="Dynamic" ErrorMessage="Email is required." ForeColor="Red" SetFocusOnError="True"
                        ValidationGroup="newGrp">*</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="valRegEmailNew" runat="server" ErrorMessage="Invalid Email" ValidationGroup="newGrp"
                        SetFocusOnError="true" Display="Dynamic" ControlToValidate="txtEmailNew" ForeColor="Red"
                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">*</asp:RegularExpressionValidator>
                </FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Phone">
                <EditItemTemplate>
                    <asp:TextBox ID="txtPhone" runat="server" Text='<%# Bind("Phone") %>' CssClass="" MaxLength="30"></asp:TextBox>                    
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblPhone" runat="server" Text='<%# Bind("Phone") %>'></asp:Label>
                </ItemTemplate>
                <FooterTemplate>
                    <asp:TextBox ID="txtPhoneNew" runat="server" CssClass="" MaxLength="30"></asp:TextBox>                    
                </FooterTemplate>
            </asp:TemplateField> 
            <asp:TemplateField HeaderText="Investor Status">
                <EditItemTemplate>
                    <asp:DropDownList ID="ddlStatus" runat="server">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="valStatus" runat="server" ControlToValidate="ddlStatus"
                        Display="Dynamic" ErrorMessage="Status is required." ForeColor="Red" SetFocusOnError="True"
                        ValidationGroup="editGrp">*</asp:RequiredFieldValidator>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("InvestStatu.StatusValue") %>'></asp:Label>
                </ItemTemplate>
                <FooterTemplate>
                    <asp:DropDownList ID="ddlStatusNew" runat="server">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="valStatusNew" runat="server" ControlToValidate="ddlStatusNew"
                        Display="Dynamic" ErrorMessage="Status is required." ForeColor="Red" SetFocusOnError="True"
                        ValidationGroup="newGrp">*</asp:RequiredFieldValidator>
                </FooterTemplate>
            </asp:TemplateField> 
        </Columns>
    </asp:GridView>    
</asp:Content>
