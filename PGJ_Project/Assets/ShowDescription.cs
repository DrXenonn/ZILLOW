using UnityEngine;
using UnityEngine.EventSystems;

public class ShowDescription : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject Description;
    private bool _isHovering;

    private void Update()
    {
        if (_isHovering)
        {
            Description.transform.position = new Vector3(Input.mousePosition.x + 80f, Input.mousePosition.y - 30f);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isHovering = true;
        Description.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isHovering = false;
        Description.SetActive(false);
    }
    
}