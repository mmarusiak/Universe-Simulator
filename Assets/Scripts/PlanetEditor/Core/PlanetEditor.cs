using UnityEngine;

/// <summary>
/// Base class for all editors.
/// </summary>
public class PlanetEditor : MonoBehaviour
{
    [SerializeField]
    private EditorBase _editorBase;
    public EditorBase EditorBase => _editorBase;

    private void Start() => Show(false);
    public void Show(bool targetState) => EditorBase.Shown = targetState;
}
