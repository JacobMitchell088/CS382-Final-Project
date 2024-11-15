using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image image;              // Reference to the Image component
    public Sprite defaultSprite;     // The sprite when not hovered
    public Sprite hoverSprite;       // The sprite when hovered
    public string sceneName = "Arena"; // Default scene name, can be set in the Inspector
    public  AudioSource clickNoise;  // Reference to the AudioSource component  
    void Start()
    {
        if (image == null)
        {
            image = GetComponent<Image>();
            clickNoise = GetComponent<AudioSource>();
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
            SceneManager.LoadScene(sceneName);
    }
}
