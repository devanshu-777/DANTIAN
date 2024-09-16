using DG.Tweening.Core.Easing;
using Mirror;
using Mirror.Examples.Pong;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mirror.Examples.Pong
{
    public class PongLogic : NetworkBehaviour
    {
        public static PongLogic instance;

        [SyncVar]
        public int player1Score = 0;

        [SyncVar]
        public int player2Score = 0;

        public Text p1Text;
        public Text p2Text;

        void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                instance = this;
            }
        }

        [Command(requiresAuthority = false)]
        public void CmdIncrementScore(int playerIndex)
        {
            if (playerIndex == 1)
            {
                player1Score++;
            }
            else if (playerIndex == 2)
            {
                player2Score++;
            }
        }

        void UpdateUI()
        {
            p1Text.text = player1Score.ToString();
            p2Text.text = player2Score.ToString();
        }

        void Start()
        {
            UpdateUI();
        }

        void Update()
        {
            UpdateUI();
        }
    }
}