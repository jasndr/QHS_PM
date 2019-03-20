<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="InvoiceForm.aspx.cs" Inherits="ProjectManagement.Admin.InvoiceForm" %>

<%@ Register Assembly="DropDownChosen" Namespace="CustomDropDown" TagPrefix="ucc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">    
    <script src="../Scripts/bootstrap-datepicker.min.js"></script>
    <script src="../Scripts/jquery.validate.min.js"></script> 
    <script src="../Scripts/fileinput.js"></script>
    <script src="<%=Page.ResolveUrl("~/Scripts/jquery.dataTables.min.js")%>"></script>
    <script src="<%=Page.ResolveUrl("~/Scripts/dataTables.bootstrap.min.js")%>"></script>
    <script src="../Scripts/chosen.jquery.js"></script>
    <link href="../Content/dataTables.bootstrap.min.css" rel="stylesheet" />
    <link href="../Content/fileinput.css" rel="stylesheet" />
    <link href="../Content/chosen.css" rel="stylesheet" />
    <style>
       .rightAlign {
            text-align: right;
        }
       .file-preview-frame  {
           height : 482px;
           width : 100%;
       }
       .fileinput-upload-button{
           display : none;
       }
       .file-footer-buttons {
            display : none;
        }
       body .modal-xxl {
            /* new custom width */
            width: 90em;
            /* must be half of the width, minus scrollbar on the left (30px) */
            /*margin-left: -280px;*/
       }
       #MainContent_collabCenterType label{
           font-weight: normal;
       }
       #collabCentersSelect .chosen-container .chosen-drop{
           width: 100% !important;
       }
       #collabCentersSelect .chosen-single{
            width: 90% !important;
       }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="divInvoice" class="">
        <div class="panel panel-default">
            <div class="panel-heading"><b>Invoice</b></div>
            <div class="panel-body">

                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div class="row">
                            <div class="col-sm-5">
                                <label class="control-label">Collab Center Type:</label>
                                <asp:RadioButtonList ID="collabCenterType" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" OnSelectedIndexChanged="collabCenterType_Changed" AutoPostBack="true">
                                    <asp:ListItem Text="All" Value="all" Selected="True" />
                                    <asp:ListItem Text="Active" Value="active" />
                                    <asp:ListItem Text="Inactive" Value="inactive" />
                                </asp:RadioButtonList>
                            </div>
                            <div class="col-sm-2 text-right">
                                <label class="control-label" for="txtTitle">Collab Center:</label>
                            </div>
                            <div class="col-sm-5">
                                <%--<asp:DropDownList ID="ddlCollab" runat="server" CssClass="form-control"  OnSelectedIndexChanged="ddlCollab_Changed" AutoPostBack="True">
                                </asp:DropDownList>--%>
                                <div id="collabCentersSelect">
                                    <ucc:DropDownListChosen ID="ddlCollab" runat="server"
                                        CssClass="form-control"
                                        NoResultsText="No results match."
                                        DataPlaceHolder="Search Projects" AllowSingleDeselect="true"
                                        OnSelectedIndexChanged="ddlCollab_Changed" AutoPostBack="true"
                                        >
                                      
                                    </ucc:DropDownListChosen>
                                </div>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-12">   
                                        <asp:Repeater ID="rptInvoice" runat="server" OnItemCommand="rptInvoice_ItemCommand">
                                            <HeaderTemplate>
                                                <table class="table table-striped table-bordered dataTable no-footer" id="invoice">
                                                    <thead>
                                                        <tr>
                                                            <th>Id</th>
                                                            <%--<th style="width: 10%; padding: 0,0,0,1px;">Collab</th>--%>
                                                            <th>Start Date</th>
                                                            <th>End Date</th>
                                                            <th>Invoice Date</th>
                                                            <%--<th style="width: 10%; padding: 0,0,0,1px;">Subtotal</th>--%>
                                                            <%--<th style="width: 10%; padding: 0,0,0,1px;">Discount</th>--%>
                                                            <th>Rcvd Date</th>
                                                            <th>Payment</th>                                            
                                                            <%--<th style="width: 10%; padding: 0,0,0,1px;">Creator</th>--%>
                                                            <th></th>
                                                        </tr>
                                                    </thead>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <%--<td><%# Eval("CollabCtr") %></td>--%>
                                                    <td><%# Eval("InvoiceId") %></td>                                                    
                                                    <td><%# Eval("StartDate") %></td>
                                                    <td><%# Eval("EndDate") %></td> 
                                                    <td><%# Eval("InvoiceDate") %></td> 
                                                    <%--<td><%# Eval("Subtotal") %></td>--%> 
                                                    <%--<td><%# Eval("Discount") %></td>--%> 
                                                    <td><%# Eval("RcvdDate") %></td> 
                                                    <td><%# Eval("Payment") %></td>                                                     
                                                    <%--<td><%# Eval("Creator") %></td>   --%>                                                
                                                    <td>
                                                        <asp:Button ID="btnView" runat="server" Text="Edit" CssClass="btn btn-info" CommandArgument='<%# Eval("Id") %>' /></td>
                                                </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </table>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                   
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
        </div>

        <div id="editModal" class="modal fade" role="dialog" aria-labelledby="gridSystemModalLabel" aria-hidden="true"
            data-backdrop="static" data-keyboard="false">
            <div class="modal-dialog modal-xxl">
                <div class="modal-content">

                    <div class="panel panel-info">
                        <div class="panel-heading">              
                            <h4 class="text-center"><b>Invoice Form</b></h4>                                
                        </div>
                    </div>
                  
                    <asp:UpdatePanel ID="upEdit" runat="server">
                        <ContentTemplate>
                            <div class="modal-body">
                                <div class="row form-group-md">
                                    <div class="col-sm-2 text-left"><label class="control-label" for="txtCCAbbrv">Collaborative center:</label></div>
                                    <div class="col-sm-2">
                                        <input class="form-control" type="text" name="txtCCAbbrv" id="txtCCAbbrv" runat="Server" readonly />
                                        <input class="form-control hidden" type="text" name="txtCCId" id="txtCCId" runat="Server" readonly />
                                    </div>
                                    <label class="col-sm-2 control-label" for="txtInvoiceDate">Invoice date:</label>
                                    <div class="col-sm-2">
                                        <div class='input-group date' id='dtpInvoiceDate'>
                                            <span class="input-group-addon">
                                                <span class="glyphicon glyphicon-calendar"></span>
                                            </span>
                                            <asp:TextBox ID="txtInvoiceDate" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <label class="col-sm-2 control-label" for="txtInvoiceId">Invoice ID:</label>
                                    <div class="col-sm-2">
                                        <input class="form-control" type="text" name="txtInvoiceId" id="txtInvoiceId" runat="Server" />
                                        <input class="form-control hidden" type="text" name="txtId" id="txtId" runat="Server" />
                                    </div>                                    
                                </div>                                
                                <br />
                                <div class="row form-group-md">
                                    <label class="col-sm-2 text-left" for="txtStartDate">Invoice Start date:</label>
                                    <div class="col-sm-2">
                                        <div class='input-group date' id='dtpStartDate'>
                                            <span class="input-group-addon">
                                                <span class="glyphicon glyphicon-calendar"></span>
                                            </span>
                                            <asp:TextBox ID="txtStartDate" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <label class="col-sm-2 control-label" for="txtEndDate">Invoice End date:</label>
                                    <div class="col-sm-2">
                                        <div class='input-group date' id='dtpEndDate'>
                                            <span class="input-group-addon">
                                                <span class="glyphicon glyphicon-calendar"></span>
                                            </span>
                                            <asp:TextBox ID="txtEndDate" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-sm-1"></div>
                                    <div class="col-sm-1">
                                        <asp:CheckBox ID="chkRemoveZeros" runat="server" Text="Remove 0 hours?"></asp:CheckBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:Button ID="btnFetchInvoice" runat="server" Text="Fetch Invoice" CssClass="btn btn-info" OnClick="btnFetchInvoice_Click" />
                                    </div>
                                </div>
                                <br />
                                <hr />
                                <div class="row">

                                    <table class="table table-hover" id="newInvoice">
                                        <thead>
                                            <tr>
                                                <th>Id</th>
                                                <th>ProjectId</th>
                                                <th>PI</th>
                                                <th class="col-md-2">Title</th>
                                                <th>Agreement</th>
                                                <th>Type</th>
                                                <th>Approved</th>
                                                <th>Invoiced</th>
                                                <th>Remain</th>
                                                <th>TimeTrack</th>
                                                <th class="col-md-2">TobeBilled</th>
                                                <th class="col-md-2">Rate</th>
                                                <th class="col-md-2">SubTotal</th>
                                                <th class="col-md-2">Discount</th>
                                                <th class="col-md-2">AfterBill</th>
                                                <th>Del</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater ID="rptNewInvoice" runat="server" OnItemCommand="rptNewInvoice_ItemCommand" OnItemCreated="rptNewInvoice_ItemCreated">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblId" runat="server" Text='<%# (int)Eval("Id") > 0 ? Eval("Id") : "" %>' /> 
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblProjectId" runat="server" Text='<%#Eval("ProjectId")%>' /> 
                                                        </td>
                                                        <td>
                                                             <asp:Label ID="lblPI" runat="server" Text='<%#Eval("PI")%>' /> 
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblProjectTitle" runat="server" Text='<%#Eval("Title")%>' /> 
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblAgreement" runat="server" Text='<%#Eval("Agreement")%>' /> 
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblType" runat="server" Text='<%#Eval("Type")%>' name="lblType"/> 
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblApprovedHr" runat="server" Text='<%#Eval("Approved")%>' /> 
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblInvoicedHr" runat="server" Text='<%#Eval("Invoiced")%>' /> 
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblRemainingHr" runat="server" Text='<%#Eval("Remain")%>' name="lblRemainingHr"/> 
                                                        </td>
                                                        <td>                                                            
                                                            <asp:Label ID="lblTimeTracking" runat="server" Text='<%#Eval("TimeTracking")%>' name="lblTimeTracking" />
                                                        </td>
                                                        <td>
                                                            <%--<asp:TextBox ID="txtToBeBilled" runat="server" class="form-control" Text='<%# Eval("ToBeBilled") %>'></asp:TextBox>--%>
                                                            <input class="form-control" type="text" name="txtToBeBilled" onchange="bindNewInvoice()" id="txtToBeBilled" runat="server" value='<%#Eval("ToBeBilled")%>' />
                                                        </td>
                                                        <td>
                                                            <%--<asp:Label ID="lblRate" runat="server" Text='<%#Eval("Rate")%>' />--%>
                                                            <asp:TextBox ID="txtRate" runat="server" class="form-control" Text='<%# Eval("Rate") %>' readonly></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <%--<asp:Label ID="lblSubTotal" runat="server"  />--%>
                                                            <input class="form-control" type="text" name="txtSubTotal2" onclick="calcSubTotal2(this)" id="txtSubTotal2" runat="server" value='<%#Eval("SubTotal")%>' readonly />
                                                            <%--<asp:TextBox ID="txtSubTotal2" runat="server" class="form-control" Text='<%# Eval("SubTotal") %>'></asp:TextBox>--%>
                                                        </td>
                                                        <td>
                                                            <%--<asp:Label ID="lblDiscountHr" runat="server" Text='<%#Eval("Discount")%>' />--%>
                                                             <input class="form-control" type="text" name="lblDiscountHr" id="lblDiscountHr" runat="server" value='<%#Eval("Discount")%>' readonly />
                                                        </td>
                                                        <td>
                                                            <%--<asp:Label ID="lblRemainAfterBill" runat="server" Text='<%#Eval("RemainAfter")%>' />--%>
                                                            <input class="form-control" type="text" name="lblRemainAfterBill" id="lblRemainAfterBill" runat="server" value='<%#Eval("RemainAfter")%>' readonly />
                                                        </td>
                                                        <td>
                                                            <%--<asp:Button ID="btnDelete" runat="server" Text="Del" CssClass="btn btn-info" CommandArgument='<%# Eval("Id") %>' />--%>
                                                             <asp:LinkButton ID="lnkDelete" runat="server" Text="Delete" CommandName="Delete" 
                                                                ToolTip="Delete" OnClientClick='return confirm("Are you sure you want to delete this entry?");'
                                                                CommandArgument='<%# Eval("Id") %>'><img src="../images/icon-delete.png" /></asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <FooterTemplate>                                                    
                                                    <tr>
                                                        <td colspan="5"></td>
                                                        <td colspan="2">
                                                            <label class="control-label" for="txtPhdTotal">PhDTotal:</label>
                                                        </td>
                                                        <td colspan="2">
                                                            <input class="form-control" type="text" name="txtPhdTotal" id="txtPhdTotal" runat="server" />
                                                        </td>
                                                        <td colspan="2"></td>
                                                        <td>
                                                            <label class="control-label" for="txtSubTotalNew">SubTotal:</label>
                                                        </td>
                                                        <td colspan="2">
                                                            <input class="form-control" type="text" name="txtSubTotalNew" id="txtSubTotalNew" runat="server" />
                                                        </td>
                                                        <td></td>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="5"></td>
                                                        <td colspan="2">
                                                            <label class="control-label" for="txtMsTotal">MSTotal:</label>
                                                        </td>
                                                        <td colspan="2">
                                                            <input class="form-control" type="text" name="txtMsTotal" id="txtMsTotal" runat="server" />
                                                        </td>
                                                        <td colspan="2"></td>
                                                        <td>
                                                            <label class="control-label" for="txtDiscountNew">Discount:</label>
                                                        </td>
                                                        <td colspan="2">
                                                            <input class="form-control" type="text" name="txtDiscountNew" id="txtDiscountNew" runat="server" />
                                                        </td>
                                                        <td></td>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="11"></td>
                                                        <td>
                                                            <label class="control-label" for="txtGrandTotalNew">GrandTotal:</label>
                                                        </td>
                                                        <td colspan="2">
                                                            <input class="form-control" type="text" name="txtGrandTotalNew" id="txtGrandTotalNew" runat="server" />
                                                        </td>
                                                        <td></td>
                                                        <td></td>
                                                    </tr>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </tbody>
                                    </table>
                                </div>

                                <div class="row">
                                    <div class="col-sm-11"></div>
                                    <div class="col-sm-1">
                                        <asp:Button ID="btnExportExcel" runat="server" Text="Export" CssClass="btn btn-info" OnClick="btnExportExcel_Click" />
                                    </div>
                                </div>
                                <br />

                                <%--<div class="row hidden">
                                    <div class="hidden">
                                        <asp:DropDownList ID="ddlBiostatHdn" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddlAgreementHdn" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-xs-12 col-md-12 hidden">
                                        <asp:GridView ID="gvInvoiceItem" runat="server" AutoGenerateColumns="False"
                                            ShowFooter="True"
                                            OnRowDataBound="gvInvoiceItem_RowDataBound"
                                            OnRowDeleting="gvInvoiceItem_RowDeleting"
                                            class="table table-striped table-bordered">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Id" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblId" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Agreement" HeaderStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAgreementId" runat="server" Text='<%# Eval("AgreementId") %>' Visible="false" />
                                                        <asp:DropDownList ID="ddlAgreement" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlAgreement_SelectedIndexChanged" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                    <FooterStyle HorizontalAlign="Left" />
                                                    <FooterTemplate>
                                                        <asp:Button ID="btnAddInvoiceItem" runat="server" CssClass="btn btn-info"
                                                            Text="Add New" OnClick="btnAddInvoiceItem_Click" />
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Agmt Date" HeaderStyle-Width="10%">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtAgreementDate" runat="server" class="form-control" Text='<%# Eval("AgreementDate") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Biostat" HeaderStyle-Width="12%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBiostatId" runat="server" Text='<%# Eval("BiostatId") %>' Visible="false" />
                                                        <asp:DropDownList ID="ddlBiostat" runat="server" CssClass="form-control"  OnSelectedIndexChanged="ddlBiostat_SelectedIndexChanged" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Project" HeaderStyle-Width="8%">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtProjectId" runat="server" class="form-control" Text='<%# Eval("ProjectId") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Desc" HeaderStyle-Width="20%">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtDesc" runat="server" class="form-control" Text='<%# Eval("Desc") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Rate" HeaderStyle-Width="10%">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtRate" runat="server" class="form-control" Text='<%# Eval("Rate") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Completed Hr" HeaderStyle-Width="10%">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtCompletedHours" runat="server" class="form-control" Text='<%# Eval("CompletedHours") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Total" HeaderStyle-Width="10%">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtTotalCost" runat="server" class="form-control" Text='<%# Eval("TotalCost") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>                                                
                                                
                                                <asp:TemplateField HeaderText="Del" HeaderStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkDelete" runat="server" Text="Delete" CommandName="Delete"
                                                            ToolTip="Delete" OnClientClick='return confirm("Are you sure you want to delete this entry?");'
                                                            CommandArgument=''><img src="../images/icon-delete.png" /></asp:LinkButton>
                                                    </ItemTemplate>

                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                                       
                                    </div>
                                </div>    --%>                            

                               <%-- <div class="row hidden">
                                    <div class="col-sm-4">
                                        <table class="table table-striped table-hover" id="tblAgmt">
                                            <thead>
                                                <tr>
                                                    <th>Agreement Id</th>
                                                    <th>Total Cost</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                            </tbody>
                                        </table>
                                    </div>

                                    <div class="col-sm-8 ">
                                        <div class="row form-group-md">
                                            <div class="col-sm-7"></div>
                                            <label class="col-sm-2 control-label" for="txtSubTotal">Sub-Total:</label>
                                            <div class="col-sm-3">
                                                <input class="form-control" type="text" name="txtSubTotal" onclick="calcSubTotal(this)" id="txtSubTotal" runat="server" />
                                            </div>                                            
                                        </div>
                                        <br />
                                        <div class="row form-group-md">
                                            <div class="col-sm-7"></div>
                                            <label class="col-sm-2 control-label" for="txtDiscount">Discount:</label>
                                            <div class="col-sm-3">
                                                <input class="form-control" type="text" name="txtDiscount" id="txtDiscount" runat="server" />
                                            </div>                                           
                                        </div>
                                        <br />
                                        <div class="row form-group-md">
                                            <div class="col-sm-7"></div>
                                            <label class="col-sm-2 control-label" for="txtSubTotal">Grand-Total:</label>
                                            <div class="col-sm-3">
                                                <input class="form-control" type="text" name="txtGrandTotal" onclick="calcGrandTotal(this)" id="txtGrandTotal" runat="server" />
                                            </div>                                            
                                        </div>
                                    </div>                                                               
                                </div>--%>
                              
                                <hr />

                                <div class="row form-group-md"> 
                                    <div class="col-sm-2 text-left"><label class="control-label" for="txtRcvDate">Payment received date:</label></div>                                  
                                    <%--<label class="col-sm-3 text-left" for="txtLastName">BQHS approval date:</label>--%>
                                    <div class="col-sm-2">
                                        <div class='input-group date' id='dtpRcvdDate'>
                                            <span class="input-group-addon">
                                                <span class="glyphicon glyphicon-calendar"></span>
                                            </span>
                                            <asp:TextBox ID="txtRcvDate" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-sm-2"></div>
                                    <label class="col-sm-3 control-label" for="txtPaymentRcvd">Total payment received:</label>
                                    <div class="col-sm-2">
                                        <input class="form-control" type="text" name="txtPaymentRcvd" id="txtPaymentRcvd" runat="server" onclick="calcTotal(this)"/>
                                    </div> 
                                    
                                </div>
                                <br />
                                <%--<div class="panel panel-default">--%>
                                    <%--<div class="panel-heading">Invoice</div>--%>
                                    
                                <%--</div>--%>
                                <br />
                                <div class="row">
                                    <div class="col-sm-1 text-left"><label class="control-label" for="txtComments">Comments:</label></div> 
                                    <div class="col-sm-11">
                                        <textarea class="form-control noresize" rows="3" name="txtComments" id="txtComments" runat="Server"></textarea>
                                    </div>
                                </div>
                                <br />
                                <div>
                                    <label class="control-label" for="txtComments">Uploaded file:</label>
                                    <asp:LinkButton ID="lnkFile" runat="server" OnClick = "DownloadFile"></asp:LinkButton>
                                </div>
                            </div>                            

                    </ContentTemplate>
                        <Triggers>                            
                            <asp:AsyncPostBackTrigger ControlID="rptInvoice" EventName="ItemCommand" />
                            <asp:PostBackTrigger ControlID="btnExportExcel" />
                            <%--<asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />--%>
                            <%--<asp:AsyncPostBackTrigger ControlID="gvInvoiceItem" EventName="SelectedIndexChanged" />--%>  
                            <%--<asp:AsyncPostBackTrigger ControlID="btnAddInvoiceItem" />--%> 
                            <asp:PostBackTrigger ControlID="btnSave" />                         
                            <asp:PostBackTrigger ControlID="lnkFile" />
                            <asp:AsyncPostBackTrigger ControlID="ddlCollab" />
                        </Triggers>
                    </asp:UpdatePanel>                       

                    <div class="modal-body">
                        <div class="fileinput fileinput-new" data-provides="fileinput">
                            <span class="btn btn-default btn-file"><span>Upload invoice</span>
                                <input type="file" class="file" id="fileUpload" name="fileUpload" runat="server" />
                            </span>
                            
                        </div>
                       <%-- &nbsp;--%>
                                        
                    </div>

                    <div class="modal-footer">
                                <asp:Label ID="lblResult" Visible="false" runat="server"></asp:Label>
                                <asp:Button ID="btnSave" runat="server" Text="Update" CssClass="btn btn-info" OnClientClick="return validateControl();" OnClick="btnSave_Click" />
                                <button class="btn btn-info" data-dismiss="modal" aria-hidden="true" onclick="clearQueryString()">Close</button>
                            </div>

                     


                </div>
            </div>
        </div>


    </div>

    <script type="text/javascript">
        function pageLoad(sender, args) {

            $('#invoice').DataTable({
                "stateSave": true,
                "searching": false,
                //"fixedHeader": true,
                //"columnDefs": [
                //            { "type": "dom-text", targets: 0}
                //            ],
                "columnDefs": [
                        { type: 'natural', targets: 0 }
                ],
                "aoColumns": [
                          null,
                          null,
                          null,
                          null,
                          null,
                          null,
                          { "bSortable": false }
                ]                
            });

            var invoiceDate = new DatePicker('dtpInvoiceDate');
            invoiceDate.init();
            //requestDate.onchange();

            var startDate = new DatePicker('dtpStartDate');
            startDate.init();
            //bqhsApprovalDate.onchange();

            var endDate = new DatePicker('dtpEndDate');
            endDate.init();
            //clientApprovalDate.onchange();

            var rcvdDate = new DatePicker('dtpRcvdDate');
            rcvdDate.init();

            $("#editModal").scroll(function () {
                $('#dtpInvoiceDate').datepicker('place');
                $('#startDate').datepicker('place');
                $('#dtpEndDate').datepicker('place');
                $('#dtpRcvdDate').datepicker('place');
            });

            //if ($('#MainContent_txtId').val().length > 0) {
            //    $('#MainContent_btnFetchInvoice').hide();
            //}
            //else 
            //    $('#MainContent_btnFetchInvoice').show();


            //$('#MainContent_gvInvoiceItem tr').not(":first").not(":last").each(function (i, row) {
            //    var $row = $(row),
            //        $agmt = $row.find('select[name*="ddlAgreement"]');

            //    $($agmt).change(function () {
            //        var agmtId = $($agmt).val();

            //        if (agmtId > 0) {
            //            var $agmtDate = $row.find('input[name$="txtAgreementDate"]'),
            //                $txtRate = $row.find('input[name$="txtRate"]'),
            //                $txtCompletedHours = $row.find('input[name$="txtCompletedHours"]'),
            //                $totalCost = $row.find('input[name$="txtTotalCost"]');

            //            $($agmtDate).val(1);
            //            $($txtRate).val(2);
            //            $($txtCompletedHours).val(3);
            //            $($totalCost).val(4);

            //            var $rowNew = $row.clone();
            //            $rowNew.insertBefore($("#MainContent_gvInvoiceItem tr:last-child"));
            //        }
            //    });
                
                    
                
            //});

            //calcSubTotal($('input[name$="txtSubTotal"]'));
            bindNewInvoice();

            $(function () {
                $(':text').bind('keydown', function (e) {
                    if (e.keyCode == 13) { //if this is enter key
                        e.preventDefault();
                        return false;
                    }
                    else
                        return true;
                });
            });
           
        }

        $(document).ready(function () {
            $('#li_super').addClass('selected');
            $('#li_payment').addClass('selected');
            $('#li_invoiceform').addClass('selected');

            $('#editModal').on('shown.bs.modal', function () {
                $('#editModal').scrollTop(5);
            });

            var ccAbbrv = GetURLParameter('CCName');

            if (typeof(ccAbbrv)!='undefined' && ccAbbrv.length > 0) {
                $("#editModal").modal('show');
                $("#MainContent_txtCCAbbrv").val(ccAbbrv);

                //bindPI(projectId);
                //document.location = window.location.href.split('?')[0];

                //clearQueryString();

                bindInvoiceId(ccAbbrv);
            }
            
        });

        /// NAME: ShowPopup()
        ///
        /// FUNCTION: Opens the pop-up modal from the front-end.
        /// 
        /// PARAMETERS: None.
        function ShowPopup() {
            $('#editModal').modal("show");
        }

        /// NAME: clearQueryString()
        ///
        /// FUNCTION: Clears the query string of URL when calling function
        ///           to remove any residual linking after closing the document
        ///           the linking is referring to.
        /// 
        /// PARAMETERS: None.
        function clearQueryString() {            
            if (window.location.href.indexOf('?') > 0)
                window.location.href = window.location.href.split('?')[0];
                //window.history.pushState("object or string", "Title", "/" + window.location.href.substring(window.location.href.lastIndexOf('/') + 1).split("?")[0]);
        }

        function bindNewInvoice()
        {
            var subtotal = 0.0,
                phdtotal = 0.0,
                mstotal = 0.0,
                discount = 0.0,
                afterbill = 0.0,
                discounttotal = 0.0,
                grandtotal = 0.0;
                        
            $('#newInvoice tr').each(function (i, row) {
                var $row = $(row),
                    //$agmt = $row.find('select[name*="ddlAgreement"]'),
                    $txtRate = $row.find('input[name$="txtRate"]'),
                    $txtToBeBilled = $row.find('input[name$="txtToBeBilled"]'),
                    $totalCost = $row.find('input[name$="txtSubTotal2"]'),
                    $discount = $row.find('input[name$="lblDiscountHr"]'),
                    $afterBill = $row.find('input[name$="lblRemainAfterBill"]'),
                    $spanType = $row.find('span[name$="lblType"]'),
                    $timeTrack = $row.find('span[name$="lblTimeTracking"]'),
                    $remainHr =  $row.find('span[name$="lblRemainingHr"]');
               
                if ($txtRate.val() > 0)
                {
                    var t = $txtRate.val() * $txtToBeBilled.val();
                    $totalCost.val(t.toFixed(2));
                    subtotal += t;

                    if ($spanType.text() == 'phd') {
                        phdtotal +=  parseFloat($timeTrack.text());
                    }
                    else {
                        mstotal +=  parseFloat($timeTrack.text());
                    }

                    var d = parseFloat($timeTrack.text()) - $txtToBeBilled.val();
                    if (d > 0.0){
                        $discount.val(d);
                        discounttotal += d * $txtRate.val();
                        
                    }

                    if (parseFloat($remainHr.text()) > parseFloat($txtToBeBilled.val())) {
                        afterbill = parseFloat($remainHr.text()) - parseFloat($txtToBeBilled.val());
                    }
                    else
                        afterbill = 0.0;

                    $afterBill.val(afterbill);

                }
            });
            
            $("#MainContent_rptNewInvoice_txtSubTotalNew").val((subtotal + discounttotal).toFixed(2));  //val(subtotal.toFixed(2));
            $("#MainContent_rptNewInvoice_txtPhdTotal").val(phdtotal.toFixed(2));
            $("#MainContent_rptNewInvoice_txtMsTotal").val(mstotal.toFixed(2));
            $("#MainContent_rptNewInvoice_txtDiscountNew").val(discounttotal.toFixed(2));
            $("#MainContent_rptNewInvoice_txtGrandTotalNew").val(subtotal.toFixed(2));  //val((subtotal + discounttotal).toFixed(2));
        }

        /// NAME: bindInvoiceId(ccAbbrv)
        ///
        /// FUNCTION: Adds the newly created Agreement ID number corresponding 
        ///           to a Collaborative Center into a new instance of the 
        ///           Invoice form for sequential ordering.
        /// 
        /// PARAMETERS: ccAbbrv - The given Collaborative Center abbreviation.
        function bindInvoiceId(ccAbbrv) {
            var uri = getBaseUrl() + '../api/Project/GetInvoiceId/?ccAbbrv=' + ccAbbrv;

            $.getJSON(uri).done(function (data) {

                // If data is a number AND > 10,
                // Adds a "0" in front of the number. 
                if ($.isNumeric(data)) {
                    if (data < 10) {
                        data = 0 + "" + data;
                    }
                }

                $("#MainContent_txtInvoiceId").val('I-' + ccAbbrv + '-' + data);
            });
        }

        function calcSubTotal2(thisTxtBox) {
            
            var $tobebilled = $(thisTxtBox).closest('td').siblings().find('input[name$="txtToBeBilled"]');
            var $rate = $(thisTxtBox).closest('td').siblings().find('input[name$="txtRate"]');
            
            $(thisTxtBox).val(($tobebilled.val() * $rate.val()).toFixed(2));
        }

        function calcTotal(thisTxtBox) {
            var subtotal = 0.00;

            $('#newInvoice tr').each(function (i, row) {
                var $row = $(row),
                    //$agmt = $row.find('select[name*="ddlAgreement"]'),
                    $txtRate = $row.find('input[name$="txtRate"]'),
                    $txtCompletedHours = $row.find('input[name$="txtToBeBilled"]'),
                    $totalCost = $row.find('input[name$="txtSubTotal2"]');  

                if ($txtRate.val() > 0 && $txtCompletedHours.val() > 0)
                {
                    var t = $txtRate.val() * $txtCompletedHours.val();
                    $totalCost.val(t.toFixed(2));
                    subtotal += t;
                }
            });

            $(thisTxtBox).val(subtotal.toFixed(2));

        }

        function calcSubTotal(thisTxtBox) {
            //loop thorugh table, calc total
            var subtotal = 0.00;
            var rows = {};
            var agmts = [];
            rows.agmts = agmts;

            $('#MainContent_gvInvoiceItem tr').each(function (i, row) {
                var $row = $(row),
                    $agmt = $row.find('select[name*="ddlAgreement"]'),
                    $txtRate = $row.find('input[name$="txtRate"]'),
                    $txtCompletedHours = $row.find('input[name$="txtCompletedHours"]'),
                    $totalCost = $row.find('input[name$="txtTotalCost"]');  

                if ($txtRate.val() > 0 && $txtCompletedHours.val() > 0)
                {
                    var t = $txtRate.val() * $txtCompletedHours.val();
                    $totalCost.val(t.toFixed(2));
                    subtotal += t;
                    //alert($agmt.children('option').filter(":selected").text());
                    var agmt = {
                        "agmtId": $agmt.children('option').filter(":selected").text(),
                        "cost": t
                    };

                    rows.agmts.push(agmt);
                }
            });

            $(thisTxtBox).val(subtotal.toFixed(2));

            if (subtotal > 0) {
                bindtblAgmet(JSON.stringify(rows));
            }
        }

        function bindtblAgmet(data) {
            var obj = JSON.parse(data);
            
            var newObj = {};
            for (i in obj['agmts']) {
                var item = obj['agmts'][i];
               
                if (newObj[item.agmtId] === undefined) {
                    newObj[item.agmtId] = 0;
                }
                newObj[item.agmtId] += item.cost;
                
            }

            var newData = {};
            newData.agmts = [];
            for (i in newObj) {
                newData.agmts.push({'agmtId':i, 'cost':newObj[i]});
            }

            $("[id*=tblAgmt] tr").not(":first").remove();
            for (var j = 0; j < newData.agmts.length; j++) {
                var agmtId = newData.agmts[j].agmtId;
                var cost = newData.agmts[j].cost;
                var newRowStr = "<tr><td></td><td></td></tr>"
                var rowNew = $(newRowStr);
                rowNew.children().eq(0).text(agmtId);
                rowNew.children().eq(1).text(cost.toFixed(2));
                rowNew.insertAfter($("[id*=tblAgmt] tr:last-child"));
            }

            //var newRowStr = "<tr><td>test</td><td>111</td></tr>"
            //var rowNew = $(newRowStr);
            //rowNew.insertAfter($("[id*=tblAgmt] tr:last-child"));

            //var firstRow = "$('[id*=MainContent_GridViewProject] tr:first-child')";
            //for (var i = 0; i < data.length; i++) {
            //    var projectId = data[i].Id;
            //    var funcName = 'GoToProjectForm(' + projectId + ')';
            //    var newRowStr = "<tr><td></td><td></td><td></td><td></td><td></td><td><input type='Button' class='btn btn-info' Value='Edit' onclick=" + funcName + "></td></tr>"
            //    var rowNew = $(newRowStr);
            //    rowNew.children().eq(0).text(data[i].Id);
            //    rowNew.children().eq(1).text(data[i].Title);
            //    rowNew.children().eq(2).text(data[i].PIFirstName);
            //    rowNew.children().eq(3).text(data[i].PILastName);
            //    rowNew.children().eq(4).text(data[i].InitialDate);
            //    rowNew.insertBefore($("[id*=MainContent_GridViewProject] tr:last-child"));
            //}
        }

        function calcGrandTotal(txtBox) {
            var subtotal = $('input[name$="txtSubTotal"]').val(),
                discount = $('input[name$="txtDiscount"]').val();
            
            if (subtotal > 0 && discount >= 0) {
                $('input[name$="txtGrandTotal"]').val((subtotal - discount).toFixed(2));
            }
            
        }

        function GetURLParameter(sParam) {
            var sPageURL = window.location.search.substring(1);
            var sURLVariables = sPageURL.split('&');
            for (var i = 0; i < sURLVariables.length; i++) {
                var sParameterName = sURLVariables[i].split('=');
                if (sParameterName[0] == sParam) {
                    return sParameterName[1];
                }
            }
        }

        function getBaseUrl() {
            var re = new RegExp(/^.*\//);
            return re.exec(window.location.href);
        }

        var DatePicker = function (ctrlId) {
            var ctl = ctrlId;

            return {
                init: function (e) {
                    $('#' + ctl).datepicker({
                        todayHighlight: true,
                        format: "mm/dd/yyyy",
                        autoclose: true,
                        orientation: "top"
                    })
                }
                , onchange: function (e) {
                    $('#' + ctl).on('changeDate', function (e) {
                        //revalidate
                        if ($('#' + ctl).validate()) {
                            $('#' + ctl).closest('.row').removeClass('has-error');
                        }
                    });
                }
            }
        }

        function validateControl(e) {
            var validator = $("#commentForm").validate({
                //only works with js in page
                rules: {                    
                    <%=txtInvoiceDate.UniqueID %>: {
                        required: true,
                        date: true
                    },
                    <%=txtStartDate.UniqueID %>: {
                        required: true,
                        date: true
                    },
                    <%=txtEndDate.UniqueID %>: {
                        required: true,
                        date: true
                    },
                    <%=txtInvoiceId.UniqueID %>: {
                        required: true,
                        remote: function () {
                            return {
                                async: false,
                                url: "InvoiceForm.aspx/IsInvoiceIdValid",
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                //data: JSON.stringify({ invoiceId: $('#MainContent_txtInvoiceId').val() }),
                                data: '{"invoiceId":"' + $('#MainContent_txtInvoiceId').val() + '","id":"' + $('#MainContent_txtId').val() + '"}',
                                dataFilter: function (rtndata) {
                                    var msg = JSON.parse(rtndata);
                                    if (msg.hasOwnProperty('d'))
                                        return msg.d;
                                    else
                                        return msg;
                                }
                            }
                        }
                    }
                },
                messages: {
                    <%=txtInvoiceDate.UniqueID %>: {
                        required: "Invoice data is required."
                    },
                    <%=txtStartDate.UniqueID %>: {
                        required: "Start data is required."
                    },
                    <%=txtEndDate.UniqueID %>: {
                        required: "End data is required."
                    },
                    <%=txtInvoiceId.UniqueID %>: {
                        remote: "Invoice id exists!"
                    }
                }
                //,highlight: function (element) {
                //    $(element).closest('.row').addClass('has-error');
                //}
                //,unhighlight: function (element) {
                //    $(element).closest('.row').removeClass('has-error');
                //}

            });

            var isValid = $("#commentForm").valid();

            if (validator.errorList.length > 0) {
                var firstElement = validator.errorList[0].element;
                firstElement.focus();
            }

        return isValid;

        e.preventDefault();
        }

        $.validator.setDefaults({
            errorElement: "span",
            errorClass: "help-block",
            highlight: function (element, errorClass, validClass) {
                $(element).closest('.form-group').addClass('has-error');
            },
            unhighlight: function (element, errorClass, validClass) {
                $(element).closest('.form-group').removeClass('has-error');
            },
            errorPlacement: function (error, element) {
                if (element.parent('.input-group').length || element.prop('type') === 'checkbox' || element.prop('type') === 'radio') {
                    error.insertAfter(element.parent());
                } else {
                    error.insertAfter(element);
                }
            }
        });

        (function() {
            function naturalSort (a, b, html) {
                var re = /(^-?[0-9]+(\.?[0-9]*)[df]?e?[0-9]?$|^0x[0-9a-f]+$|[0-9]+)/gi,
                    sre = /(^[ ]*|[ ]*$)/g,
                    dre = /(^([\w ]+,?[\w ]+)?[\w ]+,?[\w ]+\d+:\d+(:\d+)?[\w ]?|^\d{1,4}[\/\-]\d{1,4}[\/\-]\d{1,4}|^\w+, \w+ \d+, \d{4})/,
                    hre = /^0x[0-9a-f]+$/i,
                    ore = /^0/,
                    htmre = /(<([^>]+)>)/ig,
                    // convert all to strings and trim()
                    x = a.toString().replace(sre, '') || '',
                    y = b.toString().replace(sre, '') || '';
                // remove html from strings if desired
                if (!html) {
                    x = x.replace(htmre, '');
                    y = y.replace(htmre, '');
                }
                // chunk/tokenize
                var xN = x.replace(re, '\0$1\0').replace(/\0$/,'').replace(/^\0/,'').split('\0'),
                    yN = y.replace(re, '\0$1\0').replace(/\0$/,'').replace(/^\0/,'').split('\0'),
                    // numeric, hex or date detection
                    xD = parseInt(x.match(hre), 10) || (xN.length !== 1 && x.match(dre) && Date.parse(x)),
                    yD = parseInt(y.match(hre), 10) || xD && y.match(dre) && Date.parse(y) || null;
 
                // first try and sort Hex codes or Dates
                if (yD) {
                    if ( xD < yD ) {
                        return -1;
                    }
                    else if ( xD > yD ) {
                        return 1;
                    }
                }
 
                // natural sorting through split numeric strings and default strings
                for(var cLoc=0, numS=Math.max(xN.length, yN.length); cLoc < numS; cLoc++) {
                    // find floats not starting with '0', string or 0 if not defined (Clint Priest)
                    var oFxNcL = !(xN[cLoc] || '').match(ore) && parseFloat(xN[cLoc], 10) || xN[cLoc] || 0;
                    var oFyNcL = !(yN[cLoc] || '').match(ore) && parseFloat(yN[cLoc], 10) || yN[cLoc] || 0;
                    // handle numeric vs string comparison - number < string - (Kyle Adams)
                    if (isNaN(oFxNcL) !== isNaN(oFyNcL)) {
                        return (isNaN(oFxNcL)) ? 1 : -1;
                    }
                        // rely on string comparison if different types - i.e. '02' < 2 != '02' < '2'
                    else if (typeof oFxNcL !== typeof oFyNcL) {
                        oFxNcL += '';
                        oFyNcL += '';
                    }
                    if (oFxNcL < oFyNcL) {
                        return -1;
                    }
                    if (oFxNcL > oFyNcL) {
                        return 1;
                    }
                }
                return 0;
            }
 
            jQuery.extend( jQuery.fn.dataTableExt.oSort, {
                "natural-asc": function ( a, b ) {
                    return naturalSort(a,b,true);
                },
 
                "natural-desc": function ( a, b ) {
                    return naturalSort(a,b,true) * -1;
                },
 
                "natural-nohtml-asc": function( a, b ) {
                    return naturalSort(a,b,false);
                },
 
                "natural-nohtml-desc": function( a, b ) {
                    return naturalSort(a,b,false) * -1;
                }
            } );
 
        }());

    </script>
</asp:Content>
