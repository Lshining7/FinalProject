using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{   //UI的数值，随便改
    public static int score = 0;
    public static int lives = 10;
    //以下都是代码需求变量
    private Quaternion initalRotation;
    [SerializeField]
    private float playerSpeed = 0.2f;
    [SerializeField]
    private GameObject projectilePrefab;
    public TMPro.TextMeshProUGUI scoreUI;
    public TMPro.TextMeshProUGUI livesUI;
    public GameObject explosionPrefab;
    private Vector2 tilt;
    public enum State{Playing,Explosion,Invincible}
    private State playerState;
    // Start is called before the first frame update
    void Start()
    {
        initalRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerState != State.Explosion)
        {
            float amtToMoveX = Input.GetAxis("Horizontal") * playerSpeed * Time.deltaTime;
            float amtToMOveY = Input.GetAxis("Vertical") * playerSpeed * Time.deltaTime;
            transform.position += Vector3.right * amtToMoveX + Vector3.up * amtToMOveY;
            //JUMP 这里我跳跃是用内部坐标系，并没有使用镜头的视角坐标，所以如果地图大而且做镜头跟随的话应该是要修正的
            if (transform.position.x < -19.15f)
            {
                transform.position = new Vector3(19.15f, transform.position.y,
                    transform.position.z);
            }
            else if (transform.position.x > 19.15f)
            {
                transform.position = new Vector3(-19.15f, transform.position.y,
                    transform.position.z);
            }
            //这里这串我忘了他是干嘛的
            transform.rotation = Quaternion.Slerp(initalRotation,
                    Quaternion.Euler(tilt.y * Input.GetAxis("Vertical"), -tilt.x * Input.GetAxis("Horizontal"), 0), 1);
            //这里是限制上下移动的区间。和上面的限制一样，也是内部的坐标系。。
            if (transform.position.y <-1.0f)
            {
                transform.position = new Vector3(transform.position.x,0.0f,transform.position.z);
            }else if (transform.position.y > 20f)
            {
                transform.position = new Vector3(transform.position.x,20.0f,transform.position.z);
            }
        }
        //Fire Laser
        if (Input.GetKeyDown("space"))
        {
            Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        }
        //UI
        scoreUI.text = "Score: " + score;
        livesUI.text = "Lives: " + lives;
        //UI这块本来我是到达一定分数或者死亡时切换场景的，但这个版本作为基础代码，我就删掉了，之后按具体需求去执行。
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Enemy collideWith = other.GetComponent<Enemy>();
        if (collideWith != null && playerState!= State.Invincible && playerState == State.Playing)
        { 
            collideWith.SetSpeedAndPosition();
            lives -= 1;
            StartCoroutine(destroy());
        }
    }
    
    private IEnumerator destroy()
    {
        playerState = State.Explosion;
        Instantiate(explosionPrefab,transform.position,transform.rotation);
        GetComponent<Renderer>().enabled = false;
        yield return new WaitForSeconds(1);//收到攻击爆炸，黑一秒
        playerState = State.Invincible;//闪烁模式无敌
        GetComponent<Renderer>().enabled = true;
        transform.position = new Vector3(0.0f, -2f, 0f);//复活点
        StartCoroutine(Blink());//闪烁无敌时间三秒
        yield return new WaitForSeconds(3);
        GetComponent<Renderer>().enabled = true;//转为实体
        playerState = State.Playing;
       
    }

    private IEnumerator Blink()
    {
        while (playerState==State.Invincible)
        {
            GetComponent<Renderer>().enabled = !GetComponent<Renderer>().enabled;
            yield return new WaitForSeconds(0.3f);//疯狂在可见和不可见两者切换
        }
    }
}
