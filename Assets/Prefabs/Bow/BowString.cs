using UnityEngine;

namespace BowString
{
    [RequireComponent(typeof(LineRenderer))]
    public class BowString : MonoBehaviour
    {
        [SerializeField] private Transform startPoint, endPoint;
        private LineRenderer lineRenderer;

        private void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
        }
        private void Start()
        {
            SetStringPositions(null);
        }

        public void SetStringPositions(Vector3? midPoint)
        {
            Vector3[] linePositions = new Vector3[midPoint.HasValue ? 3 : 2];
            linePositions[0] = startPoint.localPosition;


            if(midPoint.HasValue) linePositions[1] = this.transform.InverseTransformPoint(midPoint.Value);
            linePositions[^1] = endPoint.localPosition;
            lineRenderer.positionCount = linePositions.Length;
            lineRenderer.SetPositions(linePositions);
        }
    }
    
}
