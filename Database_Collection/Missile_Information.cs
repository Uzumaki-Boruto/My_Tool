﻿using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace Database_Collection
{
    class Missile_Information
    {
        public string SpellCaster { get; set; }
        public SpellSlot Slot { get; set; }
        public bool CanMoveWhileChanneling { get; set; }
        public float ConeCastAngle { get; set; }
        public float CastRadius { get; set; }
        public float CastRange { get; set; }
        public string DisplayName { get; set; }
        public bool IsToggle { get; set; }
        public float LineWidth { get; set; }
        public float MaxSpeed { get; set; }
        public float MinSpeed { get; set; }
        public float Speed { get; set; }
        public string Name { get; set; }
        public bool IsTarget { get; set; }
        public SkillShotType Type { get; set; }
        public SpellDataTargetType TargetType { get; set; }

        public Missile_Information() { }

        public Missile_Information(MissileClient missile)
        {
            SpellCaster = missile.SpellCaster.BaseSkinName;
            Slot = missile.Slot;
            CanMoveWhileChanneling = missile.SData.CanMoveWhileChanneling;
            ConeCastAngle = missile.SData.CastConeAngle;
            CastRadius = missile.SData.CastRadius;
            CastRange = missile.SData.CastRange;
            DisplayName = missile.SData.DisplayName;
            IsToggle = missile.SData.IsToggleSpell;
            LineWidth = missile.SData.LineWidth;
            MaxSpeed = missile.SData.MissileMaxSpeed;
            MinSpeed = missile.SData.MissileMinSpeed;
            Speed = missile.SData.MissileSpeed;
            Name = missile.SData.Name;
            IsTarget = missile.Target != null;
            TargetType = missile.SData.TargettingType;
        }
        public Missile_Information(Spell.Skillshot spell)
        {
            SpellCaster = Player.Instance.ChampionName;
            Slot = spell.Slot;
            CanMoveWhileChanneling = spell.Handle.SData.CanMoveWhileChanneling;
            ConeCastAngle = spell.ConeAngleDegrees;
            CastRadius = spell.Radius;
            CastRange = spell.Range;
            DisplayName = spell.Name;
            IsToggle = spell.Handle.SData.IsToggleSpell;
            LineWidth = spell.Width;
            MaxSpeed = spell.Handle.SData.MissileMaxSpeed;
            MinSpeed = spell.Handle.SData.MissileMinSpeed;
            Speed = spell.Speed;
            Name = spell.Handle.Name;
            IsTarget = false;
            Type = spell.Type;
            TargetType = spell.Handle.SData.TargettingType;          
        }
    }
}
