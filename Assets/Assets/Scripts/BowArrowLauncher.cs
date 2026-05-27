using UnityEngine;

namespace BowString
{
    public class BowArrowLauncher : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private BowStringController bowString;
        [SerializeField] private ArrowNockZone nockZone;
        [SerializeField] private Transform shootTransform;

        [Header("Tuning")]
        [SerializeField] private float minSpeed = 6f;
        [SerializeField] private float maxSpeed = 35f;

        private void OnEnable()
        {
            if (bowString != null)
                bowString.OnBowReleased.AddListener(Fire);
        }

        private void OnDisable()
        {
            if (bowString != null)
                bowString.OnBowReleased.RemoveListener(Fire);
        }

        private void Fire(float strength01)
        {
            if (nockZone == null)
            {
                Debug.LogWarning("BowArrowLauncher: falta asignar Nock Zone.");
                return;
            }

            ArrowProjectile arrow = nockZone.TakeArrowForShot();

            if (arrow == null)
            {
                Debug.Log("No arrow nocked.");
                return;
            }

            float strength = Mathf.Clamp01(strength01);
            float speed = Mathf.Lerp(minSpeed, maxSpeed, strength);

            Vector3 direction = shootTransform != null
                ? shootTransform.forward
                : transform.forward;

            arrow.Fire(direction * speed);
        }
    }
}