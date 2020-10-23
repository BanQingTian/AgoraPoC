/****************************************************************************
* Copyright 2019 Nreal Techonology Limited. All rights reserved.
*                                                                                                                                                          
* This file is part of NRSDK.                                                                                                          
*                                                                                                                                                           
* https://www.nreal.ai/        
* 
*****************************************************************************/

namespace NRKernal
{
    using AOT;
    using System;
    using System.Runtime.InteropServices;
    using System.Threading;
    using UnityEngine;

    /// <summary>
    /// Manage the HMD device and quit 
    /// </summary>
    public partial class NRDevice : SingleTon<NRDevice>
    {
        public enum GlassesEventType
        {
            PutOn,
            PutOff,
            PlugOut
        }
        public delegate void GlassesEvent(GlassesEventType eventtype);
        public delegate void GlassesDisconnectEvent(GlassesDisconnectReason reason);
        public delegate void GlassedTempLevelChanged(GlassesTemperatureLevel level);
        public static event GlassesEvent OnGlassesStateChanged;
        public static event GlassesDisconnectEvent OnGlassesDisconnect;
        public static event GlassedTempLevelChanged OnGlassesTempLevelChanged;

        private NativeHMD m_NativeHMD;
        public NativeHMD NativeHMD
        {
            get
            {
                if (isGlassesPlugOut)
                {
                    return null;
                }
                if (!m_IsInit)
                {
                    this.Init();
                }
                return m_NativeHMD;
            }
        }

        private readonly object m_Lock = new object();
        private NativeGlassesController m_NativeGlassesController;
        public NativeGlassesController NativeGlassesController
        {
            get
            {
                if (isGlassesPlugOut)
                {
                    return null;
                }
                if (!m_IsInit)
                {
                    this.Init();
                }
                return m_NativeGlassesController;
            }
        }

        private bool m_IsInit = false;
        private static bool isGlassesPlugOut = false;

#if UNITY_ANDROID && !UNITY_EDITOR
        private static AndroidJavaObject m_UnityActivity;
#endif

        /// <summary>
        /// Init HMD device.
        /// </summary>
        public void Init()
        {
            if (m_IsInit || isGlassesPlugOut)
            {
                return;
            }
            NRTools.Init();
            MainThreadDispather.Initialize();
#if UNITY_ANDROID && !UNITY_EDITOR
            // Init before all actions.
            AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            m_UnityActivity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            NativeApi.NRSDKInitSetAndroidActivity(m_UnityActivity.GetRawObject()); 
#endif
            CreateGlassesController();
            CreateHMD();

            m_IsInit = true;
        }

        public void Pause()
        {
            if (isGlassesPlugOut)
            {
                return;
            }
            PauseGlassesController();
            PauseHMD();
        }

        public void Resume()
        {
            if (isGlassesPlugOut)
            {
                return;
            }
            ResumeGlassesController();
            ResumeHMD();
        }

        #region HMD
        private void CreateHMD()
        {
            if (isGlassesPlugOut)
            {
                return;
            }
#if !UNITY_EDITOR
            lock (m_Lock)
            {
                m_NativeHMD = new NativeHMD();
                m_NativeHMD.Create();
            }
#endif
        }

        private void PauseHMD()
        {
            if (isGlassesPlugOut)
            {
                return;
            }
#if !UNITY_EDITOR
            lock (m_Lock)
            {
                m_NativeHMD?.Pause();
            }
#endif
        }

        private void ResumeHMD()
        {
            if (isGlassesPlugOut)
            {
                return;
            }
#if !UNITY_EDITOR
            lock (m_Lock)
            {
                m_NativeHMD?.Resume();
            }
#endif
        }

        private void DestroyHMD()
        {
#if !UNITY_EDITOR
            lock (m_Lock)
            {
                m_NativeHMD?.Destroy();
                m_NativeHMD = null;
            }
#endif
        }

        public NativeResolution GetRGBCameraResolution()
        {
            if (NativeHMD == null)
            {
                throw new NRGlassesConnectError("Init hmd device faild.");
            }
            return NativeHMD.GetEyeResolution((int)NativeEye.LEFT);
        }
        #endregion

        #region Glasses Controller
        public GlassesTemperatureLevel TemperatureLevel
        {
            get
            {
                if (isGlassesPlugOut)
                {
                    return GlassesTemperatureLevel.TEMPERATURE_LEVEL_NORMAL;
                }
                this.Init();
#if !UNITY_EDITOR
                return this.NativeGlassesController.GetTempratureLevel();
#else
                return GlassesTemperatureLevel.TEMPERATURE_LEVEL_NORMAL;
#endif
            }
        }

