using UnityEngine;

using UnityEngine.UI;

public class ComponentField : MonoBehaviour
{
    [SerializeField]
    private InputFieldEvent _submitCall;
    
    public void Submit(InputField field)
    {
        _submitCall.Invoke(field);
    }
}
