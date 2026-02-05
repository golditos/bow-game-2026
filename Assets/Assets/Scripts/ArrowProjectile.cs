using UnityEngine;

namespace BowString
{
    public class ArrowProjectile : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private Rigidbody rb;
        [SerializeField] private Collider[] cols;

        [Header("Flight")]
        [SerializeField] private float alignSpeed = 20f;

        [Header("Stick")]
        [SerializeField] private bool stickOnHit = true;
        [SerializeField] private float stickDepth = 0.02f;

        private bool _fired;
        private bool _stuck;

        private void Reset()
        {
            rb = GetComponent<Rigidbody>();
            cols = GetComponentsInChildren<Collider>(true);
        }

        private void Awake()
        {
            if (!rb) rb = GetComponent<Rigidbody>();
            if (cols == null || cols.Length == 0)
                cols = GetComponentsInChildren<Collider>(true);
        }

        public void Fire(Vector3 initialVelocity)
        {
            _fired = true;
            _stuck = false;

            SetCollidersEnabled(true);

            transform.parent = null;

            rb.isKinematic = false;
            rb.useGravity = true;

            rb.linearVelocity = initialVelocity;
            rb.angularVelocity = Vector3.zero;
        }

        private void FixedUpdate()
        {
            if (!_fired || _stuck) return;

            Vector3 v = rb.linearVelocity;
            if (v.sqrMagnitude < 0.01f) return;

            Quaternion target = Quaternion.LookRotation(v.normalized, Vector3.up);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, target, alignSpeed * Time.fixedDeltaTime));
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!_fired || _stuck || !stickOnHit) return;

            _stuck = true;

            rb.isKinematic = true;
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            ContactPoint cp = collision.GetContact(0);

            transform.rotation = Quaternion.LookRotation(-cp.normal, Vector3.up);
            transform.position = cp.point - transform.forward * stickDepth;

            transform.SetParent(collision.transform, true);
        }

        private void SetCollidersEnabled(bool enabled)
        {
            if (cols == null) return;

            foreach (var c in cols)
            {
                if (c) c.enabled = enabled;
            }
        }
    }
}
