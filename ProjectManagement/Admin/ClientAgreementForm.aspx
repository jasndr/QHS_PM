<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ClientAgreementForm.aspx.cs" Inherits="ProjectManagement.Admin.ClientAgreementForm" %>

<%@ Register Assembly="DropDownChosen" Namespace="CustomDropDown" TagPrefix="ucc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Content/fileinput.css" rel="stylesheet" />
    <script src="../Scripts/bootstrap-datepicker.min.js"></script>
    <script src="../Scripts/jquery.validate.min.js"></script> 
    <script src="../Scripts/fileinput.js"></script>   
    <script src="<%=Page.ResolveUrl("~/Scripts/jquery.dataTables.min.js")%>"></script>
    <script src="<%=Page.ResolveUrl("~/Scripts/dataTables.bootstrap.min.js")%>"></script>
    <script src="../Scripts/chosen.jquery.js"></script>
    <link href="../Content/dataTables.bootstrap.min.css" rel="stylesheet" />
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
       #projectsSelect .chosen-container .chosen-drop {
           width: 200% !important;
       }
       #projectsSelect .chosen-single {
           width: 100% !important;
       }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="divClientAgreement">
        <div class="panel panel-default">
            <div class="panel-heading"><b>Client Agreement</b></div>
            <div class="panel-body">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>

                        <%--<div>
                            <asp:Button ID="btnAdd" runat="server" Text="Add a new agreement" CssClass="btn btn-info" OnClick="btnAdd_Click" />
                        </div>--%>
                        <%--<br />--%>
                        <div class="row">
                            <div class="col-sm-1">
                                <label class="control-label">Collab Center:</label>
                            </div>
                            <div class="col-sm-5">
                                <asp:DropDownList ID="ddlCollab" runat="server" CssClass="form-control"  OnSelectedIndexChanged="ddlCollab_Changed" AutoPostBack="True">
                                </asp:DropDownList>
                            </div>
                            <div class="col-sm-5">
                                <asp:Button ID="btnExportExcel" runat="server" Text="Export" CssClass="btn btn-info" OnClick="btnExportExcel_Click" />
                                <asp:TextBox ID="hdnSortOrder" runat="server" CssClass="hidden" />
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-12">
                                <table class="table table-striped table-hover table-bordered" id="agreement">
                                    <thead>
                                        <tr>
                                            <th class="hidden">Id</th>
                                            <th class="col-sm-1">Id</th>
                                            <th class="hidden">PI Name</th>
                                           <%-- <th style="width: 10%; padding: 0,0,0,1px;">Collab Ctr</th>--%>
                                            <th>Project Id</th>
                                            <th>Phase</th>
                                            <th>Project title</th>
                                            <%--<th style="width: 5%; padding: 0,0,0,1px;">Phd Rate</th>--%>
                                            <th class="hidden">Client Refnum</th>
                                            <th class="hidden">Phd Rate</th>
                                            <th>Approved Phd Hour</th>
                                            <th class="hidden">Invoiced Phd Hour</th>
                                            <th>Remaining Phd Hour</th>
                                            <th class="hidden">Ms Rate</th>
                                            <th>Approved Ms Hour</th>
                                            <th class="hidden">Invoiced Ms Hour</th>
                                            <th>Remaining Ms Hour</th>
                                            <th class="hidden">Request Date</th>
                                            <th class="hidden">Approval Date</th>
                                            <th class="hidden">Client Approval Date</th>
                                            <th class="hidden">Completion Date</th>
                                            <th>Completion Status</th>
                                            <th>Edit</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="rptClientAgmt" runat="server" OnItemCommand="rptClientAgmt_ItemCommand">
                                            <ItemTemplate>
                                                <tr>
                                                    <td class="hidden"><asp:Label ID="lblId" name="lblId" runat="server" Text='<%#Eval("Id")%>' /></td>
                                                    <td><asp:Label ID="lblAgmtId" runat="server" Text='<%#Eval("AgmtId")%>' /></td>
                                                    <td class="hidden"><asp:Label ID="lblPIName" runat="server" Text='<%#Eval("PIName")%>' /></td>
                                                    <td><asp:Label ID="lblProjectId" runat="server" Text='<%#Eval("ProjectId")%>' /></td>
                                                    <td><asp:Label ID="lblProjectPhase" runat="server" Text='<%#Eval("ProjectPhase")%>' /></td>
                                                    <td><asp:Label ID="lblProjectTitle" runat="server" Text='<%#Eval("ProjectTitle")%>' /></td>
                                                    <td class="hidden"><asp:Label ID="lblClientRefNum" runat="server" Text='<%#Eval("ClientRefNum")%>' /></td>
                                                    <td class="hidden"><asp:Label ID="lblApprovedPhdRate" runat="server" Text='<%#Eval("ApprovedPhdRate")%>' /></td>
                                                    <td><asp:Label ID="lblApprovedPhdHr" runat="server" Text='<%#Eval("ApprovedPhdHr")%>' /></td>
                                                    <td class="hidden"><asp:Label ID="lblInvoicedPhdHr" runat="server" Text='<%#Eval("InvoicedPhdHr")%>' /></td>
                                                    <td><asp:Label ID="lblRemainingPhdHr" runat="server" Text='<%#Eval("RemainingPhdHr")%>' /></td>
                                                    <td class="hidden"><asp:Label ID="lblApprovedMsRate" runat="server" Text='<%#Eval("ApprovedMsRate")%>' /></td>
                                                    <td><asp:Label ID="lblApprovedMsHr" runat="server" Text='<%#Eval("ApprovedMsHr")%>' /></td>
                                                    <td class="hidden"><asp:Label ID="lblInvoicedMsHr" runat="server" Text='<%#Eval("InvoicedMsHr")%>' /></td>
                                                    <td><asp:Label ID="lblRemainingMsHr" runat="server" Text='<%#Eval("RemainingMsHr")%>' /></td>
                                                    <td class="hidden"><asp:Label ID="lblRequestDate" runat="server" Text='<%#Eval("RequestDate")%>' /></td>
                                                    <td class="hidden"><asp:Label ID="lblApprovalDate" runat="server" Text='<%#Eval("ApprovalDate")%>' /></td>
                                                    <td class="hidden"><asp:Label ID="lblClientApprovalDate" runat="server" Text='<%#Eval("ClientApprovalDate")%>' /></td>
                                                    <td class="hidden"><asp:Label ID="lblCompletionDate" runat="server" Text='<%#Eval("CompletionDate")%>' /></td>
                                                    <td><asp:Label ID="lblStatus" runat="server" Text='<%#Eval("Status")%>' /></td>
                                                    <td>
                                                        <asp:Button ID="btnView" runat="server" Text="Edit" CssClass="btn btn-info" CommandArgument='<%# Eval("Id") %>' />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnExportExcel" />
                    </Triggers>
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
                    
                    <%--<button type="button" class="close" data-dismiss="modal" aria-hidden="true">x</button>--%>
                       <%-- <br />--%>
                         <div class="panel panel-info">
                            <div class="panel-heading">              
                                <h4 class="text-center"><b>Client Agreement Form</b></h4>                                
                            </div>
                        </div>
                  
                    <asp:UpdatePanel ID="upEdit" runat="server">
                        <ContentTemplate>
                            <div class="modal-body">
                                <div class="row form-group-md">
                                    <div class="col-sm-4 text-left"><label class="control-label" for="txtCCAbbrv">Collaborative center abbreviation:</label></div>
                                    <div class="col-sm-2">
                                        <input class="form-control" type="text" name="txtCCAbbrv" id="txtCCAbbrv" runat="Server" disabled/>
                                    </div>
                                    <label class="col-sm-3 control-label" for="txtAgreementId">Agreement ID:</label>
                                    <div class="col-sm-3">
                                        <input class="form-control" type="text" name="txtAgmtId" id="txtAgmtId" runat="Server" />
                                        <input class="form-control hidden" type="text" name="txtId" id="txtId" runat="Server" />
                                    </div>
                                </div>
                                <br />
                                <div class="row form-group-md">
                                    <div class="col-sm-2 text-left"><label class="control-label" for="txtFirstName">Project ID:</label></div>
                                    <div class="col-sm-5">

                                        <%--<asp:DropDownList ID="ddlProject" runat="server" CssClass="form-control">
                                        </asp:DropDownList>--%>

                                        <div id="projectsSelect">
                                            <ucc:DropDownListChosen ID="ddlProject" runat="server" 
                                                CssClass="form-control" Width="100px"
                                                NoResultsText="No results match."
                                                DataPlaceHolder="Search Projects" AllowSingleDeselect="true">
                                            </ucc:DropDownListChosen>
                                        </div>

                                    </div>
                                    <label class="col-sm-1 control-label" for="txtPI">PI:</label>
                                    <div class="col-sm-4">
                                        <input class="form-control" type="text" name="txtPI" id="txtPI" runat="Server" disabled  />
                                    </div>
                                </div>
                                <br />
                                <div class="row form-group-md">
                                    <div class="col-sm-3 text-left"><label class="control-label" for="txtClientRefNum">Client reference ID:</label></div>
                                    <div class="col-sm-4">
                                        <input class="form-control" type="text" name="txtClientRefNum" id="txtClientRefNum" runat="server" />
                                    </div>
                                    <label class="col-sm-2 control-label" for="txtLastName">Request date:</label>
                                    <div class="col-sm-3">
                                        <div class='input-group date' id='dtpRequestDate'>
                                            <span class="input-group-addon">
                                                <span class="glyphicon glyphicon-calendar"></span>
                                            </span>
                                            <asp:TextBox ID="txtRequestDate" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <br />
                                <div class="hidden">
                                    <asp:DropDownList ID="ddlProjectPhaseHdn" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                                <div class="row form-group-md">
                                    <div class="col-sm-2 text-left"><label class="control-label" for="txtFirstName">Project phase:</label></div>
                                    <div class="col-sm-3">
                                        <asp:DropDownList ID="ddlProjectPhase" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                    <label class="col-sm-4 control-label" for="txtProjectType">Project Type:</label>
                                    <div class="col-sm-3">
                                        <input class="form-control" type="text" name="txtProjectType" id="txtProjectType" runat="Server" disabled  />
                                    </div>
                                </div>
                                <br />
                                <table class="table table-hover" >
                                    <thead>
                                        <tr>
                                            <th style="width: 25%; "></th>
                                            <th style="width: 25%; text-align: center;">Rate</th>
                                            <th style="width: 25%; text-align: center;">Hour</th>
                                            <th style="width: 25%; text-align: center;">Total</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td><label class="control-label">Approved PhD:</label></td>
                                            <td><input class="form-control rightAlign" type="text" name="txtPhdRate" id="txtPhdRate" runat="Server" onchange="calcTotal();"/></td>
                                            <td><input class="form-control rightAlign" type="text" name="txtPhdHour" id="txtPhdHour" runat="Server" onchange="calcTotal();"/></td>
                                            <td class='input-group'><span class="input-group-addon"><span class="glyphicon glyphicon-usd"></span></span><input class="form-control rightAlign" type="text" name="txtPhdTotal" id="txtPhdTotal" disabled/></td>
                                        </tr>
                                        <tr>
                                            <td><label class="control-label">Approved MS:</label></td>
                                            <td><input class="form-control rightAlign" type="text" name="txtMsRate" id="txtMsRate" runat="Server" onchange="calcTotal();"/></td>
                                            <td><input class="form-control rightAlign" type="text" name="txtMsHour" id="txtMsHour" runat="Server" onchange="calcTotal();"/></td>
                                            <td class='input-group'><span class="input-group-addon"><span class="glyphicon glyphicon-usd"></span></span><input class="form-control rightAlign" type="text" name="txtMsTotal" id="txtMsTotal" disabled/></td>
                                        </tr>
                                         <tr>
                                            <td></td>
                                            <td></td>
                                            <td><label class="control-label">Total fees approved:</label></td>
                                            <td class='input-group'><span class="input-group-addon"><span class="glyphicon glyphicon-usd"></span></span>
                                                <input class="form-control rightAlign" type="text" name="txtTotalFee" id="txtTotalFee" disabled /></td>
                                        </tr>

                                        <%--<asp:Repeater ID="Repeater1" runat="server" >
                                            <ItemTemplate>
                                                <tr>
                                                    <td><%# Eval("Id") %></td>
                                                    <td><%# Eval("TaskValue") %></td>
                                                    <td><%# Eval("Creator") %></td>
                                                    <td><%# Eval("CreateDate") %></td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>--%>
                                    </tbody>
                                </table>
                                <br />
                                <div class="row form-group-md"> 
                                    <div class="col-sm-3 text-left"><label class="control-label" for="txtFirstName">BQHS approval date:</label></div>                                  
                                    <%--<label class="col-sm-3 text-left" for="txtLastName">BQHS approval date:</label>--%>
                                    <div class="col-sm-3">
                                        <div class='input-group date' id='dtpBqhsApprovalDate'>
                                            <span class="input-group-addon">
                                                <span class="glyphicon glyphicon-calendar"></span>
                                            </span>
                                            <asp:TextBox ID="txtBqhsApprovalDate" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-sm-3 text-right"><label class="control-label" for="txtFirstName">Client approval date:</label></div> 
                                    <div class="col-sm-3">
                                        <div class='input-group date' id='dtpClientApprovalDate'>
                                            <span class="input-group-addon">
                                                <span class="glyphicon glyphicon-calendar"></span>
                                            </span>
                                            <asp:TextBox ID="txtClientApprovalDate" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <br />
                                <div class="row form-group-md"> 
                                    <div class="col-sm-3 text-left"><label class="control-label" for="txtFirstName">Completion date:</label></div>
                                    <div class="col-sm-3">
                                        <div class='input-group date' id='dtpCompletionDate'>
                                            <span class="input-group-addon">
                                                <span class="glyphicon glyphicon-calendar"></span>
                                            </span>
                                            <asp:TextBox ID="txtCompletionDate" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-sm-3 text-right"><label class="control-label" for="ddlStatus">Completion status:</label></div> 
                                    <div class="col-sm-3">
                                        <select class="form-control" id="ddlStatus" runat="server">
                                            <option value=""></option>
                                            <option value="Active">Active</option>
                                            <option value="Inactive">Inactive</option>
                                            <option value="Complete">Complete</option>
                                        </select>
                                    </div>
                                </div>
                                <br />
                                <div class="panel panel-default">
                                    <div class="panel-heading">Agreement</div>
                                    <div class="panel-body">
                                        <div class="fileinput fileinput-new" data-provides="fileinput">
                                            <span class="btn btn-default btn-file"><span>Upload agreement</span>
                                                <input type="file" class="file" id="fileUpload" name="fileUpload" runat="server"/>
                                            </span>                                    
                                        </div>&nbsp;
                                        <div>Uploaded file:
                                            <asp:LinkButton ID="lnkFile" runat="server" OnClick = "DownloadFile"></asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-sm-2 text-left"><label class="control-label" for="txtComments">Comments:</label></div> 
                                    <div class="col-sm-9">
                                        <textarea class="form-control noresize" rows="5" name="txtComments" id="txtComments" runat="Server"></textarea>
                                    </div>
                                </div>
                                
                            </div>

                            <div class="modal-footer">
                                <asp:Label ID="lblResult" Visible="false" runat="server"></asp:Label>
                                <asp:Button ID="btnSave" runat="server" Text="Update" CssClass="btn btn-info" OnClientClick="return validateControl();" OnClick="btnSave_Click" />
                                <button class="btn btn-info" data-dismiss="modal" aria-hidden="true" onclick="cleartQueryString()">Close</button>
                            </div>

                        </ContentTemplate>
                        <Triggers>
                            <%--<asp:AsyncPostBackTrigger ControlID="rptClientAgmt" EventName="ItemCommand" />--%>
                            <%--<asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />--%>
                            <asp:PostBackTrigger ControlID="btnSave" />
                            <asp:PostBackTrigger ControlID="lnkFile" />
                        </Triggers>
                    </asp:UpdatePanel>



                </div>
            </div>
        </div>


    </div>

    <script type="text/javascript">
        function pageLoad(sender, args) {          
            var projectId = $("#MainContent_ddlProject").val();
            //$("#MainContent_txtClientRefNum").val(projectId);
            $('#MainContent_ddlProjectPhase').children('option').show();
            bindPI(projectId);
            bindPhase(projectId);

            $("#MainContent_ddlProject").change(function () {
                var _projectId = $("#MainContent_ddlProject").val();               
                bindPI(_projectId);

                $('#MainContent_ddlProjectPhase option:selected').removeAttr("selected");
                $('#MainContent_ddlProjectPhase').val();
                bindPhase(_projectId);
            });

            
            $("#MainContent_ddlProjectPhase").change(function () {                
                var phaseName = $("#MainContent_ddlProjectPhase option:selected").text();

                if (phaseName == '') {
                    $("#MainContent_txtPhdHour").val(0);
                    $("#MainContent_txtMsHour").val(0);
                }
                else {
                    jQuery.map(phaseJson, function (obj) {
                        if (obj.Name == phaseName) {
                            $("#MainContent_txtPhdHour").val(obj.PhdHrs);
                            $("#MainContent_txtMsHour").val(obj.MsHrs);                            
                        }
                    });
                }

                calcTotal();
            });

            var requestDate = new DatePicker('dtpRequestDate');
            requestDate.init();
            //requestDate.onchange();

            var bqhsApprovalDate = new DatePicker('dtpBqhsApprovalDate');
            bqhsApprovalDate.init();
            //bqhsApprovalDate.onchange();

            var clientApprovalDate = new DatePicker('dtpClientApprovalDate');
            clientApprovalDate.init();
            //clientApprovalDate.onchange();

            var completionDate = new DatePicker('dtpCompletionDate');
            completionDate.init();

            $("#editModal").scroll(function () {
                $('#dtpRequestDate').datepicker('place');
                $('#dtpBqhsApprovalDate').datepicker('place');
                $('#dtpClientApprovalDate').datepicker('place');
            });

            calcTotal();

            var table = $('#agreement').DataTable({
                "stateSave": true,
                "searching": false,
                "order": [[0, "asc"]],
                "aoColumns": [
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    { "bSortable": false } 
                ],
                columnDefs: [
                    { type: 'natural', targets: 0 }
                ],
                buttons: [
                    'excel'
                ]
            });
 
            $('#agreement').on( 'order.dt', function () {
                // This will show: "Ordering on column 1 (asc)", for example
                //var order = table.order();
                //var objCols = table.columns(order[0][0]);

                $('#MainContent_hdnSortOrder').val(table.order());
                //$('#orderInfo').html( 'Ordering on column '+order[0][0]+' ('+order[0][1]+')' );

                //var orderId = '';
                //$('#agreement tr').not(":first").each(function (i, row) {
                //    var $row = $(row),
                //        $id = $row.find('span[name$="lblId"]');

                //    orderId += $id.text() + ',';
                //});

               
            } );

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
            $('#li_clientagmtform').addClass('selected');

            //$('#MainContent_ddlProject').css("width", "100%");

            $('#editModal').on('shown.bs.modal', function () {
                $('#editModal').scrollTop(5);
            });

            var ccAbbrv = GetURLParameter('Name');            

            if (typeof(ccAbbrv)!='undefined' && ccAbbrv.length > 0) {
                $("#editModal").modal('show');
                $("#MainContent_txtCCAbbrv").val(ccAbbrv);              

                //bindPI(projectId);
                //document.location = window.location.href.split('?')[0];

                //clearQueryString();

                bindAgmtId(ccAbbrv);
            }
            
        });

        function bindAgmtId(ccAbbrv) {
            var uri = getBaseUrl() + '../api/Project/GetClientAgreementId/?ccAbbrv=' + ccAbbrv;

            $.getJSON(uri).done(function (data) {
                $("#MainContent_txtAgmtId").val('A-' + ccAbbrv + '-' + data);
            });
        }
        
        var phaseJson;
        function bindPhase(projectId) {
            if (projectId > 0) {
                //$('#MainContent_ddlProjectPhase option:selected').removeAttr("selected");
                $('#MainContent_ddlProjectPhase').children('option:not(:first)').hide();
                
                var uri = getBaseUrl() + '../api/Project/GetProjectPhase/?projectId=' + projectId;

                $.getJSON(uri).done(function (data) {
                    // On success
                    var obj = $.parseJSON(data);

                    phaseJson = obj;
                    //alert(obj.Name);
                    $.each(obj, function (index, element) {
                       
                        //$('#MainContent_ddlProjectPhase').append(new Option(element.Name, element.Name));
                        var phaseName = element.Name.split('-');
                        var phaseValue = phaseName[1];
                        
                        //$('#MainContent_ddlProjectPhase').children('option[value=phaseValue]').show();
                        $("#MainContent_ddlProjectPhase option[value=" + phaseValue + "]").show();
                    });
                });
            }
        }

        function bindPI(projectId) {
            if (projectId > 0) {
                var uri = getBaseUrl() + '../api/Project/GetProjectPIName/?projectId=' + projectId;
               
                $.getJSON(uri).done(function (data) {
                    // On success
                    var da = data.split('|');
                    $("#MainContent_txtPI").val(da[0]);
                    $("#MainContent_txtPI").prop('readonly', true);
                    $("#MainContent_txtProjectType").val(da[1]);
                    $("#MainContent_txtProjectType").prop('readonly', true);
                });
            }
            else {
                $("#MainContent_txtPI").prop('readonly', false);
            }
        }

        function calcTotal() {
            var phdRate = 0.0, phdHour = 0.0, msRate = 0.0, msHour = 0.0;
            var phdTotal = 0.0, msTotal = 0.0,
            phdRate = $("#MainContent_txtPhdRate").val(); 
            phdHour = $("#MainContent_txtPhdHour").val(); 

            if (phdRate > 0 && phdHour >= 0) {
                phdTotal = phdRate * phdHour;
                $("#txtPhdTotal").val(phdTotal.toFixed(2));
            }

            msRate = $("#MainContent_txtMsRate").val();
            msHour = $("#MainContent_txtMsHour").val();

            if (msRate > 0 && msHour >= 0) {
                msTotal = msRate * msHour;
                $("#txtMsTotal").val(msTotal.toFixed(2));
                //Math.round(num * 100) / 100
            }

            if (phdTotal >= 0 || msTotal >= 0)
            {
                $("#txtTotalFee").val((phdTotal + msTotal).toFixed(2));
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

        function validateControl() {
            //$("#MainContent_ddlProject").addClass("required");
            //$("#MainContent_txtRequestDate").addClass("required");

            var validator = $("#commentForm").validate({
                //only works with js in page
                rules: {                    
                    <%=txtRequestDate.UniqueID %>: {
                        required: true,
                        date: true
                    },
                    <%=ddlProject.UniqueID %>: {
                        required: true
                    },
                    <%=txtAgmtId.UniqueID %>: {
                        required: true,
                        remote: function () {
                            return {                                
                                async: false,
                                url: "ClientAgreementForm.aspx/IsAgmtIdValid",
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",                                   
                                data: '{"agmtId":"' + $('#MainContent_txtAgmtId').val() + '","id":"' + $('#MainContent_txtId').val() + '"}',
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
                    <%=txtRequestDate.UniqueID %>: {
                        required: "Request data is required."
                    },
                    <%=txtAgmtId.UniqueID %>: {
                        remote: "Agreement id exists!"
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
        }

        function cleartQueryString() {            
            if (window.location.href.indexOf('?') > 0)
                window.location.href = window.location.href.split('?')[0];
                //window.history.pushState("object or string", "Title", "/" + window.location.href.substring(window.location.href.lastIndexOf('/') + 1).split("?")[0]);
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
