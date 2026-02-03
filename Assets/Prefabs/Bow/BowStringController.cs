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
        [SerializeField] private float stretchLimit = 0.3f;

        private XRGrabInteractable _interactable;

        private Transform _interactor;

        private float strength;


        private void Awake()
        {
            _interactable = midPointGrab.GetComponent<XRGrabInteractable>();
        }


        private void Start()
        {
            _interactable.selectEntered.AddListener(PreparePull);
            _interactable.selectExited.AddListener(ReleaseBow);
        }

        public void OnDestroy()
        {
            _interactable.selectEntered.RemoveListener(PreparePull);
            _interactable.selectExited.RemoveListener(ReleaseBow);
        }

        private void PreparePull(SelectEnterEventArgs select)
        {
            _interactor = select.interactorObject.transform;
            OnBowPulled.Invoke();
        }

        private void ReleaseBow(SelectExitEventArgs select)
        {
            OnBowReleased.Invoke(strength);
            strength = 0;
            _interactor = null;

            midPointGrab.localPosition = Vector3.zero;
            midPointVisual.localPosition = Vector3.zero;

            bowString.SetStringPositions(null);
        }


        private void Update()
        {
            if (_interactor)
            {
                Vector3 localPos = midPointParent.InverseTransformPoint(midPointGrab.position);
                
                float grabLocalBack  = Mathf.Abs(localPos.x);


                HandlePushedTowardsBow(localPos);

                HandlePulledToLimit(grabLocalBack, localPos);

                HandlePull(grabLocalBack, localPos);


                bowString.SetStringPositions(midPointVisual.position);
            }
        }

        private void HandlePushedTowardsBow(Vector3 localPos)
        {
            if (localPos.x > 0)
            {
                strength = 0;
                midPointGrab.localPosition = Vector3.zero;
            }
        }

        private void HandlePulledToLimit(float grabLocalBack, Vector3 localPos)
        {
            if (localPos.x < 0 && grabLocalBack >= stretchLimit)
            {
                strength = 1;
                midPointVisual.localPosition = new Vector3(0, 0, -stretchLimit);
            }
        }

        private void HandlePull(float grabLocalBack, Vector3 localPos)
        {
            
            if (localPos.x < 0 && grabLocalBack < stretchLimit)
            {
                strength = Remap(grabLocalBack, 0, stretchLimit, 0, 1);

                midPointVisual.localPosition = new Vector3(0, 0, localPos.x);
            }
        }

        private float Remap(float value, float fromMin, float fromMax, float toMin, float toMax)
        {
            return (value - fromMin) / (fromMax - fromMin) * (toMax - toMin) + toMin;
        }
    }
}