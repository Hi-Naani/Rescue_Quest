using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAnimationController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler 
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        animator.SetBool("isHighlighted", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetBool("isHighlighted", false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        animator.SetBool("isPressed", true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        animator.SetBool("isPressed", false);
        animator.SetBool("isSelected", true);  // Button remains in selected state after clicking
    }
  
}
