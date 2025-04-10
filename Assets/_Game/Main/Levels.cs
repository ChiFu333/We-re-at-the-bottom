using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;

public class Levels : MonoBehaviour
{
    [System.Serializable]
    public class Level
    {
        public string name;
        public List<LocString> textLines;
        public List<GameObject> activeObjects;
        public UnityEvent unityAction;
        public List<ActionAt> subActions;
    }

    [System.Serializable]
    public class ActionAt
    {
        public int number;
        public UnityEvent actionAt;
    }
    public List<Level> levels;
    private int currentLevelIndex = 0;
    public void SetLevel(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= levels.Count)
        {
            Debug.LogError("Invalid level index!");
            return;
        }

        // Выключаем все объекты текущего уровня
        if (currentLevelIndex >= 0 && currentLevelIndex < levels.Count)
        {
            foreach (var obj in levels[currentLevelIndex].activeObjects)
            {
                obj.SetActive(false);
            }
            
        }
        currentLevelIndex = levelIndex;

        // Включаем объекты нового уровня
        foreach (var obj in levels[currentLevelIndex].activeObjects)
        {
            obj.SetActive(true);
        }
        levels[currentLevelIndex].unityAction.Invoke();
        _ = Speak();
    }

    public async UniTaskVoid Speak()
    {
        int t = currentLevelIndex;
        for (int i = 0; i < levels[t].textLines.Count; i++)
        {
            if (t != currentLevelIndex) return;
            await GameMain.inst.uiHandler.ThrowText(levels[t].textLines[i]);
            await UniTask.Delay(2000);
            DoSubAction(i);
        }
        
    }

    private void DoSubAction(int id)
    {
        foreach (var act in levels[currentLevelIndex].subActions)
        {
            if (act.number == id)
            {
                act.actionAt.Invoke();
                return;
            }
        }
    }
}

