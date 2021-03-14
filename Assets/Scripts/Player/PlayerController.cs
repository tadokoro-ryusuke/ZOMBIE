using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody body;
    // [SerializeField] private Vector3 localGravity;
    [SerializeField] public float moveSpeed = 10f;

    private bool onGround = true;
    private bool jumped = false;
    private float h = 0;
    private float v = 0;
    private Ray ray; // 飛ばすレイ
    private RaycastHit hit; // レイが何かに当たった時の情報

    private Vector3 rayPosition; // レイを発射する位置

    public void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // カメラの方向から、X-Z平面の単位ベクトルを取得
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

        // 方向キーの入力値とカメラの向きから、移動方向を決定
        Vector3 moveForward = cameraForward * v + Camera.main.transform.right * h;

        // 移動方向にスピードを掛ける。ジャンプや落下がある場合は、別途Y軸方向の速度ベクトルを足す。
        body.velocity = moveForward * moveSpeed + new Vector3(0, body.velocity.y, 0);

        // キャラクターの向きを進行方向に
        if (moveForward != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveForward);
        }
        if (jumped)
        {
            body.velocity = new Vector3(body.velocity.x, 5, body.velocity.z);
            jumped = false;
        }

        rayPosition = transform.position + new Vector3(0, 0.5f, 0); // レイを発射する位置の調整
        ray = new Ray(rayPosition, transform.up * -1); // レイを下に飛ばす
        Debug.DrawRay(ray.origin, ray.direction * 0.6f, Color.red); // レイを赤色で表示させる

        if (Physics.Raycast(ray, out hit, 0.6f)) // レイが当たった時の処理
        {
            onGround = true;
        } else {
            onGround = false;
        }
    }

    public void Update()
    {
        if (onGround && Input.GetButtonDown("Jump"))
        {
            jumped = true;
            onGround = false;
        }
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
    }
}
