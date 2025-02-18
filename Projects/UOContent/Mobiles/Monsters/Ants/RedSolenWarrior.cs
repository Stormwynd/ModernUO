using ModernUO.Serialization;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class RedSolenWarrior : BaseCreature
    {
        [SerializableField(0, setter: "private")]
        private bool _burstSac;

        [Constructible]
        public RedSolenWarrior() : base(AIType.AI_Melee)
        {
            Body = 782;
            BaseSoundID = 959;

            SetStr(196, 220);
            SetDex(101, 125);
            SetInt(36, 60);

            SetHits(96, 107);

            SetDamage(5, 15);

            SetDamageType(ResistanceType.Physical, 80);
            SetDamageType(ResistanceType.Poison, 20);

            SetResistance(ResistanceType.Physical, 20, 35);
            SetResistance(ResistanceType.Fire, 20, 35);
            SetResistance(ResistanceType.Cold, 10, 25);
            SetResistance(ResistanceType.Poison, 20, 35);
            SetResistance(ResistanceType.Energy, 10, 25);

            SetSkill(SkillName.MagicResist, 60.0);
            SetSkill(SkillName.Tactics, 80.0);
            SetSkill(SkillName.Wrestling, 80.0);

            Fame = 3000;
            Karma = -3000;

            VirtualArmor = 35;

            SolenHelper.PackPicnicBasket(this);
            PackItem(new ZoogiFungus(Utility.RandomDouble() < 0.95 ? 3 : 13));

            if (Utility.RandomDouble() < 0.05)
            {
                PackItem(new BraceletOfBinding());
            }
        }

        public override string CorpseName => "a solen warrior corpse";

        public override string DefaultName => "a red solen warrior";

        public override int GetAngerSound() => 0xB5;

        public override int GetIdleSound() => 0xB5;

        public override int GetAttackSound() => 0x289;

        public override int GetHurtSound() => 0xBC;

        public override int GetDeathSound() => 0xE4;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(1, 4));
        }

        public override bool IsEnemy(Mobile m)
        {
            if (SolenHelper.CheckRedFriendship(m))
            {
                return false;
            }

            return base.IsEnemy(m);
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            SolenHelper.OnRedDamage(from);

            if (!willKill)
            {
                if (!BurstSac)
                {
                    if (Hits < 50)
                    {
                        PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The solen's acid sac is burst open! *");
                        BurstSac = true;
                    }
                }
                else if (from != null && from != this && InRange(from, 1))
                {
                    SpillAcid(from, 1);
                }
            }

            base.OnDamage(amount, from, willKill);
        }

        public override bool OnBeforeDeath()
        {
            SpillAcid(4);

            return base.OnBeforeDeath();
        }
    }
}
