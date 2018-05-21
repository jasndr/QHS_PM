<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProjectList.aspx.cs" Inherits="ProjectManagement.ProjectList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"> 
    <%--<script src="Scripts/jquery-1.10.2.min.js"></script>--%>
    <script src="Scripts/jquery.bootpag.min.js"></script>    
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
    <div class="jumbotron">

        <div class="panel panel-default hidden">
            <div class="panel-heading"><b>Client Request</b></div>
            <div class="panel-body">

                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div>
                            <table class="table table-striped table-hover">
                                <thead>
                                    <tr>
                                        <th style="width: 5%; padding: 0,0,0,1px;">Id</th>
                                        <th style="width: 15%; padding: 0,0,0,1px;">First Name</th>
                                        <th style="width: 15%; padding: 0,0,0,1px;">Last Name</th>
                                        <th style="width: 35%; padding: 0,0,0,1px;">Project Title</th>
                                        <th style="width: 15%; padding: 0,0,0,1px;">Creation Date</th>
                                        <th style="width: 10%; padding: 0,0,0,1px;">Status</th>
                                        <th style="width: 10%; padding: 0,0,0,1px;"></th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Repeater ID="rptClientRqst" runat="server" OnItemCommand="rptClientRqst_ItemCommand">
                                        <ItemTemplate>
                                            <tr>
                                                <td><%# Eval("Id") %></td>
                                                <td><%# Eval("FirstName") %></td>
                                                <td><%# Eval("LastName") %></td>
                                                <td><%# Eval("ProjectTitle") %></td>
                                                <td><%# Eval("CreateDate") %></td>
                                                <td><%# Eval("Status") %></td>
                                                <td>
                                                    <asp:Button ID="btnRequest" runat="server" Text="Edit" CssClass="btn btn-info" CommandArgument='<%# Eval("Id") %>' /></td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tbody>
                            </table>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                    <ProgressTemplate>
                        <img src="" alt="Loading.. Please wait!" />
                    </ProgressTemplate>
                </asp:UpdateProgress>
                                      
            </div>
        </div>

        <div id="editModal" class="modal fade" role="dialog" aria-labelledby="gridSystemModalLabel" aria-hidden="true"
            data-backdrop="static" data-keyboard="false">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 id="editModalLabel">Client Request</h4>
                    </div>
                    <asp:UpdatePanel ID="upEdit" runat="server">
                        <ContentTemplate>
                            <div class="modal-body">

                                <div class="row">
                                    <div class="col-xs-11 col-md-11"></div>
                                    <div class="col-xs-1 col-md-1">
                                        Id: <b>
                                            <asp:Label ID="lblClientRqstId" runat="server"></asp:Label></b>
                                    </div>
                                </div>
                                <h5>Investigator Info</h5>
                                <div class="row form-group-md">
                                    <%--<label class="col-sm-2 control-label" for="lblFirstName">First name</label>--%>
                                    <div class="col-sm-4"><b>First name:</b>
                                        <asp:Label ID="lblFirstName" runat="server"></asp:Label>
                                         <%--<input class="form-control disbaled" type="text" Id="lblFirstName" runat="server" />--%>
                                    </div>
                                    <div class="col-sm-4"><b>Last name:</b>
                                        <asp:Label ID="lblLastName" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-sm-4"><b>Degree:</b>
                                        <asp:Label ID="lblDegree" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-sm-4"><b>Emaile:</b>
                                        <asp:Label ID="lblEmail" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-sm-4"><b>Phone:</b>
                                        <asp:Label ID="lblPhone" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-sm-12"><b>Organization/Department:</b>
                                        <asp:Label ID="lblDept" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <br />
                                <div class="row">                                    
                                    <div class="col-sm-4"><b>Investigator status:</b>
                                        <asp:Label ID="lblInvestStatus" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <h5>Project Info</h5>
                                <div class="row">
                                    <div class="col-sm-12"><b>Project Title:</b>
                                        <asp:Label ID="lblProjectTitle" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-sm-12"><b>Project Summary:</b>
                                        <asp:Label ID="lblProjectSummary" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-sm-12"><b>Study Area:</b>
                                        <asp:Label ID="lblStudyArea" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-sm-12"><b>Type of support:</b>
                                        <asp:Label ID="lblServiceType" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-sm-12"><b>Important deadlines:</b>
                                        <asp:Label ID="lblDueDate" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-sm-12"><b>BQHS faculty/staff preference (if any):</b>
                                        <asp:Label ID="lblPreferBiostat" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <br />    
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:CheckBox ID="chkCompleted" runat="server" Text="Request Completed" />
                                    </div>
                                </div>
                                <%--<div class="row">
                                    <div class="col-xs-3 col-md-3">
                                        <label class="control-label">Assign To:</label>
                                        <asp:DropDownList ID="ddlAssignTo" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-xs-3 col-md-3">
                                        <label class="control-label">Due Date:</label>
                                        <div class='input-group date' id='dtpDueDate'>
                                            <span class="input-group-addon">
                                                <span class="glyphicon glyphicon-calendar"></span>
                                            </span>
                                            <asp:TextBox ID="txtDueDate" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>--%>
                               <%-- <div class="row">
                                    <div class="col-xs-3 col-md-3">
                                        <label class="control-label">Task Status:</label>
                                        <asp:DropDownList ID="ddlTaskStatus" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-xs-3 col-md-3"></div>

                                </div>--%>
                                <%--<div class="row">
                                    <div class="col-xs-9 col-md-9">
                                        <label class="control-label">Note:</label>
                                        <asp:TextBox ID="txtTaskNote" runat="server" placeholder="Note" class="form-control"></asp:TextBox>
                                    </div>
                                </div>  --%>                          
                            </div>

                            <div class="modal-footer">
                                <asp:Label ID="lblResult" Visible="false" runat="server"></asp:Label>
                                <asp:Button ID="btnSave" runat="server" Text="Update" CssClass="btn btn-info" OnClientClick="return validateControl();" OnClick="btnSave_Click" />
                                <button class="btn btn-info" data-dismiss="modal" aria-hidden="true">Close</button>
                            </div>

                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="rptClientRqst" EventName="ItemCommand" />
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>



                </div>
            </div>
        </div>

        <div class="panel panel-default">
            <div class="panel-heading"><b>Investigator Pending Review</b></div>
            <div class="panel-body">
                <div>
                    <asp:GridView ID="GridViewPI" runat="server"
                        OnRowCommand="GridViewPI_RowCommand"
                        AutoGenerateColumns="false" AllowPaging="false"
                        DataKeyNames="Id"
                        class="table table-bordered table-hover"
                        EmptyDataText="No record">
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="Id" Visible="false" />
                            <asp:BoundField DataField="FirstName" HeaderText="FirstName" />
                            <asp:BoundField DataField="LastName" HeaderText="LastName" />
                            <asp:BoundField DataField="Email" HeaderText="Email" />
                            <asp:BoundField DataField="Phone" HeaderText="Phone" />
                            <asp:BoundField DataField="StatusValue" HeaderText="Status" />                           
                            <asp:ButtonField CommandName="editRecord"
                                ControlStyle-CssClass="btn btn-info" ButtonType="Button"
                                Text="Edit" HeaderText="" />                           
                        </Columns>
                    </asp:GridView>
                </div>               
            </div>
        </div>

        <div class="panel panel-default">
            <div class="panel-heading"><b>Project Pending Review</b></div>
            <div class="panel-body">
                <div>
                    <asp:GridView ID="GridViewPending" runat="server"
                        OnRowCommand="GridViewPending_RowCommand"
                        AutoGenerateColumns="false" AllowPaging="false"
                        DataKeyNames="Id"
                        class="table table-bordered table-hover"
                        EmptyDataText="No record">
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="Id" />                         
                            <asp:BoundField DataField="Title" HeaderText="Title" />
                            <asp:BoundField DataField="FirstName" HeaderText="PI First Name" />                       
                            <asp:BoundField DataField="LastName" HeaderText="PI Last Name" />
                            <asp:TemplateField HeaderText="Initial Date">
                                <ItemTemplate>
                                    <asp:Label ID="lblInitialDate" runat="server" Text='<%# ((DateTime)Eval("InitialDate")).ToShortDateString()%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>                           
                            <asp:ButtonField CommandName="editRecord"
                                ControlStyle-CssClass="btn btn-info" ButtonType="Button"
                                Text="Edit" HeaderText="" />                           
                        </Columns>
                    </asp:GridView>
                </div>               
            </div>
        </div>

        <div class="panel panel-default" id="divProjectList" runat="server">
            <div class="panel-heading"><b>Approved Projects</b></div>
            <div class="panel-body">
                <div class="row">
                    <div class="col-xs-6 col-md-3">
                        <asp:DropDownList ID="ddlInvestor" runat="server" CssClass="form-control" >
                        </asp:DropDownList>
                    </div>
                    <div class="col-xs-6 col-md-2">
                        <input id="btnSearch" type="button" value="Search" class="btn btn-info"/>
                    </div>
                </div>
                <br />
                <div>
                    <asp:GridView ID="GridViewProject" runat="server"
                        OnRowCommand="GridViewProject_RowCommand"                      
                        AutoGenerateColumns="false" 
                        AllowPaging="true"
                        PageSize="10"
                        DataKeyNames="Id"
                        class="table table-bordered table-hover"
                        EmptyDataText=" "                       
                        OnPreRender="GridViewProject_PreRender"                            
                    >
                        <PagerTemplate>
                            <div id="page-selection" class="pagination"></div>                           
                        </PagerTemplate>
                    <%--<PagerSettings  Mode="NextPreviousFirstLast" FirstPageText="First " PreviousPageText="Previous " NextPageText="Next " LastPageText="Last" PreviousPageImageUrl="images/icon_left_white_arrow.gif" />--%>
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="Id" HeaderStyle-Width="5%"/>                            
                            <asp:BoundField DataField="Title" HeaderText="Title" HeaderStyle-Width="50%"/>
                            <asp:BoundField DataField="FirstName" HeaderText="PI First Name" HeaderStyle-Width="10%"/>                            
                            <asp:BoundField DataField="LastName" HeaderText="PI Last Name" HeaderStyle-Width="10%"/>
                            <asp:TemplateField HeaderText="Initial Date" HeaderStyle-Width="10%">
                                <ItemTemplate>
                                    <%--<asp:Label ID="lblInitialDate" runat="server" Text='<%# ((DateTime)Eval("InitialDate")).ToShortDateString()%>'></asp:Label>--%>
                                    <asp:Label ID="lblInitialDate" runat="server" Text='<%# (Eval("InitialDate"))%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>                           
                            <%--<asp:ButtonField CommandName="editRecord" 
                                ControlStyle-CssClass="btn btn-info" ButtonType="Button"
                                Text="Edit" HeaderText="" /> --%>    
                            <asp:TemplateField HeaderText="Edit" HeaderStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:Button runat="server" Text="Edit"  CommandName="editRecord" />
                                       <%-- Visible='<%# DataBinder.Eval(Container.DataItem,"Id")=="" %>'--%> />
                                </ItemTemplate>
                            </asp:TemplateField>                      
                        </Columns>
                    </asp:GridView>
                    <asp:Button ID="btnAdd" runat="server" Text="Add a New Project" CssClass="btn btn-info" OnClick="btnAdd_Click" />
                </div>                  
          
            </div>
        </div>

    </div>

    <script src="Scripts/WebForms/ProjectList.js"></script>
   
</asp:Content>
