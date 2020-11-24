﻿using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ProfileSaver Object/Location", order = 1)]
public class LocationInformation : ScriptableObject
{
    public string locationName;
    public float openingTime;
    public float closingTime;
    public Sprite locationSprite;

    public List<Choices> thingsToDo;
}