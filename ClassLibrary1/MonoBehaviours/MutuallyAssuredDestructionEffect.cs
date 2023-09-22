using ModdingUtils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Photon.Pun;
using UnboundLib;
using UnityEngine;
using System.Reflection;
using UnboundLib.Networking;
using ModdingUtils.MonoBehaviours;
using System.Runtime.CompilerServices;


/*******************************************
 * Coded by DevLearnDev.
 * Credit to Pykess's implementation of KingMidasEffect in PCE for inspiration on how to code this.
*******************************************/

namespace Redo.MonoBehaviours
{
    public class MutuallyAssuredDestructionEffect : MonoBehaviour
    {
        private Player player;
        private readonly float range = 1.75f;

        void Awake()
        {
            this.player = this.gameObject.GetComponent<Player>();
        }

        void Start()
        {

        }

        void Update()
        {
            if (ModdingUtils.Utils.PlayerStatus.PlayerAliveAndSimulated(this.player))
            {
                List<Player> otherPlayers = PlayerManager.instance.players.Where(player => ModdingUtils.Utils.PlayerStatus.PlayerAliveAndSimulated(player) && (player.playerID != this.player.playerID)).ToList();
                Vector2 displacement;

                foreach (Player otherPlayer in otherPlayers)
                {
                    displacement = otherPlayer.transform.position - this.player.transform.position;
                    if (displacement.magnitude <= this.range)
                    {
                        if (PhotonNetwork.OfflineMode)
                        {
                            typeof(HealthHandler).InvokeMember("RPCA_Die",
                                        BindingFlags.Instance | BindingFlags.InvokeMethod |
                                        BindingFlags.NonPublic, null, otherPlayer.data.healthHandler,
                                        new object[] { new Vector2(0, 1) });

                            Unbound.Instance.ExecuteAfterSeconds(0.1f, delegate
                            {
                                typeof(HealthHandler).InvokeMember("RPCA_Die",
                                            BindingFlags.Instance | BindingFlags.InvokeMethod |
                                            BindingFlags.NonPublic, null, this.player.data.healthHandler,
                                            new object[] { new Vector2(0, 1) });

                            });
                        }

                        else if (this.player.GetComponent<PhotonView>().IsMine)
                        {
                            NetworkingManager.RPC(typeof(MutuallyAssuredDestructionEffect), "RPCA_Explode", new object[] { otherPlayer.data.view.ControllerActorNr });
                            Unbound.Instance.ExecuteAfterSeconds(0.1f, delegate
                            {
                                NetworkingManager.RPC(typeof(MutuallyAssuredDestructionEffect), "RPCA_Explode", new object[] { this.player.data.view.ControllerActorNr });
                            });
                        }


                    }
                }
            }
        }

        public void OnDestroy()
        {
        }
        public void Destroy()
        {
            UnityEngine.Object.Destroy(this);
        }

        [UnboundRPC]
        private static void RPCA_Explode(int actorID)
        {
            Player playerToEffect = (Player)typeof(PlayerManager).InvokeMember("GetPlayerWithActorID",
                    BindingFlags.Instance | BindingFlags.InvokeMethod |
                    BindingFlags.NonPublic, null, PlayerManager.instance, new object[] { actorID });

            typeof(HealthHandler).InvokeMember("RPCA_Die",
                        BindingFlags.Instance | BindingFlags.InvokeMethod |
                        BindingFlags.NonPublic, null, playerToEffect.data.healthHandler,
                        new object[] { new Vector2(0, 1) });
        }
    }
}
