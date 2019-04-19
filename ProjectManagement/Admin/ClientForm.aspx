<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ClientForm.aspx.cs" Inherits="ProjectManagement.Admin.ClientForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <div class="panel panel-default">
            <div class="panel-heading">
                <%--<a data-toggle="collapse" data-target="#collapseRequest"
                    class="collapsed"><strong>Request</strong>
                </a>--%>
                <strong>Request</strong>
            </div>

            <div id="request">
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
                            <h3 id="editModalLabel">Client Request</h3>
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
                                    <div class="panel panel-info">
                                        <div class="panel-heading">
                                            <h4><b>Investigator Info</b></h4>
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row form-group-md">
                                        <%--<label class="col-sm-2 control-label" for="lblFirstName">First name</label>--%>
                                        <div class="col-sm-4">
                                            <b><u>First name:</u></b>
                                            <asp:Label ID="lblFirstName" runat="server"></asp:Label>
                                            <%--<input class="form-control disbaled" type="text" Id="lblFirstName" runat="server" />--%>
                                        </div>
                                        <div class="col-sm-4">
                                            <b><u>Last name:</u></b>
                                            <asp:Label ID="lblLastName" runat="server"></asp:Label>
                                        </div>
                                        <div class="col-sm-4">
                                            <b><u>Degree:</u></b>
                                            <asp:Label ID="lblDegree" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="col-sm-4">
                                            <b><u>Email:</u></b>
                                            <asp:Label ID="lblEmail" runat="server"></asp:Label>
                                        </div>
                                        <div class="col-sm-4">
                                            <b><u>Phone number:</u></b>
                                            <asp:Label ID="lblPhone" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="col-sm-6">
                                            <b><u>Organization/Department:</u></b>
                                            <asp:Label ID="lblDept" runat="server"></asp:Label>
                                        </div>
                                        <div class="col-sm-6">
                                            <b><u>Investigator status:</u></b>
                                            <asp:Label ID="lblInvestStatus" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="col-sm-4">
                                            <b><u>Is PI a junior investigator?</u></b>
                                            <asp:Label ID="lblJuniorPI" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="col-sm-4">
                                            <b><u>Does PI have mentor?</u></b>
                                            <asp:Label ID="lblMentor" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <br />


                                    <hr />
                                    <%--<h4 style="background-color: cornflowerblue">Project Info</h4>--%>
                                    <div class="panel panel-info">
                                        <div class="panel-heading">
                                            <h4><b>Project Info</b></h4>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-12">
                                            <b><u>Project Title:</u></b>
                                            <asp:Label ID="lblProjectTitle" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="col-sm-12">
                                            <b><u>Project Summary:</u></b>
                                            <asp:Label ID="lblProjectSummary" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="col-sm-6">
                                            <b><u>Study Area:</u></b>
                                            <asp:Label ID="lblStudyArea" runat="server"></asp:Label>
                                        </div>
                                        <div class="col-sm-6">
                                            <b><u>Health Database(s) Utilized:</u></b>
                                            <asp:Label ID="lblHealthData" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="col-sm-6">
                                            <b><u>Study Type:</u></b>
                                            <asp:Label ID="lblStudyType" runat="server"></asp:Label>
                                        </div>
                                        <div class="col-sm-6">
                                            <b><u>Study Population:</u></b>
                                            <asp:Label ID="lblStudyPopulation" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="col-sm-6"></div>
                                        <div class="col-sm-5 col-sm-offset-1">
                                            <b><u>Health Disparity?</u></b>
                                            <asp:Label ID="lblHealthDisparity" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="col-sm-12">
                                            <b><u>Type of support:</u></b>
                                            <asp:Label ID="lblService" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <br />

                                    <div class="row">
                                        <div class="col-sm-6">
                                            <b><u>Is project a funded infrastructure grant pilot study?</u></b>
                                            <asp:Label ID="lblPilot" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <br />


                                    <div class="row">
                                        <div class="col-sm-6">
                                            <b><u>Is this project for a grant proposal?</u></b>
                                            <asp:Label ID="lblProposal" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <br />

                                    <div class="row">
                                        <div class="col-sm-11 col-sm-offset-1">
                                            <b><u>Is this application to a pilot program of a UH infrastructure grant?</u></b>
                                            <asp:Label ID="lblUHPilotGrant" runat="server"></asp:Label>
                                        </div>
                                        <div class="col-sm-11 col-sm-offset-1">
                                            <b><u>What is the grant / funding agency?</u></b>
                                            <asp:Label ID="lblPilotGrantName" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <br />

                                    <div class="row">
                                        <div class="col-sm-12">
                                            <b><u>Funding Source:</u></b>
                                            <asp:Label ID="lblGrant" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <br />


                                    <div class="row">
                                        <div class="col-sm-12">
                                            <b><u>Important deadlines:</u></b>
                                            <asp:Label ID="lblDueDate" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="col-sm-12">
                                            <b><u>QHS faculty/staff preference (if any):</u></b>
                                            <asp:Label ID="lblBiostat" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:CheckBox ID="chkCompleted" runat="server" Text="Request Completed" Enabled="false"  />
                                        </div>
                                        <%--<div class="col-md-4">
                                            <asp:Button ID="btnCreateProject" runat="server" Text="Create Project" CssClass="btn btn-dark btn-sm" OnClientClick="return validateControl();" OnClick="btnCreateProject_Click" />
                                        </div>--%>
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
                                    <%--<asp:Button ID="btnSave" runat="server" Text="Update" CssClass="btn btn-info" OnClientClick="return validateControl();" OnClick="btnSave_Click" />--%>
                                    <asp:Button ID="btnCreateProject" runat="server" Text="Create Project" CssClass="btn btn-dark" OnClientClick="return validateControl();" OnClick="btnCreateProject_Click" />
                                       
                                    <button class="btn btn-info" data-dismiss="modal" aria-hidden="true">Close</button>
                                </div>

                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="rptClientRqst" EventName="ItemCommand" />
                                <%--<asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />--%>
                                <asp:AsyncPostBackTrigger ControlID="btnCreateProject" EventName="Click" />
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
                                            <input id="btnViewSurvey" type="button" onclick="openSurvey(this)" value="View" class="btn btn-info" />
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

            var url = baseurl + "../Guest/PIsurveyform/?id=" + ($(btnView).closest('tr').children('td.hidden').text());

            window.open(url, '_blank');

        }

        function getBaseUrl() {
            var re = new RegExp(/^.*\//);
            return re.exec(window.location.href);
        }


    </script>
</asp:Content>
