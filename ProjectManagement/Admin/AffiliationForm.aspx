<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AffiliationForm.aspx.cs" Inherits="ProjectManagement.Admin.AffiliationForm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery.validate.min.js"></script>   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel panel-default">
            <div class="panel-heading"><b>Affiliation</b></div>
            <div class="panel-body">
                <div class="row">
                    <div class="col-md-1"></div>
                    <div class="col-xs-6 col-md-2">
                        <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control" onchange="ddlCategoryChange()">
                        </asp:DropDownList>
                    </div>
                </div>
                <br />  
                <div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-1"></div>
                                <div class="col-md-6">
                                    <table class="table table-striped table-bordered dataTable no-footer" id="tblAffiliation">
                                        <thead>
                                            <tr>
                                                <th class="hidden">Id</th>
                                                <th class="hidden">CategoryName</th>
                                                <th>Category</th>
                                                <th>Name</th>
                                                <th>View/Edit</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater ID="rptAffiliation" runat="server"  OnItemCommand="rptAffiliation_ItemCommand">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td class="hidden"><%# Eval("Id") %></td>                                                       
                                                        <td class="hidden">
                                                            <input class="form-control" type="text" name="txtCategory" id="txtCategory" value='<%#Eval("Type")%>' readonly />
                                                        </td>
                                                         <td><%# Eval("Type") %></td>
                                                        <td><%# Eval("Name") %></td>
                                                        <td>
                                                            <asp:Button ID="btnView" runat="server" Text="Edit" CssClass="btn btn-info" CommandArgument='<%# string.Format("Id: \"{0}\", Type: \"{1}\", Name: \"{2}\"", Eval("Id"), Eval("Type"), Eval("Name")) %>' /></td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-1"></div>
                                <div class="col-md-3">
                                    <asp:Button ID="btnAdd" runat="server" Text="Add new affiliation" CssClass="btn btn-info" OnClick="btnAdd_Click" />
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                        <ProgressTemplate>
                            <img src="" alt="Loading.. Please wait!" />
                        </ProgressTemplate>
                    </asp:UpdateProgress>

                </div>

                <div id="editModal" class="modal fade" role="dialog" aria-labelledby="gridSystemModalLabel" aria-hidden="true"
                    data-backdrop="static" data-keyboard="false">
                    <div class="modal-dialog modal-lg">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                                <h4 id="editModalLabel">Add/Edit</h4>
                            </div>
                            <asp:UpdatePanel ID="upEdit" runat="server">
                                <ContentTemplate>
                                    <div class="modal-body">
                                        
                                        <div class="row">
                                            <div class="col-xs-11 col-md-10"></div>
                                            <div class="col-xs-1 col-md-2">
                                                Id: <b><asp:Label ID="lblAffilId" runat="server"></asp:Label></b>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-1 text-left">
                                                <label class="control-label" for="txtTitle">Category:</label>
                                            </div>
                                            <div class="col-sm-3">
                                                <asp:DropDownList ID="ddlCategoryEdit" runat="server" CssClass="form-control">
                                                </asp:DropDownList>                                                
                                            </div>
                                        </div>
                                        <br />
                                        <div class="row">
                                            <div class="col-sm-1 text-left">
                                                <label class="control-label" for="txtTitle">Name:</label></div>
                                            <div class="col-sm-6">
                                                <input class="form-control" type="text" name="txtName" id="txtName" runat="Server" />
                                            </div>
                                        </div>                                     
                                       <br />
                                        </div>
                                        <div class="modal-footer">
                                            <asp:Label ID="lblResult" Visible="false" runat="server"></asp:Label>
                                            <asp:Button ID="btnSave" runat="server" Text="Update" CssClass="btn btn-info" OnClientClick="return validateControl();" OnClick="btnSave_Click" />
                                            <button class="btn btn-info" data-dismiss="modal" aria-hidden="true">Close</button>
                                        </div>
                                 
                                </ContentTemplate>
                                <Triggers>                                   
                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>                                    
                                
                        </div>
                    </div>
                </div>
            </div>
        </div>

    

    <script type="text/javascript">
        function pageLoad(sender, args) {
        }

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            ddlCategoryChange();
            validateControl();
        });

        function ddlCategoryChange() {
            var category = $("#MainContent_ddlCategory").val();

            $('#tblAffiliation tr').not(":first").each(function (i, row) {
                var $row = $(row),
                    $family = $row.find('input[name$="txtCategory"]');

                if ($family.val() == category || category.length == 0)
                    $row.show();
                else
                    $row.hide();
            });

        }

        function validateControl() {
            var validator = $("#commentForm").validate({
                //only works with js in page
                rules: {
                    <%=ddlCategoryEdit.UniqueID %>: {
                        required:true
                    },
                    <%=txtName.UniqueID %>: {
                        required:true,
                        minlength: 10
                    }
                }

            });

            var isValid = $("#commentForm").valid();

            if (validator.errorList.length > 0) {
                var firstElement = validator.errorList[0].element;
                firstElement.focus();
            }

            return isValid;
        }

    </script>
</asp:Content>
