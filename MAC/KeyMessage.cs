namespace MAC
{
    public class KeyMessage : Message
    {
        public int Key { get; set; }

        public KeyMessage(int key)
        {
            Key = key;
        }
    }
}