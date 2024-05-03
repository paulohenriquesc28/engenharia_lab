// Initialize and add the map
let map;

async function initMap(lat, long) {
    // The location of Uluru
    const position = { lat: lat, lng: long };
    // Request needed libraries.
    //@ts-ignore
    const { Map } = await google.maps.importLibrary("maps");
    const { AdvancedMarkerView } = await google.maps.importLibrary("marker");

    // The map, centered at Uluru
    map = new Map(document.getElementById("map"), {
        zoom: 15,
        center: position,
        mapId: "DEMO_MAP_ID",
    });

    // The marker, positioned at Uluru
    const marker = new AdvancedMarkerView({
        map: map,
        position: position,
        title: "Uluru",
    });

}
