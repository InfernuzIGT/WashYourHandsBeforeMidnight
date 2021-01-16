using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class FieldOfView : MonoBehaviour
{
    public float viewRadius = 10;
    [Range(0, 360)]
    public float viewAngle = 90;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();

    [Range(0f, 1f)] public float meshResolution = 0.1f;
    [ReadOnly] public int stepCount;
    private int edgeResolveIterations;
    private float edgeDstThreshold;

    MeshFilter viewMeshFilter;
    MeshRenderer viewMeshRenderer;
    Mesh viewMesh;

    public UnityAction<Vector3> OnFindTarget;
    public UnityAction<Vector3> OnLossTarget;

    private bool _enable;
    private bool _targetVisible;
    private bool _targetDetected;
    private Collider[] _targetsInViewRadius;
    private Transform _target;
    private Vector3 _targetLastPosition;
    private Vector3 _directionToTarget;
    private float _distanceToTarget;

    public void Init()
    {
        viewMeshFilter = GetComponent<MeshFilter>();
        viewMeshRenderer = GetComponent<MeshRenderer>();

        viewMesh = new Mesh();
        viewMeshFilter.mesh = viewMesh;

        StartCoroutine("FindTargetsWithDelay", .25f);

        _enable = true;
    }

    private IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    private void LateUpdate()
    {
        DrawFieldOfView();
    }

    private void FindVisibleTargets()
    {
        visibleTargets.Clear();
        _targetVisible = false;

        _targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for (int i = 0; i < _targetsInViewRadius.Length; i++)
        {
            _target = _targetsInViewRadius[i].transform;
            _directionToTarget = (_target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, _directionToTarget) < viewAngle / 2)
            {
                _distanceToTarget = Vector3.Distance(transform.position, _target.position);

                if (!Physics.Raycast(transform.position, _directionToTarget, _distanceToTarget, obstacleMask))
                {
                    visibleTargets.Add(_target);
                    viewMeshRenderer.material.SetFloat("_Detected", 1);

                    _targetLastPosition = _target.transform.position;
                    _targetVisible = true;
                    _targetDetected = true;

                    OnFindTarget.Invoke(_targetLastPosition);
                }

            }
        }

        if (_targetDetected && !_targetVisible)
        {
            viewMeshRenderer.material.SetFloat("_Detected", 0);
            _targetDetected = false;

            OnLossTarget.Invoke(_targetLastPosition);
        }

    }

    private void DrawFieldOfView()
    {
        if (!_enable)return;

        stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        ViewCastInfo oldViewCast = new ViewCastInfo();
        for (int i = 0; i <= stepCount; i++)
        {
            float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);

            if (i > 0)
            {
                bool edgeDstThresholdExceeded = Mathf.Abs(oldViewCast.dst - newViewCast.dst) > edgeDstThreshold;
                if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDstThresholdExceeded))
                {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                    if (edge.pointA != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointA);
                    }
                    if (edge.pointB != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointB);
                    }
                }

            }

            viewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];
        Color[] colors = new Color[vertexCount];

        vertices[0] = Vector3.zero;
        colors[0] = Color.white;
        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);
            colors[i + 1] = Color.black;

            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        viewMesh.Clear();

        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.colors = colors;
        viewMesh.RecalculateNormals();
    }

    private EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < edgeResolveIterations; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);

            bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast.dst - newViewCast.dst) > edgeDstThreshold;
            if (newViewCast.hit == minViewCast.hit && !edgeDstThresholdExceeded)
            {
                minAngle = angle;
                minPoint = newViewCast.point;
            }
            else
            {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }

        return new EdgeInfo(minPoint, maxPoint);
    }

    private ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirFromAngle(globalAngle, true);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, dir, out hit, viewRadius, obstacleMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, globalAngle);
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)angleInDegrees += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float dst;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle)
        {
            hit = _hit;
            point = _point;
            dst = _dst;
            angle = _angle;
        }
    }

    public struct EdgeInfo
    {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
        {
            pointA = _pointA;
            pointB = _pointB;
        }
    }

}