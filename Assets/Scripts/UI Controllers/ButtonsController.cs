using UnityEngine;

public class ButtonsController : MonoBehaviour
{
    public void PlayPause() => GravityObjectsController.Instance.PlayPause();

    public void ResetScene() => GravityObjectsController.Instance.ResetScene();

    public void LineCheck() => GravityObjectsController.Instance.LineCheck();
}
