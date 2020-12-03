using System;
using System.Collections.Generic;
using UnityEngine;

namespace Yube
{
	public class ResourceManager : Singleton<ResourceManager>
	{
		public T AcquireInstance<T>(T prefab, Transform parent, Vector3 worldPosition, Quaternion rotation, bool startActive = true) where T : MonoBehaviour
		{
			T component = AcquireInstance(prefab, parent, startActive);
			component.transform.position = worldPosition;
			component.transform.rotation = rotation;
			return component;
		}

		public T AcquireInstance<T>(T prefab, Transform parent, Vector3 worldPosition, bool startActive = true) where T : MonoBehaviour
		{
			T component = AcquireInstance(prefab, parent, startActive);
			component.transform.position = worldPosition;
			return component;
		}

		public T AcquireInstance<T>(T prefab, Transform parent, bool startActive = true) where T : MonoBehaviour
		{
			GameObject instance = AcquireInstance(prefab.gameObject, parent, startActive);
			T component = instance.GetComponent<T>();
			Assertion.Check(component != null, $"There is no componenent of type {typeof(T)} on prefab {prefab.name}", prefab);
			return component;
		}

		public GameObject AcquireInstance(GameObject prefab, Transform parent, bool startActive = true)
		{
			Queue<PooledObject> pooledObjs = null;
			if (!m_pooledObjects.TryGetValue(prefab, out pooledObjs))
			{
				pooledObjs = new Queue<PooledObject>();
				m_pooledObjects.Add(prefab, pooledObjs);
			}
			if (pooledObjs.Count == 0)
			{
				GameObject instance = CreateInstance(prefab, parent, startActive);
				pooledObjs.Enqueue(new PooledObject(prefab, instance));
			}
			PooledObject pooledObject = pooledObjs.Dequeue();
			m_usedObjects.Add(pooledObject);
			pooledObject.Instance.SetActive(startActive);
			pooledObject.Instance.transform.SetParent(parent);
			pooledObject.Instance.transform.localPosition = Vector3.zero;
			return pooledObject.Instance;
		}

		public void ReleaseInstance<T>(T instance) where T : MonoBehaviour
		{
			ReleaseInstance(instance.gameObject);
		}

		public void ReleaseInstance(GameObject instance)
		{
			PooledObject pooledObject = null;
			foreach (PooledObject pooledObj in m_usedObjects)
			{
				if (pooledObj.Instance == instance)
				{
					pooledObject = pooledObj;
					break;
				}
			}

			if (pooledObject != null)
			{
				m_usedObjects.Remove(pooledObject);
				m_pooledObjects[pooledObject.Prefab].Enqueue(pooledObject);
				pooledObject.Instance.SetActive(false);
			}
			else
			{
				Destroy(instance);
			}
		}

		#region Private

		[Serializable]
		private class ObjectToLoad
		{
			public int PoolSize = 10;
			public GameObject Prefab = null;
		}

		private class Pool
		{
			public Transform PoolParent { get; private set; }
			public GameObject Prefab { get; private set; }

			public Pool(GameObject prefab, ResourceManager resourceManager)
			{
				Prefab = prefab;
				GameObject parent = new GameObject();
				parent.name = Prefab.name;
				parent.transform.SetParent(resourceManager.transform);
				PoolParent = parent.transform;
			}

			public GameObject AcquireInstance()
			{
				if (m_pooledInstances.Count == 0)
				{
					CreateInstance();
				}
				return m_pooledInstances.Dequeue();
			}

			private void CreateInstance()
			{
				bool oldPrefabState = Prefab.gameObject.activeSelf;
				Prefab.gameObject.SetActive(false);
				GameObject instance = GameObject.Instantiate(Prefab, PoolParent);
				Prefab.gameObject.SetActive(oldPrefabState);
				m_pooledInstances.Enqueue(instance);
			}

			private Queue<GameObject> m_pooledInstances = new Queue<GameObject>();
		}

		private class PooledObject
		{
			public GameObject Prefab { get; private set; }
			public GameObject Instance { get; private set; }

			public PooledObject(GameObject prefab, GameObject instance)
			{
				Prefab = prefab;
				Instance = instance;
			}
		}

		protected void Start()
		{
			foreach (ObjectToLoad objToLoad in m_objectsToLoad)
			{
				Queue<PooledObject> pooledObjs = new Queue<PooledObject>();
				for (int i = 0; i < objToLoad.PoolSize; i++)
				{
					GameObject obj = CreateInstance(objToLoad.Prefab, gameObject.transform, false);
					pooledObjs.Enqueue(new PooledObject(objToLoad.Prefab, obj));
				}
				m_pooledObjects.Add(objToLoad.Prefab, pooledObjs);
			}
		}

		private GameObject CreateInstance(GameObject prefab, Transform parent, bool startActive = true)
		{
			bool oldPrefabState = prefab.gameObject.activeSelf;
			prefab.gameObject.SetActive(startActive);
			GameObject instance = GameObject.Instantiate(prefab, parent);
			prefab.gameObject.SetActive(oldPrefabState);
			return instance;
		}

		[SerializeField]
		private List<ObjectToLoad> m_objectsToLoad = new List<ObjectToLoad>();

		[NonSerialized]
		private List<PooledObject> m_usedObjects = new List<PooledObject>();
		[NonSerialized]
		private Dictionary<GameObject, Queue<PooledObject>> m_pooledObjects = new Dictionary<GameObject, Queue<PooledObject>>();

		#endregion Private
	}
}