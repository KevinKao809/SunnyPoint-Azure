<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TripPlaybackGoogleMap.aspx.cs" Inherits="spWebConsole.embed.TripPlaybackGoogleMap" %>

<!-- ION Slider -->
<link href="/assets/plugins/ion-rangeslider/ion.rangeSlider.css" rel="stylesheet" type="text/css" />
<link href="/assets/plugins/ion-rangeslider/ion.rangeSlider.skinFlat.css" rel="stylesheet" type="text/css" />

<!-- range slider js -->
<script src="/assets/plugins/ion-rangeslider/ion.rangeSlider.min.js"></script>
<script src="/assets/pages/jquery.ui-sliders.js"></script>

<div style="padding: 10px;">
    <div class="row">
        <div class="col-lg-3 col-md-6">
            <div class="card-box widget-user">
                <div>
                    <img src="assets/images/carPlayback/rating-128.png" class="img-responsive img-circle" alt="user">
                    <div class="wid-u-info">
                        <h4 class="m-t-0 m-b-5">Rating</h4>
                        <h2 class="text-warning"><%=tripScore %></h2>
                        <h5 class="text-custom"><%=tripNegativeEvent %> Negative Evts</h5>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-lg-3 col-md-6">
            <div class="card-box widget-user">
                <div>
                    <img src="assets/images/carPlayback/trip-start-128.png" class="img-responsive img-circle" alt="user">
                    <div class="wid-u-info">
                        <h4 class="m-t-0 m-b-5">Trip Start</h4>
                        <h3 class="text-success"><%=tripStartDT %></h3>
                        <h5 class="text-custom">&nbsp;</h5>
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
                        <h2 class="text-custom"><%=tripDistance %></h2>
                        <h5 class="text-custom">Duration <%=tripDuration %></h5>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-lg-3 col-md-6">
            <div class="card-box widget-user">
                <div>
                    <img src="assets/images/carPlayback/max-speed-128.png" class="img-responsive img-circle" alt="user">
                    <div class="wid-u-info">
                        <h4 class="m-t-0 m-b-5">Max Speed</h4>
                        <h2 class="text-info"><%=tripMaxSpeed %></h2>
                        <h5 class="text-custom">Avg <%=tripAvgSpeed %> km</h5>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <table style="width: 100%; height: 680px;">
        <tr>
            <td style="width: 80%" rowspan="5">
                <div id='map' style='width: 100%; height: 680px;'></div>
            </td>
            <td style="width: 60px; padding-left: 10px;">
                <label for="playSpeed"><b>Speed</b></label>
            </td>
            <td>
                <div class="col-sm-10">
                    <input type="text" id="playSpeed" onchange="javascript:changePlaySpeed();">
                </div>
            </td>
        </tr>
        <tr>
            <td style="width: 60px; padding-left: 10px;">
                <label for="checkbox2">Locked Car</label>
            </td>
            <td>
                <input style="margin-left: 10px;" id="lockedCarFlag" type="checkbox">
            </td>
        </tr>
        <tr>
            <td colspan="2" style="padding-left: 10px;">
                <div id="nodeID" style="width: 146px; text-align: center;"></div>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="padding-left: 10px;">
                <button type="button" id="PlayPauseButton" style="width: 120px;" onclick="PlayPauseClick()">Play</button></td>
        </tr>
        <tr style="height: 200px">
            <td colspan="2">&nbsp;</td>
        </tr>
    </table>
</div>

