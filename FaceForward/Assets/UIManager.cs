using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI; // Make sure you have this using statement
using UnityEngine.XR;
using System.Linq;
using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using System.Collections;
using System.Diagnostics;

public class UIManager : MonoBehaviour
{
    public GameObject shelfPage;
    public GameObject diaryPage;
    public GameObject homePage;
    public GameObject SkinLog1;
    public GameObject SkinLog2;
    public GameObject SkinLog3;
    public GameObject SkinLog4;
    public GameObject Shop;
    public GameObject ShopInfo;
    public GameObject SelectRoutine;
    public GameObject routinePage;
    public GameObject insightsPage;

    public TMP_Text diaryButtonText;
    public TMP_Text shelfButtonText;
    public TMP_Text homeButtonText;

    public Button diaryButton;
    public Button shelfButton;
    public Button homeButton;

    //insights
    public Button openInsightsBtn;
    public Button closeInsightsBtn;
    public Button toggleTimeBtn;
    public Sprite[] insightSprites;
    private Image InsightsImageToToggle;
    private bool onWeek;

    // okay
    public GameObject dailySkinLog;

    // HappySadButtons
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
    private int currSelectedMood;

    // Questionnaire buttons
    public Button[] allQuestionnaireButtons;
    public Sprite questionnaireButtonUnfilled;
    public Sprite questionnaireButtonFilled;
    public Button nextPageButton1;
    public Button nextPageButton2;
    public Button prevPageButton2;
    public Button nextPageButton3;
    public Button prevPageButton3;
    public Button prevPageButton4;
    public Button completeSkinLog;
    public Button closeButton1;
    public Button closeButton2;
    public Button closeButton3;
    public Button closeButton4;
    public Slider sleepSlider;
    public Slider cupSlider;
    public TMP_Text sleepText;
    public TMP_Text cupText;

    // shop vars
    private Tuple<int, int> toAddShelfLoc;
    public Button leaveShopButton;
    public Sprite[] bottleSprites;
    public Button[] addThisBottle;
    public Button[] checkOutThisBottle;
    public List<List<List<GameObject>>> bottles;
    public List<List<int>> bottlesEasy;
    public Button closeInfoButton;
    public Button addFromInfoButton;

    // navbar
    public Sprite diaryButtonNormalSprite;
    public Sprite diaryButtonSelectedSprite;
    public Sprite shelfButtonNormalSprite;
    public Sprite shelfButtonSelectedSprite;
    public Sprite homeButtonNormalSprite;
    public Sprite homeButtonSelectedSprite;
    private bool areDiaryButtonListenersAdded = false;

    // logroutine vars
    public Button leaveSelectRoutineButton;
    public Button leaveRoutinePageButton;
    public Button beginLogRoutineButton;
    public Button finalLogRoutineButton;
    public Sprite uncheckedCheckbox;
    public Sprite checkedCheckbox;

    private int pageOpen;

    // animators
    public Animator moveUpAnimator;
    public Animator[] fadeOutAnimator;
    public Animator[] fadeInAnimator;
    public Animator[] pageDownAnimator;
    public Animator slideShopAnimator;
    public Animator slideShopInfoAnimator;
    public Animator slideInsightsAnimator;



    public Animator slideSelectRoutineAnimator;
    public Animator slideRoutineAnimator;

    public List<ProductDetails> products;

    // Text colors
    private Color normalTextColor = Color.black; // Adjust as needed
    private Color selectedTextColor = new Color(1.0f, 0.549f, 0.659f, 1.0f); // Adjust as needed

    private Stopwatch stopWatch;


    public struct ProductDetails
    {
        public string productType;
        public string brandName;
        public string productName;
        public Sprite sprite;

        public ProductDetails(string productType, string brandName, string productName, Sprite sprite)
        {
            this.productType = productType;
            this.brandName = brandName;
            this.productName = productName;
            this.sprite = sprite;
        }
    }

