
var isMobile = /iPhone|iPad|iPod|Android/i.test(navigator.userAgent);
var user = localStorage.getItem("username");
var auth = localStorage.getItem("auth");
var logReportName = "";
var device = "";
if (isMobile) {
    device = "mobile"
}
else {
    device = "web"
}

var date = new Date();
var sqlDate = date.toISOString().slice(0, 19).replace('T', ' ');

var year = date.getFullYear().toString();

var month = date.getMonth() + 1;
month = month.toString();

var testData = "";
var testDataChild = "";
var headId = "";

var getMenuTop = [
    {
        menuId: "QuickStats",
        menuTop: "94"
    },
    {
        menuId: "Scorecard",
        menuTop: "155"
    },
    {
        menuId: "Operations",
        menuTop: "215"
    },
    {
        menuId: "Finance",
        menuTop: "275"
    },
    {
        menuId: "HeadOffice",
        menuTop: "345"
    },
    {
        menuId: "Help",
        menuTop: "405"
    }
];

if (localStorage.getItem("userRole") == 1) {
    testData = w_data_One;
    testDataChild = w_data_childmenu;
}
else {
    testData = w_data_zero;
    testDataChild = w_data_childmenu;
}

//var link = document.getElementById("loadCss");
//if (isMobile) {
//    link.setAttribute('href', "../Content/style/mobile.css");
//    location.replace(window.location.href + '&d=m');
//} else {
//    debugger;
//    link.setAttribute('href', "../Content/style/chatime.css");
//    location.replace(window.location.href + '&d=d');
//}

$(document).ready(function () {
    window.addEventListener("orientationchange", function () {
        // Announce the new orientation number
        if (window.innerWidth < window.innerHeight) {
            //  alert("landscape");
            $(".mobile-landind, .desktop_cont").css("display", "none");
            $(".orientation").css("display", "block");
        } else {
            //alert("portrait");          
            $(".orientation").css("display", "none");
            $(".mobile-landind, .desktop_cont").css("display", "block");
            location.reload();
        }
    }, false);

    var activeMenu = '';
    if (activeMenu !== '') {
        console.log(activeMenu);
        //console.log($('#' + activeMenu + ''));
        $('#' + activeMenu + ".blueDot").css("display", "inline-block");
        console('>>>>>>', sessionStorage.getItem('bluedot'));
    }
    $(".topMenu").click(function () {
        //alert("topMenuClicked");
        $(".submenu").css("display", "none");
        if ($(".sidebar").hasClass("sidebarActive")) {
            $(".sidebar").removeClass("sidebarActive");
            $(".sidebar").addClass("sidebarInactive");
        }
        else {
            $(".sidebar").removeClass("sidebarInactive");
            $(".sidebar").addClass("sidebarActive");
        }
    });

    $(".head-block-list,.option-head").click(function () {
        $(".submenu").css("display", "none");
        if ($(this).parents().find("div.sidebar").hasClass("sidebarActive")) {
            if ($(this).children(".submenu").hasClass('activeSubmenu')) {
                $(this).children(".submenu").removeClass('activeSubmenu');
                $(this).children(".submenu").css("display", "none");
            }
            else {
                $(this).children(".submenu").addClass('activeSubmenu');
                $(this).children(".submenu").css("display", "block");
            }
        }
    });

    $(document).click(function (e) {
        //debugger;
        var container = $(".sidebar");
        var currentClass = $(e.target).attr("class");
        if (!container.is(e.target) && container.has(e.target).length === 0) {
            container.removeClass("sidebarActive");
            container.addClass("sidebarInactive");
            $(".submenu").css("display", "none");
        }
    });

    //for mobile
    //$(".mobile-menu-icon i").click(function () {
    //    $('.m_menubar_overlay').css("display", "block");
    //});
    //$('.m_overoption_head').click(function () {
    //    $('.m_overlay_submenu').css("display", "none");
    //    $(this).siblings('.m_overlay_submenu').toggle();
    //})
    //$('.close-overlay').click(function () {
    //    $('.m_menubar_overlay').css("display", "none");
    //});

    //pass submenu value to the url
    $(".submenu-new li, .submenu li, .m_overlay_submenu li, .childlnk-onhover li, .childlnk-onclick li").click(function () {
        var bChildLinks = false;
        //code to check menu's has child links
        for (var i = 0; i < testDataChild.length; i++) {
            if (testDataChild[i].submenuid) {
                if (testDataChild[i].submenuid == this.id) {
                    bChildLinks = true;
                }
            }
        }

        if (bChildLinks)
            return false;

        var report_name = $(this).attr('id');
        logReportName = report_name;
        $(this).addClass('activeLink');
        //  console.log(''child);
        //alert(report_name)
        var newReport_name = report_name.replace(/&/g, "");
        callLog();
        if (isMobile) {
            //for mobile 
            window.location.href = baseURL + "Home/EmbedReport?name=" + newReport_name + "&d=m";
            //window.location.href = "http://localhost:42734/Home/EmbedReport?name=" + newReport_name + "&d=d";
        }
        else {
            //for Web
            if (report_name === "Finance") {
                window.location.href = baseURL + "Home/HeadOffice?name=Finance";
            }
            else if (report_name === "Marketing") {
                window.location.href = baseURL + "Home/HeadOfficeMarketing?name=Marketing";
            }
            else {
                window.location.href = baseURL + "Home/EmbedReport?name=" + newReport_name + "&d=d";
            }

            //window.location.href = "http://localhost:42734/Home/EmbedReport?name=" + newReport_name;
        }
        var activeMenu = report_name;
        console.log(document.getElementById(activeMenu).children[0].className);
        var ide = document.getElementById(activeMenu).children[0].className;
        //console.log('.' + ide);
        //$(this).children('.' + ide).css('display', 'Inline-block');
        //sessionStorage.setItem("bluedot", ide);
    });
    function callLog() {
        debugger;
        var data = {
            "UserName": user,
            "Year": year,
            "ProjectName": "Chatime",
            "Month": month,
            "Date": sqlDate,
            "createdTimeStamp": sqlDate,
            "UpdatedTimeStamp": sqlDate,
            "ReportName": logReportName,
            "DeviceType": device,
            "Session": auth
        }
        console.log(data, "data")
        $.ajax({
            url: RESTURL + 'api/userlogin/logg',
            type: 'post',
            data: data,
            dataType: 'json',
            async: true,
            success: function (response) {
                console.log(JSON.parse(response));
            }
        })
    }
    $("#logoutBtn, .m_logout").click(function () {
        window.location.href = baseURL + "Home/Index";
        sessionStorage.clear();
    });

    $(".mobile-chatime-logo img").click(function () {
        //alert("Hiiiii");
        window.location.href = baseURL + "Home/Landing";
        sessionStorage.clear();
    });
});

