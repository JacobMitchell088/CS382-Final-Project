using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UpgradeChoiceMenu : MonoBehaviour
{
    public static UpgradeChoiceMenu instance;


    public GameObject choicePanel;        // Panel that contains the upgrade choices
    public Button choiceOrbital;          // Button for Orbital Wep
    public Button choiceBoomerang;          // Button for Boomerang Wep
    public Button choiceMeteor;          // Button for Meteor Wep

    //private bool upgradeChosen = false;   // Check if an upgrade has been selected

    private void Start()
    {
        // Initially hide the choice panel
        choicePanel.SetActive(false);

        // Attach listeners to each button to handle the upgrades
        choiceOrbital.onClick.AddListener(() => ChooseUpgrade(1));
        choiceBoomerang.onClick.AddListener(() => ChooseUpgrade(2));
        choiceMeteor.onClick.AddListener(() => ChooseUpgrade(3));
    }

    private void Awake()
    {
        // Ensure only one instance exists
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {

    }

    // Show the choice panel and pause the game
    public void ShowUpgradeMenu()
    {
        choicePanel.SetActive(true);
        Time.timeScale = 0;  // Pause the game
    }

    // Hide the choice panel and unpause the game
    private void HideUpgradeMenu()
    {
        choicePanel.SetActive(false);
        Time.timeScale = 1;  // Resume the game
    }

    // Handle the choice the player makes
    private void ChooseUpgrade(int choice)
    {
        switch (choice)
        {
            case 1:
                ApplyUpgrade();  // Apply the specific upgrade for choice 1
                break;
            case 2:
                ApplyUpgrade();  // Apply the specific upgrade for choice 2
                break;
            case 3:
                ApplyUpgrade();  // Apply the specific upgrade for choice 3
                break;
        }

        //upgradeChosen = true;
        HideUpgradeMenu();
    }

    // Example upgrade application - divide weapon spawn timer by 2
    private void ApplyUpgrade()
    {
        // Implement logic to halve weapon spawn timer
        Debug.Log("Upgrade applied: Weapon spawn timers reduced.");
    }

}
