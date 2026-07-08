using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InsanitySystem : MonoBehaviour
{
    public Player player;
    public Opponent opp;
    public InsanityViewCount ivc;
    public BrainUI BUI;

    public AudioSource Scaredpanting;
    public AudioClip panting;

    public Image yellowOutline;
    public enum insanityLevel 
    {   low, 
        medium, 
        high, 
        insane
    };
    public insanityLevel insanity = insanityLevel.low;

    private void Start()
    {
    }
    private void Update()
    {
        BUI.InventoryPanel.SetActive(Input.GetKey(KeyCode.Tab));

        UpdateInsanityFromOpponent();

        if(ivc.count >= 7)
        {
            insanity = insanityLevel.high;
        }

        HandleCurrentOption();
    }
    /// Low → Medium on first sight; Medium → High after the opponent has been watching for 7+ seconds (Pursue state).
    private void UpdateInsanityFromOpponent()
    {
        if (!opp.Spotted) return;

        if (insanity == insanityLevel.low)
        {
            insanity = insanityLevel.medium;
        }

        if (opp.currentState == Opponent.State.Pursue && insanity == insanityLevel.medium)
        {
            insanity = insanityLevel.high;
        }
    }
    public void HandleCurrentOption()
    {
        switch (insanity)
        {
            case insanityLevel.low:
                lowInsanity();
                break;
            case insanityLevel.medium:
                midInsanity();
                break;
            case insanityLevel.high:
                highInsanity();
                break;
            case insanityLevel.insane:
                completeInsanity();
                break;
        }
    }
    public void lowInsanity()
    {
        Debug.Log("Low insanity");

        Color yellowOutlineAlpha = yellowOutline.color;
        yellowOutlineAlpha.a = 0f;
        yellowOutline.color = yellowOutlineAlpha;
    }
    public void midInsanity()
    {
        Debug.Log("Mid insanity");

        Color yellowOutlineAlpha = yellowOutline.color;
        yellowOutlineAlpha.a = 0.015f;
        yellowOutline.color = yellowOutlineAlpha;

        //Scaredpanting.PlayOneShot(panting);
    }
    public void highInsanity()
    {
        Debug.Log("High insanity");
    }
    public void completeInsanity()
    {
        Debug.Log("Complete insanity");
    }
}
