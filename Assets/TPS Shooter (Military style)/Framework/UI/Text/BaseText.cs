using UnityEngine;
using UnityEngine.UI;

using LightDev.Core;



namespace LightDev.UI
{
  [RequireComponent(typeof(Text))]
  public class BaseText : Base
  {
    protected Text textComponent;

    protected virtual void Awake()
    {
      textComponent = GetComponent<Text>();
    }

    public virtual Text GetTextComponent()
    {
      return textComponent;
    }

    public virtual void SetText(string text)
    {
      textComponent.text = text;
    }

    public virtual void SetText(int text)
    {
      SetText(text.ToString());
    }

    
  }
}
