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
        if(attachedTransform.GetComponent<GravityObject>() != GlobalVariables.Instance.CurrentGravityObject && ((!GlobalVariables.Instance.OverlayShown && !GravityObjectsController.Instance.Paused) 
            || lastAssigned != GlobalVariables.Instance.CurrentGravityObject))
            UpdateText();
        else if (attachedTransform.GetComponent<GravityObject>() == GlobalVariables.Instance.CurrentGravityObject || GlobalVariables.Instance.CurrentGravityObject == null)
            GetComponent<RectTransform>().parent.localScale = Vector3.zero;

    }

    void UpdateText()
    {
        GetComponent<RectTransform>().parent.localScale = Vector3.one;
        lastAssigned = GlobalVariables.Instance.CurrentGravityObject;
        txt.text = Vector2.Distance(attachedTransform.position, lastAssigned.gameObject.transform.position) + " km";
        Debug.Log(lastAssigned + " || " + GlobalVariables.Instance.CurrentGravityObject + " || " + txt.text);
    }
}
