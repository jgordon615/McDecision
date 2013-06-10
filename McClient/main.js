"use strict";

var net = require("net"),
    mc = require("./lib/mc.js");

var settings = {
    server: {
        host: "mc.gordon-online.com",
        port: 25545
    },
    login: {
        name: "shortstack615",
        password: "ricky123"
    }
};

function connectPlayer(loginInfo) {
    var conn = net.connect(settings.server, function() {
        console.log("Connected to ", settings.server);
        mc.connectPlayer(conn, settings.server, loginInfo, function(err) {
            if (err) {
                console.warn(err);
                conn.end();
            }

            console.log("???");
        });
    });

    conn.on("error", function(err) {
        console.warn("Error:", err);
    });
    conn.on("timeout", function(err) {
        console.warn("Timeout:", err);
    });
    conn.on("data", function(data) {
        console.warn("Data:", data.toString('ucs2'));
    });
    conn.on("end", function() {
        console.warn("Connection closed.");
    });
}

function login() {
    mc.login(settings.login, function(err, loginInfo) {
        console.log("Login Success");
        console.log(loginInfo);

        connectPlayer(loginInfo);
    });
}

login();


