let pontosGrafico = [];
let poligono = null;
async function montarMapa() {
    let dataInicio = $('#dataInicio').val();
    let dataFim = $('#dataFim').val();
    var resp;

    if (dataInicio == null || dataFim == null || dataFim < dataInicio) {
        alert('datas invalidas');
        return;
    }

    let url = baseUrl + `api/medicoes/grafico?dataInicio=${dataInicio}&dataFim=${dataFim}`;
    var settings = {
        "url": url,
        "method": "GET",
        "timeout": 0,
        "success": function (data) {
            resp = data;
        },
        async: false
    };

    $.ajax(settings);

    const { Map } = await google.maps.importLibrary("maps");
    const { AdvancedMarkerView } = await google.maps.importLibrary("marker");
    const { drawing } = await google.maps.importLibrary("drawing");
    const { geometry } = await google.maps.importLibrary("geometry");

    var bounds = new google.maps.LatLngBounds();
    // The map, centered at Uluru
    map = new Map(document.getElementById("map"), {
        zoom: 15,
        center: { lat: 0, lng: 0 },
        mapId: "DEMO_MAP_ID",
    });

    // The marker, positioned at Uluru
    resp.forEach(function (item, i) {
        var data = new Date(item.data_Medicao);
        item.data_Medicao = data.getHours() + ":" + data.getMinutes() + " " + data.getDate() + "/" + (data.getMonth() + 1) + "/" + data.getFullYear();
        var html = `<div class="conteiner">
                        <span>Latitude: ${item.latitude}</span><br>
                        <span>Longitude: ${item.longitude}</span><br>
                        <span>Temperatura: ${item.temperatura}</span><br>
                        <span>Umidade: ${item.umidade}</span><br>
                        <span>Velocidade: ${item.velocidade}</span><br>
                        <span>Data: ${item.data_Medicao}</span><br>

                    </div>`;

        var marker = new AdvancedMarkerView({
            map: map,
            position: {
                lat: item.latitude,
                lng: item.longitude
            },
            title: item.data_medicao,
        });
        var infowindow = new google.maps.InfoWindow({
            content: html,
            ariaLabel: "marker",
        });
        marker.addListener("click", () => {
            infowindow.open({
                anchor: marker,
                map,
            });
        });
        bounds.extend(marker.position);

    });
    map.fitBounds(bounds);



    var drawingManager = new google.maps.drawing.DrawingManager({
        drawingMode: google.maps.drawing.OverlayType.POLYGON,
        drawingControl: true,
        drawingControlOptions: {
            position: google.maps.ControlPosition.TOP_CENTER,
            drawingModes: [
                google.maps.drawing.OverlayType.POLYGON
            ],
        },

    });
    drawingManager.setMap(map);

    google.maps.event.addListener(drawingManager, 'polygoncomplete', function (poli) {
        if (poligono != null) {
            poligono.setMap(null);
        }
        poligono = poli;
        pontosGrafico = [];
        for (var i = 0; i < resp.length; i++) {
            if (google.maps.geometry.poly.containsLocation({ lat: resp[i].latitude, lng: resp[i].longitude }, poli)) {
                pontosGrafico.push(resp[i]);
            }
        }
        montarGrafico();
    });
}