<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TaskForm.aspx.cs" Inherits="ProjectManagement.Admin.TaskForm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<script src="../Scripts/highcharts/4.2.0/highcharts.js"></script>--%>
    <script src="../Scripts/bootstrap-datepicker.min.js"></script>
    <script src="../Scripts/jquery.validate.min.js"></script>    
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
    
    <div id="divTask" class="jumbotron">
        <div class="panel panel-default">
            <div class="panel-heading"><b>Tasks</b></div>
            <div class="panel-body">
                <div class="row">
                    <div class="col-xs-6 col-md-2">
                        Assign To:
                        <asp:DropDownList ID="ddlBiostat" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                    <div class="col-xs-6 col-md-2">
                        Task Status:
                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                    <div class="col-xs-6 col-md-2">
                        Priority:
                        <asp:DropDownList ID="ddlPriority" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                    <div class="col-xs-6 col-md-2">
                        &nbsp;
                        <div>
                            <asp:Button ID="btnSumbit" runat="server" Text="Submit" CssClass="btn btn-info" OnClick="btnSumbit_Click" OnClientClick="ClientSideClick(this)" UseSubmitBehavior="False" />
                        </div>
                    </div>
                </div>
                
                <hr />
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div>
                            <asp:Button ID="btnAdd" runat="server" Text="Add a new task" CssClass="btn btn-info" OnClick="btnAdd_Click" />
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-12">
                                <table class="table table-striped table-hover">
                                    <thead>
                                        <tr>
                                            <th style="width: 5%; padding: 0,0,0,1px;">Id</th>
                                            <th style="width: 20%; padding: 0,0,0,1px;">Title</th>
                                            <th style="width: 25%; padding: 0,0,0,1px;">Notes</th>
                                            <th style="width: 10%; padding: 0,0,0,1px;">Status</th>
                                            <th style="width: 10%; padding: 0,0,0,1px;">Priority</th>
                                            <th style="width: 15%; padding: 0,0,0,1px;">Assign To</th>
                                            <th style="width: 10%; padding: 0,0,0,1px;">Due Date</th>
                                            <th style="width: 5%; padding: 0,0,0,1px;"></th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="rptTask" runat="server" OnItemCommand="rptTask_ItemCommand">
                                            <ItemTemplate>
                                                <tr>
                                                    <td><%# Eval("Id") %></td>
                                                    <td><%# Eval("Title") %></td>
                                                    <td><%# Eval("Notes") %></td>
                                                    <td><%# Eval("Status") %></td>
                                                    <td><%# Eval("Priority") %></td>
                                                    <td><%# Eval("AssignTo") %></td>
                                                    <%--<td><%# ((DateTime)Eval("DueDate")).ToShortDateString()%></td>--%>
                                                    <td><%# Eval("DueDate") %></td>
                                                    <td>
                                                        <asp:Button ID="btnView" runat="server" Text="Edit" CssClass="btn btn-info" CommandArgument='<%# Eval("Id") %>' /></td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tbody>
                                </table>
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
                    <div class="modal-dialog modal-lg">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                                <h4 id="editModalLabel">Add/Edit Task</h4>
                            </div>
                            <asp:UpdatePanel ID="upEdit" runat="server">
                                <ContentTemplate>
                                    <div class="modal-body">
                                        
                                        <div class="row">
                                            <div class="col-xs-11 col-md-10"></div>
                                            <div class="col-xs-1 col-md-2">
                                                Id: <b><asp:Label ID="lblTaskId" runat="server"></asp:Label></b>
                                            </div>
                                        </div>
                                        <h5>Task Info</h5>
                                        <div class="row">
                                            <div class="col-xs-9 col-md-9">
                                                <label class="control-label">Task Title:</label>
                                                <asp:TextBox ID="txtTaskTitle" runat="server" placeholder="Task Title" class="form-control"></asp:TextBox>
                                            </div>
                                            <div class="col-xs-3 col-md-3">
                                                <label class="control-label">Priority:</label>
                                                <asp:DropDownList ID="ddlTaskPriority" runat="server" CssClass="form-control">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="row">
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
                                        </div>
                                        <div class="row">
                                            <div class="col-xs-3 col-md-3">
                                                <label class="control-label">Task Status:</label>
                                                <asp:DropDownList ID="ddlTaskStatus" runat="server" CssClass="form-control">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-xs-3 col-md-3"></div>
                                            
                                        </div>
                                        <div class="row">
                                            <div class="col-xs-9 col-md-9">
                                                <label class="control-label">Note:</label>
                                                <asp:TextBox ID="txtTaskNote" runat="server" placeholder="Note" class="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                        <br />
                                        <h5>Previous Status</h5>
                                        <br />
                                        <table class="table table-striped table-hover">
                                            <thead>
                                                <tr>
                                                    <th style="width: 10%; padding: 0,0,0,1px;">Id</th>
                                                    <th style="width: 70%; padding: 0,0,0,1px;">Previous Value</th>
                                                    <th style="width: 10%; padding: 0,0,0,1px;">Creator</th>
                                                    <th style="width: 10%; padding: 0,0,0,1px;">Date</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <asp:Repeater ID="rptTaskHistory" runat="server" OnItemCommand="rptTask_ItemCommand">
                                                    <ItemTemplate>
                                                        <tr>                                                            
                                                            <td><%# Eval("Id") %></td>
                                                            <td><%# Eval("TaskValue") %></td>
                                                            <td><%# Eval("Creator") %></td>
                                                            <td><%# Eval("CreateDate") %></td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </tbody>
                                        </table>
                                        </div>

                                        <div class="modal-footer">
                                            <asp:Label ID="lblResult" Visible="false" runat="server"></asp:Label>
                                            <asp:Button ID="btnSave" runat="server" Text="Update" CssClass="btn btn-info" OnClientClick="return validateControl();" OnClick="btnSave_Click" />
                                            <button class="btn btn-info" data-dismiss="modal" aria-hidden="true">Close</button>
                                        </div>
                                    
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="rptTask" EventName="ItemCommand" />
                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>

                                    
                                
                        </div>
                    </div>
                </div>

        <%--<div class="pdsa-submit-progress hidden">
            <i class="fa fa-2x fa-spinner fa-spin"></i>&nbsp;
            <label>Loading ...</label>
        </div>--%>

    </div>   

    
    <script type="text/javascript">
        function pageLoad(sender, args) { 
            $('#li_admin').addClass('selected');
            $('#li_taskform').addClass('selected');

            var dueDate = new DatePicker('dtpDueDate');
            dueDate.init();
            dueDate.onchange();

            //$(function () {
            //    $('#dtpDueDate').datepicker({
            //        todayHighlight: true,
            //        format: "mm/dd/yyyy",
            //        autoclose: true,
            //        orientation: "top"
            //    })
            //    .on('changeDate', function (e) {
            //         //revalidate
            //        if ($('#dtpDueDate').validate()) {
            //            $('#dtpDueDate').closest('.row').removeClass('has-error');
            //         }
            //     });
            //    $('#dtpSubDate').datepicker({
            //        todayHighlight: true,
            //        format: "mm/dd/yyyy",
            //        autoclose: true,
            //        orientation: "top"
            //    });
            //});

            //datepicker module
            

            $("#MainContent_txtTaskTitle").addClass("required");
            $("#MainContent_txtDueDate").addClass("required");
            $("#MainContent_ddlAssignTo").addClass("required");
            $("#MainContent_ddlTaskStatus").addClass("required");
            $("#MainContent_ddlTaskPriority").addClass("required");

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
                ,onchange: function (e) {
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
            var validator = $("#commentForm").validate({

                highlight: function (element) {
                    $(element).closest('.row').addClass('has-error');
                },
                unhighlight: function (element) {
                    $(element).closest('.row').removeClass('has-error');
                }

            });

            var isValid = $("#commentForm").valid();

            if (validator.errorList.length > 0) {
                var firstElement = validator.errorList[0].element;
                firstElement.focus();
            }

            return isValid;
        }

        function ClientSideClick(myButton) {
            //make sure the button is not of type "submit" but "button"
            if (myButton.getAttribute('type') == 'button') {
                // disable the button                
                myButton.disabled = true;
                //myButton.className = "btn-inactive";
                myButton.value = "Processing......";
            }
            return true;
        }

        //fix jquery validator for datetime
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

    </script>
    
</asp:Content>
