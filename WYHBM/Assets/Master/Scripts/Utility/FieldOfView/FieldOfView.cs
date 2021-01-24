using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class FieldOfView : MonoBehaviour
{
    [SerializeField] private WorldConfig _worldConfig;
    [Space]
    [Range(0f, 1f)] public float meshResolution = 0.1f;
    [ReadOnly] public int stepCount;

    private int _edgeResolveIterations;
    private float _edgeDstThreshold;

    public UnityAction<Vector3> OnFindTarget;
    public UnityAction<Vector3> OnLossTarget;

    private MeshFilter _viewMeshFilter;
    private MeshRenderer _viewMeshRenderer;
    private Mesh _viewMesh;

    private Collider[] _targetsInViewRadius;
    private List<Vector3> _viewPoints;
    private Transform _target;
    private Vector3 _targetLastPosition;
    private Vector3 _directionToTarget;
    private bool _enable;
    private bool _targetVisible;
    private bool _targetDetected;
    private float _distanceToTarget;
    private float _duration;
    private float _viewRadius;
    private float _viewAngle;

    private Coroutine _coroutineFindTarget;
    private WaitForSeconds _waitForSeconds;

    // private Tween _fillAnimation;

    // private int hash_IsDetected = Shader.PropertyToID("_IsDetected");

    // Properties
    private List<Transform> _visibleTargets;
    public List<Transform> VisibleTargets { get { return _visibleTargets; } }

    public void Init(float duration, float radius, float angle)
    {
        _visibleTargets = new List<Transform>();
        _viewPoints = new List<Vector3>();

        _viewMeshFilter = GetComponent<MeshFilter>();
        _viewMeshRenderer = GetComponent<MeshRenderer>();

        _viewMesh = new Mesh();
        _viewMeshFilter.mesh = _viewMesh;
        
        UpdateView(duration, radius, angle);

        _waitForSeconds = new WaitForSeconds(.25f);
        _coroutineFindTarget = StartCoroutine(FindTargetsWithDelay());

        _enable = true;
    }
    public void UpdateView(float duration, float radius, float angle)
    {
        _duration = duration;
        _viewRadius = radius;
        _viewAngle = angle;
    }

    private IEnumerator FindTargetsWithDelay()
    {
        while (true)
        {
            yield return _waitForSeconds;
            FindVisibleTargets();
        }
    }

    private void LateUpdate()
    {
        DrawFieldOfView();
    }

    public void SetState(bool active)
    {
        if (active)
        {
            _coroutineFindTarget = StartCoroutine(FindTargetsWithDelay());
        }
        else
        {
            StopCoroutine(_coroutineFindTarget);
        }
    }

    private void FindVisibleTargets()
    {
        _visibleTargets.Clear();
        _targetVisible = false;

        _targetsInViewRadius = Physics.OverlapSphere(transform.position, _viewRadius, _worldConfig.layerFOVTarget);

        for (int i = 0; i < _targetsInViewRadius.Length; i++)
        {
            _target = _targetsInViewRadius[i].transform;
            _directionToTarget = (_target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, _directionToTarget) < _viewAngle / 2)
            {
                _distanceToTarget = Vector3.Distance(transform.position, _target.position);

                if (!Physics.Raycast(transform.position, _directionToTarget, _distanceToTarget, _worldConfig.layerFOVObstacle))
                {
                    if (!_targetDetected)
                    {
                        _targetDetected = true;

                        _visibleTargets.Add(_target);

                        // _fillAnimation = _viewMeshRenderer.material.DOFloat(1, hash_IsDetected, _duration);

                        OnFindTarget.Invoke(_targetLastPosition);
                    }

                    _targetLastPosition = _target.transform.position;
                    _targetVisible = true;
                }
            }
        }

        if (_targetDetected && !_targetVisible)
        {
            _targetDetected = false;

            // _fillAnimation.Kill();
            // _viewMeshRenderer.material.SetFloat(hash_IsDetected, 0);

            OnLossTarget.Invoke(_targetLastPosition);
        }
    }

    private void DrawFieldOfView()
    {
        if (!_enable)return;

        stepCount = Mathf.RoundToInt(_viewAngle * meshResolution);
        float stepAngleSize = _viewAngle / stepCount;

        _viewPoints.Clear();

        ViewCastInfo oldViewCast = new ViewCastInfo();

        for (int i = 0; i <= stepCount; i++)
        {
            float angle = transform.eulerAngles.y - _viewAngle / 2 + (_viewAngle / stepCount) * i;
            ViewCastInfo newViewCast = ViewCast(angle);

            if (i > 0)
            {
                bool edgeDstThresholdExceeded = Mathf.Abs(oldViewCast.dst - newViewCast.dst) > _edgeDstThreshold;

                if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDstThresholdExceeded))
                {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);

                    if (edge.pointA != Vector3.zero)
                    {
                        _viewPoints.Add(edge.pointA);
                    }

                    if (edge.pointB != Vector3.zero)
                    {
                        _viewPoints.Add(edge.pointB);
                    }
                }

            }

            _viewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
        }

        int vertexCount = _viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];
        Color[] colors = new Color[vertexCount];

        vertices[0] = Vector3.zero;
        colors[0] = Color.white;
        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(_viewPoints[i]);
            colors[i + 1] = Color.black;

            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        _viewMesh.Clear();

        _viewMesh.vertices = vertices;
        _viewMesh.triangles = triangles;
        _viewMesh.colors = colors;
        _viewMesh.RecalculateNormals();
    }

    private EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < _edgeResolveIterations; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);

            bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast.dst - newViewCast.dst) > _edgeDstThreshold;
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

        if (Physics.Raycast(transform.position, dir, out hit, _viewRadius, _worldConfig.layerFOVObstacle))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + dir * _viewRadius, _viewRadius, globalAngle);
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