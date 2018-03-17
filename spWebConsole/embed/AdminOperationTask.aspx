<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminOperationTask.aspx.cs" Inherits="spWebConsole.embed.AdminOperationTask" %>

<script type="text/javascript">
    $(document).ready(function () {
            var _logAppName = '<%=_logAppName%>';
            var _logLevel, _logDateRange;
            var _submitTasksMessage = '<%=_submitTasksMessage%>';
            var interval = null;

            if (_submitTasksMessage.length > 0) {
                $('#logRecords').val(_submitTasksMessage);
                interval = setInterval(refreshLog, 15000);
            }

            $('#manualRefresh').click(function () {
                refreshLog();
            });

            $('#auto_stopRefresh').click(function () {
                var o = document.getElementById('auto_stopRefresh');
                if (o.value == '1') {
                    clearInterval(interval);
                    o.value = '0';
                    o.innerHTML = "Auto Refresh";
                }
                else {
                    interval = setInterval(refreshLog, 15000);
                    o.value = '1';
                    o.innerHTML = "Stop Refresh";
                }
            });

            function refreshLog() {
                clearInterval(interval);
                var logLevel = document.getElementById('logLevel');
                _logLevel = logLevel.value;                
                var logDateRange = document.getElementById('logDateTimeRange');
                _logDateRange = logDateRange.value;                

                $('#logRecords').val("");
                $.ajax({
                    type: "GET",
                    url: "services/getLog.aspx?appName=" + _logAppName + "&logLevel=" + _logLevel + "&logDateTimeRange=" + _logDateRange + "&t=" + Date.now(),
                    success: function (data) {
                        var json_obj = $.parseJSON(data);
                        var content = "";
                        for (var i in json_obj) {
                            content = content + json_obj[i].Time + "  :  " + json_obj[i].Level + " , " + json_obj[i].Message + " ,Detail:" + json_obj[i].Detail + " ,Class:" + json_obj[i].ClassName + " ,Method:" + json_obj[i].Method + "\n";
                        }
                        content = "Log Updated. \n" + content;
                        $('#logRecords').val(content);
                        var o = document.getElementById('auto_stopRefresh');
                        if (o.value == '1')
                            interval = setInterval(refreshLog, 15000);
                    },
                    failure: function (response) {
                        alert("failed");
                    }
                });
            }
        });
</script>

<div style="padding: 10px; width: 80%;">
    <div style="width: 300px; height: 50px;">
        <h2><%=_taskName %></h2>
    </div>
    <table border="0">
        <tr>
            <td rowspan="7">
                <textarea id="logRecords" cols="90" rows="32" style="overflow: auto;"></textarea>
            </td>
            <td style="padding-left: 6px">Log Group  
            </td>
            <td>
                <select style="width: 100px" name="logGroup">
                    <option value="dongleService" selected="selected">Dongle Service</option>
                    <option value="queueTask">Web Admin</option>
                    <option value="adminWeb">Backend Task</option>
                </select>
            </td>
        </tr>
        <tr>
            <td style="padding-left: 6px">Log Level
            </td>
            <td>
                <select style="width: 100px" id="logLevel" name="logLevel">
                    <option value="info" selected>Inforamtion</option>
                    <option value="debug">Debug</option>
                    <option value="warning">Warning</option>
                    <option value="error">Error</option>
                </select>
            </td>
        </tr>
        <tr>
            <td style="padding-left: 6px">Date Range
            </td>
            <td>
                <select style="width: 100px" id="logDateTimeRange" name="logDateTimeRange">
                    <option value="30min" selected>Less 30 Min</option>
                    <option value="1hour">Less 1 Hour</option>
                    <option value="today">Today</option>
                    <option value="week">Week</option>
                </select>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="padding-left: 6px">
                <button style="width: 180px" name="manualRefresh" id="manualRefresh" type="button" value="Manual Refresh">Manual Refresh</button><br />
            </td>
        </tr>
        <tr>
            <td colspan="2" style="padding-left: 6px">
                <button style="width: 180px" name="auto_stopRefresh" id="auto_stopRefresh" type="button" value="1">Stop Refresh</button>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="padding-left: 6px">
                <button style="width: 180px" name="exportLog" id="exportLog" type="button" value="1">Export Logs</button>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="padding-left: 6px; height: 120px">
                <button style="width: 180px; height: 80px; visibility: <%=_runButtonVisibility%>" name="runButton" id="runButton" type="button" onclick="taskSelection('<%=_taskCommand %>', 'true');" value="1">Submit Task</button>
            </td>
        </tr>
    </table>
</div>
