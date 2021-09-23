using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(TankController))]
public class Player : MonoBehaviour
{
    //int _currentTreasure;
    [SerializeField] UIWriter _ui;

    TankController _tankController;

    private void Awake()
    {
        _tankController = GetComponent<TankController>();
    }

    private void Start()
    {
        //refreshUI();
    }

    public void Kill()
    {
        StartCoroutine(Reset());
        _tankController.enabled = false;
        gameObject.GetComponent<Collider>().enabled = false;
        gameObject.GetComponent<Rigidbody>().detectCollisions = false;
        MeshRenderer[] tankArt = gameObject.GetComponentsInChildren<MeshRenderer>();

        foreach(MeshRenderer mr in tankArt)
        {
            mr.enabled = false;
        }
    }

    /*public void IncreaseTreasure(int amount)
    {
        _currentTreasure += amount;
        refreshUI();
        Debug.Log("Player's treasure: " + _currentTreasure);
    }*/

    /*public void refreshUI()
    {
        _ui.SetTreasureUI(_currentTreasure);
    }*/

    IEnumerator Reset()
    {
        yield return new WaitForSeconds(2);
        Debug.Log("Reset Scene");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
