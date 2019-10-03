using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFire : MonoBehaviour {

    public GameObject Player;
    public GameObject AttackParticle;
    public GameObject ParticlesContainer;

    private void Awake()
    {
        this.AttackParticle.SetActive(false);
    }

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            GameObject particle = PlayParticle(this.AttackParticle, this.transform.position + new Vector3(0.3f, 0.5f, 0), 3);
            Vector3 playerposition = Player.transform.forward;
            particle.transform.rotation = Quaternion.LookRotation(playerposition);
            Destroy(particle, 1);
        } 


   }

    public GameObject PlayParticle(GameObject particle, Vector3 position, float time)
    {
        GameObject instance = Utils.CreateInstance(particle, this.ParticlesContainer, true);
        instance.transform.position = position;
        Destroy(instance, 3);
        return instance;
    }
}
