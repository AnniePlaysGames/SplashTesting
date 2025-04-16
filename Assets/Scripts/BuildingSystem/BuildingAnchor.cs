using UnityEngine;

public class BuildingAnchor : MonoBehaviour
{
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(transform.position, 0.05f);

        // Можно добавить стрелку вверх — визуально помогает ориентироваться
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.up * 0.2f);
    }
#endif
}