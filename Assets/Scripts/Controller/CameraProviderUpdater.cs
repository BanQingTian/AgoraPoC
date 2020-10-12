using System;
using UnityEngine;

public class CameraProviderUpdater : MonoBehaviour
{
    private static CameraProviderUpdater m_Instance;

    public static CameraProviderUpdater Instance
    {
        get
        {
            if (m_Instance == null)
            {
                GameObject updateObj = new GameObject("CameraProviderUpdater");
                GameObject.DontDestroyOnLoad(updateObj);
                m_Instance = updateObj.AddComponent<CameraProviderUpdater>();
            }
            return m_Instance;
        }
    }

    public event Action OnUpdate;

    private void Update()
    {
        OnUpdate?.Invoke();
    }
}