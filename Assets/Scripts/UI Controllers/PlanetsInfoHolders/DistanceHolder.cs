using UnityEngine.UI;
using UnityEngine;

public class DistanceHolder : MonoBehaviour
{
    
    private Transform attachedTransform;
    private Text txt;
    private GravityObject lastAssigned = null;

    public void SetTrans(Transform ntrans) => attachedTransform = ntrans;
    // Start is called before the first frame update
    void Start()
    {
        txt = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if((attachedTransform.GetComponent<GravityObject>() != GlobalVariables.Instance.CurrentGravityObject && 
            (!GlobalVariables.Instance.OverlayShown || lastAssigned != GlobalVariables.Instance.CurrentGravityObject) 
            || lastAssigned != GlobalVariables.Instance.CurrentGravityObject && GetComponent<RectTransform>().parent.localScale == Vector3.zero) && GlobalVariables.Instance.CurrentGravityObject != null)
            UpdateText();
        else if (attachedTransform.GetComponent<GravityObject>() == GlobalVariables.Instance.CurrentGravityObject || GlobalVariables.Instance.CurrentGravityObject == null)
            GetComponent<RectTransform>().parent.localScale = Vector3.zero;

    }

    public void UpdateText()
    {
        GetComponent<RectTransform>().parent.localScale = Vector3.one;
        lastAssigned = GlobalVariables.Instance.CurrentGravityObject;
        float dist = Vector2.Distance(attachedTransform.position, lastAssigned.gameObject.transform.position);
        txt.text = $"{UniverseMath.RoundOutput(dist)} km";
    }
}
