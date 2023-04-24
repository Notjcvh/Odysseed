using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    private PlayerManger playerManger;
    public GameObject playerUi;
    [SerializeField] private Image Hud;
    [SerializeField] private int numberOfHearts;
    private Image[] hearts; // the full array of hearts in the game
                            //Hud
    private Sprite[] allHuds;
    //hearts
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;


    // Start is called before the first frame update
    void Start()
    {
        playerUi = GameObject.FindGameObjectWithTag("PlayerUI");
        playerManger = GetComponent<PlayerManger>();
        numberOfHearts = playerManger.PlayerHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateHealthBar()
    {
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
}
