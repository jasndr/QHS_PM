<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HeatMap.aspx.cs" Inherits="ProjectManagement.Guest.HeatMap" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type='text/javascript'>

        var map = null;
        var ppLayer = null;  // pushpin layer
        var hmLayer = null;  // heat map layer
        var reader = null;   // FileReader object
        var locs = [];       // lat-lon locations
        var cGrad = { '0.0': 'black', '0.2': 'purple', '0.4': 'blue', '0.6': 'green', '0.8': 'yellow', '0.9': 'orange', '1.0': 'red' };

        var hmOptions = { intensity: 0.65, radius: 7, colorGradient: cGrad };

        function GetMap() {
            var options = {
                credentials: "AjNMuq1bFP-KIchNdXxyhME7mdRsXJzoa2MAptWCzxryFupaHxN_AQYyA0sts4rA",
                center: new Microsoft.Maps.Location(20.78, -156.88), mapTypeId: Microsoft.Maps.MapTypeId.road,
                zoom: 8, enableClickableLogo: false, showCopyright: false
            };

            var mapDiv = document.getElementById("mapDiv");  // where to place map
            map = new Microsoft.Maps.Map(mapDiv, options);   // display map

            ppLayer = new Microsoft.Maps.Layer();
            var cpp = new Microsoft.Maps.Pushpin(map.getCenter(), null);  // a centered default-style pushpin
            ppLayer.add(cpp);
            map.layers.insert(ppLayer);

            //for (key in cGrad) {
            //  WriteLn(key + " " + cGrad[key]);
            //}

        }

        function Button1_Click() {
            //var f = file1.files[0];  // get filename
            //WriteLn('Loading data from ' + f.name + "\n");
            //reader = new FileReader();

            //reader.onload = function (e) {  // after file is read . . 
            //    var lines = reader.result.split('\n');
            //    for (var i = 0; i < lines.length; ++i) {  // each line
            //        var line = lines[i];
            //        var tokens = line.split('\t');  // split on tabs
            //        var loc = new Microsoft.Maps.Location(tokens[12], tokens[13]);  // lat-lon at [12] and [13]
            //        locs[i] = loc;
            //    }

            //    //Microsoft.Maps.loadModule('Microsoft.Maps.HeatMap', function () {
            //    //    hmLayer = new Microsoft.Maps.HeatMapLayer(locs, hmOptions);
            //    //    map.layers.insert(hmLayer);
            //    //});

                

            //}
            //reader.readAsText(f);  // read the file asynchronously

            WriteLn('Show heat map' + "\n");
            //Load the GeoJSON and HeatMap modules
            Microsoft.Maps.loadModule(['Microsoft.Maps.GeoJson', 'Microsoft.Maps.HeatMap'], function () {
                // earthquake data in the past 30 days from usgs.gov
                Microsoft.Maps.GeoJson.readFromUrl('http://earthquake.usgs.gov/earthquakes/feed/v1.0/summary/all_month.geojson', function (shapes) {
                    hmLayer = new Microsoft.Maps.HeatMapLayer(shapes, { radius: 5 });
                    map.layers.insert(hmLayer);
                });
            });
        }


        function Button2_Click() {
            WriteLn('Clear heat map' + "\n");
            hmLayer.clear();
            reader = null;
            locs = [];
        }


        function WriteLn(txt) {
            var existing = msgArea.value;
            msgArea.value = existing + txt + "\n";
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
   
    <div style="float:left; width:10px; height:800px"></div> <!-- spacer -->
    <div id='mapDiv' style="float:left; width:1400px; height:800px; border:1px solid red;"></div>
    
    <div style="float:left; width:10px; height:800px"></div> <!-- spacer -->     
    <div id='controlPanel' style="float:left; width:262px; height:580px; border:1px solid green; padding:10px; background-color: beige">

       
        <input type="file" id="file1" size="24" hidden>  <!-- initial value not possible -->
        <span style="display:block; height:10px"></span>  <!-- vertical spacer -->

        <input id="button1" type='button' value='Show Heat Map' style="width:120px;" onclick="Button1_Click();"></input>
        <div style="width:2px; display:inline-block"></div>    <!-- left-right spacer -->
        <input id="textbox1" type='text' size='16' value='<not used>' hidden></input><br/>
        <span style="display:block; height:10px"></span>  <!-- vertical spacer -->

        <input id="button2" type='button' value='Clear Heat Map' style="width:120px;" onclick="Button2_Click();"></input>
        <div style="width:2px; display:inline-block"></div>    <!-- left-right spacer -->
        <input id="textbox2" type='text' size='16' value='<not used>' hidden></input><br/>
        <span style="display:block; height:10px"></span>  <!-- vertical spacer -->

        

        <textarea id='msgArea' rows="34" cols="36" style="font-family:Consolas; font-size:12px"></textarea>
    </div>
    <br style="clear: left;" />  <!-- magic formatting -->
        
    <script type='text/javascript' src='http://www.bing.com/api/maps/mapcontrol?callback=GetMap' async defer></script>
    </form>
</body>
</html>
