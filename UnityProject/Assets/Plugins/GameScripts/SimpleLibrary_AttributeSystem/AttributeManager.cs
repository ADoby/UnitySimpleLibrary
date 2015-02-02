using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SimpleLibrary
{
    public class AttributeManager : MonoBehaviour
    {
        #region Singleton
        private static AttributeManager instance;
        public static AttributeManager Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType<AttributeManager>();
                return instance;
            }
        }
        protected void Awake()
        {
            Init();
            instance = this;
        }
        #endregion

        [SerializeField]
        public List<Attribute> Attributes = new List<Attribute>();

        //Used for performance and convenience reasons :P
        private Dictionary<string, Attribute> AttributeDictionary = new Dictionary<string, Attribute>();

        #region UI
        public enum UIFilterMethods
        {
            SHOW_ALL,
            ONLY_THESE,
            ALL_EXCEPT_THESE
        }
        public UIFilterMethods SelectedFilterMode = UIFilterMethods.SHOW_ALL;
        public string Filter = string.Empty;

        [SerializeField]
        public List<string> AttributeNames = new List<string>();

        int selectedID = -1;

        public bool Attributes_Debug = false;
        public bool Attributes_FoldOut = false;
        public bool NameList_FoldOut = false;
        #endregion

        //Singleton Awake
        void Init()
        {
            foreach (var attribute in Attributes)
            {
                AttributeDictionary.Add(attribute.Name, attribute);
            }
        }

        public virtual void Start()
        {
            Reset();
        }

        public virtual void Reset()
        {
            ResetAttributes();
        }

        public virtual void ResetAttributes()
        {
            foreach (var item in Attributes)
            {
                item.Reset();
            }
        }

        public void AddAttribute(Attribute attribute)
        {
            if (Application.isPlaying || attribute == null || Attributes.Contains(attribute))
                return;
            if (AttributeNames.Contains(attribute.Name))
                return;
            Attributes.Add(attribute);
            AttributeNames.Add(attribute.Name);
        }
        public void RemoveAttribute(Attribute attribute)
        {
            if (Application.isPlaying || attribute == null || !Attributes.Contains(attribute))
                return;
            Attributes.Remove(attribute);
            if (AttributeNames.Contains(attribute.Name))
                AttributeNames.Remove(attribute.Name);
        }
        public void RemoveAttributeByName(string name)
        {
            RemoveAttribute(GetAttributeByName(name));
        }
        public void UpdateAttributeNamesList()
        {
            AttributeNames.Clear();
            foreach (var item in Attributes)
            {
                AttributeNames.Add(item.Name);
            }
        }

        public Attribute GetAttributeByName(string name)
        {
            foreach (var item in Attributes)
            {
                if (string.Equals(item.Name, name, System.StringComparison.OrdinalIgnoreCase))
                    return item;
            }
            return null;
        }
    }
}