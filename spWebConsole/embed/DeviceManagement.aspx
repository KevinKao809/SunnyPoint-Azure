<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeviceManagement.aspx.cs" Inherits="spWebConsole.embed.DeviceManagement" %>

<div style="padding: 10px;">
    <div class="row">
        <div class="col-lg-3 col-md-6">
            <div class="card-box widget-user">
                <div>
                    <img src="assets/images/carPlayback/dongleN-128.png" class="img-responsive img-circle" alt="user">
                    <div class="wid-u-info">
                        <h4 class="m-t-0 m-b-5">Dongle #</h4>
                        <h2 class="text-warning"><%=_dongleAmount %></h2>
                    </div>
                </div>
            </div>
        </div>

        <div class="col-lg-3 col-md-6">
            <div class="card-box widget-user">
                <div>
                    <img src="assets/images/carPlayback/distance-128.png" class="img-responsive img-circle" alt="user">
                    <div class="wid-u-info">
                        <h4 class="m-t-0 m-b-5">Distance</h4>
                        <h2 class="text-custom"><%=_tripDistance %> km</h2>
                    </div>
                </div>
            </div>
        </div>

        <div class="col-lg-3 col-md-6">
            <div class="card-box widget-user">
                <div>
                    <img src="assets/images/carPlayback/trips-128.png" class="img-responsive img-circle" alt="user">
                    <div class="wid-u-info">
                        <h4 class="m-t-0 m-b-5">Trips</h4>
                        <h2 class="text-success"><%=_trips %></h2>
                    </div>
                </div>
            </div>
        </div>

        <div class="col-lg-3 col-md-6">
            <div class="card-box widget-user">
                <div>
                    <img src="assets/images/carPlayback/negative-evt-128.png" class="img-responsive img-circle" alt="user">
                    <div class="wid-u-info">
                        <h4 class="m-t-0 m-b-5">Negative Evt</h4>
                        <h2 class="text-info"><%=_negativeEvent %></h2>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-sm-12">
            <div class="card-box">
                <a href="#" class="waves-effect"><i class="ti-archive"></i><span> OBD Dongle List </span></a>
                <div class="table-responsive">
                    <table id="mainTable" class="table table-striped m-b-0">
                        <thead>
                            <tr>
                                <th>ID</th>
                                <th>IoT Host</th>
                                <th>Axis</th>
                                <th>HardAcc Value</th>
                                <th>HardBreak Value</th>
                                <th>Registed Cty</th>
                                <th>Delete</th>
                            </tr>
                        </thead>
                        <tbody>
                            <%=_deviceTableList %>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>
</div>

