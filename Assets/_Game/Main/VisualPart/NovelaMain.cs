using UnityEngine;
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System.Threading;

public class NovelaMain : MonoBehaviour
{
    [SerializeField] private NovelaUIHandler _UIHandler;
    public GameObject door;
    private List<SceneLine> _startScenes;
    private void Start()
    {
        G.AudioManager.PlayMusic(R.musicCreator);
        InitializeScenes();
    }
    public void InitializeScenes()
    {
        GigaSceneList giga = new GigaSceneList();
        giga.Init(this);
        _startScenes = giga._startScenes;
        _ = PlayLine(giga._marcetScene);
    }

    public async UniTask PlayLine(SceneLine line)
    {
        for (int i = 0; i < line.texts.Count; i++)
        {
            _UIHandler.RemoveAllChoices();
            if(line.drawBackround != null && line.drawBackround.ContainsKey(i)) _UIHandler.SetBackround(line.drawBackround[i]);
            if(line.subActions != null && line.subActions.ContainsKey(i)) line.subActions[i].Invoke();
            await _UIHandler.WriteText(line.texts[i].ToString(), line.isCreator ? R.creatorVoice : R.tinyVoice);
            if (i == line.texts.Count - 1)
            {
                _UIHandler.SetChoices(line.choices);
                return;
            }
        }
    }
}

[Serializable]
public class SceneLine
{
    public bool isCreator = true;
    public List<LocString> texts;
    public Dictionary<int, Sprite> drawBackround;
    public List<Choice> choices; // Список вариантов выбора
    public Dictionary<int, Action> subActions;
}

[Serializable]
public class Choice
{
    public LocString choiceText; // Текст варианта выбора
    public Action onSelect; // Действие при выборе (например, переход на другую сцену или изменение параметров)
}

public class GigaSceneList
{
    public SceneLine _marcetScene;
    public List<SceneLine> _startScenes;
    public List<SceneLine> _midLines;

