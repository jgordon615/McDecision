"use strict";

var packetId = 201;

exports.writePlayerInfoPacket = function writePlayerInfoPacket(str, user) {
    str.writeShort(packetId);
    str.writeString(user);
    str.writeByte(1);
    str.writeShort(50);
}