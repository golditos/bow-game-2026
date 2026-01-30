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

        public void SetStringPositions(Vector3? midPosition)
        {
            Vector3[] linePoints = new Vector3[midPosition.HasValue ? 3 : 2];
            linePoints[0] = startPoint.localPosition;


            if(midPosition.HasValue) linePoints[1] = transform.InverseTransformPoint(midPosition.Value);
            linePoints[^1] = endPoint.localPosition;
            lineRenderer.positionCount = linePoints.Length;
            lineRenderer.SetPositions(linePoints);
        }
    }
    
}
