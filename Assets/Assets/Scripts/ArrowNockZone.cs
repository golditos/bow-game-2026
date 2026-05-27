using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace BowString
{
    public class ArrowNockZone : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private Transform attachPoint;

        [Header("Settings")]
        [SerializeField] private bool disableGrabWhileNocked = true;

        public ArrowProjectile CurrentArrow { get; private set; }

        private XRGrabInteractable currentGrab;
        private Rigidbody currentRb;

        private void Reset()
        {
            SphereCollider sphere = GetComponent<SphereCollider>();

            if (sphere == null)
                sphere = gameObject.AddComponent<SphereCollider>();

            sphere.isTrigger = true;
            sphere.radius = 0.5f;

            attachPoint = transform;
        }

        private void Awake()
        {
            if (attachPoint == null)
                attachPoint = transform;
        }

        private void OnTriggerEnter(Collider other)
        {
            TryNockArrow(other);
        }

        private void OnTriggerStay(Collider other)
        {
            TryNockArrow(other);
        }

        private void TryNockArrow(Collider other)
        {
            if (CurrentArrow != null)
                return;

            ArrowProjectile arrow = other.GetComponentInParent<ArrowProjectile>();

            if (arrow == null)
                return;

            NockArrow(arrow);
        }

        private void NockArrow(ArrowProjectile arrow)
        {
            CurrentArrow = arrow;

            currentGrab = arrow.GetComponent<XRGrabInteractable>();
            currentRb = arrow.GetComponent<Rigidbody>();

            if (currentRb != null)
            {
                currentRb.isKinematic = true;
                currentRb.useGravity = false;
                currentRb.linearVelocity = Vector3.zero;
                currentRb.angularVelocity = Vector3.zero;
            }

            if (currentGrab != null && disableGrabWhileNocked)
            {
                currentGrab.enabled = false;
            }

            arrow.transform.SetParent(attachPoint, false);
            arrow.transform.localPosition = Vector3.zero;
            arrow.transform.localRotation = Quaternion.identity;

            Debug.Log("Arrow nocked in bow.");
        }

        public ArrowProjectile TakeArrowForShot()
        {
            if (CurrentArrow == null)
                return null;

            ArrowProjectile arrow = CurrentArrow;

            if (currentGrab != null)
                currentGrab.enabled = true;

            arrow.transform.SetParent(null, true);

            CurrentArrow = null;
            currentGrab = null;
            currentRb = null;

            return arrow;
        }
    }
}