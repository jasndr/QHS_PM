<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClientRequestForm.aspx.cs" Inherits="ProjectManagement.Guest.ClientRequestForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>BQHS Request Form</title>    
    <link href="~/Content/bootstrap.min.css" rel="stylesheet" />
    <script src="../Scripts/jquery-1.10.2.min.js"></script>    
    <script src="../Scripts/bootstrap-datepicker.min.js"></script>
    <script src="../Scripts/jquery.validate.js"></script>   
    <script src="../Scripts/bootstrap.min.js"></script>
    <%--<script src="../Scripts/jquery.cookie.js"></script>--%>
    
    <style>
        /*h3 {
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
        }*/
        body {
            padding-top: 0.5em;
            padding-left: 20em;
            padding-right: 20em;
            padding-bottom: 1.0em;
        }

        /* Wrapping element */
        /* Set some basic padding to keep content from hitting the edges */
        .body-content { 
            padding-top: 1.0em;           
            /*background-color: 	#f1f3f8;*/
            background-color: whitesmoke;
        }
        img
        {   
            width: 100%;
        }
        hr {
            border: none;
            height: 2px;
            /* Set the hr color */
            color: #333; /* old IE */
            background-color: #333; /* Modern Browsers */
        }
        .noresize {
            resize: none; 
        }

        .table th, .table td { 
            background-color: white;
            border-top: none !important; 
        }

        .error {
            color: #a99588;
            font-size: 1em;
        }
    </style>
    
