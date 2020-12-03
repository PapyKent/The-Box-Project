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
			Pool pool = FindPool(prefab);
			GameObject pooledObject = pool.AcquireInstance();
			pooledObject.SetActive(startActive);
			pooledObject.transform.SetParent(parent);
			pooledObject.transform.localPosition = Vector3.zero;
			return pooledObject;
		}

		public void ReleaseInstance<T>(T instance) where T : MonoBehaviour
		{
			ReleaseInstance(instance.gameObject);
		}

		public void ReleaseInstance(GameObject instance)
		{
			foreach (Pool pool in m_pools)
			{
				if (pool.ReleaseInstance(instance))
				{
					return;
				}
			}
			Destroy(instance);
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
			public GameObject Prefab { get; private set; }
			public Transform PoolTransform { get; private set; }

			public Pool(GameObject prefab, int size, ResourceManager resourceManager)
			{
				Prefab = prefab;
				GameObject parent = new GameObject();
				parent.name = Prefab.name;
				parent.transform.SetParent(resourceManager.transform);
				PoolTransform = parent.transform;

				for (int i = 0; i < size; i++)
				{
					CreateInstance();
				}
			}

			public GameObject AcquireInstance()
			{
				if (m_unusedInstances.Count == 0)
				{
					CreateInstance();
				}
				return m_unusedInstances.Dequeue();
			}

			public bool ReleaseInstance(GameObject instance)
			{
				int instanceIndex = m_usedInstances.IndexOf(instance);
				if (instanceIndex != -1)
				{
					m_usedInstances.Remove(instance);
					instance.SetActive(false);
					instance.transform.SetParent(PoolTransform);
					m_unusedInstances.Enqueue(instance);
					return true;
				}
				return false;
			}

			#region Private

			private void CreateInstance()
			{
				bool oldPrefabState = Prefab.gameObject.activeSelf;
				Prefab.gameObject.SetActive(false);
				GameObject instance = GameObject.Instantiate(Prefab, PoolTransform);
				Prefab.gameObject.SetActive(oldPrefabState);
				m_unusedInstances.Enqueue(instance);
			}

			private List<GameObject> m_usedInstances = new List<GameObject>();
			private Queue<GameObject> m_unusedInstances = new Queue<GameObject>();

			#endregion Private
		}

		protected override void Awake()
		{
			base.Awake();
			foreach (ObjectToLoad objToLoad in m_objectsToLoad)
			{
				Pool pool = new Pool(objToLoad.Prefab, objToLoad.PoolSize, this);
				m_pools.Add(pool);
			}
		}

		private Pool FindPool(GameObject prefab, bool createIfNotExists = true)
		{
			Pool pool = null;
			foreach (Pool existingPool in m_pools)
			{
				if (existingPool.Prefab == prefab)
				{
					pool = existingPool;
					break;
				}
			}
			if (pool == null && createIfNotExists)
			{
				pool = new Pool(prefab, 5, this);
			}
			return pool;
		}

		[SerializeField]
		private List<ObjectToLoad> m_objectsToLoad = new List<ObjectToLoad>();

		[NonSerialized]
		private List<Pool> m_pools = new List<Pool>();

		#endregion Private
	}
}