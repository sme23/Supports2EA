

namespace Supports2EA
{
    public class Character
    {
        public string name;
        public byte index;
        public string[] supportPartners = new string[7];
        public byte[] initialValues = new byte[7];
        public byte[] growthRates = new byte[7];
        public byte numPartners = 0;
        
        public void init(string n, byte id) 
        {
            this.index = id;
            this.name = n;
        }

    }
}

