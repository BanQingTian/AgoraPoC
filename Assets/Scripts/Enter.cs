using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Enter : MonoBehaviour
{
    public Button EnterBtn;

    // Start is called before the first frame update
    void Start()
    {
        EnterBtn.onClick.AddListener(() => 
        {
            SceneManager.LoadScene("Agore_A2");
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
