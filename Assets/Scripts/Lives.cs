using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lives : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> error_images;
    
    int lives_ = 0; //зачем жизни декрементировать, если считаем ошибки. можно же сравнивать кол-во ошибок с кол-вом жизней

    int error_number_ = 0;

    [SerializeField]
    private GameObject gameOverPopUp;

    // Start is called before the first frame update
    void Start()
    {
        lives_ = error_images.Count;
        error_number_ = 0;
    }

    private void WrongNumber()
    {
        if (error_number_ < error_images.Count) // i don't like it
        {
            error_images[error_number_].SetActive(true);
            error_number_++;
            lives_--;
        }
        CheckForGameOver();
    }

    private void CheckForGameOver()
    {
        if (lives_ <= 0)
        {
            gameOverPopUp.SetActive(true);
        }
    }

    private void OnEnable() 
    {
        GameEvents.OnWrongNumber += WrongNumber;
    }

    private void OnDisable() 
    {
        GameEvents.OnWrongNumber -= WrongNumber;
    }

}
