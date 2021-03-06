﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace SimpleLibrary
{
    public class AttributeUI : MonoBehaviour
    {
        public Text mainText;
        public Button Reset, Maximize, Decrease, Increase;

        public AttributeInfo AttrType;

        public virtual void UpdateUI(Attribute attribute)
        {
            if (attribute == null)
                return;

            mainText.text = string.Format("{0}-{1}:{2}\n{3}/{4}", AttributeManager.Instance.GetCategoryName(attribute.AttrInfo.category), AttributeManager.Instance.GetAttributeType(attribute.AttrInfo.category, attribute.AttrInfo.type), attribute.Name, attribute.Points, attribute.MaxPoints);

            Maximize.interactable = !attribute.IsFull;
            Decrease.interactable = !attribute.IsEmpty;
            Increase.interactable = !attribute.IsFull;
            Reset.interactable = !attribute.IsReset;
        }

        void Start()
        {
            UpdateUI(AttributeManager.Instance.GetAttribute(AttrType.category, AttrType.type));
        }

        public void DecreasePoints()
        {
            AttributeManager.Instance.GetAttribute(AttrType.category, AttrType.type).RemovePoint();
            UpdateUI(AttributeManager.Instance.GetAttribute(AttrType.category, AttrType.type));
        }
        public void IncreasePoints()
        {
            AttributeManager.Instance.GetAttribute(AttrType.category, AttrType.type).AddPoint();
            UpdateUI(AttributeManager.Instance.GetAttribute(AttrType.category, AttrType.type));
        }
        public void MaximizePoints()
        {
            AttributeManager.Instance.GetAttribute(AttrType.category, AttrType.type).MaximizePoints();
            UpdateUI(AttributeManager.Instance.GetAttribute(AttrType.category, AttrType.type));
        }
        public void ResetPoints()
        {
            AttributeManager.Instance.GetAttribute(AttrType.category, AttrType.type).ResetPoints();
            UpdateUI(AttributeManager.Instance.GetAttribute(AttrType.category, AttrType.type));
        }
    }
}