<script>
    var map;
    var car;            
    var counter = 0;
    var playSpeed = 10;
    var isPlaying = false;
    var infowindow;
    var routingPath = [<%=_routingPath%>];
    var EventTitles = [<%=_eventTitle%>];
    var EventDesc = [<%=_eventDesc%>];            
    var EventImage = [<%=_eventImage%>];
    var tripEventPoints = [<%=_eventPoint%>];        
    var tripPoints = [<%=_tripPoints%>];
    var eventMarker = [];

    function initMap() {
        map = new google.maps.Map(document.getElementById('map'), {
            center: <%=_mapCenter%>,
            zoom: <%=_zoom%>
            });

        /* Put Routing Path */            
        var flightPath = new google.maps.Polyline({
            path: routingPath,
            geodesic: true,
            strokeColor: '#FF0000',
            strokeOpacity: 1.0,
            strokeWeight: 2
        });
        flightPath.setMap(map);

        /*  Put the Car  */            
        var imageCar = '<%=_carIcon%>';
            car = new google.maps.Marker({
                position: <%=_carStartLocation%>,
                map: map,
                icon: imageCar
            });

        /* Put All Message Event */
            for (var i = 0; i < tripEventPoints.length; i++) {
                var loc = tripEventPoints[i].split(",");
                var position = new google.maps.LatLng(
                    parseFloat(loc[0]),
                    parseFloat(loc[1])
                )
                eventMarker[i] = new google.maps.Marker({
                    position: position,
                    map: map,
                    icon: EventImage[i],
                    id: i
                });
                google.maps.event.addListener(eventMarker[i], 'click', function() {
                    showInfoBox(map, eventMarker[this.id].id);
                });
            }

        /* Put InfoBox */
            infowindow = new google.maps.InfoWindow({
                content: ''
            });
        }

        function showInfoBox(map, i)
        {
            var loc = tripEventPoints[i].split(",");
            var position = new google.maps.LatLng(
                parseFloat(loc[0]),
                parseFloat(loc[1])
            )
            infowindow.open(map);
            infowindow.setContent(EventTitles[i] + '<br/>' + EventDesc[i]);
            infowindow.setPosition(position);
        }

        var continuePlayback = false;
        function PlayPauseClick()
        {
            var o = document.getElementById('PlayPauseButton');
            if (o.innerHTML == "Play")
            {
                o.innerHTML = "Pause";
                continuePlayback = true;
                playback();
            }
            else
            {
                continuePlayback = false;
                o.innerHTML = "Play";
            }
        }

        var interval = null;
        var totalSegment, currentSegment;
        var latOffset, lngOffset;
        var preLat, preLng;
        function playback()
        {            
            if (counter == tripPoints.length-1)
            {
                document.getElementById('nodeID').innerHTML = "Playback Done";
                var o = document.getElementById('PlayPauseButton');
                continuePlayback = false;
                o.innerHTML = "Play";
                counter = 0;
                return;
            }

            if (!continuePlayback)
                return;

            var pointAValue = tripPoints[counter].split(",");
            var pointBValue = tripPoints[counter+1].split(",");
            preLat = pointAValue[0];
            preLng = pointAValue[1];
            counter = counter + 1;

            totalSegment = (pointBValue[2]-pointAValue[2]) / playSpeed;

            document.getElementById('nodeID').innerHTML = "Point: " + counter + "/ Speed: " + pointAValue[3];

            if (totalSegment > 0)
            {
                latOffset = (pointBValue[0]-pointAValue[0]) / totalSegment;
                lngOffset = (pointBValue[1]-pointAValue[1]) / totalSegment;

                currentSegment = 0;
                interval = setInterval(moveCar, 100);
            }
            else
            {
                playback(); 
            }                       
        }

        function moveCar()
        {            
            if (currentSegment >= totalSegment)
            {
                clearInterval(interval);                
                playback();                
            }
            else
            {
                var nextLat = preLat + latOffset;
                var nextLng = preLng + lngOffset;

                var newPosition = new google.maps.LatLng(
                    parseFloat(nextLat),
                    parseFloat(nextLng)
                )                          
                car.setPosition(newPosition);

                var o = document.getElementById('lockedCarFlag');
                if (o.checked)
                {
                    map.setCenter(newPosition)
                }            
            }
            currentSegment = currentSegment + 1;
        }

        function changePlaySpeed()
        {
            playSpeed = document.getElementById('playSpeed').value;
        }
</script>
<script src="http://maps.googleapis.com/maps/api/js?key=<%=_GoogleAPIKey%>&callback=initMap"
    async defer></script>

