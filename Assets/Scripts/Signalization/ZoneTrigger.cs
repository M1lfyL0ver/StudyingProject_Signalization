using System;
using UnityEngine;

public class ZoneTrigger : MonoBehaviour
{
    public event Action PlayerEntered;
    public event Action PlayerLeft;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.TryGetComponent<PlayerMover>(out _))
        {
            PlayerEntered?.Invoke();
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.TryGetComponent<PlayerMover>(out _))
        {
            PlayerLeft?.Invoke();
        }
    }
}
