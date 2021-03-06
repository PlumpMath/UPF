﻿using UnityEngine;

namespace FirstWave.Unity.Core.Utilities
{
    public abstract class SafeSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected static T _instance;
        public bool persistAcrossScenes = true;
        
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    var existing = FindObjectOfType<T>();
                    if (existing != null)
                        _instance = existing;
                    else
                    {
                        var managerObject = new GameObject();

                        _instance = managerObject.AddComponent<T>();

                        managerObject.name = _instance.GetComponent<SafeSingleton<T>>().managerName;
                    }

                    var ss = _instance.GetComponent<SafeSingleton<T>>();
                    if (ss.persistAcrossScenes)
                        DontDestroyOnLoad(_instance.gameObject);
                }

                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        protected abstract string managerName { get; }

        void Awake()
        {
            if (Instance == null)
            {
                DontDestroyOnLoad(gameObject);
                Instance = this as T;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}
