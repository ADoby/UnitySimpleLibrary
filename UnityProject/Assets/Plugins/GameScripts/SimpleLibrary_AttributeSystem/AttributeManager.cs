using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SimpleLibrary
{
    [System.Serializable]
    public class FilterAttribute
    {
        public int Value = 0;
    }
    [System.Serializable]
    public class NameAttribute
    {
        public int Value = 0;
    }

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
            set
            {
                instance = value;
            }
        }
        protected void Awake()
        {
            Init();
            Instance = this;
        }
        #endregion


        [SerializeField]
        private List<Attribute> Attributes = new List<Attribute>();
        public Attribute GetAttribute(int index)
        {
            if (index < 0 || index >= Attributes.Count)
                return null;
            return Attributes[index];
        }
        public void Switch(int index1, int index2)
        {
            Attribute temp = Attributes[index2];
            Attributes[index2] = Attributes[index1];
            Attributes[index1] = temp;
        }
        public int AttributeCount
        {
            get
            {
                return Attributes.Count;
            }
        }
        public List<Attribute> GetAttributes()
        {
            return Attributes;
        }

        //Used for performance and convenience reasons :P
        private Dictionary<string, Dictionary<string, Attribute>> AttributeDictionary = new Dictionary<string, Dictionary<string, Attribute>>();

        #region UI
        public bool Filters_FoldOut = false;
        [SerializeField]
        public List<string> Filters = new List<string>();
        public int CurrentFilterMask = 0;

        [SerializeField]
        public List<string> AttributeNames = new List<string>();

        public bool Attributes_Debug = false;
        public bool Attributes_FoldOut = false;
        public bool NameList_FoldOut = false;
        #endregion

        //Singleton Awake
        void Init()
        {
            string filterKey = string.Empty;
            foreach (var attribute in Attributes)
            {
                filterKey = Filters[attribute.SelectedFilter];
                if (!AttributeDictionary.ContainsKey(filterKey))
                {
                    AttributeDictionary.Add(filterKey, new Dictionary<string, Attribute>());
                }
                if (AttributeDictionary[filterKey].ContainsKey(attribute.Name))
                    Debug.LogWarning(string.Format("You have a duplicate Attribute: Filter:{0} Name:{1}", filterKey, attribute.Name));
                else
                    AttributeDictionary[filterKey].Add(attribute.Name, attribute);
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
        public void RemoveAttribute(string filter, string name)
        {
            RemoveAttribute(GetAttribute(filter, name));
        }
        public void UpdateAttributeNamesList()
        {
            AttributeNames.Clear();
            foreach (var item in Attributes)
            {
                if(!AttributeNames.Contains(item.Name))
                    AttributeNames.Add(item.Name);
            }
        }

        public Attribute GetAttribute(string filter, string name)
        {
            if (string.IsNullOrEmpty(filter) || string.IsNullOrEmpty(name))
                return null;
            Attribute value = null;
            if (AttributeDictionary.ContainsKey(filter))
            {
                AttributeDictionary[filter].TryGetValue(name, out value);
            }
            return value;
        }

        public Attribute GetAttribute(int indexFilter, int indexName)
        {
            return GetAttribute(GetFilter(indexFilter), GetName(indexName));
        }

        public int GetNameIndex(string name)
        {
            if (!AttributeNames.Contains(name))
                return -1;
            return AttributeNames.IndexOf(name);
        }
        public string GetName(int index)
        {
            if (index < 0 || index >= AttributeNames.Count)
                return string.Empty;
            return AttributeNames[index];
        }

        public int GetFilterIndex(string filter)
        {
            if (!Filters.Contains(filter))
                return -1;
            return Filters.IndexOf(filter);
        }
        public string GetFilter(int index)
        {
            if (index < 0 || index >= Filters.Count)
                return string.Empty;
            return Filters[index];
        }
    }
}