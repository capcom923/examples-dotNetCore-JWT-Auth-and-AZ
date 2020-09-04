using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace DotNetJWT_Self_Issue_Token_Auth
{
    public static class RsaUtils
    {
        public static RsaSecurityKey CreateRsaPublicKeyByPem(string pemPublicKey)
        {
            var keyParams = ConvertFromPemPublicKey(pemPublicKey);
            var rsa = RSA.Create();
            rsa.ImportParameters(keyParams);
            return new RsaSecurityKey(rsa);
        }

        public static RsaSecurityKey CreateRsaPrivateKeyByPem(string pemPrivateKey)
        {
            var keyParams = ConvertFromPemPrivateKey(pemPrivateKey);
            var rsa = RSA.Create();
            rsa.ImportParameters(keyParams);
            return new RsaSecurityKey(rsa);
        }

        private static RSAParameters ConvertFromPemPublicKey(string pemPublicKey)
        {
            if (string.IsNullOrEmpty(pemPublicKey))
            {
                throw new Exception("PEM格式公钥不可为空。");
            }
            //移除干扰文本
            pemPublicKey = pemPublicKey.Replace("-----BEGIN PUBLIC KEY-----", "")
                .Replace("-----END PUBLIC KEY-----", "").Replace("\n", "").Replace("\r", "");
            byte[] keyData = Convert.FromBase64String(pemPublicKey);
            bool keySize1024 = (keyData.Length == 162);
            bool keySize2048 = (keyData.Length == 294);
            if (!(keySize1024 || keySize2048))
            {
                throw new Exception("公钥长度只支持1024和2048。");
            }
            byte[] pemModulus = (keySize1024 ? new byte[128] : new byte[256]);
            byte[] pemPublicExponent = new byte[3];
            Array.Copy(keyData, (keySize1024 ? 29 : 33), pemModulus, 0, (keySize1024 ? 128 : 256));
            Array.Copy(keyData, (keySize1024 ? 159 : 291), pemPublicExponent, 0, 3);
            RSAParameters para = new RSAParameters();
            para.Modulus = pemModulus;
            para.Exponent = pemPublicExponent;
            return para;
        }

        private static RSAParameters ConvertFromPemPrivateKey(string pemPrivateKey)
        {
            if (string.IsNullOrEmpty(pemPrivateKey))
            {
                throw new Exception("PEM格式私钥不可为空。");
            }
            //移除干扰文本
            pemPrivateKey = pemPrivateKey.Replace("-----BEGIN RSA PRIVATE KEY-----", "")
                .Replace("-----END RSA PRIVATE KEY-----", "").Replace("\n", "").Replace("\r", "");
            byte[] keyData = Convert.FromBase64String(pemPrivateKey);
            RSAParameters rsaParams = new RSAParameters();
            byte[] MODULUS, E, D, P, Q, DP, DQ, IQ;
            byte bt = 0;
            ushort twobytes = 0;
            int elems = 0;
            //set up stream to decode the asn.1 encoded RSA private key
            //wrap Memory Stream with BinaryReader for easy reading
            using (BinaryReader binaryReader = new BinaryReader(new MemoryStream(keyData)))
            {
                twobytes = binaryReader.ReadUInt16();
                //data read as little endian order (actual data order for Sequence is 30 81)
                if (twobytes == 0x8130)
                {
                    //advance 1 byte
                    binaryReader.ReadByte();
                }
                else if (twobytes == 0x8230)
                {
                    //advance 2 bytes
                    binaryReader.ReadInt16();
                }
                else
                {
                    return rsaParams;
                }
                twobytes = binaryReader.ReadUInt16();
                //version number
                if (twobytes != 0x0102)
                {
                    return rsaParams;
                }
                bt = binaryReader.ReadByte();
                if (bt != 0x00)
                {
                    return rsaParams;
                }
                //all private key components are Integer sequences
                elems = GetIntegerSize(binaryReader);
                MODULUS = binaryReader.ReadBytes(elems);
                elems = GetIntegerSize(binaryReader);
                E = binaryReader.ReadBytes(elems);
                elems = GetIntegerSize(binaryReader);
                D = binaryReader.ReadBytes(elems);
                elems = GetIntegerSize(binaryReader);
                P = binaryReader.ReadBytes(elems);
                elems = GetIntegerSize(binaryReader);
                Q = binaryReader.ReadBytes(elems);
                elems = GetIntegerSize(binaryReader);
                DP = binaryReader.ReadBytes(elems);
                elems = GetIntegerSize(binaryReader);
                DQ = binaryReader.ReadBytes(elems);
                elems = GetIntegerSize(binaryReader);
                IQ = binaryReader.ReadBytes(elems);
                //create RSACryptoServiceProvider instance and initialize with public key
                rsaParams = new RSAParameters
                {
                    Modulus = MODULUS,
                    Exponent = E,
                    D = D,
                    P = P,
                    Q = Q,
                    DP = DP,
                    DQ = DQ,
                    InverseQ = IQ
                };
            }
            return rsaParams;
        }

        private static int GetIntegerSize(BinaryReader binaryReader)
        {
            byte bt = 0;
            byte lowbyte = 0x00;
            byte highbyte = 0x00;
            int count = 0;

            bt = binaryReader.ReadByte();

            //expect integer
            if (bt != 0x02)
            {
                return 0;
            }
            bt = binaryReader.ReadByte();

            if (bt == 0x81)
            {
                //data size in next byte
                count = binaryReader.ReadByte();
            }
            else if (bt == 0x82)
            {
                //data size in next 2 bytes
                highbyte = binaryReader.ReadByte();
                lowbyte = binaryReader.ReadByte();
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                count = BitConverter.ToInt32(modint, 0);
            }
            else
            {
                //we already have the data size
                count = bt;
            }
            while (binaryReader.ReadByte() == 0x00)
            {   //remove high order zeros in data
                count -= 1;
            }
            //last ReadByte wasn't a removed zero, so back up a byte
            binaryReader.BaseStream.Seek(-1, SeekOrigin.Current);
            return count;
        }
    }
}
