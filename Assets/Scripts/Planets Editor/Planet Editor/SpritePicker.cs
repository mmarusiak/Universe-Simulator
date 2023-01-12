using UnityEngine;

public class SpritePicker : MonoBehaviour
{
    public void PickSprite()
    {
        GameObject.Find("Preview").GetComponent<PreviewController>().LoadSpriteFromDropdown();
    }
}
