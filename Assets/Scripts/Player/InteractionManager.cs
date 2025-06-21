using Items.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class InteractionManager : MonoBehaviour
    {
        public float interactionDistance;
        public float checkRate;
        public TextMeshProUGUI promtText;
        public LayerMask layerMask;
        
        private GameObject _curInteractGameObject;
        private IInteractable _curInteractable;
        private float _lastCheckTime;
        private Camera _camera;
        
        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (Time.time - _lastCheckTime > checkRate)
            {
                _lastCheckTime = Time.time;
                var ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
                
                if (Physics.Raycast(ray, out var hit, interactionDistance, layerMask))
                {
                    if (hit.collider.gameObject != _curInteractGameObject)
                    {
                        _curInteractGameObject = hit.collider.gameObject;
                        
                        // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
                        _curInteractable = _curInteractGameObject.GetComponent<IInteractable>();
                        
                        SetPromtText();
                    }
                }
                else
                {
                    _curInteractable = null;
                    _curInteractGameObject = null;
                    promtText.gameObject.SetActive(false);
                }
            }
        }

        private void SetPromtText()
        {
            promtText.gameObject.SetActive(true);
            promtText.text = $"{_curInteractable.GetInteractionPromt()}";
        }

        public void OnInteractInput(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started && _curInteractable is not null)
            {
                _curInteractable.OnInteract(gameObject);
            }
        }
    }
}