namespace MoenenVoxel {
using UnityEngine;
using System.Collections;

public class EnemyBehaviour : CharacterBehaviour {

	public Transform BloodCube, RedBloodCube;
	public Transform RagDoll;
	[HideInInspector]
	public float HP = 1f;
	[HideInInspector]
	public float MaxHP = 1f;

	private Vector2 AimMove;
	private Quaternion AimRotation;
	private bool Alive = true;
	private float LastHurtTime = -100f;
	private float LastCheckTime = -100f;
        Transform EnemySign;

        public string sign;

    void Start () {
		AimMove = Vector3.zero;
		AimRotation = Quaternion.identity;
		Alive = true;

        string ename = gameObject.name + "_sign";
        GameObject enemySign = new GameObject(ename);
        EnemySign = enemySign.transform;
        EnemySign.rotation = Camera.main.transform.rotation;
        TextMesh etm = enemySign.AddComponent<TextMesh>();
        etm.text = sign;
        etm.color = new Color(0.8f, 0.8f, 0.8f);
        etm.fontStyle = FontStyle.Bold;
        etm.alignment = TextAlignment.Center;
        etm.anchor = TextAnchor.MiddleCenter;
        etm.characterSize = 0.065f;
        etm.fontSize = 60;            
        }



	protected override void Update () {

            // transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * 3, 0.1f);
            EnemySign.position = gameObject.transform.position + Vector3.up * 3f;

            AimRotation = Quaternion.Lerp(AimRotation, Quaternion.Euler(
			0f, (AimMove.x > 0f ? 1f : -1f) * Vector2.Angle(Vector2.up, AimMove) + 180f, 0f
		), 0.2f);

		if (Time.time > LastCheckTime + 1f) {
			LastCheckTime = Time.time;
			ChangeDir();
			DashMaybe();
			JumpMaybe();
		}

		if (Time.time - LastHurtTime < 0.3f) {
			base.RotateDependCamera(Quaternion.Euler(Random.Range(-90f, 90f), AimRotation.eulerAngles.y, Random.Range(-90f, 90f)));
			base.Stop();
		} else {
			if (base.RunIfMove) {
				base.RunDependCamera(AimMove);
			} else {
				base.WalkDependCamera(AimMove);
			}
			base.RotateDependCamera(AimRotation);
		}

		transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * 3, 0.1f);

		if (!DemoStage.Playing && Alive) {
			Die(transform);
		}

		base.Update();
	}


	void ChangeDir () {
            // if (gameObject.name.Contains("Tiger"))
            // {
            //     AimMove = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            //     base.RunIfMove = Random.value < 0.7f;
            // }


			AimMove = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
			base.RunIfMove = Random.value < 0.7f;
        }


	void DashMaybe () {
		if (Random.value < 0.15f) {
//			base.Dash();
		}
	}


	void JumpMaybe () {
		if (Random.value < 0.1f) {
			base.Jump();
		}
	}


	public void Hurt (float damage, Transform tf) {
		transform.localScale = Vector3.one * 2f;

		Vector3 pos = (transform.position - tf.position).normalized * 0.2f;
		transform.Translate(pos.x + Random.Range(-0.1f, 0.1f), 0f, pos.z + Random.Range(-0.1f, 0.1f));

        HP -= damage;
		if (HP <= 0) {
			Die(tf);
		}

		// -xx
		GameObject o = new GameObject("-x");
		o.transform.position = transform.position + Vector3.up * Mathf.Lerp(4f, 2f, HP / MaxHP);
		o.transform.rotation = Camera.main.transform.rotation;
		TextMesh tm = o.AddComponent<TextMesh>();
		tm.text = damage.ToString("00");
		tm.characterSize = 0.3f;
		tm.color = new Color(0.9f, 0.3f, 0.3f, 1f);
		Destroy(o, 1f);
		int len = Random.Range(6, 14);
		for (int i = 0; i < len; i++) {
			Blood(tf, BloodCube);
			if (i < len * 0.3f) {
				Blood(tf, RedBloodCube);
			}
		}

		LastHurtTime = Time.time;

		DemoStage.PlaySound((int)Random.Range(2f, 6.99f));

	}

