using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_energyDrink : MonoBehaviour
{
    public struct EnergyDrink
    {
        private int power;
        private float time;

        public EnergyDrink(int power, float time)
        {
            this.power = power;
            this.time = time;
        }

        public int getPower()
        {
            return power;
        }
        
        public float getTime()
        {
            return time;
        }
    }
    public EnergyDrink energyDrink;
    public float time = 10;
    // Start is called before the first frame update
    void Start()
    {
        energyDrink = new EnergyDrink(2, time);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
