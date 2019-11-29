using UnityEngine;

using Model = UltrahapticsCoreAsset.UHKit.Model;
using Zone = UltrahapticsCoreAsset.UHKit.Zone;

namespace UltrahapticsCoreAsset.Editor
{

[ExecuteInEditMode]
public class UltrahapticsKitLayout : MonoBehaviour {

    [Tooltip(Tooltips.UltrahapticsKitLayout.ShowKitModels)]
    [SerializeField] private bool showKitModels = true;
    [Tooltip(Tooltips.UltrahapticsKitLayout.UseDefaultLayout)]
    [SerializeField] private bool useDefaultLayout = true;
    [Tooltip(Tooltips.UltrahapticsKitLayout.ApplyDefaultLayoutAtRuntime)]
    [SerializeField] public bool ApplyDefaultLayoutAtRuntime = true;

    [Tooltip(Tooltips.UltrahapticsKitLayout.ShowInteractionZone)]
    [SerializeField] public bool ShowInteractionZone;

    [Tooltip(Tooltips.UltrahapticsKitLayout.UltrahapticsKitModel)]
    [SerializeField] private Model ultrahapticsKitModel;
    [Tooltip(Tooltips.UltrahapticsKitLayout.ArrayModel)]
    [SerializeField] private UnityEngine.Transform arrayModel;
    [Tooltip(Tooltips.UltrahapticsKitLayout.TrackingOrigin)]
    [SerializeField] private UnityEngine.Transform trackingOrigin;

    private bool kitModelUpdatedToDetectedDevice = false;

    public bool ShowKitModels
    {
        get { return showKitModels; }
        set
        {
            showKitModels = value;
            if (arrayModel != null)
            {
                arrayModel.gameObject.SetActive(showKitModels);
            }

            var trackerModel = GameObject.Find("TrackerModel");
            if (trackerModel)
            {
                var mesh = (MeshRenderer)trackerModel.GetComponent<MeshRenderer>();
                mesh.enabled = showKitModels;
            }

        }
    }

    public bool UseDefaultLayout
    {
        get { return useDefaultLayout; }
        set
            {
                useDefaultLayout = value;
                UpdateTrackingOrigin();
            }
    }

    public Model UltrahapticsKitModel
    {
        get { return ultrahapticsKitModel; }
        set
        {
            ultrahapticsKitModel = value;
            UpdateTrackingOrigin();
            UpdateArrayModel();
        }
    }

    private void UpdateTrackingOrigin()
    {
        if (trackingOrigin != null && useDefaultLayout)
        {
            trackingOrigin.localPosition = UHKit.TrackingOrigins[ultrahapticsKitModel];
        }
    }

    private void UpdateArrayModel()
    {
        if (arrayModel != null)
        {
            arrayModel.localScale = UHKit.ModelScales[ultrahapticsKitModel];
        }
    }

    public UnityEngine.Transform ArrayModel
    {
        get { return arrayModel; }
        set { arrayModel = value; }
    }

    public UnityEngine.Transform TrackingOrigin
    {
        get { return trackingOrigin; }
        set { trackingOrigin = value; }
    }

    void OnValidate()
    {
        ShowKitModels = showKitModels;
        UseDefaultLayout = useDefaultLayout;
        UltrahapticsKitModel = ultrahapticsKitModel;
    }

    void OnEnable()
    {
        kitModelUpdatedToDetectedDevice = true;
    }

    void Update()
    {
        if (ApplyDefaultLayoutAtRuntime &&
            kitModelUpdatedToDetectedDevice &&
            SensationCore.Instance != null  &&
            SensationCore.Instance.IsEmitterConnected())
        {
            if (SensationCore.Instance.EmitterModelDescription().Contains("Inspire"))
            {
                UltrahapticsKitModel = UHKit.Model.STRATOSInspire;
            }
            else if (SensationCore.Instance.EmitterModelDescription().Contains("Explore"))
            {
                UltrahapticsKitModel = UHKit.Model.STRATOSExplore;
            }
            else
            {
                UCA.Logger.LogWarning("Found an unsupported array connected so could not" +
                    "set the Emitter Model to that found. Device was a " +
                    SensationCore.Instance.EmitterModelDescription());
            }

            kitModelUpdatedToDetectedDevice = false;
        }
    }

    void OnDrawGizmos()
    {
        if (ShowInteractionZone)
        {
            RenderInteractionZone();
        }
    }

    public void RenderInteractionZone()
    {
        var baseOffset = 0.05f;

        Gizmos.matrix = transform.localToWorldMatrix;

        foreach(var zone in UHKit.InteractionZones[ultrahapticsKitModel])
        {
            var zoneCentre = new UnityEngine.Vector3(0, (zone.Value.y / 2f) + baseOffset, 0);
            Gizmos.color = UHKit.ZoneColours[zone.Key];
            Gizmos.DrawWireCube(zoneCentre, zone.Value);
        }
    }
}

}
