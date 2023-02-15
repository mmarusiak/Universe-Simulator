using UnityEngine;

public class PlanetEditor : MonoBehaviour
{
    [SerializeField]
    private EditorBase _editorBase;
    public EditorBase EditorBase => _editorBase;

    void Start() => Show(false);
    public void Show(bool targetState) => EditorBase.Shown = targetState;
}
