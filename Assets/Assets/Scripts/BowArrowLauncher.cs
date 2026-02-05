using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using BowString;
namespace BowString
{
public class BowArrowLauncher : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private BowStringController bowString;
    [SerializeField] private XRSocketInteractor nockSocket;
    [SerializeField] private Transform shootTransform; // forward = dirección de disparo

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
        if (nockSocket == null || !nockSocket.hasSelection) return;

        var interactable = nockSocket.firstInteractableSelected;
        if (interactable == null) return;

        var arrow = interactable.transform.GetComponentInParent<ArrowProjectile>();
        if (arrow == null) return;

        // Suelta del socket (SelectExit limpio)
        if (nockSocket.interactionManager != null)
            nockSocket.interactionManager.SelectExit(nockSocket, interactable);

        float s = Mathf.Clamp01(strength01);
        float speed = Mathf.Lerp(minSpeed, maxSpeed, s);

        Vector3 dir = shootTransform != null ? shootTransform.forward : transform.forward;
        arrow.Fire(dir * speed);
    }
}
}