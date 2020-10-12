using agora_gaming_rtc;
using agora_utilities;
using UnityEngine;
using UnityEngine.UI;

public class ViewBase : MonoBehaviour
{
    public virtual void LoadVideSurface(uint uid)
    {
       
    }

    public virtual void UnloadVideoSurface(uint uid)
    {
       
    }

    public virtual void OnLoad(IStreamingProvider provider)
    {
        Debug.Log("View is onload...");

    }

    public VideoSurface makePlaneSurface(string goName)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Plane);

        if (go == null)
        {
            return null;
        }
        go.name = goName;
        // set up transform
        go.transform.Rotate(-90.0f, 0.0f, 0.0f);
        float yPos = Random.Range(3.0f, 5.0f);
        float xPos = Random.Range(-2.0f, 2.0f);
        go.transform.position = new Vector3(xPos, yPos, 0f);
        go.transform.localScale = new Vector3(0.5f, 0.25f, .5f);

        // configure videoSurface
        VideoSurface videoSurface = go.AddComponent<VideoSurface>();
        return videoSurface;
    }

    private const float Offset = 100;
    public VideoSurface makeImageSurface(string goName)
    {
        GameObject go = new GameObject();

        if (go == null)
        {
            return null;
        }

        go.name = goName;

        // to be renderered onto
        go.AddComponent<RawImage>();

        // make the object draggable
        go.AddComponent<UIElementDragger>();
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas != null)
        {
            go.transform.parent = canvas.transform;
        }
        // set up transform
        go.transform.Rotate(0f, 0.0f, 180.0f);
        float xPos = Random.Range(Offset - Screen.width / 2f, Screen.width / 2f - Offset);
        float yPos = Random.Range(Offset, Screen.height / 2f - Offset);
        go.transform.localPosition = new Vector3(xPos, yPos, 0f);
        go.transform.localScale = new Vector3(3f, 4f, 1f);

        // configure videoSurface
        VideoSurface videoSurface = go.AddComponent<VideoSurface>();
        return videoSurface;
    }
}