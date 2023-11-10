using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI; // Make sure you have this using statement
using UnityEngine.XR;

public class UIManager : MonoBehaviour
{
    public GameObject shelfPage;
    public GameObject diaryPage;
    public GameObject homePage;
    public GameObject SkinLog1;
    public GameObject SkinLog2;

    public TMP_Text diaryButtonText;
    public TMP_Text shelfButtonText;
    public TMP_Text homeButtonText;

    public Button diaryButton;
    public Button shelfButton;
    public Button homeButton;

    // calendar variables
    public Button createLogFromDiaryButton;
    public GameObject calArrowExpandObject;
    public GameObject calArrowCloseObject;
    public Button calArrowExpand;
    public Button calArrowRight;
    public Button calArrowLeft;
    public Button calArrowClose;
    public GameObject octCalMin;
    public GameObject octCalMin2;
    public GameObject sepCalMin4;
    public GameObject octCalMax;
    public GameObject sepCalMin;
    public GameObject sepCalMax;
    public GameObject novCalMin;
    public GameObject novCalMax;
    public GameObject monthName;

    //HappySadButtons
    public Button awfulButton;
    public Button badButton;
    public Button neutralButton;
    public Button goodButton;
    public Button greatButton;
    public Sprite awfulButtonUnfilled;
    public Sprite awfulButtonFilled;
    public Sprite badButtonUnfilled;
    public Sprite badButtonFilled;
    public Sprite neutralButtonUnfilled;
    public Sprite neutralButtonFilled;
    public Sprite goodButtonUnfilled;
    public Sprite goodButtonFilled;
    public Sprite greatButtonUnfilled;
    public Sprite greatButtonFilled;
    private Button currSelectedMood;

    public Button[] describeSkinButtons;
    public Sprite questionnaireButtonUnfilled;
    public Sprite questionnaireButtonFilled;

    public Button nextPageButton1;

    public GameObject gradientHome;
    public GameObject gradientHomeExpanded;
    private string month;
    private string week;
    private bool isWeek;

    public Sprite diaryButtonNormalSprite;
    public Sprite diaryButtonSelectedSprite;
    public Sprite shelfButtonNormalSprite;
    public Sprite shelfButtonSelectedSprite;
    public Sprite homeButtonNormalSprite;
    public Sprite homeButtonSelectedSprite;

    private bool areDiaryButtonListenersAdded = false;

    public Animator moveUpAnimator;
    public Animator[] fadeAnimator;

    // Text colors
    private Color normalTextColor = Color.black; // Adjust as needed
    private Color selectedTextColor = new Color(1.0f, 0.549f, 0.659f, 1.0f); // Adjust as needed

    private void Start()
    {
        currSelectedMood = null;

        describeSkinButtons =
            FindDeepChild(SkinLog1.GetComponent<Transform>(), "Q2Buttons").GetComponentsInChildren<Button>();
        foreach (var button in describeSkinButtons)
        {
            button.GetComponent<Image>().sprite = questionnaireButtonUnfilled;
            button.onClick.AddListener(() => SelectQuestionnaireButton(button));
        }

        calendarSetup();
        moodSetup();

        diaryButtonText = diaryButton.GetComponentInChildren<TMP_Text>();
        shelfButtonText = shelfButton.GetComponentInChildren<TMP_Text>();
        homeButtonText = homeButton.GetComponentInChildren<TMP_Text>();

        // Subscribe to button click events
        diaryButton.onClick.AddListener(() => SwitchPage(diaryPage));
        shelfButton.onClick.AddListener(() => SwitchPage(shelfPage));
        homeButton.onClick.AddListener(() => SwitchPage(homePage));

        diaryButton.onClick.AddListener(() => SwitchPage(diaryPage));

        nextPageButton1.onClick.AddListener(() => SwitchPage(SkinLog2));
        initializeMoodButtons();

        SwitchPage(homePage);
        UpdateButtonAppearance(homePage);
    }

