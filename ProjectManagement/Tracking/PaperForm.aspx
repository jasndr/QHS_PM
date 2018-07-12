<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PaperForm.aspx.cs" Inherits="ProjectManagement.Admin.PaperForm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">  
    <%--<script src="Scripts/bootstrap.min.js"></script>--%>    
    <script src="../Scripts/bootstrap-datepicker.min.js"></script>
    <script src="../Scripts/jquery.validate.min.js"></script>   
    <script src="../Scripts/typeahead.jquery.min.js"></script>    
    <%--<link href="../Content/Site.css" rel="stylesheet" />--%>  
    <style>
        .twitter-typeahead{
             width: 100%;
        }
        .tt-menu{
            width: 100%;
        }
    </style>
    <script type="text/javascript">
        function pageLoad(sender, args) {
            $('#li_admin').addClass('selected');
            $('#li_paperform').addClass('selected');

            $('#editModal').on('shown.bs.modal', function () {
                $('#editModal').scrollTop(0);
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

            //$("#btnAddNew").on("click", function () {                
            //    var templateRow = "<tr><td> <input name='Name' id='Name' type='text' class='form-control'/> </td>" +
            //                          "<td> <input name='Affil' id='Affil' type='text' class='form-control'/> </td>"
            //                       "</tr>";
            //    $('#controls').append(templateRow);
            //});

            //$("#btnRemove").on("click", function () {
            //    if ($("#controls tr").length == 1) { alert("Last row can't be deleted from table."); }
            //    else
            //    {
            //        //$(this).closest("tr").remove();
            //        $('#controls tr:last').remove();
            //    }
            //});

            $('#idTourDateDetails').datepicker({
                dateFormat: 'dd-mm-yy',
                minDate: '+5d',
                changeMonth: true,
                changeYear: true,
                altField: "#idTourDateDetailsHidden",
                altFormat: "yy-mm-dd"
            });

            $(function () {
                $('#datetimepicker1').datepicker({
                    todayHighlight: true,
                    format: "mm/dd/yyyy",
                    autoclose: true,
                    orientation: "top"
                });
                $('#datetimepicker2').datepicker({
                    todayHighlight: true,
                    format: "mm/dd/yyyy",
                    autoclose: true,
                    orientation: "top"
                });
                $('#datetimepicker3').datepicker({
                    todayHighlight: true,
                    format: "mm/dd/yyyy",
                    autoclose: true,
                    orientation: "top"
                });

            });

            $("#editModal").scroll(function () {
                $('#datetimepicker1').datepicker('place');
                $('#datetimepicker2').datepicker('place');
                $('#datetimepicker3').datepicker('place');
            });

            if ($('#MainContent_optionAbstract').is(':checked')) {
                $('#divAbstract').show();
            }
            else {
                $('#divAbstract').hide();
            }

            //rptAffiliation
                        
  //          var pinames = [];

  //          var map = {};

  //          //var data = JSON.parse($('#MainContent_textAreaPI').val());

  //          //$.each(data, function (i, piname) {
  //          //    map[piname.FullName] = piname;
  //          //    pinames.push(piname.FullName);
  //          //});

  //          $('.typeahead').typeahead({
  //              hint: true,
  //              highlight: true,
  //              minLength: 1
  //          },
  //          {
  //              name: 'piname',
  //              source: substringMatcher(pinames)
            //          });

            var pinames = [];
            var map = {};
            //var selectedAffil;

            var data = JSON.parse($('#MainContent_textAreaAffil').val());
            
            $.each(data, function (i, piname) {
                
                map[piname.Name] = piname;
                pinames.push(piname.Name);
            });

            $('#the-basics .typeahead').typeahead({
                hint: true,
                highlight: true,
                minLength: 1
            },
            {
                name: 'piname',
                limit: 10,
                source: substringMatcher(pinames)
                //,selected: function (item) {
                //    //selectedAffil = map[item.Id]; alert(selectedAffil);
                //    $('#txtSelectedAffil').val(map[item].Id);
                //    return item;
                //}
            }).on('typeahead:selected', function (event, selection) {
                $(this).closest('tr').find('input[name$="lblId"]').val(map[selection].Id);               
                $(this).closest('tr').find('input[name$="txtTitle"]').val(map[selection].Name);

                // clearing the selection requires a typeahead method
                //$(this).typeahead('setQuery', '');
            })
            ;

            //$('#txtSelectedAffil').val(selectedAffil);

            
        }

        //$('#txtSelectedAffil').bind('.typeahead:selected', function (obj, datum, name) {
        //    $('#txtSelectedAffil').val((JSON.stringify(obj)));

        //    alert(JSON.stringify(obj)); // object
        //    // outputs, e.g., {"type":"typeahead:selected","timeStamp":1371822938628,"jQuery19105037956037711017":true,"isTrigger":true,"namespace":"","namespace_re":null,"target":{"jQuery19105037956037711017":46},"delegateTarget":{"jQuery19105037956037711017":46},"currentTarget":
        //    alert(JSON.stringify(datum)); // contains datum value, tokens and custom fields
        //    // outputs, e.g., {"redirect_url":"http://localhost/test/topic/test_topic","image_url":"http://localhost/test/upload/images/t_FWnYhhqd.jpg","description":"A test description","value":"A test value","tokens":["A","test","value"]}
        //    // in this case I created custom fields called 'redirect_url', 'image_url', 'description'   

        //    alert(JSON.stringify(name)); // contains dataset name
        //    // outputs, e.g., "my_dataset"

        //});


        function divAbstractShow() {
            $('#divAbstract').show();
        }

        function divAbstractHide() {
            $('#divAbstract').hide();
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

        

        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="rootwizard">
        <div class="panel panel-default">
            <div class="panel-heading"><b>Papers</b></div>
            <div class="panel-body">
                <div class="row">
                    <div class="col-xs-6 col-md-3">
                        Paper Type
                        <asp:DropDownList ID="ddlPubType" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                    <div class="col-xs-6 col-md-3">
                        QHS Faculty and Staff
                        <asp:DropDownList ID="ddlBiostat" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                    <div class="col-xs-6 col-md-3">&nbsp;
                        <div><asp:Button ID="btnSumbit" runat="server" Text="Submit" CssClass="btn btn-info" OnClick="btnSumbit_Click" OnClientClick="ClientSideClick(this)" UseSubmitBehavior="False"/></div>
                    </div>
                    <div class="hidden">
                         <textarea id="textAreaAffil" rows="3" runat="server"></textarea>
                    </div>                   
                </div>               
                <hr /> 
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div>
                            <asp:GridView ID="GridViewPaper" runat="server"
                                OnRowCommand="GridViewPaper_RowCommand"
                                AutoGenerateColumns="false" AllowPaging="false"
                                DataKeyNames="Id"
                                class="table table-striped"
                                EmptyDataText="No record"
                                Gridline="none">
                                <Columns>                                    
                                    <asp:BoundField DataField="Id" HeaderText="Id" HeaderStyle-Width="5%"/>  
                                    <%--<asp:TemplateField HeaderText="Id">
                                        <ItemTemplate>
                                            <a href='PaperForm?id=<%# Eval("Id") %>'>
                                                <asp:Label ID="lblPaperId" runat="server" Text='<%# Bind("Id") %>'></asp:Label>
                                            </a>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="Type" HeaderStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblType" runat="server" Text='<%# Eval("PubType").ToString() == "1" ? "Abstract" : "Manuscript"  %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Title" HeaderText="Title" HeaderStyle-Width="40%"/>
                                    <asp:BoundField DataField="Author" HeaderText="Author" HeaderStyle-Width="35%"/>
                                    <asp:BoundField DataField="Status" HeaderText="Status" HeaderStyle-Width="5%"/>
                                    <%--<asp:TemplateField HeaderText="Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDate" runat="server" Text='<%# ((DateTime)Eval("PaperDate")).ToShortDateString()%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <asp:BoundField DataField="PaperDate" HeaderText="Date" HeaderStyle-Width="5%"/>
                                    <asp:ButtonField CommandName="editRecord"
                                        ControlStyle-CssClass="btn btn-info" ButtonType="Button"
                                        Text="Edit" HeaderText="" HeaderStyle-Width="5%"/>
                                </Columns>
                            </asp:GridView>
                            <div><asp:Button ID="btnAdd" runat="server" Text="Add a new paper" CssClass="btn btn-info" OnClick="btnAdd_Click" /></div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                    <ProgressTemplate>
                        <img src="" alt="Loading.. Please wait!" />
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <br />                
            </div>
        </div>

        <div id="editModal" class="modal fade" role="dialog" aria-labelledby="gridSystemModalLabel" aria-hidden="true"
            data-backdrop="static" data-keyboard="false">
            <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 id="editModalLabel">Add/Edit Paper</h4>
                </div>
                <asp:UpdatePanel ID="upEdit" runat="server">
                    <ContentTemplate>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-xs-6 col-md-12">Id:
                                    <b><asp:Label ID="lblPaperId" runat="server"></asp:Label></b>
                                </div>
                            </div>
                            <div class="radio">
                                <label>
                                    <input type="radio" name="pubRadios" id="optionAbstract" value="Abstract" runat="server" checked onclick="divAbstractShow()" />
                                    Abstract
                                </label>
                            </div>
                            <div class="radio">
                                <label>
                                    <input type="radio" name="pubRadios" id="optionManuscript" value="Manuscript" runat="server" onclick="divAbstractHide()" />
                                    Manuscript
                                </label>
                            </div>     
                            <div class="row">
                                <div class="col-xs-6 col-md-6">Project
                                    <asp:DropDownList ID="ddlProject" runat="server" CssClass="form-control">
                                    </asp:DropDownList>                                  
                                </div>
                            </div>                            
                            <br />
                            <div class="row">
                                <div class="col-xs-6 col-md-12">Title
                                    <asp:TextBox ID="TextBoxTitle" runat="server" placeholder="Title" class="form-control" ></asp:TextBox>                                    
                                </div>                                
                            </div>                            
                            <br />
                            <div class="row">
                                <div class="col-xs-6 col-md-12">Authors:
                                    <asp:TextBox ID="TextBoxAuthor" runat="server" placeholder="Authors" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <br />
                           
                            <div>Affiliations:
                             <asp:GridView ID="gvAffil" runat="server" AutoGenerateColumns="False"
                                ShowFooter="True"
                                OnRowDeleting="gvAffil_RowDeleting"                               
                                class="table">
                                <Columns>
                                    <asp:TemplateField HeaderText="Id" HeaderStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="lblId" runat="server" class="form-control" Text='<%# Eval("Id") %>' ReadOnly></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>                                    
                                    <asp:TemplateField HeaderText="Name" HeaderStyle-Width="80%">                                        
                                        <ItemTemplate>
                                            <div id="the-basics">
                                                <asp:TextBox ID="txtTitle" runat="server" class="form-control typeahead" Text='<%# Eval("Name") %>' ></asp:TextBox>
                                            </div>
                                        </ItemTemplate>                                        
                                         <FooterStyle HorizontalAlign="Left" />
                                        <FooterTemplate>
                                            <asp:Button ID="btnAddPhase" runat="server" CssClass="btn btn-info"
                                                Text="Add New" OnClick="btnAddAffil_Click" />
                                        </FooterTemplate>
                                    </asp:TemplateField>                                  

                                    <asp:TemplateField HeaderText="Del" HeaderStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDelete" runat="server" Text="Delete" CommandName="Delete"
                                                ToolTip="Delete" OnClientClick='return confirm("Are you sure you want to delete this entry?");'
                                                CommandArgument=''><img src="../images/icon-delete.png" /></asp:LinkButton>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            </div>

                            <div class="hidden">
                                <asp:Repeater ID="rptAffiliation" runat="server">
                                    <HeaderTemplate>
                                        <table class="table table-striped table-hover table-bordered" id="tblBiostat">
                                            <%--<tr>
                                        <th>Biostat</th>
                                        <th>Biostat</th>
                                    </tr>--%>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td style="width: 25%">
                                                <asp:CheckBox ID="FirstchkId" runat="server"></asp:CheckBox>
                                                <asp:HiddenField ID="FirstId" Value='<%#Eval("Id1")%>' runat="server" />
                                                <%# Eval("Name1") %>
                                            </td>
                                            <td style="width: 25%">
                                                <asp:CheckBox ID="SecondchkId" runat="server" Visible='<%# (int)Eval("Id2") > 0 %>' ></asp:CheckBox>
                                                <asp:HiddenField ID="SecondId" Value='<%#Eval("Id2")%>' runat="server" />
                                                <%# Eval("Name2") %>
                                            </td>
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <td style="width: 25%">
                                            <asp:CheckBox ID="FirstchkId" runat="server" ></asp:CheckBox>
                                            <asp:HiddenField ID="FirstId" Value='<%#Eval("Id1")%>' runat="server" />
                                            <%# Eval("Name1") %>
                                        </td>
                                        <td style="width: 25%">
                                            <asp:CheckBox ID="SecondchkId" runat="server" Visible='<%# (int)Eval("Id2") > 0 %>' ></asp:CheckBox>
                                            <asp:HiddenField ID="SecondId" Value='<%#Eval("Id2")%>' runat="server" />
                                            <%# Eval("Name2") %>
                                        </td>
                                        </tr>
                                    </AlternatingItemTemplate>
                                    <FooterTemplate>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>

                            <div class="row">
                                <div class="col-sm-12">Other Affiliations, separate with vertical bar
                                    <input class="form-control" type="text" name="txtAffiliationOther" id="txtAffiliationOther" runat="Server" />
                                </div>
                                <div class="hidden">
                                    <input class="form-control" type="text" name="txtAffiliationId" id="txtAffiliationId" runat="Server" />
                                </div>
                            </div>
                            <br />

                            <div class="hidden">
                                <asp:Repeater ID="rptAffil" runat="server">
                                    <HeaderTemplate>
                                        <table class="table table-striped table-hover table-bordered" id="tblAffil">
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td style="width: 50%">
                                                <asp:HiddenField ID="Id" Value='<%#Eval("Id")%>' runat="server" />
                                                <%# Eval("Name") %>
                                            </td>
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <td style="width: 50%">
                                            <asp:HiddenField ID="Id" Value='<%#Eval("Id")%>' runat="server" />
                                            <%# Eval("Name") %>
                                        </td>
                                        </tr>
                                    </AlternatingItemTemplate>
                                    <FooterTemplate>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>

                            </div>

                            <%--<div class="row hidden">
                                <div class="col-xs-6 col-md-12">
                                    <div class="panel panel-default">
                                        <div class="panel-heading">Authors & Affiliations</div>
                                        <div class="panel-body">                                         
                                            <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333"
                                                GridLines="None" AutoGenerateColumns="False"
                                                OnRowCommand="GridView1_RowCommand"
                                                OnRowEditing="GridView1_RowEditing"
                                                OnRowDataBound="GridView1_RowDataBound"
                                                OnRowUpdating="GridView1_RowUpdating" 
                                                OnRowDeleting="GridView1_RowDeleting"
                                                ShowFooter="True"
                                                class="table table-bordered table-hover">                                                
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Id"  HeaderStyle-Width="5%">
                                                        <ItemTemplate>
                                                            
                                                            <asp:Label ID="lblId" runat="server" Text='<%# Bind("Id") %>'></asp:Label>
                                                            
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Author Name"  HeaderStyle-Width="20%">
                                                        <EditItemTemplate>
                                                            <div id="the-basics2">
                                                            <asp:TextBox ID="txtName" runat="server" Text='<%# Bind("Name") %>' CssClass="form-control typeahead" MaxLength="30"></asp:TextBox>
                                                            </div>
                                                            <asp:RequiredFieldValidator ID="valName" runat="server" ControlToValidate="txtName"
                                                                Display="Dynamic" ErrorMessage="Name is required." ForeColor="Red" SetFocusOnError="True"
                                                                ValidationGroup="editGrp">*</asp:RequiredFieldValidator>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblName" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtNameNew" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="valNameNew" runat="server" ControlToValidate="txtNameNew"
                                                                Display="Dynamic" ErrorMessage="Name is required." ForeColor="Red" SetFocusOnError="True"
                                                                ValidationGroup="newGrp">*</asp:RequiredFieldValidator>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Affiliation"  HeaderStyle-Width="50%">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtType" runat="server" Text='<%# Bind("Type") %>' CssClass="form-control" MaxLength="500"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="valType" runat="server" ControlToValidate="txtType"
                                                                Display="Dynamic" ErrorMessage="Type is required." ForeColor="Red" SetFocusOnError="True"
                                                                ValidationGroup="editGrp">*</asp:RequiredFieldValidator>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblType" runat="server" Text='<%# Bind("Type") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtTypeNew" runat="server" CssClass="form-control" MaxLength="500"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="valTypeNew" runat="server" ControlToValidate="txtTypeNew"
                                                                Display="Dynamic" ErrorMessage="Type is required." ForeColor="Red" SetFocusOnError="True"
                                                                ValidationGroup="newGrp">*</asp:RequiredFieldValidator>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>                                                  
                                                                                                      
                                                    <asp:TemplateField HeaderText="Edit"  HeaderStyle-Width="10%">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkEdit" runat="server" Text="" CommandName="Edit" ToolTip="Edit" 
                                                                CommandArgument=''><img src="../images/icon-edit.png" /> </asp:LinkButton>
                                                            <asp:LinkButton ID="lnkDelete" runat="server" Text="Delete" CommandName="Delete" 
                                                                ToolTip="Delete" OnClientClick='return confirm("Are you sure you want to delete this entry?");'
                                                                CommandArgument=''><img src="../images/icon-delete.png" /></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:LinkButton ID="lnkInsert" runat="server" Text="" ValidationGroup="editGrp" CommandName="Update" ToolTip="Save"
                                                                CommandArgument=''><img src="../images/icon-save.gif" /></asp:LinkButton>
                                                            <asp:LinkButton ID="lnkCancel" runat="server" Text="" CommandName="Cancel" ToolTip="Cancel"
                                                                CommandArgument=''><img src="../images/icon-cancel.gif" /></asp:LinkButton>
                                                        </EditItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:LinkButton ID="lnkInsert" runat="server" Text="" ValidationGroup="newGrp" CommandName="InsertNew" ToolTip="Add New Entry"
                                                                CommandArgument=''><img src="../images/icon-save.gif" /></asp:LinkButton>
                                                            <asp:LinkButton ID="lnkCancel" runat="server" Text="" CommandName="CancelNew" ToolTip="Cancel"
                                                                CommandArgument=''><img src="../images/icon-cancel.gif" /></asp:LinkButton>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>                                            
                                        </div>
                                    </div>
                                </div>
                            </div>--%>

                            <div class="row">
                                <div class="col-xs-6 col-md-12">Comments:
                                    <asp:TextBox ID="TextBoxComment" runat="server" placeholder="Comments" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <br />                             
                            <div class="row">
                                <div class="col-xs-6 col-md-6">
                                    <div class="panel panel-default">
                                        <div class="panel-heading">QHS Faculty and Staff</div>
                                        <div class="panel-body">
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
                                <div class="col-xs-6 col-md-6">
                                    <div class="panel panel-default">
                                        <div class="panel-heading">Status</div>
                                        <div class="panel-body">
                                            <div class="form-group">
                                                <%--<label class="col-xs-3 control-label">Date Range</label>--%>
                                                <div class="col-xs-9">
                                                    <div class="radio">
                                                        <label>
                                                            <input id="optionSubmitted" runat="server" name="statusRadios" value="Submitted" type="radio" checked>
                                                            Submitted
                                                        </label>
                                                    </div>
                                                    <div class="radio">
                                                        <label>
                                                            <input id="optionAccepted" runat="server" name="statusRadios" value="Accepted" type="radio" >
                                                            Accepted</label>
                                                    </div>
                                                    <div class="radio">
                                                        <label>
                                                            <input type="radio" runat="server" name="statusRadios" id="optionPublished" value="Published" >
                                                            Published
                                                        </label>
                                                    </div>
                                                    <div class="radio">
                                                        <label>
                                                            <input type="radio" runat="server" name="statusRadios" id="optionNotAccepted" value="NotAccepted" >
                                                            Not Accepted
                                                        </label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div>
                                        Current Status Date
                                        <div class='input-group date' id='datetimepicker1'>
                                            <span class="input-group-addon">
                                                <span class="glyphicon glyphicon-calendar"></span>
                                            </span>
                                            <asp:TextBox ID="TextBoxSubmitDate" runat="server" class="form-control" Width="50%"></asp:TextBox>
                                        </div>
                                        <%--<div class="input-group">
                                        <input type="text" name="idTourDateDetails" id="idTourDateDetails" readonly="readonly" class="form-control clsDatePicker">
                                        <span class="input-group-addon"><i id="calIconTourDateDetails" class="glyphicon glyphicon-th"></i></span>

                                        </div>--%>
                                    </div>
                                    <br />

                                    <div class="panel panel-default">
                                        <div class="panel-heading">Grants</div>
                                        <div class="panel-body">
                                            <asp:GridView ID="GridViewGrant" runat="server" AutoGenerateColumns="False" class="table table-bordered table-hover">
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
                                    <%--&nbsp;--%>                              
                                    
                                </div>
                            </div>
                            
                            <div class="row">
                                <div id="divManuscript">
                                    <div class="col-xs-6 col-md-6">
                                        <div class="panel panel-default">
                                            <div class="panel-heading">Journal</div>
                                            <div class="panel-body">
                                                <div class="row">
                                                    <div class="col-xs-6 col-md-12">
                                                        Name of Journal:
                                                            <asp:TextBox ID="TextBoxJournal" runat="server" class="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <br />
                                                <div class="row">
                                                    <div class="col-xs-6 col-md-6">
                                                        Volume #:
                                                            <asp:TextBox ID="TextBoxVolumeNo" runat="server" class="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <br />
                                                <div class="row">
                                                    <div class="col-xs-6 col-md-6">
                                                        Issue #:
                                                            <asp:TextBox ID="TextBoxIssueNo" runat="server" class="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <br />
                                                <div class="row">
                                                    <div class="col-xs-6 col-md-6">
                                                        Pages:
                                                            <asp:TextBox ID="TextBoxPages" runat="server" class="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <br />
                                                <div class="row">
                                                    <div class="col-xs-6 col-md-6">
                                                        DOI: (Manuscript only)
                                                            <asp:TextBox ID="TextBoxDOI" runat="server" class="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <br />
                                                <div class="row">
                                                    <div class="col-xs-6 col-md-6">
                                                        Pmid: (Manuscript only)
                                                            <asp:TextBox ID="TextBoxPmid" runat="server" class="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <br />
                                                <div class="row">
                                                    <div class="col-xs-6 col-md-6">
                                                        Pmcid: (Manuscript only)
                                                            <asp:TextBox ID="TextBoxPmcid" runat="server" class="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                &nbsp;
                                                <div class="row">
                                                    <div class="col-xs-6 col-md-12">
                                                        In Press Information:
                                                        <asp:TextBox ID="TextBoxSampleAccept" runat="server" class="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div id="divAbstract">
                                    <div class="col-xs-6 col-md-6">
                                        <div class="panel panel-default">
                                            <div class="panel-heading">Conference</div>
                                            <div class="panel-body">
                                                <div class="row">
                                                    <div class="col-xs-6 col-md-12">
                                                        Name of Conference/Meeting:
                                                        <asp:TextBox ID="TextBoxName" runat="server" class="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <br />
                                                <div class="row">
                                                    <div class="col-xs-6 col-md-12">
                                                        Location:
                                                        <asp:TextBox ID="TextBoxLocation" runat="server" class="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <br />
                                                <div class="row">
                                                 <div class="col-xs-6 col-md-12">Start Date:
                                                     <div class='input-group date' id='datetimepicker2'>
                                                         <span class="input-group-addon">
                                                             <span class="glyphicon glyphicon-calendar"></span>
                                                         </span>
                                                         <asp:TextBox ID="TextBoxStartDate" runat="server" class="form-control" Width="50%"></asp:TextBox>
                                                     </div>
                                                 </div>
                                                </div>
                                                <br />
                                                <div class="row">                                                                            
                                                 <div class="col-xs-6 col-md-12">End Date:
                                                     <div class='input-group date' id='datetimepicker3'>
                                                         <span class="input-group-addon">
                                                             <span class="glyphicon glyphicon-calendar"></span>
                                                         </span>
                                                         <asp:TextBox ID="TextBoxEndDate" runat="server" class="form-control" Width="50%"></asp:TextBox>
                                                     </div>
                                                 </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-xs-6 col-md-6">
                                                        <div class="radio">
                                                            <label>
                                                                <input type="radio" name="optionsRadios" id="optionsPoster" value="option1" runat="server">
                                                                Poster
                                                            </label>
                                                            <label>
                                                                <input type="radio" name="optionsRadios" id="optionsPresentation" value="option2" runat="server">
                                                                Presentation
                                                            </label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="panel panel-default">
                                <div class="panel-heading">History (capture changes for paper status, journal or Conference info.)</div>
                                <div class="panel-body">

                                    <asp:GridView ID="GridViewHistory" runat="server" AutoGenerateColumns="False" class="table table-bordered table-hover"
                                        OnRowDeleting="GridViewHistory_RowDeleting"
                                        onrowdatabound="GridViewHistory_RowDataBound"
                                        OnRowCommand="GridViewHistory_RowCommand"
                                    >
                                        <Columns>
                                            <asp:TemplateField HeaderText="Id" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblId" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Type" ItemStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPubType" runat="server" Text='<%# (int)Eval("PubType") == 1 ? "Abstract" : "Manuscript" %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Status" ItemStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Name") != null ? Eval("Name") : "" %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Journal" ItemStyle-Width="40%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblJournal" runat="server" Text='<%# Eval("JournalName") %>'></asp:Label>
                                                </ItemTemplate>                                                
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Conference" ItemStyle-Width="40%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblConf" runat="server" Text='<%# Eval("ConfName") %>'></asp:Label>
                                                </ItemTemplate>                                                
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Date" ItemStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStartDate" runat="server" Text='<%# Eval("PaperDate") != "" ? Convert.ToDateTime(Eval("PaperDate")).ToShortDateString() : "" %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>                                            
                                           <%-- <asp:TemplateField HeaderText="Creator" ItemStyle-Width="20%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCreator" runat="server" Text='<%# Eval("Creator") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                            <asp:TemplateField HeaderText="" ItemStyle-Width="10%">
                                                <ItemTemplate>
                                                    <%--<asp:LinkButton ID="lnkEdit" runat="server" Text="" CommandName="Edit" ToolTip="Edit"
                                                        CommandArgument=''><img src="images/icon-edit.gif" /></asp:LinkButton>--%>
                                                    <asp:LinkButton ID="lnkDelete" runat="server" Text="Delete" CommandName="Delete" Visible='<%# Eval("Id") != "" ? true : false %>'
                                                        ToolTip="Delete" OnClientClick='return confirm("Are you sure you want to delete this record?");'
                                                        CommandArgument=''><img src="../images/icon-delete.png" /></asp:LinkButton>
                                                </ItemTemplate>
                                                <%--<EditItemTemplate>
                                                    <asp:LinkButton ID="lnkInsert" runat="server" Text="" ValidationGroup="editGrp" CommandName="Update" ToolTip="Save"
                                                        CommandArgument=''><img src="images/icon-save.gif" /></asp:LinkButton>
                                                    <asp:LinkButton ID="lnkCancel" runat="server" Text="" CommandName="Cancel" ToolTip="Cancel"
                                                        CommandArgument=''><img src="images/icon-cancel.gif" /></asp:LinkButton>
                                                </EditItemTemplate>--%>                                               
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>

                        </div>
                        <div class="modal-footer">
                            <asp:Label ID="lblResult" Visible="false" runat="server"></asp:Label>
                            <asp:Button ID="btnSave" runat="server" Text="Update" CssClass="btn btn-info" OnClick="btnSave_Click" OnClientClick="ClientSideClick(this)" UseSubmitBehavior="False"/>
                            <button class="btn btn-info" data-dismiss="modal" aria-hidden="true">Close</button>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="GridViewPaper" EventName="RowCommand" />
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
      </div>

        <div class="clearfix"></div>
    </div>
     <asp:HiddenField ID="HiddenFieldCurrentDate" Value="" runat="server" />
    <input type="hidden" id="hdnPaperType" value="0" runat="server" />
    <input type="hidden" id="hdnBiostatId" value="0" runat="server" />
</asp:Content>
