using System.Collections.Generic;
using System.Linq;

namespace MAC
{
    public enum Direction
    {
        Left,
        Right
    }

    public class Channel
    {
        private readonly List<Man> participants;

        public Channel(params Man[] participants)
        {
            this.participants = participants.ToList();
        }

        public virtual void SendMessage(Message msg, Direction direction)
        {
            switch (direction)
            {
                case Direction.Right:
                {
                    var current = msg;
                    for (var i = 1; i < participants.Count; ++i)
                    {
                        current = participants[i].ReceiveMessage(current);
                    }
                    break;
                }
                case Direction.Left:
                {
                    var current = msg;
                    for (var i = participants.Count - 2; i >= 0; --i)
                    {
                        current = participants[i].ReceiveMessage(current);
                    }
                    break;
                }
                default:
                {
                    break;
                }
            }
        }
    }
}