using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] int cost = 75;

    [SerializeField][Range(0.05f, 2f)] float buildDelay = .2f;

    [SerializeField] MeshRenderer baseMeshRenderer;
    [SerializeField] MeshRenderer topMesgRenderer;
    [SerializeField] ParticleSystem particleSystem;


    private Bank bank;

    void Start() {
        baseMeshRenderer.enabled = false;
        topMesgRenderer.enabled = false;
        particleSystem.Stop();

        StartCoroutine(Build());
    }

    private IEnumerator Build()
    {
        baseMeshRenderer.enabled = true;

        yield return new WaitForSeconds(buildDelay);

        topMesgRenderer.enabled = true;

        yield return new WaitForSeconds(buildDelay);

        particleSystem.Play();
    }

    public bool CreateTower(Tower towerPrefab, Vector3 position)
    {
        if(bank == null) {
            bank = FindObjectOfType<Bank>();

            if(bank == null) {
                return false;
            }
        }
        
        if(bank.CurrentBalance < cost) return false;

        Instantiate(towerPrefab, position, Quaternion.identity);
        bank.Withdraw(cost);

        return true;
    }
}
