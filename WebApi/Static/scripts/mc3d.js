function init3d() {
    function render() {
        requestAnimationFrame(render);
        //camera.rotation.x += 0.001;
        //camera.rotation.y += 0.001;
        renderer.render(scene, camera);

        showStats();
    }
    function showStats() {
        stats.find('#camX').text(camera.position.x);
        stats.find('#camY').text(camera.position.y);
        stats.find('#camZ').text(camera.position.z);
    }

    var structure = new THREE.Object3D;

    var width = window.innerWidth;
    var height = window.innerHeight;

    var scene = new THREE.Scene();
    scene.add(structure);

    var camera = new THREE.PerspectiveCamera(75, width / height, 0.1, 100);
    camera.position.set(2, 4.9, 2);
    camera.lookAt(new THREE.Vector3(0, 0, 0));

    var ambientLight = new THREE.AmbientLight(0xFFFFFF);
    scene.add(ambientLight);

    //var directionalLight = new THREE.DirectionalLight(0xffffff);
    //directionalLight.position.set(0, 5, 0);
    //directionalLight.lookAt(new THREE.Vector3(0, 0, 0));
    //scene.add(directionalLight);


    var renderer = new THREE.WebGLRenderer();
    renderer.setSize(width, height);

    setTimeout(function () {
        $('canvas').css('position', 'absolute');
    }, 1);

    document.body.appendChild(renderer.domElement);
    window.addEventListener('resize', onWindowResize, false);

    var stats = $('<div><span>camX: </span><span id="camX"></span><br /><span>camY: </span><span id="camY"></span><br /><span>camZ: </span><span id="camZ"></span></div>');
    $('body').append(stats);

    document.body.addEventListener('mousewheel', function (event) {
        if (event.wheelDelta > 0)
            camera.position.y -= 0.1;
        else
            camera.position.y += 0.1;
        return false;
    }, false)

    drawGuideLines(scene);

    var loader = new THREE.OBJMTLLoader();
    loader.addEventListener('load', function (event) {
        structure.add(event.content);
        render();
    }, false);    loader.load("render/McDecision.obj", "render/McDecision.mtl");
}

function onWindowResize() {

    windowHalfX = window.innerWidth / 2;
    windowHalfY = window.innerHeight / 2;

    camera.aspect = window.innerWidth / window.innerHeight;
    camera.updateProjectionMatrix();

    renderer.setSize(window.innerWidth, window.innerHeight);

}




function drawGuideLines(scene) {
    var geometry = new THREE.CubeGeometry(0.05, 0.05, 0.05);
    var redMaterial = new THREE.MeshBasicMaterial({ color: 0xff0000 });
    var greenMaterial = new THREE.MeshBasicMaterial({ color: 0x00ff00 });
    var blueMaterial = new THREE.MeshBasicMaterial({ color: 0x0000ff });
    for (var i = -10.0; i < 10.0; i += 0.1) {
        var cubeX = new THREE.Mesh(geometry, redMaterial);
        cubeX.position.set(i, 0, 0);
        scene.add(cubeX);

        var cubeY = new THREE.Mesh(geometry, blueMaterial);
        cubeY.position.set(0, i, 0);
        scene.add(cubeY);

        var cubeZ = new THREE.Mesh(geometry, greenMaterial);
        cubeZ.position.set(0, 0, i);
        scene.add(cubeZ);
    }
}