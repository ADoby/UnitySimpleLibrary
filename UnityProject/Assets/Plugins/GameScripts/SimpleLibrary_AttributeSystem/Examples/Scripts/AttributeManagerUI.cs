using UnityEngine;
using System.Collections;

namespace SimpleLibrary
{
    //If you use the Singleton always keep in mind to override Awake when you wan't to use it
    public class AttributeManagerUI : Singleton<AttributeManagerUI>
    {
        public GameObject AttributePanelPrefab;
        public RectTransform AttributeParent;

        public Attribute attribute;

        void Start()
        {
            //Spawn an AttributeUI for each attribute
            //This is not perfect, but it works
            //You could even filter out "Filters" and make a tab for each or something
            GameObject currentAttribute = null;
            RectTransform attributePanel = null;
            AttributeUI attributeUI = null;
            int index = 0;
            float height = 50f;

            foreach (var attribute in AttributeManager.Instance.GetAttributes())
            {
                currentAttribute = GameObject.Instantiate(AttributePanelPrefab) as GameObject;
                attributeUI = currentAttribute.GetComponent<AttributeUI>();
                attributePanel = currentAttribute.GetComponent<RectTransform>();

                if (attributePanel)
                {
                    attributePanel.parent = AttributeParent;

                    attributePanel.localScale = new Vector3(1, 1, 1);

                    attributePanel.localPosition = new Vector3(0, -index * attributePanel.rect.height, 0);

                    height = attributePanel.rect.height;
                }
                if (attributeUI)
                {
                    attributeUI.AttrType.category = attribute.AttrType.category;
                    attributeUI.AttrType.type = AttributeManager.Instance.GetNameIndex(attribute.Name);
                }
                index++;
            }

            AttributeParent.sizeDelta = new Vector2(AttributeParent.sizeDelta.x, index * height);
        }
    }
}