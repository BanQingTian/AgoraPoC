using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Enter : MonoBehaviour
{
    public Button EnterBtn;

    public Button b1;
    public Button b2;
    public Button b3;

    // Start is called before the first frame update
    void Start()
    {
        EnterBtn.onClick.AddListener(() => { SceneManager.LoadScene("Agore_A2"); });


        b1.onClick.AddListener(() => { UIManager_SampleMode.curChannelName = "nreal1"; Debug.Log("cur channel name is nreal1"); });
        b2.onClick.AddListener(() => { UIManager_SampleMode.curChannelName = "nreal2"; Debug.Log("cur channel name is nreal2"); });
        b3.onClick.AddListener(() => { UIManager_SampleMode.curChannelName = "nreal3"; Debug.Log("cur channel name is nreal3"); });
    }

}
