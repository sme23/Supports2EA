

namespace Supports2EA
{
    public class Character
    {
        public string name;
        public byte index;
        public string[] supportPartners = new string[6];
        public byte[] initialValues = new byte[6];
        public byte[] growthRates = new byte[6];
        public byte numPartners = 0;
        
        public void init(string n, byte id) 
        {
            this.index = id;
            this.name = n;
        }

    }
}

