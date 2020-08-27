using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualPerform : MonoBehaviour
{
    public GameObject manager;

    int rows, coloumns;
    public bool [][] visited = new bool [500][];
    public float yellowTime = 1f;
    public float iterationTime = 0.7f;

    // Start is called before the first frame update
    void Start()
    {
        rows = manager.GetComponent<MatSizIn>().rows;
        coloumns = manager.GetComponent<MatSizIn>().coloumns;

        for(int i = 0; i < rows; i++){
            visited[i] = new bool[coloumns];
        }

        StartCoroutine(slowIteration());

    }

    
    IEnumerator slowIteration(){
        for(int i = 0; i < rows; i++){
            for(int j = 0; j < coloumns; j++){
                if(!visited[i][j])
                    yield return new WaitForSeconds(iterationTime);
            
                dfs(i, j);
            }
        }
    }

    void dfs(int r, int c){
        if(visited[r][c])
            return;
        
        visited[r][c] = true;
        

        /*
            This if statment is not logically at all,
            but i was forced to handle it due to a bug in GameObject.Find function.
            please contact me if you have information about that.

        */
        string s;
        if(r % 11 != 0)
            s = string.Concat(r.ToString(), " ", c.ToString());
        else
            s = string.Concat(r.ToString(), r.ToString(), " ", c.ToString());

        GameObject currentCell = GameObject.Find(s);
        SpriteRenderer currentColor = currentCell.GetComponent<SpriteRenderer>();
        

        if(currentColor.color == Color.black){
            StartCoroutine(waitThenColorYellow(currentColor, false));
            
        } else {
            StartCoroutine(waitThenColorYellow(currentColor, true));
            return;
        }


        if(r+1 < rows)
            dfs(r+1, c);
        if(r-1 >= 0)
            dfs(r-1, c);
        if(c+1 < coloumns)
            dfs(r, c+1);
        if(c-1 >= 0)
            dfs(r, c-1);
    }

    IEnumerator waitThenColorYellow(SpriteRenderer currentSprite, bool returnRed){
        yield return new WaitForSeconds(0.0f);
        currentSprite.color = Color.yellow;
        if(returnRed){
            StartCoroutine(waitThenColorRed(currentSprite));
        } else {
            StartCoroutine(waitThenColorGreen(currentSprite));
        }
    }

    IEnumerator waitThenColorRed(SpriteRenderer currentSprite){
        yield return new WaitForSeconds(yellowTime);
        currentSprite.color = Color.red;
    }

    IEnumerator waitThenColorGreen(SpriteRenderer currentSprite){
        yield return new WaitForSeconds(yellowTime);
        currentSprite.color = Color.green;
    }
}
