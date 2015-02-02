using UnityEngine;
using System.Collections;

namespace SimpleLibrary
{
    public class AttributeUI : MonoBehaviour
    {
        #region Singleton
        private static AttributeUI instance;
        public static AttributeUI Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType<AttributeUI>();
                return instance;
            }
        }
        protected void Awake()
        {
            Init();
            instance = this;
        }
        #endregion

        public GameObject AttributePanelPrefab;

        //Singleton Awake
        void Init()
        {

        }

        void Start()
        {

        }

        void Update()
        {

        }
    }
}