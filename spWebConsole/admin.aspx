<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="admin.aspx.cs" Inherits="spWebConsole.admin" %>

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
                <img src="assets/images/service.png" style="margin-top:10px; width:100px; height:65px;" />                
            </div>            
        </div>
    </div>
    <!-- Top Bar End -->
    <!-- ========== Left Sidebar Start ========== -->
    <div class="left side-menu">
        <div class="sidebar-inner slimscrollleft">            
            <!-- User -->
            <div id="userBox" class="user-box" style="visibility: visible">
                <div class="user-img">
                    <img id="accountImage" src="assets/images/users/avatar-1.jpg" alt="user-img" title="Admin" class="img-circle img-thumbnail img-responsive">
                    <div class="user-status offline"><i class="zmdi zmdi-dot-circle"></i></div>
                </div>
                <div id="accountName">
                    <h5>Administrator</h5>
                </div>
            </div>
            <!-- End User -->
            <!--- Sidemenu -->
            <div id="sidebar-menu" style="visibility: visible">
                <ul>
                    <li id="DongleAppLink">
                        <a href="#" class="waves-effect"><i class="zmdi zmdi-view-dashboard"></i><span>Dongle Application </span></a>
                    </li>

                    <li id="DongleProvisioningLink">
                        <a href="#" class="waves-effect"><i class="zmdi zmdi-view-dashboard"></i><span>Dongle Provisioning </span></a>
                    </li>

                    <li id="OperationTask" class="has_sub">
                        <a href="javascript:void(0);" class="waves-effect"><i class="ti-car"></i><span>Operation Tasks </span><span class="menu-arrow"></span></a>
                        <ul id="Tasks" class="list-unstyled">
                            <li><a href='javascript:taskSelection("incompleteTrip")'>Incomplete Trip</a></li>
                            <li><a href='javascript:taskSelection("cleanFailTrip")'>Clean Fail Trip</a></li>
                            <li><a href='javascript:taskSelection("reScoring")'>Re-Scoring</a></li>
                            <li><a href='javascript:taskSelection("accountStatistic")'>Account Statistic</a></li>
                            <li><a href='javascript:taskSelection("updateDashboard")'>Update Dashboard</a></li>
                        </ul>
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


    <div id="contentDiv" class="content-page" style="visibility:collapse;">
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

            $('#DongleAppLink').click(function () {
                loadContent('embed/AdminAppConfig.aspx?dummy=1');
            });

            $('#DongleProvisioningLink').click(function () {
                loadContent('embed/AdminDongleProvisioning.aspx?dummy=1');
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

        var selectedTaskName;
        function taskSelection(taskName, run) {
            if (selectedTaskName == taskName)
                return;

            selectedTaskName = taskName;
            $(function () {
                SwitchToLoadingDiv();
                $.ajax({
                    type: "GET",
                    url: "embed/AdminOperationTask.aspx?command=" + taskName + "&run=" + run + "&t=" + Date.now(),
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

        
    function mapTypeChange(mapProvider)
    {
        $(function () {
            $.ajax({
                type: "GET",
                url: "services/setAppValue.aspx?name=mapProvider&value=" + mapProvider + "&t=" + Date.now(),
                success: function (data) {
                    
                },
                failure: function (response) {
                    alert("failed");
                }
            });
        });
    }

    </script>
</body>
</html>