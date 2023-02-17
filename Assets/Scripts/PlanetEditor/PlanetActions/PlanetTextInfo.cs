using UnityEngine;
using UnityEngine.UI;

public class PlanetTextInfo : MonoBehaviour
{
    [Header("Left Side + Value + RightSide")]
    [SerializeField] [Range(1, 0)] private float _smoothSpeed = 0.02f;
    [SerializeField] private Text _targetOutput;
    [SerializeField] private string _leftSide, _rightSide;
    [SerializeField] private Vector2 _textSize = new (600, 0);
    [SerializeField] private int _fontSize = 20;
    [SerializeField] private Sprite _icon;
    [SerializeField] private float _xDistanceBetweenIconAndText = 20.0f;
    private static float _yOffsetBetweenTexts = 1f;
    private RectTransform _textTransform;
    private Vector2 _targetPos;
    private Vector2 _iconOffset = Vector2.zero;

    void Start()
    { 
        if (_textTransform != null) return;
        Initialize();
    }

    void Initialize()
    {
        var text = CreateNewText();
        if (_icon == null) return;
        CreateIcon(text);
    }

    GameObject CreateNewText()
    {
        var textHolder = new GameObject(transform.parent.name);
        _targetOutput = textHolder.AddComponent<Text>();
        _targetOutput.color = Color.white;
        _targetOutput.font = GlobalVariables.Instance.GlobalFont;
        _targetOutput.fontSize = _fontSize;
        _targetOutput.alignment = TextAnchor.MiddleLeft;
        _targetOutput.fontStyle = FontStyle.BoldAndItalic;

        _textTransform = _targetOutput.GetComponent<RectTransform>();
        _textSize = new(_textSize.x, Mathf.Ceil(_fontSize * 1.35f));
        _textTransform.sizeDelta = _textSize;

        textHolder.transform.SetParent(GameObject.Find("PlanetsUI").transform);
        textHolder.transform.localScale = Vector3.one;
        return textHolder;
    }

    void CreateIcon(GameObject holder)
    {
        var iconHolder = new GameObject(transform.parent.name + "Icon");
        var img = iconHolder.AddComponent<Image>();
        img.sprite = _icon;
        iconHolder.transform.SetParent(holder.transform);
        iconHolder.GetComponent<RectTransform>().sizeDelta = new Vector2(_textSize.y, _textSize.y);
        iconHolder.GetComponent<RectTransform>().position = new Vector3(-_textSize.x/2 - iconHolder.GetComponent<RectTransform>().sizeDelta.x/2 - _xDistanceBetweenIconAndText, 0, 0);
        _iconOffset = new Vector2(iconHolder.GetComponent<RectTransform>().sizeDelta.x + _xDistanceBetweenIconAndText, 0);
    }
    
    void LateUpdate()
    {
        // calculating target pos - each next text container should have position of y + 1 - that will make them to display one row below
        // f.e. name holder y = 0 and velocity holder y = 1 -> that makes displaying name text in correct position, and one row below name holder velocity text
        _targetPos = Camera.main.WorldToScreenPoint(transform.parent.position - new Vector3
                         (0,  transform.parent.localScale.y + 3.0f * transform.localPosition.y * _textTransform.lossyScale.y + _yOffsetBetweenTexts * transform.localPosition.y)) 
                     + new Vector3(_textSize.x / 2, 0, 0) + (Vector3)_iconOffset ;

        if(!PlaybackController.Instance.Playback.IsPaused) StrictFollow();
        else SmoothFollow();
    }

    void StrictFollow() => _textTransform.position = _targetPos;
    

    void SmoothFollow()
    {
        Vector3 smoothFollow = Vector3.Lerp(_textTransform.position,_targetPos, _smoothSpeed);
        _textTransform.position = smoothFollow;
    }
    
    public void ChangeValue(string value)
    {
        if (_targetOutput == null) Initialize();
        _targetOutput.gameObject.name = value;
        _targetOutput.text = _leftSide + value + _rightSide;
    }

    void OnDestroy()
    {
        Destroy(_targetOutput.gameObject);
    }
}
