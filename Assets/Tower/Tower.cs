using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] int cost = 75;

    private Bank bank;

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
