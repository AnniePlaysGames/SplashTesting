using System;
using UnityEngine;

public abstract class BaseInteractable : MonoBehaviour, IInteractable
{
    [field: SerializeField] public int Priority { get; private set; }
    public event Action<bool> OnHoverChangeState;

    public virtual void Interact() 
        => Debug.Log($"Взаимодействуем с {gameObject.name}");

    public virtual void OnHoverEnter() 
        => OnHoverChangeState?.Invoke(true);

    public virtual void OnHoverExit() 
        => OnHoverChangeState?.Invoke(false);
}