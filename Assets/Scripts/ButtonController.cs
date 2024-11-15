using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image image;              // Reference to the Image component
    public Sprite defaultSprite;     // The sprite when not hovered
    public Sprite hoverSprite;       // The sprite when hovered
    public  AudioSource clickNoise;  // Reference to the AudioSource component  
    public TextMeshProUGUI buttonText;
    
    void Start()
    {
        if (image == null)
        {
            image = GetComponent<Image>();
            clickNoise = GetComponent<AudioSource>();
            buttonText = GetComponentInChildren<TextMeshProUGUI>();
                if (clickNoise != null) {
                    clickNoise.Stop(); // Ensure the audio doesn't play automatically on load
                }

        }

        if (defaultSprite != null)
        {
            image.sprite = defaultSprite;
        }
    }

    // Called when the pointer enters the GameObject (UI Image)
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (image != null && hoverSprite != null)
        {
            image.sprite = hoverSprite;  // Change to the hover sprite
        }
    }

    // Called when the pointer exits the GameObject (UI Image)
    public void OnPointerExit(PointerEventData eventData)
    {
        if (image != null && defaultSprite != null)
        {
            image.sprite = defaultSprite;  // Revert back to the default sprite
        }
    }

    // Called when the button is clicked to load the scene and play sound
    public void onClick()
    {
            clickNoise.Play();
            string sceneToLoad = DetermineSceneBasedOnButtonText(buttonText.text);
            SceneManager.LoadScene(sceneToLoad);
    }

    private string DetermineSceneBasedOnButtonText(string buttonTextContent)
    {
        if (buttonTextContent == "Play" || buttonTextContent == "Try Again?") {
            return "Arena";
        }
        else if (buttonTextContent == "Back to Start") {
            return "Start";
        }
        else {
            Debug.LogWarning("Unknown button text: " + buttonTextContent);
            return null; // Default to the default scene if unknown
        }
    }

}
