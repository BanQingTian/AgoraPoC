using UnityEngine;

public class LoadObjsByPlatform : MonoBehaviour
{
    public GameObject[] androidObjs;
    public GameObject[] windowsObjs;

    private void Awake()
    {
        Load();
    }


    private void Load()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            // load NRSDK
            foreach (var item in androidObjs)
            {
                Instantiate(item);
            }
        }
        else if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            // load a normal camera
            foreach (var item in windowsObjs)
            {
                Instantiate(item);
            }
        }
    }
}
