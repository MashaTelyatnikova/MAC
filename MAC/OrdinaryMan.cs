using System;
using System.Numerics;
using System.Text;

namespace MAC
{
    public class OrdinaryMan : Man
    {
        private readonly string mac;
        private readonly string message;
        private int opponentKey;
        private int secretKey;
        private byte[] firstHalf;
        private byte[] secondHalf;
        public OrdinaryMan(string mac, string message)
        {
            this.mac = mac;
            this.message = message;
        }
        public override Message ReceiveMessage(Message msg)
        {
            if (msg is KeyMessage)
            {
                opponentKey = ((KeyMessage) (msg)).Key;
                secretKey = (int)BigInteger.ModPow(opponentKey, key, p);
                return new KeyMessage(key);
            }

            var textMessage = (TextMessage) msg;
            if (firstHalf == null)
            {
                firstHalf = textMessage.Content;
            }else if (secondHalf == null)
            {
                secondHalf = textMessage.Content;
                if (!firstHalf.ToString().Equals(secondHalf.ToMD5().ToString()))
                {
                    throw new Exception("MAN IN THE MIDDLE. Parts are not match");
                }

                string result = null;
                try
                {
                    result = AESHelper.Decrypt(secondHalf, $"{secretKey}");
                }
                catch (Exception ex)
                {
                    throw new Exception("MAN IN THE MIDDLE. Can't decrypt message");
                }

                Console.WriteLine($"Message is {result}");
                if (result.Length < mac.Length + 1)
                {
                    throw new Exception("MAN IN THE MIDDLE. MAC doesn't exists");
                }

                var macPart = result.Substring(result.Length - 1 - mac.Length);
                if (!macPart.Equals($".{mac}"))
                {
                    throw new Exception("MAN IN THE MIDDLE. MAC doesn't exists");
                }
            }
            return null;
        }

        public byte[] GetEncryptedMessage()
        {
            return AESHelper.Encrypt(Encoding.UTF8.GetBytes($"{message}.{mac}"), $"{secretKey}");
        }
    }
}
