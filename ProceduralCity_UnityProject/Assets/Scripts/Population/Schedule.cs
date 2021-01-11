using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Schedule", menuName = "Scriptables/Schedule", order = 1)]
public class Schedule : ScriptableObject
{
    public ActionList[] actions = new ActionList[24];
}
