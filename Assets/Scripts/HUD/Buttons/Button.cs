using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Linq;

public class Button : MonoBehaviour, 
    IPointerClickHandler, 
    IPointerEnterHandler,
    IPointerExitHandler
{
    private Image _background;
    protected TextMeshProUGUI _text;

    [SerializeField]
    private Color _backgroundColor = Color.red;
    [SerializeField]
    private Color _textFontColor = Color.blue;
    [SerializeField]
    private Color _backgroundColorOnClick = Color.green;
    [SerializeField]
    private Color _textFontColorOnClick = Color.cyan;
    [SerializeField]
    private Color _backgroundColorOnMouseOver = Color.green;
    [SerializeField]
    private Color _textFontColorOnMouseOver = Color.cyan;


    // Start is called before the first frame update
    void Start()
    {
        Initialization();
    }

    /// <summary>
    /// Initialization of the component
    /// Get all components in children
    /// </summary>
    protected virtual void Initialization()
    {
        this._background = GetComponentsInChildren<Image>().FirstOrDefault(x => x.name == "Background");
        this._text = GetComponentsInChildren<TextMeshProUGUI>().FirstOrDefault(x => x.name == "Text");

        if(this._background != null)
        {
            this._background.color = this._backgroundColor;
        }
        if(this._text != null)
        {
            this._text.color = this._textFontColor;
        }
    }

    #region Mouse Actions
    public void OnPointerClick(PointerEventData eventData)
    {
        print("OnPointerClick");
        OnPointClickVirtual();
    }
    protected virtual void OnPointClickVirtual()
    {
        this._background.color = this._backgroundColorOnClick;
        this._text.color = this._textFontColorOnClick;
        OnClick();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        print("OnPointerEnter");
        OnPointerEnterVirtual();
    }
    protected virtual void OnPointerEnterVirtual()
    {
        this._background.color = this._backgroundColorOnMouseOver;
        this._text.color = this._textFontColorOnMouseOver;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        print("OnPointerExit");
        OnPointerExitVirtual();
    }

    protected virtual void OnPointerExitVirtual()
    {
        this._background.color = this._backgroundColor;
        this._text.color = this._textFontColor;
    }
    #endregion

    /// <summary>
    /// Function to implement for the action of the button
    /// </summary>
    protected virtual void OnClick()
    {
        throw new Exception("OnClick");
    }
}
