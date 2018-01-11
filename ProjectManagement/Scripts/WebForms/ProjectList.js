function GoToProjectForm(id) {
    location.href = "ProjectForm?Id=" + id;
}

function pageLoad(sender, args) {
    $('#li_project').addClass('selected');
    $('#li_projectlist').addClass('selected');

    $("#btnSearch").click(function () {
        // init bootpag
        var piId = $("#MainContent_ddlInvestor").val();
        if (!piId) piId = -1;
        var count = GetTotalPageCount(piId);

        $('#page-selection').bootpag(
            {
                total: count,
                page: 1,
                maxVisible: 10,
                leaps: true,
                firstLastUse: true,
                first: '←',
                last: '→',
                wrapClass: 'pagination',
                activeClass: 'active',
                disabledClass: 'disabled',
                nextClass: 'next',
                prevClass: 'prev',
                lastClass: 'last',
                firstClass: 'first'
            }).on("page", function (event, num) {
                //alert("piId: " + piId + " | " + num);
                GetGridData(num);
            });

        GetGridData(1);
    })
                       

    //$(document).ready(function () { })         

                 
    function GetGridData(num) {
        var piId = $("#MainContent_ddlInvestor").val();                
        if (!piId) piId = -1;
               
        var count = GetTotalPageCount(piId);

        $.ajax({
            type: "POST",
            url: "ProjectList.aspx/GetProjectList",
            //data: "{ \"pagenumber\":" + num + "}",
            data: '{"piId":"' + piId + '","pagenumber":"' + num + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (rtndata) {
                //alert(JSON.stringify(rtndata.d));
                bindGrid(rtndata.d);
            },
            error: function (xhr, status, err) {
                var err = eval("(" + xhr.responseText + ")");
                alert(err.Message);

            }
        });
    }            
            
    function bindGrid(data) {
        //alert(data);
        $("[id*=MainContent_GridViewProject] tr").not(":first").not(":last").remove();
        var table1 = $('[id*=MainContent_GridViewProject]');
        var firstRow = "$('[id*=MainContent_GridViewProject] tr:first-child')";
        for (var i = 0; i < data.length; i++) {
            var projectId = data[i].Id;
            var funcName = 'GoToProjectForm(' + projectId + ')';
            var newRowStr = "<tr><td></td><td></td><td></td><td></td><td></td><td><input type='Button' class='btn btn-info' Value='Edit' onclick=" + funcName + "></td></tr>"
            var rowNew = $(newRowStr);
            rowNew.children().eq(0).text(data[i].Id);
            rowNew.children().eq(1).text(data[i].Title);
            rowNew.children().eq(2).text(data[i].PIFirstName);
            rowNew.children().eq(3).text(data[i].PILastName);
            rowNew.children().eq(4).text(data[i].InitialDate);
            rowNew.insertBefore($("[id*=MainContent_GridViewProject] tr:last-child"));
        }
    }            

    function GetTotalPageCount(piId) {
        var mytempvar = 0; 
        $.ajax({
            url: "ProjectList.aspx/GetTotalPageCount",
            type: "POST",                    
            data: '{"piId":"' + piId + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (rtndata) {
                //alert(JSON.stringify(rtndata));
                mytempvar = rtndata.d;

            },
            error: function (xhr, status, err) {
                var err = eval("(" + xhr.responseText + ")");
                alert(err.Message);

            }
        });

        return mytempvar;
    }
}
