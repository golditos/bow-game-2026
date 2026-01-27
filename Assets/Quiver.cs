using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;




public class Quiver : MonoBehaviour
{
    [SerializeField] private int maxArrows = 15;
    [SerializeField] private GameObject arrow;
    [SerializeField] private Transform spawnPoint;

    private int currentArrows;
    private GameObject currentArrowInstance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentArrows = maxArrows;
        spawnArrow();
    }



    // Update is called once per frame
    void Update()
    {

    }

    private void spawnArrow()
    {
        if (currentArrows <= 0 || currentArrowInstance != null) return;

        currentArrowInstance = Instantiate(arrow, spawnPoint.position, spawnPoint.rotation, spawnPoint);
        XRGrabInteractable grab = currentArrowInstance.GetComponent<XRGrabInteractable>();
        grab.selectExited.AddListener(OnArrowTaken);
        if (grab != null)
        {
            grab.selectExited.RemoveListener(OnArrowTaken);
        }

    }

    private void OnArrowTaken(BaseInteractionEventArgs args)
    {
        if (currentArrowInstance != null)
        {
            XRGrabInteractable grab = currentArrowInstance.GetComponent<XRGrabInteractable>();
            if (grab != null)
            {
                grab.selectExited.RemoveListener(OnArrowTaken);
            }
            currentArrowInstance.GetComponent<XRGrabInteractable>().selectExited.RemoveListener(OnArrowTaken);
            currentArrowInstance.transform.parent = null;
            currentArrowInstance = null;
            currentArrows--;
            Debug.Log("Arrow taken. Remaining: " + currentArrows);

            Invoke(nameof(spawnArrow), 0.5f);
        }
    }

    public void Refill(int amount)
    {

    }
}
