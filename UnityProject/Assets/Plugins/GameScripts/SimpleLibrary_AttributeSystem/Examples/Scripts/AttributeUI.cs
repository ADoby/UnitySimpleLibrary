using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace SimpleLibrary
{
    public class AttributeUI : MonoBehaviour
    {
        public Text mainText;
        public Button Reset, Maximize, Decrease, Increase;

        public FilterAttribute AttributeFilter;
        public NameAttribute AttributeName;

        public virtual void UpdateUI(Attribute attribute)
        {
            if (attribute == null)
                return;

            mainText.text = string.Format("{0}:{1}\n{2}/{3}", AttributeManager.Instance.GetFilter(attribute.SelectedFilter), attribute.Name, attribute.Points, attribute.MaxPoints);

            Maximize.interactable = !attribute.IsFull;
            Decrease.interactable = !attribute.IsEmpty;
            Increase.interactable = !attribute.IsFull;
            Reset.interactable = !attribute.IsEmpty;
        }

        void Start()
        {
            UpdateUI(AttributeManager.Instance.GetAttribute(AttributeFilter.Value, AttributeName.Value));
        }

        public void DecreasePoints()
        {
            AttributeManager.Instance.GetAttribute(AttributeFilter.Value, AttributeName.Value).RemovePoint();
            UpdateUI(AttributeManager.Instance.GetAttribute(AttributeFilter.Value, AttributeName.Value));
        }
        public void IncreasePoints()
        {
            AttributeManager.Instance.GetAttribute(AttributeFilter.Value, AttributeName.Value).AddPoint();
            UpdateUI(AttributeManager.Instance.GetAttribute(AttributeFilter.Value, AttributeName.Value));
        }
        public void MaximizePoints()
        {
            AttributeManager.Instance.GetAttribute(AttributeFilter.Value, AttributeName.Value).MaximizePoints();
            UpdateUI(AttributeManager.Instance.GetAttribute(AttributeFilter.Value, AttributeName.Value));
        }
        public void ResetPoints()
        {
            AttributeManager.Instance.GetAttribute(AttributeFilter.Value, AttributeName.Value).ResetPoints();
            UpdateUI(AttributeManager.Instance.GetAttribute(AttributeFilter.Value, AttributeName.Value));
        }
    }
}