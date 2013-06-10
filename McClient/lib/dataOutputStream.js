"use explicit";

module.exports = function setup(stream){
    var obj  = {};

    obj.writeShort = function(v) {
        stream.write(new Buffer([
            0xff & v,
            0xff & (v >> 8)
        ]));
    };

    obj.writeInt = function(v) {
        stream.write(
            new Buffer([
                0xff & v,
                0xff & (v >> 8),
                0xff & (v >> 16),
                0xff & (v >> 24)
            ])
        );
    };

    obj.writeString = function(v) {
        var s = v.split('');
        var b = new Buffer(v, 'ucs2');

//        for(; i<len; i++){
//            b[i*2] = 0xff & s[i];
//            b[i*2+1] = 0xff & (s[i] >> 8);
//        }

        obj.writeShort(b.length);
        stream.write(b);
    };

    obj.writeByte = function(byte) {
        var b = new Buffer(1);
        b[0] = byte;
        stream.write(b);
    }

    return obj;
};
