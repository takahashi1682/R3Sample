using UnityEngine;
using R3;

public class Test : MonoBehaviour
{
    private void Start()
    {
        Observable.Range(1, 9) // 1,2,3,4,5,6,7,8,9
            .Select(x => x * x) // 1,4,9,16,25,36,49,64,81　
            .Where(x => x % 2 == 1) // 1,9,25,49,81
            .Where(x => x % 3 == 0) // 9,81
            .Select(x => x + "個") // 「x個」と変換して流す（xが1なら「1個」、xが2なら「2個」）
            .Subscribe(number => Debug.Log(number))
            .AddTo(this);
    }
}