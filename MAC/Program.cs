using System;

namespace MAC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            int mode = 0;
            if (args.Length < 1 || !int.TryParse(args[0], out mode) || (mode != 0 && mode != 1))
            {
                Console.WriteLine("Parameter is mode (0 or 1)");
                Environment.Exit(0);
            }
           
            /*
             * 0 - подмена ключей и просто передача без подмены, не сможем расшифровать своое изначальное
             * 1 - ключи поменяет, но меняет просто сообщения, обнаружим раньше, не будет мака
             * */

            var alice = new OrdinaryMan("1234567890", "Bob, you need to go to Japan");
            var bob = new OrdinaryMan("1234567890", "Alice, you need to go to Munich");
            var eve = new ManInTheMiddle(mode, "Bob, you need to go to Zurich", "Alice, you need to go to Moscow");

            var channel = new Channel(alice, eve, bob);
            channel.SendMessage(new KeyMessage(alice.GetKey()), Direction.Right);
            channel.SendMessage(new KeyMessage(bob.GetKey()), Direction.Left);

            var aliceMsg = alice.GetEncryptedMessage();
            var bobMsg = bob.GetEncryptedMessage();

            try
            {
                channel.SendMessage(new TextMessage(aliceMsg.ToMD5()), Direction.Right);
                channel.SendMessage(new TextMessage(bobMsg.ToMD5()), Direction.Left);
                channel.SendMessage(new TextMessage(aliceMsg), Direction.Right);
                channel.SendMessage(new TextMessage(bobMsg), Direction.Left);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}