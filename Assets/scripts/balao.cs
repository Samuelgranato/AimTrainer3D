using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class balao : MonoBehaviour
{

    public Transform explosionPrefab;
    private AudioSource audioSource;
    public AudioClip audio_attack;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator pop()
    {

        audioSource.PlayOneShot(audio_attack);
        gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        yield return new WaitForSeconds(2f);
        GameObject foo = transform.parent.gameObject;
        transform.parent = null;
        Destroy(gameObject);
        Destroy(foo);

    }


    void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 pos = contact.point;
        var instancia = Instantiate(explosionPrefab, pos, rot);
        Destroy(instancia.gameObject, 0.5f);
        Destroy(collision.gameObject);
        StartCoroutine(pop());


    }

}
