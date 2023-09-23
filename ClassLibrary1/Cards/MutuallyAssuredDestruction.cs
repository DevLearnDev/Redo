using Redo.MonoBehaviours;
using UnboundLib;
using UnboundLib.Cards;
using UnityEngine;
using static CardInfo;


namespace RedoNameSpace.Cards
{
    class MutuallyAssuredDestruction : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            statModifiers.movementSpeed = 2f;
            statModifiers.health = 0.4f;
            cardInfo.allowMultiple = false;
            //Edits values on card itself, which are then applied to the player in `ApplyCardStats`
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            //Edits values on player when card is selected

            MutuallyAssuredDestructionEffect mutuallyAssuredDestructionEffect = player.gameObject.GetOrAddComponent<MutuallyAssuredDestructionEffect>();
            (GameObject AddToProjectile, GameObject effect, _) = LoadExplosion("MADExplosion");
            mutuallyAssuredDestructionEffect.Explosion = new ObjectsToSpawn
            {
                AddToProjectile = AddToProjectile,
                direction = ObjectsToSpawn.Direction.forward,
                effect = effect,
                normalOffset = 0.1f,
                scaleFromDamage = 0.5f,
                scaleStackM = 0.7f,
                scaleStacks = true,
                spawnAsChild = false,
                spawnOn = ObjectsToSpawn.SpawnOn.all,
                stacks = 0,
                stickToAllTargets = false,
                stickToBigTargets = false,
                zeroZ = false
            };
            effect.GetOrAddComponent<SpawnedAttack>().spawner = player;
            player.gameObject.GetOrAddComponent<MutuallyAssuredDestructionEffect>();

        }
        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            //Run when the card is removed from the player
        }


        protected override string GetTitle()
        {
            return "Mutually Assured Destruction";
        }
        protected override string GetDescription()
        {
            return "Touching another player will kill you both.";
        }
        protected override GameObject GetCardArt()
        {
            return null;
        }
        protected override CardInfo.Rarity GetRarity()
        {
            return CardInfo.Rarity.Rare;
        }
        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat()
                {
                    positive = true,
                    stat = "Speed",
                    amount = "+100%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat()
                {
                    positive = false,
                    stat = "Health",
                    amount = "-60%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                }
            };
        }
        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.DestructiveRed;
        }
        public override string GetModName()
        {
            return Redo.ModInitials;
        }

        public static (GameObject AddToProjectile, GameObject effect, Explosion explosion) LoadExplosion(string name, Gun? gun = null)
        {
            // load explosion effect from Explosive Bullet card
            GameObject? explosiveBullet = (GameObject)Resources.Load("0 cards/Explosive bullet");

            Gun explosiveGun = explosiveBullet.GetComponent<Gun>();

            if (gun != null)
            {
                // change the gun sounds
                gun.soundGun.AddSoundImpactModifier(explosiveGun.soundImpactModifier);
            }

            // load assets
            GameObject A_ExplosionSpark = explosiveGun.objectsToSpawn[0].AddToProjectile;
            GameObject explosionCustom = Instantiate(explosiveGun.objectsToSpawn[0].effect);
            explosionCustom.transform.position = new Vector3(1000, 0, 0);
            explosionCustom.hideFlags = HideFlags.HideAndDontSave;
            explosionCustom.name = name;
            DestroyImmediate(explosionCustom.GetComponent<RemoveAfterSeconds>());
            Explosion explosion = explosionCustom.GetComponent<Explosion>();

            return (A_ExplosionSpark, explosionCustom, explosion);
        }
    }
}
