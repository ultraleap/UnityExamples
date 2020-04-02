using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LineRenderer))]
public class FollowPath : MonoBehaviour
{

    public GameObject _target;
    public  float _moveSpeed;
   
    private Vector3[] _nodes;
    private float _paraTime;
    private float _startTime;
    public float pathDuration;

    private void OnValidate()
    {
        _nodes = new Vector3[1];
    }
    void Update()
    {
        PathLerp();
    }

    public void updateNodes(List<Vector3> newNodes)
    {
        _nodes = new Vector3[newNodes.Count];
        newNodes.CopyTo(_nodes);

        GetComponent<LineRenderer>().positionCount = _nodes.Length;
        GetComponent<LineRenderer>().SetPositions(_nodes);

        _startTime = Time.time;
    }

    public void UpdateDuration(float duration)
    {
        pathDuration = duration;
    }

    private float SampleInterval()
    {
        return _nodes.Length / pathDuration;
    }

    private int PreviousPoint()
    {
        return Mathf.FloorToInt((_paraTime) * _nodes.Length);
    }

    private void PathLerp()
    {
        if (_nodes.Length < 2) return;

        _paraTime = Mathf.Repeat((Time.time - _startTime) * _moveSpeed, pathDuration) / pathDuration;
        
        int previous = PreviousPoint();
        previous = previous < _nodes.Length - 1 ? previous : 0;
        int next = previous < (_nodes.Length - 2) ? previous + 1 : 0;

        float segmentTime = (_paraTime - (previous * SampleInterval())) * SampleInterval();

        Vector3 lerpedPos = Vector3.Lerp(_nodes[previous], _nodes[next], segmentTime);
        _target.transform.position = transform.TransformPoint(lerpedPos);
    }

    public void SetSpeed(Slider slider)
    {
        _moveSpeed = slider.value;
    }

    public void setTartgetPosition(Vector3 position)
    {
            _target.transform.position = transform.TransformPoint(position);
    }
}
