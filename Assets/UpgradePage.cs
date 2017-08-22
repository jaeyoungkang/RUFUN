namespace MoenenVoxel
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class UpgradePage : MonoBehaviour
    {
        public float powerValue = 10;

        // Use this for initialization
        void Start()
        {

        }

        public void OnClickPower()
        {
            DemoStage.Main.Power += powerValue;
            CloseWin();
        }
        public void OnClickSpeed()
        {
            CloseWin();
        }
        public void OnClickTime()
        {
            CloseWin();
        }

        void CloseWin()
        {
            gameObject.SetActive(false);
        }
    }
}