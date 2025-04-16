using UnityEngine;
using Zenject;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float _interactDistance = 3f;
    [SerializeField] private LayerMask _interactableMask;

    private IInputService _input;
    private IInteractable _currentTarget;

    [Inject]
    private void Init(IInputService input)
    {
        _input = input;
    }

    private void Update()
    {
        ScanForInteractable();

        if (_input.InteractPressed && _currentTarget != null)
        {
            _currentTarget.Interact();
        }
    }

    private void ScanForInteractable()
    {
        Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);
        RaycastHit[] hits = Physics.RaycastAll(ray, _interactDistance, _interactableMask);

        if (hits.Length == 0)
        {
            ClearCurrentTarget();
            return;
        }

        IInteractable bestTarget = null;
        int bestPriority = int.MinValue;

        foreach (var hit in hits)
        {
            if (hit.collider.TryGetComponent<IInteractable>(out var interactable))
            {
                if (interactable.Priority > bestPriority)
                {
                    bestTarget = interactable;
                    bestPriority = interactable.Priority;
                }
            }
        }

        if (bestTarget != null && bestTarget != _currentTarget)
        {
            ClearCurrentTarget();
            _currentTarget = bestTarget;
            _currentTarget.OnHoverEnter();
        }
        else if (bestTarget == null)
        {
            ClearCurrentTarget();
        }
    }

    private void ClearCurrentTarget()
    {
        if (_currentTarget != null)
        {
            _currentTarget.OnHoverExit();
            _currentTarget = null;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (_camera == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(_camera.transform.position, _camera.transform.forward * _interactDistance);
    }
#endif
}