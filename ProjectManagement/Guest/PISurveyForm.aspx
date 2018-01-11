<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PISurveyForm.aspx.cs" Inherits="ProjectManagement.Guest.PISurveyForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        h3 {
            overflow: hidden;
            text-align: center;
            font-weight: bold;
        }

            h3:before,
            h3:after {
                background-color: #000;
                content: "";
                display: inline-block;
                height: 1px;
                position: relative;
                vertical-align: middle;
                width: 50%;
            }

            h3:before {
                right: 0.5em;
                margin-left: -50%;
            }

            h3:after {
                left: 0.5em;
                margin-right: -50%;
            }

        .table > tbody > tr > th {
            font-weight: 100;
            font-size: 16px;
            text-align: center;
            padding-bottom: 3px;
            /*color:  #666666;*/
            padding-top: 3px;
            font-family: sans-serif, Verdana, Geneva, Arial, Helvetica, sans-serif;
            /*background-color: #EEEEEE;*/
            vertical-align: bottom;
            width: 10%;
        }

        .table tr {
            font-size: 16px;
            font-weight: 100;
        }

        input[type="checkbox"] {
            display: inline-block;
            width: 1em;
            height: 1em;
            margin-left: 40%;
            /*vertical-align:middle;*/
            text-align: center;
            /*background:url(check_radio_sheet.png) left top no-repeat;*/
            cursor: pointer;
        }

        label {
            /*display:inline-block;*/
            font-size: 16px;
            /*vertical-align:bottom;*/
        }
        img
        {   
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">    
    <img src="<%=Page.ResolveUrl("~/images/Banner_Print3.jpg")%>" class="img-rounded" />
    <br />
    <h3>JABSOM BQHS FOLLOW-UP SURVEY</h3>
    <%-- <div class="text-center"><h4>Project Title</h4></div>--%>
    <br />
    <div>
        <h4>1. How satisfied are you with each of the following?</h4>
    </div>
    <br />
    <div>
        <asp:GridView runat="server" ID="gvQuestion" DataKeyNames="ID" AutoGenerateColumns="false" GridLines="None" class="table table-striped table-hover">
            <Columns>
                <asp:TemplateField HeaderText="Id" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblId" runat="server" Text='<%# Bind("Id") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderStyle-Width="30%">
                    <ItemTemplate>
                        <asp:Label ID="lblGroupName" runat="server" Text='<%# Bind("Question") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Very Satisfied">
                    <ItemTemplate>
                        <asp:CheckBox ID="chk1" runat="server"></asp:CheckBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Satisfied">
                    <ItemTemplate>
                        <asp:CheckBox ID="chk2" runat="server"></asp:CheckBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Somewhat Satisfied">
                    <ItemTemplate>
                        <asp:CheckBox ID="chk3" runat="server"></asp:CheckBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Neutral">
                    <ItemTemplate>
                        <asp:CheckBox ID="chk4" runat="server"></asp:CheckBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Somewhat Dissatisfied">
                    <ItemTemplate>
                        <asp:CheckBox ID="chk5" runat="server"></asp:CheckBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Dissatisfied">
                    <ItemTemplate>
                        <asp:CheckBox ID="chk6" runat="server"></asp:CheckBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Very Dissatisfied">
                    <ItemTemplate>
                        <asp:CheckBox ID="chk7" runat="server"></asp:CheckBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="N/A">
                    <ItemTemplate>
                        <asp:CheckBox ID="chk8" runat="server"></asp:CheckBox>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <%--<hr />--%>
    </div>
    <br />
    <div>
        <h4>2.	Would you recommend your colleagues to BQHS for quantitative health sciences support?</h4>
        <div class="radio">
            <div class="col-xs-1 col-md-1"></div>
            <label>
                <input type="radio" name="options" id="option1" value="Yes" >
                Yes
            </label>
            &nbsp;           
            <label>
                <input type="radio" name="options" id="option2" value="No" >
                No
            </label>
        </div>
    </div>
    <br />
    <div>
        <h4>3.	We encourage you to provide additional comments, feedback, or suggestions. (character limit:
            <label id="charNum" for="charNum">500</label>)
        </h4>
    </div>
    <div class="row">
        <div class="col-xs-1 col-md-1"></div>
        <div class="col-xs-10 col-md-10">
            <asp:TextBox ID="txtComments" runat="server" class="form-control" TextMode="MultiLine" Rows="5"></asp:TextBox>
        </div>
    </div>
    <br />
    <div class="text-center" id="divSubmit">
        <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" class="btn" />
    </div>

    <div id="surveyModal" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title">Information</h4>
                </div>
                <asp:UpdatePanel ID="upSurvey" runat="server">
                    <ContentTemplate>
                        <div class="modal-body">
                            <span>
                                <h4>
                                    Thank you, but we've already received your feedback!
                                </h4>
                            </span>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                        <%--<asp:AsyncPostBackTrigger ControlID="btnSendSurvey" EventName="Click" />--%>
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hdnSurveyFormId" runat="server" Value='' />

    <script type="text/javascript">
        $(document).ready(function () {
            var url = window.location.href;
            var surveyId = url.substr(url.indexOf('?id=') + 4, url.length);
            
            if (typeof surveyId != 'undefined') {
                var uri = getBaseUrl() + '../../api/Project/GetSurveyStatus/?surveyId=' + surveyId;
                
                $.getJSON(uri).done(function (data) {
                    // On success
                    if (data)
                        $("#divSubmit").hide();
                });                
            }

            $("div.navbar").replaceWith("");

            var textarea = document.getElementById("MainContent_txtComments");

            $('#MainContent_txtComments').keydown(function (e) {
                var max = 499;
                var len = $(this).val().length;
                if (len >= max) {
                    $('#charNum').text(' you have reached the limit');
                    if (e.keyCode !== 8) {
                        e.preventDefault();
                        return false;
                    }
                } else {
                    var char = max - len;
                    $('#charNum').text(char);
                }
            });

            $('input[type="checkbox"]').on('change', function () {

                // uncheck sibling checkboxes (checkboxes on the same row)
                //$(this).siblings().prop('checked', false);

                var $this = $(this);
                var $row = $(this).closest("tr");
                $row.find('input[type="checkbox"]').each(function (i, e) {
                    $checkbox = $(e);
                    $checkbox.prop('checked', false);
                });

                $this.prop('checked', true);
                // uncheck checkboxes in the same column
                //$('div').find('input[type="checkbox"]:eq(' + $(this).index() + ')').not(this).prop('checked', false);

            });

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

        function wordCount(val) {
            var wom = val.match(/\S+/g);
            return {
                charactersNoSpaces: val.replace(/\s+/g, '').length,
                characters: val.length,
                words: wom ? wom.length : 0,
                lines: val.split(/\r*\n/).length
            };
        }

        function getBaseUrl() {
            var re = new RegExp(/^.*\//);
            return re.exec(window.location.href);
        }

        //function validateControl() {
        //    var isValid = false;

        //    if ($('#option1').is(':checked') || $('#option2').is(':checked')) {
        //        isValid = true;
        //    }

        //    if (!isValid) {
        //        document.getElementById('requiredLabel').style.display = "inline";
        //        alert("Please make your selections.");
        //    }

        //    return isValid;
        //}

    </script>
</asp:Content>
