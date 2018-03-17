<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminAppConfig.aspx.cs" Inherits="spWebConsole.embed.AdminAppConfig" %>

<div style="padding: 10px;">
    <div class="row">
        <div class="col-lg-3 col-md-6">
            <div class="card-box widget-user">
                <div>
                    <img src="assets/images/carPlayback/dongleN-128.png" class="img-responsive img-circle" alt="user">
                    <div class="wid-u-info">
                        <h4 class="m-t-0 m-b-5">Dongle Qty</h4>
                        <h2 class="text-warning">200</h2>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-lg-3 col-md-6">
            <div class="card-box widget-user">
                <div>
                    <img src="assets/images/carPlayback/dongleAct-128.png" class="img-responsive img-circle" alt="user">
                    <div class="wid-u-info">
                        <h4 class="m-t-0 m-b-5">Activated #</h4>
                        <h2 class="text-warning">132</h2>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-lg-3 col-md-6">
            <div class="card-box widget-user">
                <div>
                    <img src="assets/images/carPlayback/dongleConnected-128.png" class="img-responsive img-circle" alt="user">
                    <div class="wid-u-info">
                        <h4 class="m-t-0 m-b-5">Connected # C</h4>
                        <h2 class="text-warning">58</h2>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="panel panel-color panel-tabs panel-success">
        <div class="panel-heading">
            <ul class="nav nav-pills pull-right">
                <li class="active">
                    <a href="#navpills-1" data-toggle="tab" aria-expanded="true">Dongle Application Config</a>
                </li>
                <li class="">
                    <a href="#navpills-2" data-toggle="tab" aria-expanded="false">Backend Application Config</a>
                </li>
            </ul>
            <h3 class="panel-title">&nbsp;</h3>
        </div>
        <div class="panel-body">
            <div class="tab-content">
                <div id="navpills-1" class="tab-pane fade in active">
                    <div class="row">
                        <div class="col-md-12">
                            <form class="form-horizontal" role="form">
                                <div class="form-group">
                                    <label for="inputEmail3" class="col-sm-3 control-label">Message Upload Interval in Sec</label>
                                    <div class="col-sm-9">
                                        <input type="text" class="form-control" style="width: 400px;" value="300">
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="inputPassword3" class="col-sm-3 control-label">Heartbeat Interval in Sec </label>
                                    <div class="col-sm-9">
                                        <input type="text" class="form-control" style="width: 400px;" value="15">
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="inputPassword4" class="col-sm-3 control-label">Adaptive Heartbeat </label>
                                    <div class="col-sm-9">
                                        <div class="checkbox checkbox-primary">
                                            <input id="checkbox2" type="checkbox" checked="checked">
                                            <label for="checkbox2">
                                                Enabled
                                            </label>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="inputPassword3" class="col-sm-3 control-label">Accept Log on Cloud </label>
                                    <div class="col-sm-9">
                                        <div class="checkbox checkbox-primary">
                                            <input id="checkbox3" type="checkbox" checked="checked">
                                            <label for="checkbox3">
                                                Enabled
                                            </label>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="inputPassword3" class="col-sm-3 control-label">Mininum Application Version </label>
                                    <div class="col-sm-9">
                                        <input type="text" class="form-control" style="width: 400px;" value="1.0.1">
                                    </div>
                                </div>
                                <div class="form-group m-b-0">
                                    <div class="col-sm-offset-3 col-sm-9">
                                        <button type="submit" class="btn btn-info waves-effect waves-light">Save</button>
                                    </div>
                                </div>
                            </form>
                        </div>
                    </div>
                </div>
                <div id="navpills-2" class="tab-pane fade">
                    <div class="row">
                        <div class="col-md-12">
                            <form class="form-horizontal" role="form">
                                <div class="card-box">
                                    <div class="radio radio-info radio-inline">
                                        <input type="radio" onchange="mapTypeChange('Bing');" id="BingMap" value="Bing" name="MapProvider" <%=_bingChecked %>>
                                        <label for="BingMap">Microsoft Bing Map </label>
                                    </div>
                                    <div class="radio radio-inline">
                                        <input type="radio" onchange="mapTypeChange('Google');" id="GoogleMap" value="Google" name="MapProvider" <%=_googleChecked%>>
                                        <label for="GoogleMap">Google Map </label>
                                    </div>
                                </div>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>



