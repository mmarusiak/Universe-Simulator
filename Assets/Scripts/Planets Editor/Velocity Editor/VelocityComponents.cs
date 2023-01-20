using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class VelocityComponents : MonoBehaviour
{
    public VectorsController VectorsController;
    [SerializeField]
    private InputField xField, yField;
    [SerializeField] private Text magnitudeText;

    private bool updatedOnChange = true;
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
                updatedOnReset = false;

                // update Texts
                xField.text = VectorsController.Velocity.x.ToString(CultureInfo.InvariantCulture);
                yField.text = VectorsController.Velocity.y.ToString(CultureInfo.InvariantCulture);
                magnitudeText.text = VectorsController.Velocity.magnitude.ToString(CultureInfo.InvariantCulture);
            }

            // paused
            else if (GravityObjectsController.Instance.Reseted && !updatedOnReset)
            { 
                updatedOnReset = true; // dont look at this loop :)
                while (!VectorsController.Ready) { }
                xField.text = GlobalVariables.Instance.CurrentGravityObject.InitialVelocity.x.ToString(CultureInfo
                    .InvariantCulture);
                yField.text = GlobalVariables.Instance.CurrentGravityObject.InitialVelocity.y.ToString(CultureInfo
                    .InvariantCulture);
                magnitudeText.text = GlobalVariables.Instance.CurrentGravityObject.InitialVelocity.magnitude.ToString(CultureInfo
                    .InvariantCulture);
            }
            
        }
    }

    public void SubmitValue(int type)
    {
        if (Time.timeScale == 0)
        {
            
            float valueX = float.Parse(xField.text, CultureInfo.InvariantCulture.NumberFormat);

            if(!GravityObjectsController.Instance.Reseted)
            { 
                GlobalVariables.Instance.CurrentGravityObject.GetComponent<Rigidbody2D>().velocity =
                    new Vector2(valueX, GlobalVariables.Instance.CurrentGravityObject.GetComponent<Rigidbody2D>().velocity.y);
            }

            GlobalVariables.Instance.CurrentGravityObject.InitialVelocity.x = valueX;
            
            float valueY = float.Parse(yField.text, CultureInfo.InvariantCulture.NumberFormat);

            if(!GravityObjectsController.Instance.Reseted) 
            { 
                GlobalVariables.Instance.CurrentGravityObject.GetComponent<Rigidbody2D>().velocity = 
                    new Vector2(GlobalVariables.Instance.CurrentGravityObject.GetComponent<Rigidbody2D>().velocity.x, valueY);
                        
            }
                    
            GlobalVariables.Instance.CurrentGravityObject.InitialVelocity.y = valueY;
            
        }
        
        if(GravityObjectsController.Instance.Reseted) 
            GlobalVariables.Instance.CurrentGravityObject.UpdatePlanet(); 
        
        updatedOnChange = false;
    } 
}


