using UnityEngine;

public class TargetComponent : MonoBehaviour
{
    public System.Action OnHit;
    public void Hit()
    {
        OnHit?.Invoke();
    }
}