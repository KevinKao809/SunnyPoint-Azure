<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminDongleProvisioning.aspx.cs" Inherits="spWebConsole.embed.AdminDongleProvisioning" %>

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
                    <a href="#navpills-1" data-toggle="tab" aria-expanded="true">Add A New Dongle</a>
                </li>
                <li class="">
                    <a href="#navpills-2" data-toggle="tab" aria-expanded="false">Batch Provisioning</a>
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
                                    <label for="inputEmail3" class="col-sm-3 control-label">Device ID </label>
                                    <div class="col-sm-9">
                                        <input type="text" class="form-control" style="width: 400px;" value="">
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="inputEmail3" class="col-sm-3 control-label">IoT Hub Host Name</label>
                                    <div class="col-sm-9">
                                        <input type="text" class="form-control" style="width: 400px;" value="">
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-3 control-label">Protocol </label>
                                    <div class="col-sm-9">
                                        <select class="form-control" style="width: 400px;">
                                            <option selected="selected">MQTT</option>
                                            <option>HTTPS</option>
                                            <option>AMQP</option>
                                        </select>
                                    </div>
                                </div>
                                <div class="form-group m-b-0">
                                    <div class="col-sm-offset-3 col-sm-9">
                                        <button type="submit" class="btn btn-info waves-effect waves-light" >Add</button>
                                    </div>
                                </div>
                                <div style="height:30px;"></div>
                                <div class="form-group">
                                    <label class="col-sm-3 control-label">Result</label>
                                    <div class="col-md-9">
                                        <textarea class="form-control" rows="5" style="width: 400px;"></textarea>
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
                                <div class="form-group">
                                    <label for="inputEmail3" class="col-sm-3 control-label">CSV File </label>
                                    <div class="col-sm-9">
                                        <input type="file" class="form-control" style="width: 400px;" value="">
                                    </div>
                                </div>
                                
                                <div class="form-group m-b-0">
                                    <div class="col-sm-offset-3 col-sm-9">
                                        <button type="submit" class="btn btn-info waves-effect waves-light" >Upload</button>
                                    </div>
                                </div>
                                <div style="height:30px;"></div>
                                <div class="form-group">
                                    <label class="col-sm-3 control-label">Result</label>
                                    <div class="col-md-9">
                                        <textarea class="form-control" rows="5" style="width: 400px;"></textarea>
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

