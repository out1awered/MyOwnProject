using System.Collections;
using System.Collections.Generic;


using UnityEngine;
using TMPro;

public class MovementTutorialZone : MonoBehaviour
{
    public enum TriggerType
    {
        Start,
        Finish
    }

    public TriggerType triggerType;
    public TMP_Text tutorialText;
    public GameObject startTrigger;

    private bool triggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (triggered)
        {
            return;
        }

        if (other.GetComponentInParent<PlayerMovement>() == null)
        {
            return;
        }

        triggered = true;

        if (triggerType == TriggerType.Start)
        {
            ShowTutorial("WASD - Move");
        }
        else if (triggerType == TriggerType.Finish)
        {
            if (startTrigger != null)
            {
                startTrigger.SetActive(false);
            }

            ShowTutorial(
                "Press W+Space and try to Jump through the block" 
                );
        }
    }

    void ShowTutorial(string message)
    {
        if (tutorialText != null)
        {
            tutorialText.gameObject.SetActive(true);
            tutorialText.text = message;
        }
    }
}


