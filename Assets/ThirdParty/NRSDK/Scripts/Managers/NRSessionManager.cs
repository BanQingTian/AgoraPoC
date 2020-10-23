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
    using System;
    using System.IO;
    using UnityEngine;
    using System.Collections;
    using NRKernal.Record;
    using System.Threading.Tasks;

    /// <summary>
    ///  Manages AR system state and handles the session lifecycle.
    ///  this class, application can create a session, configure it, start/pause or stop it.
    /// </summary>
    public class NRSessionManager
    {
        private static readonly NRSessionManager m_Instance = new NRSessionManager();

        public static NRSessionManager Instance
        {
            get
            {
                return m_Instance;
            }
        }

        private LostTrackingReason m_LostTrackingReason = LostTrackingReason.INITIALIZING;
        /// <summary>
        /// Current lost tracking reason.
        /// </summary>
        public LostTrackingReason LostTrackingReason
        {
            get
            {
                return m_LostTrackingReason;
            }
        }

        private SessionState m_SessionState = SessionState.UnInitialized;

        public SessionState SessionState
        {
            get
            {
                return m_SessionState;
            }
            private set
            {
                m_SessionState = value;
            }
        }

        public NRSessionBehaviour NRSessionBehaviour { get; private set; }

        public NRHMDPoseTracker NRHMDPoseTracker { get; private set; }

        internal NativeInterface NativeAPI { get; private set; }

        private NRRenderer NRRenderer { get; set; }

        public NRVirtualDisplayer VirtualDisplayer { get; set; }

        public bool IsInitialized
        {
            get
            {
                return (SessionState != SessionState.UnInitialized
                    && SessionState != SessionState.Destroyed);
            }
        }

        public void CreateSession(NRSessionBehaviour session)
        {
            if (SessionState != SessionState.UnInitialized && SessionState != SessionState.Destroyed)
            {
                return;
            }

            SetAppSettings();

            if (NRSessionBehaviour != null)
            {
                NRDebugger.LogError("[NRSessionManager] Multiple SessionBehaviour components cannot exist in the scene. " +
                  "Destroying the newest.");
                GameObject.DestroyImmediate(session.gameObject);
                return;
            }
            NRSessionBehaviour = session;
            NRHMDPoseTracker = session.GetComponent<NRHMDPoseTracker>();

            NRAndroidPermissionsManager.GetInstance().
                RequestAndroidPermission(NativeConstants.READ_EXTERNAL_STORAGE_PERMISSION)
                .ThenAction((result) =>
                {
                    NRDebugger.Log("[NRSessionManager] Get Permission request result:" + result.IsAllGranted);
                    if (!result.IsAllGranted)
                    {
                        ShowErrorTips(NativeConstants.SdcardPermissionDenyErrorTip);
                        throw new NRSdcardPermissionDenyError(NativeConstants.SdcardPermissionDenyErrorTip);
                    }

                    try
                    {
                        NRDevice.Instance.Init();
                    }
                    catch (Exception)
                    {
                        throw;
                    }

                    NativeAPI = new NativeInterface();
                    AsyncTaskExecuter.Instance.RunAction(() =>
                    {
                        NRDebugger.Log("[NRSessionManager] AsyncTaskExecuter: Create tracking");
                        switch (NRHMDPoseTracker.TrackingMode)
                        {
                            case NRHMDPoseTracker.TrackingType.Tracking6Dof:
                                NativeAPI.NativeTracking.Create();
                                NativeAPI.NativeTracking.SetTrackingMode(TrackingMode.MODE_6DOF);
                                break;
                            case NRHMDPoseTracker.TrackingType.Tracking3Dof:
                                NativeAPI.NativeTracking.Create();
                                NRSessionBehaviour.SessionConfig.PlaneFindingMode = TrackablePlaneFindingMode.DISABLE;
                                NRSessionBehaviour.SessionConfig.ImageTrackingMode = TrackableImageFindingMode.DISABLE;
                                NativeAPI.NativeTracking.SetTrackingMode(TrackingMode.MODE_3DOF);
                                break;
                            default:
                                break;
                        }
                    });

                    NRKernalUpdater.OnPreUpdate -= OnPreUpdate;
                    NRKernalUpdater.OnPreUpdate += OnPreUpdate;
                    SessionState = SessionState.Initialized;

                    LoadNotification();
                });
        }

        private void LoadNotification()
        {
            if (this.NRSessionBehaviour.SessionConfig.EnableNotification &&
                GameObject.FindObjectOfType<NRNotificationListener>() == null)
            {
                GameObject.Instantiate(Resources.Load("NRNotificationListener"));
            }
        }

        private bool m_IsSessionError = false;
        public void OprateInitException(Exception e)
        {
            if (m_IsSessionError)
            {
                return;
            }
            m_IsSessionError = true;
            if (e is NRGlassesConnectError)
            {
                ShowErrorTips(NativeConstants.GlassesDisconnectErrorTip);
            }
            else if (e is NRSdkVersionMismatchError)
            {
                ShowErrorTips(NativeConstants.SdkVersionMismatchErrorTip);
            }
            else if (e is NRSdcardPermissionDenyError)
            {
                ShowErrorTips(NativeConstants.SdcardPermissionDenyErrorTip);
            }
            else
            {
                ShowErrorTips(NativeConstants.UnknowErrorTip);
            }
        }

        private void ShowErrorTips(string msg)
        {
            var sessionbehaviour = GameObject.FindObjectOfType<NRSessionBehaviour>();
            if (sessionbehaviour != null)
            {
                GameObject.Destroy(sessionbehaviour.gameObject);
            }
            var input = GameObject.FindObjectOfType<NRInput>();
            if (input != null)
            {
                GameObject.Destroy(input.gameObject);
            }
            var virtualdisplay = GameObject.FindObjectOfType<NRVirtualDisplayer>();
            if (virtualdisplay != null)
            {
                GameObject.Destroy(virtualdisplay.gameObject);
            }

            NRGlassesInitErrorTip errortips;
            if (sessionbehaviour != null && sessionbehaviour.SessionConfig.ErrorTipsPrefab != null)
            {
                errortips = GameObject.Instantiate<NRGlassesInitErrorTip>(sessionbehaviour.SessionConfig.ErrorTipsPrefab);
            }
            else
            {
                errortips = GameObject.Instantiate<NRGlassesInitErrorTip>(Resources.Load<NRGlassesInitErrorTip>("NRErrorTips"));
            }
            errortips.Init(msg, () =>
            {
                NRDevice.ForceKill();
            });
        }

        private void OnPreUpdate()
        {
            if (SessionState != SessionState.Running)
            {
                return;
            }

            m_LostTrackingReason = NativeAPI.NativeHeadTracking.GetTrackingLostReason();
            NRFrame.OnUpdate();
        }

        public void SetConfiguration(NRSessionConfig config)
        {
            if (SessionState == SessionState.UnInitialized
                || SessionState == SessionState.Destroyed
                || SessionState == SessionState.Paused)
            {
                Debug.LogError("[NRSessionManager] Can not SetConfiguration in state:" + SessionState.ToString());
                return;
            }
#if !UNITY_EDITOR
            if (config == null)
            {
                return;
            }
            AsyncTaskExecuter.Instance.RunAction(() =>
            {
                NRDebugger.Log("AsyncTaskExecuter: UpdateConfig");
                NativeAPI.Configration.UpdateConfig(config);
            });
#endif
        }

        public void Recenter()
        {
            if (SessionState != SessionState.Running)
            {
                return;
            }
            AsyncTaskExecuter.Instance.RunAction(() =>
            {
                NativeAPI.NativeTracking.Recenter();
            });
        }

        private void SetAppSettings()
        {
            Application.targetFrameRate = 60;
            QualitySettings.maxQueuedFrames = -1;
            QualitySettings.vSyncCount = 0;
            Screen.fullScreen = true;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }

        public void StartSession()
        {
            if (SessionState == SessionState.Running
                || SessionState == SessionState.Destroyed)
            {
                return;
            }

            AfterInitialized(() =>
            {
                NRDebugger.Log("[NRSessionManager] StartSession After session initialized");
#if !UNITY_EDITOR
                if (NRSessionBehaviour.gameObject.GetComponent<NRRenderer>() == null)
                {
                    NRRenderer = NRSessionBehaviour.gameObject.AddComponent<NRRenderer>();
                    NRRenderer.Initialize(NRHMDPoseTracker.leftCamera, NRHMDPoseTracker.rightCamera);
                }
#endif

                AsyncTaskExecuter.Instance.RunAction(() =>
                {
                    NRDebugger.Log("[NRSessionManager] AsyncTaskExecuter: start tracking");
                    NativeAPI.NativeTracking.Start();
                    NativeAPI.NativeHeadTracking.Create();
                    SessionState = SessionState.Running;
                });

#if UNITY_EDITOR
                InitEmulator();
#endif
                SetConfiguration(NRSessionBehaviour.SessionConfig);
            });
        }

        public void AfterInitialized(Action afterInitialized)
        {
            if (NRSessionBehaviour != null)
            {
                NRKernalUpdater.Instance.StartCoroutine(WaitForInitialized(afterInitialized));
            }
            else
            {
                afterInitialized?.Invoke();
            }
        }

        private IEnumerator WaitForInitialized(Action affterInitialized)
        {
            while (SessionState == SessionState.UnInitialized)
            {
                yield return new WaitForEndOfFrame();
            }
            affterInitialized?.Invoke();
        }

        public void DisableSession()
        {
            if (SessionState != SessionState.Running)
            {
                return;
            }

            // Do not put it in other thread...
            NRRenderer?.Pause();
            NativeAPI.NativeTracking?.Pause();
            NRDevice.Instance.Pause();
            SessionState = SessionState.Paused;
        }

        public void ResumeSession()
        {
            if (SessionState != SessionState.Paused)
            {
                return;
            }

            // Do not put it in other thread...
            NativeAPI.NativeTracking.Resume();
            NRRenderer?.Resume();
            NRDevice.Instance.Resume();
            SessionState = SessionState.Running;
        }

        public void DestroySession()
        {
            if (SessionState == SessionState.Destroyed || SessionState == SessionState.UnInitialized)
            {
                return;
            }

            // Do not put it in other thread...
            SessionState = SessionState.Destroyed;
            NRRenderer?.Destroy();
            NativeAPI.NativeHeadTracking.Destroy();
            NativeAPI.NativeTracking.Destroy();
            NRDevice.Instance.Destroy();
            VirtualDisplayer.Destory();

            FrameCaptureContextFactory.DisposeAllContext();
        }

        private void InitEmulator()
        {
            if (!NREmulatorManager.Inited && !GameObject.Find("NREmulatorManager"))
            {
                NREmulatorManager.Inited = true;
                GameObject.Instantiate(Resources.Load("Prefabs/NREmulatorManager"));
            }
            if (!GameObject.Find("NREmulatorHeadPos"))
            {
                GameObject.Instantiate(Resources.Load("Prefabs/NREmulatorHeadPose"));
            }
        }
    }
}
