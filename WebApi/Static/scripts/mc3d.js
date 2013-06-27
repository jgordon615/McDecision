function init3d() {
    function onWindowResize() {
        camera.aspect = window.innerWidth / window.innerHeight;
        camera.updateProjectionMatrix();

        renderer.setSize(window.innerWidth, window.innerHeight);

        render();
    }
    function animate() {
        requestAnimationFrame(animate);
        controls.update();
        //console.log(camera.position);
    }
    function render() {
        renderer.render(scene, camera);
    }


    var structure = new THREE.Object3D;

    var width = window.innerWidth;
    var height = window.innerHeight;

    var scene = new THREE.Scene();
    scene.add(structure);

    var camera = new THREE.PerspectiveCamera(45, width / height, 0.1, 10000);
    camera.position.set(0, 1.434, 3.153);
    camera.lookAt(new THREE.Vector3(1, 0.55, 1));

    var controls = new THREE.OrbitControls(camera);
    controls.addEventListener('change', render);

    // Disable panning.  It doesn't work right for us.
    controls.userPanSpeed = 0.0;

    var ambientLight = new THREE.AmbientLight(0x999999);
    scene.add(ambientLight);

    var renderer = new THREE.WebGLRenderer();
    renderer.setSize(width, height);

    document.body.appendChild(renderer.domElement);
    window.addEventListener('resize', onWindowResize, false);

    var skybox = addSkybox(scene, render);
    //addGuideLines(scene);
    addMinecraftModel(structure, render);

    setTimeout(function () {
        $('canvas').css('position', 'absolute');
        $('canvas').css('top', '0px');
        $('canvas').css('left', '0px');
        $('canvas').css('width', '100%');
        $('canvas').css('height', '100%');
        $('canvas').css('z-index', '-1');
        animate();
        render();
    }, 1);
    setInterval(function () {
        render();
    }, 100);

    var result = {
        setText: function (text) {
            //console.log("setText:", text);
            if (textObj) {
                scene.remove(textObj);
            }

            textObj = addTextToScene(text, scene);
            scene.add(textObj);
            render();
        }
    };
    var textObj = null;
    result.setText("Press the button!", scene);
    return result;
}


function addTextToScene(text, scene) {
    function createText() {

        textGeo = new THREE.TextGeometry(text, {

            size: size,
            height: height,
            curveSegments: curveSegments,

            font: font,
            weight: weight,
            style: style,

            bevelThickness: bevelThickness,
            bevelSize: bevelSize,
            bevelEnabled: bevelEnabled,

            material: 0,
            extrudeMaterial: 1

        });

        textGeo.computeBoundingBox();
        textGeo.computeVertexNormals();

        // "fix" side normals by removing z-component of normals for side faces
        // (this doesn't work well for beveled geometry as then we lose nice curvature around z-axis)

        if (!bevelEnabled) {

            var triangleAreaHeuristics = 0.1 * (height * size);

            for (var i = 0; i < textGeo.faces.length; i++) {

                var face = textGeo.faces[i];

                if (face.materialIndex == 1) {

                    for (var j = 0; j < face.vertexNormals.length; j++) {

                        face.vertexNormals[j].z = 0;
                        face.vertexNormals[j].normalize();

                    }

                    var va = textGeo.vertices[face.a];
                    var vb = textGeo.vertices[face.b];
                    var vc = textGeo.vertices[face.c];

                    var s = THREE.GeometryUtils.triangleArea(va, vb, vc);

                    if (s > triangleAreaHeuristics) {

                        for (var j = 0; j < face.vertexNormals.length; j++) {

                            face.vertexNormals[j].copy(face.normal);

                        }

                    }

                }

            }

        }

        var centerOffset = -0.5 * (textGeo.boundingBox.max.x - textGeo.boundingBox.min.x);

        textMesh1 = new THREE.Mesh(textGeo, material);

        textMesh1.position.x = centerOffset;
        textMesh1.position.y = hover;
        textMesh1.position.z = 0;

        textMesh1.rotation.x = 0;
        textMesh1.rotation.y = Math.PI * 2;

        parent.add(textMesh1);
    }

    var height = 20,
		size = 70,
		hover = 30,

		curveSegments = 4,

		bevelThickness = 2,
		bevelSize = 1.5,
		bevelSegments = 3,
		bevelEnabled = true,

		font = "helvetiker", // helvetiker, optimer, gentilis, droid sans, droid serif
		weight = "normal", // normal bold
		style = "normal"; // normal italic

    var fontMap = {
        "helvetiker": 0,
        "optimer": 1,
        "gentilis": 2,
        "droid sans": 3,
        "droid serif": 4
    };

    var weightMap = {
        "normal": 0,
        "bold": 1
    };

    var reverseFontMap = {};
    var reverseWeightMap = {};

    for (var i in fontMap) reverseFontMap[fontMap[i]] = i;
    for (var i in weightMap) reverseWeightMap[weightMap[i]] = i;

    var postprocessing = { enabled: false };
    var glow = 0.9;

    var hash = document.location.hash.substr(1);

    //material = new THREE.MeshFaceMaterial([
    //    new THREE.MeshPhongMaterial({ color: 0x000000, shading: THREE.FlatShading }), // front
    //    new THREE.MeshPhongMaterial({ color: 0x000000, shading: THREE.SmoothShading }) // side
    //]);

    material = new THREE.MeshBasicMaterial({ color: 0xFFFFFF });
    //material = new THREE.LineBasicMaterial({ color: 0xAAAAFF, linewidth: 0.01 });

    parent = new THREE.Object3D();
    parent.scale.set(0.003, 0.003, 0.003);
    parent.position.y = 0.5;

    createText();

    return parent;
}

function addSkybox(scene) {
    var urlPrefix = "render/skybox/";
    var urls = [
		urlPrefix + "pos-x.png",
		urlPrefix + "neg-x.png",
		urlPrefix + "pos-y.png",
		urlPrefix + "neg-y.png",
		urlPrefix + "pos-z.png",
		urlPrefix + "neg-z.png"
    ];
    var cubemap = THREE.ImageUtils.loadTextureCube(urls);
    cubemap.format = THREE.RGBFormat;

    // following code from https://github.com/mrdoob/three.js/blob/master/examples/webgl_materials_cubemap.html
    var shader = THREE.ShaderLib["cube"];
    shader.uniforms["tCube"].value = cubemap;

    var material = new THREE.ShaderMaterial({
        fragmentShader: shader.fragmentShader,
        vertexShader: shader.vertexShader,
        uniforms: shader.uniforms,
        depthWrite: false,
        side: THREE.BackSide
    }),
        mesh = new THREE.Mesh(new THREE.CubeGeometry(10000, 10000, 10000), material);

    scene.add(mesh);
    return mesh;
}

function addGuideLines(scene) {
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

function addMinecraftModel(scene, callback) {
    var loader = new THREE.OBJMTLLoader();
    loader.addEventListener('load', function (event) {
        scene.add(event.content);
        callback();
    }, false);
    loader.load("render/McDecision3.obj", "render/McDecision3.mtl");
}

