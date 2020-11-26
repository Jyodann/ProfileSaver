﻿using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Profile[] profiles;
    public bool isInDevelopment = true;
    private Decision[] decisions;
    public static GameManager instance;
    public Profile currentProfile;

    private float _health = 50f;
    private float _happiness = 50f;
    private float _energy = 50f;
    private float _hunger = 10f;
    private string[] days = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };

    #region Variable Declaration

    public float Health
    {
        get => _health;
        set
        {
            if (value <= 100)
            {
                _health = value;
            }
        }
    }

    public float Happiness
    {
        get => _happiness;
        set
        {
            if (value <= 100)
            {
                _happiness = value;
            }
        }
    }

    public float Money { get; set; } = 500f;

    private float timeInHours = 0f;

    public float Energy
    {
        get => _energy;
        set
        {
            if (value <= 100)
            {
                _energy = value;
            }
        }
    }

    public float Hunger
    {
        get => _hunger;
        set
        {
            if (value <= 10)
            {
                _hunger = value;
            }
        }
    }

    #endregion Variable Declaration

    private void Awake()
    {
        profiles = Resources.LoadAll<Profile>("Profiles");
        decisions = Resources.LoadAll<Decision>("Decisions");

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        if (instance.currentProfile == null && !isInDevelopment)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            currentProfile = profiles.Where(x => x.profileName == "Student").First();
            SetUpValues();
        }
    }

    public Profile ReturnRandomProfile()
    {
        currentProfile = profiles[Random.Range(0, profiles.Length)];
        return currentProfile;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            AddTime(2.5f);
            print(ReturnTimeString());
            print(ReturnDayOfWeek());
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            SetTime(23.5f);
        }
    }

    public float ReturnTime()
    {
        return timeInHours;
    }

    public void SetTime(float timeToSet)
    {
        if (timeToSet > 24)
        {
            Debug.LogWarning("Invalid time set: " + timeToSet);
            return;
        }
        timeInHours = (ReturnDayNumber() - 1) * 24;
        timeInHours += timeToSet;
    }

    public float AddTime(float timeToIncrease)
    {
        timeInHours += timeToIncrease;
        return timeInHours;
    }

    public int ReturnDayNumber()
    {
        return (int)timeInHours / 24 + 1;
    }

    public int ReturnHour()
    {
        return (int)timeInHours % 24;
    }

    public float ReturnHourRaw()
    {
        return timeInHours % 24;
    }

    public float ReturnMinutes()
    {
        return (timeInHours % 24 % 1) * 60;
    }

    public string ReturnTimeString()
    {
        var isWeekend = ReturnDayNumber() % 7 == 0 || (ReturnDayNumber() + 1) % 7 == 0;

        return $"{ReturnHour().ToString().PadLeft(2, '0')}:{ReturnMinutes().ToString().PadLeft(2, '0')} " +
        $"DAY {ReturnDayNumber()} ({ReturnDayOfWeek()})";
    }

    public string ReturnDayOfWeek()
    {
        var dayTest = ReturnDayNumber() % 7;
        return days[dayTest];
    }

    public Decision ReturnRandomDecision()
    {
        return decisions[Random.Range(0, decisions.Length)];
    }

    public void SetUpValues(float health, float happiness, float money, float time, float hunger, float energy)
    {
        instance.Energy = energy;
        instance.Health = health;
        instance.Money = money;
        instance.SetTime(time);
        instance.Hunger = hunger;
        instance.Happiness = happiness;
    }

    public void SetUpValues()
    {
        instance.Energy = 50f;
        instance.Health = 50f;
        instance.Money = instance.currentProfile.income;
        instance.SetTime(instance.currentProfile.timeToWake);
        instance.Hunger = 10f;
        instance.Happiness = 50f;
    }
}