namespace MoenenVoxel {

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum WeaponType {
	None = 0,
	Hand = 1,
	Axe,
	AxeShort,
	Crossbow,
	Hammer,
	HammerShort,
	Pistol,
	Shield,
	LongShield,
	Shotgun,
	Spear,
	SpearShort,
	Staff,
	Wand,
	Sword,
	SwordShort,
}

public enum WeaponBehaviour {
	None = 0,

	Hand = 1,
	HandMagic = 2,

	Single = 3,
	SingleMagic = 4,
	SingleGun = 5,

	Double = 6,
	DoubleGun = 7,

	TwoHands = 8,
	TwoHandsMagic = 9,
	TwoHandsGun = 10
}


[System.Serializable]
public struct WeaponInfo {


	public WeaponType Type {
		get {
			return _type;
		}
	}

	public WeaponBehaviour WeaponBehaviour {
		get {
			return weaponBehaviour;
		}
	}

	public float MinDamage {
		get {
			return minDamage;
		}
	}

	public float MaxDamage {
		get {
			return maxDamage;
		}
	}

	public float Damage {
		get {
			return Random.Range(minDamage, maxDamage);
		}
	}

	public float AttackSpeed1 {
		get {
			return attackSpeed1;
		}
	}

	public float AttackSpeed2 {
		get {
			return attackSpeed2;
		}
	}

	public float AttackRange {
		get {
			return attackRange;
		}
	}


	[SerializeField]
	private WeaponType _type;
	[SerializeField]
	private WeaponBehaviour weaponBehaviour;
	[SerializeField]
	[Range(0f, 1000f)]
	private float minDamage;
	[SerializeField]
	[Range(0f, 1000f)]
	private float maxDamage;
	[SerializeField]
	[Range(0.1f, 100f)]
	private float attackSpeed1;
	[SerializeField]
	[Range(0.1f, 100f)]
	private float attackSpeed2;
	[SerializeField]
	[Range(0.01f, 100f)]
	private float attackRange;


	public void Default () {
		_type = WeaponType.Hand;
		weaponBehaviour = WeaponBehaviour.Hand;
		minDamage = 5;
		maxDamage = 6;
		attackSpeed1 = 4;
		attackSpeed2 = 3;
		attackRange = 1;
	}




}




public class Weapon : Item {

	
	#region -------- VAR --------


	public WeaponInfo WeaponInfo;

	public override ItemBehaviour ItemBehaviourID {
		get {
			return (
				WeaponInfo.WeaponBehaviour == WeaponBehaviour.TwoHands ||
				WeaponInfo.WeaponBehaviour == WeaponBehaviour.TwoHandsGun ||
				WeaponInfo.WeaponBehaviour == WeaponBehaviour.TwoHandsMagic
				) ? 
				ItemBehaviour.TwoHands: (
				WeaponInfo.WeaponBehaviour == WeaponBehaviour.Double ||
				WeaponInfo.WeaponBehaviour == WeaponBehaviour.DoubleGun
				) ? 
				ItemBehaviour.Double :(
				WeaponInfo.WeaponBehaviour == WeaponBehaviour.Single ||
				WeaponInfo.WeaponBehaviour == WeaponBehaviour.SingleGun ||
				WeaponInfo.WeaponBehaviour == WeaponBehaviour.SingleMagic
				) ? ItemBehaviour.Single :
				ItemBehaviour.None;
		}
	}


	#endregion


	#region -------- MSG --------



	public void OnAttack1 (Transform tf) {

		float h = 1f;
		float r = 0.5f;
		CharacterBehaviour ch = tf.GetComponent<CharacterBehaviour>();
		if (ch) {
			CharacterController chc = tf.GetComponent<CharacterController>();
			h = chc.height;
			r = chc.radius;
		}
        
            MakeAttack(tf, h, r, WeaponInfo.Damage + DemoStage.Main.Power, false);
        }


	public void OnAttack2 (Transform tf) {

		float h = 1f;
		float r = 0.5f;
		CharacterBehaviour ch = tf.GetComponent<CharacterBehaviour>();
		if (ch) {
			CharacterController chc = tf.GetComponent<CharacterController>();
			h = chc.height;
			r = chc.radius;
		}

            float addDamage = 0f;
            if (DemoStage.Main.powerUpCount > 0)
            {
                addDamage = 100f;
                DemoStage.Main.powerUpCount--;
            }

            MakeAttack(tf, h, r * 1.5f, WeaponInfo.Damage * 1.5f + addDamage, true);
//            MakeAttack(tf, h, r * 1.5f, WeaponInfo.Damage * 1.5f + DemoStage.Main.GetWeaponPower2(), true);

	}


	public void StopAttack (Transform tf) {



	}

	#endregion


	private void MakeAttack (Transform tf, float h, float r, float d, bool muti = false) {
		

		RaycastHit[] hits = Physics.BoxCastAll(
			tf.position + Vector3.up * h * 0.5f,
			new Vector3(r, h, r),
			-tf.forward,
			Quaternion.identity,
			WeaponInfo.AttackRange
		);

		List<CharacterBehaviour> chList = new List<CharacterBehaviour>();

		for (int i = 0; i < hits.Length; i++) {
			
			EnemyBehaviour en = hits[i].transform.GetComponent<EnemyBehaviour>();

			if (en && !chList.Contains(en) && (chList.Count == 0 || muti)) {
				
                if(en.isBoss) 
				{
					en.BossHurt(d, tf);
				}
                else 
				{
					en.Hurt(d, tf);
				}

				chList.Add(en);
			} 

			Rigidbody rig = hits[i].transform.GetComponent<Rigidbody>();
			if (rig) {
				rig.AddExplosionForce(250f, tf.position, 2.5f);
			}

		}

		chList = null;

		DemoStage.PlaySound((int)Random.Range(8f,10.99f), 0.25f);
		
	}



}
}
