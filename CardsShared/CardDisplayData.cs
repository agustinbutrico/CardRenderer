using System.Collections.Generic;
using UnityEngine;

namespace CardsShared
{
    public enum Category { Tower, DOT, Building, Buff, Monster }

    public enum Subcategory
    {
        Ballista, Mortar, TeslaCoil,
        FrostKeep, FlameThrower, PoisonSprayer,
        Shredder, Encampment, Lookout,
        VampireLair, Cannon, Monument,
        Radar, Obelisk, ParticleCannon,
        DOTBleed, DOTBurn, DOTPoison,
        ManaSiphon, Mine, ManaBank, HauntedHouse, University,
        DrawBuff, GoldBuff, ManaBuff, CritsBuff, TowerBuff, TreeBuff,
        MonsterHealthBuff, MonsterArmorBuff, MonsterShieldBuff, MonsterDamageBuff,
        MonsterMoveSpeedBuff, MonsterSlowDebuff, MonsterHasteBuff, MonsterTowerDamageBuff, MonsterGoldDebuff
    }

    [System.Flags]
    public enum AcquisitionType
    {
        None = 0,
        AlwaysInRun = 1 << 0,
        StoreUnlocksInRun = 1 << 1,
        StoreAppliesPermanent = 1 << 2
    }

    public class CardDisplayData
    {
        public string Title;
        public string Description;
        public string SpriteName;
        public Sprite Sprite;
        public string UnlockName;
        public bool UnlockedByDefault;
        public bool Unlocked;
        public List<string> Unlocks;
        public Category Category;
        public Subcategory Subcategory;
        public AcquisitionType AcquisitionType;
    }
}