    private void productsSetup()
    {
        products = new List<ProductDetails>();
        products.Add(new ProductDetails("Cleanser", "La Roche Posay", "Hydrating Gentle Cleanser", bottleSprites[0]));
        products.Add(new ProductDetails("Cleanser", "CeraVe", "Foaming Facial Cleanser", bottleSprites[1]));
        products.Add(new ProductDetails("Serum", "CosRX", "Advanced Snail 96% Mucin Power Essence", bottleSprites[2]));
        products.Add(new ProductDetails("Cleanser", "The Ordinary", "Squalane Cleanser", bottleSprites[3]));
        products.Add(new ProductDetails("Sunscreen", "Round Lab", "Birch Moisturizing Sunscreen SPF 50+", bottleSprites[4]));
        products.Add(new ProductDetails("Moisturizer", "innisfree", "Deju Glow Tone-up Cream with Jeju Cherry Blossom", bottleSprites[5]));

    }

    private void logRoutineSetup()
    {
        leaveSelectRoutineButton.onClick.AddListener(leaveSelectRoutine);
        leaveRoutinePageButton.onClick.AddListener(leaveRoutinePage);
        beginLogRoutineButton.onClick.AddListener(goToSelectRoutine);

        Button[] selectRoutineButtons =
            FindDeepChild(SelectRoutine.GetComponent<Transform>(), "Routines").GetComponentsInChildren<Button>();
        for (int i = 0; i < selectRoutineButtons.Length; i++)
        {
            int tempi = i;

            selectRoutineButtons[tempi].onClick.AddListener(() => goToRoutinePage(tempi));
        }
        finalLogRoutineButton.onClick.AddListener(() => finishedLoggingRoutine(pageOpen));
    }

    private void toggleTime()
    {
        if (onWeek)
        {
            onWeek = false;
            toggleTimeBtn.GetComponent<Image>().sprite = insightSprites[0];
            InsightsImageToToggle.sprite = insightSprites[2];
        }
        else
        { 
            onWeek = true;
            toggleTimeBtn.GetComponent<Image>().sprite = insightSprites[1];
            InsightsImageToToggle.sprite = insightSprites[3];
        }

    }
    private void Start()
    {
        onWeek = false;
        openInsightsBtn.onClick.AddListener(openInsights);
        closeInsightsBtn.onClick.AddListener(closeInsights);
        toggleTimeBtn.onClick.AddListener(toggleTime);
        InsightsImageToToggle = FindDeepChild(insightsPage.GetComponent<Transform>(), "InsightsImage").GetComponent<Image>();

        stopWatch = new Stopwatch();
        sleepSlider.onValueChanged.AddListener(updateSleepText);
        updateSleepText(sleepSlider.value);
        cupSlider.onValueChanged.AddListener(updateCupText);
        updateCupText(sleepSlider.value);

        productsSetup();
        logRoutineSetup();
        shelfShopSetup();
        questionnaireSetup();
        moodSetup(-1);
        initializeMoodButtons();

        diaryButtonText = diaryButton.GetComponentInChildren<TMP_Text>();
        shelfButtonText = shelfButton.GetComponentInChildren<TMP_Text>();
        homeButtonText = homeButton.GetComponentInChildren<TMP_Text>();

        diaryButton.onClick.AddListener(() => SwitchPage(diaryPage));
        shelfButton.onClick.AddListener(() => SwitchPage(shelfPage));
        homeButton.onClick.AddListener(() => SwitchPage(homePage));

        SwitchPage(homePage);
        UpdateButtonAppearance(homePage);
    }

