using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace BowString
{
    public class BowStringController : MonoBehaviour
    {
        public UnityEvent OnBowPulled;
        public UnityEvent<float> OnBowReleased;

        [SerializeField] private Transform midPointGrab, midPointVisual, midPointParent;
        [SerializeField] private BowString bowString;
        [SerializeField] private float stretchLimit= 0.5f;

        private XRGrabInteractable grabInteractable;

        private Transform interactor;

        private float strenght;


        private void Awake()
        {
            grabInteractable = midPointGrab.GetComponent<XRGrabInteractable>();
        }


        private void Start()
        {
            grabInteractable.selectEntered.AddListener(PreparePull);
            grabInteractable.selectExited.AddListener(ReleaseBow);
        }

        public void OnDestroy()
        {
            grabInteractable.selectEntered.RemoveListener(PreparePull);
            grabInteractable.selectExited.RemoveListener(ReleaseBow);
        }

        private void PreparePull(SelectEnterEventArgs select)
        {
            interactor =  select.interactorObject.transform;
            OnBowPulled.Invoke();
        }

        private void ReleaseBow(SelectExitEventArgs select)
        {
            OnBowReleased.Invoke(strenght);
            strenght = 0;
            grabInteractable = null;

            midPointGrab.localPosition = Vector3.zero;
            midPointVisual.localPosition = Vector3.zero;

            bowString.SetStringPositions(null);
        }


        private void Update()
        {
            if (grabInteractable)
            {
                Vector3 localPos = midPointParent.InverseTransformPoint(midPointGrab.position);
                
                float grabLocalBack  = Mathf.Abs(localPos.z);


                HandlePushedTowardsBow(localPos);

                HandlePulledToLimit(grabLocalBack, localPos);

                HandlePull(grabLocalBack, localPos);


                bowString.SetStringPositions(midPointVisual.position);
            }
        }

        private void HandlePushedTowardsBow(Vector3 localPos)
        {
            if (localPos.z > 0)
            {
                strenght = 0;
                midPointGrab.localPosition = Vector3.zero;
            }
        }

        private void HandlePulledToLimit(float grabLocalBack, Vector3 localPos)
        {
            if (localPos.z > 0 && grabLocalBack >= stretchLimit)
            {
                strenght = 1;
                midPointVisual.localPosition = new Vector3(0, 0, -stretchLimit);
            }
        }

        private void HandlePull(float grabLocalBack, Vector3 localPos)
        {
            
            if (localPos.z < 0 && grabLocalBack < stretchLimit)
            {
                strenght = Remap(grabLocalBack, 0, stretchLimit, 0, 1);

                midPointVisual.localPosition = new Vector3(0, 0, localPos.z);
            }
        }

        private float Remap(float value, float fromMin, float fromMax, float toMin, float toMax)
        {
            return (value - fromMin) / (fromMax - fromMin) * (toMax - toMin) + toMin;
        }
    }
}