using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.IO;

public class SaveSystem : MonoBehaviour
{
   public static void SaveData(GridGenerator gridRef)
    {
       /* BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/game.fun";
        FileStream stream = new FileStream(path, FileMode.Create);

        //formatter.Serialize(stream, gridRef.BallPrefabs);
       // formatter.Serialize(stream, gridRef.BackgroundPrefab);
        formatter.Serialize(stream, gridRef.FillTime);
        formatter.Serialize(stream, gridRef.QueueBall);
        formatter.Serialize(stream, gridRef.X);
        formatter.Serialize(stream, gridRef.Y);
        //formatter.Serialize(stream, gridRef.Tiles);
        //formatter.Serialize(stream, gridRef.BallPrefabDict);

        formatter.Serialize(stream, gridRef.ListBalls);
        formatter.Serialize(stream, gridRef.BallCounter);
        formatter.Serialize(stream, gridRef.MaxBall);
        formatter.Serialize(stream, gridRef.BallQueue);
        formatter.Serialize(stream, gridRef.GameOver);

        stream.Close();*/
    }

    public static GridGenerator LoadData()
    {
        /*string path = Application.persistentDataPath + "/game.fun";
        if (File.Exists(path))
        {

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            GridGenerator gridRef = new GridGenerator();
            //gridRef.BallPrefabs = formatter.Deserialize(stream) as GridGenerator.BallPrefab[];
            //gridRef.BackgroundPrefab = formatter.Deserialize(stream) as Tile;
            gridRef.FillTime = (float)formatter.Deserialize(stream);
            gridRef.QueueBall= (int)formatter.Deserialize(stream);
            gridRef.X = (int)formatter.Deserialize(stream);
            gridRef.Y = (int)formatter.Deserialize(stream);
            //gridRef.Tiles = formatter.Deserialize(stream) as Dictionary<Vector2, Tile>;
            //gridRef.BallPrefabDict = formatter.Deserialize(stream) as
                //Dictionary<GridGenerator.BallType, GameObject>;

            gridRef.ListBalls = formatter.Deserialize(stream) as Ball[,];
            gridRef.BallCounter = (int)formatter.Deserialize(stream);
            gridRef.MaxBall = (int)formatter.Deserialize(stream);
            gridRef.BallQueue = formatter.Deserialize(stream) as Queue<Ball>;
            gridRef.GameOver = (bool)formatter.Deserialize(stream);

            stream.Close();
            return gridRef;
        }
        else
        {
            Debug.LogError("Can't find save file in " + path);
            return null;
        }*/
        return null;
    }
}
