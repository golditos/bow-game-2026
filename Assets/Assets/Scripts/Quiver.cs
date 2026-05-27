using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace BowString
{
    public class Quiver : MonoBehaviour
    {
        [SerializeField] private int maxArrows = 15;
        [SerializeField] private GameObject arrow;
        [SerializeField] private Transform spawnPoint;

        private int currentArrows;
        private GameObject currentArrowInstance;
        private XRGrabInteractable currentGrab;

        private void Start()
        {
            currentArrows = maxArrows;
            SpawnArrow();
        }

        private void SpawnArrow()
        {
            if (currentArrows <= 0)
                return;

            if (currentArrowInstance != null)
                return;

            if (arrow == null)
            {
                Debug.LogError("Quiver: falta asignar el prefab de la flecha en el Inspector.");
                return;
            }

            if (spawnPoint == null)
            {
                Debug.LogError("Quiver: falta asignar el Spawn Point en el Inspector.");
                return;
            }

            currentArrowInstance = Instantiate(
                arrow,
                spawnPoint.position,
                spawnPoint.rotation,
                spawnPoint
            );

            currentGrab = currentArrowInstance.GetComponent<XRGrabInteractable>();

            if (currentGrab == null)
            {
                Debug.LogError("Quiver: la flecha instanciada no tiene XRGrabInteractable.");
                return;
            }

            currentGrab.selectEntered.AddListener(OnArrowGrabbed);
        }

        private void OnArrowGrabbed(SelectEnterEventArgs args)
        {
            if (currentArrowInstance == null)
                return;

            if (currentGrab != null)
            {
                currentGrab.selectEntered.RemoveListener(OnArrowGrabbed);
            }

            currentArrowInstance.transform.SetParent(null, true);

            currentArrowInstance = null;
            currentGrab = null;

            currentArrows--;

            Debug.Log("Arrow taken. Remaining: " + currentArrows);

            Invoke(nameof(SpawnArrow), 0.5f);
        }

        public void Refill(int amount)
        {
            currentArrows = Mathf.Clamp(currentArrows + amount, 0, maxArrows);

            if (currentArrowInstance == null)
            {
                SpawnArrow();
            }
        }
    }
}