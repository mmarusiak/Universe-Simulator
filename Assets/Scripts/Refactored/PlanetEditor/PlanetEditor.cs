using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetEditor : MonoBehaviour
{
    [SerializeField]
    private EditorWindowBase _myBase;
    public EditorWindowBase MyBase
    {
        get => _myBase;
        set => _myBase = value;
    }
}
