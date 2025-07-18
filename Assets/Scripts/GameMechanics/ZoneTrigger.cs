using System;
using UnityEngine;

public class ZoneTrigger : MonoBehaviour
{
    public static event Action PlayerEntered;
    public static event Action PlayerLeft;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.TryGetComponent<PlayerController>(out _))
        {
            PlayerEntered?.Invoke();
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.TryGetComponent<PlayerController>(out _))
        {
            PlayerLeft?.Invoke();
        }
    }
}