    void SelectQuestionnaireButton(Button selectedButton)
    {
        // Set the selected button sprite
        if (selectedButton.GetComponent<Image>().sprite == questionnaireButtonFilled)
        {
            selectedButton.GetComponent<Image>().sprite = questionnaireButtonUnfilled;
            TMP_Text buttonText = selectedButton.GetComponentInChildren<TMP_Text>();
            buttonText.color = new Color(0.19f, 0.19f, 0.19f, 1.0f);
        }
        else
        {
            selectedButton.GetComponent<Image>().sprite = questionnaireButtonFilled;
            TMP_Text buttonText = selectedButton.GetComponentInChildren<TMP_Text>();
            buttonText.color = Color.white;
        }

    }
    private void moodSetup()
    {
        awfulButton = FindDeepChild(homePage.GetComponent<Transform>(), "AwfulButton").GetComponent<Button>();
        badButton = FindDeepChild(homePage.GetComponent<Transform>(), "BadButton").GetComponent<Button>();
        neutralButton = FindDeepChild(homePage.GetComponent<Transform>(), "NeutralButton").GetComponent<Button>();
        goodButton = FindDeepChild(homePage.GetComponent<Transform>(), "GoodButton").GetComponent<Button>();
        greatButton = FindDeepChild(homePage.GetComponent<Transform>(), "GreatButton").GetComponent<Button>();
        awfulButton.onClick.AddListener(() => SelectMoodButton(awfulButton));
        awfulButton.onClick.AddListener(() => selectedMoodButtonAnimationStart(0));

        badButton.onClick.AddListener(() => SelectMoodButton(badButton));
        badButton.onClick.AddListener(() => selectedMoodButtonAnimationStart(1));

        neutralButton.onClick.AddListener(() => SelectMoodButton(neutralButton));
        neutralButton.onClick.AddListener(() => selectedMoodButtonAnimationStart(2));

        goodButton.onClick.AddListener(() => SelectMoodButton(goodButton));
        goodButton.onClick.AddListener(() => selectedMoodButtonAnimationStart(3));

        greatButton.onClick.AddListener(() => SelectMoodButton(greatButton));
        greatButton.onClick.AddListener(() => selectedMoodButtonAnimationStart(4));
    }

    private void moodSetupForQuestionnaire(int x)
    {
        awfulButton = FindDeepChild(SkinLog1.GetComponent<Transform>(), "AwfulButton").GetComponent<Button>();
        badButton = FindDeepChild(SkinLog1.GetComponent<Transform>(), "BadButton").GetComponent<Button>();
        neutralButton = FindDeepChild(SkinLog1.GetComponent<Transform>(), "NeutralButton").GetComponent<Button>();
        goodButton = FindDeepChild(SkinLog1.GetComponent<Transform>(), "GoodButton").GetComponent<Button>();
        greatButton = FindDeepChild(SkinLog1.GetComponent<Transform>(), "GreatButton").GetComponent<Button>();
        awfulButton.onClick.AddListener(() => SelectMoodButton(awfulButton));
        badButton.onClick.AddListener(() => SelectMoodButton(badButton));
        neutralButton.onClick.AddListener(() => SelectMoodButton(neutralButton));
        goodButton.onClick.AddListener(() => SelectMoodButton(goodButton));
        greatButton.onClick.AddListener(() => SelectMoodButton(greatButton));
        switch (x) {
            case 0:
                SelectMoodButton(awfulButton);
                break;
            case 1:
                SelectMoodButton(badButton);
                break;
            case 2:
                SelectMoodButton(neutralButton);
                break;
            case 3:
                SelectMoodButton(goodButton);
                break;
            case 4:
                SelectMoodButton(greatButton);
                break;
        }
    }

