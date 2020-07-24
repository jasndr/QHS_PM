<%@ Page Title="Log in" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ProjectManagement.Account.Login" Async="true" %>

<%@ Register Src="~/Account/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h3><%: Title %></h3>


    <div class="jumbotron" style="background-color: #DCDCDC !important; color: black;">
        <h3 style="color: red; font-weight: bold;">Notice: Site no longer in use</h3>
        <h5>Logging into this site is now only permitted for the QHS Tracking Team.  Please visit the 
            new Project Tracking websites for the corresponding Core Facility below. Note that external links
            allows you access to the websites outside of JABSOM, the internal links can only be accessed
            through JABSOM-connected computers, which is fundamentally more secure.
        </h5>
        <br />
        <div>
            <p style="color: maroon; font-size: 16pt;">
                [Biostatistics Core Facility]
            </p>
            <ul style="font-size: 12pt;">
                <li>External Link - <a href="https://bqhsportal.jabsom.hawaii.edu/PM-Biostat">bqhsportal.jabsom.hawaii.edu/PM-Biostat</a></li>
                <li>Internal Link - <a href="http://qhsdb.jabsomit.hawaii.edu/PM-Biostat">qhsdb.jabsomit.hawaii.edu/PM-Biostat</a></li>
            </ul>
            <p style="color: green; font-size: 16pt;">[Bioinformatics Core Facility]</p>
            <ul style="font-size: 12pt;">
                <li>External Link - <a href="https://bqhsportal.jabsom.hawaii.edu/PM-Bioinfo">bqhsportal.jabsom.hawaii.edu/PM-Bioinfo</a></li>
                <li>Internal Link - <a href="http://qhsdb.jabsomit.hawaii.edu/PM-Bioinfo">qhsdb.jabsomit.hawaii.edu/PM-Bioinfo</a></li>
            </ul>
        </div>
    </div>



    <div class="row">
        <div class="col-md-8">
            <section id="loginForm">
                <div class="form-horizontal">
                    <hr />
                    <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                        <p class="text-danger">
                            <asp:Literal runat="server" ID="FailureText" />
                        </p>
                    </asp:PlaceHolder>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="UserName" CssClass="col-md-2 control-label">User name</asp:Label>
                        <div class="col-md-6">
                            <asp:TextBox runat="server" ID="UserName" CssClass="form-control" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="UserName"
                                CssClass="text-danger" ErrorMessage="The user name field is required." />
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="Password" CssClass="col-md-2 control-label">Password</asp:Label>
                        <div class="col-md-6">
                            <asp:TextBox runat="server" ID="Password" TextMode="Password" CssClass="form-control" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="Password" CssClass="text-danger" ErrorMessage="The password field is required." />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-offset-1 col-md-6">
                            <div class="checkbox">
                                <asp:CheckBox runat="server" ID="RememberMe" />
                                <asp:Label runat="server" AssociatedControlID="RememberMe">Remember me?</asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-offset-6 col-md-6">
                            <asp:Button runat="server" OnClick="LogIn" Text="Log in" CssClass="btn btn-default" />
                        </div>
                    </div>
                </div>
                <br />
                <p>
                    <asp:HyperLink runat="server" ID="RegisterHyperLink" ViewStateMode="Disabled">Register</asp:HyperLink>
                    if you don't have a local account.
                </p>
                <p>
                    <asp:HyperLink runat="server" ID="ForgotPasswordHyperLink" ViewStateMode="Disabled">Forgot your password?</asp:HyperLink>
                </p>
            </section>
        </div>

        <%-- <div class="col-md-4">
            <section id="socialLoginForm">
                <uc:OpenAuthProviders runat="server" ID="OpenAuthLogin" />
            </section>
        </div>--%>
    </div>

</asp:Content>
