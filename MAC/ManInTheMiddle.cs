using System;
using System.Numerics;
using System.Text;

namespace MAC
{
    public class ManInTheMiddle : Man
    {
        private int? keyA;
        private int? keyB;
        private int secretA;
        private int secretB;
        private int mode;
        private byte[] firstA;
        private byte[] firstB;
        private byte[] secondA;
        private byte[] secondB;
        private byte[] message1;
        private byte[] message2;
        public ManInTheMiddle(int mode, string message1, string message2)
        {
            this.mode = mode;
            this.message1 = Encoding.UTF8.GetBytes(message1);
            this.message2 = Encoding.UTF8.GetBytes(message2);
        }

        public override Message ReceiveMessage(Message msg)
        {
            if (msg is KeyMessage)
            {
                var receviedKey = ((KeyMessage) msg).Key;
                if (keyA == null)
                {

                    keyA = receviedKey;
                    secretA = (int) BigInteger.ModPow(keyA.Value, this.key, p);
                }
                else
                {
                    keyB = receviedKey;
                    secretB = (int) BigInteger.ModPow(keyB.Value, this.key, p);
                }

                return new KeyMessage(GetKey());
            }

            var textMessage = (TextMessage) msg;
            if (firstA == null)
            {
                firstA = textMessage.Content;
                if (mode == 1)
                {
                    textMessage.Content = AESHelper.Encrypt(message1, $"{secretB}").ToMD5();
                }
            }

            else if (firstB == null)
            {
                firstB = textMessage.Content;
                if (mode == 1)
                {
                    textMessage.Content = AESHelper.Encrypt(message2, $"{secretA}").ToMD5();
                }
            }

            else if (secondA == null)
            {
                secondA = textMessage.Content;
                var decrypted = AESHelper.Decrypt(secondA, $"{secretA}");
                Console.WriteLine($"Eve got message from A = {decrypted}");

                if (mode == 1)
                {
                    textMessage.Content = AESHelper.Encrypt(message1, $"{secretB}");
                }
            }

            else if (secondB == null)
            {
                secondB = textMessage.Content;
                var decrypted = AESHelper.Decrypt(secondA, $"{secretB}");
                Console.WriteLine($"Eve got message from B = {decrypted}");
                if (mode == 1)
                {
                    textMessage.Content = AESHelper.Encrypt(message2, $"{secretA}");
                }
            }
            
            return textMessage;
        }
    }
}