    void updateSleepText(float value)
    {
        // Map the value to the range 0-8 and round to an integer
        int hours = Mathf.RoundToInt(value * 8);
        sleepText.text = hours + " hours";
    }
    void updateCupText(float value)
    {
        // Map the value to the range 0-8 and round to an integer
        int hours = Mathf.RoundToInt(value * 8);
        cupText.text = hours + " cups";
    }
    private void finishedLoggingRoutine(int routineToLog)
    {
        SwitchPage(diaryPage);
        if (routineToLog == 0)
        {
            //morningRoutineLog.SetActive(true);
        }
        if (routineToLog == 1)
        {
            //eveningRoutineLog.SetActive(true);
        }
    }
    private void shelfShopSetup()
    {
        closeInfoButton.onClick.AddListener(() => closeInfoScreen());
        GameObject routineGroup = FindDeepChild(shelfPage.GetComponent<Transform>(), "RoutineGroup");
        bottlesEasy = new List<List<int>>();
        bottles = new List<List<List<GameObject>>>();
        GameObject[] shelves = new GameObject[routineGroup.transform.childCount - 1];
        for (int i = 0; i < routineGroup.transform.childCount - 1; i++)
        {
            bottlesEasy.Add(new List<int>());
            bottles.Add(new List<List<GameObject>>());
            shelves[i] = FindDeepChild(shelfPage.GetComponent<Transform>(), "RoutineGroup").transform.GetChild(i).gameObject;
        }
        for (int i = 0; i < shelves.Length; i++)
        {
            bottles[i].Add(GetChildren(FindDeepChild(shelves[i].GetComponent<Transform>(), "addBottles")));
            bottles[i].Add(GetChildren(FindDeepChild(shelves[i].GetComponent<Transform>(), "realBottles")));
            foreach (GameObject realBottle in bottles[i][1])
            {
                realBottle.GetComponent<CanvasGroup>().alpha = 0;
                bottlesEasy[i].Add(-1);
            }
            for (int j = 0; j < bottles[i][0].Count; j++)
            {
                GameObject addBottle = bottles[i][0][j];
                Button toAdd = FindDeepChild(addBottle.GetComponent<Transform>(), "plusMorning").GetComponent<Button>();

                int tempi = i;
                int tempj = j;

                toAdd.onClick.AddListener(() => beginAddProcess(tempi, tempj));
            }
        }
        leaveShopButton.onClick.AddListener(leaveShop);
        for (int i = 0; i < addThisBottle.Length; i++)
        {
            int tempi = i;
            addThisBottle[tempi].onClick.AddListener(() => updateShelfNewProduct(tempi));
        }
        for (int i = 0; i < checkOutThisBottle.Length; i++)
        {
            int tempi = i;
            checkOutThisBottle[tempi].onClick.AddListener(() => openInfoBox(tempi));
        }
    }



    public List<GameObject> GetChildren(GameObject parentGameObject)
    {
        List<GameObject> children = new List<GameObject>();

        for (int i = 0; i < parentGameObject.transform.childCount; i++)
        {
            children.Add(parentGameObject.transform.GetChild(i).gameObject);
        }
        return children;
    }

    private void beginAddProcess(int shelf, int shelfSpot)
    {
        stopWatch.Start();

        UnityEngine.Debug.Log("setting toAddShelfLoc as " + shelf + ", " + shelfSpot);

        toAddShelfLoc = new Tuple<int, int>(shelf, shelfSpot);

        TMP_InputField input = Shop.GetComponentInChildren<TMP_InputField>();
        input.text = "";

        Shop.SetActive(true);
        slideShopAnimator.SetTrigger("SlideLeft");

    }

    private void openInsights()
    {
        insightsPage.SetActive(true);
        slideInsightsAnimator.SetTrigger("SlideUpInsights");
    }
    private void openInfoBox(int productNum)
    {
        FindDeepChild(ShopInfo.GetComponent<Transform>(), "bottleSprite").GetComponent<Image>().sprite = products[productNum].sprite;
        FindDeepChild(ShopInfo.GetComponent<Transform>(), "brandName").GetComponent<TMP_Text>().text = products[productNum].brandName;
        FindDeepChild(ShopInfo.GetComponent<Transform>(), "productName").GetComponent<TMP_Text>().text = products[productNum].productName;
        addFromInfoButton.onClick.RemoveAllListeners();
        addFromInfoButton.onClick.AddListener(() => addFromInfoFunction(productNum));

        ShopInfo.SetActive(true);
        slideShopInfoAnimator.SetTrigger("SlideUp");
    }

