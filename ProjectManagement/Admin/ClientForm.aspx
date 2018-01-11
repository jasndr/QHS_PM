<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ClientForm.aspx.cs" Inherits="ProjectManagement.Admin.ClientForm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <div class="panel panel-default">
            <div class="panel-heading">
                <a data-toggle="collapse" data-target="#collapseRequest"
                    class="collapsed"><b>Request</b>
                </a>
            </div>

            <div id="collapseRequest" class="panel-collapse collapse">
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
            
        </div>

        <div class="panel panel-default">
            <div class="panel-heading"><b>Survey</b></div>

            <div class="panel-body">

                <div>
                    <table class="table table-striped table-hover">
                        <thead>
                            <tr>
                                <th style="width: 10%; padding: 0,0,0,1px;">Project Id</th>
                                <th style="width: 40%; padding: 0,0,0,1px;">Project Title</th>
                                <th style="width: 10%; padding: 0,0,0,1px;">Email</th>
                                <th style="width: 10%; padding: 0,0,0,1px;">Sent Date</th>
                                <th style="width: 10%; padding: 0,0,0,1px;">Responded</th>
                                <th style="width: 15%; padding: 0,0,0,1px;">Respond Date</th>
                                
                                <th style="width: 5%; padding: 0,0,0,1px;"></th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptSurvey" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Eval("ProjectId") %></td>
                                        <td><%# Eval("ProjectTitle") %></td>
                                        <td><%# Eval("SendTo") %></td>
                                        <td><%# Eval("SendDate") %></td>
                                        <td><%# Eval("Responded") %></td>
                                        <td><%# Eval("RespondDate") %></td>
                                        <td class="hidden"><%# Eval("Id") %></td>
                                        <td>
                                            <%--<input id="btnViewSurvey" type="button" onclick='<%# "return openSurvey(" + Eval("ProjectId") + ");" %>' value="View" class="btn btn-info"/>--%>
                                            <input id="btnViewSurvey" type="button" onclick="openSurvey(this)" value="View" class="btn btn-info"/>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>

            </div>
        </div>

        
    </div>

    <script type="text/javascript">
        function openSurvey(btnView) {
            //alert($(btnView).parent().siblings(":hidden"));
            var baseurl = getBaseUrl();
            
            var url = baseurl +  "../Guest/PIsurveyform/?id=" + ($(btnView).closest('tr').children('td.hidden').text());

            window.open(url, '_blank');

        }

        function getBaseUrl() {
            var re = new RegExp(/^.*\//);
            return re.exec(window.location.href);
        }


    </script>
</asp:Content>
