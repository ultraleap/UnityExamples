using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UltrahapticsCoreAsset
{
    using InputsCache = Dictionary<string, SensationBlockInputs>;

    [ExecuteInEditMode]
    public class SensationSource : ISensationSource
    {
        // We sometimes call set `Xxx = xxx_;`, which while it looks useless is triggering a call to SCL
        // We keep two states for the following, one for the current unity state `_xxx`, one for the current SCL state `_lastSetXxxToSCL`
        // To update the SCL state we use the properties below. This is confusing, ideally we would have one property which could
        // be rendered in the inspector (with custom set function to propagate to SCL) but as far as I know we cannot do this

        // The serialized fields represent Unity UI, whilst non serialized represents the reflected state in SCL

        [SerializeField] private string sensationBlock_ = "None";
        [NonSerialized] private string lastSetBlockToSCL_ = "";
        [SerializeField] private string lastSetBlockToUCA_ = "";

        [NonSerialized] public InputsCache inputsCache = new InputsCache();

        public override String SensationBlock
        {
            get { return sensationBlock_; }
            set
            {
                sensationBlock_ = value;
                if (SensationCore.Instance != null)
                {
                    if (BlockChanged() || BlockHasNeverBeenSet())
                    {
                        if (IsValidBlockName(sensationBlock_))
                        {
                            CacheInputValues();
                            CreateBlock(sensationBlock_);

                            if (isActiveAndEnabled)
                            {
                                StartPlaying();
                            }
                            lastSetBlockToSCL_ = sensationBlock_;
                            lastSetBlockToUCA_ = sensationBlock_;
                        }
                    }
                }
            }
        }

        [SerializeField] private bool running_ = false;
        [NonSerialized] private bool lastSetRunningStateToSCL_ = false;
        [NonSerialized] private bool hasPreviouslySetRunning_ = false;
        public override bool Running
        {
            get { return running_; }
            set
            {
                running_ = value;
                if (SensationCore.Instance != null)
                {
                    if (RunningChanged() || !hasPreviouslySetRunning_)
                    {
                        if (IsPlaying())
                        {
                            if (value)
                            {
                                SensationCore.Instance.Unmute(playbackInstanceHandle_);
                            }
                            else
                            {
                                SensationCore.Instance.Mute(playbackInstanceHandle_);
                            }
                            lastSetRunningStateToSCL_ = value;
                            hasPreviouslySetRunning_ = true;
                        }
                    }
                }
            }
        }

        [SerializeField] private uint priority_ = 0;
        [NonSerialized] private uint lastSetPriorityToSCL_ = 0;
        [NonSerialized] private bool hasPreviouslySetPriority_ = false;
        public override uint Priority
        {
            get { return priority_; }
            set
            {
                priority_ = value;
                if (SensationCore.Instance != null)
                {
                    if (PriorityChanged() || !hasPreviouslySetPriority_)
                    {
                        if (IsPlaying())
                        {
                            SensationCore.Instance.SetPriority(playbackInstanceHandle_, value);
                            lastSetPriorityToSCL_ = value;
                            hasPreviouslySetPriority_ = true;
                            if (sensationEmitter_ != null)
                            {
                                sensationEmitter_.SortActiveSources();
                            }
                        }
                    }
                }
            }
        }

        [SerializeField] public SensationBlockInputs Inputs = null;
        [NonSerialized] internal bool requiresReloadingListOfSensations_ = true;

        [NonSerialized] private uhsclHandle blockHandle_ = uhsclHandle.INVALID_HANDLE;
        [NonSerialized] private uhsclHandle playbackInstanceHandle_ = uhsclHandle.INVALID_HANDLE;
        public override uhsclHandle PlaybackInstanceHandle
        {
            get
            {
                return playbackInstanceHandle_;
            }
        }
        [NonSerialized] private uhsclHandle inputSourceHandle_ = uhsclHandle.INVALID_HANDLE;
        private SensationEmitter sensationEmitter_;

        private bool IsPlaying()
        {
            return playbackInstanceHandle_ != uhsclHandle.INVALID_HANDLE;
        }

        protected virtual void LateUpdate()
        {
            if (SensationCore.Instance != null && IsPlaying() && Inputs != null)
            {
                Inputs.ApplySaved(inputSourceHandle_);
                UpdateSensation();
            }
        }

        public void UpdateSensation()
        {
            try
            {
                SensationCore.Instance.CallUpdate();
            }
            catch (Exception)
            {
                UCA.Logger.LogWarning("Unable to update Sensation Source. Check your Sensation Block does not contain errors and your scene contains an enabled Sensation Emitter component.");
            }
        }

        public void RecreateBlockWithDefaultInputs()
        {
            if (Inputs != null)
            {
                Inputs.Clear();
            }

            var trackingObject = Inputs.TrackingObject;
            CreateBlock(SensationBlock);
            Inputs.TrackingObject = trackingObject;
        }

        public void RecreateBlockWithCurrentInputs()
        {
            CacheInputValues();
            CreateBlock(SensationBlock);
        }

        public void setBlockValidAfterBlockLibraryReload()
        {
            lastSetBlockToUCA_ = sensationBlock_;
        }

        private void CreateBlock(string blockName)
        {
            try
            {
                ReleaseCurrentBlock();
                blockHandle_ = SensationCore.Instance.CreateBlock(blockName);
                lastSetBlockToSCL_ = blockName;
                inputSourceHandle_ = SensationCore.Instance.CreateInputSource(blockHandle_);

                SetInputValues();

                Inputs.ReassignInputsHandles(blockHandle_);
                Inputs.RegisterAutoMapper();
                Inputs.RegisterInputs();
            }
            catch
            {
                ReleaseCurrentBlock();
                Inputs.Clear();
            }
        }

        private void CacheInputValues()
        {
            if (BlockHasNeverBeenSet())
            {
                if (BlockHasInputValuesAsigned())
                {
                    inputsCache[sensationBlock_] = Inputs;
                    inputsCache[sensationBlock_].TrackingObject = Inputs.TrackingObject;
                }
            }
            else
            {
                inputsCache[lastSetBlockToUCA_] = Inputs;
                inputsCache[lastSetBlockToUCA_].TrackingObject = Inputs.TrackingObject;
            }
        }

        private bool BlockHasInputValuesAsigned()
        {
            return Inputs != null && Inputs.Count != 0;
        }

        private void SetInputValues()
        {
            if (inputsCache.ContainsKey(sensationBlock_))
            {
                Inputs = inputsCache[sensationBlock_];
            }
            else
            {
                Inputs = SensationBlockInputs.DefaultInputsForBlock(blockHandle_, inputSourceHandle_);
            }
        }

        public void ReleaseCurrentBlock()
        {
            StopPlaying();
            if (Inputs != null)
            {
                Inputs.DeregisterInputs();
            }
            inputSourceHandle_ = uhsclHandle.INVALID_HANDLE;
            blockHandle_ = uhsclHandle.INVALID_HANDLE;
            lastSetBlockToUCA_ = "";
        }

        internal void InvalidateListOfSensationBlocks()
        {
            requiresReloadingListOfSensations_ = true;
            inputsCache.Clear();
        }

        private void StartPlaying()
        {
            if (IsPlaying())
            {
                StopPlaying();
            }

            if (blockHandle_ == uhsclHandle.INVALID_HANDLE && IsValidBlockName(sensationBlock_))
            {
                RecreateBlockWithCurrentInputs();
            }

            if (SensationCore.Instance != null && blockHandle_ != uhsclHandle.INVALID_HANDLE)
            {
                playbackInstanceHandle_ = SensationCore.Instance.CallStart(blockHandle_, inputSourceHandle_);
            }
            Priority = priority_;
            Running = running_;
        }

        public virtual void Start()
        {
            // This is here because OnEnable fails on the first call because SensationCore is null. We catch it before going into normal update loop by calling it here in Start
            // See : https://docs.unity3d.com/Manual/ExecutionOrder.html
            OnEnable();
        }

        protected virtual void OnEnable()
        {
            if (SensationCore.Instance == null)
            {
                return;
            }

            if (sensationEmitter_ == null)
            {
                sensationEmitter_ = GameObject.FindObjectOfType<SensationEmitter>();
            }
            if (sensationEmitter_ != null)
            {
                sensationEmitter_.RegisterSensationSource(this);
            }

            if (TransitionedFromEditModeToPlayMode())
            {
                SensationBlock = sensationBlock_;
            }
            else
            {
                StartPlaying();
            }
        }

        private void StopPlaying()
        {
            if (SensationCore.Instance != null && IsPlaying())
            {
                SensationCore.Instance.Stop(playbackInstanceHandle_);
            }
            playbackInstanceHandle_ = uhsclHandle.INVALID_HANDLE;
            hasPreviouslySetRunning_ = false;
            hasPreviouslySetPriority_ = false;
        }

        protected virtual void OnDisable()
        {
            StopPlaying();

            if (sensationEmitter_ != null)
            {
                sensationEmitter_.DeregisterSensationSource(this);
            }
        }

        private bool BlockChanged()
        {
            return sensationBlock_ != lastSetBlockToSCL_ || lastSetBlockToUCA_ != sensationBlock_;
        }
        private bool BlockHasNeverBeenSet()
        {
            return lastSetBlockToUCA_ == "";
        }

        private bool IsValidBlockName(string blockName)
        {
            return blockName != "" && blockName != "None";
        }

        private bool TransitionedFromEditModeToPlayMode()
        {
            return sensationBlock_ != "" && lastSetBlockToSCL_ == "" && lastSetBlockToUCA_ == sensationBlock_;
        }

        private bool RunningChanged()
        {
            return running_ != lastSetRunningStateToSCL_;
        }

        private bool PriorityChanged()
        {
            return priority_ != lastSetPriorityToSCL_;
        }

        public virtual void OnValidate()
        {
            try
            {
                if (BlockChanged())
                {
                    SensationBlock = sensationBlock_;
                }
                if (RunningChanged())
                {
                    Running = running_;
                }
                if (PriorityChanged())
                {
                    Priority = priority_;
                }
            }
            catch (DllNotFoundException)
            {
                // When re-importing the asset, scripts may be loaded before the native plugin
                // and OnValidate called on them, causing these spurious errors.
                // Functionality is not affected.
            }
        }

        public void RunForDuration(float seconds)
        {
            if (seconds < 0)
            {
                UCA.Logger.LogWarning("Playback Duration cannot be negative");
                return;
            }

            Running = true;
            StartCoroutine(StopRunningAfterSeconds(seconds));
        }

        public List<string> GetVisibleInputs()
        {
            return Inputs.Where(x => x.IsVisible == true).Select( x => x.Name ).ToList();
        }

        public List<string> GetHiddenInputs()
        {
            return Inputs.Where(x => x.IsVisible == false).Select( x => x.Name ).ToList();
        }

        private IEnumerator StopRunningAfterSeconds(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            Running = false;
        }

        public T GetMetaData<T>(string identifier)
        {
            return SensationCore.Instance.GetMetaData<T>(blockHandle_, identifier);
        }

    }
}
