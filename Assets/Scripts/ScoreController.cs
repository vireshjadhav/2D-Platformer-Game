<<<<<<< .merge_file_GTI9zS
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
=======
using System.Collections;
using System.Collections.Generic;
>>>>>>> .merge_file_LfYazN
using UnityEngine;

public class ScoreController : MonoBehaviour
{
<<<<<<< .merge_file_GTI9zS

    [SerializeField] private TextMeshProUGUI scoreText;

    private int score = 0;


    // Start is called before the first frame update
    void Start()
    {
        RefreshUI();
=======
    // Start is called before the first frame update
    void Start()
    {
        
>>>>>>> .merge_file_LfYazN
    }

    // Update is called once per frame
    void Update()
    {
        
    }
<<<<<<< .merge_file_GTI9zS

    private void Awake()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
    }

    public void IncreseScore(int increment)
    {
        score += increment;
        RefreshUI();
    }

    private void RefreshUI()
    {
        scoreText.text = "Score : " + score;
    }
=======
>>>>>>> .merge_file_LfYazN
}
