using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowDescription : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject Description;
    public bool ShowDesc = true;
    private bool _isHovering;

    private void Update()
    {
        if (_isHovering)
        {
            Description.transform.position = new Vector3(Input.mousePosition.x + 75f, Input.mousePosition.y - 33f);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!ShowDesc) return;
        
        _isHovering = true;
        Description.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!ShowDesc) return;

        _isHovering = false;
        Description.SetActive(false);
    }
    
}