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
    private RectTransform _textTransform;
    private Vector2 _targetPos;

    void Start()
    {
       CreateNewText();
    }

    void CreateNewText()
    {
        var nameHolder = new GameObject(transform.parent.name);
        _targetOutput = nameHolder.AddComponent<Text>();
        _targetOutput.color = Color.white;
        _targetOutput.font = GlobalVariables.Instance.GlobalFont;
        _targetOutput.fontSize = _fontSize;
        _targetOutput.alignment = TextAnchor.LowerLeft;
        _targetOutput.fontStyle = FontStyle.BoldAndItalic;

        _textTransform = _targetOutput.GetComponent<RectTransform>();
        _textSize = new(_textSize.x, Mathf.Ceil(_fontSize * 1.35f));
        _textTransform.sizeDelta = _textSize;

        nameHolder.transform.SetParent(GameObject.Find("PlanetsUI").transform);
        nameHolder.transform.localScale = Vector3.one;
    }

    void LateUpdate()
    {
        // calculating target pos - each next text container should have position of y + 1 - that will make them to display one row below
        // f.e. name holder y = 0 and velocity holder y = 1 -> that makes displaying name text in correct position, and one row below name holder velocity text
        _targetPos = Camera.main.WorldToScreenPoint(transform.parent.position 
                                                    - new Vector3(0,  transform.parent.localScale.y + 3.0f * transform.localPosition.y * _textTransform.lossyScale.y)) 
                     + new Vector3(_textSize.x / 2, 0, 0);

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
        _targetOutput.gameObject.name = value;
        _targetOutput.text = _leftSide + value + _rightSide;
    }

    void OnDestroy()
    {
        Destroy(_targetOutput.gameObject);
    }
}
