using UnityEngine;
using TMPro;
using NCalc;

public class calculatorApp : MonoBehaviour
{
    bool oldOpen;
    [Header("calculate")]
    public Expression problem;

    [Header("writing a problem")]
    public TMP_InputField input;

    app me;
    private void Awake()
    {
        me = GetComponent<app>();
    }
    private void Update()
    {
        if (me.isOpen == true)
        {
            input.Select();
            if (oldOpen == false)
            {
                me.sayWhenUse(true);
            }
        }

        input.gameObject.SetActive(me.isOpen);
        oldOpen = me.isOpen;
    }
    public void Solve()
    {
        problem = new Expression(input.text);
        if (problem.HasErrors() == false)
        {
            input.text = problem.Evaluate().ToString();

            if (problem.Evaluate() is double d)
            {
                if (double.IsNaN(d) || double.IsInfinity(d))
                {
                    me.sayWhenUse(false);
                }
            }
        }
    }
}
