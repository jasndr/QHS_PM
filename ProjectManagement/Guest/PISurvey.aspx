<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PISurvey.aspx.cs" Inherits="ProjectManagement.Guest.PISurvey" %>
<asp:Content ID="HeadContent" ContentPlaceHolderID="head" runat="server">
     <script type="text/javascript">        

         $(document).ready(function () {
             $('.navbar').hide();
         });

    </script>
    <style>
        .radioButtonList label{
            display:inline;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <img src="../images/Banner_Print3.jpg" class="img-rounded" width="100%"/>
    <div class="jumbotron">
        <div><h4>
            <span>The JABSOM Office of Biostatistics and Quantitative Health Sciences strives for excellence in research,
            education, and service through collaboration and innovation in quantitative health sciences. We
            have provided <asp:Label ID="lblPhdHours" runat="server" CssClass="label label-info"></asp:Label> 
            Ph.D. hours and <asp:Label ID="lblMsHours" runat="server" CssClass="label label-info"></asp:Label> M.S. hours of support
             (<asp:Label ID="lblBiostat" runat="server" CssClass="label label-info"></asp:Label>)            
            for your project <asp:Label ID="lblProjectTitle" runat="server" CssClass="label label-info"></asp:Label> 
                from <asp:Label ID="lblInitialDate" runat="server" CssClass="label label-info"></asp:Label>
            to <asp:Label ID="lblCompletionDate" runat="server" CssClass="label label-info"></asp:Label>. 
            We would like for you to complete a brief survey to give us feedback regarding our collaborations and services provided. 
            </span>
            <br />
            Thank you for working with us.
            </h4>
        </div>
    </div>
    <hr />
    <div class="form">        
        <div id="">
            <asp:DataList ID="dlQuestions" runat="server" RepeatDirection="Vertical" DataKeyField="ID" OnItemDataBound="dlQuestions_ItemDataBound" Width="630px">
                <ItemTemplate>
                    <div>
                        <h3>
                            <div class="row">
                        <asp:Label ID="Label2" Text='<%#(((DataListItem)Container).ItemIndex+1).ToString()%>' runat="server" />
                        <asp:Label ID="q" runat="server" Text='<%# Eval("Question") %>'  CssClass="label label-success"></asp:Label>
                                </div>
                        </h3>
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                            ErrorMessage="Required" ForeColor="Red" ControlToValidate="dtlAnswers" ValidationGroup="Bla"></asp:RequiredFieldValidator>--%>
                        <asp:HiddenField ID="hdnQuestionId" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Id")%>' />
                    </div>
                    <br />
                    <asp:RadioButtonList ID="dtlAnswers" runat="server" ValidationGroup="Bla" RepeatDirection="Horizontal" RepeatColumns="1" CssClass="list-group-item radioButtonList">
                    </asp:RadioButtonList>
                </ItemTemplate>                
            </asp:DataList>
            <br />
            <div class="row">
                <div class="col-xs-6 col-md-6">We encourage you to provide additional comments and suggestions:
                    <asp:TextBox ID="txtComments" runat="server" class="form-control" TextMode="MultiLine" Rows="5"></asp:TextBox>
                </div>
            </div>
        </div>
        <br />
        <div>
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" class="btn btn-primary" />
        </div>
        <asp:HiddenField ID="hdnSurveyFormId" runat="server" Value='' />
    </div>
</asp:Content>
