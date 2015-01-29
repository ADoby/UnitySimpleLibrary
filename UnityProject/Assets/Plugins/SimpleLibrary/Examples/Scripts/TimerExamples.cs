using UnityEngine;
using System.Collections;
using SimpleLibrary;

public class TimerExamples : MonoBehaviour 
{
    public Timer ConstTimer;

	void Start ()
    {
        ConstTimer.Start();
	}
	
	void Update () 
    {
        ConstTimer.UpdateAutoReset();
	}
}
