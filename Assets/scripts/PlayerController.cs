using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
    public float speed = 8; // velocidade do jogador
    public float gravity = -9.8f; // valor da gravidade
    public LayerMask groundMask;
    CharacterController character;
    Vector3 velocity;
    bool isGrounded;
    private Animator anim;

    public GameObject playerbody;
    public GameObject baloes;

    public GameObject bullet;

    public float fireDelta = 0.5F;
    private float nextFire = 0.5F;
    private float myTime = 0.0F;

    private AudioSource audioSource;
    public AudioClip audio_attack;
    public AudioClip audio_fire;


    void Start()
    {
        character = gameObject.GetComponent<CharacterController>();
        anim = playerbody.GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

    }

    void Update()
    {
        myTime = myTime + Time.deltaTime;
        if (Input.GetButton("Fire1") && myTime > nextFire)
        {
            audioSource.PlayOneShot(audio_attack);

            nextFire = myTime + fireDelta;
            Vector3 offset = new Vector3(0, 15, 0);
            GameObject instancia = Instantiate(bullet, transform.position + (transform.forward * 2) + offset, transform.rotation) as GameObject;
            instancia.GetComponent<Rigidbody>().velocity = 200.0f * transform.forward;
            Destroy(instancia, 5.0f); // Destroi o tiro depois de 5 segundos
            nextFire = nextFire - myTime;
            myTime = 0.0F;
        }

        // Verifica se encostando no chão (o centro do objeto deve ser na base)
        isGrounded = Physics.CheckSphere(transform.position, 0.2f, groundMask);

        // Se no chão e descendo, resetar velocidade
        if (isGrounded && velocity.y < 0.0f)
        {
            velocity.y = -1.0f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        anim.SetFloat("virar", x);
        anim.SetFloat("correr", z);


        // Rotaciona personagem
        //transform.Rotate(0, x * speed * 10 * Time.deltaTime, 0);

        // Move personagem
        Vector3 move = transform.forward * z;
        character.Move(move * Time.deltaTime * speed);

        Vector3 move2 = transform.right * x;
        character.Move(move2 * Time.deltaTime * speed);

        // Aplica gravidade no personagem
        velocity.y += gravity * Time.deltaTime;
        character.Move(velocity * Time.deltaTime);

        //detecta se o player estorou todos os baloes
        if (baloes.transform.childCount == 0)
        {
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene(0);
        }

    }
}
