using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

using LightDev.Core;



namespace LightDev.UI
{
  [RequireComponent(typeof(Image))]
  public abstract class BaseButton : Base, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
  {
    public Image target;
    public UnityEvent onClick;

    private Image image;

    public bool IsInteractable() { return image.raycastTarget; }
    public void SetIsInteractable(bool isInteractable) { image.raycastTarget = isInteractable; }

    protected virtual void Awake()
    {
      image = GetComponent<Image>();
      image.raycastTarget = true;
      target.raycastTarget = false;
    }

    protected virtual void OnEnable()
    {
      ResetButton();
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
      onClick?.Invoke();
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
      AnimatePress();
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
      AnimateUnpress();
    }

  

   

    
    protected abstract void AnimatePress();
    protected abstract void AnimateUnpress();
    public abstract void ResetButton();
  }
}
