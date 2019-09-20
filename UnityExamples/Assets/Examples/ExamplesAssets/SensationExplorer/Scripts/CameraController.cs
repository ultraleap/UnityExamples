using UnityEngine;

// Originally modified from example at: https://wiki.unity3d.com/index.php/MouseOrbitInfiteRotateZoom
public class CameraController : MonoBehaviour
{
    public Transform target;

    //  The Viewer Pane
    public RectTransform viewerPaneRect;

    // Camera Transform Presets
    public Transform CameraDefault;
    public Transform CameraTop;
    public Transform CameraLeft;
    public Transform CameraRight;
    public Transform CameraFront;

    public float xSpeed = 6.0f;
    public float ySpeed = 6.0f;
    public float scrollSpeed = 1.0f;
    public float zoomMin = 0.25f;
    public float zoomMax = 2.0f;
    private float distance;

    private Vector3 position;
    private bool rotationIsActivated;
    private bool panIsActivated;

    float xRotate = 0.0f;
    float yRotate = 0.0f;

    private Vector3 lastPosition;
    private float mouseSensitivity = 0.0005f;

    // Use this for initialization
    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        xRotate = angles.y;
        yRotate = angles.x;
    }

    public void SetCameraDefault()
    {
        transform.localPosition = CameraDefault.localPosition;
        transform.localRotation = CameraDefault.localRotation;
        transform.localScale = CameraDefault.localScale;
    }

    public void SetCameraTop()
    {
        transform.localPosition = CameraTop.localPosition;
        transform.localRotation = CameraTop.localRotation;
        transform.localScale = CameraTop.localScale;
    }

    public void SetCameraLeft()
    {
        transform.localPosition = CameraLeft.localPosition;
        transform.localRotation = CameraLeft.localRotation;
        transform.localScale = CameraLeft.localScale;
    }

    public void SetCameraRight()
    {
        transform.localPosition = CameraRight.localPosition;
        transform.localRotation = CameraRight.localRotation;
        transform.localScale = CameraRight.localScale;
    }

    public void SetCameraFront()
    {
        transform.localPosition = CameraFront.localPosition;
        transform.localRotation = CameraFront.localRotation;
        transform.localScale = CameraFront.localScale;
    }

    void LateUpdate()
    {
        // If the Mouse Focus is in Viewer Section-only, handle Camera Interaction
        Vector2 mousePosition = Input.mousePosition - new Vector3(Screen.width / 2, Screen.height / 2);
        if (!viewerPaneRect.rect.Contains(mousePosition))
        {
            return;
        };

        // only update rotation if the right-mouse button is held down
        if (Input.GetMouseButtonDown(1))
        {
            rotationIsActivated = true;
        }

        // if mouse button is let UP then stop rotating camera
        if (Input.GetMouseButtonUp(1))
        {
            rotationIsActivated = false;
        }

        // only update panning if the middle mousebutton is held down
        if (Input.GetMouseButtonDown(2))
        {
            panIsActivated = true;
            lastPosition = Input.mousePosition;
        }

        // if mouse button is let UP then stop panning camera
        if (Input.GetMouseButtonUp(2))
        {
            panIsActivated = false;
        }

        if (target && rotationIsActivated)
        {

            //  get the distance the mouse moved in the respective direction
            xRotate += Input.GetAxis("Mouse X") * xSpeed;
            yRotate -= Input.GetAxis("Mouse Y") * ySpeed;

            // when mouse moves left and right we actually rotate around local y axis
            transform.RotateAround(target.position, transform.up, xRotate);

            // when mouse moves up and down we actually rotate around the local x axis
            transform.RotateAround(target.position, transform.right, yRotate);

            // reset back to 0 so it doesn't continue to rotate while holding the button
            xRotate = 0;
            yRotate = 0;
        }
        else if (target && panIsActivated)
        {
            Vector3 delta = Input.mousePosition - lastPosition;
            transform.Translate(-delta.x * mouseSensitivity, -delta.y * mouseSensitivity, 0);
            lastPosition = Input.mousePosition;
        }
        else
        {
            // see if mouse wheel is used
            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                // get the distance between camera and target
                distance = Vector3.Distance(transform.position, target.position);

                // get mouse wheel info to zoom in and out
                distance = ZoomLimit(distance - Input.GetAxis("Mouse ScrollWheel") * scrollSpeed, zoomMin, zoomMax);
                
                // position the camera FORWARD the right distance towards target
                position = -(transform.forward * distance) + target.position;

                // move the camera
                transform.position = position;
            }
        }
    }

    public static float ZoomLimit(float dist, float min, float max)
    {
        if (dist < min)
            dist = min;

        if (dist > max)
            dist = max;
        return dist;
    }
}
