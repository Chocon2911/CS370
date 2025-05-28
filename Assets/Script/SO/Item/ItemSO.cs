using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Item")]
public class ItemSO : ScriptableObject
{
    public bool IsRestorable;
    public SkillType UnlockedSkill;
    public int RestoredHealth;
}
