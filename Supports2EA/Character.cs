

namespace Supports2EA
{
    public class Character
    {
        byte index;
        byte[] supportPartners = new byte[6];
        byte[] initialValues = new byte[6];
        byte[] growthRates = new byte[6];
        byte numPartners = 0;
        
        public void init(byte id) 
        {
            this.index = id;
        }

    }
}

