using UnityEngine;

public class AntiDarknessPotion : MonoBehaviour
{
    [SerializeField] private GameEvent OnAntiDarkness;
    [SerializeField] private float Time;

    private void Start()
    {
        OnAntiDarkness.Raise(Time);
        Destroy(gameObject);
    }
}