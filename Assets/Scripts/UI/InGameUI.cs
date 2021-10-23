using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private GrowableImageContainer inventoryGrowableImageContainer = null;
    [SerializeField] private GrowableImageContainer alarmsGrowableImageContainer = null;
    [SerializeField] private Image snakeFaceImage = null;

    [SerializeField] private Texture2D alarmSpritesheet = null;
    [SerializeField] private Texture2D snakeFacesSpritesheet = null;
    [SerializeField] private Sprite keycardTexture = null;

    private Rect alarmSpriteRect = new Rect(0, 8, 8, 8);
    private Rect alarmDetectedSpriteRect = new Rect(0, 0, 8, 8);
    private Sprite alarmSprite = null;
    private Sprite alarmDetectedSprite = null;

    private Rect snakeNormalRect = new Rect(0, 32, 16, 16);
    private Rect snakeTiredRect = new Rect(16, 32, 16, 16);
    private Rect snakeHurtRect = new Rect(0, 16, 16, 16);
    private Rect snakeInjuredRect = new Rect(16, 16, 16, 16);
    private Rect snakeColdRect = new Rect(0, 0, 16, 16);
    private Sprite snakeNormalSprite = null;
    private Sprite snakeTiredSprite = null;
    private Sprite snakeHurtSprite = null;
    private Sprite snakeInjuredSprite = null;
    private Sprite snakeColdSprite = null;

    public enum SnakeStatus
    {
        Normal,
        Tired,
        Hurt,
        Injured,
        Cold
    };

    public void SetSnakeStatus(SnakeStatus status)
    {
        switch(status)
        {
            case SnakeStatus.Normal:
                snakeFaceImage.sprite = snakeNormalSprite;
                break;

            case SnakeStatus.Tired:
                snakeFaceImage.sprite = snakeTiredSprite;
                break;

            case SnakeStatus.Hurt:
                snakeFaceImage.sprite = snakeHurtSprite;
                break;

            case SnakeStatus.Injured:
                snakeFaceImage.sprite = snakeInjuredSprite;
                break;

            case SnakeStatus.Cold:
                snakeFaceImage.sprite = snakeColdSprite;
                break;
        }
    }

    public void OnKeycardPickedUp(KeycardScript keycard)
    {
        // Check the inventory image container has been set
        if(inventoryGrowableImageContainer)
        {
            // Add an image to the inventory container
            GameObject imageObject = inventoryGrowableImageContainer.AddImage();

            // Get the image component from the image
            var image = imageObject.GetComponent<Image>();

            // Check a keycard texture has been set
            if(!keycardTexture)
            {
                Debug.LogError("A keycard texture has not been set on the InGameUI instance");
                return;
            }

            // Set the texture of the image to the neutral keycard texture
            image.sprite = keycardTexture;

            // Check the image is valid
            if (image)
            {
                // Set the image color to the color of the keycard
                switch(keycard.GetKeycardColor())
                {
                    case KeycardScript.KeycardColor.Blue: image.color = Color.blue; break;
                    case KeycardScript.KeycardColor.Orange: image.color = new Color(0.255f * 6.0f, 0.165f * 6.0f, 0.0f); break;
                }
            }
        }
    }

    public void OnPlayerDetected()
    {
        var detectedCount = GameController.Instance.playerDetectedCount - 1;
        // Check the player detected count is less than the number of alarm images in the container
        if(detectedCount < alarmsGrowableImageContainer.GetImageCount())
        {
            // Set the alarm for the number of times the player is detected to the detected sprite
            alarmsGrowableImageContainer.SetImageTo(alarmDetectedSprite, detectedCount);
        }
    }

    private void CreateAlarmSprites()
    {
        // Check an alarm spritesheet texture has been set
        if (alarmSpritesheet)
        {
            // Create alarm sprites
            alarmSprite = Sprite.Create(alarmSpritesheet, alarmSpriteRect, new Vector2(0, 0));
            alarmDetectedSprite = Sprite.Create(alarmSpritesheet, alarmDetectedSpriteRect, new Vector2(0, 0));

            // Set the default alarm image
            alarmsGrowableImageContainer.SetAllImagesTo(alarmSprite);
        }
    }

    private void CreateSnakeFaceSprites()
    {
        // Check a snake face spritesheet texture has been set
        if(snakeFacesSpritesheet)
        {
            // Create sprites for each state
            snakeNormalSprite = Sprite.Create(snakeFacesSpritesheet, snakeNormalRect, new Vector2(0, 0));
            snakeTiredSprite = Sprite.Create(snakeFacesSpritesheet, snakeTiredRect, new Vector2(0, 0));
            snakeHurtSprite = Sprite.Create(snakeFacesSpritesheet, snakeHurtRect, new Vector2(0, 0));
            snakeInjuredSprite = Sprite.Create(snakeFacesSpritesheet, snakeInjuredRect, new Vector2(0, 0));
            snakeColdSprite = Sprite.Create(snakeFacesSpritesheet, snakeColdRect, new Vector2(0, 0));

            // Set the default status
            SetSnakeStatus(SnakeStatus.Normal);
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        CreateAlarmSprites();
        CreateSnakeFaceSprites();
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
