"use strict";

var request = require("request"),
    dos = require("./dataOutputStream");

var packetIds = {
    KeepAlive: 0,
    Login: 1,
    ClientProtocol: 2,
    Chat: 3,
    UpdateTime: 4,
    PlayerInventory: 5,
    SpawnPosition: 6,
    UseEntity: 7,
    UpdateHealth: 8,
    Respawn: 9,
    Flying: 10,
    PlayerPosition: 11,
    PlayerLook: 12,
    PlayerLookMove: 13,
    BlockDig: 14,
    Place: 15,
    BlockItemSwitch: 16,
    Sleep: 17,
    Animation: 18,
    EntityAction: 19,
    NamedEntitySpawn: 20,
    Collect: 22,
    VehicleSpawn: 23,
    MobSpawn: 24,
    EntityPainting: 25,
    EntityExpOrb: 26,
    EntityVelocity: 27,
    DestroyEntity:28,
    Entity: 30,
    RelEntityMove: 31,
    EntityLook: 32,
    RelEntityMoveLook: 33,
    EntityTeleport: 34,
    EntityHeadRotation: 35,
    EntityStatus: 36,
    AttachEntity: 39,
    EntityMetaData: 40,
    EntityEffect: 41,
    RemoveEntityEffect: 42,
    Experience: 43,

    MapChunk: 51,
    MultiBlockChange: 52,
    BlockChange: 53,
    PlayNoteBlock: 54,
    BlockDestroy: 55,
    MapChunks: 56,

    Explosion: 60,
    DoorChange: 61,
    LevelSound: 62,
    WorldParticles: 63,

    GameEvent: 70,
    Weather: 71,

    OpenWindow: 100,
    CloseWindow: 101,
    WindowClick: 102,
    SetSlot: 103,
    WindowItems: 104,
    UpdateProgressbar: 105,
    Transation: 106,
    CreativeSetSlot: 107,
    EnchantItem: 108,

    UpdateSign: 130,
    MapData: 131,
    TileEntityData: 132,

    Statistic: 200,
    PlayerAbilities: 202,
    AutoComplete: 203,
    ClientInfo: 204,
    ClientCommand: 205,
    SetObjective: 206,
    SetScore: 207,
    DisplayObjective: 208,
    SetPlayerTeam: 209,

    CustomPayload: 250,
    SharedKey: 252,
    ServerAuthData: 253,
    ServerPing: 254,
    KickDisconnect: 255
};
var protocolVersion = 61;

exports.login = function login(creds, callback) {
    var url = "http://login.minecraft.net";

    request.post(url, {form:{user:creds.name, password: creds.password, version: 13}}, function(err, response, body) {
        if (err)
            return callback(err);

        if (body.match(/\:/)){
            var terms = body.split(":");
            callback(null, {
                user: terms[2],
                token: terms[3]
            });
        } else {
            return callback("Failed to authenticate: " + body);
        }
    });
}


exports.connectPlayer = function connectPlayer(connection, serverInfo, loginInfo, callback) {
    var str = dos(connection);

    //console.log(loginInfo.user);
    //console.log(serverInfo.host);
    //console.log(serverInfo.port);

    str.writeShort(2);
    str.writeByte(61);
    str.writeString(loginInfo.user);
    str.writeString(serverInfo.host);
    str.writeInt(serverInfo.port);


//    str.writeShort(packetIds.Login);
//    str.writeInt(0);                            // Client Entity Id
//    str.writeString("flat");                    // World Type
//    str.writeByte(0);                           // Hardcore
//    str.writeByte(0);                           // Dimension
//    str.writeByte(0);                           // Difficulty
//    str.writeByte(0);                           // World Height
//    str.writeByte(0);                           // Max Players
//    str.writeByte(0); //?
//    str.writeByte(0); //?
//    str.writeInt();

//    str.writeShort(protocolVersion);
//    str.writeString(loginInfo.user, 16);
//    str.writeString(serverInfo.host, 255);
//    str.writeInt(serverInfo.port);


};