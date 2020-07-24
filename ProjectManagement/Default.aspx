<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ProjectManagement._Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<script src="<%=Page.ResolveUrl("~/Scripts/highcharts/4.2.0/highcharts.js")%>"></script>--%>
    <script src="<%=Page.ResolveUrl("~/Scripts/jquery.dataTables.min.js")%>"></script>
    <script src="<%=Page.ResolveUrl("~/Scripts/dataTables.bootstrap.min.js")%>"></script>
    <link href="Content/dataTables.bootstrap.min.css" rel="stylesheet" />
    <style>
        img {
            width: 100%;
            /*height: auto;
            border: solid 1px black;*/
        }
    </style>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="loader"></div>

    <div class="" id="divDashboard" runat="server" visible="false">
        <%--<img src="images/Banner_Print3.jpg" class="img-rounded"/>--%>
        <div class="panel panel-bqhs">
            <div class="panel-heading">Active Projects</div>
            <div class="panel-body">
                <div class="row">
                    <div class="col-md-12">
                        <table class="table table-striped table-hover" id="project">
                            <thead>
                                <tr>
                                    <th style="width: 10%;">Id</th>
                                    <th style="width: 20%;">PI Name</th>
                                    <th style="width: 55%;">Project Title</th>
                                    <th style="width: 15%;">Last Entry</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rptProject" runat="server" OnItemCommand="rptProject_ItemCommand">
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <asp:LinkButton runat="server" ID="btnProject" CommandName="Project" CommandArgument='<%# Eval("projectid") %>' OnClientClick="javascript:return loading();">
                                                    <asp:Label ID="lblProjectId" runat="server" Text='<%# Eval("projectid") %>'></asp:Label>
                                                </asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton runat="server" ID="btnPI" CommandName="PI" CommandArgument='<%# Eval("piid") %>' OnClientClick="javascript:return loading();">
                                                    <asp:Label ID="lblPI" runat="server" Text='<%# Eval("piname") %>'></asp:Label>
                                                </asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton runat="server" ID="btnTitle" CommandName="Project" CommandArgument='<%# Eval("projectid") %>' OnClientClick="javascript:return loading();">
                                                    <asp:Label ID="lblTitle" runat="server" Text='<%# Eval("title") %>'></asp:Label>
                                                </asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton runat="server" ID="btnTimeEntry" CommandName="TimeEntry" CommandArgument='<%# Eval("projectid") %>' OnClientClick="javascript:return loading();">
                                                    <asp:Label ID="lblLastEntry" runat="server" Text='<%# Eval("lastentry") %>'></asp:Label>
                                                </asp:LinkButton>
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

        <div class="hidden">
            <div class="row">
                <div class="col-md-6">
                    <div class="panel panel-primary">
                        <div class="panel-heading">Active Projects</div>
                        <div class="panel-body">
                            You have <b>
                                <asp:Label ID="lblProjectCnt" runat="server"></asp:Label></b> active projects.
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="panel panel-primary">
                        <div class="panel-heading">Hours logged</div>
                        <div class="panel-body">
                            You have logged  
                          <asp:HyperLink NavigateUrl="~/TimeEntry" ID="hlTimeCnt" runat="server"></asp:HyperLink>
                            hours for this month.
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-6">
                    <div id="divPaperChart"></div>
                </div>
                <div class="col-md-6">
                    <div id="divHoursChart"></div>
                </div>
            </div>

        </div>

        <%--<div class="pdsa-submit-progress hidden">
            <i class="fa fa-2x fa-spinner fa-spin"></i>&nbsp;
            <label>Loading ...</label>
        </div>--%>
    </div>

    <div class="" id="divIdleProject" runat="server" visible="false">
        <%--<img src="images/Banner_Print3.jpg" class="img-rounded"/>--%>
        <div class="panel panel-bqhs">
            <div class="panel-heading">Idle Projects (no activity for over 3 months)</div>
            <div class="panel-body">
                <div class="row">
                    <div class="col-md-12">
                        <table class="table table-striped table-hover" id="idleProject">
                            <thead>
                                <tr>
                                    <th style="width: 10%;">Id</th>
                                    <th style="width: 20%;">PI Name</th>
                                    <th style="width: 55%;">Project Title</th>
                                    <th style="width: 15%;">Last Entry</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rptIdleProject" runat="server" OnItemCommand="rptProject_ItemCommand">
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <asp:LinkButton runat="server" ID="btnProject" CommandName="Project" CommandArgument='<%# Eval("projectid") %>' OnClientClick="javascript:return loading();">
                                                    <asp:Label ID="lblProjectId" runat="server" Text='<%# Eval("projectid") %>'></asp:Label>
                                                </asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton runat="server" ID="btnPI" CommandName="PI" CommandArgument='<%# Eval("piid") %>' OnClientClick="javascript:return loading();">
                                                    <asp:Label ID="lblPI" runat="server" Text='<%# Eval("piname") %>'></asp:Label>
                                                </asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton runat="server" ID="btnTitle" CommandName="Project" CommandArgument='<%# Eval("projectid") %>' OnClientClick="javascript:return loading();">
                                                    <asp:Label ID="lblTitle" runat="server" Text='<%# Eval("title") %>'></asp:Label>
                                                </asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton runat="server" ID="btnTimeEntry" CommandName="TimeEntry" CommandArgument='<%# Eval("projectid") %>' OnClientClick="javascript:return loading();">
                                                    <asp:Label ID="lblLastEntry" runat="server" Text='<%# Eval("lastentry") %>'></asp:Label>
                                                </asp:LinkButton>
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
    </div>

    <div id="warningModal" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <img style="height: 100px; width: 100px; display: block; margin-left: auto; margin-right: auto;" src="images/No_Admittance.png" />
                    <br />
                    <div id="doNotUseSiteMsg" class="modal-title">
                        <h1>Site is no longer in use</h1>
                        <h4 style="font-family: 'Times New Roman'"><em>A new site has been created to you to continue tracking your activities.</em></h4>
                    </div>
                </div>
                <div class="modal-body">
                    
                    <br />
                    <p class="text-warning" id="textWarning"></p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Continue onto Project Tracking System (admin only)</button>
                </div>
            </div>
        </div>
    </div>

    <div class="col-sm-2">
        <button type="button" style="display: none" id="btnShowWarningModal" class="btn btn-primary btn-lg"
            data-toggle="modal" data-target="#warningModal">
            Warning Modal</button>
    </div>


    <script type="text/javascript">       
        $(document).ready(function () {
            //getPaperCount(2015);
            //getProjectHours(2015);

            $('#project').DataTable({
                "stateSave": true,
                "order": [[3, "desc"]]
            });

            $(".loader").fadeOut("slow");
        });

        function ShowWarningModal() {
            $('#btnShowWarningModal').click();
        }

        function getBaseUrl() {
            var re = new RegExp(/^.*\//);
            return re.exec(window.location.href);
        }

        function getPaperCount(year) {
            var uri1 = '/api/Dashboard/GetProjectHours/?year=' + 2015;
            var uri2 = '/api/Dashboard/GetProjectHours/?year=' + 2016;

            var project1, project2;
            //$.getJSON(uri1).done(function (data1) {
            //    $.getJSON(uri2).done(function (data2) {
            //        // On success
            //        drawProjectChart(data1, data2);
            //    })
            //});


            $.getJSON(getBaseUrl() + uri2).done(function (data2) {
                // On success
                drawProjectHourChart(data2);
            })


            //var uri3 = '/api/Dashboard/GetPaperSubmitted/?year=' + 2015;
            //var uri4 = '/api/Dashboard/GetPaperSubmitted/?year=' + 2016;

            //$.getJSON(uri3).done(function (data3) {
            //    // On success
            //    $.getJSON(uri4).done(function (data4) {
            //        // On success
            //        drawPaperChart(data3, data4);
            //    })
            //});

            uri = './api/Dashboard/GetProjectCount/?year=' + 2016;
            $.getJSON(uri).done(function (data) {
                // On success
                drawProjectCountChart(data);

            });

            //drawPaperChart(project1, project2);
        }

        function drawProjectCountChart(series1) {
            $('#divPaperChart').highcharts({
                chart: {
                    type: 'area'
                },
                title: {
                    text: 'Active Projects'
                },
                xAxis: {
                    categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec']
                },
                yAxis: {
                    title: {
                        text: 'Total Projects'
                    }
                },
                series: [{
                    data: series1,
                    name: 2016
                }
                ]
            });
        }

        function drawPaperChart(series1, series2) {
            $('#divPaperChart').highcharts({
                chart: {
                    type: 'spline'
                },
                title: {
                    text: 'Paper Submitted'
                },
                xAxis: {
                    categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec']
                },
                plotOptions: {
                    series: {
                        allowPointSelect: true
                    }
                },
                series: [{
                    data: series1,
                    name: 2015
                }
                    , {
                    data: series2,
                    name: 2016
                }
                ]
            });
        }

        function drawProjectHourChart(series2) {
            $('#divHoursChart').highcharts({
                chart: {
                    type: 'column'
                },
                title: {
                    text: 'Logged Hours'
                },
                xAxis: {
                    categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec']
                },
                yAxis: {
                    title: {
                        text: 'Total Hours'
                    }
                },
                plotOptions: {
                    series: {
                        allowPointSelect: true
                    }
                },
                series: [{
                    data: series2,
                    name: 2016
                }
                ]
            });
        }

        function checkAuth() {
            var logged = (function () {
                var isLogged = null;
                $.ajax({
                    'async': false,
                    'global': false,
                    'url': '/user/isLogged/',
                    'success': function (resp) {
                        isLogged = (resp === "1");
                    }
                });
                return isLogged;
            })();
            return logged;
        }

    </script>
</asp:Content>
