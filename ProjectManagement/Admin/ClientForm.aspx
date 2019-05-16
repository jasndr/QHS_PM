<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ClientForm.aspx.cs" Inherits="ProjectManagement.Admin.ClientForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .notAllowed {
            opacity: 0.6;
            /*cursor: not-allowed;*/
        }
        .validation{
            font-weight: bold;
            color: red;
        }
        #tblClientRequest_filter label,
        #tblClientRequest_length label,
        #tblClientSurvey_filter label,
        #tblClientSurvey_length label{
            font-weight: normal;
            display: block;
        }
        #tblClientRequest_filter input,
        #tblClientSurvey_filter input{
            margin-left: 10px;
            display: inline-block;
           
        }
        #tblClientRequest_filter,
        #tblClientSurvey_filter input{
            
            max-width: inherit;
        }


    </style>
    <link href="../Content/dataTables.bootstrap.min.css" rel="stylesheet" />
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
                                <table class="table table-striped table-hover table-bordered no-footer dataTable" id="tblClientRequest">
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
                            <span><em>Loading... Please wait!</em><img style="height: 25px; width: 25px;" src="../images/Loading_2.gif" alt="Loading icon" /></span><%--<img src="../images/Logo_Final.png" alt="Loading.. Please wait!" />--%>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </div>
            </div>

            <div id="editModal" class="modal fade" role="dialog" aria-labelledby="gridSystemModalLabel" aria-hidden="true"
                data-backdrop="static" data-keyboard="false">
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true" onclick="cleartQueryString()">×</button>
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
                                        <%--<div class="col-sm-4">
                                            <b><u>First name:</u></b>
                                            <asp:Label ID="lblFirstName" runat="server"></asp:Label>
                                        </div>--%>
                                        <label class="col-sm-2 control-label" for="txtFirstName"><strong>First name:</strong></label>
                                        <div class="col-sm-2">
                                            <input class="form-control" type="text" name="txtFirstName" id="txtFirstName" placeholder="First name" runat="server" />
                                            <asp:RequiredFieldValidator ID="rfTxtFirstName" runat="server" 
                                                ErrorMessage="Required." ControlToValidate="txtFirstName"
                                                ValidationGroup="clientRequestFormValidations" CssClass="validation"></asp:RequiredFieldValidator>
                                        </div>
                                        <label class="col-sm-2 control-label" for="txtLastName"><strong>Last name:</strong></label>
                                        <div class="col-sm-2">
                                            <input class="form-control" type="text" name="txtLastName" id="txtLastName" placeholder="Last name" runat="server" />
                                            <asp:RequiredFieldValidator ID="rdTxtLastName" runat="server" 
                                                ErrorMessage="Required." ControlToValidate="txtLastName"
                                                ValidationGroup="clientRequestFormValidations" CssClass="validation"></asp:RequiredFieldValidator>
                                        </div>
                                        <%--<div class="col-sm-4">
                                            <b><u>Degree:</u></b>
                                            <asp:Label ID="lblDegree" runat="server"></asp:Label>
                                        </div>--%>
                                        <label class="col-sm-1 control-label" for="ddlDegree"><strong>Degree</strong></label>
                                        <div class="col-sm-3">
                                            <asp:DropDownList ID="ddlDegree" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="row form-group-md">
                                        <div class="col-sm-9"></div>
                                        <div class="col-sm-3">
                                            <label class="control-label" for="txtDegreeOther">Other:</label>
                                            <input class="form-control" type="text" name="txtDegreeOther" id="txtDegreeOther" placeholder="Degree - Other" runat="Server" />
                                            <%--<asp:RequiredFieldValidator ID="rfTxtDegreeOther" runat="server" 
                                                ErrorMessage="Required." ControlToValidate="txtDegreeOther"
                                                ValidationGroup="clientRequestFormValidations" CssClass="validation">
                                            </asp:RequiredFieldValidator>--%>
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <label class="col-sm-2 control-label" for="txtEmail"><strong>Email</strong></label>
                                        <div class="col-sm-2">
                                            <input class="form-control" type="text" name="txtEmail" id="txtEmail" placeholder="Email" runat="server" />
                                            <asp:RequiredFieldValidator ID="rfTxtEmail" runat="server" 
                                                ErrorMessage="Required." ControlToValidate="txtEmail"
                                                ValidationGroup="clientRequestFormValidations" CssClass="validation">
                                            </asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="reTxtEmail" runat="server"
                                                ControlToValidate="txtEmail" CssClass="validation" ErrorMessage="Enter valid e-mail address."
                                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                                            </asp:RegularExpressionValidator>
                                        </div>
                                        <label class="col-sm-2 control-label" for="txtPhone"><strong>Phone number</strong></label>
                                        <div class="col-sm-2">
                                            <input class="form-control phoneNum" type="text" name="txtPhone" id="txtPhone" placeholder="Phone" runat="server" />
                                            <asp:RequiredFieldValidator ID="rfTxtPhone" runat="server" 
                                                ErrorMessage="Required." ControlToValidate="txtPhone"
                                                ValidationGroup="clientRequestFormValidations" CssClass="validation">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <label class="col-sm-3 control-label" for="txtDept"><strong>Organization/Department</strong></label>
                                        <div class="col-sm-4">
                                            <input class="form-control typeahead" type="text" name="txtDept" id="txtDept" placeholder="Organization/Department" runat="server" onchange="updateId(this)" />
                                            <asp:RequiredFieldValidator ID="rfTxtDept" runat="server" 
                                                ErrorMessage="Required." ControlToValidate="txtDept"
                                                ValidationGroup="clientRequestFormValidations" CssClass="validation">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                        <label class="col-sm-2 control-label">Investigator status</label>
                                        <div class="col-sm-3">
                                            <asp:DropDownList ID="ddlPIStatus" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="row offset2 hidden">
                                        <div class="col-md-2 text-right">
                                            <label class="control-label">Organization Affiliation Id:</label>
                                        </div>
                                        <div class="col-md-1">
                                            <input class="form-control" type="text" name="txtDeptAffilId" id="txtDeptAffilId" runat="Server" />
                                        </div>
                                    </div>
                                    <br />
                                    <%--<div class="row">
                                        <div class="col-sm-4">
                                            <b><u>Is PI a junior investigator?</u></b>
                                            <asp:Label ID="lblJuniorPI" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <br />--%>
                                    <div class="row" id="divJuniorPiQ">
                                        <div class="col-sm-4">
                                            <label for="chkJuniorPIYes">Is PI a junior investigator?</label>
                                        </div>
                                        <div class="col-sm-2">
                                            <asp:CheckBox ID="chkJuniorPIYes" value="1" runat="server" Text="Yes"></asp:CheckBox>
                                        </div>
                                        <div class="col-sm-2">
                                            <asp:CheckBox ID="chkJuniorPINo" value="0" runat="server" Text="No"></asp:CheckBox>
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row" id="divMentorQ">
                                        <div class="col-sm-4">
                                            <label for="chkMentorYes">Does PI have mentor?</label>
                                        </div>
                                        <div class="col-sm-2">
                                            <asp:CheckBox ID="chkMentorYes" value="1" runat="server" Text="Yes"></asp:CheckBox>
                                        </div>
                                        <div class="col-sm-2">
                                            <asp:CheckBox ID="chkMentorNo" value="0" runat="server" Text="No"></asp:CheckBox>
                                        </div>
                                    </div>
                                    <br />

                                    <div class="row" id="divMentor">
                                        <div class="col-sm-2 text-left">
                                            <label class="control-label" for="txtTitle">Mentor first name:</label>
                                        </div>
                                        <div class="col-sm-2">
                                            <input class="form-control" type="text" name="txtMentorFirstName" id="txtMentorFirstName" runat="Server" />
                                            <%--<asp:RequiredFieldValidator ID="rfTxtMentorFirstName" runat="server" 
                                                ErrorMessage="Required." ControlToValidate="txtMentorFirstName"
                                                ValidationGroup="clientRequestFormValidations" CssClass="validation">
                                            </asp:RequiredFieldValidator>--%>
                                        </div>
                                        <div class="col-sm-2 text-right">
                                            <label class="control-label" for="txtTitle">Mentor last name:</label>
                                        </div>
                                        <div class="col-sm-2">
                                            <input class="form-control" type="text" name="txtMentorLastName" id="txtMentorLastName" runat="Server" />
                                            <%--<asp:RequiredFieldValidator ID="rfTxtMentorLastName" runat="server" 
                                                ErrorMessage="Required." ControlToValidate="txtMentorLastName"
                                                ValidationGroup="clientRequestFormValidations" CssClass="validation">
                                            </asp:RequiredFieldValidator>--%>
                                        </div>
                                        <div class="col-sm-1 text-right">
                                            <label class="control-label" for="txtTitle">Email:</label>
                                        </div>
                                        <div class="col-sm-3">
                                            <input class="form-control" type="text" name="txtMentorEmail" id="txtMentorEmail" runat="Server" />
                                           <%-- <asp:RequiredFieldValidator ID="rfTxtMentorEmail" runat="server" 
                                                ErrorMessage="Required." ControlToValidate="txtMentorEmail"
                                                ValidationGroup="clientRequestFormValidations" CssClass="validation">
                                            </asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="reTxtMentorEmail" runat="server"
                                                ControlToValidate="txtMentorEmail" CssClass="validation" ErrorMessage="Enter valid e-mail address."
                                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                                            </asp:RegularExpressionValidator>--%>
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
                                        <%--<div class="col-sm-12">
                                            <b><u>Project Title:</u></b>
                                            <asp:Label ID="lblProjectTitle" runat="server"></asp:Label>
                                        </div>--%>
                                        <label class="col-sm-2 control-label" for="txtProjectTitle">Project title</label>
                                        <div class="col-sm-10">
                                            <input class="form-control" type="text" name="txtProjectTitle" id="txtProjectTitle" placeholder="Project title" runat="server" />
                                            <asp:RequiredFieldValidator ID="rfTxtProjectTitle" runat="server" 
                                                ErrorMessage="Required." ControlToValidate="txtProjectTitle"
                                                ValidationGroup="clientRequestFormValidations" CssClass="validation">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <label class="col-sm-2 control-label" for="txtProjectSummary">
                                            Summary of project and QHS need
                                        </label>
                                        <div class="col-sm-10">
                                            <textarea class="form-control noresize" rows="5" id="txtProjectSummary" name="txtProjectSummary" runat="server"></textarea>
                                            <asp:RequiredFieldValidator ID="rfTxtProjectSummary" runat="server" 
                                                ErrorMessage="Required." ControlToValidate="txtProjectSummary"
                                                ValidationGroup="clientRequestFormValidations" CssClass="validation">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <hr />
                                    <div class="row">
                                        <label class="col-sm-2 control-label">
                                            Study Area(s)<span class="help-block">(check all that apply)</span>
                                        </label>
                                        <div class="col-sm-10">
                                            <table class="table" id="tblStudyArea">
                                                <%--runat="server"--%>
                                                <tbody>
                                                    <asp:Repeater ID="rptStudyArea" runat="server">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td style="width: 50%">
                                                                    <asp:CheckBox ID="chkId" runat="server"></asp:CheckBox>
                                                                    <asp:HiddenField ID="Id" Value='<%#Eval("Id")%>' runat="server" />
                                                                    <asp:HiddenField ID="BitValue" Value='<%#Eval("BitValue")%>' runat="server" />
                                                                    <%# Eval("Name") %>
                                                                </td>
                                                        </ItemTemplate>
                                                        <AlternatingItemTemplate>
                                                            <td style="width: 50%">
                                                                <asp:CheckBox ID="chkId" runat="server"></asp:CheckBox>
                                                                <asp:HiddenField ID="Id" Value='<%#Eval("Id")%>' runat="server" />
                                                                <asp:HiddenField ID="BitValue" Value='<%#Eval("BitValue")%>' runat="server" />
                                                                <%# Eval("Name") %>
                                                            </td>
                                                            </tr>
                                                        </AlternatingItemTemplate>
                                                    </asp:Repeater>
                                                </tbody>
                                            </table>
                                            <div class="row">
                                                <div class="col-sm-6">
                                                    <label class="control-label" for="txtStudyAreaOther">Other:</label>
                                                    <input class="form-control" type="text" name="txtStudyAreaOther" id="txtStudyAreaOther" placeholder="Study Area(s) - Other" runat="Server" />
                                                    <br />
                                                </div>
                                                <div class="col-sm-2">
                                                    <input class="form-control hidden" type="text" name="txtStudyAreaBitSum" id="txtStudyAreaBitSum" runat="Server" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <hr />

                                    <div class="row">
                                        <label class="col-sm-2 control-label">
                                            Health Database(s) Utilized<span class="help-block">(check all that apply)</span>
                                        </label>
                                        <div class="col-sm-10">
                                            <table class="table" id="tblHealthData">
                                                <%--runat="server"--%>
                                                <tbody>
                                                    <asp:Repeater ID="rptHealthData" runat="server">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td style="width: 50%">
                                                                    <asp:CheckBox ID="chkId" runat="server"></asp:CheckBox>
                                                                    <asp:HiddenField ID="Id" Value='<%#Eval("Id")%>' runat="server" />
                                                                    <asp:HiddenField ID="BitValue" Value='<%#Eval("BitValue")%>' runat="server" />
                                                                    <%# Eval("Name") %>
                                                                </td>
                                                        </ItemTemplate>
                                                        <AlternatingItemTemplate>
                                                            <td style="width: 50%">
                                                                <asp:CheckBox ID="chkId" runat="server"></asp:CheckBox>
                                                                <asp:HiddenField ID="Id" Value='<%#Eval("Id")%>' runat="server" />
                                                                <asp:HiddenField ID="BitValue" Value='<%#Eval("BitValue")%>' runat="server" />
                                                                <%# Eval("Name") %>
                                                            </td>
                                                            </tr>
                                                        </AlternatingItemTemplate>
                                                    </asp:Repeater>
                                                </tbody>
                                            </table>
                                            <div class="row">
                                                <div class="col-sm-6">
                                                    <label class="control-label" for="txtHealthDataOther">Other:</label>
                                                    <input class="form-control" type="text" name="txtHealthDataOther" id="txtHealthDataOther" placeholder="Health Database(s) Utilized - Other" runat="Server" />
                                                    <br />
                                                </div>
                                                <div class="col-sm-2">
                                                    <input class="form-control hidden" type="text" name="txtHealthDataBitSum" id="txtHealthDataBitSum" runat="Server" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <hr />
                                    <div class="row">
                                        <label class="col-sm-2 control-label">
                                            Study Type<span class="help-block">(check all that apply)</span>
                                        </label>
                                        <div class="col-sm-10">
                                            <table class="table" id="tblStudyType">
                                                <%--runat="server"--%>
                                                <tbody>
                                                    <asp:Repeater ID="rptStudyType" runat="server">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td style="width: 50%">
                                                                    <asp:CheckBox ID="chkId" runat="server"></asp:CheckBox>
                                                                    <asp:HiddenField ID="Id" Value='<%#Eval("Id")%>' runat="server" />
                                                                    <asp:HiddenField ID="BitValue" Value='<%#Eval("BitValue")%>' runat="server" />
                                                                    <%# Eval("Name") %>
                                                                </td>
                                                        </ItemTemplate>
                                                        <AlternatingItemTemplate>
                                                            <td style="width: 50%">
                                                                <asp:CheckBox ID="chkId" runat="server"></asp:CheckBox>
                                                                <asp:HiddenField ID="Id" Value='<%#Eval("Id")%>' runat="server" />
                                                                <asp:HiddenField ID="BitValue" Value='<%#Eval("BitValue")%>' runat="server" />
                                                                <%# Eval("Name") %>
                                                            </td>
                                                            </tr>
                                                        </AlternatingItemTemplate>
                                                    </asp:Repeater>
                                                </tbody>
                                            </table>
                                            <div class="row">
                                                <div class="col-sm-6">
                                                    <label class="control-label" for="txtStudyTypeOther">Other:</label>
                                                    <input class="form-control" type="text" name="txtStudyTypeOther" id="txtStudyTypeOther" placeholder="Study Type - Other" runat="Server" />
                                                    <br />
                                                </div>
                                                <div class="col-sm-2">
                                                    <input class="form-control hidden" type="text" name="txtStudyTypeBitSum" id="txtStudyTypeBitSum" runat="Server" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <hr />

                                    <div class="row">
                                        <label class="col-sm-2 control-label">
                                            Study Population<span class="help-block">(check all that apply)</span>
                                        </label>
                                        <div class="col-sm-10">
                                            <table class="table" id="tblStudyPopulation">
                                                <%--runat="server"--%>
                                                <tbody>
                                                    <asp:Repeater ID="rptStudyPopulation" runat="server">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td style="width: 50%">
                                                                    <asp:CheckBox ID="chkId" runat="server"></asp:CheckBox>
                                                                    <asp:HiddenField ID="Id" Value='<%#Eval("Id")%>' runat="server" />
                                                                    <asp:HiddenField ID="BitValue" Value='<%#Eval("BitValue")%>' runat="server" />
                                                                    <%# Eval("Name") %>
                                                                </td>
                                                        </ItemTemplate>
                                                        <AlternatingItemTemplate>
                                                            <td style="width: 50%">
                                                                <asp:CheckBox ID="chkId" runat="server"></asp:CheckBox>
                                                                <asp:HiddenField ID="Id" Value='<%#Eval("Id")%>' runat="server" />
                                                                <asp:HiddenField ID="BitValue" Value='<%#Eval("BitValue")%>' runat="server" />
                                                                <%# Eval("Name") %>
                                                            </td>
                                                            </tr>
                                                        </AlternatingItemTemplate>
                                                    </asp:Repeater>
                                                </tbody>
                                            </table>
                                            <div class="row">
                                                <div class="col-sm-6" id="divHealthDisparity">
                                                    <div class="col-sm-3">
                                                        <label for="chkHealthDisparityYes">Health disparity?</label>
                                                    </div>
                                                    <div class="col-sm-3">
                                                        <asp:CheckBox ID="chkHealthDisparityYes" runat="server" Text="Yes"></asp:CheckBox>
                                                    </div>
                                                    <div class="col-sm-3">
                                                        <asp:CheckBox ID="chkHealthDisparityNo" runat="server" Text="No"></asp:CheckBox>
                                                    </div>
                                                    <div class="col-sm-3">
                                                        <asp:CheckBox ID="chkHealthDisparityNA" runat="server" Text="N/A"></asp:CheckBox>
                                                    </div>
                                                </div>
                                                <div class="col-sm-6">
                                                    <label class="control-label" for="txtStudyPopulationOther">Specify:</label>
                                                    <input class="form-control" type="text" name="txtStudyPopulationOther" id="txtStudyPopulationOther" placeholder="International Populations - Specify" runat="Server" />
                                                    <br />
                                                </div>
                                                <div class="col-sm-2">
                                                    <input class="form-control hidden" type="text" name="txtStudyPopulationBitSum" id="txtStudyPopulationBitSum" runat="Server" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <hr />

                                    <div class="row">
                                        <div class="col-sm-2">
                                            <label class="control-label">
                                                Type(s) of support needed
                                            </label>
                                        </div>
                                        <div class="col-sm-10">
                                            <table class="table" id="tblService">
                                                <%--runat="server"--%>
                                                <tbody>
                                                    <asp:Repeater ID="rptService" runat="server">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td style="width: 50%">
                                                                    <asp:CheckBox ID="chkId" runat="server"></asp:CheckBox>
                                                                    <asp:HiddenField ID="Id" Value='<%#Eval("Id")%>' runat="server" />
                                                                    <asp:HiddenField ID="BitValue" Value='<%#Eval("BitValue")%>' runat="server" />
                                                                    <%# Eval("Name") %>
                                                                </td>
                                                        </ItemTemplate>
                                                        <AlternatingItemTemplate>
                                                            <td style="width: 50%">
                                                                <asp:CheckBox ID="chkId" runat="server"></asp:CheckBox>
                                                                <asp:HiddenField ID="Id" Value='<%#Eval("Id")%>' runat="server" />
                                                                <asp:HiddenField ID="BitValue" Value='<%#Eval("BitValue")%>' runat="server" />
                                                                <%# Eval("Name") %>
                                                            </td>
                                                            </tr>
                                                        </AlternatingItemTemplate>
                                                    </asp:Repeater>
                                                </tbody>
                                            </table>
                                            <div class="row">
                                                <div class="col-sm-6">
                                                    <label class="control-label" for="txtServiceOther">Other:</label>
                                                    <input class="form-control" type="text" name="txtServiceOther" id="txtServiceOther" placeholder="Type(s) of support needed - Other" runat="Server" />
                                                    <br />
                                                </div>
                                                <div class="col-sm-2">
                                                    <input class="form-control hidden" type="text" name="txtServiceBitSum" id="txtServiceBitSum" runat="Server" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <hr />

                                    <div class="row" id="divPilotQ">
                                        <div>
                                            <div class="col-sm-4">
                                                <label for="chkPilotYes">Is project a funded infrastructure grant pilot study?</label>
                                            </div>
                                            <div class="col-sm-2">
                                                <asp:CheckBox ID="chkPilotYes" value="1" runat="server" Text="Yes"></asp:CheckBox>
                                            </div>
                                            <div class="col-sm-2">
                                                <asp:CheckBox ID="chkPilotNo" value="0" runat="server" Text="No"></asp:CheckBox>
                                            </div>
                                        </div>
                                    </div>
                                    <br />

                                    <div class="row" id="divProposalQ">
                                        <div>
                                            <div class="col-sm-4">
                                                <label for="chkProposalYes">Is this project for a grant proposal?</label>
                                            </div>
                                            <div class="col-sm-2">
                                                <asp:CheckBox ID="chkProposalYes" value="1" runat="server" Text="Yes"></asp:CheckBox>
                                            </div>
                                            <div class="col-sm-2">
                                                <asp:CheckBox ID="chkProposalNo" value="0" runat="server" Text="No"></asp:CheckBox>
                                            </div>
                                        </div>
                                    </div>
                                    <br />

                                    <div class="row divGrantProposal" id="divGrantProposal">
                                        <div class="col-sm-6">
                                            <div class="col-sm-8 text-left">
                                                <label class="control-label" for="txt">Is this application to a pilot program of a UH infrastructure grant?</label>
                                            </div>
                                            <div class="col-sm-4">
                                                <input type="checkbox" id="chkIsUHPilotGrantYes" value="1" runat="server" />Yes
                                        &nbsp;
                                    <input type="checkbox" id="chkIsUHPilotGrantNo" value="0" runat="server" />No
                                            </div>
                                        </div>
                                        <div class="col-sm-6" id="divGrantProposalFundingAgency">
                                            <div class="col-sm-4 text-left">
                                                <label class="control-label" for="txtGrantProposalFundingAgency">What is the funding agency?</label>
                                            </div>
                                            <div class="col-sm-6">
                                                <input class="form-control" type="text" name="txtGrantProposalFundingAgency"
                                                    id="txtGrantProposalFundingAgency" runat="Server" />
                                            </div>
                                        </div>
                                    </div>
                                    <br />


                                    <hr />

                                    <div class="row">
                                        <label class="col-sm-2 control-label">
                                            What is the funding source to support this request?<span class="help-block">(check all that apply)</span> <%--Is your project affiliated with any of the following grants?--%>
                                        </label>
                                        <div class="col-sm-10">
                                            <table class="table" id="tblFunding">
                                                <%--runat="server"--%>
                                                <tbody>
                                                    <asp:Repeater ID="rptFunding" runat="server">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td style="width: 25%">
                                                                    <asp:CheckBox ID="FirstchkId" runat="server"></asp:CheckBox>
                                                                    <%--<asp:HiddenField ID="Id1" Value='<%#Eval("Id1")%>' runat="server" />--%>
                                                                    <asp:HiddenField ID="FirstBitValue" Value='<%#Eval("BitValue1")%>' runat="server" />
                                                                    <%# Eval("Name1") %>
                                                                </td>
                                                                <td style="width: 25%">
                                                                    <asp:CheckBox ID="SecondchkId" runat="server" Visible='<%# (int)Eval("Id2") > 0 %>'></asp:CheckBox>
                                                                    <%--<asp:HiddenField ID="Id2" Value='<%#Eval("Id2")%>' runat="server" />--%>
                                                                    <asp:HiddenField ID="SecondBitValue" Value='<%#Eval("BitValue2")%>' runat="server" />
                                                                    <%# Eval("Name2") %>
                                                                </td>
                                                        </ItemTemplate>
                                                        <AlternatingItemTemplate>
                                                            <td style="width: 25%">
                                                                <asp:CheckBox ID="FirstchkId" runat="server"></asp:CheckBox>
                                                                <%--<asp:HiddenField ID="Id1" Value='<%#Eval("Id1")%>' runat="server" />--%>
                                                                <asp:HiddenField ID="FirstBitValue" Value='<%#Eval("BitValue1")%>' runat="server" />
                                                                <%# Eval("Name1") %>
                                                            </td>
                                                            <td style="width: 25%">
                                                                <asp:CheckBox ID="SecondchkId" runat="server" Visible='<%# (int)Eval("Id2") > 0 %>'></asp:CheckBox>
                                                                <%--<asp:HiddenField ID="Id2" Value='<%#Eval("Id2")%>' runat="server" />--%>
                                                                <asp:HiddenField ID="SecondBitValue" Value='<%#Eval("BitValue2")%>' runat="server" />
                                                                <%# Eval("Name2") %>
                                                            </td>
                                                            </tr>
                                                        </AlternatingItemTemplate>
                                                    </asp:Repeater>
                                                </tbody>
                                            </table>
                                            <div class="row">
                                                <div class="col-sm-4">
                                                    <label class="control-label" for="ddlDepartmentFunding">Department Funding</label>
                                                    <asp:DropDownList ID="ddlDepartmentFunding" runat="server" CssClass="form-control">
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-sm-1"></div>
                                                <div class="col-sm-3">
                                                    <label class="control-label" for="txtDeptFundOth">Other</label>
                                                    <input class="form-control" type="text" name="txtDeptFundOth" id="txtDeptFundOth" placeholder="Department Funding - Other" runat="Server" />
                                                </div>
                                                <div class="col-md-1"></div>
                                                <div class="col-sm-3">
                                                    <label class="control-label" for="txtFundingOther">Other:</label>
                                                    <input class="form-control" type="text" name="txtFundingOther" id="txtFundingOther" placeholder="Funding Source - Other" runat="Server" />
                                                    <br />
                                                </div>
                                                <div class="col-sm-2">
                                                    <input class="form-control hidden" type="text" name="txtFundingBitSum" id="txtFundingBitSum" runat="Server" />
                                                </div>
                                            </div>
                                            <br />
                                            <div class="row">
                                                <div id="divDeptFundMou">
                                                    <div class="col-sm-6">
                                                        <div class="col-sm-4">
                                                            <label for="chkDeptFundMouYes">Is this project supported by an MOU?</label>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:CheckBox ID="chkDeptFundMouYes" runat="server" Text="Yes" />
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:CheckBox ID="chkDeptFundMouNo" runat="server" Text="No" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <br />
                                        </div>
                                    </div>

                                    <hr />


                                    <div class="row">
                                        <div class="col-sm-2">
                                            <label class="control-label">Important deadlines</label>
                                        </div>
                                        <div class="col-sm-2">
                                            <div class='input-group date' id='dtpDueDate'>
                                                <span class="input-group-addon">
                                                    <span class="glyphicon glyphicon-calendar"></span>
                                                </span>
                                                <asp:TextBox ID="txtDueDate" runat="server" class="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="col-sm-4">
                                            <label class="control-label">QHS faculty/staff preference (if any)</label>
                                        </div>
                                        <div class="col-sm-4">
                                            <asp:DropDownList ID="ddlBiostat" runat="server" CssClass="form-control"></asp:DropDownList>
                                            <%--<input class="form-control nameahead" type="text" name="txtPreferBiostat"  onchange="updateId(this)"  />--%>
                                            <%--<asp:TextBox ID="txtPreferBiostat" runat="server" class="form-control"></asp:TextBox>--%>
                                        </div>
                                    </div>
                                    <div class="row offset2 hidden">
                                        <div class="col-md-2 text-right">
                                            <label class="control-label">QHS Faculty Staff Preference Id:</label>
                                        </div>
                                        <div class="col-md-1">
                                            <input class="form-control" type="text" name="txtPreferBiostatId" id="txtPreferBiostatId" runat="Server" />
                                        </div>
                                    </div>
                                    <br />


                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:CheckBox ID="chkCompleted" runat="server" Text="Request Completed" Enabled="false" />
                                        </div>

                                    </div>

                                    <div class="modal-footer">
                                        <asp:Label ID="lblResult" Visible="false" runat="server"></asp:Label>
                                        <%--<asp:Button ID="btnSave" runat="server" Text="Update" CssClass="btn btn-info" OnClientClick="return validateControl();" OnClick="btnSave_Click" />--%>
                                        <asp:Button ID="btnCreateProject" runat="server" Text="Create Project" CssClass="btn btn-dark" 
                                             OnClientClick="ClientSideClick(this)" OnClick="btnCreateProject_Click" 
                                            ValidationGroup="clientRequestFormValidations" CausesValidation="true"/>

                                        <button class="btn btn-info" data-dismiss="modal" aria-hidden="true" onclick="cleartQueryString()">Close</button>
                                    </div>

                                    <div class="hidden">
                                        <textarea id="textAreaDeptAffil" rows="3" runat="server"></textarea>
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
                    <table class="table table-striped table-hover table-bordered no-footer dataTable" id="tblClientSurvey">
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
    <script src="../Scripts/typeahead.jquery.min.js"></script>
    <script src="../Scripts/jquery.validate.js"></script>
    <script src="../Scripts/jquery.dataTables.min.js"></script>
    <script src="../Scripts/dataTables.bootstrap.min.js"></script>
    <script type="text/javascript">


        function pageLoad(sender, args) {

            $("#editModal").on('shown.bs.modal', function () {
                $('#editModal').scrollTop(0);
            });

            bindForm();

            $('#tblClientRequest').DataTable({
                "aoColumns": [
                    null,
                    null,
                    null,
                    { "bSortable": false },
                    { "bSortable": false },
                    { "bSortable": false },
                    { "bSortable": false }
                ],
               order: [[0, 'desc']]
            });

            /*
            $('#tblClientSurvey').DataTable({
                "aoColumns": [
                    null,
                    null,
                    { "bSortable": false },
                    { "bSortable": false },
                    { "bSortable": false },
                    { "bSortable": false },
                    { "bSortable": false }
                ]
            });*/

            var rqstId = GetURLParameter('ClientRequestId');

            if (typeof (rqstId) != 'undefined' && rqstId.length > 0) {
                $('#editModal').modal('show');
            }

            
            //$("#editModal").on('hidden.bs.modal', function () {
            //    $(this).find("input,textarea,select")
            //                .val('')
            //                .end()
            //           .find("input[type=checkbox]")
            //                .prop('checked', "")
            //                .end();
            //});
            

        }



        function bindForm() {


            // --> Initialize Other/Bitsum functionality.
            var tStudyArea = new checkboxTable.TableToggle(tblStudyArea);
            initializeCheckboxTable(tStudyArea, tblStudyArea);

            var tHealthData = new checkboxTable.TableToggle(tblHealthData);
            initializeCheckboxTable(tHealthData, tblHealthData);

            var tStudyType = new checkboxTable.TableToggle(tblStudyType);
            initializeCheckboxTable(tStudyType, tblStudyType);

            var tStudyPopulation = new checkboxTable.TableToggle(tblStudyPopulation);
            initializeCheckboxTable(tStudyPopulation, tblStudyPopulation);

            var tService = new checkboxTable.TableToggle(tblService);
            initializeCheckboxTable(tService, tblService);

            var tFunding = new checkboxTable.TableToggle(tblFunding);
            initializeCheckboxTable(tFunding, tblFunding);


            // --> Shows/hide "Other" field if dropdown selection is "Other".
            showHideOtherField($('#MainContent_ddlDegree'), $('#MainContent_txtDegreeOther'));

            // --> Toggles Yes/No checkboxes to make them single choice.
            $('#MainContent_chkJuniorPIYes').change(function () {
                $('#MainContent_chkJuniorPINo').prop('checked', false);
            });

            $('#MainContent_chkJuniorPINo').change(function () {
                $('#MainContent_chkJuniorPIYes').prop('checked', false);
            });

            $('#MainContent_chkDeptFundMouYes').change(function () {
                $('#MainContent_chkDeptFundMouNo').prop('checked', false);
            });

            $('#MainContent_chkDeptFundMouNo').change(function () {
                $('#MainContent_chkDeptFundMouYes').prop('checked', false);
            });

            // --> Toggles Yes/No checkboxes to make single choice,
            //     shows/hide mentor questions
            ToggleDiv($('#MainContent_chkMentorYes'), $('#divMentor'));

            $('#MainContent_chkMentorYes').change(function () {
                ToggleDiv($(this), $('#divMentor'));
                $('#MainContent_chkMentorNo').prop('checked', false);
            });

            $('#MainContent_chkMentorNo').change(function () {
                $('#MainContent_chkMentorYes').prop('checked', false);
                $('#divMentor').hide();
            });

            $('#MainContent_chkPilotYes').change(function () {
                $('#MainContent_chkPilotNo').prop('checked', false);
            });

            $('#MainContent_chkPilotNo').change(function () {
                $('#MainContent_chkPilotYes').prop('checked', false);
            })

            ToggleDiv4($('#MainContent_rptStudyPopulation_chkId_0'),
                       $('#MainContent_rptStudyPopulation_chkId_1'),
                       $('#MainContent_rptStudyPopulation_chkId_2'),
                       $('#MainContent_rptStudyPopulation_chkId_3'),
                       $('#divHealthDisparity'));

            $('#MainContent_chkHealthDisparityYes').change(function () {
                if (this.checked) {
                    $('#MainContent_chkHealthDisparityNo').prop('checked', false);
                    $('#MainContent_chkHealthDisparityNA').prop('checked', false);
                }
            });

            $('#MainContent_chkHealthDisparityNo').change(function () {
                if (this.checked) {
                    $('#MainContent_chkHealthDisparityYes').prop('checked', false);
                    $('#MainContent_chkHealthDisparityNA').prop('checked', false);
                }
            });

            $('#MainContent_chkHealthDisparityNA').change(function () {
                if (this.checked) {
                    $('#MainContent_chkHealthDisparityYes').prop('checked', false);
                    $('#MainContent_chkHealthDisparityNo').prop('checked', false);
                }
            });

            ToggleDiv($('#MainContent_chkProposalYes'), $('#divGrantProposal'));

            $('#MainContent_chkProposalYes').change(function () {
                ToggleDiv($(this), $('#divGrantProposal'));
                $('#MainContent_chkProposalNo').prop('checked', false);
            });

            $('#MainContent_chkProposalNo').change(function () {
                $('#MainContent_chkProposalYes').prop('checked', false);
                $('#divGrantProposal').hide();
            });


            ToggleDiv($('#MainContent_chkIsUHPilotGrantYes'), $('#divGrantProposalFundingAgency'));
            ToggleDiv($('#MainContent_chkIsUHPilotGrantNo'), $('#divGrantProposalFundingAgency'));

            $("#divGrantProposal").on('click',
                'input[type="checkbox"]',
                function () {

                    if ($(this).is($('#MainContent_chkIsUHPilotGrantYes'))) {
                        ToggleDiv($(this), $('#divGrantProposalFundingAgency'));
                    }

                    if ($(this).is($('#MainContent_chkIsUHPilotGrantNo'))) {
                        ToggleDiv($(this), $('#divGrantProposalFundingAgency'));
                    }

                    if ($(this).is(':checked')){
                        if ($(this).val() == 1) 
                            $(this).next().removeAttr("checked");
                        if ($(this).val() == 0)
                            $(this).prev().removeAttr("checked");
                    }

                });

            $('#tblStudyPopulation').on('click',
                'input[type="checkbox"]',
                function () {
                    if ($(this).is($('#MainContent_rptStudyPopulation_chkId_0')) // Native Hawaiians,
                        //-- Pacific Islanders, and Filipinos
                        || $(this).is($('#MainContent_rptStudyPopulation_chkId_1')) // Hawaii Populations
                        || $(this).is($('#MainContent_rptStudyPopulation_chkId_2')) // U.S. Populations
                        || $(this).is($('#MainContent_rptStudyPopulation_chkId_3')) // International Populations

                    ) {
                        ToggleDiv($(this), $('#divHealthDisparity'));
                    }

                });

            // Disable all checkboxes, dropdowns, and input fields (not buttons) if request is completed!
            if ($('#MainContent_chkCompleted').prop('checked')) {

                //$('#editModal').css('background-color', 'green');

                $('#editModal').find("input[type=checkbox],input[type=text],textarea,select")
                    .prop('disabled', true)
                    .end()
                    //.find("input[type=checkbox],input[type=radio]")
                    //.prop('checked', "")
                    .end();
            } 


        }

        // -- Initializes Checkbox Table and bitsum fields to determine selections.
        function initializeCheckboxTable(t, tableId) {

            t.Click(tableId);

            var _table = $(tableId),
                _textOther = _table.next().find(":input[name$='Other']"),
                _textBitSum = _table.next().find(":input[name$='BitSum']"),
                _id = _table.attr('id');

            _table.on('click', 'input[type="checkbox"]', function () {
                t.Click(this.closest('table'));
            });

            //_table.find('td').each(function () {
            //    $(this).find(":input[name$='chkId']").prop('checked', false);
            //})

            //_textOther.val('');
            //_textOther.parent().closest('div').hide();

        }

        // -- Initializes and creates functionality for checkbox tables
        var checkboxTable = checkboxTable || {};

        checkboxTable.TableToggle = function (tableId) {
            var _table = $(tableId),
                _textOther = _table.next().find(":input[name$='Other']"),
                _textBitSum = _table.next().find(":input[name$='BitSum']"),
                _id = _table.attr('id');


            // --> If "Other" checkbox is checked, then the "other" field is displayed.
            //     The BitSum total is added from the bit value of the checkbox.
            return {
                Click: function (e) {
                    var _bitSum = 0;

                    _table.find('td').each(function () {
                        var _checkBox = $(this).find(":input[name$='chkId']"),
                            _bitValue = $(this).find(":input[name$='BitValue']").val(),
                            _name = $(this).eq(0).text().trim();



                        if (_name == 'Other' || _name == 'International Populations') {

                            if (_checkBox.is(':checked')) {
                                _textOther.parent().closest('div').show();
                            }
                            else {
                                _textOther.val('');
                                _textOther.parent().closest('div').hide();
                            }


                        }


                        // --> Show "Department Funding" dropdown options. ||||||
                        if (_name == 'Department Funding' && _id == 'tblFunding') {

                            if (_checkBox.is(':checked')) {
                                $('#MainContent_ddlDepartmentFunding').show();
                                $('#MainContent_ddlDepartmentFunding').parent().find('label').show();
                                $('#divDeptFundMou').hide();


                                $('#MainContent_ddlDepartmentFunding').change(function () {
                                    var selectedText = $("option:selected", this).text();

                                    if (selectedText == "Other") {
                                        $("#MainContent_txtDeptFundOth").show();
                                        $("#MainContent_txtDeptFundOth").parent().find('label').show();

                                        $('#MainContent_chkDeptFundMouYes').prop('checked', false);
                                        $('#MainContent_chkDeptFundMouNo').prop('checked', false);
                                        $('#divDeptFundMou').hide();

                                    } else if (selectedText == "School of Nursing & Dental Hygiene") {
                                        $('#divDeptFundMou').show();

                                        $('#MainContent_txtDeptFundOth').val('');
                                        $('#MainContent_txtDeptFundOth').hide();
                                        $('#MainContent_txtDeptFundOth').parent().find('label').hide();
                                    } else {
                                        $('#MainContent_txtDeptFundOth').val('');
                                        $('#MainContent_txtDeptFundOth').hide();
                                        $('#MainContent_txtDeptFundOth').parent().find('label').hide();

                                        $('#MainContent_chkDeptFundMouYes').prop('checked', false);
                                        $('#MainContent_chkDeptFundMouNo').prop('checked', false);
                                        $('#divDeptFundMou').hide();
                                    }

                                });

                                if ($("option:selected", $('#MainContent_ddlDepartmentFunding')).text() == "Other") {
                                    $("#MainContent_txtDeptFundOth").show();
                                    $("#MainContent_txtDeptFundOth").parent().find('label').show();
                                } else if ($("option:selected", $("#MainContent_ddlDepartmentFunding")).text() == "School of Nursing & Dental Hygiene") {
                                    $("#divDeptFundMou").show();

                                    $("#MainContent_txtDeptFundOth").val('');
                                    $("#MainContent_txtDeptFundOth").hide();
                                    $("#MainContent_txtDeptFundOth").parent().find('label').hide();
                                } else {
                                    $("#MainContent_txtDeptFundOth").val('');
                                    $("#MainContent_txtDeptFundOth").hide();
                                    $("#MainContent_txtDeptFundOth").parent().find('label').hide();

                                    $("#MainContent_chkDeptFundMouYes").prop('checked', false);
                                    $("#MainContent_chkDeptFundMouNo").prop('checked', false);
                                    $("#divDeptFundMou").hide();
                                }

                            }
                            else {
                                $("#MainContent_ddlDepartmentFunding").val('');
                                $("#MainContent_ddlDepartmentFunding").hide();
                                $("#MainContent_ddlDepartmentFunding").parent().find('label').hide();

                                $("#MainContent_txtDeptFundOth").val('');
                                $("#MainContent_txtDeptFundOth").hide();
                                $("#MainContent_txtDeptFundOth").parent().find('label').hide();

                                $('#MainContent_chkDeptFundMouYes').prop('checked', false);
                                $('#MainContent_chkDeptFundMouNo').prop('checked', false);
                                $('#divDeptFundMou').hide();

                            }

                        }
                        //                                                 ||||||

                        // --> Adds bitsum of current checkbox field
                        if (_checkBox.is(':checked')) {
                            _bitSum += parseInt(_bitValue, 10);
                        }

                        // --> Deselects all other checkboxes in current checkbox
                        //     if N/A is selected.
                        if (_name == 'N/A' && _checkBox.is(':checked')) {
                            ToggleTable(_table, true);
                        }
                        else if (_bitSum == 0) {
                            ToggleTable(_table, false);
                        }

                    });

                    //--> Puts BitSum total into hidden BitSum field.
                    $(_textBitSum).val(_bitSum);


                }
            }
        }

        // -- Disables all checkboxes if checkbox choice "N/A" is selected.
        function ToggleTable(tbl, isNA) {
            tbl.find('td').each(function () {
                var _checkBox = $(this).find(":input[name$='chkId']"),
                    _bitValue = $(this).find(":input[name$='BitValue']"),
                    _name = $(this).eq(0).text().trim();

                if (isNA) {
                    if (_name != "N/A") {
                        _checkBox.prop("checked", false);
                        _checkBox.prop("disabled", true);
                    }
                } else {
                    _checkBox.prop("disabled", false);
                }

            });
        }

        // ToggleDiv - shows section if check; otherwise remain hidden.
        function ToggleDiv(checkBox, theDiv) {
            if (checkBox.is(":checked")) {
                theDiv.show();
            }
            else {
                theDiv.hide();
            }
        }

        // ToggleDiv4 - Same functionality as 'ToggleDiv' with ability to handle four checkboxes.
        function ToggleDiv4(checkBox1, checkBox2, checkBox3, checkBox4, theDiv) {
            if (checkBox1.is(":checked")
                || checkBox2.is(":checked")
                || checkBox3.is(":checked")
                || checkBox4.is(":checked"))
                theDiv.show();
            else
                theDiv.hide();
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


        // -- Given dropdown trigger, shows or hides the text "Other" field if "Other" is selected in the dropdown.-- \\
        function showHideOtherField(dropdown, otherField) {
            otherField.val('');
            otherField.hide();
            otherField.parent().find('label').hide()

            dropdown.on('change', function () {

                // (for testing purposes) --> alert("This value has been changed!");

                var selectedText = $("option:selected", this).text();

                if (selectedText == "Other") {
                    otherField.show();
                    otherField.parent().find('label').show();
                }
                else {
                    otherField.val('');
                    otherField.hide();
                    otherField.parent().find('label').hide();
                }
            });
        }

        /// NAME: GetURLParameter(sParam)
        ///
        /// FUNCTION: Grabs URL after ampersand (&) symbol, checks
        ///           the parameter before the equals (=) symbol is the same as given
        ///           given parameter, and returns the value after the "=".
        ///
        /// PARAMETERS: sParam - given parameter.
        ///
        /// RETURNS: Value after the equals (=) sign (the Collaborative Center abbreviation).
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

        function cleartQueryString() {
            if (window.location.href.indexOf('?') > 0)
                window.location.href = window.location.href.split('?')[0];
            
            // Also moved "Clear form" functionality here.
             $("#editModal").find("input[type=text],textarea,select")
                .val('')
                .end()
                .find("input[type=checkbox],input[type=radio]")
                .prop('checked', "")
                .end();
            //$("#editModal").find("input,textarea,select").val('');
                
            //$("#editModal").find("input[type=checkbox],input[type=radio]").prop('checked', "");
                

        }

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


        function updateId(txtbox) {
            if (txtbox.Value == '') {
                if (txtbox.id == 'MainContent_txtDept') {
                    $('#MainContent_txtDeptAffilId').val('');
                } else if (txtbox.id == 'MainContent_txtPreferBiostat') {
                    $('#MainContent_txtPreferBiostatId').val('');
                }
            }
        }

        var substringMatcher = function (strs) {
            return function findMatches(q, cb) {
                // an array that will be populated with substring matches
                var matches = [];

                // regex used to determine if a string contains the substring `q`
                var substrRegex = new RegExp(q, 'i');

                // iterate through the pool of strings and for any string that
                // contains the substring `q`, add it to the `matches` array
                $.each(strs, function (i, str) {
                    if (substrRegex.test(str)) {
                        matches.push(str);
                    }
                });

                cb(matches);
            };
        };

        var names = [];
        var map = {};

        var data = "";//JSON.parse($('#<%=textAreaDeptAffil.ClientID%>').val());

        $.each(data, function (i, name) {
            map[name.Name] = name;
            names.push(name.Name);
        });

        $('.typeahead').typeahead({
            hint: true,
            highlight: true,
            minLength: 1
        },
            {
                name: 'name',
                limit: 10,
                source: substringMatcher(names),
                updater: function (item) {
                    alert(map[item].Name);
                }
            }).on('typeahead:selected', function (event, selection) {

                $('#<%=txtDeptAffilId.ClientID%>').val(map[selection].Id);


            });

    </script>
</asp:Content>
