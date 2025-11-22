using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public abstract class ImprovementEffect : ScriptableObject
{
    public abstract void Apply(Player player);
}
