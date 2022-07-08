
using System;

namespace Supports2EA { 

	public class CharacterPairs: Object
	{

		public string char1;
		public string char2;
		public CharacterPairs(string c1, string c2)
		{
			char1 = c1;
			char2 = c2;
		}

		public override bool Equals(Object obj)
		{
			CharacterPairs that = obj as CharacterPairs;
			bool retVal = false;
			
			if (this != null &&  that != null) retVal = (this.char1 == that.char1 && this.char2 == that.char2);			
			return retVal;
		}
	}
}