using UnityEditor;
using UnityEngine;

namespace UltrahapticsCoreAsset.Editor
{

public class HardwareInfoEditorWindow : EditorWindow
{
    private const string NoDeviceModelIdentifier = "No Ultrahaptics device detected";
    private const string NoDeviceSerialNumber = "";
    private const string NoDeviceFirmwareVersion = "";

    Texture icon_;

    private string modelIdentifier_ = NoDeviceModelIdentifier;
    public string ModelIdentifier
    {
        get
        {
            return modelIdentifier_;
        }
    }

    private string serialNumber_ = NoDeviceSerialNumber;
    public string SerialNumber
    {
        get
        {
            return serialNumber_;
        }
    }

    private string firmwareVersion_ = NoDeviceFirmwareVersion;
    public string FirmwareVersion
    {
        get
        {
            return firmwareVersion_;
        }
    }

    private bool SensationCoreInstanceIsValid()
    {
        return SensationCore.Instance != null;
    }

    private bool IsHardwareConnected()
    {
        return SensationCore.Instance.IsEmitterConnected();
    }

    private bool IsMockDeviceConnected()
    {
        return SensationCore.Instance.IsEmitterConnected() && SensationCore.Instance.EmitterSerialNumber().Equals("MOCK"); // SDK defined
    }

    private void UpdateLastSeenHardwareToBeDisconnectedState()
    {
        modelIdentifier_ = NoDeviceModelIdentifier;
        serialNumber_ = NoDeviceSerialNumber;
        firmwareVersion_ = NoDeviceFirmwareVersion;
    }

    private void UpdateLastSeenHardwareToBeCurrentHardware()
    {
        modelIdentifier_ = SensationCore.Instance.EmitterModelDescription();
        serialNumber_ = SensationCore.Instance.EmitterSerialNumber();
        firmwareVersion_ = SensationCore.Instance.EmitterFirmwareVersion();
    }

    public void PollHardwareState()
    {
        if (!SensationCoreInstanceIsValid())
        {
            UpdateLastSeenHardwareToBeDisconnectedState();
            return;
        }

        if (IsHardwareConnected() && !IsMockDeviceConnected())
        {
            UpdateLastSeenHardwareToBeCurrentHardware();
        }
        else
        {
            try
            {
                UCA.Logger.Enabled = false;
                SensationCore.Instance.AcquireEmitter();
                if (IsHardwareConnected() && !IsMockDeviceConnected())
                {
                    UpdateLastSeenHardwareToBeCurrentHardware();
                }
                else
                {
                    UpdateLastSeenHardwareToBeDisconnectedState();
                }
                SensationCore.Instance.ReleaseEmitter();
            }
            catch // No Error to report while polling
            {
                UpdateLastSeenHardwareToBeDisconnectedState();
            }
            UCA.Logger.Enabled = true;
        }
    }


    [MenuItem("Ultrahaptics/Hardware Info")]
    static void Init()
    {
        HardwareInfoEditorWindow window = (HardwareInfoEditorWindow)EditorWindow.GetWindow(typeof(HardwareInfoEditorWindow));
        window.Show();
    }

    void OnEnable()
    {
        PollHardwareState();
    }

    void OnGUI()
    {
        titleContent.text = "Hardware Info";
        if (icon_ == null)
        {
            icon_ = AssetDatabase.LoadAssetAtPath<Texture>("Assets/UltrahapticsCoreAsset/Resources/Images/SensationSource.png");
        }
        titleContent.image = icon_;

        GUILayout.Label("Ultrahaptics Device Info", EditorStyles.boldLabel);

        EditorGUILayout.TextField("Device:", ModelIdentifier);
        EditorGUILayout.TextField("Serial:", SerialNumber);
        EditorGUILayout.TextField("Firmware:", FirmwareVersion);

        if (GUILayout.Button("Refresh"))
        {
            PollHardwareState();
        }

        if (!SensationCoreInstanceIsValid())
        {
            EditorGUILayout.HelpBox("No Sensation Core component found. Please add one to your Scene, or add an UltrahapticsKit Prefab.", MessageType.Warning);
        }
    }
}
}