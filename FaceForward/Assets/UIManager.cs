using TMPro;
using UnityEngine;
using UnityEngine.UI; // Make sure you have this using statement
using UnityEngine.XR;

public class UIManager : MonoBehaviour
{
    public GameObject shelfPage;
    public GameObject diaryPage;
    public GameObject homePage;

    public TMP_Text diaryButtonText;
    public TMP_Text shelfButtonText;
    public TMP_Text homeButtonText;

    public Button diaryButton;
    public Button shelfButton;
    public Button homeButton;

    public Button createLogFromDiaryButton;
    public Button calArrowExpand;
    public Button calArrowRight;
    public Button calArrowLeft;
    public Button calArrowClose;

    public GameObject octCalMin;
    public GameObject octCalMax;
    public GameObject sepCalMin;
    public GameObject sepCalMax;
    public GameObject novCalMin;
    public GameObject novCalMax;


    public Sprite diaryButtonNormalSprite;
    public Sprite diaryButtonSelectedSprite;
    public Sprite shelfButtonNormalSprite;
    public Sprite shelfButtonSelectedSprite;
    public Sprite homeButtonNormalSprite;
    public Sprite homeButtonSelectedSprite;

    private bool areDiaryButtonListenersAdded = false;


    // Text colors
    private Color normalTextColor = Color.black; // Adjust as needed
    private Color selectedTextColor = new Color(1.0f, 0.549f, 0.659f, 1.0f); // Adjust as needed

    private void Start()
    {
        diaryButtonText = diaryButton.GetComponentInChildren<TMP_Text>();
        shelfButtonText = shelfButton.GetComponentInChildren<TMP_Text>();
        homeButtonText = homeButton.GetComponentInChildren<TMP_Text>();

        // Subscribe to button click events
        diaryButton.onClick.AddListener(() => SwitchPage(diaryPage));
        shelfButton.onClick.AddListener(() => SwitchPage(shelfPage));
        homeButton.onClick.AddListener(() => SwitchPage(homePage));
        homeButton.onClick.AddListener(clickingButton);

        calArrowExpand = FindDeepChild(diaryPage.GetComponent<Transform>(), "calArrowExpand").GetComponent<Button>();
        calArrowRight = FindDeepChild(diaryPage.GetComponent<Transform>(), "calArrowRight").GetComponent<Button>();
        calArrowLeft = FindDeepChild(diaryPage.GetComponent<Transform>(), "calArrowLeft").GetComponent<Button>();
        calArrowClose = FindDeepChild(diaryPage.GetComponent<Transform>(), "calArrowClose").GetComponent<Button>();

        // Set default states
        SwitchPage(homePage);
        UpdateButtonAppearance(homePage);
    }

    private void clickingButton()
    {
        Debug.Log("beingClicked");
    }

    GameObject FindDeepChild(Transform parent, string childName)
    {
        foreach (Transform child in parent)
        {
            if (child.name == childName)
                return child.gameObject;

            GameObject found = FindDeepChild(child, childName);
            if (found != null)
                return found;
        }
        return null;
    }

    private void SwitchPage(GameObject pageToActivate)
    {
        // Deactivate all pages
        shelfPage.SetActive(false);
        diaryPage.SetActive(false);
        homePage.SetActive(false);

        // Activate the desired page
        pageToActivate.SetActive(true);

        if (pageToActivate == diaryPage && !areDiaryButtonListenersAdded)
        {
            createLogFromDiaryButton.onClick.AddListener(() => SwitchPage(homePage));
            

            areDiaryButtonListenersAdded = true;
        }

        // Update the appearance of buttons
        UpdateButtonAppearance(pageToActivate);
    }


    private void UpdateButtonAppearance(GameObject activePage)
    {
        // Set all buttons to their normal appearance
        diaryButton.GetComponent<Image>().sprite = diaryButtonNormalSprite;
        shelfButton.GetComponent<Image>().sprite = shelfButtonNormalSprite;
        homeButton.GetComponent<Image>().sprite = homeButtonNormalSprite;

        diaryButtonText.color = normalTextColor;
        shelfButtonText.color = normalTextColor;
        homeButtonText.color = normalTextColor;

        diaryButtonText.fontStyle = FontStyles.Normal;
        shelfButtonText.fontStyle = FontStyles.Normal;
        homeButtonText.fontStyle = FontStyles.Normal;

        // Change appearance of the active button
        if (activePage == diaryPage)
        {
            diaryButton.GetComponent<Image>().sprite = diaryButtonSelectedSprite;
            diaryButtonText.color = selectedTextColor;
            diaryButtonText.fontStyle = FontStyles.Bold;
        }
        else if (activePage == shelfPage)
        {
            shelfButton.GetComponent<Image>().sprite = shelfButtonSelectedSprite;
            shelfButtonText.color = selectedTextColor;
            shelfButtonText.fontStyle = FontStyles.Bold;
        }
        else if (activePage == homePage)
        {
            homeButton.GetComponent<Image>().sprite = homeButtonSelectedSprite;
            homeButtonText.color = selectedTextColor;
            homeButtonText.fontStyle = FontStyles.Bold;
        }
    }
}