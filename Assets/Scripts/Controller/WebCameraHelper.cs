using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebCameraHelper : MonoBehaviour
{
    public RawImage rawImage;

    private int index = 0;

    private WebCamTexture currentWebCam;
    public void Run()
    {
        StartCoroutine(Call());
    }

    public IEnumerator Call()
    {
        // 请求权限
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);

        if (Application.HasUserAuthorization(UserAuthorization.WebCam) && WebCamTexture.devices.Length > 0)
        {
            

            // 创建相机贴图
            currentWebCam = new WebCamTexture(WebCamTexture.devices[index].name, Screen.width, Screen.height, 60);
            rawImage.canvasRenderer.SetTexture(currentWebCam);
            currentWebCam.Play();

            //前置后置摄像头需要旋转一定角度,否则画面是不正确的,必须置于Play()函数后
            //rawImage.rectTransform.localEulerAngles = new Vector3(0, 0, -currentWebCam.videoRotationAngle);
        }
        else
        {
            Debug.Log("No Camera");
        }
    }

    //切换前后摄像头
    public void SwitchCamera()
    {
        if (WebCamTexture.devices.Length < 1)
            return;

        if (currentWebCam != null)
            currentWebCam.Stop();


        index++;
        index = index % WebCamTexture.devices.Length;

        // 创建相机贴图
        currentWebCam = new WebCamTexture(WebCamTexture.devices[index].name, Screen.width, Screen.height, 60);
        rawImage.canvasRenderer.SetTexture(currentWebCam);
        currentWebCam.Play();

        //前置后置摄像头需要旋转一定角度，否则画面是不正确的,必须置于Play()函数后
        //rawImage.rectTransform.localEulerAngles = new Vector3(0, 0, -currentWebCam.videoRotationAngle);
    }


}
