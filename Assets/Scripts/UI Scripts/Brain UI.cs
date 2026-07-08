using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrainUI : MonoBehaviour
{
    public GameObject InventoryPanel;

    public Player player;
    public InsanitySystem insanitySystem;

    public GameObject WhiteBrain;
    public GameObject YellowBrain;
    public GameObject RedBrain;

    private void Update()
    {
        UpdateBrain();
    }
    public void UpdateBrain()
    {
        if(insanitySystem.insanity == InsanitySystem.insanityLevel.low)
        {
            WhiteBrain.SetActive(true);
            YellowBrain.SetActive(false);
            RedBrain.SetActive(false);
        }
        if (insanitySystem.insanity == InsanitySystem.insanityLevel.medium)
        {
            WhiteBrain.SetActive(false);
            YellowBrain.SetActive(true);
            RedBrain.SetActive(false);
        }
        if (insanitySystem.insanity == InsanitySystem.insanityLevel.high)
        {
            WhiteBrain.SetActive(false);
            YellowBrain.SetActive(false);
            RedBrain.SetActive(true);
        }
    }
}
