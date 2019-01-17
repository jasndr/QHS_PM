<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CollaborativeCenterForm.aspx.cs" Inherits="ProjectManagement.Admin.CollaborativeCenterForm" %>

<%@ Register Assembly="DropDownChosen" Namespace="CustomDropDown" TagPrefix="ucc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/bootstrap-datepicker.min.js"></script>
    <script src="../Scripts/jquery.validate.min.js"></script>
    <script src="<%=Page.ResolveUrl("~/Scripts/jquery.dataTables.min.js")%>"></script>
    <script src="<%=Page.ResolveUrl("~/Scripts/dataTables.bootstrap.min.js")%>"></script>
    <script src="../Scripts/chosen.jquery.js"></script>
    <link href="../Content/dataTables.bootstrap.min.css" rel="stylesheet" />
    <link href="../Content/chosen.css" rel="stylesheet" />
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
    <div id="divCollabCenter">
        <div class="panel panel-default">
            <div class="panel-heading"><b>Collaborative Center</b></div>
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
                                <%--<asp:DropDownList ID="ddlCollab_old" runat="server" CssClass="form-control"  OnSelectedIndexChanged="ddlCollab_Changed" AutoPostBack="True">
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
                            <div class="col-md-12" >
                                <table class="table table-striped table-hover table-bordered dataTable no-footer" id="CollabCtr">
                                    <thead>
                                        <tr>
                                            <th style="width: 5%;">Id</th>
                                            <th style="width: 30%;">Name</th>
                                            <th style="width: 10%;">Name Abbrv</th>
                                            <th style="width: 10%;">Start Date</th>
                                            <th style="width: 10%;">End Date</th>
                                            <th style="width: 10%;">Billing Schedule</th>
                                            <th style="width: 15%;">Next schedule invoice date</th>
                                            <th style="width: 5%;"></th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="rptCC" runat="server" OnItemCommand="rptCC_ItemCommand">
                                            <ItemTemplate>
                                                <tr>
                                                    <td><%# Eval("Id") %></td>
                                                    <td><%# Eval("Name") %></td>
                                                    <td><%# Eval("NameAbbrv") %></td>
                                                    <td><%# Eval("StartDate") %></td>
                                                    <td><%# Eval("EndDate") %></td>
                                                    <td><%# Eval("BillingSchedule") %></td>
                                                    <td><%# Eval("NextInvoiceDate") %></td>
                                                    <td>
                                                        <asp:Button ID="btnView" runat="server" Text="Edit" CssClass="btn btn-info" CommandArgument='<%# Eval("Id") %>' /></td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <asp:Repeater ID="rptCC_inactive" runat="server" OnItemCommand="rptCC_ItemCommand">
                                            <ItemTemplate>
                                                <tr>
                                                    <td><%# Eval("Id") %></td>
                                                    <td><%# Eval("Name") %></td>
                                                    <td><%# Eval("NameAbbrv") %></td>
                                                    <td><%# Eval("StartDate") %></td>
                                                    <td><%# Eval("EndDate") %></td>
                                                    <td><%# Eval("BillingSchedule") %></td>
                                                    <td><%# Eval("NextInvoiceDate") %></td>
                                                    <td>
                                                        <asp:Button ID="btnView" runat="server" Text="Edit" CssClass="btn btn-info" CommandArgument='<%# Eval("Id") %>' /></td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>

                                    </tbody>
                                </table>
                            </div>
                        </div>

                        <div>
                            <asp:Button ID="btnAdd" runat="server" Text="Add New" CssClass="btn btn-info" OnClick="btnAdd_Click" />
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

                    <%--<button type="button" class="close" data-dismiss="modal" aria-hidden="true">x</button>--%>
                    <%--<br />--%>
                    <div class="panel panel-info">
                        <div class="panel-heading">
                            <h4 class="text-center"><b>COLLABORATIVE CENTER</b></h4>
                        </div>
                    </div>
                    <asp:UpdatePanel ID="upEdit" runat="server">
                        <ContentTemplate>
                            <div class="modal-body">
                                <div class="row">
                                    <div class="col-xs-11 col-md-10"></div>
                                    <div class="col-xs-1 col-md-2">
                                        <b>CC ID:
                                            <asp:Label ID="lblCCId" runat="server"></asp:Label></b>
                                    </div>
                                </div>
                                <br />
                                <div class="row form-group-md">
                                    <div class="col-sm-4 text-left">
                                        <label class="control-label" for="txtCCName">Collaborative Center name:</label>
                                    </div>
                                    <div class="col-sm-6">
                                        <input class="form-control" type="text" name="txtCCName" id="txtCCName" runat="Server" />
                                    </div>
                                </div>
                                <br />
                                <div class="row form-group-md">
                                    <div class="col-sm-4 text-left">
                                        <label class="control-label" for="txtCCAbbrv">Collaborative center abbreviation:</label>
                                    </div>
                                    <div class="col-sm-2">
                                        <input class="form-control" type="text" name="txtCCAbbrv" id="txtCCAbbrv" runat="Server" />
                                    </div>
                                </div>
                                <br />
                                <div class="row form-group-md">
                                    <div class="col-sm-3 text-left">
                                        <label class="control-label" for="txtStartDate">Collaboration start:</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <div class='input-group date' id='dtpStartDate'>
                                            <span class="input-group-addon">
                                                <span class="glyphicon glyphicon-calendar"></span>
                                            </span>
                                            <asp:TextBox ID="txtStartDate" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <label class="col-sm-3 control-label" for="txtEndDate">Collaboration end:</label>
                                    <div class="col-sm-3">
                                        <div class='input-group date' id='dtpEndDate'>
                                            <span class="input-group-addon">
                                                <span class="glyphicon glyphicon-calendar"></span>
                                            </span>
                                            <asp:TextBox ID="txtEndDate" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <br />
                                <div class="row form-group-md" id="collabCtrProjSxn" runat="server" visible="false">
                                    <div class="col-sm-4 text-left">
                                        <label class="control-label" for="ddlCollabCtrProjects">Projects affiliated with Collaborative Center:</label>
                                    </div>
                                    <div class="col-sm-6">
                                        <asp:DropDownList ID="ddlCollabCtrProjects" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlCollabCtrProjects_Changed" AutoPostBack="True">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <br />
                                <div class="row form-group-md" id="collabCtrClientAgmtSxn" runat="server" visible="false">
                                    <div class="col-sm-4 text-left">
                                        <label class="control-label" for="ddlCollabCtrClientAgmts">Client Agreements affiliated with Collaborative Center:</label>
                                    </div>
                                    <div class="col-sm-6">
                                        <asp:DropDownList ID="ddlCollabCtrClientAgmts" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlCollabCtrClientAgmts_Changed" AutoPostBack="True">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <br />

                                <h5>Collaboration Contact Information</h5>
                                <br />
                                <div class="row form-group-md">
                                    <div class="col-sm-1 text-left">
                                        <label class="control-label" for="txtName">Name:</label>
                                    </div>
                                    <div class="col-sm-6">
                                        <input class="form-control" type="text" name="txtName" id="txtName" runat="Server" />
                                    </div>
                                    <div class="col-sm-1 text-left">
                                        <label class="control-label" for="txtPhone">Phone:</label>
                                    </div>
                                    <div class="col-sm-4">
                                        <input class="form-control" type="text" name="txtPhone" id="txtPhone" runat="Server" />
                                    </div>
                                </div>
                                <br />
                                <div class="row form-group-md">
                                    <div class="col-sm-1 text-left">
                                        <label class="control-label" for="txtEmail">Email:</label>
                                    </div>
                                    <div class="col-sm-5">
                                        <input class="form-control" type="text" name="txtEmail" id="txtEmail" runat="Server" />
                                    </div>
                                </div>
                                <br />
                                <h5>Fiscal Contact Information</h5>
                                <br />
                                <div class="row form-group-md">
                                    <div class="col-sm-1 text-left">
                                        <label class="control-label" for="txtFiscalName">Name:</label>
                                    </div>
                                    <div class="col-sm-6">
                                        <input class="form-control" type="text" name="txtFiscalName" id="txtFiscalName" runat="Server" />
                                    </div>
                                    <div class="col-sm-1 text-left">
                                        <label class="control-label" for="txtFiscalPhone">Phone:</label>
                                    </div>
                                    <div class="col-sm-4">
                                        <input class="form-control" type="text" name="txtFiscalPhone" id="txtFiscalPhone" runat="Server" />
                                    </div>
                                </div>
                                <br />
                                <div class="row form-group-md">
                                    <div class="col-sm-1 text-left">
                                        <label class="control-label" for="txtFiscalEmail">Email:</label>
                                    </div>
                                    <div class="col-sm-5">
                                        <input class="form-control" type="text" name="txtFiscalEmail" id="txtFiscalEmail" runat="Server" />
                                    </div>
                                </div>
                                <br />
                                <div class="row form-group-md">
                                    <div class="col-sm-2 text-left">
                                        <label class="control-label" for="txtFiscalMail1">Mail address 1:</label>
                                    </div>
                                    <div class="col-sm-4">
                                        <input class="form-control" type="text" name="txtFiscalMail1" id="txtFiscalMail1" runat="Server" />
                                    </div>
                                    <div class="col-sm-3 text-right">
                                        <label class="control-label" for="txtFiscalMail2">Mail address 2 (Apt/Ste#):</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <input class="form-control" type="text" name="txtFiscalMail2" id="txtFiscalMail2" runat="Server" />
                                    </div>
                                </div>
                                <br />
                                <div class="row form-group-md">
                                    <div class="col-sm-1 text-left">
                                        <label class="control-label" for="txtFiscalCity">City:</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <input class="form-control" type="text" name="txtFiscalCity" id="txtFiscalCity" runat="Server" />
                                    </div>
                                    <div class="col-sm-1 text-left">
                                        <label class="control-label" for="txtFiscalState">State:</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <%--<input class="form-control" type="text" name="txtFiscalState" id="txtFiscalState"  />--%>
                                        <select class="form-control" name="ddlFiscalState" id="ddlFiscalState" runat="Server">
                                            <option value=""></option>
                                            <option value="HI">Hawaii</option>
                                            <option value="AL">Alabama</option>
                                            <option value="AK">Alaska</option>
                                            <option value="AZ">Arizona</option>
                                            <option value="AR">Arkansas</option>
                                            <option value="CA">California</option>
                                            <option value="CO">Colorado</option>
                                            <option value="CT">Connecticut</option>
                                            <option value="DE">Delaware</option>
                                            <option value="DC">District Of Columbia</option>
                                            <option value="FL">Florida</option>
                                            <option value="GA">Georgia</option>

                                            <option value="ID">Idaho</option>
                                            <option value="IL">Illinois</option>
                                            <option value="IN">Indiana</option>
                                            <option value="IA">Iowa</option>
                                            <option value="KS">Kansas</option>
                                            <option value="KY">Kentucky</option>
                                            <option value="LA">Louisiana</option>
                                            <option value="ME">Maine</option>
                                            <option value="MD">Maryland</option>
                                            <option value="MA">Massachusetts</option>
                                            <option value="MI">Michigan</option>
                                            <option value="MN">Minnesota</option>
                                            <option value="MS">Mississippi</option>
                                            <option value="MO">Missouri</option>
                                            <option value="MT">Montana</option>
                                            <option value="NE">Nebraska</option>
                                            <option value="NV">Nevada</option>
                                            <option value="NH">New Hampshire</option>
                                            <option value="NJ">New Jersey</option>
                                            <option value="NM">New Mexico</option>
                                            <option value="NY">New York</option>
                                            <option value="NC">North Carolina</option>
                                            <option value="ND">North Dakota</option>
                                            <option value="OH">Ohio</option>
                                            <option value="OK">Oklahoma</option>
                                            <option value="OR">Oregon</option>
                                            <option value="PA">Pennsylvania</option>
                                            <option value="RI">Rhode Island</option>
                                            <option value="SC">South Carolina</option>
                                            <option value="SD">South Dakota</option>
                                            <option value="TN">Tennessee</option>
                                            <option value="TX">Texas</option>
                                            <option value="UT">Utah</option>
                                            <option value="VT">Vermont</option>
                                            <option value="VA">Virginia</option>
                                            <option value="WA">Washington</option>
                                            <option value="WV">West Virginia</option>
                                            <option value="WI">Wisconsin</option>
                                            <option value="WY">Wyoming</option>
                                        </select>
                                    </div>
                                    <div class="col-sm-2 text-right">
                                        <label class="control-label" for="txtFiscalZip">Zip code:</label>
                                    </div>
                                    <div class="col-sm-2">
                                        <input class="form-control" type="text" name="txtFiscalZip" id="txtFiscalZip" runat="Server" />
                                    </div>
                                </div>
                                <hr />
                                <div class="row form-group-md">
                                    <div class="col-sm-2 text-left">
                                        <label class="control-label" for="ddlBillingSchedule">Billing schedule:</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <%--<asp:DropDownList ID="ddlBillingSchedule" runat="server" CssClass="form-control">
                                        </asp:DropDownList>--%>
                                        <select class="form-control" name="ddlBillingSchedule" id="ddlBillingSchedule" runat="Server" onchange="toggleBillOther();">
                                            <option value=""></option>
                                            <option value="Annual">Annual</option>
                                            <option value="Bi-annual">Bi-annual</option>
                                            <option value="Quarterly">Quarterly</option>
                                            <option value="Monthly">Monthly</option>
                                            <option value="Bi-monthly">Bi-monthly</option>
                                            <option value="Upon request">Upon request</option>
                                            <option value="Other">Other</option>
                                        </select>
                                    </div>
                                    <div id="divBillOther">
                                        <div class="col-sm-1 text-left">
                                            <label for="txtBillOther" id="lbltxtBillOther">Other:</label>
                                        </div>
                                        <div class="col-sm-3">
                                            <asp:TextBox ID="txtBillOther" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                    </div>

                                </div>
                                <br />
                                <div class="row">
                                    <label class="col-sm-2 text-left" for="txtNextInvoiceDate">Next schedule invoice date:</label>
                                    <div class="col-sm-3">
                                        <div class='input-group date' id='dtpNextInvoiceDate'>
                                            <span class="input-group-addon">
                                                <span class="glyphicon glyphicon-calendar"></span>
                                            </span>
                                            <asp:TextBox ID="txtNextInvoiceDate" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-sm-2 text-left">
                                        <label class="control-label" for="txtMemo">Comments:</label>
                                    </div>
                                    <div class="col-sm-7">
                                        <textarea class="form-control noresize" rows="3" name="txtMemo" id="txtMemo" runat="Server"></textarea>
                                    </div>
                                    <div class="col-md-3">
                                        <asp:Button ID="btnAddAgreement" runat="server" Text="Add New Agreement" OnClick="btnAddAgreement_Click" class="btn btn-info" />
                                    </div>
                                    &nbsp;
                                    <div class="col-md-3">
                                        <asp:Button ID="btnAddInvoice" runat="server" Text="Add New Invoice" OnClick="btnAddInvoice_Click" class="btn btn-info" />
                                    </div>

                                </div>

                            </div>

                            <div class="modal-footer">
                                <%--<asp:Label ID="lblResult" Visible="false" runat="server"></asp:Label>--%>
                                <asp:Button ID="btnSave" runat="server" Text="Update" CssClass="btn btn-info" OnClick="btnSave_Click" />
                                <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="btn btn-info" OnClick="btnClose_Click" UseSubmitBehavior="false" data-dismiss="modal" aria-hidden="true" />
                            </div>

                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="rptCC" EventName="ItemCommand" />
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnClose" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>



                </div>
            </div>
        </div>


    </div>

    <script type="text/javascript">
        function pageLoad(sender, args) {

            $('#li_super').addClass('selected');
            $('#li_payment').addClass('selected');
            $('#li_ccform').addClass('selected');

            var startDate = new DatePicker('dtpStartDate');
            startDate.init();
            startDate.onchange();

            var endDate = new DatePicker('dtpEndDate');
            endDate.init();
            endDate.onchange();

            var nextDate = new DatePicker('dtpNextInvoiceDate');
            nextDate.init();

            $("#editModal").scroll(function () {
                $('#dtpStartDate').datepicker('place');
                $('#dtpEndDate').datepicker('place');
            });

            toggleBillOther();

            $('#CollabCtr').DataTable({
                "iDisplayLength": 50,
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
                    { "bSortable": false }
                ]
            });
        }

        function toggleBillOther() {
            if ($("#MainContent_ddlBillingSchedule option:selected").text() == "Other")
                document.getElementById('divBillOther').style.display = "inherit";
            else
                document.getElementById('divBillOther').style.display = "none";
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

        $('form').validate({
            rules: {
                txtCCName: {
                    required: true
                },
                txtCCAbbrv: {
                    required: true
                }
                //txtDegree: {
                //    required: true
                //},
                //txtEmail: {
                //    required: true,
                //    email: true
                //},
                //txtPhone: {
                //    required: true,
                //    digits: true
                //},
                //txtDept: {
                //    required: true
                //},
                //txtProjectTitle: {
                //    required: true
                //}
            },
            //messages: {
            //    txtCaptchaCode: {
            //        remote: "Invalid Code"
            //    }
            //},
            //submitHandler: function (form) {
            //    //alert('ajax being called...');            
            //},
            invalidHandler: function (event, validator) {
                //alert('number of invalid fields: ' + validator.numberOfInvalids());
            },
            highlight: function (element, errorClass) {
                $(element).closest('.form-group').addClass('has-error');
            },
            unhighlight: function (element, errorClass) {
                $(element).closest('.form-group').removeClass('has-error');
            },
            onkeyup: false,
            onblur: true

        });

    </script>
</asp:Content>