//Added function to display the subbmenu-> child menu
$(document).ready(function () {
    $(".submenu-new li").mouseover(function () {
        var bChildLinks = false;
        var menuTopH = "0";

        for (var i = 0; i < testData.length; i++) {
            if (testData[i].submenu) {
                for (var j = 0; j < testData[i].submenu.length; j++) {
                    if (testData[i].submenu[j] == this.id) {
                        headId = testData[i].blockid;
                    }
                }
            }
        }
        for (var i = 0; i < testDataChild.length; i++) {
            if (testDataChild[i].submenuid) {
                if (testDataChild[i].submenuid == this.id) {
                    bChildLinks = true;
                }
            }
        }
        var submenuNewLi = [];
        $('#' + headId + ' ' + '.submenu-new li').each(function () {
            submenuNewLi.push($(this).text().trim());
        });

        var offsetTopH = "", liHeight = 30, index=0;
        if(submenuNewLi.length > 0) {
            index = submenuNewLi.indexOf(this.id);
            offsetTopH = (index == 0) ? parseInt(menuTopH) : liHeight * index;
        } 

        var pId = (this.id).replace(/[\s, \s&]+/g, "") 
        $('.' + pId + '-onHover').addClass("childdiv-onhover");
        $('.' + pId + '-onHover ul').addClass("childlnk-onhover");

        $('.' + pId + '-onHover').css("top", offsetTopH + 'px');
        $('.' + pId + '-onHover').css("display", "inline-block");

        $('.' + pId + '-onHover').mouseover(function () {
            $('.' + pId + '-onHover').css("display", "inline-block");
        });

        $('.' + pId + '-onHover').mouseout(function () {
            $('.' + pId + '-onHover').css("display", "none");
        });

    });

    $(".submenu-new li").mouseleave(function () {
        var id = (this.id).replace(/[\s, \s&]+/g, "");
        $('.' + id + '-onHover').css("display", "none");
    });

});

//Added function to display the subbmenu-> child menu by Navigation click
$(document).ready(function () {
    $(".submenu li").mouseover(function () {
        var bChildLinks = false;
        var menuTopH = "0";

        for (var i = 0; i < testData.length; i++) {
            if (testData[i].submenu) {
                for (var j = 0; j < testData[i].submenu.length; j++) {
                    if (testData[i].submenu[j] == this.id) {
                        headId = testData[i].blockid;
                    }
                }
            }
        }
        for (var i = 0; i < testDataChild.length; i++) {
            if (testDataChild[i].submenuid) {
                if (testDataChild[i].submenuid == this.id) {
                    bChildLinks = true;
                }
            }
        }
        var submenuNewLi = [];
        $('#' + headId + ' ' + '.submenu li').each(function () {
            submenuNewLi.push($(this).text().trim());
        });

        var offsetTopH = "", liHeight = 30, index = 0, liTop = "0";
        for (var j = 0; j < getMenuTop.length; j++) {
            if (getMenuTop[j].menuId === headId) {
                //getting Top if navbar-list scrolled down
                var scrollMove = ($(".navbar-list").scrollTop());
                liTop = (scrollMove == 0) ? getMenuTop[j].menuTop : (getMenuTop[j].menuTop) - scrollMove;
            }    
        }

        if (submenuNewLi.length > 0)
            index = submenuNewLi.indexOf(this.id);

        offsetTopH = (index == 0) ? parseInt(liTop) : (submenuNewLi.length - 1) * liHeight + parseInt(liTop);

        var pId = (this.id).replace(/[\s, \s&]+/g, "")
        $('.' + pId + '-onclick').addClass("childdiv-onclick");
        $('.' + pId + '-onclick ul').addClass("childlnk-onclick");

        $('.' + pId + '-onclick').css("top", offsetTopH + 'px');
        $('.' + pId + '-onclick').css("display", "inline-block");

        $('.' + pId + '-onclick').mouseover(function () {
            $('.' + pId + '-onclick').css("display", "inline-block");
        });

        $('.' + pId + '-onclick').mouseout(function () {
            $('.' + pId + '-onclick').css("display", "none");
        });

    });

    $(".submenu li").mouseleave(function () {
        var id = (this.id).replace(/[\s, \s&]+/g, "");
        $('.' + id + '-onclick').css("display", "none");
    });

});