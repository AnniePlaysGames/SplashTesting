public interface IInteractable
{
    int Priority { get; }
    void Interact();
    void OnHoverEnter();
    void OnHoverExit();
}
