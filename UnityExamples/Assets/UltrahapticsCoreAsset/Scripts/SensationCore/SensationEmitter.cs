using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace UltrahapticsCoreAsset
{

    [DisallowMultipleComponent]
    public class SensationEmitter : MonoBehaviour {

        public bool AllowMockEmitter = false;
        public bool LogEmissionToFile = false;

        public UnityEngine.Transform ArrayTransform;
        public float SphereRadius = 0.005f;
        public Color SphereColor = new Color(100.0f/255.0f, 167.0f/255.0f, 11.0f/255.0f, 1.0f); // Ultrahaptics Green 2
        public uint HistorySize = 50;

        public ISensationSource CurrentSensation { get; private set; }

        [SerializeField] private List<ISensationSource> registeredSources_ = new List<ISensationSource>();
        public List<ISensationSource> ActiveSources { get{ return registeredSources_; } }
        private static int ByPriority(ISensationSource p1, ISensationSource p2)
        {
            return p2.Priority.CompareTo(p1.Priority);
        }

        internal void SortActiveSources()
        {
            registeredSources_.Sort(ByPriority);
        }

        public void RegisterSensationSource(ISensationSource sensationSource)
        {

            registeredSources_.RemoveAll(x => x == null);

            if (!registeredSources_.Contains(sensationSource))
            {
                registeredSources_.Add(sensationSource);
                SortActiveSources();
            }
        }

        public void DeregisterSensationSource(ISensationSource sensationSource)
        {
            if (registeredSources_.Contains(sensationSource))
            {
                registeredSources_.Remove(sensationSource);
            }
        }

        private int NumberOfSensationEmittersInScene()
        {
            return FindObjectsOfType<SensationEmitter>().Length;
        }

        void Start()
        {
            if (NumberOfSensationEmittersInScene() > 1)
            {
                UCA.Logger.LogError("There is more than one Sensation Emitter in the scene. Please ensure there is only one Sensation Emitter in the scene.");
            }
        }

        private bool IsConnectedToAnEmitter()
        {
            return SensationCore.Instance.IsEmitterConnected();
        }

        private void ConnectToEmitter()
        {
            UCA.MockEmitterLoggingEnabled = LogEmissionToFile;

            try
            {
                SensationCore.Instance.AcquireEmitter();
            }
            catch
            {
                if (AllowMockEmitter)
                {
                    Debug.LogWarning("No physical Ultrahaptics device detected. Using a Mock " +
                        UCA.MockEmitterModel + " device");

                    UCA.Logger.Enabled = false;
                    try
                    {
                        SensationCore.Instance.AcquireMockEmitter();
                    }
                    catch
                    {
                    }
                    finally
                    {
                        UCA.Logger.Enabled = true;
                    }
                }
            }
        }

        void OnEnable()
        {
            if (SensationCore.Instance != null && !IsConnectedToAnEmitter())
            {
                ConnectToEmitter();
            }
        }

        ISensationSource GetSensationSourceOfPlaybackInstance(uhsclHandle playbackInstanceHandle)
        {
            var sensationSource = ActiveSources.FirstOrDefault(x => x.PlaybackInstanceHandle == playbackInstanceHandle);
            return sensationSource;
        }

        public void Update()
        {
            if (SensationCore.Instance != null)
            {
                if (!IsConnectedToAnEmitter())
                {
                    ConnectToEmitter();
                }

                if (IsConnectedToAnEmitter())
                {
                    SensationCore.Instance.SetEvaluationHistorySize(HistorySize);
                    if (SensationCore.Instance.IsCurrentlyPlaying())
                    {
                        var currentInstance = SensationCore.Instance.GetCurrentlyPlayingInstance();
                        CurrentSensation = GetSensationSourceOfPlaybackInstance(currentInstance);
                    }
                    else
                    {
                        CurrentSensation = null;
                    }
                }
                else
                {
                    CurrentSensation = null;
                }
            }
        }

        void OnDisable()
        {
            if (SensationCore.Instance != null && IsConnectedToAnEmitter())
            {
                SensationCore.Instance.ReleaseEmitter();
            }
        }

        void OnDrawGizmos()
        {
            if (SensationCore.Instance != null && IsConnectedToAnEmitter())
            {
                Render();
            }
        }

        public void Render()
        {
            if (SensationCore.Instance != null && IsConnectedToAnEmitter())
            {
                var focalPoints = SensationCore.Instance.GetEvaluationHistory();
                if (focalPoints != null)
                {
                    RenderFocalPoints(focalPoints, SphereColor, SphereRadius);
                }
            }
        }

        private void RenderFocalPoints(List<UnityEngine.Vector4> focalPoints, Color color, float radius)
        {
            Color previousColor = Gizmos.color;
            Gizmos.color = color;

            foreach (var focalPoint in focalPoints)
            {
                var focalPointInEmitterSpace = new UnityEngine.Vector3(focalPoint.x, focalPoint.y, focalPoint.z);
                var focalPointInUnitySpace = UnityToEmitterSpace.Transform.inverse * focalPointInEmitterSpace;
                if (ArrayTransform != null)
                {
                    focalPointInUnitySpace = ArrayTransform.TransformPoint(focalPointInUnitySpace);
                }
                color.a = focalPoint.w;
                Gizmos.color = color;
                Gizmos.DrawSphere(focalPointInUnitySpace, radius);
            }

            Gizmos.color = previousColor;
        }
    }
}
