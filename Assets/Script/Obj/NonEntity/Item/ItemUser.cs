using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ItemUser
{
    void AddHealth(int restoredHealth);
    void UnlockSkill(SkillType unlockedSkill);
    void AddCoin(int add);
}
