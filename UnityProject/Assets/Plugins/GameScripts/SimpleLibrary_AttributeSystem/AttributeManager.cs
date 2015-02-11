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
        public int CurrentFilterMask = 0;

        [SerializeField]
        public List<AttributeCategory> Categories = new List<AttributeCategory>();
        [SerializeField]
        public List<string> CategoryNames = new List<string>();

        [ContextMenu("Reset Categories")]
        public void ResetCategories()
        {
            Categories.Clear();
            CategoryNames.Clear();
        }

        public void UpdateCategoryNames()
        {
            CategoryNames.Clear();
            foreach (var attrCat in Categories)
            {
                CategoryNames.Add(attrCat.name);
            }
        }

        public void RenameCategory(int index, string name)
        {
            if (index < 0 || index >= Categories.Count)
                return;
            Categories[index].name = name;
            UpdateCategoryNames();
        }
        public void MoveCategory(int from, int to)
        {
            if (from < 0 || from > Categories.Count)
                return;
            if (to < 0 || to > Categories.Count)
                return;

            AttributeCategory tmp = Categories[to];
            Categories[to] = Categories[from];
            Categories[from] = tmp;
            UpdateCategoryNames();
        }

        public void AddCategory()
        {
            AttributeCategory category = new AttributeCategory();
            Categories.Add(category);
            UpdateCategoryNames();
        }
        public void RemoveCategory(int index)
        {
            if (index > 0 && index < Categories.Count)
                Categories.RemoveAt(index);
            UpdateCategoryNames();
        }
        public void RemoveType(int catIndex, int typeIndex)
        {
            if (catIndex < 0 || catIndex >= Categories.Count || typeIndex < 0)
                return;
            AttributeCategory category = Categories[catIndex];
            if (typeIndex >= category.Types.Count)
                return;
            category.Types.RemoveAt(typeIndex);
        }

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
                filterKey = CategoryNames[attribute.AttrType.category];
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
            if (!CategoryNames.Contains(filter))
                return -1;
            return CategoryNames.IndexOf(filter);
        }
        public string GetFilter(int index)
        {
            if (index < 0 || index >= CategoryNames.Count)
                return string.Empty;
            return CategoryNames[index];
        }
    }
}