</head>
<body>
    <form id="requestForm" runat="server" class="form-horizontal">
        <asp:ScriptManager runat="server">
            <Scripts>
                <%--To learn more about bundling scripts in ScriptManager see http://go.microsoft.com/fwlink/?LinkID=301884 --%>
                <%--Framework Scripts--%>
                <asp:ScriptReference Name="MsAjaxBundle" />                
                <%--Site Scripts--%>

            </Scripts>
        </asp:ScriptManager>

    <div class="container body-content">
    <%--<div class="jumbotron">--%>
        <%--<img src="../images/Banner_Print3.jpg" class="img-rounded" />--%>
        <img src="<%=Page.ResolveUrl("~/images/Banner_Print3.jpg")%>" class="img-rounded" />
        <br />
        <br />
        <div id="divRequest" visible="false">
        <div class="text-center">
            <h2><b>BQHS COLLABORATION & SUPPORT REQUEST FORM</b></h2>
        </div>
        <hr /> 
        <div class="text-center">
            <h4><b>Mahalo for contacting the Office of Biostatistics & Quantitative Health Sciences (BQHS)! To request our quantitative health sciences support, please complete and submit the form below.</b></h4>
        </div>     
        <br />
        <div class="panel panel-success">
            <div class="panel-heading">              
                <h4 class="text-center"><b>Investigator Information</b></h4>
            </div>
        </div>

        <%--<div class="form-group form-group-lg">
            <label class="col-sm-2 control-label" for="formGroupInputLarge">Large label</label>
            <div class="col-sm-10">
                <input class="form-control" type="text" id="formGroupInputLarge" placeholder="Large input" />
            </div>
        </div>--%>
        <div class="row form-group-md">
            <label class="col-sm-2 control-label" for="txtFirstName">First name</label>
            <div class="col-sm-2">
                <input class="form-control" type="text" name="txtFirstName" id="txtFirstName" placeholder="First name" />
            </div>
            <label class="col-sm-2 control-label" for="txtLastName">Last name</label>
            <div class="col-sm-2">
                <input class="form-control" type="text" name="txtLastName" placeholder="Last name" />
            </div>
            <label class="col-sm-2 control-label" for="txtDegree">Degree</label>
            <div class="col-sm-2">
                <input class="form-control" type="text" name="txtDegree" placeholder="Degree" />
            </div>
        </div>
        <br />
        <div class="row form-group-md">
            <label class="col-sm-2 control-label" for="txtEmail">Email</label>
            <div class="col-sm-2">
                <input class="form-control" type="text" name="txtEmail" placeholder="Email" />
            </div>
            <label class="col-sm-2 control-label" for="txtPhone">Phone number
                <p class="help-block">(only enter numbers)</p>
            </label>
            <div class="col-sm-2">
                <input class="form-control" type="text" name="txtPhone" placeholder="Phone" />
            </div>
        </div>
        <br />
        <div class="row form-group-md">
            <label class="col-sm-2 control-label" for="txtEmail">Organization/Department</label>
            <div class="col-sm-5">
                <input class="form-control" type="text" name="txtDept" placeholder="Organization/Department" />
            </div>
            <label class="col-sm-2 control-label" >Investigator status</label>
            <div class="col-sm-3">
                <select class="form-control" name="ddlPIStatus">
                    <option value="UH Faculty">UH Faculty</option>
                    <option value="UH Fellow/Resident">UH Fellow/Resident</option>
                    <option value="UH Student">UH Student</option>
                    <option value="Other Research Staff">Other Research Staff</option>
                    <option value="Community Researcher">Community Researcher</option>
                    <option value="Other">Other</option>
                    <option value="Unknown">Unknown</option>
                </select>
            </div>

        </div>       
        <br />
        <div class="panel panel-success">
            <div class="panel-heading">              
                <h4 class="text-center"><b>Project Information</b></h4>
            </div>
        </div>
        <div class="row form-group-md">
            <label class="col-sm-2 control-label" for="txtProjectTitle">Project title</label>
            <div class="col-sm-10">
                <input class="form-control" type="text" name="txtProjectTitle" placeholder="Project title" />
            </div>
        </div>
        <br />
        <div class="row">
            <label class="col-sm-2 control-label" for="txtProjectSummary">Summary of project and QHS need
                 </label>
            <div class="col-sm-10">
                <textarea class="form-control noresize" rows="5" name="txtProjectSummary"></textarea>
                <p class="help-block">Please provide a short summary of your project: description of data, outcomes, and a short statement of your service needs. It will be helpful for BQHS to evaluate your need ahead of our initial meeting. (250 word limitation)</p>
           
            </div>
        </div>
        <div class="row">
            <label class="col-sm-2 control-label">Study Area(s)<p class="help-block">(check all that apply)</p>
            </label>
            <div class="col-sm-10">
                <table class="table" id="tblStudyArea" runat="server">
                    <tbody>
                        <tr>
                            <td>
                                <label>
                                    <input type="checkbox" value="1" runat="server" />
                                    Aging & Chronic Disease Prevention/Management
                                </label>
                            </td>
                            <td>
                                <label>
                                    <input type="checkbox" value="4" runat="server" />
                                    Nutrition & Metabolic Health
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    <input type="checkbox" value="2" runat="server" />
                                    Cardiovascular Health
                                </label>
                            </td>
                            <td>
                                <label>
                                    <input type="checkbox" value="5" runat="server" />
                                    Growth, Development, & Reproductive Health
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    <input type="checkbox" value="3" runat="server" />
                                    Cancer Prevention & Health Recovery
                                </label>
                            </td>
                            <td>
                                <label>
                                    <input type="checkbox" value="6" runat="server" />
                                    Respiratory Health
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <label>
                                    <input type="checkbox" value="99" runat="server" />
                                    Other
                                </label>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>

        <div class="row">
            <div class="col-sm-2">
                <label class="control-label">Type(s) of support needed
                <p class="help-block">(check all that apply)</p></label>
            </div>
            <div class="col-sm-10">
                <table class="table" id="tblServiceType" runat="server">
                    <tbody>
                        <tr>
                            <td>
                                <label>
                                    <input type="checkbox" value="1" runat="server" />
                                    Study Design
                                </label>
                            </td>
                            <td>
                                <label>
                                    <input type="checkbox" value="4" runat="server" />
                                    Grant Proposal Development
                                </label>
                            </td>
                            <td>
                                <label>
                                    <input type="checkbox" value="7" runat="server" />
                                    Education
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    <input type="checkbox" value="2" runat="server" />
                                    Database Design & Management
                                </label>
                            </td>
                            <td>
                                <label>
                                    <input type="checkbox" value="5" runat="server" />
                                    Bioinformatics Analysis
                                </label>
                            </td>
                            <td>
                                <label>
                                    <input type="checkbox" value="9" runat="server" />
                                    Other
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    <input type="checkbox" value="3" runat="server" />
                                    Biostatistical Data Analysis
                                </label>
                            </td>
                            <td>
                                <label>
                                    <input type="checkbox" value="6" runat="server" />
                                    Dissemination
                                </label>
                            </td>
                            <td>
                            </td>
                        </tr>                        
                    </tbody>
                </table>
            </div>
        </div>

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
            <div class="col-sm-3">
                <label class="control-label">BQHS faculty/staff preference (if any)</label>
            </div>
            <div class="col-sm-8">
                <input class="form-control" type="text" name="txtPreferBiostat" />
                <%--<asp:TextBox ID="txtPreferBiostat" runat="server" class="form-control"></asp:TextBox>--%>
            </div>
        </div>
        <br />
        <div class="row">
            <div class="col-sm-2">
                <label class="control-label">Enter Code Below</label>
            </div>
            <div class="col-sm-2">
                <input class="form-control" type="text" name="txtCaptchaCode" id="txtCaptchaCode" />
                <%--<asp:TextBox ID="txtPreferBiostat" runat="server" class="form-control"></asp:TextBox>--%>
            </div>
        </div>
        <br />

            <asp:UpdatePanel ID="UP1" runat="server">
                <ContentTemplate>
                    <div class="row form-group-md"">
                        <div class="col-sm-2">
                            <label class="control-label">Captcha code</label>
                        </div>
                        <div class="col-sm-2">
                            <asp:Image ID="imgCaptcha" runat="server" class="img-responsive" />                            
                        </div>
                        <div class="col-sm-1">
                            <button id="btnRefresh" runat="server" onserverclick="btnRefresh_Click" class="btn btn-info">
                                <span class="glyphicon glyphicon-refresh"></span> Refresh</button>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

        <br />
        <div class="row">
            <div class="col-sm-12">
                <asp:Button ID="btnSubmit" name="btnSubmit" runat="server" Text="Submit" class="btn center-block" OnClick="btnSubmit_Click" />     
            </div>
        </div>
        <br />

        </div>
              
        
    </div>
    </form>

    <script type="text/javascript">
        $(document).ready(function (){
            $(function () {
                $('#dtpDueDate').datepicker({
                    todayHighlight: true,
                    format: "mm/dd/yyyy",
                    autoclose: true,
                    orientation: "top"
                });
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
       

        $('form').validate({
            //onkeyup: function (element) { //turn off onkeyup validation for ajax checking fields
            //    var element_id = jQuery(element).attr('id');
            //    if (this.settings.rules[element_id].onkeyup !== false) {
            //        jQuery.validator.defaults.onkeyup.apply(this, arguments);
            //    }
            //},
            rules: {
                txtFirstName: {
                    required: true
                },
                txtLastName: {
                    required: true
                },
                txtDegree: {
                    required: true
                },
                txtEmail: {
                    required: true,
                    email: true
                },
                txtPhone: {
                    required: true,
                    digits: true
                },
                txtDept: {
                    required: true
                },
                txtProjectTitle: {
                    required: true
                },
                txtCaptchaCode: {
                    required: true,
                    remote: function () {
                        return {
                            async: false,
                            url: "ClientRequestForm.aspx/IsCaptchaCodeValid",
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: JSON.stringify({ captchaCode: $('#txtCaptchaCode').val() }),
                            dataFilter: function (data) {
                                var msg = JSON.parse(data);
                                if (msg.hasOwnProperty('d'))
                                    return msg.d;
                                else
                                    return msg;
                            }
                        }
                    }
                }
            },
            messages: {
                txtCaptchaCode: {
                    remote: "Invalid Code"
                }
            },
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
            onblur: true,
            onfocusout: function (element) {
                if (element.id != "txtCaptchaCode")
                    $(element).valid();
            }
        });

        //function GetCheckResult() {
        //    var captchaCode = $("#txtCaptchaCode").val();

        //    $.ajax({
        //        type: "POST",
        //        url: "ClientRequestForm.aspx/CheckCaptchaCode",
        //        data: '{"captchaCode":"' + captchaCode + '"}',
        //        contentType: "application/json; charset=utf-8",
        //        dataType: "json",
        //        success: function (rtndata) {
        //            $("#txtCaptchaCode").addClass('has-error');
        //            if (JSON.stringify(rtndata.d) == 'Valid Captcha Code') {
        //                this.submit();
        //            }
        //            else {
        //                //$("#txtCaptchaCode").closest('.form-group').addClass('has-error');
        //                return captchaCode;
        //            }
        //        },
        //        error: function (xhr, status, err) {
        //            var err = eval("(" + xhr.responseText + ")");
        //            alert(err.Message);

        //        }
        //    });
        //}
        

    </script>
</body>
</html>
