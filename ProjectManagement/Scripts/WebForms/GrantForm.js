function pageLoad(sender, args) {
    var startDate = new DatePicker('dtpStartDate'),
        endDate = new DatePicker('dtpEndDate'),
        subDate = new DatePicker('dtpSubDate'),
        subDeadline = new DatePicker('dtpSubDeadline'),
        initDate = new DatePicker('dtpInitDate'),
        fundDate = new DatePicker('dtpFundDate'),
        endDate = new DatePicker('dtpEndDate');

    startDate.init();
    endDate.init();
    subDate.init();
    subDeadline.init();
    initDate.init();
    initDate.onchange();
    fundDate.init();
    endDate.init();

    //validate fields
    $("#MainContent_ddlProject").addClass("required");
    $("#MainContent_TextBoxGrantTitle").addClass("required");
    $("#MainContent_TextBoxInitDate").addClass("required");
    $("#MainContent_ddlGrantStatus").addClass("required");
    $("#MainContent_TextBoxFirstYrAmt").addClass("number");
    $("#MainContent_TextBoxTotalAmt").addClass("number");
    $("#MainContent_TextBoxTotalCost").addClass("number");

    $("#editModal").scroll(function () {
        $('#dtpStartDate').datepicker('place');
        $('#dtpEndDate').datepicker('place');
        $('#dtpSubDate').datepicker('place');
        $('#dtpSubDeadline').datepicker('place');
        $('#dtpInitDate').datepicker('place');
        $('#dtpFundDate').datepicker('place');
    });

    $("#MainContent_ddlProject").change(function () {
        var projectId = $("#MainContent_ddlProject").val();
        bindPI(projectId);
    });
}

$(document).ready(function () {
    //alert(window.location.href);
    $('#li_admin').addClass('selected');
    $('#li_grantform').addClass('selected');

    $(".loader").fadeOut("slow");

    $('#editModal').on('shown.bs.modal', function () {
        $('#editModal').scrollTop(0);
    });

    $(function () {
        $("#MainContent_ddlInvestor").chosen();
        $("#MainContent_ddlInvestor_chosen").width("100%");
    });

    var projectId = GetURLParameter('ProjectId');

    if (projectId > 0) {
        $("#editModal").modal('show');
        $("#MainContent_ddlProject").val(projectId);
        bindPI(projectId);
        //document.location = window.location.href.split('?')[0];

        //clearQueryString();
    }

});

var DatePicker = function (ctrlId) {
    var ctl = ctrlId;

    return {
        init: function (e) {
            $('#' + ctl).datepicker({
                todayHighlight: true,
                format: "mm/dd/yyyy",
                autoclose: true,
                orientation: "top"
            })
        }
        , onchange: function (e) {
            $('#' + ctl).on('changeDate', function (e) {
                //revalidate
                if ($('#' + ctl).validate()) {
                    $('#' + ctl).closest('.row').removeClass('has-error');
                }
            });
        }
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

function showhideProces() {
    $(".pdsa-submit-progress").removeClass("hidden");
}

function loading() {
    //$(".pdsa-submit-progress").removeClass("hidden");
    $(".loader").fadeOut("slow");
}

function bindPI(projectId) {
    if (projectId > 0) {
        var uri = getBaseUrl() + '../api/Project/GetProjectPIName/?projectId=' + projectId;

        $.getJSON(uri).done(function (data) {
            // On success
            $("#MainContent_TextBoxPI").val(data);
            $("#MainContent_TextBoxPI").prop('readonly', true);
        });
    }
    else {
        $("#MainContent_TextBoxPI").prop('readonly', false);
    }
}

function validateControl() {
    var validator = $("#commentForm").validate({
        //only works with js in page
        //rules: {
        //    <%=TextBoxInitDate.UniqueID %>: {
        //        required: true,
        //        date: true
        //    },
        //    MainContent_ddlProject: {
        //        required: true
        //    }
        //},
        //messages: {
        //    <%=TextBoxInitDate.UniqueID %>: {
        //        required: "Initial data is required."
        //    }
        //},
        highlight: function (element) {
            $(element).closest('.row').addClass('has-error');
        },
        unhighlight: function (element) {
            $(element).closest('.row').removeClass('has-error');
        }

    });

    var isValid = $("#commentForm").valid();

    if (validator.errorList.length > 0) {
        var firstElement = validator.errorList[0].element;
        firstElement.focus();
    }

    return isValid;
}

function GetURLParameter(sParam) {
    var sPageURL = window.location.search.substring(1);
    var sURLVariables = sPageURL.split('&');
    for (var i = 0; i < sURLVariables.length; i++) {
        var sParameterName = sURLVariables[i].split('=');
        if (sParameterName[0] == sParam) {
            return sParameterName[1];
        }
    }
}

function getBaseUrl() {
    var re = new RegExp(/^.*\//);
    return re.exec(window.location.href);
}

function clearQueryString() {
    /*
 * queryParameters -> handles the query string parameters
 * queryString -> the query string without the fist '?' character
 * re -> the regular expression
 * m -> holds the string matching the regular expression
 */
    var queryParameters = {}, queryString = location.search.substring(1),
        re = /([^&=]+)=([^&]*)/g, m;

    // Creates a map with the query string parameters
    while (m = re.exec(queryString)) {
        queryParameters[decodeURIComponent(m[1])] = decodeURIComponent(m[2]);
    }

    // Add new parameters or update existing ones
    //queryParameters['newParameter'] = 'new parameter';
    queryParameters['ProjectId'] = '';

    /*
     * Replace the query portion of the URL.
     * jQuery.param() -> create a serialized representation of an array or
     *     object, suitable for use in a URL query string or Ajax request.
     */
    location.search = $.param(queryParameters); // Causes page to reload

}

// override jquery validate plugin defaults
//$.validator.setDefaults({
//    highlight: function (element) {
//        $(element).closest('.form-group').addClass('has-error');
//    },
//    unhighlight: function (element) {
//        $(element).closest('.form-group').removeClass('has-error');
//    },
//    errorElement: 'span',
//    errorClass: 'help-block',
//    errorPlacement: function (error, element) {
//        if (element.parent('.input-group').length) {
//            error.insertAfter(element.parent());
//        } else {
//            error.insertAfter(element);
//        }
//    }
//});

//another version for jquery validate plugin
$.validator.setDefaults({
    errorElement: "span",
    errorClass: "help-block",
    highlight: function (element, errorClass, validClass) {
        $(element).closest('.form-group').addClass('has-error');
    },
    unhighlight: function (element, errorClass, validClass) {
        $(element).closest('.form-group').removeClass('has-error');
    },
    errorPlacement: function (error, element) {
        if (element.parent('.input-group').length || element.prop('type') === 'checkbox' || element.prop('type') === 'radio') {
            error.insertAfter(element.parent());
        } else {
            error.insertAfter(element);
        }
    }
});