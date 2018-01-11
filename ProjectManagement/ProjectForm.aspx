<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProjectForm.aspx.cs" Inherits="ProjectManagement.ProjectForm" %>
<%@ Register Assembly="DropDownChosen" Namespace="CustomDropDown" TagPrefix="ucc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      
    <%--<script src="Scripts/bootstrap.min.js"></script>--%>    
    <script src="Scripts/bootstrap-datepicker.min.js"></script> 
    <script src="Scripts/jquery.bootstrap.wizard.min.js"></script>
    <script src="Scripts/jquery.validate.min.js"></script>
    <%--<script src="Scripts/modernizr-2.6.2.js"></script>--%>
    <script src="Scripts/chosen.jquery.js"></script> 
    <link href="Content/chosen.css" rel="stylesheet" />   
    <script type="text/javascript">        
        $(document).ready(function () {
            //$("html, body").animate({ scrollTop: $(document).height() }, 1500);

            $(".loader").fadeOut("slow");

            $('#li_project').addClass('selected');
            $('#li_projectform').addClass('selected');

            $('#rootwizard').bootstrapWizard({
                //onNext: function (tab, navigation, index) {
                //    if (index == 1) {
                //        if ($('#MainContent_ddlInvestor').val() === "") {
                //            alert('PI is required.');
                           
                //        }
                //    }
                //} 
            });

            if ($("#MainContent_dvInvoice_lblProjectInvoiceID").text() == "0") {
                $('a[href*="Edit"]').hide();
                $('a[href*="Delete"]').hide();
            }            

           

            //$(document).click(function (event) {
            //    window.lastElementClicked = event.target;
            //    //alert(window.lastElementClicked);
            //    $(window).load(function () {
            //        $("html, body").animate({ scrollTop: $(document).height() }, 1000);
            //    });
            //});

            $('#divProjectType').hide();

            $(function () {
                //alert($(this).attr('id'));
                //alert(localStorage.getItem('lastTab'));
             
                // for bootstrap 3 use 'shown.bs.tab', for bootstrap 2 use 'shown' in the next line
                $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
                    // save the latest tab; use cookies if you like 'em better:
                    localStorage.setItem('lastTab', $(this).attr('href'));
                });

                $('a[data-toggle="menu"]').on('click', function () {
                    // save the first tab; use cookies if you like 'em better:
                    localStorage.setItem('lastTab', $('#tab1').attr('href'));
                });

                // go to the latest tab, if it exists:                
                var lastTab = localStorage.getItem('lastTab');
                //if (lastTab && document.getElementById('MainContent_lblProjectId').innerHTML != '') {
                if (lastTab && $("#<%=lblProjectId.ClientID%>").text().length > 0) {
                    $('[href="' + lastTab + '"]').tab('show');
                }
            });           
                  

            $(function () {
                $("#MainContent_ddlInvestor").chosen();
                $("#MainContent_ddlInvestor_chosen").width("100%");
            });

            if (document.getElementById('optionsRadios1').checked) {
                $('#divServiceType2').hide();
            }
            else if (document.getElementById('optionsRadios2').checked) {
                $('#divServiceType1').hide();
            }

            $('input[type=radio][name=optionsRadios]').change(function () {
                if (this.value == 'option1') {
                    $('#divServiceType2').hide();
                    $('#divServiceType1').show();
                }
                else if (this.value == 'option2') {
                    $('#divServiceType1').hide();
                    $('#divServiceType2').show();
                }
            });

            if (document.getElementById('optionsPilot1').checked) {
                $('#divPilot').show();
            }
            else if (document.getElementById('optionsPilot2').checked) {
                $('#divPilot').hide();
            }

            $('input[type=radio][name=optionsPilot]').change(function () {
                if (this.value == 'Yes') {
                    $('#divPilot').show();
                }
                else {
                    $('#divPilot').hide();
                }
            });
          

            if (document.getElementById('optionsMentor1').checked) {
                $('#divMentor').show();
            }
            else if (document.getElementById('optionsMentor2').checked) {
                $('#divMentor').hide();
            }

            $('input[type=radio][name=optionsMentors]').change(function () {
                if (this.value == 'Yes') {
                    $('#divMentor').show();
                }
                else {
                    $('#divMentor').hide();
                }
            });
            
            //GrantOtherToggle();
            if (document.getElementById('MainContent_GridViewGrant_chkRow_15').checked)
                $('#MainContent_GridViewGrant_txtGrantNameNew').show();
            else {
                $('#MainContent_GridViewGrant_txtGrantNameNew').hide();
                $('#MainContent_GridViewGrant_txtGrantNameNew').val('');
            }

            //StudyAreaToggle();
            if (document.getElementById('MainContent_GridViewStudyArea_chkRow_6').checked)
                $('#MainContent_GridViewStudyArea_txtStudyOther').show();
            else {
                $('#MainContent_GridViewStudyArea_txtStudyOther').hide();
                $('#MainContent_GridViewStudyArea_txtStudyOther').val('');
            }

            //StudyTypeToggle();
            if (document.getElementById('MainContent_GridViewStudyType_chkRow_3').checked)
                $('#MainContent_GridViewStudyType_txtStudyTypeOther').show();
            else {
                $('#MainContent_GridViewStudyType_txtStudyTypeOther').hide();
                $('#MainContent_GridViewStudyType_txtStudyTypeOther').val('');
            }

            //StudyPopToggle();
            if (document.getElementById('MainContent_GridViewStudyPop_chkRow_0').checked) {
                $('#MainContent_GridViewStudyPop').find('input[type="checkbox"]').each(function () {
                    if (this.id != 'MainContent_GridViewStudyPop_chkRow_0') {
                        this.checked = false;
                        this.disabled = true;
                    }
                });

                $('#MainContent_GridViewStudyPop_txtStudyPopOther').hide();
            }
            else {
                $('#MainContent_GridViewStudyPop').find('input[type="checkbox"]:checked').each(function () {
                    if (this.id != 'MainContent_GridViewStudyPop_chkRow_0') {
                        $('#MainContent_GridViewStudyPop_chkRow_0').attr("disabled", true);
                    }
                });
            }

            if (document.getElementById('MainContent_GridViewStudyPop_chkRow_3').checked)
                $('#MainContent_GridViewStudyPop_txtStudyPopOther').show();
            else {
                $('#MainContent_GridViewStudyPop_txtStudyPopOther').hide();
                $('#MainContent_GridViewStudyPop_txtStudyPopOther').val('');
            }          

            <%-- $("#btnSearch").click(function () {
                var piId = $("#MainContent_ddlInvestor").val();
               
                if (!piId) piId = 0;
                if (piId > 0) {
                    $.ajax({
                        type: "POST",
                        url: "ProjectForm.aspx/GetProjectList",
                        data: '{"piId":"' + piId + '"}',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        success: function (rtndata) {
                            //alert(JSON.stringify(rtndata.d));
                            bindProjectList(rtndata.d);
                        },
                        error: function (xhr, status, err) {
                            var err = eval("(" + xhr.responseText + ")");
                            alert(err.Message);

                        }
                    });
                }
                else {
                    $("#<%=ddlInvestor.ClientID%>").val(1);
               }
            });--%>    
            
            var projectId = $("#MainContent_lblProjectId").text();
            if (projectId > 0) {
                $("#MainContent_btnAddInvoice").prop("disabled", false);
                $("#MainContent_btnAddGrant").prop("disabled", false);
                $("#MainContent_btnSurvey").prop("disabled", false);
            }
            else {
                $("#MainContent_btnAddInvoice").prop("disabled", true);
                $("#MainContent_btnAddGrant").prop("disabled", true);
                $("#MainContent_btnSurvey").prop("disabled", true);

                $('#MainContent_gvPhase').find('select[name$="ddlPhase"]').val(0);
                selectPhase($('#MainContent_gvPhase').find('select[name$="ddlPhase"]'));
            }

            if ($('#chkRmatrix').is(':checked')){
                $('#divRmatrix').show();
            }
            else {
                $('#divRmatrix').hide();
            }
            $('input[type=checkbox][id=chkRmatrix]').change(function () {
                if (this.checked == true) {
                    $('#divRmatrix').show();
                }
                else {
                    $('#divRmatrix').hide();
                }
            });
            
        });

        function selectPhase(item) {
            var val = $(item).val();
            if (val == 0 && val !='') {
                $(item).closest('tr').find('input[name$="txtMsHrs"]').val(1.0);
                $(item).closest('tr').find('input[name$="txtPhdHrs"]').val(1.0);
                $(item).closest('tr').find('input[name$="txtTitle"]').val('Consultation');
            }
            else {
                $(item).closest('tr').find('input[name$="txtMsHrs"]').val('');
                $(item).closest('tr').find('input[name$="txtPhdHrs"]').val('');
                $(item).closest('tr').find('input[name$="txtTitle"]').val('');
            }
        }


        function GetProjectHours() {
            var projectId = $("#MainContent_lblProjectId").text();
            var startDate = $("#MainContent_TextBoxInvoiceStartDate").val();
            var endDate = $("#MainContent_TextBoxInvoiceEndDate").val();

            if (projectId > 0 && startDate !='' && endDate != '')
            {
                $.ajax({
                    type: "POST",
                    url: "ProjectForm.aspx/GetProjectHours",
                    data: '{"projectId":"' + projectId + '","startDate":"' + startDate + '","endDate":"' + endDate + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (rtndata) {
                        //alert(JSON.stringify(rtndata.d));
                        bindProjectHours(rtndata.d);
                    },
                    error: function (xhr, status, err) {
                        var err = eval("(" + xhr.responseText + ")");
                        alert(err.Message);

                    }
                });
            }
        }

        function bindProjectHours(data) {
            if (data.length > 0) {
                $("#MainContent_txtPhdHours").val(data[0]);
                $("#MainContent_txtMsHours").val(data[1]);
            }
        }

        function ClientSideClick(myButton) {
            // Client side validation
            //if (typeof (Page_ClientValidate) == 'function') {
            //    if (Page_ClientValidate() == false)
            //    { return false; }
            //}
            
            //make sure the button is not of type "submit" but "button"
            if (myButton.getAttribute('type') == 'button') {
                // disable the button                
                myButton.disabled = true;
                //myButton.className = "btn-inactive";
                myButton.value = "Processing......";
            }
            return true;
        }
               
        function clearProject() {
            $('#<%=ddlProject.ClientID %>').empty();
            $('#<%=ddlInvestor.ClientID%>').val(-1);
        }

        <%-- function bindProjectList(data) {
            $('#<%=ddlProject.ClientID %>').empty();

            var defaultOpt = "<option selected='selected' value='-1'>Add a New Project</option>"
            $('#<%=ddlProject.ClientID %>').prepend(defaultOpt);

               for (var i = 0; i < data.length; i++) {
                   var id = data[i].Id;
                   var title = data[i].Id + ' ' + data[i].Title;
                   var newOption = "<option value='" + id + "'>" + title + "</option>";
                   $('#<%=ddlProject.ClientID %>').append(newOption);
               }
        }--%>

        function StudyPopToggle(e) {
            if (e.id == 'MainContent_GridViewStudyPop_chkRow_0') {
                if (e.checked)
                {
                    $('#MainContent_GridViewStudyPop').find('input[type="checkbox"]').each(function () {
                        if (this.id != 'MainContent_GridViewStudyPop_chkRow_0') {
                            this.checked = false;
                            this.disabled = true;
                        }
                    });

                    $('#MainContent_GridViewStudyPop_txtStudyPopOther').hide();
                }
                else
                {
                    $('#MainContent_GridViewStudyPop').find('input[type="checkbox"]').each(function () {
                        if (this.id != 'MainContent_GridViewStudyPop_chkRow_0') {
                            this.disabled = false;
                        }
                    });
                    
                }
            }
            else {
                if (e.checked) {
                    $('#MainContent_GridViewStudyPop_chkRow_0').attr("disabled", true);
                }
                else {
                    $('#MainContent_GridViewStudyPop_chkRow_0').attr("disabled", false);

                    $('#MainContent_GridViewStudyPop').find('input[type="checkbox"]:checked').each(function () {
                        if (this.id != 'MainContent_GridViewStudyPop_chkRow_0') {
                            $('#MainContent_GridViewStudyPop_chkRow_0').attr("disabled", true);
                        }
                    });
                }

            }

            if (e.id == 'MainContent_GridViewStudyPop_chkRow_3') {
                if (e.checked)
                    $('#MainContent_GridViewStudyPop_txtStudyPopOther').show();
                else {
                    //$('#MainContent_GridViewStudyArea_txtStudyOther').val('');
                    $('#MainContent_GridViewStudyPop_txtStudyPopOther').hide();
                }
            }
        }

        function StudyTypeToggle(e) {
            if (e.id == 'MainContent_GridViewStudyType_chkRow_3') {
                if (e.checked)
                    $('#MainContent_GridViewStudyType_txtStudyTypeOther').show();
                else {
                    //$('#MainContent_GridViewStudyArea_txtStudyOther').val('');
                    $('#MainContent_GridViewStudyType_txtStudyTypeOther').hide();
                }
            }
        }

        function StudyAreaToggle(e) {
            if (e.id == 'MainContent_GridViewStudyArea_chkRow_6') {
                if (e.checked)
                    $('#MainContent_GridViewStudyArea_txtStudyOther').show();
                else {
                    //$('#MainContent_GridViewStudyArea_txtStudyOther').val('');
                    $('#MainContent_GridViewStudyArea_txtStudyOther').hide();
                }
            }
        }

        function GrantOtherToggle(e) {
            if (e.id == 'MainContent_GridViewGrant_chkRow_15') {
                if (e.checked)
                    $('#MainContent_GridViewGrant_txtGrantNameNew').show();
                else {
                    //$('#MainContent_GridViewGrant_txtGrantNameNew').val('');
                    $('#MainContent_GridViewGrant_txtGrantNameNew').hide();
                }
            }
        }

        function gridviewServiceTypeToggle() {
            $('input[type=radio][name=optionsRadios]').change(function () {
                if (this.value == 'option1') {
                    $('#divServiceType2').hide();
                    $('#divServiceType1').show();
                }
                else if (this.value == 'option2') {
                    $('#divServiceType1').hide();
                    $('#divServiceType2').show();
                }
            });
        }

        $(function () {
            $('#datetimepicker1').datepicker({
                format: "mm/dd/yyyy",
                autoclose: true,
                orientation: "top",
                todayHighlight: true
            });
            $('#datetimepicker2').datepicker({
                format: "mm/dd/yyyy",
                autoclose: true,
                orientation: "top",
                todayHighlight: true
            });
            $('#datetimepicker3').datepicker({
                format: "mm/dd/yyyy",
                autoclose: true,
                orientation: "top",
                todayHighlight: true
            });
            $('#datetimepicker4').datepicker({
                format: "mm/dd/yyyy",
                autoclose: true,
                orientation: "top",
                todayHighlight: true
            });
            $('#datetimepicker5').datepicker({
                format: "mm/dd/yyyy",
                autoclose: true,
                orientation: "top",
                todayHighlight: true
            });
            $('#datetimepicker6').datepicker({
                format: "mm/dd/yyyy",
                autoclose: true,
                orientation: "top",
                todayHighlight: true
            });
                        
            $('#datetimepickerInvoiceStart').datepicker({
                format: "mm/dd/yyyy",
                autoclose: true,
                todayHighlight: true
            });
            $('#datetimepickerInvoiceEnd').datepicker({
                format: "mm/dd/yyyy",
                autoclose: true,
                todayHighlight: true
            });
        });        

    </Script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%--<div class="se-pre-con"></div> --%>
    <div class="loader"></div>
    <div id="rootwizard" class="jumbotron">
      <ul>
         <li><a href="#tab1" data-toggle="tab">Project</a></li>
         <li><a href="#tab2" data-toggle="tab">Grant</a></li>
         <li><a href="#tab3" data-toggle="tab">Pilot</a></li>
         <li><a href="#tab4" data-toggle="tab">Biostat</a></li>
         <li><a href="#tab5" data-toggle="tab">Study</a></li>
         <li><a href="#tab6" data-toggle="tab">Service</a></li>
         <li><a href="#tab7" data-toggle="tab">Final</a></li>
      </ul>
      <%--&nbsp;--%>
    <div class="row">
        <div class="col-md-11"></div>
        <div class="col-md-1">Id:
            <asp:Label ID="lblProjectId" runat="server" Text=""></asp:Label>
        </div>
    </div>
      <div class="tab-content">
          <div class="tab-pane" id="tab1">
              <div class="panel panel-default">
                  <div class="panel-heading">Project</div>
                  <div class="panel-body">
                      <br />
                      <asp:Label runat="server" ID="lblDebugMsg"></asp:Label>                      
                      <div class="row">
                          <div class="col-xs-6 col-md-2">
                              Principal Investigator:
                          </div>   
                          <div class="col-xs-6 col-md-3">
                              <%-- <asp:DropDownList ID="ddlInvestor" runat="server" CssClass="form-control" >
                               </asp:DropDownList>--%>
                              <ucc:DropDownListChosen ID="ddlInvestor" runat="server" width="200px"
                                NoResultsText="No results match."            
                                DataPlaceHolder="Search PI" AllowSingleDeselect="true">            
                              </ucc:DropDownListChosen>
                          </div>
                          <div class="col-xs-6 col-md-2">
                               <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" class="btn btn-info"/>
                             <%-- <input id="btnSearch" type="button" value="Search" class="btn btn-info"/>--%>
                          </div>
                          <div class="col-xs-6 col-md-2">
                              <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnRest_Click" class="btn btn-info"/>
                          </div>
                      </div>
                      <br />
                      <div class="row">
                          <div class="col-xs-6 col-md-2">
                              Is the PI a Junior Investigator?
                          </div>
                          <div class="col-xs-6 col-md-6">
                              <asp:CheckBox ID="chkJuniorPI" runat="server" /> Junior Investigator
                          </div>
                      </div>
                      <br />
                      <div class="row">
                          <div class="col-xs-6 col-md-2">
                              Does the PI have a mentor?                              
                          </div> 
                          <div class="col-xs-6 col-md-6">
                              <div class="panel panel-default">
                                  <%--<div class="panel-heading">Does the PI have a mentor?</div>--%>
                                  <div class="panel-body">
                                      <div class="radio">
                                          <label>
                                              <input type="radio" name="optionsMentors" id="optionsMentor1" value="Yes">
                                              Yes
                                          </label>
                                          <label>
                                              <input type="radio" name="optionsMentors" id="optionsMentor2" value="No" checked>
                                              No
                                          </label>
                                      </div>
                                      
                                      <br />
                                      <div id="divMentor">
                                          <div class="row">
                                              <div class="col-xs-6 col-md-9">
                                                  <asp:TextBox ID="TextBoxMentorFirst" runat="server" placeholder="FirstName" class="form-control"></asp:TextBox>
                                              </div>
                                          </div>
                                          <br />
                                          <div class="row">
                                              <div class="col-xs-6 col-md-9">
                                                  <asp:TextBox ID="TextBoxMentorLast" runat="server" placeholder="LastName" class="form-control"></asp:TextBox>
                                              </div>
                                          </div>
                                          <br />
                                          <div class="row">
                                              <div class="col-xs-6 col-md-9">
                                                  <asp:TextBox ID="TextBoxMentorEmail" runat="server" placeholder="Email" class="form-control"></asp:TextBox>
                                              </div>
                                          </div>
                                      </div>
                                  </div>
                              </div>
                          </div>
                      </div>
                      <div class="row">
                          <div class="col-xs-6 col-md-2">
                               Project: 
                          </div> 
                          <div class="col-xs-6 col-md-6">
                              <asp:DropDownList ID="ddlProject" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlProject_Changed" AutoPostBack="True" ValidateRequestMode="Disabled">
                              </asp:DropDownList>
                          </div>
                      </div>
                      <br />
                      <div class="row">
                          <div class="col-xs-6 col-md-2">
                             Title:
                          </div>
                          <div class="col-xs-6 col-md-6">                                   
                            <asp:TextBox ID="TextBoxTitle" runat="server" placeholder="Project Title" class="form-control"></asp:TextBox>
                          </div>
                      </div>
                      <br />
                      <div class="row">
                          <div class="col-xs-6 col-md-2">
                             Summary:   
                          </div>
                          <div class="col-xs-6 col-md-6">
                              <asp:TextBox ID="TextBoxSummary" runat="server" placeholder="Project Summary" class="form-control"></asp:TextBox>
                          </div>
                      </div>
                      <br />
                      
                      <div class="row">
                          <div class="col-xs-6 col-md-2">
                             Initial Date:   
                          </div>
                          <div class="col-xs-6 col-md-3">
                              <div class='input-group date' id='datetimepicker1'>
                                  <span class="input-group-addon">
                                      <span class="glyphicon glyphicon-calendar"></span>
                                  </span>
                                  <asp:TextBox ID="TextBoxInitialDate" runat="server" class="form-control" placeholder="Initial Date"></asp:TextBox>
                              </div>
                          </div>
                      </div>
                      <br />
                      <div class="row">
                          <div class="col-xs-6 col-md-2">
                             Deadline:   
                          </div>
                          <div class="col-xs-6 col-md-3">
                              <div class='input-group date' id='datetimepicker2'>
                                  <span class="input-group-addon">
                                      <span class="glyphicon glyphicon-calendar"></span>
                                  </span>
                                  <asp:TextBox ID="TextBoxTargetDate" runat="server" class="form-control" placeholder="Deadline"></asp:TextBox>
                              </div>
                          </div>
                      </div>
                      <br />                      
                      <div class="row">
                          <div class="col-xs-6 col-md-3">
                              <asp:CheckBox ID="chkInternal" runat="server" />&emsp;Internal Study
                          </div>
                      </div>
                  </div>
              </div>
          </div>

          <div class="tab-pane" id="tab2">
              <div class="panel panel-default">
                  <div class="panel-heading">Grant Affiliation</div>
                  <div class="panel-body">
                      <asp:Label ID="lblMsg" runat="server" Text="" Visible="false"></asp:Label>
                      <asp:GridView ID="GridViewGrant" runat="server" AutoGenerateColumns="False"
                          OnRowCommand="GridViewGrant_RowCommand"
                          OnRowEditing="GridViewGrant_RowEditing"
                          OnRowCancelingEdit="GridViewGrant_RowCancelingEdit"
                          OnRowDataBound="GridViewGrant_RowDataBound"
                          OnRowUpdating="GridViewGrant_RowUpdating"
                          OnRowDeleting="GridViewGrant_RowDeleting"
                          ShowFooter="true"
                          class="table table-bordered table-hover">
                          <Columns>
                              <asp:TemplateField HeaderText="Select" ItemStyle-Width="20%">
                                  <ItemTemplate>
                                      <asp:CheckBox ID="chkRow" runat="server" OnClick="GrantOtherToggle(this);" ></asp:CheckBox>
                                  </ItemTemplate>
                              </asp:TemplateField>
                              <asp:TemplateField HeaderText="Id" Visible="false">
                                  <ItemTemplate>
                                      <asp:Label ID="lblId" runat="server" Text='<%# Bind("Id") %>'></asp:Label>
                                  </ItemTemplate>
                              </asp:TemplateField>
                              <asp:TemplateField HeaderText="Name" ItemStyle-Width="80%">
                                  <EditItemTemplate>
                                      <asp:TextBox ID="txtName" runat="server" Text='<%# Bind("GrantAffilName") %>' CssClass="" MaxLength="30"></asp:TextBox>
                                      <asp:RequiredFieldValidator ID="valName" runat="server" ControlToValidate="txtName"
                                          Display="Dynamic" ErrorMessage="Name is required." ForeColor="Red" SetFocusOnError="True"
                                          ValidationGroup="editGrp">*</asp:RequiredFieldValidator>
                                  </EditItemTemplate>
                                  <ItemTemplate>
                                      <asp:Label ID="lblName" runat="server" Text='<%# Bind("GrantAffilName") %>'></asp:Label>
                                  </ItemTemplate>
                                  <FooterTemplate>
                                      <asp:TextBox ID="txtGrantNameNew" runat="server" class="form-control" width="50%"></asp:TextBox>
                                  </FooterTemplate>
                              </asp:TemplateField>
                          </Columns>
                      </asp:GridView>
                      
                      <%--<div id="otherGrant" class="row">
                          <div class="col-xs-6 col-md-1">
                             Other:
                          </div>
                          <div class="col-xs-6 col-md-3">                                   
                            <asp:TextBox ID="TextBoxOtherGrant" runat="server" placeholder="Project Title" class="form-control"></asp:TextBox>
                          </div>
                      </div>--%>
                      <br />
                  </div>
              </div>
          </div>

         <div class="tab-pane" id="tab3">
            <div class="panel panel-default">
               <div class="panel-heading">Pilot Grant</div>
               <div class="panel-body">
                   <div class="row">
                       <div class="col-xs-6 col-md-6">
                           <label>Is Pilot?</label>
                           <div class="radio">
                               <label>
                                   <input type="radio" name="optionsPilot" id="optionsPilot1" value="Yes" >
                                   Yes
                               </label>
                               <label>
                                   <input type="radio" name="optionsPilot" id="optionsPilot2" value="No" checked >
                                   No
                               </label>
                           </div>
                       </div>
                   </div>
                   <div id="divPilot">
                    <asp:GridView ID="GridViewPilot" runat="server" AutoGenerateColumns="False"                         
                          class="table table-bordered table-hover">
                          <Columns>
                              <asp:TemplateField HeaderText="Select">
                                  <ItemTemplate>
                                      <asp:CheckBox ID="chkRow" runat="server"></asp:CheckBox>
                                  </ItemTemplate>
                              </asp:TemplateField>
                              <asp:TemplateField HeaderText="Id" Visible="false">
                                  <ItemTemplate>
                                      <asp:Label ID="lblId" runat="server" Text='<%# Bind("Id") %>'></asp:Label>
                                  </ItemTemplate>
                              </asp:TemplateField>
                              <asp:TemplateField HeaderText="Name">
                                  <EditItemTemplate>
                                      <asp:TextBox ID="txtName" runat="server" Text='<%# Bind("GrantAffilName") %>' CssClass="" MaxLength="30"></asp:TextBox>
                                      <asp:RequiredFieldValidator ID="valName" runat="server" ControlToValidate="txtName"
                                          Display="Dynamic" ErrorMessage="Name is required." ForeColor="Red" SetFocusOnError="True"
                                          ValidationGroup="editGrp">*</asp:RequiredFieldValidator>
                                  </EditItemTemplate>
                                  <ItemTemplate>
                                      <asp:Label ID="lblName" runat="server" Text='<%# Bind("GrantAffilName") %>'></asp:Label>
                                  </ItemTemplate>
                                  <FooterTemplate>
                                      <asp:TextBox ID="txtPilotNameNew" runat="server" CssClass="" MaxLength="30"></asp:TextBox>
                                     <%-- <asp:RequiredFieldValidator ID="valNameNew" runat="server" ControlToValidate="txtPilotNameNew"
                                          Display="Dynamic" ErrorMessage="Name is required." ForeColor="Red" SetFocusOnError="True"
                                          ValidationGroup="newGrp">*</asp:RequiredFieldValidator>--%>
                                  </FooterTemplate>
                              </asp:TemplateField>
                          </Columns>
                      </asp:GridView>
                    </div>
               </div>
            </div>
         </div>

          <div class="tab-pane" id="tab4">
              <div class="panel panel-default">
                  <div class="panel-heading">BQHS Faculty and Staff</div>
                  <div class="panel-body">
                      <div class="row">
                          <div class="col-xs-6 col-md-4">
                              <div class="dropdown">
                                  Lead Biostatistician:      
                           <asp:DropDownList ID="ddlBiostat" runat="server" CssClass="form-control">
                           </asp:DropDownList>
                              </div>
                          </div>
                          <div class="col-xs-6 col-md-6">
                              Other Member:
                        <asp:GridView ID="GridViewBioStat" runat="server" AutoGenerateColumns="False" class="table table-bordered table-hover">
                            <Columns>
                                <asp:TemplateField HeaderText="Select">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkRow" runat="server"></asp:CheckBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Id" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblId" runat="server" Text='<%# Eval("Key") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblName" runat="server" Text='<%# Eval("Value") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                          </div>
                      </div>
                  </div>
              </div>
          </div>

          <div class="tab-pane" id="tab5">
              <div class="panel panel-default">
                  <div class="panel-heading">Study</div>
                  <div class="panel-body">
                      <div class="row">
                       <div class="col-xs-6 col-md-6">
                      <div class="panel panel-default">
                            <div class="panel-heading">Study Area</div>
                                <div class="panel-body">
                                    <asp:GridView ID="GridViewStudyArea" runat="server" AutoGenerateColumns="False" class="table table-bordered table-hover" ShowFooter="true">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Select" HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkRow" runat="server" OnClick="StudyAreaToggle(this);"></asp:CheckBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Id" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblId" runat="server" Text='<%# Eval("Key") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Name" HeaderStyle-Width="80%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("Value") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="txtStudyOther" runat="server" class="form-control"></asp:TextBox>
                                                </FooterTemplate>
                                            </asp:TemplateField>                                    
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                           </div>
                           <div class="col-xs-6 col-md-6">
                      <div class="panel panel-default">
                            <div class="panel-heading">Health Data</div>
                                <div class="panel-body">
                                    <asp:GridView ID="GridViewHealthData" runat="server" AutoGenerateColumns="False" class="table table-bordered table-hover">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Select" HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkRow" runat="server" OnClick="StudyAreaToggle(this);"></asp:CheckBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Id" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblId" runat="server" Text='<%# Eval("Key") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Name" HeaderStyle-Width="80%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("Value") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>                                    
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                           </div>
                          </div>
                                                             
                      <%--<br />--%>
                      <div class="row">
                       <div class="col-xs-6 col-md-6">
                      <div class="panel panel-default">
                            <div class="panel-heading">Study Type</div>
                                <div class="panel-body">
                                    <asp:GridView ID="GridViewStudyType" runat="server" AutoGenerateColumns="False" class="table table-bordered table-hover" ShowFooter="true">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Select" HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkRow" runat="server" OnClick="StudyTypeToggle(this);"></asp:CheckBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Id" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblId" runat="server" Text='<%# Eval("Key") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Name" HeaderStyle-Width="80%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("Value") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="txtStudyTypeOther" runat="server" class="form-control"></asp:TextBox>
                                                </FooterTemplate>
                                            </asp:TemplateField>                                    
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                           </div>

                        <%--<br />--%>
                          <div class="col-xs-6 col-md-6">
                      <div class="panel panel-default">
                            <div class="panel-heading">Study Popluation</div>
                                <div class="panel-body">
                                    <asp:GridView ID="GridViewStudyPop" runat="server" AutoGenerateColumns="False" class="table table-bordered table-hover" ShowFooter="true">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Select" HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkRow" runat="server" OnClick="StudyPopToggle(this);"></asp:CheckBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Id" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblId" runat="server" Text='<%# Eval("Key") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Name" HeaderStyle-Width="80%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("Value") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="txtStudyPopOther" runat="server" class="form-control"></asp:TextBox>
                                                </FooterTemplate>
                                            </asp:TemplateField>                                    
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                              </div>
                          </div>

                      <br />
                      <div class="row">
                         <%-- <div class="col-xs-6 col-md-2">
                              Ethnic Group:   
                          </div>--%>
                          <div class="col-xs-6 col-md-6">
                            <asp:DropDownList ID="ddlEthnicGroup" runat="server" CssClass="form-control" Visible="false">
                            </asp:DropDownList>
                          </div>
                      </div>      

                  </div>
              </div>
          </div>

          <div class="tab-pane" id="tab6">
              <div class="panel panel-default">
                  <div class="panel-heading">Service</div>
                  <div class="panel-body">

                        <div class="panel panel-default">
                        <div class="panel-heading">Service Category</div>
                        <div class="panel-body">                             
                            <asp:GridView ID="GridViewServiceType" runat="server" AutoGenerateColumns="False" class="table table-bordered table-hover">
                                <Columns>
                                    <asp:TemplateField HeaderText="Select">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkRow" runat="server" ></asp:CheckBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Id" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblId" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        </div>                

                      <div class="panel panel-default">
                            <div class="panel-heading">Service Details</div>
                                <div class="panel-body">
                                    <asp:GridView ID="GridViewServiceDesc" runat="server"
                                                GridLines="None" AutoGenerateColumns="False"
                                                OnRowCommand="GridViewServiceDesc_RowCommand"
                                                OnRowEditing="GridViewServiceDesc_RowEditing"                                                
                                                OnRowDataBound="GridViewServiceDesc_RowDataBound"
                                                OnRowUpdating="GridViewServiceDesc_RowUpdating" 
                                                OnRowDeleting="GridViewServiceDesc_RowDeleting"
                                                ShowFooter="True"
                                                class="table table-bordered table-hover btn-group-justified" >                                                
                                        <Columns>
                                            <asp:TemplateField HeaderText="Id" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblId" runat="server" Text='<%# Bind("Id") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Service Desc"  HeaderStyle-Width="50%">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtServiceDesc" runat="server" Text='<%# Bind("ServiceDetail") %>' CssClass="form-control" MaxLength="128"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="valServiceDesc" runat="server" ControlToValidate="txtServiceDesc"
                                                        Display="Dynamic" ErrorMessage="Service Desc is required." ForeColor="Red" SetFocusOnError="True"
                                                        ValidationGroup="editGrp">*</asp:RequiredFieldValidator>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblServiceDesc" runat="server" Text='<%# Bind("ServiceDetail") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="txtServiceDescNew" runat="server" CssClass="form-control" MaxLength="128"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="valServiceDescNew" runat="server" ControlToValidate="txtServiceDescNew"
                                                        Display="Dynamic" ErrorMessage="Service Desc is required." ForeColor="Red" SetFocusOnError="True"
                                                        ValidationGroup="newGrp">*</asp:RequiredFieldValidator>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Start Date"  HeaderStyle-Width="15%">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtStartDate" runat="server" Text='<%# String.Format("{0:MM/dd/yyyy}", Eval("StartDate"))%>' CssClass="form-control" MaxLength="10"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="valStartDate" runat="server" ControlToValidate="txtStartDate"
                                                        Display="Dynamic" ErrorMessage="Start Date is required." ForeColor="Red" SetFocusOnError="True"
                                                        ValidationGroup="editGrp">*</asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate = "txtStartDate"
                                                        ValidationExpression = "(0[1-9]|1[0-2])\/(((0|1)[1-9]|2[1-9]|3[0-1])\/((19|20)\d\d))$"
                                                        runat="server" SetFocusOnError="true" Display="Dynamic" ForeColor="Red">*
                                                    </asp:RegularExpressionValidator>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStartDate" runat="server" Text='<%# String.Format("{0:MM/dd/yyyy}", Eval("StartDate"))%>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="txtStartDateNew" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="valStartDateNew" runat="server" ControlToValidate="txtStartDateNew"
                                                        Display="Dynamic" ErrorMessage="Start Date is required." ForeColor="Red" SetFocusOnError="True"
                                                        ValidationGroup="newGrp">*</asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate = "txtStartDateNew"
                                                        ValidationExpression = "(0[1-9]|1[0-2])\/(((0|1)[1-9]|2[1-9]|3[0-1])\/((19|20)\d\d))$"
                                                        runat="server" SetFocusOnError="true" Display="Dynamic" ForeColor="Red">*
                                                    </asp:RegularExpressionValidator>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="End Date"  HeaderStyle-Width="15%">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtEndDate" runat="server" Text='<%# String.Format("{0:MM/dd/yyyy}", Eval("EndDate"))%>' CssClass="form-control" MaxLength="10"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="valEndDate" runat="server" ControlToValidate="txtEndDate"
                                                        Display="Dynamic" ErrorMessage="End Date is required." ForeColor="Red" SetFocusOnError="True"
                                                        ValidationGroup="editGrp">*</asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" ControlToValidate = "txtEndDate"
                                                        ValidationExpression = "(0[1-9]|1[0-2])\/(((0|1)[1-9]|2[1-9]|3[0-1])\/((19|20)\d\d))$"
                                                        runat="server" SetFocusOnError="true" Display="Dynamic" ForeColor="Red">*
                                                    </asp:RegularExpressionValidator>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEndDate" runat="server" Text='<%# String.Format("{0:MM/dd/yyyy}", Eval("EndDate"))%>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="txtEndDateNew" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="valEndDateNew" runat="server" ControlToValidate="txtEndDateNew"
                                                        Display="Dynamic" ErrorMessage="End Date is required." ForeColor="Red" SetFocusOnError="True"
                                                        ValidationGroup="newGrp">*</asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator4" ControlToValidate = "txtEndDateNew"
                                                        ValidationExpression = "(0[1-9]|1[0-2])\/(((0|1)[1-9]|2[1-9]|3[0-1])\/((19|20)\d\d))$"
                                                        runat="server" SetFocusOnError="true" Display="Dynamic" ForeColor="Red">*
                                                    </asp:RegularExpressionValidator>
                                                </FooterTemplate>
                                            </asp:TemplateField>                                                                                                  
                                                                                                      
                                            <asp:TemplateField HeaderText="Edit"  HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkEdit" runat="server" Text="" CommandName="Edit" ToolTip="Edit" 
                                                        CommandArgument=''><img src="images/icon-edit.png" /> </asp:LinkButton>
                                                    <asp:LinkButton ID="lnkDelete" runat="server" Text="Delete" CommandName="Delete" 
                                                        ToolTip="Delete" OnClientClick='return confirm("Are you sure you want to delete this record?");'
                                                        CommandArgument=''><img src="images/icon-delete.png" /></asp:LinkButton>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:LinkButton ID="lnkInsert" runat="server" Text="" ValidationGroup="editGrp" CommandName="Update" ToolTip="Save"
                                                        CommandArgument=''><img src="images/icon-save.gif" /></asp:LinkButton>
                                                    <asp:LinkButton ID="lnkCancel" runat="server" Text="" CommandName="Cancel" ToolTip="Cancel"
                                                        CommandArgument=''><img src="images/icon-cancel.gif" /></asp:LinkButton>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:LinkButton ID="lnkInsert" runat="server" Text="" ValidationGroup="newGrp" CommandName="InsertNew" ToolTip="Add New Entry"
                                                        CommandArgument=''><img src="images/icon-save.gif" /></asp:LinkButton>
                                                    <asp:LinkButton ID="lnkCancel" runat="server" Text="" CommandName="CancelNew" ToolTip="Cancel"
                                                        CommandArgument=''><img src="images/icon-cancel.gif" /></asp:LinkButton>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                        </div>

                      <asp:UpdatePanel ID="upEdit" runat="server">
                          <ContentTemplate>
                              <div class="modal-body">
                                  <div class="row">
                                      <div class="hidden">
                                          <asp:DropDownList ID="ddlPhaseHdn" runat="server" CssClass="form-control">
                                          </asp:DropDownList>
                                      </div>
                                      <div class="col-xs-12 col-md-12">
                                          <asp:GridView ID="gvPhase" runat="server" AutoGenerateColumns="False"
                                              ShowFooter="True"
                                              OnRowDataBound="gvPhase_RowDataBound"
                                              OnRowDeleting="gvPhase_RowDeleting"
                                              class="table table-striped table-bordered">
                                              <Columns>
                                                  <asp:TemplateField HeaderText="Id" Visible="false">
                                                      <ItemTemplate>
                                                          <asp:Label ID="lblId" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                                                      </ItemTemplate>
                                                  </asp:TemplateField>
                                                  <asp:TemplateField HeaderText="Phase" HeaderStyle-Width="15%">
                                                      <ItemTemplate>
                                                          <asp:Label ID="lblPhase" runat="server" Text='<%# Eval("Name") %>' Visible="false" />
                                                          <asp:DropDownList ID="ddlPhase" runat="server" CssClass="form-control" onchange="selectPhase(this)">
                                                          </asp:DropDownList>
                                                      </ItemTemplate>
                                                      <FooterStyle HorizontalAlign="Left" />
                                                      <FooterTemplate>
                                                          <asp:Button ID="btnAddPhase" runat="server" CssClass="btn btn-info"
                                                              Text="Add New" OnClick="btnAddPhase_Click" />
                                                      </FooterTemplate>
                                                  </asp:TemplateField>
                                                  <asp:TemplateField HeaderText="Start Date (mm/dd/yyyy)" HeaderStyle-Width="15%">
                                                      <ItemTemplate>
                                                          <asp:TextBox ID="txtStartDate" runat="server" class="form-control" Text='<%# Eval("StartDate") %>'></asp:TextBox>
                                                      </ItemTemplate>
                                                  </asp:TemplateField>
                                                  <asp:TemplateField HeaderText="Title" HeaderStyle-Width="45%">
                                                      <ItemTemplate>
                                                          <asp:TextBox ID="txtTitle" runat="server" class="form-control" Text='<%# Eval("Title") %>'></asp:TextBox>
                                                      </ItemTemplate>
                                                  </asp:TemplateField>
                                                  <asp:TemplateField HeaderText="Ms Hrs" HeaderStyle-Width="10%">
                                                      <ItemTemplate>
                                                          <asp:TextBox ID="txtMsHrs" runat="server" class="form-control" Text='<%# Eval("MsHrs") %>'></asp:TextBox>
                                                      </ItemTemplate>
                                                  </asp:TemplateField>
                                                  <asp:TemplateField HeaderText="Phd Hrs" HeaderStyle-Width="10%">
                                                      <ItemTemplate>
                                                          <asp:TextBox ID="txtPhdHrs" runat="server" class="form-control" Text='<%# Eval("PhdHrs") %>'></asp:TextBox>
                                                      </ItemTemplate>
                                                  </asp:TemplateField>

                                                  <asp:TemplateField HeaderText="Del" HeaderStyle-Width="5%">
                                                      <ItemTemplate>
                                                          <asp:LinkButton ID="lnkDelete" runat="server" Text="Delete" CommandName="Delete"
                                                              ToolTip="Delete" OnClientClick='return confirm("Are you sure you want to delete this entry?");'
                                                              CommandArgument=''><img src="images/icon-delete.png" /></asp:LinkButton>
                                                      </ItemTemplate>

                                                  </asp:TemplateField>
                                              </Columns>
                                          </asp:GridView>

                                      </div>
                                  </div>
                              </div>

                          </ContentTemplate>
                          <Triggers>
                              <%-- <asp:AsyncPostBackTrigger ControlID="rptInvoice" EventName="ItemCommand" />--%>
                              <%--<asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />--%>
                              <%--<asp:AsyncPostBackTrigger ControlID="gvInvoiceItem" EventName="SelectedIndexChanged" />--%>
                              <%--<asp:AsyncPostBackTrigger ControlID="btnAddInvoiceItem" />--%>
                              <%--<asp:PostBackTrigger ControlID="btnSave" />--%>
                          </Triggers>
                      </asp:UpdatePanel>

                    </div>
                </div>

              

          </div>

          <div class="tab-pane" id="tab7">
              <div class="panel panel-default">
                  <div class="panel-heading">Final</div>
                  <div class="panel-body">                     
                                                                
                      <div class="row" id="divProjectType">
                          <%--<div class="col-xs-6 col-md-2">--%>
                            <label class="col-md-2 control-label">Project Type:</label>
                          <%--</div>--%>  
                          <div class="col-md-6">                              
                              <%--<div class="radio">--%>
                                  <label class="radio-inline">
                                      <input type="radio" name="optionsRadios" id="optionsRadios1" value="option1"  >
                                      Consult
                                  </label>
                                  <label class="radio-inline">
                                      <input type="radio" name="optionsRadios" id="optionsRadios2" value="option2"   checked>
                                      Project
                                  </label>
                              <%--</div>--%>
                          </div>
                          <%--<div class="col-xs-6 col-md-1">
                          </div> --%>                         
                      </div>
                      <br />
                      <div id="divAdmin" runat="server">
                          <div class="panel panel-default">
                              <div class="panel-heading">Admin</div>
                              <div class="panel-body">

                                  <div class="panel panel-default hidden">
                                      <div class="panel-heading">Invoice & Request</div>
                                      <div class="panel-body">
                                      <div class="row">
                                          <div class="col-md-4">
                                              <!-- Add Button -->
                                              <asp:Button ID="btnAddInvoice" class="btn btn-link" Text="Add Invoice" OnClick="btnAddInvoice_Click" runat="server"></asp:Button>
                                          </div>
                                          <%--<div class="col-md-6">
                                              <asp:Button ID="btnGetHours" runat="server" class="btn btn-primary" Text="Get Work Hours"/>
                                           </div>--%>
                                      </div>
                                      <div class="row" id="gridArea">
                                          <div class="col-md-6">
                                              <div class="table-responsive">
                                                  <asp:DetailsView ID="dvInvoice" runat="Server" CellPadding="5" Visible="false"
                                                      AllowPaging="True" AutoGenerateRows="False" DataKeyNames="Id"
                                                     
                                                      OnPreRender="dvInvoice_PreRender"
                                                      OnItemUpdating="dvInvoice_ItemUpdating"
                                                      OnItemInserting="dvInvoice_ItemInserting"
                                                      OnItemDeleting="dvInvoice_ItemDeleting"
                                                      OnModeChanging="dvInvoice_ModeChanging"
                                                      OnPageIndexChanging="dvInvoice_PageIndexChanging"
                                                      class="table table-bordered table-hover btn-group-justified">
                                                      <FieldHeaderStyle Font-Bold="True" Width="40%" />
                                                      <RowStyle Width="60%" />
                                                      <Fields>
                                                          <asp:TemplateField HeaderText="Id" Visible="false">
                                                              <ItemTemplate>
                                                                  <asp:Label ID="lblProjectInvoiceID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Id") %>'></asp:Label>
                                                              </ItemTemplate>
                                                          </asp:TemplateField>
                                                          <asp:TemplateField HeaderText="Request Number">
                                                              <EditItemTemplate>
                                                                  <asp:TextBox ID="txtRequestNumber" runat="server" CssClass="form-control" Text='<%# DataBinder.Eval(Container.DataItem,"RequestNumber") %>'></asp:TextBox>
                                                              </EditItemTemplate>
                                                              <InsertItemTemplate>
                                                                  <asp:TextBox ID="txtRequestNumberNew" runat="server" CssClass="form-control"></asp:TextBox>
                                                              </InsertItemTemplate>
                                                              <ItemTemplate>
                                                                  <asp:Label ID="lblRequestNumber" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"RequestNumber") %>'></asp:Label>
                                                              </ItemTemplate>
                                                          </asp:TemplateField>
                                                          <asp:TemplateField HeaderText="Request Date">
                                                              <EditItemTemplate>
                                                                  <asp:TextBox ID="txtRequestDate" runat="server" CssClass="form-control" Text='<%# String.Format("{0:MM/dd/yyyy}", DataBinder.Eval(Container.DataItem,"RequestDate")) %>'></asp:TextBox>
                                                                      <%--Text='<%# String.Format("{0:MM/dd/yyyy}", (bool)((int)DataBinder.Eval(Container.DataItem, "Id") > 0) ? DataBinder.Eval(Container.DataItem,"RequestDate") : string.Empty) %>'></asp:TextBox>--%>
                                                              </EditItemTemplate>
                                                              <InsertItemTemplate>
                                                                  <asp:TextBox ID="txtRequestDateNew" runat="server" CssClass="form-control"></asp:TextBox>
                                                              </InsertItemTemplate>
                                                              <ItemTemplate>
                                                                  <asp:Label ID="lblRequestDate" runat="server" Text='<%# String.Format("{0:MM/dd/yyyy}", DataBinder.Eval(Container.DataItem,"RequestDate")) %>'></asp:Label>
                                                              </ItemTemplate>
                                                          </asp:TemplateField>
                                                          <asp:TemplateField HeaderText="Requested Hours Phd">
                                                              <EditItemTemplate>
                                                                  <asp:TextBox ID="txtRequestedPhd" runat="server" CssClass="form-control" Text='<%# DataBinder.Eval(Container.DataItem,"RequestedPhd") %>'></asp:TextBox>
                                                              </EditItemTemplate>
                                                              <InsertItemTemplate>
                                                                  <asp:TextBox ID="txtRequestedPhdNew" runat="server" CssClass="form-control"></asp:TextBox>
                                                              </InsertItemTemplate>
                                                              <ItemTemplate>
                                                                  <asp:Label ID="lblRequestedPhd" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"RequestedPhd") %>'></asp:Label>
                                                              </ItemTemplate>
                                                          </asp:TemplateField>
                                                          <asp:TemplateField HeaderText="Requested Hours ms">
                                                              <EditItemTemplate>
                                                                  <asp:TextBox ID="txtRequestedMS" runat="server" CssClass="form-control" Text='<%# DataBinder.Eval(Container.DataItem,"RequestedMS") %>'></asp:TextBox>
                                                              </EditItemTemplate>
                                                              <InsertItemTemplate>
                                                                  <asp:TextBox ID="txtRequestedMSNew" runat="server" CssClass="form-control"></asp:TextBox>
                                                              </InsertItemTemplate>
                                                              <ItemTemplate>
                                                                  <asp:Label ID="lblRequestedMS" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"RequestedMS") %>'></asp:Label>
                                                              </ItemTemplate>
                                                          </asp:TemplateField>

                                                          <asp:TemplateField HeaderText="BQHS Approve Date">
                                                              <EditItemTemplate>
                                                                  <asp:TextBox ID="txtRequestApproveDate" runat="server" CssClass="form-control" Text='<%# String.Format("{0:MM/dd/yyyy}", DataBinder.Eval(Container.DataItem,"RequestApproveDate")) %>'></asp:TextBox>
                                                              </EditItemTemplate>
                                                              <InsertItemTemplate>
                                                                  <asp:TextBox ID="txtRequestApproveDateNew" runat="server" CssClass="form-control"></asp:TextBox>
                                                              </InsertItemTemplate>
                                                              <ItemTemplate>
                                                                  <asp:Label ID="lblRequestApproveDate" runat="server" Text='<%# String.Format("{0:MM/dd/yyyy}", DataBinder.Eval(Container.DataItem,"RequestApproveDate")) %>'></asp:Label>
                                                              </ItemTemplate>
                                                          </asp:TemplateField>

                                                          <asp:TemplateField HeaderText="Approved Hours Phd">
                                                              <EditItemTemplate>
                                                                  <asp:TextBox ID="txtApprovedPhd" runat="server" CssClass="form-control" Text='<%# DataBinder.Eval(Container.DataItem,"ApprovedPhd") %>'></asp:TextBox>
                                                              </EditItemTemplate>
                                                              <InsertItemTemplate>
                                                                  <asp:TextBox ID="txtApprovedPhdNew" runat="server" CssClass="form-control"></asp:TextBox>
                                                              </InsertItemTemplate>
                                                              <ItemTemplate>
                                                                  <asp:Label ID="lblAprovedPhd" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ApprovedPhd") %>'></asp:Label>
                                                              </ItemTemplate>
                                                          </asp:TemplateField>
                                                          <asp:TemplateField HeaderText="Approved Hours ms">
                                                              <EditItemTemplate>
                                                                  <asp:TextBox ID="txtApprovedMS" runat="server" CssClass="form-control" Text='<%# DataBinder.Eval(Container.DataItem,"ApprovedMS") %>'></asp:TextBox>
                                                              </EditItemTemplate>
                                                              <InsertItemTemplate>
                                                                  <asp:TextBox ID="txtApprovedMSNew" runat="server" CssClass="form-control"></asp:TextBox>
                                                              </InsertItemTemplate>
                                                              <ItemTemplate>
                                                                  <asp:Label ID="lblAprovedMS" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ApprovedMS") %>'></asp:Label>
                                                              </ItemTemplate>
                                                          </asp:TemplateField>

                                                          <asp:TemplateField HeaderText="Client Approve Date">
                                                              <EditItemTemplate>
                                                                  <asp:TextBox ID="txtClientApproveDate" runat="server" CssClass="form-control" Text='<%# String.Format("{0:MM/dd/yyyy}", DataBinder.Eval(Container.DataItem,"ClientApproveDate")) %>'></asp:TextBox>
                                                              </EditItemTemplate>
                                                              <InsertItemTemplate>
                                                                  <asp:TextBox ID="txtClientApproveDateNew" runat="server" CssClass="form-control"></asp:TextBox>
                                                              </InsertItemTemplate>
                                                              <ItemTemplate>
                                                                  <asp:Label ID="lblClientApproveDate" runat="server" Text='<%# String.Format("{0:MM/dd/yyyy}", DataBinder.Eval(Container.DataItem,"ClientApproveDate")) %>'></asp:Label>
                                                              </ItemTemplate>
                                                          </asp:TemplateField>

                                                          <asp:TemplateField HeaderText="From Date">
                                                              <EditItemTemplate>
                                                                  <asp:TextBox ID="txtInvoiceFromDate" runat="server" CssClass="form-control"
                                                                      Text='<%# String.Format("{0:MM/dd/yyyy}", DataBinder.Eval(Container.DataItem,"FromDate")) %>'></asp:TextBox>
                                                              </EditItemTemplate>
                                                              <InsertItemTemplate>
                                                                  <asp:TextBox ID="txtInvoiceFromDateNew" runat="server" CssClass="form-control"></asp:TextBox>
                                                              </InsertItemTemplate>
                                                              <ItemTemplate>
                                                                  <asp:Label ID="lblInvoiceFromDate" runat="server" Text='<%# String.Format("{0:MM/dd/yyyy}", DataBinder.Eval(Container.DataItem,"FromDate")) %>'></asp:Label>
                                                              </ItemTemplate>
                                                          </asp:TemplateField>
                                                          <asp:TemplateField HeaderText="To Date">
                                                              <EditItemTemplate>
                                                                  <asp:TextBox ID="txtInvoiceToDate" runat="server" CssClass="form-control" Text='<%# String.Format("{0:MM/dd/yyyy}", DataBinder.Eval(Container.DataItem,"ToDate")) %>'></asp:TextBox>                                                                  
                                                              </EditItemTemplate>
                                                              <InsertItemTemplate>
                                                                  <asp:TextBox ID="txtInvoiceToDateNew" runat="server" CssClass="form-control"></asp:TextBox>
                                                              </InsertItemTemplate>
                                                              <ItemTemplate>
                                                                  <asp:Label ID="lblInvoiceToDate" runat="server" Text='<%# String.Format("{0:MM/dd/yyyy}", DataBinder.Eval(Container.DataItem,"ToDate")) %>'></asp:Label>                                                                  
                                                              </ItemTemplate>
                                                          </asp:TemplateField>

                                                          <asp:TemplateField HeaderText="Worked Hours Phd">
                                                              <%--<EditItemTemplate>
                                                    <asp:TextBox ID="txtWorkedPhd" runat="server" CssClass="form-control"></asp:TextBox>
                                                </EditItemTemplate> 
                                                 <InsertItemTemplate>
                                                    <asp:TextBox ID="txtWorkedPhdNew" runat="server" CssClass="form-control"></asp:TextBox>
                                                </InsertItemTemplate>--%>
                                                              <ItemTemplate>
                                                                  <asp:Label ID="lblWorkedPhd" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"WorkedPhd") %>'></asp:Label>
                                                              </ItemTemplate>
                                                          </asp:TemplateField>
                                                          <asp:TemplateField HeaderText="Worked Hours ms">
                                                              <%--<EditItemTemplate>
                                                    <asp:TextBox ID="txtWorkedMS" runat="server" CssClass="form-control"></asp:TextBox>
                                                </EditItemTemplate> 
                                                 <InsertItemTemplate>
                                                    <asp:TextBox ID="txtWorkedMSNew" runat="server" CssClass="form-control"></asp:TextBox>
                                                </InsertItemTemplate>--%>
                                                              <ItemTemplate>
                                                                  <asp:Label ID="lblWorkedMS" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"WorkedMS") %>'></asp:Label>
                                                              </ItemTemplate>
                                                          </asp:TemplateField>
                                                          <asp:TemplateField HeaderText="Invoiced Hours Phd">
                                                              <EditItemTemplate>
                                                                  <asp:TextBox ID="txtInvoicedPhd" runat="server" CssClass="form-control" Text='<%# DataBinder.Eval(Container.DataItem,"InvoicedPhd") %>'></asp:TextBox>
                                                              </EditItemTemplate>
                                                              <InsertItemTemplate>
                                                                  <asp:TextBox ID="txtInvoicedPhdNew" runat="server" CssClass="form-control"></asp:TextBox>
                                                              </InsertItemTemplate>
                                                              <ItemTemplate>
                                                                  <asp:Label ID="lblInvoicedPhd" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"InvoicedPhd") %>'></asp:Label>
                                                              </ItemTemplate>
                                                          </asp:TemplateField>
                                                          <asp:TemplateField HeaderText="Invoiced Hours MS">
                                                              <EditItemTemplate>
                                                                  <asp:TextBox ID="txtInvoicedMS" runat="server" CssClass="form-control" Text='<%# DataBinder.Eval(Container.DataItem,"InvoicedMS") %>'></asp:TextBox>
                                                              </EditItemTemplate>
                                                              <InsertItemTemplate>
                                                                  <asp:TextBox ID="txtInvoicedMSNew" runat="server" CssClass="form-control"></asp:TextBox>
                                                              </InsertItemTemplate>
                                                              <ItemTemplate>
                                                                  <asp:Label ID="lblInvoicePhd" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"InvoicedMS") %>'></asp:Label>
                                                              </ItemTemplate>
                                                          </asp:TemplateField>
                                                          <asp:TemplateField HeaderText="Invoice Date">
                                                              <EditItemTemplate>
                                                                  <asp:TextBox ID="txtInvoiceDate" runat="server" CssClass="form-control" Text='<%# String.Format("{0:MM/dd/yyyy}", DataBinder.Eval(Container.DataItem,"InvoiceDate")) %>'></asp:TextBox>
                                                              </EditItemTemplate>
                                                              <InsertItemTemplate>
                                                                  <asp:TextBox ID="txtInvoiceDateNew" runat="server" CssClass="form-control"></asp:TextBox>
                                                              </InsertItemTemplate>
                                                              <ItemTemplate>
                                                                  <asp:Label ID="lblInvoiceDate" runat="server" Text='<%# String.Format("{0:MM/dd/yyyy}", DataBinder.Eval(Container.DataItem,"InvoiceDate")) %>'></asp:Label>
                                                              </ItemTemplate>
                                                          </asp:TemplateField>
                                                          <asp:TemplateField HeaderText="Invoice Number">
                                                              <EditItemTemplate>
                                                                  <asp:TextBox ID="txtInvoiceNumber" runat="server" CssClass="form-control" Text='<%# DataBinder.Eval(Container.DataItem,"InvoiceNumber") %>'></asp:TextBox>
                                                              </EditItemTemplate>
                                                              <InsertItemTemplate>
                                                                  <asp:TextBox ID="txtInvoiceNumberNew" runat="server" CssClass="form-control"></asp:TextBox>
                                                              </InsertItemTemplate>
                                                              <ItemTemplate>
                                                                  <asp:Label ID="lblInvoiceNumber" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"InvoiceNumber") %>'></asp:Label>
                                                              </ItemTemplate>
                                                          </asp:TemplateField>

                                                          <asp:TemplateField HeaderText="Comment">
                                                              <EditItemTemplate>
                                                                  <asp:TextBox ID="txtComment" runat="server" CssClass="form-control" Text='<%# DataBinder.Eval(Container.DataItem,"Comment") %>'></asp:TextBox>
                                                              </EditItemTemplate>
                                                              <InsertItemTemplate>
                                                                  <asp:TextBox ID="txtCommentNew" runat="server" CssClass="form-control"></asp:TextBox>
                                                              </InsertItemTemplate>
                                                              <ItemTemplate>
                                                                  <asp:Label ID="lblComment" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Comment") %>'></asp:Label>
                                                              </ItemTemplate>
                                                          </asp:TemplateField>
                                                          
                                                          <asp:CommandField ShowDeleteButton="True" ShowInsertButton="True" ShowEditButton="True" ButtonType="Button" ControlStyle-CssClass="btn btn-primary" />
                                                      </Fields>

                                                  </asp:DetailsView>
                                              </div>
                                          </div>
                                      </div>
                                    </div>
                                </div>
                                  
                                      <%--<div class="row">
                                        <div class="col-md-3">
                                            Start Date
                                            <div class='input-group date' id='datetimepickerInvoiceStart'>
                                                <span class="input-group-addon">
                                                    <span class="glyphicon glyphicon-calendar"></span>
                                                </span>
                                                <asp:TextBox ID="TextBoxInvoiceStartDate" runat="server" class="form-control" ></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                        End Date
                                            <div class='input-group date' id='datetimepickerInvoiceEnd'>
                                                <span class="input-group-addon">
                                                    <span class="glyphicon glyphicon-calendar"></span>
                                                </span>
                                                <asp:TextBox ID="TextBoxInvoiceEndDate" runat="server" class="form-control" ></asp:TextBox>
                                            </div>
                                        </div>
                                          <div class="col-md-2">Get Hours
                                              <div>
                                                <asp:Button ID="btnGetHours" runat="server" Text="Get Hours" class="btn btn-info" OnClientClick="GetProjectHours(); return false" />                                                  
                                                </div>
                                          </div>
                                           <div class="col-md-1">Phd
                                                <input class="form-control" id="txtPhdHours" type="text" placeholder="0.0" disabled runat="server">
                                            </div>
                                           <div class="col-md-1">Master
                                                <input class="form-control" id="txtMsHours" type="text" placeholder="0.0" disabled runat="server">
                                            </div>                                           
                                       </div>--%>
                                                                    
                                  <%--<asp:GridView ID="GridViewInvoice" runat="server" 
                                        GridLines="None" AutoGenerateColumns="False"
                                        OnRowCommand="GridViewInvoice_RowCommand"
                                        OnRowEditing="GridViewInvoice_RowEditing"
                                        OnRowCancelingEdit="GridViewInvoice_RowCancelingEdit"
                                        OnRowDataBound="GridViewInvoice_RowDataBound"
                                        OnRowUpdating="GridViewInvoice_RowUpdating" 
                                        OnRowDeleting="GridViewInvoice_RowDeleting"
                                        ShowFooter="True"
                                        class="table table-bordered table-hover btn-group-justified"
                                    > 
                                    <Columns> 
                                        <asp:TemplateField HeaderText="Id"> 
                                            <ItemTemplate> 
                                                <asp:Label ID="lblProjectInvoiceID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Id") %>'></asp:Label> 
                                            </ItemTemplate> 
                                            <FooterTemplate> 
                                                <asp:TextBox ID="txtProjectInvoiceId" runat="server"></asp:TextBox> 
                                            </FooterTemplate> 
                   
                                        </asp:TemplateField> 
                                        <asp:TemplateField HeaderText="Request Number"> 
                                            <ItemTemplate> 
                                                <asp:Label ID="lblRequestNumber" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"RequestNumber") %>'></asp:Label> 
                                            </ItemTemplate> 
                                            <FooterTemplate> 
                                                <asp:TextBox ID="txtRequestNumber" runat="server"></asp:TextBox> 
                                            </FooterTemplate> 
                                        </asp:TemplateField> 
                                        <asp:TemplateField HeaderText="Request Date"> 
                                            <ItemTemplate> 
                                                <asp:Label ID="lblRequestDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"RequestDate") %>'></asp:Label> 
                                            </ItemTemplate> 
                                            <FooterTemplate> 
                                                <asp:TextBox ID="txtRequestDate" runat="server"></asp:TextBox> 
                                            </FooterTemplate> 
                                        </asp:TemplateField> 
                                        <asp:TemplateField HeaderText="Requested Hours Phd"> 
                                            <ItemTemplate> 
                                                <asp:Label ID="lblRequestedPhd" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"RequestedPhd") %>'></asp:Label> 
                                            </ItemTemplate> 
                                            <FooterTemplate> 
                                                <asp:TextBox ID="txtRequestedPhd" runat="server"></asp:TextBox> 
                                            </FooterTemplate> 
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Requested Hours ms"> 
                                            <ItemTemplate> 
                                                <asp:Label ID="lblRequestedMS" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"RequestedMS") %>'></asp:Label> 
                                            </ItemTemplate> 
                                            <FooterTemplate> 
                                                <asp:TextBox ID="txtRequestedMS" runat="server"></asp:TextBox> 
                                            </FooterTemplate> 
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Worked Hours Phd"> 
                                            <ItemTemplate> 
                                                <asp:Label ID="lblWorkedPhd" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"WorkedPhd") %>'></asp:Label> 
                                            </ItemTemplate> 
                                            <FooterTemplate> 
                                                <asp:TextBox ID="txtWorkedPhd" runat="server"></asp:TextBox> 
                                            </FooterTemplate> 
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Worked Hours ms"> 
                                            <ItemTemplate> 
                                                <asp:Label ID="lblWorkedMS" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"WorkedMS") %>'></asp:Label> 
                                            </ItemTemplate> 
                                            <FooterTemplate> 
                                                <asp:TextBox ID="txtWorkedMS" runat="server"></asp:TextBox> 
                                            </FooterTemplate> 
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Invoice Number"> 
                                            <ItemTemplate> 
                                                <asp:Label ID="lblInvoiceNumber" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"InvoiceNumber") %>'></asp:Label> 
                                            </ItemTemplate> 
                                            <FooterTemplate> 
                                                <asp:TextBox ID="txtInvoiceNumber" runat="server"></asp:TextBox> 
                                            </FooterTemplate> 
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="From Date"> 
                                            <ItemTemplate> 
                                                <asp:Label ID="lblInvoiceFromDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"FromDate") %>'></asp:Label> 
                                            </ItemTemplate> 
                                            <FooterTemplate> 
                                                <asp:TextBox ID="txtInvoiceFromDate" runat="server"></asp:TextBox> 
                                            </FooterTemplate> 
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="To Date"> 
                                            <ItemTemplate> 
                                                <asp:Label ID="lblInvoiceToDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ToDate") %>'></asp:Label> 
                                            </ItemTemplate> 
                                            <FooterTemplate> 
                                                <asp:TextBox ID="txtInvoiceToDate" runat="server"></asp:TextBox> 
                                            </FooterTemplate> 
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Edit"  HeaderStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkEdit" runat="server" Text="" CommandName="Edit" ToolTip="Edit" 
                                                    CommandArgument=''><img src="images/icon-edit.png" /> </asp:LinkButton>
                                                <asp:LinkButton ID="lnkDelete" runat="server" Text="Delete" CommandName="Delete" 
                                                    ToolTip="Delete" OnClientClick='return confirm("Are you sure you want to delete this invoice?");'
                                                    CommandArgument=''><img src="images/icon-delete.png" /></asp:LinkButton>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:LinkButton ID="lnkInsert" runat="server" Text="" ValidationGroup="editGrp" CommandName="Update" ToolTip="Save"
                                                    CommandArgument=''><img src="images/icon-save.gif" /></asp:LinkButton>
                                                <asp:LinkButton ID="lnkCancel" runat="server" Text="" CommandName="Cancel" ToolTip="Cancel"
                                                    CommandArgument=''><img src="images/icon-cancel.gif" /></asp:LinkButton>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:LinkButton ID="lnkInsert" runat="server" Text="" ValidationGroup="newGrp" CommandName="InsertNew" ToolTip="Add New Entry"
                                                    CommandArgument=''><img src="images/icon-save.gif" /></asp:LinkButton>
                                                <asp:LinkButton ID="lnkCancel" runat="server" Text="" CommandName="CancelNew" ToolTip="Cancel"
                                                    CommandArgument=''><img src="images/icon-cancel.gif" /></asp:LinkButton>
                                            </FooterTemplate>
                                        </asp:TemplateField> 
                                    </Columns> 
                                </asp:GridView>--%>

                                  <div class="panel panel-default">
                                      <div class="panel-heading">
                                          <div class="checkbox">
                                              <label>
                                                  <input type="checkbox" id="chkRmatrix" name="chkRmatrix">RMATRIX-II Request for Resources
                                              </label>&nbsp;
                                              <label>
                                                  <input type="checkbox" id="chkNotReportRmatrix" name="chkNotReportRmatrix">Do Not Report to RMATRIX
                                              </label>
                                          </div>
                                      </div>
                                      <div class="panel-body" id="divRmatrix">
                                          <div class="row">
                                              <div class="col-md-3">
                                                  Request Number(3 digits)
                                          <asp:TextBox ID="TextBoxRmatrixNum" runat="server" class="form-control" placeholder="Number"></asp:TextBox>
                                              </div>
                                              <div class="col-md-3">
                                                  Submission Date
                                          <div class='input-group date' id='datetimepicker5'>
                                              <span class="input-group-addon">
                                                  <span class="glyphicon glyphicon-calendar"></span>
                                              </span>
                                              <asp:TextBox ID="TextBoxRmatrixSubDate" runat="server" class="form-control" placeholder="Date Submitted"></asp:TextBox>
                                          </div>
                                              </div>
                                              <div class="col-md-3">
                                                  Received Date
                                          <div class='input-group date' id='datetimepicker6'>
                                              <span class="input-group-addon">
                                                  <span class="glyphicon glyphicon-calendar"></span>
                                              </span>
                                              <asp:TextBox ID="TextBoxRmatrixRcvDate" runat="server" class="form-control" placeholder="Date BQHS Received"></asp:TextBox>
                                          </div>
                                              </div>
                                          </div>
                                      </div>
                                  </div>

                                  <div class="panel panel-default">
                                      <div class="panel-heading"></div>
                                      <div class="panel-body">
                                          &nbsp;
                                          <div class="row">
                                              <div class="col-md-2">
                                                  Phase completion:   
                                              </div>
                                              <div class="col-md-6">
                                                  <table class="table">
                                                      <thead>
                                                          <tr>
                                                              <th>Phase</th>
                                                              <th>Ms Hrs</th>
                                                              <th>Phd Hrs</th>
                                                              <th>Completion Date</th>
                                                          </tr>
                                                      </thead>
                                                      <tbody>
                                                          <asp:Repeater ID="rptPhaseCompletion" runat="server">
                                                              <ItemTemplate>
                                                                  <tr>
                                                                      <td><%# Eval("Phase") %></td>
                                                                      <td><%# Eval("MsHrs") %></td>
                                                                      <td><%# Eval("PhdHrs") %></td>
                                                                      <td><asp:TextBox runat="server" ID="txtPhaseCompletionDate" Text='<%#Eval("CompletionDate") %>' class="form-control"></asp:TextBox>
                                                                      </td>
                                                                  </tr>
                                                              </ItemTemplate>
                                                          </asp:Repeater>
                                                      </tbody>
                                                  </table>
                                              </div>
                                              <div class="col-md-2">
                                              </div>
                                              <div class="col-md-2">
                                                  <asp:CheckBox ID="chkApproved" runat="server" Text="Approved" />
                                              </div>
                                          </div>

                                          <div class="row">
                                              <div class="col-md-2">
                                                  Analysis Completion Date:   
                                              </div>
                                              <div class="col-md-3">
                                                  <div class='input-group date' id='datetimepicker3'>
                                                      <span class="input-group-addon">
                                                          <span class="glyphicon glyphicon-calendar"></span>
                                                      </span>
                                                      <asp:TextBox ID="TextBoxAnalysisCompletionDate" runat="server" class="form-control" placeholder="Analysis Completion Date"></asp:TextBox>
                                                  </div>
                                              </div>
                                              <div class="col-md-2">
                                                  Project Completion Date:   
                                              </div>
                                              <div class="col-md-3">
                                                  <div class='input-group date' id='datetimepicker4'>
                                                      <span class="input-group-addon">
                                                          <span class="glyphicon glyphicon-calendar"></span>
                                                      </span>
                                                      <asp:TextBox ID="TextBoxProjectCompletionDate" runat="server" class="form-control" placeholder="Project Completion Date"></asp:TextBox>
                                                  </div>
                                              </div>
                                              
                                          </div>
                                          <br />
                                          <div class="row">
                                              <div class="col-md-2">
                                                  Comment:   
                                              </div>
                                              <div class="col-md-8">
                                                  <asp:TextBox ID="TextBoxComment" runat="server" class="form-control" placeholder="Comment"></asp:TextBox>
                                              </div>
                                              <div class="col-md-2">
                                                  <%--<button type="button" ID="btnSurvey" OnClick="ShowSurveyModal()"  class="btn btn-primary">Survey</button>--%>
                                                  <asp:Button class="btn btn-link" ID="btnSurvey" runat="server" Text="Survey" OnClick="btnSurvey_Click"></asp:Button>
                                              </div>                                              
                                          </div>
                                          <div class="row">
                                              <div class="col-md-2">
                                                <asp:Button ID="btnAddGrant" runat="server" Text="Add Grant" OnClick="btnAddGrant_Click" class="btn btn-link" />
                                              </div>
                                          </div>
                                      </div>
                                  </div>
                                  
                              </div>
                          </div>
                      </div>

                      <br />
                      <div class="row">
                          <div class="col-xs-6 col-md-10">
                          </div>
                          <div class="col-xs-6 col-md-2">
                              <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" class="btn btn-primary" OnClientClick="ClientSideClick(this)" UseSubmitBehavior="False" />
                          </div>
                      </div>
                  </div>
              </div>
          </div>

         <ul class="pager wizard">
            <li class="previous first" style="display: none;"><a href="#">First</a></li>
            <li class="previous"><a href="#">Previous</a></li>
            <li class="next last" style="display: none;"><a href="#">Last</a></li>
            <li class="next"><a href="#">Next</a></li>
         </ul>
      </div>
    </div>
    <div id="surveyModal" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title">Confirmation</h4>
                </div>
                <asp:UpdatePanel ID="upSurvey" runat="server">
                    <ContentTemplate>
                        <div class="modal-body">
                            <asp:Label ID="lblSurveyMsg" runat="server" ></asp:Label><br />
                            <div style="margin-left:.5in" id="divProjectInfo" runat="server">
                                Project title: <asp:Label ID="lblProjectTitle" runat="server"></asp:Label><br />
                                Faculty/Staff: <asp:Label ID="lblBiostats" runat="server"></asp:Label><br />
                                Project period: <asp:Label ID="lblProjectPeriod" runat="server"></asp:Label><br />
                                Service hours: <asp:Label ID="lblServiceHours" runat="server"></asp:Label><br />
                            </div>
                            <br />
                            <p class="text-warning">The survey can only be sent once.</p>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                            <asp:Button class="btn btn-primary" ID="btnSendSurvey" runat="server" Text="Submit" OnClick="btnSendSurvey_Click" OnClientClick="ClientSideClick(this)" UseSubmitBehavior="False"></asp:Button>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSurvey" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnSendSurvey" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="HiddenFieldCurrentDate" Value="" runat="server" />

    <asp:HiddenField ID="hdnInvoiceRequestNumber" Value="" runat="server" />
    <asp:HiddenField ID="hdnInvoiceRequestDate" Value="" runat="server" />
    <asp:HiddenField ID="hdnInvoiceRequestdHoursPhd" Value="" runat="server" />
    <asp:HiddenField ID="hdnInvoiceRequestedHoursMS" Value="" runat="server" />
    <asp:HiddenField ID="hdnInvoiceApprovedHoursPhd" Value="" runat="server" />
    <asp:HiddenField ID="hdnInvoiceApprovedHoursMS" Value="" runat="server" />
</asp:Content>
