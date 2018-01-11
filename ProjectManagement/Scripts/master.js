
 function getPageName(url) {
     var index = url.lastIndexOf("/") + 1;
     var filename = url.substr(index);
     return filename;
 }

$(document).ready(function () {
    // Set the 'active' class on current menu item's <li>
    $("a[href='" + getPageName(window.location.pathname) + "']").parent().addClass("active");
});

