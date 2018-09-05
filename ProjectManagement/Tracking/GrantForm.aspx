<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GrantForm.aspx.cs" Inherits="ProjectManagement.Admin.GrantForm" %>
<%@ Register Assembly="DropDownChosen" Namespace="CustomDropDown" TagPrefix="ucc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Content/chosen.css" rel="stylesheet" />
    <script src="../Scripts/bootstrap-datepicker.min.js"></script>
    <script src="../Scripts/jquery.validate.min.js"></script>   
    <script src="../Scripts/chosen.jquery.js"></script>     
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
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="loader"></div>
    <div id="divGrant" class="jumbotron">
        <div class="panel panel-default">
            <div class="panel-heading"><b>Grants</b></div>
            <div class="panel-body">
                <div class="row">
                    <div class="col-xs-6 col-md-3">
                        Principal Investigator
                        <ucc:DropDownListChosen ID="ddlInvestor" runat="server" Width="200px"
                            NoResultsText="No results match."
                            DataPlaceHolder="Search PI" AllowSingleDeselect="true">
                        </ucc:DropDownListChosen>
                    </div>
                    <div class="col-xs-6 col-md-3">
                        QHS Faculty/Staff
                        <asp:DropDownList ID="ddlBiostat" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                    <div class="col-xs-6 col-md-2">
                        Fund Status
                        <asp:DropDownList ID="ddlFundStatus1" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>                   
                    <div class="col-xs-6 col-md-2">
                        Internal/External
                        <asp:DropDownList ID="ddlInternal" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                    <div class="col-xs-6 col-md-2">&nbsp;
                        <div><asp:Button ID="btnSumbit" runat="server" Text="Submit" CssClass="btn btn-info" OnClick="btnSumbit_Click" OnClientClick="ClientSideClick(this)" UseSubmitBehavior="False"/></div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2">
                        <asp:Button ID="btnExportExcel" runat="server" Text="Export to Excel" CssClass="btn btn-info" OnClick="btnExportExcel_Click" />
                    </div>
                </div>
                <hr />
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div>
                            <div>
                                <asp:Button ID="btnAdd" runat="server" Text="Add a new grant" CssClass="btn btn-info" OnClick="btnAdd_Click" />
                            </div>
                            <br />
                            <asp:GridView ID="GridViewGrant" runat="server"
                                OnRowCommand="GridViewGrant_RowCommand"
                                AutoGenerateColumns="false" AllowPaging="false"
                                DataKeyNames="Id"
                                class="table table-striped table-bordered"
                                EmptyDataText="No record">
                                <Columns>
                                    <asp:TemplateField HeaderText="Id" HeaderStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" ID="btnGrant" CommandName="editRecord" CommandArgument='<%# Eval("Id") %>' OnClientClick="javascript:return loading();">
                                                <asp:Label ID="lblGrantId" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="PI" HeaderText="PI" HeaderStyle-Width="15%" />
                                    <asp:BoundField DataField="Biostat" HeaderText="Biostat" HeaderStyle-Width="15%" />
                                    <asp:BoundField DataField="ProjectId" HeaderText="ProjectId" HeaderStyle-Width="5%" /> 
                                    <asp:BoundField DataField="GrantTitle" HeaderText="GrantTitle" HeaderStyle-Width="40%" />                                    
                                    <asp:BoundField DataField="GrantStatus" HeaderText="GrantStatus" HeaderStyle-Width="5%" />
                                    <asp:BoundField DataField="FundStatus" HeaderText="FundStatus" HeaderStyle-Width="5%" />
                                    <asp:BoundField DataField="FundDate" HeaderText="FundDate" HeaderStyle-Width="5%" />  
                                    <asp:BoundField DataField="InitDate" HeaderText="InitDate" HeaderStyle-Width="5%" />                                                                      
                                </Columns>
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

            </div>
        </div>

        <div id="editModal" class="modal fade" role="dialog" aria-labelledby="gridSystemModalLabel" aria-hidden="true"
            data-backdrop="static" data-keyboard="false">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 id="editModalLabel">Add/Edit Grant</h4>
                    </div>
                    <asp:UpdatePanel ID="upEdit" runat="server">
                        <ContentTemplate>                            
                            <div class="modal-body">
                                <div class="row">
                                    <div class="col-xs-10 col-md-10"></div>
                                    <div class="col-xs-2 col-md-2">
                                        Id: <b><asp:Label ID="lblGrantId" runat="server"></asp:Label></b>
                                    </div>
                                </div>
                                <h5>PROJECT INFO</h5>                                
                                <div class="row">                                    
                                    <div class="col-md-6">
                                        <label class="control-label">Project ID:</label>
                                        <asp:DropDownList ID="ddlProject" runat="server" CssClass="form-control">
                                        </asp:DropDownList>        
                                    </div>
                                </div>
                                <div class="row">                                    
                                    <div class="col-xs-6 col-md-6">
                                        <label class="control-label">Principal Investigator:</label>
                                        <asp:TextBox ID="TextBoxPI" runat="server" placeholder="PI" class="form-control" ></asp:TextBox>                                    
                                    </div>                                     
                                </div>
                                <div class="row">
                                    <div class="col-xs-3 col-md-3">
                                         <label class="control-label">Initial Date: </label>
                                        <div class='input-group date' id='dtpInitDate'>
                                            <span class="input-group-addon">
                                                <span class="glyphicon glyphicon-calendar"></span>
                                            </span>
                                            <asp:TextBox ID="TextBoxInitDate" runat="server" class="form-control"></asp:TextBox>
                                        </div>                                   
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-xs-12 col-md-12">
                                        <label class="control-label">Grant Title:</label>
                                        <asp:TextBox ID="TextBoxGrantTitle" runat="server" placeholder="Grant Title" class="form-control" ></asp:TextBox>                                    
                                    </div>                                
                                </div>
                                <div class="row">
                                    <div class="col-xs-12 col-md-12">
                                        <label class="control-label">Collaborations:</label>
                                        <asp:TextBox ID="TextBoxCollab" runat="server" placeholder="Collaborations" class="form-control" ></asp:TextBox>                                    
                                    </div>                                                                   
                                </div>
                                <div class="row">
                                    <div class="col-xs-6 col-md-6">
                                        <label class="control-label">Health Initiative:</label>
                                        <asp:TextBox ID="TextBoxHealthInit" runat="server" placeholder="Health Initiative" class="form-control" ></asp:TextBox>                                    
                                    </div>                                    
                                    <div class="col-md-2">
                                        <label class="control-label">Native Hawaiian:</label>
                                        <asp:CheckBox ID="chkNativeHawaiian" runat="server" ></asp:CheckBox>
                                    </div>                              
                                </div>
                                <br />         
                                <h5>GRANT INFO</h5>
                                <div class="row">
                                    <div class="col-xs-6 col-md-6">
                                        <label class="control-label">Program Announcement:</label>
                                        <asp:TextBox ID="TextBoxPgmAnn" runat="server" placeholder="PgmAnn" class="form-control" ></asp:TextBox>                                    
                                    </div>                   
                                </div>
                                <div class="row">
                                    <div class="col-xs-6 col-md-6">
                                        <label class="control-label">Funding Agency:</label>
                                        <asp:TextBox ID="TextBoxFundAgcy" runat="server" placeholder="FundAgcy" class="form-control" ></asp:TextBox>                                    
                                    </div>  
                                </div>
                                <div class="row">
                                    <div class="col-xs-6 col-md-6">
                                        <label class="control-label">Grant Type:</label>
                                        <asp:TextBox ID="TextBoxGrantType" runat="server" placeholder="Grant Type" class="form-control" ></asp:TextBox>                                    
                                    </div>
                                    <div class="col-md-1">
                                        <label class="control-label">Internal:</label>
                                        <asp:CheckBox ID="chkInternal" runat="server" ></asp:CheckBox>
                                    </div>                                
                                </div>
                                <div class="row">
                                    <div class="col-xs-3 col-md-3">
                                        <label class="control-label">Grant Status:</label>
                                        <asp:DropDownList ID="ddlGrantStatus" runat="server" CssClass="form-control">
                                        </asp:DropDownList>                                  
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-xs-3 col-md-3">
                                        <label class="control-label">Submission Deadline:</label>
                                        <div class='input-group date' id='dtpSubDeadline'>
                                            <span class="input-group-addon">
                                                <span class="glyphicon glyphicon-calendar"></span>
                                            </span>
                                            <asp:TextBox ID="TextBoxSubDeadline" runat="server" class="form-control"></asp:TextBox>
                                        </div>                                   
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-xs-3 col-md-3">
                                        <label class="control-label">Submission Date:</label>
                                        <div class='input-group date' id='dtpSubDate'>
                                            <span class="input-group-addon">
                                                <span class="glyphicon glyphicon-calendar"></span>
                                            </span>
                                            <asp:TextBox ID="TextBoxSubDate" runat="server" class="form-control"></asp:TextBox>
                                        </div>                                   
                                    </div>
                                    <div class="col-xs-3 col-md-3"></div>
                                    <div class="col-xs-3 col-md-3">
                                        <label class="control-label">MyGrant Number:</label>
                                        <asp:TextBox ID="TextBoxMyGrantNum" runat="server" placeholder="MyGrantNum" class="form-control" ></asp:TextBox>                                    
                                    </div>
                                </div>
                                <br />
                                <h5>FUNDING INFO</h5>
                                <div class="row">
                                    <div class="col-xs-3 col-md-3">
                                        <label class="control-label">Fund Status:</label>
                                        <asp:DropDownList ID="ddlFundStatus" runat="server" CssClass="form-control">
                                        </asp:DropDownList>        
                                    </div>
                                    <div class="col-xs-3 col-md-3"></div>
                                    <div class="col-xs-3 col-md-3">
                                        <label class="control-label">Fund Number:</label>
                                        <asp:TextBox ID="TextBoxFundNum" runat="server" placeholder="FundNum" class="form-control" ></asp:TextBox>                                    
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-xs-3 col-md-3">
                                        <label class="control-label">Fund Date:</label>
                                        <div class='input-group date' id='dtpFundDate'>
                                            <span class="input-group-addon">
                                                <span class="glyphicon glyphicon-calendar"></span>
                                            </span>
                                            <asp:TextBox ID="TextBoxFundDate" runat="server" class="form-control"></asp:TextBox>
                                        </div>                                   
                                    </div> 
                                </div>
                                <div class="row">
                                    <div class="col-xs-3 col-md-3">
                                        <label class="control-label">Award Start Date:</label>
                                        <div class='input-group date' id='dtpStartDate'>
                                            <span class="input-group-addon">
                                                <span class="glyphicon glyphicon-calendar"></span>
                                            </span>
                                            <asp:TextBox ID="TextBoxStartDate" runat="server" class="form-control"></asp:TextBox>
                                        </div>                                   
                                    </div>
                                    <div class="col-xs-3 col-md-3"></div>
                                    <div class="col-xs-3 col-md-3">
                                        <label class="control-label">Award End Date:</label>
                                        <div class='input-group date' id='dtpEndDate'>
                                            <span class="input-group-addon">
                                                <span class="glyphicon glyphicon-calendar"></span>
                                            </span>
                                            <asp:TextBox ID="TextBoxEndDate" runat="server" class="form-control"></asp:TextBox>
                                        </div>                                   
                                    </div>                                  
                                </div>
                                <div class="row">
                                    <div class="col-xs-3 col-md-3">                                        
                                        <label class="control-label">Grant First Year Amount:</label>
                                        <div class='input-group'>
                                            <span class="input-group-addon">
                                                <span class="glyphicon glyphicon-usd"></span>
                                            </span>                                      
                                            <asp:TextBox ID="TextBoxFirstYrAmt" runat="server" placeholder="FirstYrAmt" class="form-control" ></asp:TextBox>
                                        </div>                                    
                                    </div>                                                        
                                </div>
                                <div class="row">
                                    <div class="col-xs-3 col-md-3">
                                        <label class="control-label">Grant Total Amount:</label>
                                        <div class='input-group'>
                                            <span class="input-group-addon">
                                                <span class="glyphicon glyphicon-usd"></span>
                                            </span>
                                            <asp:TextBox ID="TextBoxTotalAmt" runat="server" placeholder="TotalAmt" class="form-control" ></asp:TextBox>
                                        </div>                                  
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-xs-3 col-md-3">
                                        <label class="control-label">SubAward Total Cost:</label>
                                        <div class='input-group'>
                                            <span class="input-group-addon">
                                                <span class="glyphicon glyphicon-usd"></span>
                                            </span>
                                            <asp:TextBox ID="TextBoxTotalCost" runat="server" placeholder="TotalCost" class="form-control" ></asp:TextBox>
                                        </div>                                  
                                    </div>
                                </div>
                                <br />
                                <h5>BIOSTATISTICIAN</h5>
                                <div class="row">
                                    <div class="col-xs-12 col-md-12">
                                        <asp:GridView ID="GridViewBiostat" runat="server" AutoGenerateColumns="False"
                                            ShowFooter="True" 
                                            OnRowDataBound="GridViewBiostat_RowDataBound"
                                            OnRowDeleting="GridViewBiostat_RowDeleting"                                            
                                            class="table table-striped table-bordered" >
                                            <Columns>
                                                <asp:TemplateField HeaderText="Id" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblId" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Biostat" HeaderStyle-Width="20%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBiostatId" runat="server" Text='<%# Eval("BiostatId") %>' Visible="false" />
                                                        <asp:DropDownList ID="ddlBiostats" runat="server" CssClass="form-control">
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                    <FooterStyle HorizontalAlign="Left" />
                                                    <FooterTemplate>
                                                        <asp:Button ID="btnAddBiostat" runat="server" CssClass="btn btn-info"
                                                                Text="Add New" OnClick="btnAddBiostat_Click" />
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Percentage" HeaderStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TextBoxPct" runat="server" class="form-control" Text='<%# Eval("Pct") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Fee" HeaderStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TextBoxFee" runat="server" class="form-control" Text='<%# Eval("Fee") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Year" HeaderStyle-Width="10%">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TextBoxYear" runat="server" class="form-control" Text='<%# Eval("Year") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Note" HeaderStyle-Width="35%">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TextBoxNote" runat="server" class="form-control" Text='<%# Eval("Note") %>'></asp:TextBox>
                                                    </ItemTemplate>                                                    
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="Del"  HeaderStyle-Width="5%">
                                                        <ItemTemplate>                                                            
                                                            <asp:LinkButton ID="lnkDelete" runat="server" Text="Delete" CommandName="Delete" 
                                                                ToolTip="Delete" OnClientClick='return confirm("Are you sure you want to delete this entry?");'
                                                                CommandArgument=''><img src="../images/icon-delete.png" /></asp:LinkButton>
                                                        </ItemTemplate>
                                                        
                                                    </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                                <h5>OTHER</h5>
                                <div class="row">
                                    <div class="col-xs-12 col-md-12">
                                        <label class="control-label">Followup:</label>
                                        <asp:TextBox ID="TextBoxFollowup" runat="server" placeholder="Followup" class="form-control" ></asp:TextBox>                                    
                                    </div>                                
                                </div>
                                <div class="row">
                                    <div class="col-xs-12 col-md-12">
                                        <label class="control-label">Comment:</label>
                                        <asp:TextBox ID="TextBoxComment" runat="server" placeholder="Comment" class="form-control" ></asp:TextBox>                                    
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
                            <asp:AsyncPostBackTrigger ControlID="GridViewGrant" EventName="RowCommand" />
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>

        <%--<div class="pdsa-submit-progress hidden">
            <i class="fa fa-2x fa-spinner fa-spin"></i>&nbsp;
            <label>Please wait ...</label>
        </div>--%>

    </div>

    <script src="../Scripts/WebForms/GrantForm.js"></script>
    
</asp:Content>
