<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="spWebConsole.index" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="description" content="A fully featured admin theme which can be used to build CRM, CMS, etc.">
    <meta name="author" content="Coderthemes">
    <link rel="shortcut icon" href="assets/images/favicon.ico">

    <title>Sunny Point UBI</title>

    <!-- App css -->
    <link href="assets/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="assets/css/core.css" rel="stylesheet" type="text/css" />
    <link href="assets/css/components.css" rel="stylesheet" type="text/css" />
    <link href="assets/css/icons.css" rel="stylesheet" type="text/css" />
    <link href="assets/css/pages.css" rel="stylesheet" type="text/css" />
    <link href="assets/css/menu.css" rel="stylesheet" type="text/css" />
    <link href="assets/css/responsive.css" rel="stylesheet" type="text/css" />

    <!-- HTML5 Shiv and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
    <script src="https://oss.maxcdn.com/libs/html5shiv/3.7.0/html5shiv.js"></script>
    <script src="https://oss.maxcdn.com/libs/respond.js/1.3.0/respond.min.js"></script>
    <![endif]-->

    <script src="assets/js/modernizr.min.js"></script>
</head>
<body class="fixed-left">
    <div id="loadingDiv" style="position:absolute; background-color:#0095c8; width:2560px; height:1440px; visibility:collapse"><img id="loadingImage" src='assets/images/Loading.jpg' style='width:400px; height:200px; margin-left:400px; margin-top:300px;' /></div>
    <!-- Begin page -->
    <div id="wrapper">
        <!-- Top Bar Start -->
        <div class="topbar">
            <!-- LOGO -->
            <div class="topbar-left">
                <img onclick="window.open('/index.aspx')" src="assets/images/service.png" style="margin-top:10px; width:100px; height:65px;" />
            </div>            
        </div>
    </div>
    <!-- Top Bar End -->
    <!-- ========== Left Sidebar Start ========== -->
    <div class="left side-menu">
        <div class="sidebar-inner slimscrollleft">
            <div class="card-box">
                <div class="radio radio-info radio-inline">
                    <input type="radio" onchange="accountTypeChange('enterprise');" id="accountTypeEnterprise" value="Enterprise" name="accountType" checked>
                    <label for="accountTypeEnterprise">Enterprise </label>
                </div>
                <div class="radio radio-inline">
                    <input type="radio" onchange="accountTypeChange('consumer');" id="accountTypeConsumer" value="Consumer" name="accountType" checked>
                    <label for="accountTypeConsumer">Consumer </label>
                </div>
            </div>
            <div style="margin-left: 20px;">
                <div class="btn-group dropdown">
                    <button type="button" class="btn btn-primary waves-effect waves-light" data-toggle="dropdown" style="width: 170px;">Account List</button>
                    <button type="button" class="btn btn-primary dropdown-toggle waves-effect waves-light" data-toggle="dropdown" aria-expanded="false"><i class="caret"></i></button>
                    <ul id="accountList" class="dropdown-menu" role="menu">
                        <%=_consumerList %>
                    </ul>
                </div>
            </div>
            <!-- User -->
            <div id="userBox" class="user-box" style="visibility: collapse">
                <div class="user-img">
                    <img id="accountImage" src="assets/images/users/avatar-1.jpg" alt="user-img" title="Mat Helme" class="img-circle img-thumbnail img-responsive">
                    <div class="user-status offline"><i class="zmdi zmdi-dot-circle"></i></div>
                </div>
                <div id="accountName">
                    <h5></h5>
                </div>
            </div>
            <!-- End User -->
            <!--- Sidemenu -->
            <div id="sidebar-menu" style="visibility: collapse">
                <ul>
                    <li id="DashboardLink">
                        <a href="#" class="waves-effect"><i class="zmdi zmdi-view-dashboard"></i><span>Dashboard </span></a>
                    </li>

                    <li id="TripAnalytics" class="has_sub">
                        <a href="javascript:void(0);" class="waves-effect"><i class="ti-car"></i><span>Trip Analytics </span><span class="menu-arrow"></span></a>
                        <ul id="TripItems" class="list-unstyled">
                        </ul>
                    </li>

                    <li id="OBDDongleLink">
                        <a href="#" class="waves-effect"><i class="ti-archive"></i><span>OBD Dongle </span></a>
                    </li>
                </ul>
                <div class="clearfix"></div>
            </div>

            <!-- Sidebar -->
            <div class="clearfix"></div>

        </div>

    </div>
    <!-- Left Sidebar End -->
    <!-- ============================================================== -->
    <!-- Start right Content here -->
    <!-- ============================================================== -->


    <div id="contentDiv" class="content-page" style="visibility:visible;">
        <img src="assets/images/referenceSolution.JPG" style="width:1024px; height:576px;" />
    </div>

    <!-- ============================================================== -->
    <!-- End Right content here -->
    <!-- ============================================================== -->
    <!-- END wrapper -->

    <script>
        var resizefunc = [];
    </script>

    <!-- jQuery  -->
    <script src="assets/js/jquery.min.js"></script>
    <script src="assets/js/bootstrap.min.js"></script>
    <script src="assets/js/detect.js"></script>
    <script src="assets/js/fastclick.js"></script>
    <script src="assets/js/jquery.blockUI.js"></script>
    <script src="assets/js/waves.js"></script>
    <script src="assets/js/jquery.nicescroll.js"></script>
    <script src="assets/js/jquery.slimscroll.js"></script>
    <script src="assets/js/jquery.scrollTo.min.js"></script>

    <!-- KNOB JS -->
    <!--[if IE]>
    <script type="text/javascript" src="assets/plugins/jquery-knob/excanvas.js"></script>
        <script src="assets/plugins/jquery-knob/jquery.knob.js"></script>
    <![endif]-->


    <!--Morris Chart
    <script src="assets/plugins/morris/morris.min.js"></script>
    <script src="assets/plugins/raphael/raphael-min.js"></script>
    -->
    <!-- Dashboard init
    <script src="assets/pages/jquery.dashboard.js"></script>
     -->    

    <!-- App js -->
    <script src="assets/js/jquery.core.js"></script>
    <script src="assets/js/jquery.app.js"></script>

    <script type="text/javascript">

        var loadingImage = document.getElementById('loadingImage');
        loadingImage.style.marginLeft = (screen.availWidth * 7 / 24) + "px";
        loadingImage.style.marginTop = (screen.availHeight * 7 / 24) + "px";        

        $(document).ready(function () {

            $('#DashboardLink').click(function () {
                SwitchToLoadingDiv();
                var monitorPage = '<iframe src="https://msit.powerbi.com/view?r=eyJrIjoiZDVlY2IxZjktYWI2ZC00MTU1LTkyNWMtZWY3NGExODBhZDc4IiwidCI6IjcyZjk4OGJmLTg2ZjEtNDFhZi05MWFiLTJkN2NkMDExZGI0NyIsImMiOjV9' + '&t=' + Date.now() + '" style="width:100%; height:650px;" frameBorder="0" ></iframe>';
                $("#contentDiv").html(monitorPage);
                SwitchToContentDiv();
            });

            $('#OBDDongleLink').click(function () {
                loadContent('embed/DeviceManagement.aspx?aid=' + selectedAccountID);
            });

            function loadContent(pageURL) {
                SwitchToLoadingDiv();
                $.ajax({
                    type: "GET",
                    url: pageURL + "&t=" + Date.now(),
                    success: function (data) {
                        var o = document.getElementById('contentDiv');
                        o.innerHTML = data;
                        SwitchToContentDiv();
                    },
                    failure: function (response) {
                        alert("failed");
                    }
                });
            }
        });
    
        var consumerAccountList = '<%=_consumerList%>';
        var enterpriseAccountList = '<%=_enterpriseList%>';
        function accountTypeChange(accountType) {
            var o = document.getElementById('accountList');

            if (accountType == 'consumer')
                o.innerHTML = consumerAccountList;
            else
                o.innerHTML = enterpriseAccountList;

            selectedAccountID = -1;

            var userBox = document.getElementById('userBox');
            userBox.style.visibility = "collapse";

            var menu = document.getElementById('sidebar-menu');
            menu.style.visibility = "collapse";
        }

        var selectedAccountID = -1;
        function accountSelection(accountID, accountName, photoURL) {
            if (selectedAccountID == accountID)
                return;

            selectedAccountID = accountID;
            /* AJAX to get TripList by account ID */
            $(function () {
                $.ajax({
                    type: "GET",
                    url: "services/getTripListByAccountID.aspx?aid=" + accountID + "&t=" + Date.now(),
                    success: function (data) {
                        var json_obj = $.parseJSON(data);
                        var tripListContent = "";
                        for (var i in json_obj) {
                            tripListContent = tripListContent + "<li><a href='javascript:tripSelection(\"" + json_obj[i].tripID + "\")'>" + json_obj[i].tripName + "</a></li>";
                        }
                        o = document.getElementById('TripItems');
                        o.innerHTML = tripListContent;
                    },
                    failure: function (response) {
                        alert("failed");
                    }
                });
            });

            var o = document.getElementById('accountName');
            o.innerHTML = "<h5>" + accountName + "</h5>";

            var accountImage = document.getElementById('accountImage');
            accountImage.src = photoURL;

            var userBox = document.getElementById('userBox');
            userBox.style.visibility = "visible";

            var menu = document.getElementById('sidebar-menu');
            menu.style.visibility = "visible";
        }

        var selectedTripID;
        function tripSelection(tripID) {            
            selectedTripID = tripID;
            $(function () {
                SwitchToLoadingDiv();
                $.ajax({
                    type: "GET",
                    url: "embed/<%=_tripPlaybackPage%>?tid=" + selectedTripID + "&t=" + Date.now(),
                    success: function (data) {                        
                        $("#contentDiv").html(data);
                        $("#contentDiv").find("script").each(function (i) {
                            eval($(this).text());
                        });
                        SwitchToContentDiv();                        
                    },
                    failure: function (response) {
                        alert("failed");
                    }
                });
            });
        }

        var contentDiv = document.getElementById('contentDiv');
        var loadingDiv = document.getElementById('loadingDiv');
        function SwitchToLoadingDiv()
        {            
            contentDiv.style.visibility = "collapse";            
            loadingDiv.style.visibility = "visible";
        }

        function SwitchToContentDiv()
        {            
            loadingDiv.style.visibility = "collapse";            
            contentDiv.style.visibility = "visible";
        }
    </script>
</body>
</html>
