using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System;

public class ArrayAligner : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Enable if the ArrayOrigin transform is going to change position depending " +
        "on what array is used e.g. flat on table vs 40 degrees.")]
    private bool _matchOrientationOfArrayOriginInScene;

    [SerializeField]
    [Tooltip("If Match Orientation Of Array Origin In Scene is set to true, " +
        "this should be a Transform representing the origin of the UH array.")]
    private Transform _arrayOrigin;

    [SerializeField]
    [Tooltip("This GameObject should be the parent of the Main Camera in your scene." +
        "It's the object that will be transformed when the VR controller buttons are clicked.")]
    private Transform _cameraRig;

    // Unity configurable options
    [Header("Calibration Poses")]

    // The orientations of Oculus Touch controllers reported by the SteamVR and OVR (Oculus) SDKs differ
    // so we need separate calibration transforms for each
    [SerializeField]
    private GameObject _riftLeftCalibrationSteamVR;
    [SerializeField]
    private GameObject _riftRightCalibrationSteamVR;
    [SerializeField]
    private GameObject _riftLeftCalibrationOVR;
    [SerializeField]
    private GameObject _riftRightCalibrationOVR;

    [SerializeField]
    private GameObject _viveCalibration;

    [SerializeField]
    private GameObject _wmrLeftCalibration;
    [SerializeField]
    private GameObject _wmrRightCalibration;

    public bool _saveCalibration = true;

    [Header("Sampling")]
    [SerializeField]
    float _timeWindowMs = 2000f;

    [SerializeField]
    float _timeStepMs = 100f;


    // Public events
    public Action CalibrationStarted;
    public Action CalibrationStopped;
    public Action CalibrationFinished;
    public Action CalibrationNotFound;

    // Private members
    private int _numSamples => (int)(_timeWindowMs / _timeStepMs);

    float _thresholdStdPosition = 0.005f;
    float _thresholdStdRotation = 0.005f;

    private Queue<Vector3> _controllerPosition = new Queue<Vector3>();
    private Queue<Vector3> _controllerForward = new Queue<Vector3>();

    private XRNode _nodeToBePositioned;
    private Stopwatch _stopwatch = new Stopwatch();
    private bool _acquiringSamples;

    private bool _leftTriggerPreviouslyPulled, _rightTriggerPreviouslyPulled;

    void Start()
    {
        if (_matchOrientationOfArrayOriginInScene)
        {
            if (_arrayOrigin == null)
            {
                UnityEngine.Debug.LogWarning("ArrayAligner could not find ArrayOrigin in scene to match orientation.");
            }
            else
            {
                this.transform.position = _arrayOrigin.transform.position;
                this.transform.rotation = _arrayOrigin.transform.rotation;
            }
        }

        // Load previous calibration
        if (_saveCalibration || Application.isEditor)
        {
            if (PlayerPrefs.HasKey("ArrayAlignerCalibration"))
            {
                LoadLocation();
            }
            else
            {
                CalibrationNotFound?.Invoke();
            }
        }

        // Check which XR device is connected and display controllers if available
        Transform controllerTransform = GetCalibrationPoseForController(_nodeToBePositioned);
        if (controllerTransform != null)
        {
            controllerTransform.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        // Note: if errors occur here, check ExampleAssets/VRArrayAlignment/InputSettings.png for correct Input Settings.
        var leftTriggerCurrentlyPulled = Mathf.Approximately(Input.GetAxis("LeftVRTriggerAxis"), 1f);
        var rightTriggerCurrentlyPulled = Mathf.Approximately(Input.GetAxis("RightVRTriggerAxis"), 1f);

        if (leftTriggerCurrentlyPulled && !_leftTriggerPreviouslyPulled)
        {
            OnTriggerClicked(XRNode.LeftHand);
        }
        else if (!leftTriggerCurrentlyPulled && _leftTriggerPreviouslyPulled)
        {
            OnTriggerUnclicked(XRNode.LeftHand);
        }

        if (rightTriggerCurrentlyPulled && !_rightTriggerPreviouslyPulled)
        {
            OnTriggerClicked(XRNode.RightHand);
        }
        else if (!rightTriggerCurrentlyPulled && _rightTriggerPreviouslyPulled)
        {
            OnTriggerUnclicked(XRNode.RightHand);
        }

        if (_acquiringSamples)
        {
            if (_stopwatch.ElapsedMilliseconds > _timeStepMs)
            {
                _stopwatch.Reset();
                _stopwatch.Start();

                // All the calculation is done only when the vector is full
                if (_controllerPosition.Count + 1 > _numSamples)
                {
                    AverageAndCalibrate();
                    CalibrationFinished?.Invoke();
                }
                else
                {
                    _controllerPosition.Enqueue(GetNodePosition(_nodeToBePositioned));
                    _controllerForward.Enqueue(GetNodeForwardVector(_nodeToBePositioned));
                }
            }
        }

        _leftTriggerPreviouslyPulled = leftTriggerCurrentlyPulled;
        _rightTriggerPreviouslyPulled = rightTriggerCurrentlyPulled;
    }

    void AverageAndCalibrate()
    {
        List<Vector3> listPositions = _controllerPosition.ToList();
        List<Vector3> listForward = _controllerForward.ToList();

        Vector3 stdPosition = new Vector3(
            CalculateStdDev(listPositions.Select(element => element[0])),
            CalculateStdDev(listPositions.Select(element => element[1])),
            CalculateStdDev(listPositions.Select(element => element[2])));

        Vector3 stdRotation = new Vector3(
            CalculateStdDev(listForward.Select(element => element[0])),
            CalculateStdDev(listForward.Select(element => element[1])),
            CalculateStdDev(listForward.Select(element => element[2])));

        if (stdPosition.magnitude > _thresholdStdPosition || stdRotation.magnitude > _thresholdStdRotation)
        {
            UnityEngine.Debug.LogWarning("Standard deviation exceeds defined threshold");
        }

        Vector3 averagePosition = new Vector3(listPositions.Select(element => element[0]).Average(),
                                                listPositions.Select(element => element[1]).Average(),
                                                listPositions.Select(element => element[2]).Average());

        Vector3 averageForward = new Vector3(listForward.Select(element => element[0]).Average(),
                                                listForward.Select(element => element[1]).Average(),
                                                listForward.Select(element => element[2]).Average());

        var calibrationPose = GetCalibrationPoseForController(_nodeToBePositioned);

        DoPositioning(averagePosition, averageForward, calibrationPose, _cameraRig);
        if (_saveCalibration || Application.isEditor)
        {
            SaveLocation();
        }

        // Clear samples
        _controllerPosition.Clear();
        _controllerForward.Clear();

        _acquiringSamples = false;
    }

    private float CalculateStdDev(IEnumerable<float> values)
    {
        values = values.ToList();
        float ret = 0;
        if (values.Any())
        {
            //Compute the Average
            float avg = values.Average();
            //Perform the Sum of (value-avg)_2_2
            float sum = values.Sum(d => Mathf.Pow(d - avg, 2));
            //Put it all together
            ret = Mathf.Sqrt(sum / (values.Count() - 1));
        }
        return ret;
    }

    void OnTriggerUnclicked(XRNode node)
    {
        if (_acquiringSamples)
        {
            AverageAndCalibrate();
            CalibrationStopped?.Invoke();
        }
    }

    Transform GetCalibrationPoseForController(XRNode controllerNode)
    {
        if (XRDevice.model.Equals("Oculus Rift CV1"))
        {
            // Check whether we're running on SteamVR or Oculus runtime
            if (XRSettings.loadedDeviceName == "OpenVR")
            {
                if (controllerNode == XRNode.LeftHand)
                {
                    UnityEngine.Debug.Log("Using SteamVR Rift left controller for calibration");
                    return _riftLeftCalibrationSteamVR.transform;
                }
                else
                {
                    UnityEngine.Debug.Log("Using SteamVR Rift right controller for calibration");
                    return _riftRightCalibrationSteamVR.transform;
                }
            }
            else if (XRSettings.loadedDeviceName == "Oculus")
            {
                if (controllerNode == XRNode.LeftHand)
                {
                    UnityEngine.Debug.Log("Using OVR Rift left controller for calibration");
                    return _riftLeftCalibrationOVR.transform;
                }
                else
                {
                    UnityEngine.Debug.Log("Using OVR Rift right controller for calibration");
                    return _riftRightCalibrationOVR.transform;
                }
            }
            else
            {
                UnityEngine.Debug.LogErrorFormat("Using Rift, but unrecognised SDK: {0}", XRSettings.loadedDeviceName);
                return null;
            }
        }
        else if (XRDevice.model.Contains("Windows"))
        {
            // Windows 'Mixed Reality'
            if (controllerNode == XRNode.LeftHand)
            {
                UnityEngine.Debug.Log("Using WMR left controller for calibration");
                return _wmrLeftCalibration.transform;
            }
            else
            {
                UnityEngine.Debug.Log("Using WMR right controller for calibration");
                return _wmrRightCalibration.transform;
            }
        }
        else if (XRDevice.model.StartsWith("Vive"))
        {
            UnityEngine.Debug.Log("Using Vive controller for calibration");
            return _viveCalibration.transform;
        }
        else
        {
            UnityEngine.Debug.LogErrorFormat("Unrecognised XR device: {0}", XRDevice.model);
            return null;
        }
    }

    void OnTriggerClicked(XRNode node)
    {
        StopAllCoroutines();

        _nodeToBePositioned = node;

        if (!_acquiringSamples)
        {
            CalibrationStarted?.Invoke();

            // Start putting 2 elements into the queues, so they are not going to be empty if the user instantly unclick.
            _controllerPosition.Enqueue(GetNodePosition(node));
            _controllerForward.Enqueue(GetNodeForwardVector(node));
            _acquiringSamples = true;
            _stopwatch.Start();
        }
    }

    Vector3 GetNodePosition(XRNode node)
    {
        var pos = InputTracking.GetLocalPosition(node);
        if (_cameraRig != null)
        {
            // pos is in local space but we need it in world space
            pos = _cameraRig.TransformPoint(pos);
        }

        return pos;
    }

    Vector3 GetNodeForwardVector(XRNode node)
    {
        var rot = InputTracking.GetLocalRotation(node);
        if (_cameraRig != null)
        {
            // rot is in local space but we need it in world space
            rot = _cameraRig.rotation * rot;
        }

        // Get the forward vector from the quaternion
        return rot * Vector3.forward;
    }

    void DoPositioning(Vector3 controllerPosAvrg, Vector3 controllerForwardAvrg, Transform calibrationPose, Transform cameraRig)
    {
        //shift rotation so the angle parallel to XZ between the controller attach point and the array is 0
        UnityEngine.Debug.DrawRay(controllerPosAvrg, controllerForwardAvrg, Color.white, 1);

        Vector3 poseFwd = calibrationPose.forward;
        UnityEngine.Debug.DrawRay(calibrationPose.position, poseFwd, Color.red, 1);

        //project on XZ
        Vector3 ctrlFwdProj = Vector3.ProjectOnPlane(controllerForwardAvrg, Vector3.up);
        UnityEngine.Debug.DrawRay(controllerPosAvrg, ctrlFwdProj);

        //project on XZ
        Vector3 arrFwdProj = Vector3.ProjectOnPlane(poseFwd, Vector3.up);

        Vector3 cross = Vector3.Cross(ctrlFwdProj, arrFwdProj);

        float angleBetween = Vector3.Angle(ctrlFwdProj, arrFwdProj);

        //find out which way we need to rotate
        if (cross.y < 0)
        {
            angleBetween = -angleBetween;
        }
        cameraRig.transform.RotateAround(controllerPosAvrg, Vector3.up, angleBetween);

        var diff = calibrationPose.position - controllerPosAvrg;
        cameraRig.transform.position += diff;
    }

    public static Quaternion AverageQuaternion(List<Quaternion> multipleQuaternions)
    {
        //Global variable which represents the additive quaternion
        Quaternion addedRotation = Quaternion.identity;

        //The averaged rotational value
        Quaternion averageRotation = new Quaternion();
        float numQuat = multipleQuaternions.Count;
        foreach (Quaternion singleRotation in multipleQuaternions)
        {
            var newRotation = singleRotation;

            //Before we add the new rotation to the average (mean), we have to check whether the quaternion has to be inverted. Because
            //q and -q are the same rotation, but cannot be averaged, we have to make sure they are all the same.
            if (!AreQuaternionsClose(newRotation, multipleQuaternions[0]))
            {
                newRotation = InverseSignQuaternion(newRotation);
            }

            //Average the values
            averageRotation.w += newRotation.w;
            averageRotation.x += newRotation.x;
            averageRotation.y += newRotation.y;
            averageRotation.z += newRotation.z;

        }

        averageRotation = new Quaternion(averageRotation.x / numQuat, averageRotation.y / numQuat, averageRotation.z / numQuat, averageRotation.w / numQuat);
        return averageRotation;
    }

    //Changes the sign of the quaternion components. This is not the same as the inverse.
    public static Quaternion InverseSignQuaternion(Quaternion q)
    {
        return new Quaternion(-q.x, -q.y, -q.z, -q.w);
    }

    //Returns true if the two input quaternions are close to each other. This can
    //be used to check whether or not one of two quaternions which are supposed to
    //be very similar but has its component signs reversed (q has the same rotation as
    //-q)
    public static bool AreQuaternionsClose(Quaternion q1, Quaternion q2)
    {
        float dot = Quaternion.Dot(q1, q2);

        if (dot < 0.0f)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    void SaveLocation()
    {
        PlayerPrefs.SetFloat("ContainerPositionX", _cameraRig.position.x);
        PlayerPrefs.SetFloat("ContainerPositionY", _cameraRig.position.y);
        PlayerPrefs.SetFloat("ContainerPositionZ", _cameraRig.position.z);

        PlayerPrefs.SetFloat("ContainerQuatX", _cameraRig.rotation.x);
        PlayerPrefs.SetFloat("ContainerQuatY", _cameraRig.rotation.y);
        PlayerPrefs.SetFloat("ContainerQuatZ", _cameraRig.rotation.z);
        PlayerPrefs.SetFloat("ContainerQuatW", _cameraRig.rotation.w);

        PlayerPrefs.SetFloat("ArrayAlignerCalibration", 0f);

        PlayerPrefs.Save();
    }


    public void DeleteSavedArrayPreferences()
    {
        PlayerPrefs.DeleteKey("ContainerPositionX");
        PlayerPrefs.DeleteKey("ContainerPositionY");
        PlayerPrefs.DeleteKey("ContainerPositionZ");

        PlayerPrefs.DeleteKey("ContainerQuatX");
        PlayerPrefs.DeleteKey("ContainerQuatY");
        PlayerPrefs.DeleteKey("ContainerQuatZ");
        PlayerPrefs.DeleteKey("ContainerQuatW");

        PlayerPrefs.DeleteKey("ArrayAlignerCalibration");
        UnityEngine.Debug.Log("Array Alignment Preferences Deleted");
    }

    void LoadLocation()
    {
        var positionX = PlayerPrefs.GetFloat("ContainerPositionX");
        var positionY = PlayerPrefs.GetFloat("ContainerPositionY");
        var positionZ = PlayerPrefs.GetFloat("ContainerPositionZ");
        _cameraRig.position = new Vector3(positionX, positionY, positionZ);

        var quatX = PlayerPrefs.GetFloat("ContainerQuatX");
        var quatY = PlayerPrefs.GetFloat("ContainerQuatY");
        var quatZ = PlayerPrefs.GetFloat("ContainerQuatZ");
        var quatW = PlayerPrefs.GetFloat("ContainerQuatW");
        _cameraRig.rotation = new Quaternion(quatX, quatY, quatZ, quatW);
    }
}