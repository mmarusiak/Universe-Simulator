using UnityEngine;

public class ComponentField : MonoBehaviour
{
    [SerializeField]
    private InputFieldEvent _submitCall;
    
    public void Submit(string value)
    {
        _submitCall.Invoke(value);
    }
}
