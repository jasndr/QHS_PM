<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Project.aspx.cs" Inherits="ProjectManagement.Report.Project" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .twitter-typeahead{
             width: 100%;
        }
        .tt-menu{
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel panel-default">
        <div class="panel-heading">
            <b>Project Report</b>
        </div>

        <div class="panel-body">
            <div class="row offset2">
                <div class="col-md-1 text-right">
                    <label class="control-label">Project Type:</label>
                </div>
                <div class="col-md-1 offset2">
                    <asp:CheckBox ID="chkProject" runat="server" Text="Project" Checked="True"></asp:CheckBox>
                </div>
                 <div class="col-md-1 offset2">
                    <asp:CheckBox ID="chkConsult" runat="server" Text="Consult" ></asp:CheckBox>
                </div>
            </div>
            <div class="row offset2">
                <div class="col-md-1 text-right">
                    <label class="control-label">From Date:</label>
                </div>
                <div class="col-md-2">
                    <div class="input-group date" id="dtpFromDate">
                        <span class="input-group-addon">
                            <span class="glyphicon glyphicon-calendar"></span>
                        </span>
                        <asp:TextBox ID="txtFromDate" runat="server" class="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-1 text-right">
                    <label class="control-label">To Date:</label>
                </div>
                <div class="col-md-2">
                    <div class="input-group date" id="dtpToDate">
                        <span class="input-group-addon">
                            <span class="glyphicon glyphicon-calendar"></span>
                        </span>
                        <asp:TextBox ID="txtToDate" runat="server" class="form-control"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="row offset2">
                <div class="col-md-1 text-right">
                    <label class="control-label">PhD:</label>
                </div>
                <div class="col-xs-6 col-md-2">
                    <asp:DropDownList ID="ddlPhd" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
                <div class="col-md-1 text-right">
                    <label class="control-label">Master:</label>
                </div>
                <div class="col-xs-6 col-md-2">
                    <asp:DropDownList ID="ddlMs" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row offset2">
                <div class="col-md-1 text-right">
                    <label class="control-label">Core:</label>
                </div>
                <div class="col-md-1 offset2">
                    <asp:CheckBox ID="chkBiostat" runat="server" Text="Biostat" Checked="True"></asp:CheckBox>
                </div>
                 <div class="col-md-1 offset2">
                    <asp:CheckBox ID="chkBioinfo" runat="server" Text="BioInfo"></asp:CheckBox>
                </div>
                <div class="col-md-1 text-right">
                    <label class="control-label">Credit To:</label>
                </div>
                <div class="col-md-1 offset2">
                    <asp:CheckBox ID="chkCreditToBiostat" runat="server" Text="Biostat" Checked="True"></asp:CheckBox>
                </div>
                <div class="col-md-1 offset2">
                    <asp:CheckBox ID="chkCreditToBioinfo" runat="server" Text="BioInfo"></asp:CheckBox>
                </div>
                <div class="col-md-1 offset2">
                    <asp:CheckBox ID="chkCreditToBoth" runat="server" Text="Both"></asp:CheckBox>
                </div>
            </div>
            <div class="row offset2">
                <div class="col-md-1 text-right">
                    <label class="control-label">PI Name:</label>
                </div>
                <div class="col-xs-6 col-md-2">
                    <input class="form-control nameahead" type="text" name="txtPIName" id="txtPIName" runat="Server" onchange="updateId(this)"/>
                </div>
                <div class="col-md-1 text-right">
                    <label class="control-label">PI Status:</label>
                </div>
                <div class="col-xs-6 col-md-3">
                    <asp:DropDownList ID="ddlPIStatus" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
                <div class="col-md-1 text-right">
                    <label class="control-label">Affiliation:</label>
                </div>
                <div class="col-xs-6 col-md-3">
                    <input class="form-control typeahead" type="text" name="txtAffiliation" id="txtAffiliation" runat="Server" onchange="updateId(this)" />
                </div>
            </div>
            <div class="row offset2 hidden">
                <div class="col-md-1 text-right">
                    <label class="control-label">PI Id:</label>
                </div>
                <div class="col-md-1">
                    <input class="form-control" type="text" name="txtPIId" id="txtPIId" runat="Server" />
                </div>
                <div class="col-md-2 text-right">
                    <label class="control-label">Affiliation Id:</label>
                </div>
                <div class="col-md-1">
                    <input class="form-control" type="text" name="txtAffilId" id="txtAffilId" runat="Server" />
                </div>
            </div>
            <div class="row offset2">
                <div class="col-md-1 text-right">
                    <label class="control-label">Health Data:</label>
                </div>
                <div class="col-xs-6 col-md-2">
                    <asp:DropDownList ID="ddlHealthData" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
                <div class="col-md-1 text-right">
                    <label class="control-label">Grant:</label>
                </div>
                <div class="col-xs-6 col-md-3">
                    <asp:DropDownList ID="ddlGrant" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
                <div class="col-md-1"></div>
                <div class="col-md-1">
                    <asp:Button ID="btnSumbit" runat="server" Text="Get Report" CssClass="btn btn-info" OnClick="btnSumbit_Click" OnClientClick="ClientSideClick(this)" UseSubmitBehavior="False" />
                </div>
                <div class="col-md-1">
                    <asp:Button ID="btnExportExcel" runat="server" Text="Download" CssClass="btn btn-info" OnClick="btnExportExcel_Click" OnClientClick="ClientSideClick(this)" UseSubmitBehavior="False" />
                </div>
            </div>
            <div class="hidden">
                <textarea id="textAreaPI" rows="3" runat="server"></textarea>
                <textarea id="textAreaAffil" rows="3" runat="server"></textarea>
                <input id="hdnRowCount" runat="server"/>
                <input id="download_token" runat="server"/>
            </div>

            <hr/>
            <div class="row" id="divProject">
                <div class="col-md-12">
                    <table class="table table-striped table-hover table-bordered">
                        <thead>
                            <tr>
                                <th class="col-sm-1">Id</th>
                                <th class="col-sm-1">First Name</th>
                                <th class="col-sm-1">Last Name</th>
                                <th class="col-sm-1">Affiliation</th>
                                <th class="col-sm-2">Title</th>
                                <th class="col-sm-1">Initial Date</th>
                                <th class="col-sm-1">Completion Date</th>
                                <th class="col-sm-1">Lead</th>
                                <th class="col-sm-1">Member</th>
                                <th class="col-sm-1">Service Category</th>
                                <th class="col-sm-1">Study Area</th>
                                <th class="col-sm-1">Study Type</th>
                                <th class="col-sm-1">Study Population</th>
                                <th>Grant</th>
                                <th>Phd Hrs</th>
                                <th>Ms Hrs</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptProjectSummary" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Eval("Id") %></td>
                                        <td><%# Eval("FirstName") %></td>
                                        <td><%# Eval("LastName") %></td>
                                        <td><%# Eval("Affiliation") %></td>
                                        <td><%# Eval("Title") %></td>
                                        <td><%# Eval("InitialDate") %></td>
                                        <td><%# Eval("CompletionDate") %></td>
                                        <td><%# Eval("LeadBio") %></td>
                                        <td><%# Eval("Member") %></td>
                                        <td><%# Eval("ServiceCategory") %></td>
                                        <td><%# Eval("StudyArea") %></td>
                                        <td><%# Eval("StudyType") %></td>
                                        <td><%# Eval("StudyPopulation") %></td>
                                        <td><%# Eval("GrantName") %></td>
                                        <td><%# Eval("PhdHrs") %></td>
                                        <td><%# Eval("MsHrs") %></td>
                                    </tr>
                                </ItemTemplate>
                                 <FooterTemplate>
                                    <tr>
                                        <td>Total</td>
                                        <td><asp:Label ID="lblTotal" runat="server" /></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td><asp:Label ID="lblPhdHrs" runat="server" /></td>
                                        <td><asp:Label ID="lblMsHrs" runat="server" /></td>
                                    </tr>
                                </FooterTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
            </div>

        </div>
    </div>
    

    <script src="../Scripts/bootstrap-datepicker.min.js"></script>
    <script src="../Scripts/jquery.cookie.js"></script>
    <script src="../Scripts/typeahead.jquery.min.js"></script> 
    <script type="text/javascript">
        $(document).ready(function () {
            //if ($("#MainContent_txtReportDate").val() == '') {
            //    var today = new Date();
            //    var mm = today.getMonth() + 1;
            //    if (mm < 10) mm = '0' + mm;
            //    var yy = today.getFullYear();
            //    $("#MainContent_txtReportDate").val(mm + '/' + yy);
            //}

            //var reportDate = new DatePicker('dtpReportDate');
            //reportDate.init();

            var fromDate = new DatePicker('dtpFromDate');
            fromDate.init();

            var toDate = new DatePicker('dtpToDate');
            toDate.init();
            
            if ($('#MainContent_hdnRowCount').val() > 0)
                $('#MainContent_btnExportExcel').prop("disabled", false);
            else {
                $('#MainContent_btnExportExcel').prop("disabled", true);
            }

            //$('#MainContent_txtPIName').change(function() {
            //    alert('changed');
            //});
            //$('#MainContent_txtPIId').val('');

        });

        $("#MainContent_chkProject").change(function () {
            if (this.checked) {
                $('#MainContent_chkConsult').prop('checked', false);
            }
            else
                $('#MainContent_chkConsult').prop('checked', true);
        });

        $("#MainContent_chkConsult").change(function () {
            if (this.checked) {
                $('#MainContent_chkProject').prop('checked', false);
            }
            else
                $('#MainContent_chkProject').prop('checked', true);
        });

        //$("#MainContent_chkBiostat").change(function () {
        //    if (this.checked) {
        //        $('#MainContent_chkBioinfo').prop('checked', false);
        //    }
        //    else
        //        $('#MainContent_chkBioinfo').prop('checked', true);
        //});

        //$("#MainContent_chkBioinfo").change(function () {
        //    if (this.checked) {
        //        $('#MainContent_chkBiostat').prop('checked', false);
        //    }
        //    else
        //        $('#MainContent_chkBiostat').prop('checked', true);
        //});

        $("#MainContent_chkCreditToBiostat").change(function () {
            if (this.checked) {
                $('#MainContent_chkCreditToBioinfo').prop('checked', false);
                $('#MainContent_chkCreditToBoth').prop('checked', false);
            }
            //else
            //    $('#MainContent_chkCreditToBioinfo').prop('checked', true);
        });

        $("#MainContent_chkCreditToBioinfo").change(function () {
            if (this.checked) {
                $('#MainContent_chkCreditToBiostat').prop('checked', false);
                $('#MainContent_chkCreditToBoth').prop('checked', false);
            }
        });

        $("#MainContent_chkCreditToBoth").change(function () {
            if (this.checked) {
                $('#MainContent_chkCreditToBiostat').prop('checked', false);
                $('#MainContent_chkCreditToBioinfo').prop('checked', false);
            }
        });

        function updateId(txtbox) {
            if (txtbox.value == '') {
                if (txtbox.id == 'MainContent_txtPIName') {
                    $('#MainContent_txtPIId').val('');
                } else {
                    $('#MainContent_txtAffilId').val('');
                }
            }
        }

        var DatePicker = function (ctrlId) {
            var ctl = ctrlId;
           
            return {
                init: function (e) {
                    $('#' + ctl).datepicker({
                        //starView: "year",
                        //viewMode: "months",
                        //minViewMode: "months",
                        format: "mm/dd/yyyy",
                        autoclose: true,
                        orientation: "top",
                        todayHighlight: true
                    });
                }
            }
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

        var fileDownloadCheckTimer;
        function blockUIForDownload(button) {
            var token = new Date().getTime(); //use the current timestamp as the token value
            $('#MainContent_download_token').val(token);
            $('#MainContent_btnExportExcel').prop("disabled", true);
            $('#MainContent_btnExportExcel').val("Downloading");
            //$(".pdsa-submit-progress").removeClass("hidden");
            fileDownloadCheckTimer = window.setInterval(function () {
                var cookieValue = $.cookie('fileDownloadToken');
                if (cookieValue <= token) {

                    finishDownload();
                }
            }, 3000);
        }

        function finishDownload() {
            window.clearInterval(fileDownloadCheckTimer);
            $.cookie('fileDownloadToken', null); //clears this cookie value
            //$(".pdsa-submit-progress").addClass("hidden");
            $('#MainContent_btnExportExcel').prop("disabled", false);
            $('#MainContent_btnExportExcel').val("Download");
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

        var data = JSON.parse($('#MainContent_textAreaAffil').val());

        $.each(data, function (i, name) {
            map[name.Name] = name;
            names.push(name.Name);
        });

        //for (var step = 0; step < 3; step++) {
        //    alert(pinames[step]);
        //}

        $('.typeahead').typeahead({
            hint: true,
            highlight: true,
            minLength: 1
        },
        {
            name: 'name',
            limit: 10,
            source: substringMatcher(names)
            //,selected: function (item) {
            //    //selectedAffil = map[item.Id]; alert(selectedAffil);
            //    $('#txtSelectedAffil').val(map[item].Id);
            //    return item;
            //}
            , updater: function (item) {
                alert(map[item].Name);
            }
        }).on('typeahead:selected', function (event, selection) {
            //$(this).val(map[selection].Name);
            $('#MainContent_txtAffilId').val(map[selection].Id);

            // clearing the selection requires a typeahead method
            //$(this).typeahead('setQuery', '');
           
        });

        var pinames = [];
        var pimap = {};

        var pidata = JSON.parse($('#MainContent_textAreaPI').val());

        $.each(pidata, function (i, name) {
            pimap[name.Name] = name;
            //pimap[name.Id] = name.Id;
            pinames.push(name.Name);
        });

        //for (var step = 0; step < 3; step++) {
        //    alert(pinames[step]);
        //}

        $('.nameahead').typeahead({
                hint: true,
                highlight: true,
                minLength: 1
            },
            {
                name: 'name',
                limit: 10,
                source: substringMatcher(pinames)
                //,selected: function (item) {
                //     alert(map[item.Id]);
                //    //$('#txtSelectedAffil').val(map[item].Id);
                //    return item;
                //}
            }).on("typeahead:selected", function (obj, datum) {
                $('#MainContent_txtPIId').val(pimap[datum].Id);
                //$(this).data("seletectedId", datum.Id);
            });

       
    </script>
</asp:Content>
