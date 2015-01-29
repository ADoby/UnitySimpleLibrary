using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SimpleLibrary
{
    public enum EnemyType
    {
        NONE
    }

    [System.Serializable]
    public class EnemyTypeWeightedSpawnInfo
    {
        public EnemyType enemyType;
        public float Weight = 0f;
    }

    [System.Serializable]
    public class EnemyTypeSortedSpawnInfo
    {
        public List<EnemyTypeWeightedSpawnInfo> weightedRandomSpawnInfo = new List<EnemyTypeWeightedSpawnInfo>();
        public int Amount = 0;
        public Timer SpawnTimer;
        public Timer WaitAfterThisSpawn;
        private int spawned = 0;
        public int Spawned
        {
            get
            {
                return spawned;
            }
            private set
            {
                spawned = value;
            }
        }

        public bool IsLastSpawn
        {
            get
            {
                return Amount >= 0 && spawned == Amount + 1;
            }
        }

        public void Next()
        {
            if (IsLastSpawn)
                return;
            Randomize();
            spawned++;
        }

        private float WeightFunc(EnemyTypeWeightedSpawnInfo value)
        {
            return value.Weight;
        }
        private void Randomize()
        {
            CurrentInfo = weightedRandomSpawnInfo.RandomEntry(WeightFunc);
        }

        private EnemyTypeWeightedSpawnInfo currentInfo;
        public EnemyTypeWeightedSpawnInfo CurrentInfo
        {
            get
            {
                return currentInfo;
            }
            private set
            {
                currentInfo = value;
            }
        }

        public void Reset()
        {
            SpawnTimer.Reset();
            WaitAfterThisSpawn.Reset();
            Spawned = 0;
        }
    }

    public class Arena_Spawner : MonoBehaviour
    {
        public Collider Bounds;
        public List<EnemyTypeSortedSpawnInfo> sortedSpawnInfo = new List<EnemyTypeSortedSpawnInfo>();
        private int CurrentIndex = -1;

        public EnemyTypeSortedSpawnInfo CurrentSpawnInfo
        {
            get
            {
                return sortedSpawnInfo[CurrentIndex];
            }
        }
        public void NextInfo()
        {
            CurrentIndex++;
            if (CurrentIndex >= sortedSpawnInfo.Count)
                CurrentIndex = -1;
        }
        public bool FinishedSpawning
        {
            get
            {
                return CurrentIndex < 0;
            }
        }


        void Awake()
        {

        }

        void OnEnable()
        {
            Listen();
        }
        void OnDisable()
        {
            UnListen();
        }

        public void Listen()
        {

        }
        public void UnListen()
        {

        }

        public void LevelPartStart()
        {
            foreach (var item in sortedSpawnInfo)
            {
                item.Reset();
            }

            EnableSpawning = true;
            CurrentIndex = 0;
        }

        public void LevelPartStop()
        {
            EnableSpawning = false;
        }

        public bool EnableSpawning = false;

        void Update()
        {
            if (!EnableSpawning || FinishedSpawning || sortedSpawnInfo.Count <= 0)
                return;

            EnemyTypeSortedSpawnInfo info = sortedSpawnInfo[CurrentIndex];
            if ((info.IsLastSpawn && info.WaitAfterThisSpawn.Update()))
            {
                NextInfo();
            }
            else if (info.SpawnTimer.Update())
            {
                info.SpawnTimer.Reset();
                if (info.CurrentInfo == null)
                {
                    info.Next();
                }
                if (info.CurrentInfo != null && Spawn(info.CurrentInfo.enemyType))
                {
                    info.Next();
                }
            }
        }

        public int TryCountPerSpawn = 20;
        public LayerMask CheckGroundMask;
        public float FloorCastLength = 10f;
        public LayerMask CheckSpawnPositionMask;
        public float OccluderCheckHeight = 1f;
        public float OccluderCheckRadius = 0.5f;

        public bool Spawn(EnemyType type)
        {
            Vector3 position = Vector3.zero;
            for (int tries = 0; tries < TryCountPerSpawn; tries++)
            {
                position.x = Random.Range(Bounds.bounds.min.x, Bounds.bounds.max.x);
                position.y = Random.Range(Bounds.bounds.min.y, Bounds.bounds.max.y);
                position.z = Random.Range(Bounds.bounds.min.z, Bounds.bounds.max.z);
                RaycastHit hit;
                if (Physics.Raycast(position, Vector3.down, out hit, FloorCastLength, CheckGroundMask))
                {
                    position = hit.point + Vector3.up * OccluderCheckHeight;
                    if (!Physics.CheckSphere(position, OccluderCheckRadius, CheckSpawnPositionMask))
                    {
                        //TODO Spawn Enemy
                        return true;
                    }
                    else
                    {
                        //Debug.Log(string.Format("NoSpace {0}", position));
                    }
                }
                else
                {
                    //Debug.Log(string.Format("NoFloor {0}", position));
                }
            }
            return false;
        }
    }
}