using UnityEngine;

public class Transitions : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private float timeStamp = .01f, timeIndex = 0, time, scalePerStamp;
    public void Scale(float time)
    {
        timeIndex = 0;
        gameObject.GetComponent<RectTransform>().localScale = Vector3.zero + new Vector3(0,0,1);
        this.time = time;
        scalePerStamp = 1 / (time / timeStamp);
        Invoke(nameof(StartScale), timeStamp);
    }

    public void StartScale()
    {
        Debug.Log(time + " " + timeIndex);
        if (timeIndex < time)
        {
            timeIndex += timeStamp;
            gameObject.GetComponent<RectTransform>().localScale += new Vector3(scalePerStamp, scalePerStamp);
            Invoke("StartScale", timeStamp);
        }
    }
}
