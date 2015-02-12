using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SimpleLibrary
{
    

    public class AttributeManager : Singleton<AttributeManager>
    {
        //Used for performance and convenience reasons :P
		//Only filled during play (after awake)
        private Dictionary<string, Dictionary<string, Attribute>> AttributeDictionary = new Dictionary<string, Dictionary<string, Attribute>>();


		//Singleton Awake
		protected override void Awake()
        {
            base.Awake();
            UpdateAttributeList();
        }

        public virtual void Start()
        {
            Reset();
        }

        public virtual void Reset()
        {
            ResetAttributes();
        }

        #region Initialization
        /// <summary>
        /// You need to call this after you add or remove one or more attributes
        /// Or they won't be available using GetAttribute
        /// </summary>
        public virtual void UpdateAttributeList()
        {
            AttributeDictionary.Clear();
            string category = string.Empty;
            string type = string.Empty;
            AttributeCategory attrCategory = null;
            foreach (var attribute in Attributes)
            {
                attrCategory = Categories[attribute.AttrInfo.category];

                category = CategoryNames[attribute.AttrInfo.category];
                type = attrCategory.Types[attribute.AttrInfo.type];

                if (!AttributeDictionary.ContainsKey(category))
                {
                    AttributeDictionary.Add(category, new Dictionary<string, Attribute>());
                }
                if (AttributeDictionary[category].ContainsKey(type))
                {
                    Debug.LogWarning(string.Format("Attribute Manager: You have a duplicate Attribute: \"{0}\" in Category:\"{1}\" of Type:\"{2}\"", attribute.Name, category, type));
                }
                else
                {
                    AttributeDictionary[category].Add(type, attribute);
                }
            }
        }
        #endregion

		#region AttributeCategory
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

			//Test if any attribute uses this category, if so reset the category
			foreach (var attribute in Attributes)
			{
				if (attribute.AttrInfo.category == index)
					attribute.AttrInfo.category = 0;
			}
		}
		public void RenameCategory(int index, string newName)
		{
			if (index < 0 || index >= Categories.Count)
				return;
			Categories[index].name = newName;
			UpdateCategoryNames();
		}
		public void MoveCategory(int from, int to)
		{
			//Check out of bounds, just because we can
			if (from < 0 || from > Categories.Count)
				return;
			if (to < 0 || to > Categories.Count)
				return;

			//Actually move the category
			AttributeCategory tmp = Categories[to];
			Categories[to] = Categories[from];
			Categories[from] = tmp;

			//CategoryName list needs to be updated
			UpdateCategoryNames();

			//All attributes using this category should stay in it
			foreach (var attribute in Attributes)
			{
				if (attribute.AttrInfo.category == from)
					attribute.AttrInfo.category = to;
			}
		}

		public int GetCategoryIndex(string name)
		{
			if (!CategoryNames.Contains(name))
				return -1;
			return CategoryNames.IndexOf(name);
		}
		public string GetCategoryName(int index)
		{
			if (index < 0 || index >= CategoryNames.Count)
				return string.Empty;
			return CategoryNames[index];
		}
		public AttributeCategory GetCategory(int index)
		{
			if (index < 0 || index >= Categories.Count)
				return null;
			return Categories[index];
		}
		#endregion

		#region AttributeType
		public void AddType(int categoryIndex, string name)
		{
			if (categoryIndex < 0 || categoryIndex > Categories.Count)
				return;
			AttributeCategory category = Categories[categoryIndex];
			category.Types.Add(name);
		}
		public void RemoveType(int categoryIndex, int typeIndex)
		{
			if (categoryIndex < 0 || categoryIndex >= Categories.Count || typeIndex < 0)
				return;
			AttributeCategory category = Categories[categoryIndex];
			if (typeIndex >= category.Types.Count)
				return;
			category.Types.RemoveAt(typeIndex);

			//Test if any attribute uses this type, if so reset the type
			foreach (var attribute in Attributes)
			{
				if (attribute.AttrInfo.type == typeIndex)
					attribute.AttrInfo.type = 0;
			}
		}
		public void RenameType(int categoryIndex, int typeIndex, string newName)
		{
			if (categoryIndex < 0 || categoryIndex > Categories.Count)
				return;
			AttributeCategory category = Categories[categoryIndex];

			if (typeIndex < 0 || typeIndex > category.Types.Count)
				return;

			category.Types[typeIndex] = newName;
		}
		public void MoveType(int categoryIndex, int from, int to)
		{
			if (categoryIndex < 0 || categoryIndex > Categories.Count)
				return;
			AttributeCategory category = Categories[categoryIndex];

			if (from < 0 || from > category.Types.Count)
				return;
			if (to < 0 || to > category.Types.Count)
				return;

			string tmp = category.Types[to];
			category.Types[to] = category.Types[from];
			category.Types[from] = tmp;

			foreach (var attribute in Attributes)
			{
				if (attribute.AttrInfo.type == from)
					attribute.AttrInfo.type = to;
			}
		}

		public string GetAttributeType(int categoryIndex, int typeIndex)
		{
			AttributeCategory cat = GetCategory(categoryIndex);
			if (cat == null)
				return string.Empty;
			if (typeIndex < 0 || typeIndex >= cat.Types.Count)
				return string.Empty;
			return cat.Types[typeIndex];
		}
		#endregion

        #region Attribute
		[SerializeField]
		private List<Attribute> Attributes = new List<Attribute>();

		/// <summary>
		/// Should not be used during play, use overload with category and type instead
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public Attribute GetAttributeByIndex(int index)
		{
			if (index < 0 || index >= Attributes.Count)
				return null;
			return Attributes[index];
		}
		public void MoveAttribute(int from, int to)
		{
			if (from < 0 || from >= Attributes.Count)
				return;
			if (to < 0 || to >= Attributes.Count)
				return;
			Attribute temp = Attributes[to];
			Attributes[to] = Attributes[from];
			Attributes[from] = temp;
		}
		public int AttributeCount
		{
			get
			{
				return Attributes.Count;
			}
		}
		public List<Attribute> GetAttributes
		{
			get
			{
				return Attributes;
			}
		}

        /// <summary>
        /// Resets all attributes to 0 points
        /// </summary>
        public virtual void ResetAttributes()
        {
            foreach (var attribute in Attributes)
            {
                attribute.Reset();
            }
        }

        /// <summary>
        /// Adds an attribute
        /// </summary>
        /// <param name="attribute"></param>
        public void AddAttribute(Attribute attribute)
        {
            if (Application.isPlaying || attribute == null || Attributes.Contains(attribute))
                return;
            Attributes.Add(attribute);

            if(Application.isPlaying)
                UpdateAttributeList();
        }

        /// <summary>
        /// Removes an attribute
        /// </summary>
        /// <param name="attribute"></param>
        public void RemoveAttribute(Attribute attribute)
        {
            if (Application.isPlaying || attribute == null || !Attributes.Contains(attribute))
                return;
            Attributes.Remove(attribute);

            if (Application.isPlaying)
                UpdateAttributeList();
        }

		/// <summary>
		/// Remove a attribute using category and type
		/// </summary>
		/// <param name="category"></param>
		/// <param name="type"></param>
        public void RemoveAttribute(string category, string type)
        {
            RemoveAttribute(GetAttribute(category, type));
        }

		/// <summary>
		/// Gets an attribute using attributeType parameter
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public Attribute GetAttribute(AttributeInfo type)
		{
			return GetAttribute(type.category, type.type);
		}
		/// <summary>
		/// Can throw NullPointerException !
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public float GetAttributeValue(AttributeInfo type)
		{
			return GetAttribute(type).Value;
		}

		/// <summary>
		/// Gets an attribute using category and type NAME
		/// </summary>
		/// <param name="category"></param>
		/// <param name="type"></param>
		/// <returns></returns>
        public Attribute GetAttribute(string category, string type)
        {
            if (string.IsNullOrEmpty(category) || string.IsNullOrEmpty(type))
                return null;
            Attribute value = null;
            if (AttributeDictionary.ContainsKey(category))
            {
                AttributeDictionary[category].TryGetValue(type, out value);
            }
            return value;
        }
		/// <summary>
		/// Can throw NullPointerException !
		/// </summary>
		/// <param name="category"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public float GetAttributeValue(string category, string type)
		{
			return GetAttribute(category, type).Value;
		}

		/// <summary>
		/// Gets an attribute using category and type INDEX
		/// </summary>
		/// <param name="categoryIndex"></param>
		/// <param name="typeIndex"></param>
		/// <returns></returns>
        public Attribute GetAttribute(int categoryIndex, int typeIndex)
        {
            return GetAttribute(GetCategoryName(categoryIndex), GetAttributeType(categoryIndex, typeIndex));
        }
		/// <summary>
		/// Can throw NullPointerException !
		/// </summary>
		/// <param name="categoryIndex"></param>
		/// <param name="typeIndex"></param>
		/// <returns></returns>
		public float GetAttributeValue(int categoryIndex, int typeIndex)
		{
			return GetAttribute(categoryIndex, typeIndex).Value;
		}
        #endregion


		#region EDITOR
		//This one saves the current tab (Categories/Attributes)
		public int EditorUIState = 0;
		//If Debug mode should be used (Some more buttons to edit attribute points on the fly)
		public bool Attributes_Debug = false;
		//Category Filter
		public int CurrentFilterMask = 0;
		#endregion


		#region Static
		public static Attribute IGetAttribute(AttributeInfo type)
		{
			return Instance.GetAttribute(type);
		}
		public static float IGetAttributeValue(AttributeInfo type)
		{
			return Instance.GetAttributeValue(type);
		}

		public static Attribute IGetAttribute(string category, string type)
		{
			return Instance.GetAttribute(category, type);
		}
		public static float IGetAttributeValue(string category, string type)
		{
			return Instance.GetAttributeValue(category, type);
		}

		public static Attribute IGetAttribute(int categoryIndex, int typeIndex)
		{
			return Instance.GetAttribute(categoryIndex, typeIndex);
		}
		public static float IGetAttributeValue(int categoryIndex, int typeIndex)
		{
			return Instance.GetAttributeValue(categoryIndex, typeIndex);
		}
		#endregion
	}
}