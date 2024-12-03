using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems; // Required for EventTrigger

public class UpgradeChoiceMenu : MonoBehaviour
{
    public static UpgradeChoiceMenu instance;

    public GameObject choicePanel;        // Panel that contains the upgrade choices
    public Button choiceOrbital;          // Button for Orbital Wep
    public Button choiceBoomerang;        // Button for Boomerang Wep
    public Button choiceMeteor;           // Button for Meteor Wep
    
    // Description Texts for each button
    public TMP_Text descriptionOrbital;      // Text for Orbital description
    public TMP_Text descriptionBoomerang;    // Text for Boomerang description
    public TMP_Text descriptionMeteor;       // Text for Meteor description

    private void Awake()
    {
        // Ensure only one instance exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Keep across scenes
        }
        else
        {
            Debug.LogWarning("Multiple instances of UpgradeChoiceMenu detected, destroying new instance.");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Initially hide the choice panel and description texts
        choicePanel.SetActive(false);
        descriptionOrbital.gameObject.SetActive(false);
        descriptionBoomerang.gameObject.SetActive(false);
        descriptionMeteor.gameObject.SetActive(false);

        // Attach listeners to each button to handle the upgrades
        choiceOrbital.onClick.AddListener(() => ChooseUpgrade(1));
        choiceBoomerang.onClick.AddListener(() => ChooseUpgrade(2));
        choiceMeteor.onClick.AddListener(() => ChooseUpgrade(3));

        // Add listeners for hover effects
        AddButtonHoverListeners();
    }

    // Add listeners to buttons for hover effect
    private void AddButtonHoverListeners()
    {
        EventTrigger triggerOrbital = choiceOrbital.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entryOrbitalEnter = new EventTrigger.Entry();
        entryOrbitalEnter.eventID = EventTriggerType.PointerEnter;
        entryOrbitalEnter.callback.AddListener((data) => ShowDescription(1));  // Show description for Orbital
        triggerOrbital.triggers.Add(entryOrbitalEnter);

        EventTrigger.Entry entryOrbitalExit = new EventTrigger.Entry();
        entryOrbitalExit.eventID = EventTriggerType.PointerExit;
        entryOrbitalExit.callback.AddListener((data) => HideDescription());
        triggerOrbital.triggers.Add(entryOrbitalExit);

        EventTrigger triggerBoomerang = choiceBoomerang.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entryBoomerangEnter = new EventTrigger.Entry();
        entryBoomerangEnter.eventID = EventTriggerType.PointerEnter;
        entryBoomerangEnter.callback.AddListener((data) => ShowDescription(2));  // Show description for Boomerang
        triggerBoomerang.triggers.Add(entryBoomerangEnter);

        EventTrigger.Entry entryBoomerangExit = new EventTrigger.Entry();
        entryBoomerangExit.eventID = EventTriggerType.PointerExit;
        entryBoomerangExit.callback.AddListener((data) => HideDescription());
        triggerBoomerang.triggers.Add(entryBoomerangExit);

        EventTrigger triggerMeteor = choiceMeteor.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entryMeteorEnter = new EventTrigger.Entry();
        entryMeteorEnter.eventID = EventTriggerType.PointerEnter;
        entryMeteorEnter.callback.AddListener((data) => ShowDescription(3));  // Show description for Meteor
        triggerMeteor.triggers.Add(entryMeteorEnter);

        EventTrigger.Entry entryMeteorExit = new EventTrigger.Entry();
        entryMeteorExit.eventID = EventTriggerType.PointerExit;
        entryMeteorExit.callback.AddListener((data) => HideDescription());
        triggerMeteor.triggers.Add(entryMeteorExit);
    }

    // Show the appropriate description text
    private void ShowDescription(int buttonIndex)
    {
        // Hide all descriptions first
        descriptionOrbital.gameObject.SetActive(false);
        descriptionBoomerang.gameObject.SetActive(false);
        descriptionMeteor.gameObject.SetActive(false);

        // Show the description based on the button index
        switch (buttonIndex)
        {
            case 1:
                descriptionOrbital.gameObject.SetActive(true);  // Show Orbital description
                break;
            case 2:
                descriptionBoomerang.gameObject.SetActive(true);  // Show Boomerang description
                break;
            case 3:
                descriptionMeteor.gameObject.SetActive(true);  // Show Meteor description
                break;
        }
    }

    // Hide all description texts
    private void HideDescription()
    {
        descriptionOrbital.gameObject.SetActive(false);
        descriptionBoomerang.gameObject.SetActive(false);
        descriptionMeteor.gameObject.SetActive(false);
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
                WeaponManager.instance.OrbitalUpgrade(); 
                break;
            case 2:
                WeaponManager.instance.BoomerangUpgrade();  
                break;
            case 3:
                WeaponManager.instance.MeteorUpgrade();
                break;
        }

        // Hide the menu and resume the game
        HideUpgradeMenu();
    }
}
