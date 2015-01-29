using UnityEngine;
using System.Collections;

namespace SimpleLibrary
{
    public class Singleton<ChildType> : MonoBehaviour
    {
        //Never use this directly, thats why its private NOT protected
        private static Singleton<ChildType> instance = null;

        //Use this to get the current single instance of this type
        public static Singleton<ChildType> Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType<Singleton<ChildType>>();
                if (instance == null)
                {
                    Debug.Log(string.Format("There was no object of type <{0}>", typeof(ChildType)));
                }
                return instance;
            }
            protected set
            {
                instance = value;
            }
        }
        protected virtual void Awake()
        {
            Instance = this;
        }
    }
}