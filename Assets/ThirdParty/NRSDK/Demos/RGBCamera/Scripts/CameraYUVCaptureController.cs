using UnityEngine;
using UnityEngine.UI;

namespace NRKernal.NRExamples
{
    [HelpURL("https://developer.nreal.ai/develop/unity/rgb-camera")]
    public class CameraYUVCaptureController : MonoBehaviour
    {
        public RawImage CaptureImage;
        public Text FrameCount;
        private NRRGBCamTextureYUV YuvCamTexture { get; set; }

        private void Start()
        {
            YuvCamTexture = new NRRGBCamTextureYUV();
            BindYuvTexture(YuvCamTexture.GetTexture());
            YuvCamTexture.Play();
        }

        void Update()
        {
            FrameCount.text = YuvCamTexture.FrameCount.ToString();
        }

        public void Play()
        {
            YuvCamTexture.Play();

            // The origin texture will be destroyed after call "Stop",
            // Rebind the texture.
            BindYuvTexture(YuvCamTexture.GetTexture());
        }

        private void BindYuvTexture(NRRGBCamTextureYUV.YUVTextureFrame frame)
        {
            CaptureImage.material.SetTexture("_MainTex", frame.textureY);
            CaptureImage.material.SetTexture("_UTex", frame.textureU);
            CaptureImage.material.SetTexture("_VTex", frame.textureV);
        }

        public void Pause()
        {
            YuvCamTexture.Pause();
        }

        public void Stop()
        {
            YuvCamTexture.Stop();
        }

        void OnDestroy()
        {
            YuvCamTexture.Stop();
        }
    }
}