    private void calendarSetup()
    {
        month = "oct";
        week = "oct1";
        isWeek = true;

        octCalMin = FindDeepChild(diaryPage.GetComponent<Transform>(), "octCalMin");
        octCalMin2 = FindDeepChild(diaryPage.GetComponent<Transform>(), "octCalMin2");
        sepCalMin4 = FindDeepChild(diaryPage.GetComponent<Transform>(), "calSepMin4");
        octCalMax = FindDeepChild(diaryPage.GetComponent<Transform>(), "octCalMax");
        sepCalMin = FindDeepChild(diaryPage.GetComponent<Transform>(), "septCalMin");
        sepCalMax = FindDeepChild(diaryPage.GetComponent<Transform>(), "septCalMax");
        novCalMin = FindDeepChild(diaryPage.GetComponent<Transform>(), "novCalMin");
        novCalMax = FindDeepChild(diaryPage.GetComponent<Transform>(), "novCalMax");
        monthName = FindDeepChild(diaryPage.GetComponent<Transform>(), "monthName");

        gradientHome = FindDeepChild(diaryPage.GetComponent<Transform>(), "gradientHome");
        gradientHomeExpanded = FindDeepChild(diaryPage.GetComponent<Transform>(), "gradientHomeExpanded");
        calArrowExpandObject = FindDeepChild(diaryPage.GetComponent<Transform>(), "calArrowExpand");
        calArrowCloseObject = FindDeepChild(diaryPage.GetComponent<Transform>(), "calArrowClose");
        calArrowExpand = calArrowExpandObject.GetComponent<Button>();
        calArrowClose = calArrowCloseObject.GetComponent<Button>();
        calArrowRight = FindDeepChild(diaryPage.GetComponent<Transform>(), "calArrowRight").GetComponent<Button>();
        calArrowLeft = FindDeepChild(diaryPage.GetComponent<Transform>(), "calArrowLeft").GetComponent<Button>();
        calArrowExpand.onClick.AddListener(() => calHandler("expand"));
        calArrowClose.onClick.AddListener(() => calHandler("close"));
        calArrowLeft.onClick.AddListener(() => calHandler("left"));
        calArrowRight.onClick.AddListener(() => calHandler("right"));
    }



    void SelectMoodButton(Button selectedButton)
    {
        initializeMoodButtons();
        if (currSelectedMood == selectedButton)
        {
            return;
        }
        else
        {
            currSelectedMood = selectedButton;
            if (selectedButton == awfulButton)
            {
                selectedButton.GetComponent<Image>().sprite = awfulButtonFilled;
            }
            if (selectedButton == badButton)
            {
                selectedButton.GetComponent<Image>().sprite = badButtonFilled;
            }
            if (selectedButton == neutralButton)
            {
                selectedButton.GetComponent<Image>().sprite = neutralButtonFilled;
            }
            if (selectedButton == goodButton)
            {
                selectedButton.GetComponent<Image>().sprite = goodButtonFilled;
            }
            if (selectedButton == greatButton)
            {
                selectedButton.GetComponent<Image>().sprite = greatButtonFilled;
            }
        }

    }
    private void selectedMoodButtonAnimationStart(int j)
    {
        moodSetupForQuestionnaire(j);
        SkinLog1.SetActive(true);

        moveUpAnimator.SetTrigger("MoveUp");
        foreach (var animator in fadeAnimator)
        {
            animator.SetTrigger("FadeOut");
        }
        Invoke("DisableHome", 0.3f);
    }

    private void DisableHome()
    {
        homePage.SetActive(false);
    }
    private void initializeMoodButtons()
    {
        awfulButton.GetComponent<Image>().sprite = awfulButtonUnfilled;
        badButton.GetComponent<Image>().sprite = badButtonUnfilled;
        neutralButton.GetComponent<Image>().sprite = neutralButtonUnfilled;
        goodButton.GetComponent<Image>().sprite = goodButtonUnfilled;
        greatButton.GetComponent<Image>().sprite = greatButtonUnfilled;
    }

