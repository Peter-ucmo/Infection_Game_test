using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int maxCureCharges = 5;
    int cureCharges = 5;
    int cureChargeProgress = 0; //In-Between from having no charge and having a fully usable charge -- +1 per enemy hit(?)
    int newCureChargeThreshold = 25;  //Number cureChargeProgress must reach before being fully charged

    int maxHealth = 100;
    int health = 100;
    float healthDegradationRate = 0.5f;  //In health per second
    bool isSafeFromDegradation = false;

    void Start()
    {
        if (!isSafeFromDegradation) 
        {
            //Use reciprocal of healthDegradationRate for seconds per health
            InvokeRepeating("degradeHealth", 0.5f, (1 / healthDegradationRate));
        }
    }

    void Update()
    {

    }

    void degradeHealth()
    {
        if (!isSafeFromDegradation)
        {
            addHealth(-1);
        }
    }

    public void addHealth(int amount)
    {
        if (health + amount >= maxHealth) { health = maxHealth; }
        else { health += amount; }
    }

    public void addCureCharges(int amount)
    {
        if (cureCharges + amount >= maxCureCharges) { maxCureCharges = maxCureCharges; }
        else { cureCharges += amount; }
    }

    public void setIsSafeFromDegradation(bool state)
    {
        isSafeFromDegradation = state;
    }

    //Debug UI, switch later when we have proper assets
    private void OnGUI()
    {
        GUI.Label(
           new Rect(5, 0, 300f, 150f),
           "Health: " + health.ToString() + "\n" +
           "Cure Charges: " + cureCharges.ToString() + "\n\n" + 
           "Safe From Infection?: " + isSafeFromDegradation.ToString());
    }
}
