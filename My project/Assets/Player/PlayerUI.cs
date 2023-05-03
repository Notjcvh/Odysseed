using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    private PlayerManger playerManger;
    private Canvas canvas;
    public GameObject playerUi;
    [SerializeField] private Image Hud;
    [SerializeField] private int numberOfHearts;
    private Image[] hearts; // the full array of hearts in the game
                            //Hud
    private Sprite[] allHuds;
    //hearts
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;


    public GameObject objectiveHolder;
    public Slider objectiveSlider;
    private float _startSliderValue; 
    public float sliderLerpDuration;
    private Coroutine _displayObjective;
    public bool open;
    public TextMeshProUGUI objectiveText;
    public float textLerpDuration;

    public float sliderMult;
    public float textMult;


    [Header("Block UI")]
    public Slider blockForeground;
    public Slider blockMiddleground;
    public float updateSpeedInSeconds_1;
    public float updateSpeedInSeconds_2;

    private string currentObjective = "";


    public IEnumerator setObjective;
    public bool coroutineRunning;


    public GameObject checkpointHolder;

    // Start is called before the first frame update
    void Start()
    {
        playerUi = GameObject.FindGameObjectWithTag("PlayerUI");
        canvas = playerUi.GetComponent<Canvas>();
        playerManger = GetComponent<PlayerManger>();
        numberOfHearts = playerManger.PlayerHealth;


        // Assign the camera to the loading screen canvas
        canvas.worldCamera = Camera.main;
        canvas.planeDistance = .15f;
        canvas.sortingLayerName = "UI";
        canvas.sortingOrder = 0;
    }

    private void Update()
    {
       /* if(Input.GetKeyDown(KeyCode.B))
        {
            if(coroutineRunning == true)
            {
                StopCoroutine(SetObjective());
            }
            StartCoroutine(SetObjective());
        }*/

        if (playerManger.subStates != SubStates.Guarding)
        {
            blockMiddleground.value = playerManger.PlayerBlockHealth/100;
            blockForeground.value = playerManger.PlayerBlockHealth /100;
        }
    }

    public void CreateHealthBar()
    {
        playerUi = GameObject.FindGameObjectWithTag("PlayerUI");
        playerUi.SetActive(true);
        //Set up Health Bar
        if (playerUi != null)
        {
            var childrenList = new List<Image>();
            // the string name needs to be exact for the function to work
            Transform root = playerUi.transform;
            Hud = GetChildByName(root, "Hud").GetComponent<Image>();
            Transform heartHolder = GetChildByName(root, "Heart Holder");
            if (heartHolder != null)
            {
                foreach (Transform child in heartHolder.transform)
                {
                    Image i = child.GetComponent<Image>();
                    childrenList.Add(i);
                }
                hearts = childrenList.ToArray();
                childrenList.Clear();
                playerManger.UiCreated = true;
            }
        }
        else
        {
            Debug.Log("Player Ui is null");
            return;
        }
    }
    Transform GetChildByName(Transform parent, string name)
    {
        // Base Case: If the parent has no children, return null
        if (parent.childCount == 0)
        {
            return null;
        }

        // Check each child of the parent
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);

            // If the child has the desired name, return it
            if (child.name == name)
            {
                return child;
            }

            // If not, recursively search for the child in the children of the child
            Transform result = GetChildByName(child, name);

            // If the child is found in the recursive call, return it
            if (result != null)
            {
                return result;
            }
        }

        // If no child with the desired name is found, return null
        Debug.Log("name must be wrong");
        return null;
    }
    public void VisualizeHealth()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            //This is for creating our final health bar, change number of hearts to make amount visible in game 
            if (i < numberOfHearts)
                hearts[i].enabled = true;
            else
                hearts[i].enabled = false;

            //Handling visually representing players health in realtion to number of hearts  
            if (i < playerManger.PlayerHealth)
                hearts[i].sprite = fullHeart;
            else
                hearts[i].sprite = emptyHeart;
        }
        // Saftey net check making sure our hearts equal the current health
        if (numberOfHearts < playerManger.PlayerHealth)
        {
            numberOfHearts = playerManger.PlayerHealth;
        }
    }
    public void ReachedCheckpoint()
    {
        checkpointHolder.SetActive(true);
    }

    public void AssignObjectiveWithString(string objective)
    {
        setObjective = SetObjective();
        currentObjective = objective;
        if (coroutineRunning == false)
            StartCoroutine(setObjective);
    }   
     public void AssignObjectiveWithEvent(PlayerEventsWithData data)
     { 
        setObjective = SetObjective();
        currentObjective = data.text;
        if (coroutineRunning == false)
            StartCoroutine(setObjective);
     }

    public void RecallObjective()
    {
        if(currentObjective != "")
        {
            setObjective = SetObjective();
            if (coroutineRunning == false)
                StartCoroutine(setObjective);
        }
       
    }


    IEnumerator SetObjective()
    {
        coroutineRunning = true;
        objectiveText.text = currentObjective;
        objectiveHolder.SetActive(true);

        Color _startColor = objectiveText.color;
        Color _endColor = _startColor;
        _startSliderValue = objectiveSlider.value;
        float sliderEndValue;

        if (_startSliderValue > 0.9f)
        {
            sliderEndValue = 0;
            _endColor.a = 0;
        }
        else
        {
            sliderEndValue = 1;
            _endColor.a = 1;
        }

       if(sliderEndValue == 1)
       {
            //objective panel is open and we want to close it
            yield return StartCoroutine(DisplayObjectivePanel(sliderEndValue));
            yield return StartCoroutine(DisplayObjectiveText(_startColor, _endColor));
            open = true;
       }
       else
       {
            //objective panel is closed and we want to open it
            yield return StartCoroutine(DisplayObjectiveText(_startColor, _endColor));
            yield return StartCoroutine(DisplayObjectivePanel(sliderEndValue));
            open = false;
       }
       coroutineRunning = false;
        Debug.Log(open);


        //If no input is pressed then close after 
        if (open)
        {
            yield return new WaitForSeconds(1.5f); // Wait for 5 seconds
            StartCoroutine(SetObjective());  // Code for closing objective panel after 5 seconds
        }
        coroutineRunning = false;
    }
   IEnumerator DisplayObjectivePanel(float end)
   {
        float timeElapsed = 0;
        //Slider lerp
        while(timeElapsed < sliderLerpDuration)
        {
            objectiveSlider.value = Mathf.Lerp(_startSliderValue, end, timeElapsed * sliderMult);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
   }

   IEnumerator DisplayObjectiveText(Color start, Color end)
   {
         //fade in the text 
        float timeElapsed = 0;
        while (timeElapsed < textLerpDuration)
        {
            objectiveText.color = Color.Lerp(start, end, timeElapsed * textMult);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
   }


    public IEnumerator ReduceStamina(float percent)
    {
        float percentChange = blockForeground.value * percent;
        float percentChange2 = blockMiddleground.value * percent;
        float elapsed = 0f;
        while (elapsed < updateSpeedInSeconds_2)
        {
            elapsed += Time.deltaTime;
            blockForeground.value = Mathf.Lerp(blockForeground.value, percent, elapsed / updateSpeedInSeconds_1);
            blockMiddleground.value = Mathf.Lerp(blockMiddleground.value, percent, elapsed / updateSpeedInSeconds_2);
            Debug.Log(" foregroundImage.Fillamount" + blockForeground.value + " middlegroundImage.fillAmount" + blockMiddleground.value + " Percentage" + percent);
            yield return null;
        }
        //}
        //blockForeground.value = ;
        //blockMiddleground.value = percent;
    }

}
