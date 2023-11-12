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

    [SerializeField] private int currentMonth = DateTime.Now.Month;
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


    // Start is called before the first frame update
    void Start()
    {
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
        clearLabels();
        CreateMonths();
        CreateCalendar();
        currentDay = (DateTime.Now.Day);
    }

    // Update is called once per frame
    /*void Update()
    {
        
    }*/

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
                count++;
            }
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
                Days[i].GetComponentInChildren<TextMeshProUGUI>().color = new Color(1f, 176/255f, 160/255f, 1f);
                currentDayCircle.transform.position = Days[i].transform.position + new Vector3(0, -3, 0);
                count++;
            }
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
        CreateCalendar();
    }

    void clearLabels()
    {
        for (int  i = 0; i < Days.Length; i++)
        {
            Days[i].GetComponentInChildren<TextMeshProUGUI>().text = null;
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
