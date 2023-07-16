using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {
    public int Id { get; set; }
    public int Depth { get; set; }
    public Vector3 Position { get; set; }
    public Encounter EncounterType { get; set; }
    public enum Encounter
    {
        ENEMY,
        ELITE,
        REST,
        EVENT,
        CHEST,
        BOSS
    }
}
