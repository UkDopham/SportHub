using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MovementData")]
public class MovementDetectionData : ScriptableObject
{
    [SerializeField]
    public bool isLock = false;

    [SerializeField]
    public int Repetition = 5;

    [SerializeField]
    public float TimeInterval = 0.5f;

    [SerializeField]
    public List<BodyTransform> Movements = new List<BodyTransform>();
}
