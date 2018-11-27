<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StaffForm.aspx.cs" Inherits="ProjectManagement.Admin.StaffForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function pageLoad(sender, args) {
            $('#li_super').addClass('selected');
            $('#li_staffform').addClass('selected');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <hr />
    <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
    <div class="panel panel-default">
        <div class="panel-heading">QHS Faculty and Staff</div>
        <div class="panel-body">
            <div>
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"
                    OnRowCommand="GridView1_RowCommand"
                    OnRowEditing="GridView1_RowEditing"
                    OnRowCancelingEdit="GridView1_RowCancelingEdit"
                    OnRowDataBound="GridView1_RowDataBound"
                    OnRowUpdating="GridView1_RowUpdating" ShowFooter="True"
                    class="table table-striped table-bordered table-hover btn-group-justified">
                    <Columns>
                        <asp:TemplateField HeaderText="Id" HeaderStyle-Width="5%">
                            <ItemTemplate>
                                <asp:Label ID="lblId" runat="server" Text='<%# Bind("Id") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Name" HeaderStyle-Width="17%">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtName" runat="server" Text='<%# Bind("Name") %>' CssClass="" MaxLength="50"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="valName" runat="server" ControlToValidate="txtName"
                                    Display="Dynamic" ErrorMessage="Name is required." ForeColor="Red" SetFocusOnError="True"
                                    ValidationGroup="editGrp">*</asp:RequiredFieldValidator>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblName" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtNameNew" runat="server" CssClass="" MaxLength="50"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="valNameNew" runat="server" ControlToValidate="txtNameNew"
                                    Display="Dynamic" ErrorMessage="Name is required." ForeColor="Red" SetFocusOnError="True"
                                    ValidationGroup="newGrp">*</asp:RequiredFieldValidator>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Type" HeaderStyle-Width="10%">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtType" runat="server" Text='<%# Bind("Type") %>' CssClass="" MaxLength="10"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="valType" runat="server" ControlToValidate="txtType"
                                    Display="Dynamic" ErrorMessage="Type is required." ForeColor="Red" SetFocusOnError="True"
                                    ValidationGroup="editGrp">*</asp:RequiredFieldValidator>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblType" runat="server" Text='<%# Bind("Type") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtTypeNew" runat="server" CssClass="" MaxLength="10"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="valTypeNew" runat="server" ControlToValidate="txtTypeNew"
                                    Display="Dynamic" ErrorMessage="Type is required." ForeColor="Red" SetFocusOnError="True"
                                    ValidationGroup="newGrp">*</asp:RequiredFieldValidator>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Email" HeaderStyle-Width="15%">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEmail" runat="server" Text='<%# Bind("Email") %>' CssClass="" MaxLength="50"></asp:TextBox>
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
                                <asp:TextBox ID="txtEmailNew" runat="server" CssClass="" MaxLength="50"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="valEmailNew" runat="server" ControlToValidate="txtEmailNew"
                                    Display="Dynamic" ErrorMessage="Email is required." ForeColor="Red" SetFocusOnError="True"
                                    ValidationGroup="newGrp">*</asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="valRegEmailNew" runat="server" ErrorMessage="Invalid Email" ValidationGroup="newGrp"
                                    SetFocusOnError="true" Display="Dynamic" ControlToValidate="txtEmailNew" ForeColor="Red"
                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">*</asp:RegularExpressionValidator>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="LogonId" HeaderStyle-Width="10%">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtLogonId" runat="server" Text='<%# Bind("LogonId") %>' CssClass="" MaxLength="10"></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblLogonId" runat="server" Text='<%# Bind("LogonId") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtLogonIdNew" runat="server" CssClass="" MaxLength="10"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="EndDate" HeaderStyle-Width="10%">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEndDate" runat="server" Text='<%# Convert.ToDateTime(Eval("EndDate")).ToShortDateString() %>' CssClass="" MaxLength="10"></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblEndDate" runat="server" Text='<%# String.Format("{0:MM/dd/yyyy}", Eval("EndDate"))%>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtEndDateNew" runat="server" CssClass="" MaxLength="10"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Edit" HeaderStyle-Width="5%">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkEdit" runat="server" Text="" CommandName="Edit" ToolTip="Edit"
                                    CommandArgument=''><img src="../images/icon-edit.gif" /></asp:LinkButton>
                                <%--<asp:LinkButton ID="lnkDelete" runat="server" Text="Delete" CommandName="Delete"
                                    ToolTip="Delete" OnClientClick='return confirm("Are you sure you want to delete this entry?");'
                                    CommandArgument=''><img src="../Images/icon_delete.png" /></asp:LinkButton>--%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="lnkInsert" runat="server" Text="" ValidationGroup="editGrp" CommandName="Update" ToolTip="Save"
                                    CommandArgument=''><img src="../images/icon-save.gif" /></asp:LinkButton>
                                <asp:LinkButton ID="lnkCancel" runat="server" Text="" CommandName="Cancel" ToolTip="Cancel"
                                    CommandArgument=''><img src="../images/icon-cancel.gif" /></asp:LinkButton>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:LinkButton ID="lnkInsert" runat="server" Text="" ValidationGroup="newGrp" CommandName="InsertNew" ToolTip="Add New Entry"
                                    CommandArgument=''><img src="../images/icon-save.gif" /></asp:LinkButton>
                                <asp:LinkButton ID="lnkCancel" runat="server" Text="" CommandName="CancelNew" ToolTip="Cancel"
                                    CommandArgument=''><img src="../images/icon-cancel.gif" /></asp:LinkButton>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="UserRights" HeaderStyle-Width="25%">
                            <EditItemTemplate><%--Edit Checkbox user rights for user--%>
                                <asp:Repeater ID="rptUserRights" runat="server" EnableViewState="true">
                                    <ItemTemplate>
                                        <div style="display: inline-block;">
                                            <asp:CheckBox ID="chkId" runat="server" Text='<%# Eval("Name") %>'></asp:CheckBox>
                                            <asp:HiddenField ID="Id" Value='<%#Eval("Id")%>' runat="server" />
                                            <%--<%# Eval("Name") %>--%>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Repeater ID="rptUserRights" runat="server" EnableViewState="true">
                                    <ItemTemplate>
                                        <div style="display: inline-block;">
                                            <asp:CheckBox ID="chkId" runat="server" Text='<%# Eval("Name") %>' Enabled="false" ></asp:CheckBox>
                                            <asp:HiddenField ID="Id" Value='<%#Eval("Id")%>' runat="server" />
                                            <%--<%# Eval("Name") %>--%>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ItemTemplate>
                            <FooterTemplate><%--Nothing to Add--%></FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
