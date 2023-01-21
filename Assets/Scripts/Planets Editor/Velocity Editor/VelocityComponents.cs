using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class VelocityComponents : MonoBehaviour
{
    public VectorsController VectorsController;
    [SerializeField]
    private InputField xField, yField;
    [SerializeField] private Text magnitudeText;

    private bool updatedOnChange = false;
    private bool updatedOnReset = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (VectorsController.VectorsShown)
        {
            xField.interactable = Time.timeScale == 0;
            yField.interactable = Time.timeScale == 0;

            // unpaused
            if (Time.timeScale > 0)
            {
                updatedOnChange = false;
                updatedOnReset = false;
                
                UpdateTexts(VectorsController.Velocity);
            }

            // paused
            else if (GravityObjectsController.Instance.Reseted && !updatedOnReset)
            {
                updatedOnChange = false;
                updatedOnReset = true; 
                
                // dont look at this loop :)
                while (!VectorsController.Ready) { }
                
                UpdateTexts(GlobalVariables.Instance.CurrentGravityObject.InitialVelocity);
            }
            else if (Time.timeScale == 0 && !updatedOnChange)
            {
                updatedOnChange = true;
                
                UpdateTexts(VectorsController.Velocity);
            }

        }
    }

    void UpdateTexts(Vector3 vel)
    {
        xField.text = vel.x.ToString(CultureInfo.InvariantCulture);
        yField.text = vel.y.ToString(CultureInfo.InvariantCulture);
        magnitudeText.text = vel.magnitude.ToString(CultureInfo.InvariantCulture);
    }

    public void SubmitValue()
    {
        if (Time.timeScale == 0)
        {
            
            float valueX = float.Parse(xField.text, CultureInfo.InvariantCulture.NumberFormat);
            float valueY = float.Parse(yField.text, CultureInfo.InvariantCulture.NumberFormat);

            if (GravityObjectsController.Instance.Reseted)
            {
                GlobalVariables.Instance.CurrentGravityObject.InitialVelocity.x = valueX;
                GlobalVariables.Instance.CurrentGravityObject.InitialVelocity.y = valueY;
            }
            
            GlobalVariables.Instance.CurrentGravityObject.GetComponent<Rigidbody2D>().velocity =
                new Vector2(valueX, valueY);

        }
    } 
}


