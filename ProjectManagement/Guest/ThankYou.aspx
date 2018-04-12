<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ThankYou.aspx.cs" Inherits="ProjectManagement.Guest.ThankYou" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Content/bootstrap.min.css" rel="stylesheet" />
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
        img
        {   
            width: 100%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
        <br />
        <div class="container body-content">
        <%--<img src="../images/Banner_Print3.jpg" class="img-rounded" />--%>
            <img src="<%=Page.ResolveUrl("~/images/Banner_Print3_CIM.png")%>" class="img-rounded" />
        <br />
        <br />
        <h3>QUANTITATIVE HEALTH SCIENCES (QHS) FOLLOW-UP SURVEY</h3>        
        <br />
        <div class="text-center">
            <h2>THANK YOU FOR COMPLETING THE SURVEY!</h2>
            <br />
            <h4>Additional comments and/or questions? Please contact us:</h4>
            <br />
            <p style="font-size:160%;">
                <%--<h4>Office of Biostatistics & Quantitative Health Sciences (BQHS)</h4>
            <h4>University of Hawaii John A. Burns School of Medicine</h4>
            <h4>651 Ilalo Street, Biosciences Building, Suite 211</h4>
            <h4>Honolulu, HI 96813</h4>
            <h4>Phone: (808) 692-1840</h4>
            <h4>Fax: (808) 692-1966</h4>
            <h4>E-mail: biostat@hawaii.edu</h4>--%>
                Quantitative Health Sciences (QHS)<br />
                Department of Complementary & Integrative Medicine<br />
                University of Hawaii John A. Burns School of Medicine<br />
                651 Ilalo Street, Biosciences Building, Suite 211<br />
                Honolulu, HI 96813<br />
                Phone: (808) 692-1840<br />
                Fax: (808) 692-1966<br />
                E-mail: qhs@hawaii.edu
            </p>
        </div>
        </div>
    </form>
</body>
</html>
