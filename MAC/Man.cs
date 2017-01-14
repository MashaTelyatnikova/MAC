using System;
using System.Numerics;

namespace MAC
{
    public abstract class Man
    {
        private static readonly Random random = new Random(Guid.NewGuid().GetHashCode());
        protected const int g = 5;
        protected const int p = 23;
        public abstract Message ReceiveMessage(Message msg);
        protected int key = random.Next(10000, 999999999);
        public int GetKey()
        {
            return (int)BigInteger.ModPow(g, key, p);
        }

        public int GetKey(int k)
        {
            return (int)BigInteger.ModPow(g, k, p);
        }
    }
}
