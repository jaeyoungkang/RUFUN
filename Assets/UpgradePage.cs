namespace MoenenVoxel
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class UpgradePage : MonoBehaviour
    {
        public float powerValue = 10;
		public float speedValue = 100;
		public float timeValue = 10;

        // Use this for initialization
        void Start()
        {

        }

        public void OnClickPower()
        {
            DemoStage.Main.AddPower += powerValue;
            CloseWin();
        }
        public void OnClickSpeed()
        {
			DemoStage.Main.AddSpeed += speedValue;
            CloseWin();
        }
        public void OnClickTime()
        {
			DemoStage.Main.AddTime += timeValue;
            CloseWin();
        }

        void CloseWin()
        {
            gameObject.SetActive(false);
        }
    }
}