using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using PeterO.Numbers;
using System.Security.Cryptography;


namespace crypt5
{
    class RSA
    {
        public EDecimal pubKey = 0;
        public EDecimal privKey = 0;       
        
        

        private EInteger q, p, n, phi = 0;
        private const int tokenSize = 8;
        private EContext ec;
        public RSA()
        {
            GenPrime gp = new GenPrime();
            //q = gp.Generate(1024);
            //p = gp.Generate(1024);
            q = 17; p = 19;
            n = q * p;
            phi = (q - 1) * (p - 1);
            RSACryptoServiceProvider rp = new RSACryptoServiceProvider();
            EInteger e = 2;
            while (e < phi)
            {
                if (GreatCommDiv(e, phi) == EInteger.One)
                    break;
                else
                    e++;
            }

            pubKey = EDecimal.FromEInteger(e);
            int k = 2;
            EDecimal a = (EDecimal.FromEInteger((phi * k - 1)));
            EDecimal b = EDecimal.FromEInteger(e);
            ec = new EContext(1100, ERounding.HalfDown, 0, 1100, false);
            privKey = (EDecimal.FromEInteger((phi + 1))).Divide(EDecimal.FromEInteger(e), ec);
            
        }

        private EInteger GreatCommDiv(EInteger a, EInteger b)
        {
            EInteger t;
            while (true)
            {
                t = a % b;
                if (t == EInteger.Zero)
                    return b;
                a = b;
                b = t;
            }
        }

        // ---------- ENCRYPTION ---------- //
        public string Encrypt(string message, EDecimal publicKey)
        {
            EInteger pbk = 
            Debug.WriteLine(BigInteger.ModPow(10, 2, 110).ToString());
            BigInteger n1 = 2;
            BigInteger enc = BigInteger.ModPow(n1, pbk, n);
            Debug.WriteLine(enc.ToString());
            BigInteger dec = BigInteger.ModPow(enc, prk, n);
            Debug.WriteLine(dec.ToString());
            Debug.WriteLine((pubKey * privKey % phi).ToString());


            string encryptedText = "";
            byte[] msgbytes = Encoding.UTF8.GetBytes(message);
            for (int i = 0; i < msgbytes.Length; i++)
            {
                //string token = getToken(message, i);
                //i += tokenSize - 1;

                EInteger numToken = EInteger.FromByte(msgbytes[i]);
                //for (int j = 0; j < token.Length; j++)
                //    numToken = EInteger.FromString(numToken.ToString() + (Convert.ToInt32(token[j])).ToString());

                EInteger value = ModPow(numToken, publicKey, n);
                encryptedText += value + " ";
            }
            /*            BigInteger numToken = 0;
                        for (int i = 0; i < message.Length; i++)
                        {
                            numToken = BigInteger.Parse(numToken.ToString() + (Convert.ToInt32(message[i])).ToString());
                        }
                        BigInteger value = BigInteger.ModPow(numToken, publicKey, n);
                        encryptedText += value.ToString() + " ";*/
            return encryptedText;
        }

        private string getToken(string text, int index, int length = tokenSize) 
        {
            string token = "";
            for (int i = 0; i < length; i++)
            {
                if (text.Length > index + i)
                    token += text[index + i];
                else
                    break;
            }
            return token;
        }
        // -------------------- //

        // ---------- DECRYPTION ---------- //
        public string Decrypt(string message, EDecimal privateKey)
        {
            string decryptedText = "";
            string[] tokens = message.Split(' ');
            byte[] bytes = new byte[] { };
            for (int i = 0; i < tokens.Length-1; i++)
            {
                
                EInteger value = ModPow(EDecimal.FromString(tokens[i]), privateKey, n);
                bytes.Append(value.ToByteChecked());
            }
            decryptedText = Encoding.UTF8.GetString(bytes);
            return decryptedText;
        }
        // -------------------- //

        private EInteger ModPow(EDecimal value, EDecimal exponent, EInteger modulus)
        {
            EInteger result = (value.Pow(exponent, ec)).ToEInteger().Mod(modulus);
            EDecimal res1 = (value.Pow(exponent, ec));
            EInteger res2 = res1.ToEInteger().Mod(modulus);
            return result;
        }
    }
}
