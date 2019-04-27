<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ClientForm.aspx.cs" Inherits="ProjectManagement.Admin.ClientForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .notAllowed
        {
            opacity: 0.6;
            /*cursor: not-allowed;*/
        }
    </style>
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
                            <img src="../images/Logo_Final.png" alt="Loading.. Please wait!" />
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
                                        <%--<div class="col-sm-4">
                                            <b><u>First name:</u></b>
                                            <asp:Label ID="lblFirstName" runat="server"></asp:Label>
                                        </div>--%>
                                        <label class="col-sm-2 control-label" for="txtFirstName"><strong>First name:</strong></label>
                                        <div class="col-sm-2">
                                            <input class="form-control" type="text" name="txtFirstName" id="txtFirstName" placeholder="First name" runat="server" />
                                        </div>
                                        <label class="col-sm-2 control-label" for="txtLastName"><strong>Last name:</strong></label>
                                        <div class="col-sm-2">
                                            <input class="form-control" type="text" name="txtLastName" id="txtLastName" placeholder="Last name" runat="server" />
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
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <label class="col-sm-2 control-label" for="txtEmail"><strong>Email</strong></label>
                                        <div class="col-sm-2">
                                            <input class="form-control" type="text" name="txtEmail" id="txtEmail" placeholder="Email" runat="server" />
                                        </div>
                                        <label class="col-sm-2 control-label" for="txtPhone"><strong>Phone number</strong></label>
                                        <div class="col-sm-2">
                                            <input class="form-control phoneNum" type="text" name="txtPhone" id="txtPhone" placeholder="Phone" runat="server" />
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <label class="col-sm-3 control-label" for="txtDept"><strong>Organization/Department</strong></label>
                                        <div class="col-sm-4">
                                            <input class="form-control typeahead" type="text" name="txtDept" id="txtDept" placeholder="Organization/Department" runat="server" onchange="updateId(this)" />
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
                                        </div>
                                        <div class="col-sm-2 text-right">
                                            <label class="control-label" for="txtTitle">Mentor last name:</label>
                                        </div>
                                        <div class="col-sm-2">
                                            <input class="form-control" type="text" name="txtMentorLastName" id="txtMentorLastName" runat="Server" />
                                        </div>
                                        <div class="col-sm-1 text-right">
                                            <label class="control-label" for="txtTitle">Email:</label>
                                        </div>
                                        <div class="col-sm-3">
                                            <input class="form-control" type="text" name="txtMentorEmail" id="txtMentorEmail" runat="Server" />
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
    <script src="../Scripts/typeahead.jquery.min.js"></script>
    <script type="text/javascript">

        $(document).ready(function () {
            bindForm();
        });

        function bindForm() {

            // --> Toggles Yes/No checkboxes to make them single choice.
            $('#MainContent_chkJuniorPIYes').change(function () {
                $('#MainContent_chkJuniorPINo').prop('checked', false);
            });

            $('#MainContent_chkJuniorPINo').change(function () {
                $('#MainContent_chkJuniorPIYes').prop('checked', false);
            });

            ToggleDiv($('#MainContent_chkMentorYes'), $('#MainContent_divMentor'));

            $('#MainContent_chkMentorYes').change(function () {
                ToggleDiv($(this), $('#MainContent_divMentor'));
                $('#MainContent_chkMentorNo').prop('checked', false);
            });

            $('#MainContent_chkMentorNo').change(function () {
                $('#MainContent_chkMentorYes').prop('checked', false);
                $('#MainContent_divMentor').hide();
            });

            // --> Shows "Other" field if dropdown selection is "Other".
            showHideOtherField($('#MainContent_ddlDegree'), $('#MainContent_txtDegreeOther'));
            showHideOtherField($('#MainContent_ddlPIStatus'), $('#MainContent_txtPIStatusOther'));
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

        // -- Given dropdown trigger, shows or hides the text "Other" field if "Other" is selected in the dropdown.-- \\
        function showHideOtherField(dropdown, otherField) {
            otherField.val('');
            otherField.hide();
            otherField.parent().find('label').hide()

            dropdown.on('change', function () {
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

        var data = "";//JSON.parse($('#MainContent_textAreaDeptAffil').val());

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

                $('#MainContent_txtDeptAffilId').val(map[selection].Id);


            });

    </script>
</asp:Content>
