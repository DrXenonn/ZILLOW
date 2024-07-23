using UnityEngine;

public class ItemMagnet : MonoBehaviour
{
    [SerializeField] private FloatVariable Speed;
    [SerializeField] private GameEvent OnCollectItem;
    [SerializeField] private Item Item;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Magnet"))
        {
            transform.position = Vector3.Lerp(transform.position, other.transform.position, Time.deltaTime * Speed.Value);
        }

        if (other.CompareTag("Collect"))
        {
            OnCollectItem.Raise(Item);
            Destroy(gameObject);
        }
    }
}
