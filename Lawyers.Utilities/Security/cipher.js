var crypto = require('crypto')
    ,fs = require('fs');

//加密
function cipher(algorithm, key, buf){
  var encrypted = "";
  var iv = new Buffer(16);
  iv.fill(0);
  var cip = crypto.createCipheriv(algorithm, key, iv);
  encrypted += cip.update(buf, 'utf8', 'hex');
  encrypted += cip.final('hex');
  return encrypted;
}

//解密
function decipher(algorithm, key, encrypted){
  var decrypted = "";
  var iv = new Buffer(16);
  iv.fill(0);
  var decipher = crypto.createDecipheriv(algorithm, key, iv);
  decrypted += decipher.update(encrypted, 'hex', 'utf8');
  decrypted += decipher.final('utf8');
  return decrypted;
}

var key = 'MYun456!@#ZXD*(art)!?jhb-qwe%^&w';
var algorithm = 'aes-256-cbc';

var cipher_str = cipher(algorithm,key,"{appUid:'123456789',platformId:2104}");
var decipher_str = decipher(algorithm,key,cipher_str);
console.log(cipher_str);
console.log(decipher_str);
