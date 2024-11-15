using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // For UI elements like Slider

public class ExpController : MonoBehaviour
{
    public Slider expSlider; // Reference to the Slider UI
    public int baseValue = 100; // Base starting experience required for level 1
    public int growthFactor = 20; // Determines how fast experience grows
    public float exponent = 1.5f; // Exponent to control the curve of experience growth
    private int currentExp = 0; // Current experience
    private int level = 1; // Player's level

    private void Start()
    {
        if (expSlider != null)
        {
            // Set the slider max value to the max experience and initial value to the current experience
            expSlider.maxValue = CalculateMaxExp();
            expSlider.value = currentExp;
        }
    }

    // Method to add experience
    public void AddExperience(int amount)
    {
        currentExp += amount;

        // Make sure experience doesn't exceed max value
        if (currentExp >= CalculateMaxExp())
        {
            currentExp = CalculateMaxExp();
            LevelUp(); // Level up the player
        }

        // Update the slider value to reflect the current experience
        if (expSlider != null)
        {
            expSlider.value = currentExp;
        }
    }

    // Method to handle level up
    private void LevelUp()
    {
        level++;
        Debug.Log("Level Up! Now at level " + level);

        // Optionally, reset experience or increase maxExp for the next level
        currentExp = 0; // Reset to zero or handle differently for each level
        
        expSlider.maxValue = CalculateMaxExp();
        
        if (UpgradeChoiceMenu.instance != null) {
            UpgradeChoiceMenu.instance.ShowUpgradeMenu(); // Allow the user to select their upgrade
        }
        else {
            Debug.Log("Upgradechoice.instance invalid");
        }
    }

    // Method to calculate the required experience for the next level
    private int CalculateMaxExp()
    {
        // Apply the exponential growth formula
        return baseValue + Mathf.RoundToInt(growthFactor * Mathf.Pow(level, exponent));
    }

    // Method to get the current level (optional)
    public int GetLevel()
    {
        return level;
    }

    // Method to get the current experience (optional)
    public int GetCurrentExp()
    {
        return currentExp;
    }
}