    private void closeInsights()
    {
        slideInsightsAnimator.SetTrigger("SlideDownInsights");
        Invoke("DisableInsights", 0.4f);
    }
    private void DisableInsights()
    {
        insightsPage.SetActive(false);
    }
    private void closeInfoScreen()
    {
        slideShopInfoAnimator.SetTrigger("SlideDown");
        Invoke("DisableShopInfo", 0.4f);
    }
    private void addFromInfoFunction(int productNum) {
        slideShopInfoAnimator.SetTrigger("SlideDown");
        Invoke("DisableShopInfo", 0.8f);
        updateShelfNewProduct(productNum);
        StartCoroutine(updateShelfNewProductDelay(productNum, 0.8f));

    }

    IEnumerator updateShelfNewProductDelay(int productNum, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        updateShelfNewProduct(productNum);
    }

    private void DisableShopInfo()
    {
        ShopInfo.SetActive(false);
    }

    private void updateShelfNewProduct(int i)
    {
        //Debug.Log("product " + i + " to add to shelf " + toAddShelfLoc.Item1 + " at location " + toAddShelfLoc.Item2);
        bottlesEasy[toAddShelfLoc.Item1][toAddShelfLoc.Item2] = i;
        GameObject addProductButton = bottles[toAddShelfLoc.Item1][0][toAddShelfLoc.Item2];
        addProductButton.GetComponent<CanvasGroup>().alpha = 0;
        FindDeepChild(addProductButton.GetComponent<Transform>(), "plusMorning").SetActive(false);

        bottles[toAddShelfLoc.Item1][1][toAddShelfLoc.Item2].GetComponent<CanvasGroup>().alpha = 1;
        bottles[toAddShelfLoc.Item1][1][toAddShelfLoc.Item2].GetComponent<Image>().sprite = bottleSprites[i];

        toAddShelfLoc = null;
        slideShopAnimator.SetTrigger("SlideRight");

        UnityEngine.Debug.Log(stopWatch.ElapsedMilliseconds / 1000f);

        Invoke("DisableShop", 0.5f);
    }
    private void goToRoutinePage(int shelfNum)
    {
        routinePage.SetActive(true);
        pageOpen = shelfNum;
        slideRoutineAnimator.SetTrigger("SlideLeft");
        GameObject routineName = FindDeepChild(routinePage.GetComponent<Transform>(), "routineName");
        if (shelfNum == 0)
        {
            routineName.GetComponent<TMP_Text>().text = "Morning Routine";
        }
        if (shelfNum == 1)
        {
            routineName.GetComponent<TMP_Text>().text = "Evening Routine";
        }

        List<GameObject> steps = GetChildren(FindDeepChild(routinePage.GetComponent<Transform>(), "StepsGroup"));
        for (int i = 0; i < steps.Count; i++)
        {
            GameObject stepParent = steps[i];
            //Debug.Log("bottle type to show: " + bottlesEasy[shelfNum][i]);
            if (bottlesEasy[shelfNum][i] == -1)
            {
                stepParent.SetActive(false);
            } else
            {
                stepParent.SetActive(true);
                GameObject productType = FindDeepChild(stepParent.GetComponent<Transform>(), "productType");
                productType.GetComponent<TMP_Text>().text = products[bottlesEasy[shelfNum][i]].productType;
                GameObject productName = FindDeepChild(stepParent.GetComponent<Transform>(), "productName");
                productName.GetComponent<TMP_Text>().text = products[bottlesEasy[shelfNum][i]].productName;
                GameObject brandName = FindDeepChild(stepParent.GetComponent<Transform>(), "brandName");
                brandName.GetComponent<TMP_Text>().text = products[bottlesEasy[shelfNum][i]].brandName;
                GameObject product = FindDeepChild(stepParent.GetComponent<Transform>(), "product");
                product.GetComponent<Image>().sprite = bottleSprites[bottlesEasy[shelfNum][i]];

                GameObject checkbox = FindDeepChild(stepParent.GetComponent<Transform>(), "check");
                //Debug.Log("adding checkbox listener for " + routineName.GetComponent<TMP_Text>().text);
                GameObject tempCheckBox = checkbox;
                checkbox.GetComponent<Button>().onClick.RemoveAllListeners();
                checkbox.GetComponent<Image>().sprite = uncheckedCheckbox;
                checkbox.GetComponent<Button>().onClick.AddListener(() => checkboxLol(checkbox));
            }
        }
        ScrollRect scrollRect = FindDeepChild(routinePage.GetComponent<Transform>(), "Scroll").GetComponent<ScrollRect>();
        scrollRect.verticalNormalizedPosition = 1;
    }
    private void checkboxLol(GameObject checkbox)
    {
        if (checkbox.GetComponent<Image>().sprite == uncheckedCheckbox)
        {
            checkbox.GetComponent<Image>().sprite = checkedCheckbox;
        } else
        {
            checkbox.GetComponent<Image>().sprite = uncheckedCheckbox;
        }
    }
    private void leaveRoutinePage()
    {
        slideRoutineAnimator.SetTrigger("SlideRight");
        Invoke("DisableRoutinePage", 0.5f);
    }
    private void DisableRoutinePage()
    {
        routinePage.SetActive(false);
    }

