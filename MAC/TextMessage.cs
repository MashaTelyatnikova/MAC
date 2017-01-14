namespace MAC
{
    public class TextMessage : Message
    {
        public byte[] Content { get; set; }

        public TextMessage(byte[] content)
        {
            Content = content;
        }
    }
}
