using UnityEngine;
using UnityEngine.Events;

public class OnColliderEvent : TickBehaviour
{
    [SerializeField] Collider _collider;
    [SerializeField]
    LayerMask layers = -1;
    [SerializeField, ShowIfNot(nameof(IsTrigger))]
    UnityEvent<Collider> onCollisionEnter = new();
    [SerializeField, ShowIf(nameof(IsTrigger))]
    UnityEvent<Collider> onTriggerEnter = new();

    [SerializeField, ShowIfNot(nameof(IsTrigger))]
    UnityEvent<Collider> onCollisionExit = new();
    [SerializeField, ShowIf(nameof(IsTrigger))]
    UnityEvent<Collider> onTriggerExit = new();
    bool IsTrigger()
    {
        return _collider.isTrigger;
    }
    private void OnValidate()
    {
        if (_collider == null)
        {
            TryGetComponent(out _collider);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (_collider == null) return;
        if ((layers.value & 1 << collision.gameObject.layer) != 0)
        {
            onCollisionEnter?.Invoke(collision.collider);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (_collider == null) return;
        if ((layers.value & 1 << collision.gameObject.layer) != 0)
        {
            onCollisionExit?.Invoke(collision.collider);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (_collider == null) return;
        if ((layers.value & 1 << other.gameObject.layer) != 0)
        {
            onTriggerEnter?.Invoke(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_collider == null) return;
        if ((layers.value & 1 << other.gameObject.layer) != 0)
        {
            onTriggerExit?.Invoke(other);
        }
    }
}