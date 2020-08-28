let maps = document.getElementsByClassName("facilityZone");

const mapSize = 10;

var lczTable;
var hczTable;
var ezTable;

let MapIsMoving;
let MousePos = { x: 0, y: 0 };

window.ondragstart = function() { return false; }

for (let i = 0; i < maps.length; i++) {

    let map = new Array(mapSize);

    for (let a = 0; a < mapSize; a++) {

        map[a] = new Array(mapSize);
        let tableRaw = document.createElement('tr');
        maps[i].append(tableRaw);

        for (let b = 0; b < mapSize; b++) {
            map[a][b] = document.createElement('td');
            tableRaw.append(map[a][b]);
        }
    }

    switch (i) {
        case "0":
            lcz = map;
            break;
        case "1":
            hcz = map;
            break;
        case "2":
            ez = map;
            break;
    }
}

function mapMove(ev) {
    if (MapIsMoving) {
        window.scrollBy(MousePos.x - ev.x, MousePos.y - ev.y);
        MousePos.x = ev.x;
        MousePos.y = ev.y;
    }
}

function mapPress(ev) {
    document.body.style.setProperty('cursor', 'all-scroll');
    MousePos.x = ev.x;
    MousePos.y = ev.y;
    MapIsMoving = true;
}

function mapUnpress(ev) {
    document.body.style.setProperty('cursor', 'default');
    MapIsMoving = false;
}