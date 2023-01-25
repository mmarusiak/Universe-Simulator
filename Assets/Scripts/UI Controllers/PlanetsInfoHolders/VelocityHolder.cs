using UnityEngine;
using UnityEngine.UI;

public class VelocityHolder : MonoBehaviour
{
    private Rigidbody2D rigid;
    private Text txt;

    public void SetRigid(Rigidbody2D nrigid) => rigid = nrigid;
    
    // Start is called before the first frame update
    void Start()
    {
        txt = GetComponent<Text>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float velocity = rigid.velocity.magnitude;
        txt.text = $"({velocity*10} km/h)";
    }
}