    private void goToSelectRoutine()
    {
        SelectRoutine.SetActive(true);
        slideSelectRoutineAnimator.SetTrigger("SlideLeft");
    }
    private void leaveSelectRoutine()
    {
        slideSelectRoutineAnimator.SetTrigger("SlideRight");
        Invoke("DisableSelectRoutine", 0.5f);
    }
    private void DisableSelectRoutine()
    {
        SelectRoutine.SetActive(false);
    }
    private void leaveShop()
    {
        toAddShelfLoc = null;
        slideShopAnimator.SetTrigger("SlideRight");
        Invoke("DisableShop", 0.5f);
    }
    private void DisableShop()
    {
        Shop.SetActive(false);
    }

    private void pageStayUp()
    {
        foreach (var animator in pageDownAnimator)
        {
            animator.SetTrigger("PageStayUp");
        }
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
    private void questionnaireSetup()
    {
        currSelectedMood = -1;
        Button[] qb1 =
            FindDeepChild(SkinLog1.GetComponent<Transform>(), "Q2Buttons").GetComponentsInChildren<Button>();
        Button[] qb2 =
            FindDeepChild(SkinLog2.GetComponent<Transform>(), "Q3Buttons").GetComponentsInChildren<Button>();
        Button[] qb3 =
            FindDeepChild(SkinLog2.GetComponent<Transform>(), "Q6Buttons1").GetComponentsInChildren<Button>();
        Button[] qb4 =
            FindDeepChild(SkinLog2.GetComponent<Transform>(), "Q6Buttons2").GetComponentsInChildren<Button>();

        allQuestionnaireButtons = qb1.Concat(qb2).Concat(qb3).Concat(qb4).ToArray();

        foreach (Button button in allQuestionnaireButtons)
        {
            button.GetComponent<Image>().sprite = questionnaireButtonUnfilled;
            button.onClick.AddListener(() => SelectQuestionnaireButton(button));
        }

        nextPageButton1.onClick.AddListener(() => SwitchPage(SkinLog2));
        nextPageButton1.onClick.AddListener(pageStayUp);
        prevPageButton2.onClick.AddListener(() => SwitchPage(SkinLog1));
        nextPageButton2.onClick.AddListener(() => SwitchPage(SkinLog3));
        nextPageButton2.onClick.AddListener(pageStayUp);
        prevPageButton3.onClick.AddListener(() => SwitchPage(SkinLog2));
        nextPageButton3.onClick.AddListener(() => SwitchPage(SkinLog4));
        nextPageButton3.onClick.AddListener(pageStayUp);
        prevPageButton4.onClick.AddListener(() => SwitchPage(SkinLog3));

        closeButton1.onClick.AddListener(() => closeQuestionnaireScreen(0));
        closeButton2.onClick.AddListener(() => closeQuestionnaireScreen(0));
        closeButton3.onClick.AddListener(() => closeQuestionnaireScreen(0));
        closeButton4.onClick.AddListener(() => closeQuestionnaireScreen(0));
        completeSkinLog.onClick.AddListener(() => closeQuestionnaireScreen(1));
    }

    private void moodSetup(int x)
    {
        awfulButton = FindDeepChild(homePage.GetComponent<Transform>(), "AwfulButton").GetComponent<Button>();
        badButton = FindDeepChild(homePage.GetComponent<Transform>(), "BadButton").GetComponent<Button>();
        neutralButton = FindDeepChild(homePage.GetComponent<Transform>(), "NeutralButton").GetComponent<Button>();
        goodButton = FindDeepChild(homePage.GetComponent<Transform>(), "GoodButton").GetComponent<Button>();
        greatButton = FindDeepChild(homePage.GetComponent<Transform>(), "GreatButton").GetComponent<Button>();

        awfulButton.onClick.RemoveAllListeners();
        badButton.onClick.RemoveAllListeners();
        neutralButton.onClick.RemoveAllListeners();
        goodButton.onClick.RemoveAllListeners();
        greatButton.onClick.RemoveAllListeners();

        awfulButton.onClick.AddListener(() => SelectMoodButton(0, true));
        awfulButton.onClick.AddListener(() => selectedMoodButtonAnimationStart(0));

        badButton.onClick.AddListener(() => SelectMoodButton(1, true));
        badButton.onClick.AddListener(() => selectedMoodButtonAnimationStart(1));

        neutralButton.onClick.AddListener(() => SelectMoodButton(2, true));
        neutralButton.onClick.AddListener(() => selectedMoodButtonAnimationStart(2));

        goodButton.onClick.AddListener(() => SelectMoodButton(3, true));
        goodButton.onClick.AddListener(() => selectedMoodButtonAnimationStart(3));

        greatButton.onClick.AddListener(() => SelectMoodButton(4, true));
        greatButton.onClick.AddListener(() => selectedMoodButtonAnimationStart(4));
        if (x == -1)
        {
            initializeMoodButtons();
        } else
        {
            SelectMoodButton(x, false);
        }
    }

    private void moodSetupForQuestionnaire(int x)
    {
        awfulButton = FindDeepChild(SkinLog1.GetComponent<Transform>(), "AwfulButton").GetComponent<Button>();
        badButton = FindDeepChild(SkinLog1.GetComponent<Transform>(), "BadButton").GetComponent<Button>();
        neutralButton = FindDeepChild(SkinLog1.GetComponent<Transform>(), "NeutralButton").GetComponent<Button>();
        goodButton = FindDeepChild(SkinLog1.GetComponent<Transform>(), "GoodButton").GetComponent<Button>();
        greatButton = FindDeepChild(SkinLog1.GetComponent<Transform>(), "GreatButton").GetComponent<Button>();

        awfulButton.onClick.RemoveAllListeners();
        badButton.onClick.RemoveAllListeners();
        neutralButton.onClick.RemoveAllListeners();
        goodButton.onClick.RemoveAllListeners();
        greatButton.onClick.RemoveAllListeners();

        awfulButton.onClick.AddListener(() => SelectMoodButton(0, true));
        badButton.onClick.AddListener(() => SelectMoodButton(1, true));
        neutralButton.onClick.AddListener(() => SelectMoodButton(2, true));
        goodButton.onClick.AddListener(() => SelectMoodButton(3, true));
        greatButton.onClick.AddListener(() => SelectMoodButton(4, true));

        //Debug.Log("calling select mood button for questionnaire");
        SelectMoodButton(x, false);
    }

    void SelectMoodButton(int buttonToSelect, bool removing)
    {
        //Debug.Log("select mood button called on button: " + buttonToSelect);
        initializeMoodButtons();
        if (currSelectedMood == buttonToSelect && removing)
        {
            currSelectedMood = -1;
            return;
        }
        else
        {
            currSelectedMood = buttonToSelect;
            if (buttonToSelect == 0)
            {
                awfulButton.GetComponent<Image>().sprite = awfulButtonFilled;
            }
            if (buttonToSelect == 1)
            {
                badButton.GetComponent<Image>().sprite = badButtonFilled;
            }
            if (buttonToSelect == 2)
            {
                neutralButton.GetComponent<Image>().sprite = neutralButtonFilled;
            }
            if (buttonToSelect == 3)
            {
                goodButton.GetComponent<Image>().sprite = goodButtonFilled;
            }
            if (buttonToSelect == 4)
            {
                greatButton.GetComponent<Image>().sprite = greatButtonFilled;
            }
        }
    }
    private void selectedMoodButtonAnimationStart(int j)
    {
        SkinLog1.SetActive(true);
        Vector3 curpos = SkinLog1.transform.position;
        SkinLog1.transform.position = new Vector3(curpos.x, 515.665f, curpos.z);

        moodSetupForQuestionnaire(j);

        moveUpAnimator.SetTrigger("MoveUp");
        foreach (var animator in fadeOutAnimator)
        {
            animator.SetTrigger("FadeOut");
        }
        foreach (var animator in fadeInAnimator)
        {
            animator.SetTrigger("FadeIn");
        }
        foreach (var animator in pageDownAnimator)
        {
            animator.SetTrigger("PageStayUp");
        }
        Invoke("DisableHome", 0.3f);
    }

    void closeQuestionnaireScreen(int j)
    {

        moodSetup(currSelectedMood);
        homePage.SetActive(true);

        moveUpAnimator.SetTrigger("ReturnDown");
        foreach (var animator in fadeOutAnimator)
        {
            animator.SetTrigger("reappear");
        }
        foreach (var animator in pageDownAnimator)
        {
            animator.SetTrigger("pageDown");
        }
        if (j == 1)
        {
            Invoke("DisableHome", 0.5f);
            dailySkinLog.SetActive(true);
            diaryPage.SetActive(true);
            UpdateButtonAppearance(diaryPage);
        }
        Invoke("DisableQuestionnaireScreen", 0.5f);
    }

    private void DisableHome()
    {
        homePage.SetActive(false);
    }

    private void DisableQuestionnaireScreen()
    {
        SkinLog1.SetActive(false);
        SkinLog2.SetActive(false);
        SkinLog3.SetActive(false);
        SkinLog4.SetActive(false);
    }

    private void initializeMoodButtons()
    {
        awfulButton.GetComponent<Image>().sprite = awfulButtonUnfilled;
        badButton.GetComponent<Image>().sprite = badButtonUnfilled;
        neutralButton.GetComponent<Image>().sprite = neutralButtonUnfilled;
        goodButton.GetComponent<Image>().sprite = goodButtonUnfilled;
        greatButton.GetComponent<Image>().sprite = greatButtonUnfilled;
    }

    private void activateObj(GameObject gameObject)
    {
        if (gameObject != null)
        {
            gameObject.SetActive(true);
        }
    }
    private void deactivateObj(GameObject gameObject)
    {
        if (gameObject != null)
        {
            gameObject.SetActive(false);
        }
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
        SkinLog1.SetActive(false);
        SkinLog2.SetActive(false);
        SkinLog3.SetActive(false);
        SkinLog4.SetActive(false);
        Shop.SetActive(false);
        ShopInfo.SetActive(false);
        insightsPage.SetActive(false);
        SelectRoutine.SetActive(false);
        routinePage.SetActive(false);

        // Activate the desired page
        pageToActivate.SetActive(true);

        if (pageToActivate == diaryPage && !areDiaryButtonListenersAdded)
        {
            areDiaryButtonListenersAdded = true;
        }

        // Update the appearance of buttons
        if (pageToActivate == diaryPage || pageToActivate == homePage || pageToActivate == shelfPage)
        {
            UpdateButtonAppearance(pageToActivate);
        }
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