        private void CreateGlassesController()
        {
            if (isGlassesPlugOut)
            {
                return;
            }
#if !UNITY_EDITOR
            try
            {
                lock (m_Lock)
                {
                    m_NativeGlassesController = new NativeGlassesController();
                    m_NativeGlassesController.Create();
                    m_NativeGlassesController.RegisGlassesWearCallBack(OnGlassesWear, 1);
                    m_NativeGlassesController.RegistGlassesEventCallBack(OnGlassesDisconnectEvent);
                    m_NativeGlassesController.Start();
                }
            }
            catch (Exception)
            {
                throw;
            }
#endif
        }

        private void PauseGlassesController()
        {
            if (isGlassesPlugOut)
            {
                return;
            }
#if !UNITY_EDITOR
            lock (m_Lock)
            {
                m_NativeGlassesController?.Pause();
            }
#endif
        }

        private void ResumeGlassesController()
        {
            if (isGlassesPlugOut)
            {
                return;
            }
#if !UNITY_EDITOR
            lock (m_Lock)
            {
                m_NativeGlassesController?.Resume();
            }
#endif
        }

        private void DestroyGlassesController()
        {
#if !UNITY_EDITOR
            lock (m_Lock)
            {
                m_NativeGlassesController?.Stop();
                m_NativeGlassesController?.Destroy();
                m_NativeGlassesController = null;
            }
#endif
        }

        [MonoPInvokeCallback(typeof(NativeGlassesController.NRGlassesControlWearCallback))]
        private static void OnGlassesWear(UInt64 glasses_control_handle, int wearing_status, UInt64 user_data)
        {
            Debug.Log("[NRDevice] " + (wearing_status == 1 ? "Glasses put on" : "Glasses put off"));
            MainThreadDispather.QueueOnMainThread(() =>
            {
                OnGlassesStateChanged?.Invoke(wearing_status == 1 ? GlassesEventType.PutOn : GlassesEventType.PutOff);
            });
        }

        [MonoPInvokeCallback(typeof(NativeGlassesController.NRGlassesControlNotifyQuitAppCallback))]
        private static void OnGlassesDisconnectEvent(UInt64 glasses_control_handle, IntPtr user_data, GlassesDisconnectReason reason)
        {
            if (isGlassesPlugOut)
            {
                return;
            }
            isGlassesPlugOut = true;

            Debug.Log("[NRDevice] OnGlassesDisconnectEvent:" + reason.ToString());
            try
            {
                OnGlassesDisconnect?.Invoke(reason);
            }
            catch (Exception e)
            {
                Debug.Log("[NRDevice] Operate OnGlassesDisconnect event error:" + e.ToString());
                throw e;
            }
            finally
            {
                if (NRSessionManager.Instance.SessionState == SessionState.Running)
                {
                    MainThreadDispather.QueueOnMainThread(() =>
                    {
                        Application.Quit();
                    });
                }
                else
                {
                    ForceKill(true);
                }
            }
        }
        #endregion

        #region Quit
        /// <summary>
        /// Quit the app.
        /// </summary>
        public static void QuitApp()
        {
            Debug.Log("[NRDevice] Start To Quit Application...");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        /// <summary>
        /// Force kill the app.
        /// </summary>
        public static void ForceKill(bool needrelease = false)
        {
            Debug.Log("[NRDevice] Start To ForceKill Application need release sdk?:" + needrelease);
            try
            {
                if (needrelease)
                {
                    NRInput.Destroy();
                    NRSessionManager.Instance.DestroySession();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
#if UNITY_ANDROID && !UNITY_EDITOR
                AndroidJNI.AttachCurrentThread();
                AndroidJavaClass processClass = new AndroidJavaClass("android.os.Process");
                int myPid = processClass.CallStatic<int>("myPid");
                processClass.CallStatic("killProcess", myPid);
#endif
            }
        }

        /// <summary>
        /// Destory HMD resource.
        /// </summary>
        public void Destroy()
        {
            DestroyGlassesController();
            DestroyHMD();
        }
        #endregion

#if UNITY_ANDROID && !UNITY_EDITOR
        private struct NativeApi
        {
            [DllImport(NativeConstants.NRNativeLibrary)]
            public static extern NativeResult NRSDKInitSetAndroidActivity(IntPtr android_activity);
    }
#endif
    }
}
