<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AcademicForm.aspx.cs" Inherits="ProjectManagement.Admin.AcademicForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
    <div class="loader"></div>   
    
    <div class="panel panel-default">
        <div class="panel-heading"><b>Academic</b></div>
        <div class="panel-body">
            <div class="row">
                <div class="col-sm-2 text-right">
                    <label class="control-label" for="txtTitle">Staff:</label>
                </div>
                <div class="col-sm-8">
                    <asp:Repeater ID="rptBiostat" runat="server">
                        <HeaderTemplate>
                            <table class="table table-striped table-hover table-bordered" id="tblBiostat">
                                <%--<tr>
                                        <th>Biostat</th>
                                        <th>Biostat</th>
                                    </tr>--%>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="FirstchkId" runat="server"></asp:CheckBox>
                                    <%--<asp:HiddenField ID="Id1" Value='<%#Eval("Id1")%>' runat="server" />--%>
                                    <asp:HiddenField ID="FirstBitValue" Value='<%#Eval("BitValue1")%>' runat="server" />
                                    <%# Eval("Name1") %>
                                </td>
                                <td>
                                    <asp:CheckBox ID="SecondchkId" runat="server" Visible='<%# (long)Eval("Id2") > 0 %>'></asp:CheckBox>
                                    <%--<asp:HiddenField ID="Id2" Value='<%#Eval("Id2")%>' runat="server" />--%>
                                    <asp:HiddenField ID="SecondBitValue" Value='<%#Eval("BitValue2")%>' runat="server" />
                                    <%# Eval("Name2") %>
                                </td>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <td>
                                <asp:CheckBox ID="FirstchkId" runat="server"></asp:CheckBox>
                                <%--<asp:HiddenField ID="Id1" Value='<%#Eval("Id1")%>' runat="server" />--%>
                                <asp:HiddenField ID="FirstBitValue" Value='<%#Eval("BitValue1")%>' runat="server" />
                                <%# Eval("Name1") %>
                            </td>
                            <td>
                                <asp:CheckBox ID="SecondchkId" runat="server" Visible='<%# (long)Eval("Id2") > 0 %>'></asp:CheckBox>
                                <%--<asp:HiddenField ID="Id2" Value='<%#Eval("Id2")%>' runat="server" />--%>
                                <asp:HiddenField ID="SecondBitValue" Value='<%#Eval("BitValue2")%>' runat="server" />
                                <%# Eval("Name2") %>
                            </td>
                            </tr>
                        </AlternatingItemTemplate>
                        <%--<ItemTemplate>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chkId" runat="server"></asp:CheckBox>
                                                <asp:HiddenField ID="Id" Value ='<%#Eval("Id2")%>' runat="server" />
                                                <%# Eval("Biostat2") %>
                                            </td>
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                            <td>
                                                <asp:CheckBox ID="chkId" runat="server"></asp:CheckBox>
                                                <asp:HiddenField ID="Id" Value ='<%#Eval("Id2")%>' runat="server" />
                                                <%# Eval("Biostat2") %>
                                            </td>
                                        </tr>
                                    </AlternatingItemTemplate>--%>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                    <div>
                        <input class="form-control hidden" type="text" name="txtBiostatBitSum" id="txtBiostatBitSum" runat="Server" />
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-2 text-right">
                    <label class="control-label" for="txtTitle">Academic Type:</label>
                </div>
                <div class="col-sm-3">
                    <asp:DropDownList ID="ddlAcademicType" runat="server" CssClass="form-control" AutoPostBack="false">
                    </asp:DropDownList>
                </div>
                <div class="col-sm-5 hidden">
                    <asp:DropDownList ID="ddlEventHdn" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-sm-2 text-right">
                    <label class="control-label" for="txtTitle">Academic Event:</label>
                </div>
                <div class="col-sm-8">
                    <asp:DropDownList ID="ddlEvent" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlEvent_Changed" AutoPostBack="True">
                    </asp:DropDownList>
                </div>
            </div>
            <br />


            <div class="row" id="div1">
                <div class="col-sm-2 text-right">
                    <label class="control-label">Event details:</label>
                </div>
                <div class="col-sm-8">
                    <div class="panel panel-default">
                        <div class="panel-heading">Seminar/Workshop/Lecture/Training Given</div>
                        <div class="panel-body">
                            <div class="row">

                                <label class="col-sm-2 control-label" for="txtOrganization">Organization:</label>

                                <div class="col-sm-9">
                                    <input class="form-control" type="text" name="txtOrganization" id="txtOrganization" placeholder="Organization" runat="server" />
                                </div>
                            </div>
                            <br />
                            <div class="row">

                                <label class="col-sm-2 control-label" for="txtTitle">Title:</label>

                                <div class="col-sm-9">
                                    <input class="form-control" type="text" name="txtTitle" id="txtTitle" placeholder="Title" runat="server" />
                                </div>
                            </div>
                            <br />
                            <div class="row">

                                <label class="col-sm-2 control-label" for="txtTitle">Audience:</label>

                                <div class="col-sm-9">
                                    <input class="form-control" type="text" name="txtAudience" id="txtAudience" placeholder="Audience" runat="server" />
                                </div>
                            </div>
                            <br />
                            <div class="row">

                                <label class="col-sm-2 control-label" for="txtStartDate">Start date:</label>

                                <div class="col-sm-3">
                                    <div class='input-group date' id='dtpStartDate'>
                                        <span class="input-group-addon">
                                            <span class="glyphicon glyphicon-calendar"></span>
                                        </span>
                                        <asp:TextBox ID="txtStartDate" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                </div>

                                <label class="col-sm-3 control-label" for="txtEndDate">End date:</label>

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
                            <div class="row">

                                <label class="col-sm-2 control-label" for="txtNumOfAttendees">Num of attendees:</label>

                                <div class="col-sm-3">
                                    <input class="form-control" type="text" name="txtNumOfAttendees" id="txtNumOfAttendees" placeholder="NumOfAttendees" runat="server" />
                                </div>

                                <label class="col-sm-3 control-label" for="txtCourseNum">Course Number, if part of course:</label>

                                <div class="col-sm-3">
                                    <input class="form-control" type="text" name="txtCourseNum" id="txtCourseNum" placeholder="CourseNum" runat="server" />
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>

            <div class="row" id="div2">
                <div class="col-sm-2 text-right">
                    <label class="control-label">Event details:</label>
                </div>
                <div class="col-sm-8">
                    <div class="panel panel-default">
                        <div class="panel-heading">Teaching/Courses</div>
                        <div class="panel-body">
                            <div class="row">

                                <label class="col-sm-3 control-label" for="ddlSemester">Semester:</label>

                                <div class="col-sm-3">
                                    <asp:DropDownList ID="ddlSemester" runat="server" CssClass="form-control" AutoPostBack="false">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <label class="col-sm-3 control-label" for="txtYear">Year:</label>
                                <div class="col-sm-3">
                                    <input class="form-control" type="text" name="txtYear" id="txtYear" placeholder="Year" runat="server" />
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <label class="col-sm-3 control-label" for="txtCourseTitle">Course Title:</label>
                                <div class="col-sm-9">
                                    <input class="form-control" type="text" name="txtCourseTitle" id="txtCourseTitle" placeholder="Course Title" runat="server" />
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <label class="col-sm-3 control-label" for="txtCourseNumTeaching">Course Number, if part of course:</label>
                                <div class="col-sm-3">
                                    <input class="form-control" type="text" name="txtCourseNumTeaching" id="txtCourseNumTeaching" placeholder="Course Number" runat="server" />
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <label class="col-sm-3 control-label" for="txtCourseTitle">Number of credits:</label>
                                <div class="col-sm-3">
                                    <input class="form-control" type="text" name="txtNumOfCredits" id="txtNumOfCredits" placeholder="NumOfCredits" runat="server" />
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <label class="col-sm-3 control-label" for="txtCourseTitle">Number of students:</label>
                                <div class="col-sm-3">
                                    <input class="form-control" type="text" name="txtNumOfStudents" id="txtNumOfStudents" placeholder="NumOfStudents" runat="server" />
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>

            <div class="row" id="div3">
                <div class="col-sm-2 text-right">
                    <label class="control-label">Event details:</label>
                </div>
                <div class="col-sm-8">
                    <div class="panel panel-default">
                        <div class="panel-heading">Dissertation/Thesis Committee</div>
                        <div class="panel-body">
                            <div class="row">

                                <label class="col-sm-3 control-label" for="ddlCommitteeType">Committee type:</label>

                                <div class="col-sm-3">
                                    <asp:DropDownList ID="ddlCommitteeType" runat="server" CssClass="form-control" AutoPostBack="false">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <label class="col-sm-3 control-label" for="txtDepartment">Department:</label>
                                <div class="col-sm-3">
                                    <input class="form-control" type="text" name="txtDepartment" id="txtDepartment" runat="server" />
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <label class="col-sm-3 control-label" for="txtStudentName">Student name:</label>
                                <div class="col-sm-3">
                                    <input class="form-control" type="text" name="txtStudentName" id="txtStudentName" runat="server" />
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <label class="col-sm-3 control-label" for="txtAdvisorName">Student's Advisor name:</label>
                                <div class="col-sm-3">
                                    <input class="form-control" type="text" name="txtAdvisorName" id="txtAdvisorName" runat="server" />
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <label class="col-sm-3 control-label" for="txtProjectTitle">Project title:</label>
                                <div class="col-sm-9">
                                    <input class="form-control" type="text" name="txtProjectTitle" id="txtProjectTitle" runat="server" />
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <label class="col-sm-3 control-label" for="ddlStartSemester">Start Semester:</label>
                                <div class="col-sm-3">
                                    <asp:DropDownList ID="ddlStartSemester" runat="server" CssClass="form-control" AutoPostBack="false">
                                    </asp:DropDownList>
                                </div>
                                <label class="col-sm-3 control-label" for="txtStartYear">Start year:</label>
                                <div class="col-sm-3">
                                    <input class="form-control" type="text" name="txtStartYear" id="txtStartYear" runat="server" />
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <label class="col-sm-3 control-label" for="ddlEndSemester">End Semester:</label>
                                <div class="col-sm-3">
                                    <asp:DropDownList ID="ddlEndSemester" runat="server" CssClass="form-control" AutoPostBack="false">
                                    </asp:DropDownList>
                                </div>
                                <label class="col-sm-3 control-label" for="txtEndYear">End year:</label>
                                <div class="col-sm-3">
                                    <input class="form-control" type="text" name="txtEndYear" id="txtEndYear" runat="server" />
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>

            <div class="row" id="div4">
                <div class="col-sm-2 text-right">
                    <label class="control-label">Event details:</label>
                </div>
                <div class="col-sm-8">
                    <div class="panel panel-default">
                        <div class="panel-heading">Panel/Committee</div>
                        <div class="panel-body">
                            <div class="row">
                                <label class="col-sm-2 control-label" for="txtPanelOrg">Organization:</label>
                                <div class="col-sm-9">
                                    <input class="form-control" type="text" name="txtPanelOrg" id="txtPanelOrg" runat="server" />
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <label class="col-sm-2 control-label" for="txtPanelName">Name of committee:</label>
                                <div class="col-sm-9">
                                    <input class="form-control" type="text" name="txtPanelName" id="txtPanelName" runat="server" />
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <label class="col-sm-2 control-label" for="txtPanelStartDate">Start date:</label>
                                <div class="col-sm-3">
                                    <div class='input-group date' id='dtpPanelStartDate'>
                                        <span class="input-group-addon">
                                            <span class="glyphicon glyphicon-calendar"></span>
                                        </span>
                                        <asp:TextBox ID="txtPanelStartDate" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                </div>

                                <label class="col-sm-3 control-label" for="txtPanelEndDate">End date:</label>

                                <div class="col-sm-3">
                                    <div class='input-group date' id='dtpPanelEndDate'>
                                        <span class="input-group-addon">
                                            <span class="glyphicon glyphicon-calendar"></span>
                                        </span>
                                        <asp:TextBox ID="txtPanelEndDate" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <label class="col-sm-2 control-label" for="ddlPanelCommittee">Role in committee:</label>
                                <div class="col-sm-3">
                                    <asp:DropDownList ID="ddlPanelCommittee" runat="server" CssClass="form-control" AutoPostBack="false">
                                    </asp:DropDownList>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>

            <div class="row" id="div5">
                <div class="col-sm-2 text-right">
                    <label class="control-label">Event details:</label>
                </div>
                <div class="col-sm-8">
                    <div class="panel panel-default">
                        <div class="panel-heading">Journal Review</div>
                        <div class="panel-body">
                            <div class="row">
                                <label class="col-sm-2 control-label" for="txtJournalName">Journal name:</label>
                                <div class="col-sm-9">
                                    <input class="form-control" type="text" name="txtJournalName" id="txtJournalName" runat="server" />
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <label class="col-sm-2 control-label" for="txtJournalDate">Review date:</label>
                                <div class="col-sm-3">
                                    <div class='input-group date' id='dtpJournalDate'>
                                        <span class="input-group-addon">
                                            <span class="glyphicon glyphicon-calendar"></span>
                                        </span>
                                        <asp:TextBox ID="txtJournalDate" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row" id="div6">
                <div class="col-sm-2 text-right">
                    <label class="control-label">Event details:</label>
                </div>
                <div class="col-sm-8">
                    <div class="panel panel-default">
                        <div class="panel-heading">Grant Review</div>
                        <div class="panel-body">
                            <div class="row">
                                <label class="col-sm-2 control-label" for="ddlGrantAgency">Agency:</label>
                                <div class="col-sm-3">
                                    <asp:DropDownList ID="ddlGrantAgency" runat="server" CssClass="form-control" AutoPostBack="false">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <label class="col-sm-2 control-label" for="txtGrantDate">Review date:</label>
                                <div class="col-sm-3">
                                    <div class='input-group date' id='dtpGrantDate'>
                                        <span class="input-group-addon">
                                            <span class="glyphicon glyphicon-calendar"></span>
                                        </span>
                                        <asp:TextBox ID="txtGrantDate" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row" id="div7">
                <div class="col-sm-2 text-right">
                    <label class="control-label">Event details:</label>
                </div>
                <div class="col-sm-8">
                    <div class="panel panel-default">
                        <div class="panel-heading">Honor/Award</div>
                        <div class="panel-body">
                            <div class="row">
                                <label class="col-sm-2 control-label" for="txtAwardAgency">Awarding Agency:</label>
                                <div class="col-sm-9">
                                    <input class="form-control" type="text" name="txtAwardAgency" id="txtAwardAgency" runat="server" />
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <label class="col-sm-2 control-label" for="txtAwardTitle">Award title:</label>
                                <div class="col-sm-9">
                                    <input class="form-control" type="text" name="txtAwardTitle" id="txtAwardTitle" runat="server" />
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <label class="col-sm-2 control-label" for="txtAwardDate">Award date:</label>
                                <div class="col-sm-3">
                                    <div class='input-group date' id='dtpAwardDate'>
                                        <span class="input-group-addon">
                                            <span class="glyphicon glyphicon-calendar"></span>
                                        </span>
                                        <asp:TextBox ID="txtAwardDate" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row" id="div8">
                <div class="col-sm-2 text-right">
                    <label class="control-label">Event details:</label>
                </div>
                <div class="col-sm-8">
                    <div class="panel panel-default">
                        <div class="panel-heading">Professional Training</div>
                        <div class="panel-body">
                            <div class="row">
                                <label class="col-sm-2 control-label" for="txtTrainingProvider">Provider:</label>
                                <div class="col-sm-9">
                                    <input class="form-control" type="text" name="txtTrainingProvider" id="txtTrainingProvider" runat="server" />
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <label class="col-sm-2 control-label" for="ddlTrainingType">Training type:</label>
                                <div class="col-sm-3">
                                    <asp:DropDownList ID="ddlTrainingType" runat="server" CssClass="form-control" AutoPostBack="false">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <label class="col-sm-2 control-label" for="txtTrainingTitle">Training title:</label>
                                <div class="col-sm-9">
                                    <input class="form-control" type="text" name="txtTrainingTitle" id="txtTrainingTitle" runat="server" />
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <label class="col-sm-2 control-label" for="txtTrainingDate">Training date:</label>
                                <div class="col-sm-3">
                                    <div class='input-group date' id='dtpTrainingDate'>
                                        <span class="input-group-addon">
                                            <span class="glyphicon glyphicon-calendar"></span>
                                        </span>
                                        <asp:TextBox ID="txtTrainingDate" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <label class="col-sm-2 control-label" for="txtAwardTitle">Location:</label>
                                <div class="col-sm-5">
                                    <input class="form-control" type="text" name="txtTrainingloc" id="txtTrainingloc" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row" id="div9">
                <div class="col-sm-2 text-right">
                    <label class="control-label">Event details:</label>
                </div>
                <div class="col-sm-8">
                    <div class="panel panel-default">
                        <div class="panel-heading">Mentor for K awards and other grants</div>
                        <div class="panel-body">
                            <div class="row">
                                <label class="col-sm-2 control-label" for="txtMentorTitle">Project title:</label>
                                <div class="col-sm-9">
                                    <input class="form-control" type="text" name="txtMentorTitle" id="txtMentorTitle" runat="server" />
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <label class="col-sm-2 control-label" for="txtMenteeName">Mentee name:</label>
                                <div class="col-sm-5">
                                    <input class="form-control" type="text" name="txtMenteeName" id="txtMenteeName" runat="server" />
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <label class="col-sm-2 control-label" for="txtMenteeAffil">Mentee affiliation:</label>
                                <div class="col-sm-9">
                                    <input class="form-control" type="text" name="txtMenteeAffil" id="txtMenteeAffil" runat="server" />
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <label class="col-sm-2 control-label" for="txtMentorStartDate">Start date:</label>
                                <div class="col-sm-3">
                                    <div class='input-group date' id='dtpMentorStartDate'>
                                        <span class="input-group-addon">
                                            <span class="glyphicon glyphicon-calendar"></span>
                                        </span>
                                        <asp:TextBox ID="txtMentorStartDate" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <label class="col-sm-2 control-label" for="txtMentorStartDate">End date:</label>
                                <div class="col-sm-3">
                                    <div class='input-group date' id='dtpMentorEndDate'>
                                        <span class="input-group-addon">
                                            <span class="glyphicon glyphicon-calendar"></span>
                                        </span>
                                        <asp:TextBox ID="txtMentorEndDate" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row" id="div10">
                <div class="col-sm-2 text-right">
                    <label class="control-label">Event details:</label>
                </div>
                <div class="col-sm-8">
                    <div class="panel panel-default">
                        <div class="panel-heading">Data Safety Monitoring Committee</div>
                        <div class="panel-body">
                            <div class="row">
                                <label class="col-sm-2 control-label" for="txtSafeyTitle">Project title:</label>
                                <div class="col-sm-9">
                                    <input class="form-control" type="text" name="txtSafeyTitle" id="txtSafeyTitle" runat="server" />
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <label class="col-sm-2 control-label" for="txtPI">PI:</label>
                                <div class="col-sm-5">
                                    <input class="form-control" type="text" name="txtPI" id="txtPI" runat="server" />
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <label class="col-sm-2 control-label" for="txtMenteeAffil">PI affiliation:</label>
                                <div class="col-sm-9">
                                    <input class="form-control" type="text" name="txtPIAffil" id="txtPIAffil" runat="server" />
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <label class="col-sm-2 control-label" for="txtMentorStartDate">Start date:</label>
                                <div class="col-sm-3">
                                    <div class='input-group date' id='dtpSafetyStartDate'>
                                        <span class="input-group-addon">
                                            <span class="glyphicon glyphicon-calendar"></span>
                                        </span>
                                        <asp:TextBox ID="txtSafetyStartDate" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <label class="col-sm-2 control-label" for="txtMentorStartDate">End date:</label>
                                <div class="col-sm-3">
                                    <div class='input-group date' id='dtpSafetyEndDate'>
                                        <span class="input-group-addon">
                                            <span class="glyphicon glyphicon-calendar"></span>
                                        </span>
                                        <asp:TextBox ID="txtSafetyEndDate" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row" id="div11">
                <div class="col-sm-2 text-right">
                    <label class="control-label">Event details:</label>
                </div>
                <div class="col-sm-8">
                    <div class="panel panel-default">
                        <div class="panel-heading">Other</div>
                        <div class="panel-body">
                            <div class="row">
                                <label class="col-sm-2 control-label" for="txtOtherTitle">Title:</label>
                                <div class="col-sm-9">
                                    <input class="form-control" type="text" name="txtOtherTitle" id="txtOtherTitle" runat="server" />
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <label class="col-sm-2 control-label" for="txtOtherDesc">Description:</label>
                                <div class="col-sm-9">
                                    <input class="form-control" type="text" name="txtOtherDesc" id="txtOtherDesc" runat="server" />
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <label class="col-sm-2 control-label" for="txtOtherDate">Date:</label>
                                <div class="col-sm-3">
                                    <div class='input-group date' id='dtpOtherDate'>
                                        <span class="input-group-addon">
                                            <span class="glyphicon glyphicon-calendar"></span>
                                        </span>
                                        <asp:TextBox ID="txtOtherDate" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row" id="div12">
                <div class="col-sm-2 text-right">
                    <label class="control-label">Event details:</label>
                </div>
                <div class="col-sm-8">
                    <div class="panel panel-default">
                        <div class="panel-heading">Mentor for student</div>
                        <div class="panel-body">
                            <div class="row">
                                <label class="col-sm-2 control-label" for="txtStudentMentorStudentName">Student name:</label>
                                <div class="col-sm-7">
                                    <input class="form-control" type="text" name="txtStudentMentorStudentName" id="txtStudentMentorStudentName" runat="server" />
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <label class="col-sm-2 control-label" for="txtStudentMentorDepartment">Department:</label>
                                <div class="col-sm-7">
                                    <input class="form-control" type="text" name="txtStudentMentorDepartment" id="txtStudentMentorDepartment" runat="server" />
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <label class="col-sm-2 control-label" for="txtStudentMentorTitle">Title:</label>
                                <div class="col-sm-7">
                                    <input class="form-control" type="text" name="txtStudentMentorTitle" id="txtStudentMentorTitle" runat="server" />
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <label class="col-sm-2 control-label" for="txtStudentMentorDesc">Description:</label>
                                <div class="col-sm-9">
                                    <input class="form-control" type="text" name="txtStudentMentorDesc" id="txtStudentMentorDesc" runat="server" />
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <label class="col-sm-2 control-label" for="txtStudentMentorStartDate">Start date:</label>
                                <div class="col-sm-3">
                                    <div class='input-group date' id='dtpStudentMentorStartDate'>
                                        <span class="input-group-addon">
                                            <span class="glyphicon glyphicon-calendar"></span>
                                        </span>
                                        <asp:TextBox ID="txtStudentMentorStartDate" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <label class="col-sm-2 control-label" for="txtMentorStartDate">End date:</label>
                                <div class="col-sm-3">
                                    <div class='input-group date' id='dtpStudentMentorEndDate'>
                                        <span class="input-group-addon">
                                            <span class="glyphicon glyphicon-calendar"></span>
                                        </span>
                                        <asp:TextBox ID="txtStudentMentorEndDate" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                           
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-sm-2 text-right">
                    <label class="control-label" for="txtComments">Comments:</label>
                </div>
                <div class="col-sm-8">
                    <textarea class="form-control noresize" rows="5" name="txtComments" id="txtComments" runat="Server"></textarea>
                </div>
            </div>

            <hr />
            <%--<br />--%>

            <div class="row">
                <div class="col-sm-2"></div>
                <div class="col-sm-2">
                    <asp:Button ID="btnSave" runat="server" Text="Submit" CssClass="btn btn-info" OnClientClick="return ValidateControl(this);" OnClick="btnSave_Click" />
                </div>
            </div>


        </div>
    </div>      

    <script type="text/javascript">
        $(document).ready(function () {
            $(".loader").fadeOut("slow");

            toggleDiv($("#MainContent_ddlAcademicType").val());

            bindEvent();
            $("#MainContent_ddlAcademicType").change(function () {
                bindEvent();
                //initPage();
                toggleDiv(this.value);               
            });
           
            var tBiostat = new biostatNS.TableToggle(tblBiostat);
            tBiostat.Click(tblBiostat);
            $('#tblBiostat').on('click', 'input[type="checkbox"]', function () {
                tBiostat.Click(this.closest('table'));
            })

            var startDate = new biostatNS.DatePicker('dtpStartDate');
            startDate.init();
            var endDate = new biostatNS.DatePicker('dtpEndDate');
            endDate.init();

            var panelStartDate = new biostatNS.DatePicker('dtpPanelStartDate');
            panelStartDate.init();
            var panelEndDate = new biostatNS.DatePicker('dtpPanelEndDate');
            panelEndDate.init();

            var journalDate = new biostatNS.DatePicker('dtpJournalDate');
            journalDate.init();

            var grantDate = new biostatNS.DatePicker('dtpGrantDate');
            grantDate.init();

            var awardDate = new biostatNS.DatePicker('dtpAwardDate');
            awardDate.init();

            var trainingDate = new biostatNS.DatePicker('dtpTrainingDate');
            trainingDate.init();

            var mentorStartDate = new biostatNS.DatePicker('dtpMentorStartDate');
            mentorStartDate.init();
            var mentorEndDate = new biostatNS.DatePicker('dtpMentorEndDate');
            mentorEndDate.init();

            var safetyStartDate = new biostatNS.DatePicker('dtpSafetyStartDate');
            safetyStartDate.init();
            var safetyEndDate = new biostatNS.DatePicker('dtpSafetyEndDate');
            safetyEndDate.init();

            var otherDate = new biostatNS.DatePicker('dtpOtherDate');
            otherDate.init();

            var studentMentorStartDate = new biostatNS.DatePicker('dtpStudentMentorStartDate');
            studentMentorStartDate.init();
            var studentMentorEndDate = new biostatNS.DatePicker('dtpStudentMentorEndDate');
            studentMentorEndDate.init();

            //$('#MainContent_btnSave').prop("disabled", true);
            //$("#MainContent_txtCheckCode").on('change keyup paste', function () {
            //    if ($('#MainContent_txtConfirmCode').val() == $('#MainContent_txtCheckCode').val()) {
            //        $('#MainContent_btnSave').prop("disabled", false);;
            //    }
            //    else
            //        $('#MainContent_btnSave').prop("disabled", true);
            //});

            

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

        });

        function bindEvent() {
            var filter = $("#MainContent_ddlAcademicType :selected").text();            
            
            $("#MainContent_ddlEventHdn > option").not(":first").each(function () {
                
                if (this.text.indexOf(filter) > 0 || filter.length == 0 || filter.indexOf('select') >= 0) {
                    $("#MainContent_ddlEventHdn").children("option[value=" + this.value + "]").show();
                    $("#MainContent_ddlEvent").children("option[value=" + this.value + "]").show();
                    //alert('show ' + this.value);                   
                }
                else {
                    $("#MainContent_ddlEventHdn").children("option[value=" + this.value + "]").hide();
                    $("#MainContent_ddlEvent").children("option[value=" + this.value + "]").hide();
                    //alert('hide ' + this.value);
                }                
            });

            if (filter.length == 0 || filter.indexOf('select') >= 0) {
                $('#MainContent_ddlEvent').prop("disabled", true);
                initPage();
                setTimeout(function () {
                    $('#MainContent_ddlEvent').prop("disabled", false);
                }, 3000);
                //$('#MainContent_ddlEvent').prop("disabled", false);
            }

        }

        function initPage() {            
            //alert($('#MainContent_ddlAcademicType').val());
            $("#MainContent_ddlEvent option[value='']").attr('selected', true);
            
            $('#tblBiostat').find('td').each(function () {
                $(this).find(":input[name$='chkId']").prop('checked', false);
            });

            $('#MainContent_txtComments').val('');
            $('#MainContent_txtCheckCode').val(''); 
            $('#MainContent_txtBiostatBitSum').val(0);
            
            var academicType = $('#MainContent_ddlAcademicType').val();
            switch (academicType) {
                case '1':
                    $('#MainContent_txtOrganization').val('');
                    $('#MainContent_txtTitle').val('');
                    $('#MainContent_txtStartDate').val('');
                    $('#MainContent_txtEndDate').val('');
                    $('#MainContent_txtNumOfAttendees').val('');
                    $('#MainContent_txtCourseNum').val('');
                    break;
                case '2':
                    $('#MainContent_ddlSemester').val('');
                    $('#MainContent_txtYear').val('');
                    $('#MainContent_txtCourseTitle').val('');
                    $('#MainContent_txtCourseNumTeaching').val('');
                    $('#MainContent_txtNumOfCredits').val('');
                    $('#MainContent_txtNumOfStudents').val('');
                    break;
                case '3':
                    $('#MainContent_ddlCommitteeType').val('');
                    $('#MainContent_txtDepartment').val('');
                    $('#MainContent_txtStudentName').val('');
                    $('#MainContent_txtAdvisorName').val('');
                    $('#MainContent_txtProjectTitle').val('');
                    $('#MainContent_ddlStartSemester').val('');
                    $('#MainContent_ddlEndSemester').val('');
                    $('#MainContent_txtStartYear').val('');
                    $('#MainContent_txtEndYear').val('');
                    break;
              case '4':    
                    $('#MainContent_txtPanelOrg').val('');
                    $('#MainContent_txtPanelName').val('');
                    $('#MainContent_txtPanelStartDate').val('');
                    $('#MainContent_txtPanelEndDate').val('');
                    $('#MainContent_ddlPanelCommittee').val('');
                    break;
            case '5':                
                    $('#MainContent_txtJournalName').val('');
                    $('#MainContent_txtJournalDate').val('');
                    break;
            case '6':
                    $('#MainContent_ddlGrantAgency').val('');
                    $('#MainContent_txtGrantDate').val('');
                    break;
            case '7':
                    $('#MainContent_txtAwardAgency').val('');
                    $('#MainContent_txtAwardTitle').val('');
                    $('#MainContent_txtAwardDate').val('');
                    break;
            case '8':
                    $('#MainContent_txtTrainingProvider').val('');
                    $('#MainContent_ddlTrainingType').val('');
                    $('#MainContent_txtTrainingTitle').val('');
                    $('#MainContent_txtTrainingDate').val('');
                    $('#MainContent_txtTrainingloc').val('');
                    break;
            case '9':
                    $('#MainContent_txtMentorTitle').val('');
                    $('#MainContent_txtMenteeName').val('');
                    $('#MainContent_txtMenteeAffil').val('');
                    $('#MainContent_txtMentorStartDate').val('');
                    $('#MainContent_txtMentorEndDate').val('');
                    break;
            case '10':
                    $('#MainContent_txtSafeyTitle').val('');
                    $('#MainContent_txtPI').val('');
                    $('#MainContent_txtPIAffil').val('');
                    $('#MainContent_txtSafetyStartDate').val('');
                    $('#MainContent_txtSafetyEndDate').val('');
                    break;
            case '11':
                    $('#MainContent_txtOtherTitle').val('');
                    $('#MainContent_txtOtherDesc').val('');
                    $('#MainContent_txtOtherDate').val('');
                    break;
            case '12':
                    $('#MainContent_txtStudentMentorStudentName').val('');
                    $('#MainContent_txtStudentMentorDepartment').val('');
                    $('#MainContent_txtStudentMentorTitle').val('');
                    $('#MainContent_txtStudentMentorDesc').val('');
                    $('#MainContent_txtStudentMentorStartDate').val('');
                    $('#MainContent_txtStudentMentorEndDate').val('');
                    break;
            default:
                    //alert("changed!");
                    break;
            }

            //$("#MainContent_ddlEvent").prop("disabled", false);
        }

        function toggleDiv(id) {
            $('div[id^="div"]').each(function () {
                var sdiv = 'div' + id; 
                if (this.id == sdiv) {
                    $(this).show(); 
                }
                else
                    $(this).hide();

            });

        }

        function ValidateControl(submitButton) {            
            var academicTypeId = $('#MainContent_ddlAcademicType').val(),
                staffBitSum = $('#MainContent_txtBiostatBitSum').val();

            if (academicTypeId > 0 && staffBitSum > 0) {
                ClientSideClick(submitButton);
                return true;
            }
            else {
                alert('Please check type and staff.');
                return false;
            }
        }

        function ClientSideClick(myButton) {
            if (myButton.getAttribute('type') == 'button') {
                // disable the button                
                myButton.disabled = true;
                //myButton.className = "btn-inactive";
                myButton.value = "Processing......";
            }
            return true;
        }

        var biostatNS = biostatNS || {};

        biostatNS.TableToggle = function (tableId) {
            var _table = $(tableId),                
                _textBitSum = _table.next().find(":input[name$='BitSum']");

            return {
                Click: function (e) {
                    var _bitSum = 0;

                    _table.find('td').each(function () {
                        var _checkBox = $(this).find(":input[name$='chkId']"),
                            _bitValue = $(this).find(":input[name$='BitValue']").val(); 
                        
                        if (_checkBox.is(':checked'))
                            _bitSum += parseInt(_bitValue, 10);
                    });

                    $(_textBitSum).val(_bitSum);
                }
            }
        }

        biostatNS.DatePicker = function (ctrlId) {
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

    </script>

</asp:Content>
