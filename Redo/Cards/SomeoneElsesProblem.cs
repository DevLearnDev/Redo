using ModdingUtils.Extensions;
using System.Collections.Generic;
using System.Linq;
using UnboundLib.Cards;
using UnityEngine;
using static CardInfo;


namespace RedoNameSpace.Cards
{
    class SomeoneElsesProblem : CustomCard
    {


        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            cardInfo.GetAdditionalData().canBeReassigned = false;

            //Edits values on card itself, which are then applied to the player in `ApplyCardStats`
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            //Get random enemy.
            List<Player> players = new List<Player>(PlayerManager.instance.players);
            players.Remove(player);
            Player randomEnemy = players[UnityEngine.Random.Range(0, players.Count)];

            // Make a copy of currents cards.
            var currentCardsCopy = data.currentCards.Where(card => card.GetAdditionalData().canBeReassigned).ToArray();

            // Take enemy's cards
            CardInfo[] enemyCards = randomEnemy.data.currentCards.Where(card => card.GetAdditionalData().canBeReassigned).ToArray();

            ModdingUtils.Utils.Cards.instance.RemoveAllCardsFromPlayer(player);
            ModdingUtils.Utils.Cards.instance.AddCardsToPlayer(player, enemyCards, false, null, null, null, true);

            // Give enemy your cards.
            ModdingUtils.Utils.Cards.instance.RemoveAllCardsFromPlayer(randomEnemy);
            ModdingUtils.Utils.Cards.instance.AddCardsToPlayer(randomEnemy, currentCardsCopy, false, null, null, null, true);
        }
        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            //Run when the card is removed from the player
        }


        protected override string GetTitle()
        {
            return "Someone Else's Problem";
        }
        protected override string GetDescription()
        {
            return "Swap all your cards with a those of a random player.";
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
                    stat = "Effect",
                    amount = "No",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                }
            };
        }
        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.ColdBlue;
        }
        public override string GetModName()
        {
            return Redo.ModInitials;
        }
    }
}
