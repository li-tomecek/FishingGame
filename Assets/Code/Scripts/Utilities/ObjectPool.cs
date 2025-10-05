using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private List<GameObject> _pool;

    public ObjectPool(GameObject _objectType, int amount, GameObject parent)
    {
        _pool = new List<GameObject>();
        GameObject tmp;

        for (int i = 0; i < amount; i++)
        {
            tmp = GameObject.Instantiate(_objectType, parent.transform);
            tmp.SetActive(false);
            _pool.Add(tmp);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < _pool.Count; i++)
        {
            if (!_pool[i].activeInHierarchy)
            {
                return _pool[i];
            }
        }
        return null;
    }

    public GameObject GetActivePooledObject()
    {
        for (int i = 0; i < _pool.Count; i++)
        {
            if (!_pool[i].activeInHierarchy)
            {
                _pool[i].SetActive(true);
                return _pool[i];
            }
        }
        return null;
    }

    public void DeactivateObject(GameObject toDeactivate)
    {
        if (_pool.Contains(toDeactivate))
            toDeactivate.SetActive(false);
    }

}