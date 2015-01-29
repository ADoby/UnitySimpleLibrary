using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace SimpleLibrary
{
    [System.Serializable]
    public class ConditionInfo
    {
        public WinCondition condition;
        public ConditionUI conditionUI;

        public void UpdateUI()
        {
            if (conditionUI && conditionUI.conditionText && condition != null)
                conditionUI.conditionText.Text = condition.GetText();
        }
    }

    public class Game : Singleton<Game>
    {

        public Timer UpdateConditionUITimer;

        public List<ConditionInfo> ConditionInfos = new List<ConditionInfo>();

        public List<ConditionInfo> RemovingInfos = new List<ConditionInfo>();

        public void AddCondition(WinCondition condition)
        {
            ConditionInfo info = new ConditionInfo();
            info.condition = condition;
            //TODO create conditionUIobject somewhere
            //info.conditionUI = GameUI.Instance.AddUICondition();
            info.UpdateUI();
            ConditionInfos.Add(info);

            info.conditionUI.SetPositionIndex(ConditionInfos.Count);
        }

        public bool ContainsCondition(WinCondition condition)
        {
            foreach (var con in ConditionInfos)
            {
                if (con.condition == condition)
                    return true;
            }
            return false;
        }
        public void RemoveConditionInfo(ConditionInfo info)
        {
            RemovingInfos.Remove(info);

            int index = 0;
            foreach (var item in ConditionInfos)
            {
                item.conditionUI.SetPositionIndex(index);
                index++;
            }
        }
        public void StartRemovingConditionInfo(ConditionInfo info)
        {
            if (RemovingInfos.Contains(info))
                return;
            ConditionInfos.Remove(info);
            RemovingInfos.Add(info);
            info.conditionUI.SetPositionIndex(-5);
        }
        public void RemoveCondition(WinCondition condition)
        {
            foreach (var item in ConditionInfos.ToArray())
            {
                if (item.condition == condition)
                    StartRemovingConditionInfo(item);
            }
        }
        public void ClearConditions()
        {
            foreach (var item in ConditionInfos)
            {
                StartRemovingConditionInfo(item);
            }
        }


        #region Events
        protected void OnEnable()
        {

        }
        protected void OnDisable()
        {

        }
        #endregion

        public void GameReset()
        {
            //TODO remove or reset conditions
        }

        public void UpdateConditionsUI()
        {
            foreach (var item in ConditionInfos)
            {
                item.UpdateUI();
            }
            foreach (var item in RemovingInfos.ToArray())
            {
                if (item.conditionUI.CurrentLerpValue >= 0.95f)
                    RemoveConditionInfo(item);
            }
        }

        public void Update()
        {
            if (UpdateConditionUITimer.Update())
            {
                UpdateConditionUITimer.Reset();
                UpdateConditionsUI();
            }
        }

        public static void RepairVector3(ref Vector3 testVec)
        {
            if (float.IsNaN(testVec.x))
                testVec.x = 0;
            if (float.IsNaN(testVec.y))
                testVec.y = 0;
            if (float.IsNaN(testVec.z))
                testVec.z = 0;
        }
        public static Vector3 RepairVector3(Vector3 testVec)
        {
            if (float.IsNaN(testVec.x))
                testVec.x = 0;
            if (float.IsNaN(testVec.y))
                testVec.y = 0;
            if (float.IsNaN(testVec.z))
                testVec.z = 0;
            return testVec;
        }
    }
}
