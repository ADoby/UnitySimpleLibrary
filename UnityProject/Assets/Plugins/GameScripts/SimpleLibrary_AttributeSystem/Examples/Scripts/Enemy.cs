using UnityEngine;
using System.Collections;

namespace SimpleLibrary
{
	public class Enemy : MonoBehaviour 
	{
        [Range(0, 1)]
		public int CurrentLevel = 0;

        public MyColor color;

		public AttributeInfo AttributeInfo;
		//public float AttributeInfoValue = 0f;

		//Second part is not needed, but shows a nice name in the editor, debug mode enables editing points in Editor
		//THESE attributes will NOT be available from AttributeManager
		public Attribute Health = new Attribute() { Name = "Health", DebugMode = true };
		public Attribute Damage = new Attribute() { Name = "Damage", DebugMode = true };

		void Update()
		{
			//AttributeInfoValue = AttributeInfo.Value;
		}

		public void Reset()
		{
			CurrentLevel = 0;
			Health.ResetPoints();
			Damage.ResetPoints();
		}

		public void LevelUp()
		{
			CurrentLevel++;
			//Increase Health every level
			Health.AddPoint();
			//Increase Damage every third level
			if(CurrentLevel % 3 == 0)
				Damage.AddPoint();
		}
	}
}
