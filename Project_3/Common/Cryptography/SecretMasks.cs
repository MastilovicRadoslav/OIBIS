using System;

namespace Common.Cryptography
{
    public class SecretMasks
    {
        public readonly string keyMask = "041A316984FA5C290DAADAA658A4A7A553ED767E7D7F09864308722D20D4852B8";
        public readonly string ivMask = "38C770BAB50F42896866373BCDAFFF90";

        public (byte[], byte[]) KeyAndIVEncryption(byte[] key, byte[] iV)
        {
            byte[] retKey = new byte[key.Length];
            byte[] retIV = new byte[iV.Length];

            for (int i = 0; i < key.Length; i++)
            {
                retKey[i] = (byte)(key[i] ^ Convert.ToByte(keyMask.Substring(i * 2, 2), 16));
            }

            for (int i = 0; i < iV.Length; i++)
            {
                retIV[i] = (byte)(iV[i] ^ Convert.ToByte(ivMask.Substring(i * 2, 2), 16));
            }

            return (retKey, retIV);
        }
    }
}