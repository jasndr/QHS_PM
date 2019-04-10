<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PI.aspx.cs" Inherits="ProjectManagement.PI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <%--<script src="Scripts/jquery.validate.min.js"></script>--%>
    <%--<script src="Scripts/typeahead.jquery.min.js"></script>--%>     
    <script src="Scripts/jquery.dataTables.min.js"></script>
    <script src="Scripts/dataTables.bootstrap.min.js"></script>
    <script src="Scripts/InputMask.js"></script>
    <link href="Content/dataTables.bootstrap.min.css" rel="stylesheet" />
    <%--<link href="Content/Site.css" rel="stylesheet" />--%>  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="divPI" class="">
        <div class="panel panel-default">
            <div class="panel-heading"><b>Principal Investigator</b></div>
            <div class="panel-body">                

                <%--<div class="row hidden">
                    <div class="col-xs-6 col-md-3" id="the-basics">
                        <asp:TextBox ID="txtSrchInvestorName" runat="server" placeholder="PI Name" class="form-control typeahead" data-provide="typeahead"></asp:TextBox>
                    </div>
                    <div class="col-xs-6 col-md-3">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" class="btn btn-info" />
                    </div>
                    <textarea id="textAreaPI" cols="30" rows="3" runat="server" class="hidden"></textarea>
                </div>
                <br />--%>

                <div>
                    <%--<asp:ScriptManager ID="ScriptManager1" runat="server" />--%>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <%--<div>                                
                                <asp:GridView ID="GridView1" runat="server"
                                    OnRowCommand="GridView1_RowCommand"
                                    AutoGenerateColumns="false" AllowPaging="false"
                                    DataKeyNames="Id"
                                    class="table table-striped table-hover dataTable"
                                    GridLines="None">                                    
                                    <Columns>
                                        <asp:BoundField DataField="Id" HeaderText="Id" Visible="false" />                                        
                                        <asp:BoundField DataField="FirstName" HeaderText="FirstName" />
                                        <asp:BoundField DataField="LastName" HeaderText="LastName" />
                                        <asp:BoundField DataField="Email" HeaderText="Email" />
                                        <asp:BoundField DataField="Phone" HeaderText="Phone" />
                                        <asp:BoundField DataField="StatusValue" HeaderText="Status" />
                                        <asp:ButtonField CommandName="editRecord"
                                            ControlStyle-CssClass="btn btn-info" ButtonType="Button"
                                            Text="Detail" HeaderText="Detail" />
                                    </Columns>
                                </asp:GridView>                               
                            </div>--%>
                            <div class="row">
                                <div class="col-md-12">
                                    <table class="table table-striped table-bordered dataTable no-footer" id="tblPI">
                                        <thead>
                                            <tr>
                                                <th class="hidden">Id</th>
                                                <th>FirstName</th>
                                                <th>LastName</th>
                                                <th>Email</th>
                                                <th>Phone</th>
                                                <th>Status</th>
                                                <th>View/Edit</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater ID="rptPI" runat="server"  OnItemCommand="rptPI_ItemCommand">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td class="hidden"><%# Eval("Id") %></td>
                                                        <td><%# Eval("FirstName") %></td>
                                                        <td><%# Eval("LastName") %></td>
                                                        <td><%# Eval("Email") %></td>
                                                        <td><%# Eval("Phone") %></td>
                                                        <td><%# Eval("StatusValue") %></td>
                                                        <td>
                                                            <asp:Button ID="btnView" runat="server" Text="Edit" CssClass="btn btn-info" CommandArgument='<%# Eval("Id") %>' /></td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                            <asp:Button ID="btnAdd" runat="server" Text="Add a new PI" CssClass="btn btn-info" OnClick="btnAdd_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                        <ProgressTemplate>
                            <img src="" alt="Loading.. Please wait!" />
                        </ProgressTemplate>
                    </asp:UpdateProgress>

                </div>
            </div>
        </div>

        <div id="currentdetail" class="modal fade" role="dialog" aria-labelledby="gridSystemModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title" id="gridSystemModalLabel">PI Information</h4>
                    </div>
                    <%--<div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:DetailsView ID="DetailsView1" runat="server"
                                CssClass="table table-bordered table-hover"
                                BackColor="White" ForeColor="Black"
                                FieldHeaderStyle-Wrap="false"
                                FieldHeaderStyle-Font-Bold="true"
                                FieldHeaderStyle-BackColor="LavenderBlush"
                                FieldHeaderStyle-ForeColor="Black"
                                BorderStyle="Groove" AutoGenerateRows="False">
                                <Fields>
                                    <asp:BoundField DataField="Id" HeaderText="Code" />
                                    <asp:BoundField DataField="FirstName" HeaderText="Name" />
                                    <asp:BoundField DataField="LastName" HeaderText="Continent" />
                                </Fields>
                            </asp:DetailsView>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="GridView1" EventName="RowCommand" />
                        </Triggers>
                    </asp:UpdatePanel>
                    <div class="modal-footer">
                        <button class="btn btn-info" data-dismiss="modal"
                            aria-hidden="true">
                            Close</button>
                    </div>
                </div>--%>

                    <div class="modal-body">
                        <table class="table">
                            <tr>
                                <td>FirstName : 
                            <asp:TextBox ID="txtFirstName" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>LastName : 
                            <asp:TextBox ID="txtLastName" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Email:
                            <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Phone:
                            <asp:TextBox ID="txtPhone" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                        <button type="button" class="btn btn-primary">Save changes</button>
                    </div>
                </div>
            </div>
        </div>

        <%-- <div id="editModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="editModalLabel" aria-hidden="true">--%>
        <div id="editModal" class="modal fade" role="dialog" aria-labelledby="gridSystemModalLabel" aria-hidden="true"
            data-backdrop="static" data-keyboard="false">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 id="editModalLabel">Add/Edit Record</h4>
                    </div>
                    <asp:UpdatePanel ID="upEdit" runat="server">
                        <ContentTemplate>
                            <div class="modal-body">
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
                                        <asp:TextBox ID="TextBoxPhone" runat="server" placeholder="(___) ___-____" class="form-control phoneNum"></asp:TextBox>
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

                                <div>
                                    <div class="row">
                                        <div class="col-xs-6 col-md-6">
                                            <asp:CheckBox ID="chkApproved" runat="server" Text="Reviewed" />
                                        </div>
                                    </div>
                                </div>

                            </div>
                            <div class="modal-footer">
                                <asp:Label ID="lblResult" Visible="false" runat="server"></asp:Label>
                                <asp:Button ID="btnSave" runat="server" Text="Update" CssClass="btn btn-info" OnClientClick="return validateControl();" OnClick="btnSave_Click" />
                                <button class="btn btn-info" data-dismiss="modal" aria-hidden="true">Close</button>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="rptPI" EventName="ItemCommand" />
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>

    </div>

    
    <script type="text/javascript">

        //$(document).ready(function () {
        $(window).load(function () {

            //Automatically put parentheses and dash for phone number fields
            $(function () {
                var phones = [{ "mask": "(###) ###-####" }, { "mask": "(###) ###-##############" }];
                $('.phoneNum').inputmask({
                    mask: phones,
                    greedy: false,
                    definitions: { '#': { validator: "[0-9]", cardinality: 1 } }
                });
            });

        });

        function pageLoad(sender, args) {
            $('#li_pi').addClass('selected');

            $('#tblPI').DataTable({
                "aoColumns": [
                          null,  
                          null,
                          null,
                          { "bSortable": false },
                          { "bSortable": false },
                          { "bSortable": false },
                          { "bSortable": false }
                ]                
            });

            $('#editModal').on('shown.bs.modal', function () {
                $('#editModal').scrollTop(0);
            });                               
            
            //$('#currentdetail').modal('show');
            gridviewUHDeptToggle();

            //divNonHawaiiToggle();
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
            }
            else {
                $('#MainContent_GridViewCommunityPartner_txtCommunityPartnerOther').hide();
            }

            //var jsonString = $('#MainContent_textAreaPI').val();
            //var jsonObj = $.parseJSON(jsonString);
            //var sourceArr = [];

            //for (var i = 0; i < jsonObj.length; i++) {
            //    sourceArr.push(jsonObj[i].label);
            //}
                        
            //$('#scrollable-dropdown-menu .typeahead').typeahead(null, {
            //    name: 'pies',
            //    limit: 10,
            //    source: sourceArr
            //});

            //$("#MainContent_txtSrchInvestorName").typeahead({
            //    source: sourceArr
            //});
            //var countries = ["India", "United States", "Canada"];
 
            //$("#typeahead").typeahead(null, {
            //    source: countries
            //});

            
            
        }    
                     
        
        function GridViewNonUHToggle(e) {
            if (e.id == 'MainContent_GridViewNonUH_chkRow_8') {
                if (e.checked)
                    $('#MainContent_GridViewNonUH_txtNonUHOther').show();
                else {
                    $('#MainContent_GridViewNonUH_txtNonUHOther').val('');
                    $('#MainContent_GridViewNonUH_txtNonUHOther').hide();
                }
            }
        }  
        
        function GridViewDegreeToggle(e) {
            if (e.id == 'MainContent_GridViewDegree_chkRow_10') {
                if (e.checked)
                    $('#MainContent_GridViewDegree_txtDegreeOther').show();
                else {
                    $('#MainContent_GridViewDegree_txtDegreeOther').val('');
                    $('#MainContent_GridViewDegree_txtDegreeOther').hide();
                }
            }
        } 

        function GridViewCommunityPartnerToggle(e) {
            if (e.id == 'MainContent_GridViewCommunityPartner_chkRow_4') {
                if (e.checked)
                    $('#MainContent_GridViewCommunityPartner_txtCommunityPartnerOther').show();
                else {
                    $('#MainContent_GridViewCommunityPartner_txtCommunityPartnerOther').val('');
                    $('#MainContent_GridViewCommunityPartner_txtCommunityPartnerOther').hide();
                }
            }
        } 

        function divNonHawaiiToggle() {
            bindDivNonHawaii();
            $('#MainContent_TextBoxNonHawaii').val('');
        }

        function bindDivNonHawaii() {
            var DropdownListStatus = document.getElementById('<%=ddlStatus.ClientID %>');
            var SelectedIndex = DropdownListStatus.selectedIndex;

            var SelectedText= DropdownListStatus.options[DropdownListStatus.selectedIndex].text;

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

        function gridviewUHDeptToggle() {
            var DropdownListJabsomOther = document.getElementById('<%=ddlJabsomOther.ClientID %>');
            var SelectedIndexJabsomOther = DropdownListJabsomOther.selectedIndex;
            //var SelectedValue = DropdownList.value;
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

            <%-- 
            var DropdownListHospital = document.getElementById('<%=ddlHospital.ClientID %>');
            if (SelectedTextJabsomOther.indexOf("Major Hospitals") > -1) {
                document.getElementById('<%=GridViewHph.ClientID %>').style.display = "none";
            }
            else
            {
                DropdownListHospital.selectedIndex = 0;
            }

            var DropdownListHospital = document.getElementById('<%=ddlHospital.ClientID %>');
            var SelectedIndexHospital = DropdownListHospital.selectedIndex;
            var SelectedTextHospital = DropdownListHospital.options[DropdownListHospital.selectedIndex].text;

            if (SelectedTextHospital.indexOf("Hawaii Pacific Health Hospitals") > -1 && SelectedTextJabsomOther.indexOf("Major Hospitals") > -1) {
                document.getElementById('<%=GridViewHph.ClientID %>').style.display = "inherit";
            }
            else {
                document.getElementById('<%=GridViewHph.ClientID %>').style.display = "none";
            }
            --%>
        }

        function SelectSingleRadiobutton(rdbtnid) {
            var rdBtn = document.getElementById(rdbtnid);
            var rdBtnList = document.getElementsByTagName("input");
            for (i = 0; i < rdBtnList.length; i++) {
                if (rdBtnList[i].type == "radio" && rdBtnList[i].id != rdBtn.id && rdBtnList[i].name != "optionsRadios") {
                    rdBtnList[i].checked = false;
                }
            }
        }

        function validateControl() {
            $("#commentForm").validate({
                rules: {
                    <%=TextBoxFirstName.UniqueID %>: {
                        required:true
                    },
                    <%=TextBoxLastName.UniqueID %>: {
                        required:true
                    },
                    <%=TextBoxEmail.UniqueID %>: {
                        required:true,
                        email:true
                    }
                }
            });
        }
        
        
        var substringMatcher = function(strs) {
            return function findMatches(q, cb) {
                var matches, substringRegex;

                // an array that will be populated with substring matches
                matches = [];

                // regex used to determine if a string contains the substring `q`
                substrRegex = new RegExp(q, 'i');

                // iterate through the pool of strings and for any string that
                // contains the substring `q`, add it to the `matches` array
                $.each(strs, function(i, str) {
                    if (substrRegex.test(str)) {
                        matches.push(str);
                    }
                });

                cb(matches);
            };
        };

        //var pinames = [];
        //var map = {};
            
        //var data = JSON.parse($('#MainContent_textAreaPI').val());
                    
        //$.each(data, function (i, piname) {
        //        map[piname.FullName] = piname;
        //        pinames.push(piname.FullName);
        //    });                 
                                
        //$('#the-basics .typeahead').typeahead({
        //    hint: true,
        //    highlight: true,
        //    minLength: 1
        //},
        //{
        //    name: 'piname',
        //    source: substringMatcher(pinames)

        //    //source: function (query, process) {
        //    //    var pis = [];
        //    //    var map = {};

        //    //    var data = JSON.parse($('#MainContent_textAreaPI').val());

        //    //    $.each(data, function (i, piname) {
        //    //        map[piname.FullName] = piname;
        //    //        pis.push(piname.FullName);
        //    //    });

        //    //    process(pis);
        //    //},

        //    //matcher: function (item) {
        //    //    if (item.toLowerCase().indexOf(this.query.trim().toLowerCase()) != -1) {
        //    //        return true;
        //    //    }
        //    //},

        //    //highlighter: function (item) {
        //    //    var regex = new RegExp( '(' + this.query + ')', 'gi' );
        //    //    return item.replace( regex, "<strong>$1</strong>" );
        //    //}

        //});

       
    </script>

</asp:Content>
