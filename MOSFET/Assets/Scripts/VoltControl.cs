using UnityEngine;
using UnityEngine.UI;

public class VoltControl : MonoBehaviour
{
    float Vgs, Vds, Id, Vt;
    public float speedFactor, rotationFactor;
    public Slider DrainCurrent;
    public Text region, vdsat, description;
    const float k = 1f;
    public Animator Current;
    public GameObject Channel;

    // Start is called before the first frame update
    void Start()
    {
        Vt = 1;
        Id = 0;
    }


    public void GateToSource(float value)
    {
        Vgs = value;
    }

    public void DrainToSource(float value)
    {
        Vds = value;
    }


    private void Update()
    {
        DrainCurrent.value = Id;
        rotationFactor = -Id / (Vgs) * 10;
        speedFactor = Id;
        

        if(Id != 0)
        {
            Channel.transform.rotation = Quaternion.Euler(180, rotationFactor, 0);
            Current.speed = speedFactor;
            Current.gameObject.SetActive(true);
        }
        else
        {
            Channel.transform.rotation = Quaternion.Euler(180, 0, 0);
            Current.gameObject.SetActive(false);
        }

        if(Vgs > Vt)
        {
            Channel.SetActive(true);
        }
        else
        {
            Channel.SetActive(false);
        }


        if (Vds >= (Vgs - Vt - 0.01f) && Vds <= (Vgs - Vt + 0.01f))
        {
            // Pinch-Off Region
            region.text = "Pinch-Off Region";
            region.color = Color.red;
            description.text = "Vds = Vds,sat \n The Channel is highly constricted \n Drain Current (Id) becomes constant";
            vdsat.enabled = true;
        }
        else if (Vgs <= Vt)
        {
            // Cut-Off Region
            Id = 0;
            region.text = "Cut-Off Region";
            region.color = Color.white;
            description.text = "Vgs < Vt \n Channel has not been formed \n No flow of Drain Current (Id)";
            vdsat.enabled = false;
        }
        else if (Vds > (Vgs - Vt))
        {
            // Saturation Region
            Id = k * ((Vgs - Vt) * (Vgs - Vt));
            region.color = Color.white;
            region.text = "Saturation Region";
            description.text = "Vds > (Vgs - Vt) [Overdrive voltage] \n The Channel remains highly constricted \n Drain Current (Id) is constant, irrespective of Vds";
            vdsat.enabled = true;
        }
        else if (Vds < (Vgs - Vt))
        {
            // Triode Region
            Id = k * ( (2 * (Vgs - Vt) * Vds) - (Vds * Vds) );
            region.color = Color.white;
            region.text = "Triode Region";
            description.text = "Vds < (Vgs - Vt) [Overdrive Voltage] \n The Channel width changes. \n Drain Current (Id) is linearly dependent on Vds";
            vdsat.enabled = true;
        }
        else
        {
            description.text = "Vgs = Gate-to-Source Voltage \n Vds = Drain-to-Source Voltage \n Id = Drain Current \n Vt = Threshold Voltage";
        }

    }
}
