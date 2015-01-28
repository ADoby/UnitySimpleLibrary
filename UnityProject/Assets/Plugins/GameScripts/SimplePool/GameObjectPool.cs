using System;
using UnityEngine;
using System.Collections.Generic;


public class GameObjectPool : MonoBehaviour
{

	private bool Deactivate = true;

	public Vector3 disabledPosition = new Vector3(0, -10000, 0);
		

	private static GameObjectPool instance = null;

	public static GameObjectPool Instance{
		get{
			if(instance == null)
				instance = new GameObjectPool();
				
			return instance;
		}
	}

    void Awake()
    {
        instance = this;
    }

	//Adds the GameObject to the pool and deactivates it
	public void Despawn(string poolName, GameObject go){
		if(go == null)
			return;

		if(!pools.ContainsKey(poolName)){
			Debug.LogError("No pool with name '" + poolName + "' found." + go);
			return;
		}

		Pool pool = pools[poolName];
		if(Deactivate)
			go.SetActive(false);

		go.SendMessageUpwards("OnDespawn", SendMessageOptions.DontRequireReceiver);

		go.transform.position = disabledPosition;
		pool.Enqueue(go);
	}

	//Gives you an instance of a prefab
	private GameObject ReleaseGameObject(string poolName){

		if(!pools.ContainsKey(poolName)){
			Debug.LogError("No pool with name '" + poolName + "' found.");
			return null;
		}

		Pool pool = pools[poolName];

		if(pool.Count == 0){
			InstantiatePrefab(poolName, pool.prefab);
		}

		return pool.Dequeue();
	}

	//Creates one Instance of a prefab
	private void InstantiatePrefab(string poolName, GameObject prefab){
		GameObject go = (GameObject)GameObject.Instantiate(prefab, prefab.transform.localPosition, prefab.transform.localRotation);
		Despawn(poolName, go);
	}

	//Creates one Instance of a prefab and sets his parent
	private void InstantiatePrefab(string poolName, GameObject prefab, GameObject parent){
		GameObject go = (GameObject)GameObject.Instantiate(prefab, prefab.transform.localPosition, prefab.transform.localRotation);
		go.transform.parent = parent.transform;
		Despawn(poolName, go);
	}

	public void CreatePool(string poolName, GameObject prefab){
		if(!pools.ContainsKey(poolName)){
			pools.Add (poolName, new Pool(prefab));
            publicPools.Add(pools[poolName]);
		}
	}

	public void CreatePool(string poolName, GameObject prefab, GameObject parent, int count){
		CreatePool(poolName, prefab);
		if(!parent){
			//If theres no parent we will create a PoolContainer
			GameObject go = GameObject.Find (poolName + "_PoolContainer");
			if(!go)
				go = new GameObject(poolName + "_PoolContainer");

			parent = go;
		}
		pools[poolName].parent = parent;
		for(int i=0; i<count;i++){
			InstantiatePrefab(poolName, prefab, parent);
		}
	}

	public GameObject Spawn(string poolName, Vector3 position, Quaternion rotation){

		if (!pools.ContainsKey(poolName))
			return null;
		GameObject go = Spawn(poolName, null, position, rotation, pools[poolName].prefab.transform.localScale);
			
		return go;
	}

	public GameObject Spawn(string poolName, Transform parent, Vector3 position, Quaternion rotation, Vector3 scale)
	{
		GameObject go = ReleaseGameObject(poolName);
		if(!go)
			return null;

		if (parent != null)
			go.transform.parent = parent;
		else
			go.transform.parent = pools[poolName].parent.transform;

		go.transform.localPosition = position;
		go.transform.localRotation = rotation;
		go.transform.localScale = scale;
			
		if(Deactivate)
			go.SetActive(true);

		go.SendMessageUpwards("SetPoolName", poolName, SendMessageOptions.DontRequireReceiver);
        go.SendMessageUpwards("Reset", SendMessageOptions.DontRequireReceiver);

		return go;
	}

	public GameObject Spawn(string poolName, Transform parent, string name, Vector3 position, Quaternion rotation, Vector3 scale)
	{
			
		GameObject go = Spawn(poolName, parent, position, rotation, scale);
		if(!go)
			return null;

		if(name != null && name != "")
			go.name = name;
			
		return go;
	}

    public Dictionary<string, Pool> pools = new Dictionary<string, Pool>();

    public List<Pool> publicPools = new List<Pool>();

    public bool PoolExists(string poolName)
    {
        return pools.ContainsKey(poolName);
    }
}

[System.Serializable]
public class Pool : Queue<GameObject>{

	public Pool(GameObject pPrefab){
		prefab = pPrefab;
	}

	public GameObject parent;
	public GameObject prefab;
}


