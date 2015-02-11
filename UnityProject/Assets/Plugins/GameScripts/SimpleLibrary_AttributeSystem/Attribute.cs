using System;
using System.Collections.Generic;

namespace SimpleLibrary
{
    #region ValueClass
    [System.Serializable]
    public class AttributeValue
    {
        public float Value = 0f;

        public float StartValue = 0f;
        public float ValuePerPoint = 1f;
        public float ValuePerPointMultipliedByCurrentPoints = 0f;

        public virtual void Reset()
        {
            Value = StartValue;
        }
        public virtual void Recalculate(int points)
        {
            Reset();
            for (int i = 1; i <= points; i++)
            {
                IncreaseValue(i);
            }
        }
        public virtual void IncreaseValue(int points)
        {
            Value += ValuePerPoint + points * ValuePerPointMultipliedByCurrentPoints;
        }
        public virtual void DecreaseValue(int points)
        {
            Value -= ValuePerPoint + (points - 1) * ValuePerPointMultipliedByCurrentPoints;
        }
    }
    #endregion

    [System.Serializable]
    public class AttributeType
    {
        public int category = 0;
        public int type = 0;
    }

    [System.Serializable]
    public class AttributeCategory
    {
        public string name = "";
        public List<string> Types = new List<string>();

        //EDITOR
        public bool foldOut = false;
    }

    [System.Serializable]
    public class Attribute
    {
        public string Name = "";

        public AttributeType AttrType;

        public bool Enabled = true;

        //Can't be changed from outside
        public bool Locked = false;

        public virtual void Reset()
        {
            if (!Enabled || Locked)
                return;
            ResetPoints();
        }

        #region POINTS
        public int Points = 0;
        public int StartPoints = 0;
        public int MaxPoints = 100;

        public bool IsFull
        {
            get
            {
                return Points == MaxPoints;
            }
        }
        public bool IsEmpty
        {
            get
            {
                return Points == StartPoints;
            }
        }

        public virtual bool ResetPoints()
        {
            if (!Enabled || Locked || Points == StartPoints)
                return false;
            Points = StartPoints;
            RecalculateValue();
            UpdatePoints();
            return true;
        }

        public virtual bool AddPoint()
        {
            if (!Enabled || Locked || Points >= MaxPoints)
                return false;
            Points++;
            RecalculateValue();
            UpdatePoints();
            return true;
        }
        public virtual bool RemovePoint()
        {
            if (!Enabled || Locked || Points <= StartPoints)
                return false;
            Points--;
            RecalculateValue();
            UpdatePoints();
            return true;
        }
        public virtual bool MaximizePoints()
        {
            if (!Enabled || Locked)
                return false;
            Points = MaxPoints;
            RecalculateValue();
            UpdatePoints();
            return true;
        }
        protected virtual void UpdatePoints()
        {
            TriggerAttributeChanged();
        }
        #endregion

        #region VALUE
        public AttributeValue ValueInfo;

        public float Value
        {
            get
            {
                return ValueInfo.Value;
            }
            set
            {
                ValueInfo.Value = value;
            }
        }

        private void IncreaseValue()
        {
            ValueInfo.IncreaseValue(Points);
        }
        private void DecreaseValue()
        {
            ValueInfo.DecreaseValue(Points);
        }
        private void ResetValue()
        {
            ValueInfo.Reset();
        }
        public void RecalculateValue()
        {
            ValueInfo.Recalculate(Points);
        }

        #endregion

        #region EVENTS
        public delegate void Event(Attribute info);
        public static Event AttributeChanged;
        public Event ThisAttributeChanged;

        public void TriggerAttributeChanged()
        {
            if (AttributeChanged != null)
                AttributeChanged(this);
            if (ThisAttributeChanged != null)
                ThisAttributeChanged(this);
        }
        #endregion

        #region EDITOR_PROPERTIES
        public bool FoldOut = false;
        public bool Point_FoldOut = false;
        public bool Value_FoldOut = false;
        #endregion

        public override string ToString()
        {
            return string.Format("AttribName:{0} Points:{1} Value:{2}", Name, Points, Value);
        }

    }
}

