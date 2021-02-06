using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mino : MonoBehaviour
{
    public float previousTime;
    // minoの落ちる時間
    public float fallTime = 1f;

    // ステージの大きさ
    private static int width = 10;
    private static int height = 20;

    // mino回転
    public Vector3 rotationPoint;

    // グリッドを定義
    private static Transform[,] grid = new Transform[width, height];

    void Update()
    {
        MinoMovememt();
    }

    private void MinoMovememt()
    {
        // 左矢印キーで左に動く
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1, 0, 0);
            // 今回追加
            if (!ValidMovement())
            {
                transform.position -= new Vector3(-1, 0, 0);
            }

        }
        // 右矢印キーで右に動く
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1, 0, 0);
            // ミノがはみ出したら逆の移動量を足して、移動を打ち消す
            if (!ValidMovement())
            {
                transform.position -= new Vector3(1, 0, 0);
            }
        }
        // 自動で下に移動させつつ、下矢印キーでも移動する
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Time.time - previousTime >= fallTime)
        {
            transform.position += new Vector3(0, -1, 0);
            // ミノがはみ出したら逆の移動量を足して、移動を打ち消す
            if (!ValidMovement())
            {
                transform.position -= new Vector3(0, -1, 0);
                //ミノが下に移動できなくなったら、ミノをグリッドに追加する
                AddToGrid();
                //ライン（すべてミノで埋まった行）があるかチェックする
                CheckLines();
                //ミノが動かせなくなったら新しいミノをスポーンさせる
                this.enabled = false;
                FindObjectOfType<SpawnMino>().NewMino();
            }

            previousTime = Time.time;
        }
        // 上矢印キーで右に90度回転
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // ブロックの回転
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90); //マイナスで多分右回転
            // ミノがはみ出したら逆の回転量を足して、回転を打ち消す（※自分で追加）
            if (!ValidMovement())
            {
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
            }
        }
    }

    //ライン（すべてミノで埋まった行）があるかチェックする
    public void CheckLines()
    {
        for(int i = height -1; i >= 0; i--) //i:縦、j:横
        {
            if (HasLine(i))//列がそろっているか確認
            {
                DeleteLine(i);
                RowDown(i);
            }
        }
    }

    //列がそろっているか確認
    bool HasLine(int i)
    {
        for(int j = 0; j < width; j++)
        {
            if (grid[j, i] == null)//空のグリッドがあればfalseを返す
            {
                return false;
            }
        }
        FindObjectOfType<GameManagement>().AddScore(); //列が揃ったらスコア+100
        return true; 
    }

    //ラインを消す
    void DeleteLine(int i)
    {
        for(int j = 0; j < width; j++)
        {
            Destroy(grid[j, i].gameObject);
            grid[j, i] = null;
        }
    }

    //空の列があったら列を下げる
    public void RowDown(int i)
    {
        for (int y = i; y < height; y++)
        {
            for (int j = 0; j < width; j++)
            {
                if (grid[j, y] != null)
                {
                    grid[j, y - 1] = grid[j, y];
                    grid[j, y] = null;
                    grid[j, y - 1].transform.position -= new Vector3(0, 1, 0);
                }
            }
        }
    }


    //ミノのある領域をグリッドに追加する（ミノが下端に達したら）
    void AddToGrid()
    {
        foreach (Transform children in transform)
        {
            int roundX = Mathf.RoundToInt(children.transform.position.x);
            int roundY = Mathf.RoundToInt(children.transform.position.y);

            grid[roundX, roundY] = children;
        }
    }

    // minoの移動範囲の制御
    bool ValidMovement()
    {

        foreach (Transform children in transform)
        {
            int roundX = Mathf.RoundToInt(children.transform.position.x);
            int roundY = Mathf.RoundToInt(children.transform.position.y);

            // minoがステージよりはみ出さないように制御
            if (roundX < 0 || roundX >= width || roundY < 0 || roundY >= height)
            {
                return false;
            }
            // 他のminoがgridにすでに入っていたらminoを動かせないようにする
            if (grid[roundX, roundY] != null)
            {
                return false;
            }

        }
        return true;
    }
}