using UnityEngine;

public class Web : MonoBehaviour
{
    [SerializeField] private BoolVariable CanMove;
    [SerializeField] private GameObject Visuals;
    [SerializeField] private float StunDuration;
    private bool _hit;
    private GameObject _webEffect;

    private void Start()
    {
        _webEffect = GameObject.FindWithTag("WebEffect");
        Destroy(gameObject, StunDuration + 5);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_hit)
        {
            Visuals.SetActive(false);
            _webEffect.transform.GetChild(0).gameObject.SetActive(true);
            CanMove.Value = false;
            Invoke(nameof(RemoveEffect), StunDuration);
        }
        else if (other.CompareTag("Environment") || other.CompareTag("Ground"))
        {
            Visuals.SetActive(false);
            _hit = true;
        }
    }

    private void RemoveEffect()
    {
        CanMove.Value = true;
        _webEffect.transform.GetChild(0).gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