    private void calHandler(string command)
    {
        Debug.Log("cal handler called");
        if (command == "expand")
        {
            Debug.Log("expand called");
            isWeek = false;
            deactivateObj(gradientHome);
            activateObj(gradientHomeExpanded);
            switch(month)
            {
                case "oct":
                    deactivateObj(octCalMin);
                    deactivateObj(octCalMin2);
                    deactivateObj(sepCalMin4);
                    activateObj(octCalMax);
                    break;
                case "sep":
                    deactivateObj(octCalMin);
                    deactivateObj(octCalMin2);
                    deactivateObj(sepCalMin4);
                    deactivateObj(sepCalMin);
                    activateObj(sepCalMax);
                    break;
                case "nov":
                    deactivateObj(octCalMin);
                    deactivateObj(octCalMin2);
                    deactivateObj(sepCalMin4);
                    deactivateObj(sepCalMin);
                    deactivateObj(novCalMin);
                    activateObj(novCalMax);
                    break;
                default:
                    break;
            }
            deactivateObj(calArrowExpandObject);
            activateObj(calArrowCloseObject);
        }
        if (command == "close")
        {
            isWeek = true;
            activateObj(gradientHome);
            deactivateObj(gradientHomeExpanded);
            switch (month)
            {
                case "oct":
                    activateObj(octCalMin);
                    deactivateObj(octCalMax);
                    break;
                case "sep":
                    activateObj(sepCalMin);
                    deactivateObj(sepCalMax);
                    break;
                case "nov":
                    activateObj(novCalMin);
                    deactivateObj(novCalMax);
                    break;
                default:
                    break;
            }
            activateObj(calArrowExpandObject);
            deactivateObj(calArrowCloseObject);
        }
        if (command == "left")
        {
            switch (isWeek)
            {
                case false:
                    switch (month)
                    {
                        case "oct":
                            month = "sep";
                            monthName.GetComponent<TMP_Text>().text = "September 2023";
                            deactivateObj(octCalMax);
                            activateObj(sepCalMax);
                            break;
                        case "nov":
                            month = "oct";
                            monthName.GetComponent<TMP_Text>().text = "October 2023";
                            deactivateObj(novCalMax);
                            activateObj(octCalMax);
                            break;
                        default:
                            break;
                    }
                    break;
                case true:
                    switch (week)
                    {
                        case "oct1":
                            week = "sep4";
                            month = "sep";
                            monthName.GetComponent<TMP_Text>().text = "September 2023";
                            deactivateObj(octCalMin);
                            activateObj(sepCalMin4);
                            break;
                        case "oct2":
                            week = "oct1";
                            deactivateObj(octCalMin2);
                            activateObj(octCalMin);
                            break;
                        default:
                            break;
                    }
                    break;
            }
        }
        if (command == "right")
        {
            switch (isWeek)
            {
                case false:
                    switch (month)
                    {
                        case "oct":
                            month = "nov";
                            monthName.GetComponent<TMP_Text>().text = "November 2023";

                            deactivateObj(octCalMax);
                            activateObj(novCalMax);
                            break;
                        case "sep":
                            month = "oct";
                            monthName.GetComponent<TMP_Text>().text = "October 2023";

                            deactivateObj(sepCalMax);
                            activateObj(octCalMax);
                            break;
                        default:
                            break;
                    }
                    break;
                case true:
                    switch (week)
                    {
                        case "sep4":
                            week = "oct1";
                            month = "oct";
                            monthName.GetComponent<TMP_Text>().text = "October 2023";

                            deactivateObj(sepCalMin4);
                            activateObj(octCalMin);
                            break;
                        case "oct1":
                            week = "oct2";
                            deactivateObj(octCalMin);
                            activateObj(octCalMin2);
                            break;
                        default:
                            break;
                    }
                    break;
            }
        }
    }

    private void activateObj(GameObject gameObject)
    {
        if (gameObject != null)
        {
            gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("GameObject is null.");
        }
    }
    private void deactivateObj(GameObject gameObject)
    {
        if (gameObject != null)
        {
            gameObject.SetActive(false);
        }
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

        Debug.Log("setting skinlog1 false");
        SkinLog1.SetActive(false);
        SkinLog2.SetActive(false);

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