using ModdingUtils.Extensions;
using System.Collections.Generic;
using System.Linq;
using UnboundLib.Cards;
using UnityEngine;
using static CardInfo;


namespace RedoNameSpace.Cards
{
    class FraternalTwins : CustomCard
    {


        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            cardInfo.GetAdditionalData().canBeReassigned = false;

            //Edits values on card itself, which are then applied to the player in `ApplyCardStats`
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            //Edits values on player when card is select
            List<Player> players = new List<Player>(PlayerManager.instance.players);
            players.Remove(player);
            Player randomEnemy = players[UnityEngine.Random.Range(0, players.Count)];
            CardInfo[] cards = randomEnemy.data.currentCards.Where(card => card.GetAdditionalData().canBeReassigned).ToArray();

            ModdingUtils.Utils.Cards.instance.RemoveAllCardsFromPlayer(player);
            ModdingUtils.Utils.Cards.instance.AddCardsToPlayer(player, cards, false, null, null, null, true);
        }
        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            //Run when the card is removed from the player
        }


        protected override string GetTitle()
        {
            return "Fraternal Twins";
        }
        protected override string GetDescription()
        {
            return "Replace all your cards with a copy of a random player's cards.";
        }
        protected override GameObject GetCardArt()
        {
            return null;
        }
        protected override CardInfo.Rarity GetRarity()
        {
            return CardInfo.Rarity.Uncommon;
        }
        protected override CardInfoStat[] GetStats()
        {
            return null;
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
