using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UltrahapticsCoreAsset;

[RequireComponent(typeof(Image))]
public class XYController : MonoBehaviour
{
    public SensationSource source;
    private Image xyPanel;

    public Vector2 xyPos;

    public List<Vector3> points;
    public List<Vector3> stroke;

    public FollowPath path;
    public float size;
    public float sampleRate;
    public float sampleSpacing;

    private bool pointerCaptured;
    private float sampleStart;
    private float sampleDuration;
    private Vector3 lastPoint;
    private int buttonDown;

    private bool capturing;

    // Start is called before the first frame update
    void Start()
    {
        xyPanel = GetComponent<Image>();    
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            for (int i = 1; i < stroke.Count; i++)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(stroke[i - 1], stroke[i]); 

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pointerCaptured && !capturing)
        {
            capturing = true;
            StartCoroutine("SamplePoints");
        }
    }

    public void MouseDown()
    {
        if (Input.GetMouseButton(0))
        {
            CapturePointer(true);
            sampleStart = Time.time;
            buttonDown = 1;
            source.Running = true;
        }
        else
        {
            points.Clear();
            sampleDuration = 0;
            buttonDown = 0;
            source.Running = false;
            path.setTartgetPosition(Vector3.zero);
        }
    }

    public void MouseUp()
    {
        stroke.Clear();
        StopCoroutine("SamplePoints");

        capturing = false;
        CapturePointer(false);
        path.updateNodes(points);
        sampleDuration += (Time.time - sampleStart) * buttonDown;
        path.UpdateDuration(sampleDuration);
    }

    public void CapturePointer(bool state)
    {
        pointerCaptured = state;
    }

    public void MouseToWorldPoint()
    {
        Vector2 mousePos = Input.mousePosition;

        if (MouseRaycast(mousePos, out Vector3 point))
        {
            if (Vector3.Distance(point, lastPoint) > sampleSpacing)
            { 
                points.Add(point);
                lastPoint = point;
                path.setTartgetPosition(point);
            }
        }
    }

    public bool MouseRaycast(Vector3 screenPos, out Vector3 hitPoint)
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(screenPos);
        Debug.DrawRay(mouseRay.origin, mouseRay.direction, Color.cyan);

        if (Physics.Raycast(mouseRay, out RaycastHit hit, Mathf.Infinity))
        {
            hitPoint = hit.transform.InverseTransformPoint(hit.point);
            stroke.Add(hit.point);
            return true;
        }
        hitPoint = Vector3.negativeInfinity;
        return false;
    }

    IEnumerator SamplePoints()
    {
        while (pointerCaptured)
        {
            yield return new WaitForSeconds(sampleRate);
            MouseToWorldPoint();
        }
    }
}