        public void BossHurt(float damage, Transform tf)
        {
            transform.localScale = Vector3.one * 2f;

            Vector3 pos = (transform.position - tf.position).normalized * 0.2f;
            transform.Translate(pos.x + Random.Range(-0.1f, 0.1f), 0f, pos.z + Random.Range(-0.1f, 0.1f));

            if (DemoStage.Main.bossDamageImmune)
            {
                damage = 0;
                // DemoStage.Main.life--;
            }

            HP -= damage;
            if (HP <= 0)
            {
                Die(tf);
            }

            // -xx
            GameObject o = new GameObject("-x");
            o.transform.position = transform.position + Vector3.up * Mathf.Lerp(4f, 2f, HP / MaxHP);
            o.transform.rotation = Camera.main.transform.rotation;
            TextMesh tm = o.AddComponent<TextMesh>();
            tm.text = damage.ToString("00");
            tm.characterSize = 0.3f;
            tm.color = new Color(0.9f, 0.3f, 0.3f, 1f);
            Destroy(o, 1f);

            if (!DemoStage.Main.bossDamageImmune)
            {
                int len = Random.Range(6, 14);
                for (int i = 0; i < len; i++)
                {
                    Blood(tf, BloodCube);
                    if (i < len * 0.3f)
                    {
                        Blood(tf, RedBloodCube);
                    }
                }
            }

            LastHurtTime = Time.time;

            //SFX
            if (DemoStage.Main.bossDamageImmune)
            {
                DemoStage.PlaySound(8);
            }
            else
            {
                DemoStage.PlaySound((int)Random.Range(2f, 6.99f));
            }


        }


        private void Blood (Transform tf, Transform blood) {
		GameObject bo = Instantiate<GameObject>(blood.gameObject);
		bo.transform.position = transform.position + Vector3.up * 1.5f;
		bo.transform.localScale *= Random.Range(0.5f, 1.5f);
		Rigidbody rig = bo.GetComponent<Rigidbody>();
		rig.AddExplosionForce(400f, tf.position + Vector3.down * 0.5f, 3f);
		rig.angularVelocity = Vector3.one;
		Destroy(bo, 10f);
		Collider c = bo.GetComponent<Collider>();
		if(c){
			Destroy(c, 5f);
		}
	}



	public void Die (Transform tf) {

		if (!Alive) {return;}

		Alive = false;
		Destroy(gameObject);

		if (DemoStage.Playing) {

			GameObject o = Instantiate<GameObject>(RagDoll.gameObject);
			o.transform.position = transform.position;
			o.transform.rotation = transform.rotation;
			o.transform.Rotate(new Vector3(Random.Range(-20f, 20f), Random.Range(0f, 40f), Random.Range(-20f, 20f)));

			Rigidbody rig = o.GetComponent<Rigidbody>();
			if (rig) {
				rig.AddExplosionForce(10000f, tf.position, 100f);
			}

			Collider[] cs = o.GetComponentsInChildren<Collider>(true);

			for (int i = 0; i < cs.Length; i++) {
				Destroy(cs[i], 5f);
			}

			Destroy(o, 10f);

			int len = Random.Range(28, 40);
			for (int i = 0; i < len; i++) {
				Blood(tf, BloodCube);
				if (i < len * 0.6f) {
					Blood(tf, RedBloodCube);
				}
			}
			DemoStage.CurrentEnemyNum--;
//			DemoStage.Main.FreshBar2();
			
			CameraBehaviour.CameraShake();
			DemoStage.PlaySound(1);
			DemoStage.Main.Invoke("PlayDieoutSound", 0.6f);
            if(gameObject.name.Contains("Tiger"))
            {
                DemoStage.Main.AddWeaponPower2();
                DemoStage.AddKillNum(2);
            }
            else
            {
                DemoStage.AddKillNum(1);
            }

            if (DemoStage.Main.bossPlaying)
			{
				DemoStage.numOfBoss--;
			}
			else
            {
                if (DemoStage.Main.IsCorrect(sign))
                {
                    DemoStage.Main.UpdateMsg("Success \n 공격력이 증가하였습니다!");
                    DemoStage.numOfQuiz--;
                    DemoStage.Main.NextStage();
                }
                else
                {
                    DemoStage.Main.UpdateMsg("Wrong! \n You're Damaged!!");
                    DemoStage.Main.Damage();
                }
            }
                                
            Destroy(EnemySign.gameObject);                
        }
	}

}
}