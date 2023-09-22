using UnboundLib;
using UnboundLib.Cards;
using RedoNameSpace.Cards;
using HarmonyLib;
using CardChoiceSpawnUniqueCardPatch.CustomCategories;
using BepInEx;

namespace RedoNameSpace
{
    [BepInDependency("com.willis.rounds.unbound", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("pykess.rounds.plugins.moddingutils", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("pykess.rounds.plugins.cardchoicespawnuniquecardpatch", BepInDependency.DependencyFlags.HardDependency)]
    // Declares our mod to Bepin
    [BepInPlugin(ModId, ModName, Version)]
    // The game our mod is associated with
    [BepInProcess("Rounds.exe")]
    public class Redo : BaseUnityPlugin
    {
        private const string ModId = "com.devmung.rounds.Redo";
        private const string ModName = "Redo";
        public const string Version = "1.0.0"; // What version are we on (major.minor.patch)?
        public const string ModInitials = "REDO";

        public static Redo instance { get; private set; }


        void Awake()
        {
            // Use this to call any harmony patch files your mod may have
            var harmony = new Harmony(ModId);
            harmony.PatchAll();
        }
        void Start()
        {
            instance = this;
            CustomCard.BuildCard<BackToBasics>();
            CustomCard.BuildCard<FraternalTwins>();
            CustomCard.BuildCard<SomeoneElsesProblem>();
            CustomCard.BuildCard<HeavyWeaponsGuy>();
        }
    }
}
