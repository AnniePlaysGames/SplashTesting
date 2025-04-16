using cakeslice;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public class InteractableView : MonoBehaviour
{
    [SerializeField] private BaseInteractable _interactable;
    private Outline _outline;

    private void Awake() 
        => _outline = GetComponent<Outline>();

    private void OnEnable() 
        => _interactable.OnHoverChangeState += ChangeOutlineState;

    private void OnDisable() 
        => _interactable.OnHoverChangeState -= ChangeOutlineState;

    private void ChangeOutlineState(bool isActive)
    {
        if (isActive)
        {
            _outline.Enable();
        }
        else
        {
            _outline.Disable();
        }
    }
}
