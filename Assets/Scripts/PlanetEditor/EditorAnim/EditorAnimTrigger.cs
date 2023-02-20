using UnityEngine;

public class EditorAnimTrigger : MonoBehaviour
{
    [SerializeField] private RectTransform _container;
    [SerializeField] private Vector2 _startPos, _endPos;
    [SerializeField] private float _time = 0.02f;
    private Vector2 _targetPos;
    void Start() => ChangePos(true);

    void Update() => _container.localPosition = Vector2.Lerp(_container.localPosition, _targetPos, _time);
    

    public void ChangePos(bool state)
    {
        Vector2 pos = state ? _startPos : _endPos;
        Vector2 currentPos = _container.localPosition;
        if (currentPos == pos)
            return;
        _targetPos = pos;
    }
    
}
