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

    private GravityObject lastPlanetsVelocity;
    
    // Update is called once per frame
    void Update()
    {
        if (VectorsController.VectorsShown)
        {
            xField.interactable = Time.timeScale == 0;
            yField.interactable = Time.timeScale == 0;

            //if (!xField.isFocused && yField.isFocused)
            //{
                // unpaused
                if (Time.timeScale > 0)
                {
                    updatedOnChange = false;
                    updatedOnReset = false;

                    UpdateTexts(VectorsController.Velocity);
                }

                // paused and reset
                else if (GravityObjectsController.Instance.Reseted && !updatedOnReset)
                {
                    updatedOnChange = false;
                    updatedOnReset = true;

                    // dont look at this loop :)
                    while (!VectorsController.Ready)
                    {
                    }

                    UpdateTexts(GlobalVariables.Instance.CurrentGravityObject.InitialVelocity);
                }
                else if (Time.timeScale == 0 && !updatedOnChange)
                {
                    updatedOnChange = true;

                    UpdateTexts(VectorsController.Velocity);
                }
                else if (lastPlanetsVelocity != GlobalVariables.Instance.CurrentGravityObject) updatedOnChange = false;
            //}
        }
    }

    void UpdateTexts(Vector3 velocity)
    {
        Vector2 vel = new(velocity.x, velocity.y);
        vel.x *= 10;
        vel.y *= 10;
        
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
                GlobalVariables.Instance.CurrentGravityObject.InitialVelocity.x = valueX/10;
                GlobalVariables.Instance.CurrentGravityObject.InitialVelocity.y = valueY/10;
            }
            
            GlobalVariables.Instance.CurrentGravityObject.GetComponent<Rigidbody2D>().velocity =
                new (valueX/10, valueY/10);

        }
    } 
}


