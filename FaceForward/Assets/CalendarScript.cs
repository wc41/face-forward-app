using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class CalendarScript : MonoBehaviour
{
    public GameObject[] Days;
    public string[] Months;
    public GameObject Header;
    public TextMeshProUGUI CalendarHeader;

    [SerializeField] private int currentMonth;
    [SerializeField] private int yearCounter = 0;
    
    [SerializeField] private DateTime tempMonth;
    [SerializeField] private DateTime tempWeek;
    [SerializeField] private DateTime currDisplay;
    private DateTime currentTime;
    public int currentDay;
    private bool week;
    public GameObject gradientHome;
    public GameObject gradientHomeExpanded;
    public GameObject calArrowExpand;
    public GameObject calArrowCollapse;
    public GameObject CurrentDayLabel;
    public GameObject currentDayCircle;
    public GameObject offsetInfo;

    public GameObject smallCircle;
    private List<GameObject> circles = new List<GameObject>();
    private List<DateTime> days = new List<DateTime>();

    public GameObject[] logs;
    private bool todaySet = false;


    // Start is called before the first frame update
    void Start()
    {
        currentMonth = DateTime.Now.Month;
        Debug.Log("MonthCounter: " + currentMonth);
        CalendarHeader = Header.GetComponent<TextMeshProUGUI>();
        currentTime = DateTime.Now;
        CurrentDayLabel.GetComponent<TextMeshProUGUI>().text = DateTime.Now.ToString("MMMM d, yyyy");
        tempMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        tempWeek = DateTime.Now;
        /*while (tempWeek.DayOfWeek != 0)
        {
            tempWeek = tempWeek.AddDays(-1);
        }*/
        tempWeek = resetToSunday(tempWeek);
        week = true;

        circles.Add(smallCircle);
        days.Add(DateTime.Now);

        CreateCircles();
        makeButtons();
        clearLabels();
        clearCircles();
        CreateMonths();
        CreateCalendar();
        currentDay = (DateTime.Now.Day);
        todaySet = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!todaySet)
        {
            todaySet = logs[0].activeSelf;
            circles[0].SetActive(todaySet);
        }
    }

    void turnOffLogs()
    {
        foreach (GameObject log in  logs)
        {
            log.SetActive(false);
        }
    }

    void makeWhite()
    {
        for (int i = 0; i < Days.Length; i++)
        {
            Days[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        }
    }

    void updateDate(DateTime newDate, GameObject newCirclePos)
    {
        CurrentDayLabel.GetComponent<TextMeshProUGUI>().text = newDate.ToString("MMMM d, yyyy");
        makeWhite();
        currentTime = newDate;
        currentDayCircle.SetActive(true);
        currentDayCircle.transform.position = newCirclePos.transform.position + new Vector3(0, -3, 0);
        newCirclePos.GetComponentInChildren<TextMeshProUGUI>().color = new Color(1f, 176 / 255f, 160 / 255f, 1f);
        
        turnOffLogs();
        for (int i = 0; i < days.Count; i++)
        {
            if (i == 0 && !todaySet)
            {
                continue;
            }
            DateTime day = days[i];
            if (newDate.Year.Equals(day.Year) && newDate.Month.Equals(day.Month) && newDate.Day.Equals(day.Day))
            {
                logs[i].SetActive(true);
            }
        }
    }

    void makeButtons()
    {
        foreach (GameObject day in Days)
        {
            day.AddComponent<Button>();
        }
    }

    void CreateCircles()
    {
        for (int i = 1; i <= 3; i++)
        {
            circles.Add(Instantiate(smallCircle, this.gameObject.transform));
            days.Add(DateTime.Now.AddDays(-i));
        }
    }

    void clearCircles()
    {
        smallCircle.SetActive(false);
        foreach (GameObject circle in circles)
        {
            circle.SetActive(false);
        }
    }

    void CreateMonths()
    {
        Months = new string[12];
        DateTime monthIter = new DateTime(2000, 1, 1);

        for (int i = 0; i < 12; i++)
        {
            monthIter = new DateTime(DateTime.Now.Year, i + 1, 1);
            Months[i] = monthIter.ToString("MMMM");
        }
        /*iMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);*/
        /*CalendarHeader.text = Months[DateTime.Now.Month - 1] + " " + DateTime.Now.Year;*/
        CalendarHeader.text = Months[DateTime.Now.Month - 1];
    }

    void CreateCalendar()
    {
        if (week)
        {
            CreateWeek();
        }
        else
        {
            CreateMonth();
        }
    }

    public void next()
    {
        if (week) { nextWeek(); }
        else { nextMonth(); }
    }

    public void prev()
    {
        if (week) { previousWeek(); }
        else { previousMonth(); }
    }

    public void expand()
    {
        week = false;
        gradientHome.SetActive(false);
        gradientHomeExpanded.SetActive(true);
        calArrowExpand.SetActive(false);
        calArrowCollapse.SetActive(true);
        offsetInfo.transform.position += new Vector3(0, -250, 0);
        clearLabels();
        clearCircles();
        CreateCalendar();
    }

    public void close()
    {
        week = true;
        gradientHome.SetActive(true);
        gradientHomeExpanded.SetActive(false);
        calArrowExpand.SetActive(true);
        calArrowCollapse.SetActive(false);
        offsetInfo.transform.position += new Vector3(0, 250, 0);

        int updatedMonth = tempWeek.AddDays(3).Month;
        Debug.Log("Before: " + tempMonth.ToString());
        Debug.Log(updatedMonth - currentMonth);
        tempMonth = tempMonth.AddMonths(updatedMonth - currentMonth);
        Debug.Log("After: " + tempMonth.ToString());
        currentMonth = updatedMonth;
        string year = (yearCounter != 0) ? " " + (DateTime.Now.Year + yearCounter) : "";
        CalendarHeader.text = Months[currentMonth - 1] + year;

        clearLabels();
        clearCircles();
        CreateCalendar();
    }

    void CreateMonth()
    {
        currDisplay = tempMonth;
/*        Debug.Log(tempMonth);*/
        int offset = (int) currDisplay.DayOfWeek;
        int count = 0;
        while (currDisplay.Month == tempMonth.Month)
        {
            Days[currDisplay.Day - 1 + offset].GetComponentInChildren<TextMeshProUGUI>().text = currDisplay.Day.ToString();
            Days[currDisplay.Day - 1 + offset].GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
            if (currDisplay.Year.Equals(currentTime.Year) && currDisplay.Month.Equals(currentTime.Month) && currDisplay.Day.Equals(currentTime.Day))
            {
                Days[currDisplay.Day - 1 + offset].GetComponentInChildren<TextMeshProUGUI>().color = new Color(1f, 176 / 255f, 160 / 255f, 1f);
                currentDayCircle.transform.position = Days[currDisplay.Day - 1 + offset].transform.position + new Vector3(0, -3, 0);

                /*smallCircle.transform.position = Days[currDisplay.Day - 1 + offset].transform.position + new Vector3(0, -60, 0);
                smallCircle.SetActive(true);*/

                count++;
            }

            for (int i = 0; i < circles.Count; i++)
            {
                DateTime checkDate = days[i];
                GameObject currCircle = circles[i];
                if (currDisplay.Year.Equals(checkDate.Year) && currDisplay.Month.Equals(checkDate.Month) && currDisplay.Day.Equals(checkDate.Day))
                {
                    currCircle.transform.position = Days[currDisplay.Day - 1 + offset].transform.position + new Vector3(0, -60, 0);
                    if (i == 0 && !todaySet)
                    {
                        continue;
                    }
                    currCircle.SetActive(true);
                }
            }

            DateTime forButtonDate = currDisplay;
            Days[currDisplay.Day - 1 + offset].GetComponent<Button>().onClick.AddListener(delegate { updateDate(forButtonDate, Days[forButtonDate.Day - 1 + offset]); });

            currDisplay = currDisplay.AddDays(1);
        }
        if (count == 0)
        {
            currentDayCircle.SetActive(false);
        }
        else
        {
            currentDayCircle.SetActive(true);
        }
    }

    public void nextMonth()
    {
        currentMonth++;
        if (currentMonth > 12)
        {
            currentMonth = 1;
            yearCounter++;
        }
        string year = (yearCounter != 0) ? " " + (DateTime.Now.Year + yearCounter) : "";
        CalendarHeader.text = Months[currentMonth - 1] + year;
        clearCircles();
        clearLabels();
        tempMonth = tempMonth.AddMonths(1);
        tempWeek = tempMonth;
        tempWeek = resetToSunday(tempWeek);
        CreateCalendar();
    }

    public void previousMonth()
    {
        currentMonth--;
        if (currentMonth < 1)
        {
            currentMonth = 12;
            yearCounter--;
        }
        string year = (yearCounter != 0) ? " " + (DateTime.Now.Year + yearCounter) : "";
        CalendarHeader.text = Months[currentMonth - 1] + year;
        clearLabels();
        clearCircles();
        tempMonth = tempMonth.AddMonths(-1);
        tempWeek = tempMonth;
        tempWeek = resetToSunday(tempWeek);
        CreateCalendar();
    }

    void CreateWeek()
    {
        currDisplay = tempWeek;
/*        Debug.Log(tempWeek);*/
        int count = 0;
        for (int i = 0; i < 7; i++)
        {
            Days[i].GetComponentInChildren<TextMeshProUGUI>().text = currDisplay.Day.ToString();
            Days[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
            if (currDisplay.Year.Equals(currentTime.Year) && currDisplay.Month.Equals(currentTime.Month) && currDisplay.Day.Equals(currentTime.Day))
            {
                Days[i].GetComponentInChildren<TextMeshProUGUI>().color = new Color(1f, 176 / 255f, 160 / 255f, 1f);
                currentDayCircle.transform.position = Days[i].transform.position + new Vector3(0, -3, 0);

                /*smallCircle.transform.position = Days[i].transform.position + new Vector3(0, -60, 0);
                smallCircle.SetActive(true);*/

                count++;
            }

            for (int j = 0; j < circles.Count; j++)
            {
                DateTime checkDate = days[j];
                GameObject currCircle = circles[j];
                if (currDisplay.Year.Equals(checkDate.Year) && currDisplay.Month.Equals(checkDate.Month) && currDisplay.Day.Equals(checkDate.Day))
                {
                    /*Days[currDisplay.Day - 1 + offset].GetComponentInChildren<TextMeshProUGUI>().color = new Color(1f, 176 / 255f, 160 / 255f, 1f);*/
                    currCircle.transform.position = Days[i].transform.position + new Vector3(0, -60, 0);
                    if (j == 0 && !todaySet)
                    {
                        continue;
                    }
                    currCircle.SetActive(true);
                }
            }
            DateTime forButtonDate = currDisplay;
            int num = i;
            /*Days[i].GetComponent<Button>().onClick.AddListener(() => updateDate(currDisplay));*/
            /*Debug.Log(currDisplay.ToString());*/
            Days[i].GetComponent<Button>().onClick.AddListener(delegate{ updateDate(forButtonDate, Days[num]); });

            currDisplay = currDisplay.AddDays(1);
        }
        if (count == 0)
        {
            currentDayCircle.SetActive(false);
        }
        else
        {
            currentDayCircle.SetActive(true);
        }
    }

    public void nextWeek()
    {
        int prevMonth = tempWeek.AddDays(3).Month;
        tempWeek = tempWeek.AddDays(7);
        /*DateTime checkWeek = tempWeek.AddDays(6);*/
        DateTime checkWeek = tempWeek.AddDays(3);
        int newMonth = checkWeek.Month;
        /*if (checkWeek.Day > 3 && prevMonth != newMonth)*/
        if (prevMonth != newMonth)
        {
            currentMonth++;
            if (currentMonth > 12)
            {
                currentMonth = 1;
                yearCounter++;
            }
            tempMonth = tempMonth.AddMonths(1);
        }
  
        string year = (yearCounter != 0) ? " " + (DateTime.Now.Year + yearCounter) : "";
        CalendarHeader.text = Months[currentMonth - 1] + year;
        clearLabels();
        clearCircles();
        CreateCalendar();
    }

    public void previousWeek()
    {
        int prevMonth = tempWeek.AddDays(3).Month;
        tempWeek = tempWeek.AddDays(-7);
        DateTime checkWeek = tempWeek.AddDays(3);
        int newMonth = checkWeek.Month;
        /*if (checkWeek.Day >= 28 && prevMonth != newMonth)*/
        if (prevMonth != newMonth)
        {
            currentMonth--;
            if (currentMonth < 1)
            {
                currentMonth = 12;
                yearCounter--;
            }
            tempMonth = tempMonth.AddMonths(-1);
        }
        string year = (yearCounter != 0) ? " " + (DateTime.Now.Year + yearCounter) : "";
        CalendarHeader.text = Months[currentMonth - 1] + year;
        clearLabels();
        clearCircles();
        CreateCalendar();
    }

    void clearLabels()
    {
        for (int  i = 0; i < Days.Length; i++)
        {
            Days[i].GetComponentInChildren<TextMeshProUGUI>().text = null;
            Days[i].GetComponent<Button>().onClick.RemoveAllListeners();
        }
    }

    DateTime resetToSunday(DateTime toReset)
    {
        while (toReset.DayOfWeek != 0)
        {
            toReset = toReset.AddDays(-1);
        }
        return toReset;
    }
}
