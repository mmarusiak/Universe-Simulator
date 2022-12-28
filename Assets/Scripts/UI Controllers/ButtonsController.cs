using UnityEngine;

public class ButtonsController : MonoBehaviour
{
    private GravityObjectsController _controller;

    void Start() => _controller = GameObject.FindWithTag("GravityController").GetComponent<GravityObjectsController>();
    
    public void PlayPause() => _controller.PlayPause();

    public void ResetScene() => _controller.ResetScene();

    public void LineCheck() => _controller.LineCheck();
}