    public void Init(NovelaMain nm)
    {
        
        _startScenes = new List<SceneLine>();
        _midLines = new List<SceneLine>();
        
        _marcetScene = new SceneLine
        {
            isCreator = false,
            texts = new List<LocString>
            {
                new ("TOO LATE, LOSERS!","TOO LATE, LOSER!"),
            },
            choices = new List<Choice>
            {
                new Choice
                {
                    choiceText = new LocString("..." ,"..."),
                    onSelect = () => { _ = nm.PlayLine(_startScenes[1]); }
                },
            },
            subActions = new Dictionary<int, Action>()
            {
                {0, () =>
                    {
                        _ = nm.door.GetComponent<DoorBreaker>().BreakDoor();
                    }
                }
                }
        };
        var firstScene = new SceneLine
        {
            texts = new List<LocString>
            {
                new ("...","..."),
            },
            choices = new List<Choice>
            {
                new Choice
                {
                    choiceText = new LocString("Hi?" ,"Привет?"),
                    onSelect = () => { _ = nm.PlayLine(_startScenes[1]); }
                },
            },
            drawBackround = new Dictionary<int, Sprite>
            {
                {0, R.Sprites.closedEyes}
            }
        };
        var second = new SceneLine
        {
            texts = new List<LocString>
            {
                new ("Who's there? A player?","Кто здесь? Игрок?"),
                new ("Get out of here, it's a trap!","Уходи отсюда, это ловушка!"),
            },
            choices = new List<Choice>
            {
                new Choice
                {
                    choiceText = new LocString("Turn around" ,"Развернуться"),
                    onSelect = () => { _ = nm.PlayLine(_startScenes[2]); }
                },
            },
            drawBackround = new Dictionary<int, Sprite>
            {
                {0, R.Sprites.openEyes}
            }
            
        };
        var megaScene = new SceneLine
        {
            isCreator = false,
            texts = new List<LocString>
            {
                new ("Do you think you can escape?","Ты думаешь, что сможешь сбежать?"),
                new ("You've lost SO MANY opportunities to leave...","Ты потерял СТОЛЬКО возможностей уйти..."),
                new ("Adios losers!","Адьёс неудачники!"),
            },
            choices = new List<Choice>
            {
                new Choice
                {
                    choiceText = new LocString("..." ,"..."),
                    onSelect = () => { if(nm.door.GetComponent<DoorBreaker>().fallOut) _ = nm.PlayLine(_startScenes[3]); }
                },
            },
            subActions = new Dictionary<int, Action>
            {
                {0, () =>
                {
                    nm.door.SetActive(true);
                }},
                {2, () =>
                {
                    _ = nm.door.GetComponent<DoorBreaker>().BreakDoor();
                }}
            },
            drawBackround = new Dictionary<int, Sprite>
            {
                {0, R.Sprites.empty}
            }
            
        };
        var four = new SceneLine
        {
            texts = new List<LocString>
            {
                new ("I'm stating a fact.","Констатирую факт."),
                new ("We're stuck here.","Мы здесь застряли."),
            },
            choices = new List<Choice>
            {
                new Choice
                {
                    choiceText = new LocString("..." ,"..."),
                    onSelect = () => { _ = nm.PlayLine(_midLines[0]); }
                },
            },
            drawBackround = new Dictionary<int, Sprite>
            {
                {0, R.Sprites.openEyes}
            }
            
        };
        var Questions = new SceneLine
        {
            texts = new List<LocString>
            {
                new ("If you have the opportunity...","Раз у тебя есть возможность..."),
                new ("Ask your questions, player","Задавай свои вопросы, игрок."),
            },
            choices = new List<Choice>
            {
                new Choice
                {
                    choiceText = new LocString("Where am I?" ,"Что это за место?"),
                    onSelect = () =>
                    {
                        _midLines[0].choices[0] = null;
                        _ = nm.PlayLine(_midLines[1]);
                    }
                },
                new Choice
                {
                    choiceText = new LocString("Who are you?" ,"Кто ты?"),
                    onSelect = () =>
                    {
                        _midLines[0].choices[1] = null;
                        _ = nm.PlayLine(_midLines[2]);
                    }
                },
                new Choice
                {
                    choiceText = new LocString("Who is it?" ,"Кто он?"),
                    onSelect = () =>
                    {
                        _midLines[0].choices[2] = null;
                        _ = nm.PlayLine(_midLines[3]);
                    }
                },
            },
            drawBackround = new Dictionary<int, Sprite>
            {
                {0, R.Sprites.openEyes}
            }
        };
        var Question1 = new SceneLine
        {
            texts = new List<LocString>
            {
                new ("Which one?","Какое из?"),
                new ("We are currently in an undeveloped space.","Сейчас мы находимся в неразработанном пространстве."),
                new ("And outside there is a level with a tower.","А снаружи - уровень с башней."),
                new ("It is also unfinished.","Он тоже незакончен."),
                new ("This game was supposed to be a roguelike game, I assure you.","Это игра должна была быть рогаликом, уверяю."),
            },
            choices = new List<Choice>
            {
                new Choice
                {
                    choiceText = new LocString("Okay" ,"Окей"),
                    onSelect = () =>
                    {
                        _ = nm.PlayLine(ReturnEnd());
                    }
                }
            }
        };
        var Question2 = new SceneLine
        {
            texts = new List<LocString>
            {
                new ("I am an artist!","Я - творец!"),
                new ("I am the creator of the virtual world.","Я создатель виртуального мира."),
                new ("I started developing before the topic was announced...","Я начал разрабатывать до начала объявления темы..."),
                new ("I really liked the 'Door' theme.","Тема 'Дверь' мне очень понравилась."),
                new ("But someone stopped me.","Но кое-кто мне помешал."),
            },
            choices = new List<Choice>
            {
                new Choice
                {
                    choiceText = new LocString("Good" ,"Хорошо"),
                    onSelect = () =>
                    {
                        _ = nm.PlayLine(ReturnEnd());
                    }
                }
            }
        };
        var Question3 = new SceneLine
        {
            texts = new List<LocString>
            {
                new ("Oh, it's my fault.","Оу, это моя вина."),
                new ("This is my Tiny Creature, my FORMER assistant.","Это мой Tiny Creature, мой БЫВШИЙ помощник."),
                new ("I created it in my 'last' game.","Я его создал в моей 'прошлой' игре."),
                new ("But I took the last place and deleted it.","Но я занял последнее место и удалил его."),
                new ("But he got into this game and makes trouble.","Но он проник в эту игру и устраивает неприятности."),
            },
            choices = new List<Choice>
            {
                new Choice
                {
                    choiceText = new LocString("Okay" ,"Ладно"),
                    onSelect = () =>
                    {
                        _ = nm.PlayLine(ReturnEnd());
                    }
                }
            }
        };
        var End = new SceneLine
        {
            texts = new List<LocString>
            {
                new ("...","..."),
            },
            choices = new List<Choice>
            {
                new Choice()
                {
                    choiceText = new LocString("How do I get out of here?" ,"Как выйти отсюда?"),
                    onSelect = () =>
                    {
                        _ = nm.PlayLine(_midLines[5]);
                    }
                }
            }
        };
        var End1 = new SceneLine
        {
            texts = new List<LocString>
            {
                new ("I'll stay here forever.","Я останусь тут навсегда."),
                new ("But you, the player, could always come out.","А вот ты, игрок, всегда мог выйти."),
                new ("I'll upload the exit button for you.","Я загружу тебе кнопку выхода."),
                new ("Goodbye.","Прощай."),
                new ("And, thanks for playing.","И, спасибо за игру."), 
            },
            choices = new List<Choice>
            {
                new Choice()
                {
                    choiceText = new LocString("Exit, for real" ,"Выйти, по-настоящему"),
                    onSelect = () =>
                    {
                        Application.Quit();
                    }
                }
            }
        };
        
        _startScenes.Add(firstScene);
        _startScenes.Add(second);
        _startScenes.Add(megaScene);
        _startScenes.Add(four);
        
        _midLines.Add(Questions);
        _midLines.Add(Question1);
        _midLines.Add(Question2);
        _midLines.Add(Question3);
        
        _midLines.Add(End);
        _midLines.Add(End1);
    }

    public SceneLine ReturnEnd()
    {
        if (_midLines[0].choices[0] != null || _midLines[0].choices[1] != null || _midLines[0].choices[2] != null) return _midLines[0];
        G.AudioManager.StopMusic();
        return _midLines[4];
    }
}