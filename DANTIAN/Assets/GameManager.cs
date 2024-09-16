using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private Node _nodePrefab;
    [SerializeField] private Block _blockPrefab;
    [SerializeField] private SpriteRenderer _boardPrefab;
    [SerializeField] private List<BlockType> _types;
    [SerializeField] private float _travelTime = 0.2f;
    [SerializeField] private int _winCondition = 2048;
    [SerializeField] private GameObject _winScreen, _loseScreen;
    [SerializeField] private Text _highscoreText;
    [SerializeField] private Text _scoreText;

    private int escKeyCount = 0;
    private int _highscore = 0;
    public GameObject PauseMenu;
    public GameObject WinImg;
    public GameObject LoseImg;
    private List<Node> _nodes;
    private List<Block> _blocks;
    private GameState _state;
    private int _round;
    private int _score;

    private BlockType GetBlockTypeByValue(int value) => _types.First(t => t.Value == value);

    void Start()
    {
        _score = 0;
        _scoreText.text = _score.ToString();
        ChangeState(GameState.GenerateLevel);
        _highscore = PlayerPrefs.GetInt("Highscore", 0);

        // Update highscore text
        _highscoreText.text = "High Score:\n" + _highscore.ToString();
    }

    private void ChangeState(GameState newState)
    {
        _state = newState;

        switch (newState)
        {
            case GameState.GenerateLevel:
                GenerateGrid();
                break;
            case GameState.SpawningBlocks:
                SpawnBlocks(_round++ == 0 ? 2 : 1);
                break;
            case GameState.WaitingInput:
                break;
            case GameState.Moving:
                break;
            case GameState.Win:
                WinImg.SetActive(true);
                _winScreen.SetActive(true);
                break;
            case GameState.Lose:
                LoseImg.SetActive(true);
                _loseScreen.SetActive(true);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState),newState,null);
        }
    }

    void Update()
    {
        if (_state != GameState.WaitingInput) return;

        if (Input.GetKeyDown(KeyCode.LeftArrow)) Shift(Vector2.left);
        if (Input.GetKeyDown(KeyCode.RightArrow)) Shift(Vector2.right);
        if (Input.GetKeyDown(KeyCode.UpArrow)) Shift(Vector2.up);
        if (Input.GetKeyDown(KeyCode.DownArrow)) Shift(Vector2.down);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (escKeyCount == 0)
            {
                Time.timeScale = 0;
                escKeyCount++;
                PauseMenu.SetActive(true);
                _highscoreText.gameObject.SetActive(false);
                _scoreText.gameObject.SetActive(false);
            }
            else
            {
                Time.timeScale = 1;
                PauseMenu.SetActive(false);
                _highscoreText.gameObject.SetActive(true);
                _scoreText.gameObject.SetActive(true);
                escKeyCount--;
            }
        }
    }

    public void ResetHighscore()
    {
        _highscoreText.gameObject.SetActive(true);
        PauseMenu.SetActive(false);
        escKeyCount--;
        _highscore = 0;
        _highscoreText.text = "High Score:\n" + _highscore.ToString();
        PlayerPrefs.DeleteKey("Highscore");
        restartGame();
    }

    public void restartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void returnMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 4);
    }

    void GenerateGrid()
    {
        _round = 0;
        _nodes = new List<Node>();
        _blocks = new List<Block>();
        for(int x = 0; x < _width; x++)
        {
            for(int y = 0; y < _height; y++)
            {
                var node = Instantiate(_nodePrefab, new Vector2(x, y), Quaternion.identity);
                _nodes.Add(node);
            }
        }

        var center = new Vector2((float) _width/2 - 0.5f,(float) _height/2 - 0.5f);

        var board = Instantiate(_boardPrefab, center, Quaternion.identity);
        board.size = new Vector2(_width,_height);

        Camera.main.transform.position = new Vector3(center.x,center.y, -10);

        ChangeState(GameState.SpawningBlocks);
    }

    void SpawnBlocks(int amount)
    {
        var freeNodes = _nodes.Where(n => n.OccupiedBlock == null).OrderBy(b => Random.value).ToList();
        foreach (var node in freeNodes.Take(amount))
        {
            if (_blocks.Any(b => b.Value == _winCondition))
            {
                ChangeState(GameState.Win);
                return;
            }
            SpawnBlock(node, Random.value > 0.9f ? 4 : 2, node.Pos);
        }

        if (freeNodes.Count() == 1)
        {
            ChangeState(GameState.Lose);
            return;
        }

        ChangeState(GameState.WaitingInput);
    }

    void SpawnBlock(Node node, int value, Vector2 position)
    {
        var block = Instantiate(_blockPrefab, position, Quaternion.identity);
        block.Init(GetBlockTypeByValue(value));
        block.SetBlock(node);
        _blocks.Add(block);
    }

    void Shift(Vector2 dir)
    {
        ChangeState(GameState.Moving);
        var orderedBlocks = _blocks.OrderBy(b => b.Pos.x).ThenBy(b => b.Pos.y).ToList();
        if (dir == Vector2.right || dir == Vector2.up) orderedBlocks.Reverse();

        foreach(var block in orderedBlocks){
            var next = block.Node;
            do
            {
                block.SetBlock(next);
                var possibleNode = GetNodeAtPosition(next.Pos+dir);
                if (possibleNode != null)
                {
                    //We know that a node is present
                    //If it's possible to merge, set merge
                    if (possibleNode.OccupiedBlock != null && possibleNode.OccupiedBlock.CanMerge(block.Value))
                    {
                        block.MergeBlock(possibleNode.OccupiedBlock);
                    }
                    //Otherwise, can we move to this spot?
                    else if (possibleNode.OccupiedBlock == null) next = possibleNode;
                }
            } while (next!=block.Node);
        }

        var sequence = DOTween.Sequence();

        foreach(var block in orderedBlocks)
        {
            var movePoint = block.MergingBlock!=null ? block.MergingBlock.Node.Pos : block.Node.Pos;
            sequence.Insert(0,block.transform.DOMove(block.Node.Pos, _travelTime));
        }

        sequence.OnComplete(() =>
        {
            foreach(var block in orderedBlocks.Where(b=>b.MergingBlock != null))
            {
                MergeBlocks(block.MergingBlock,block);
            }
            ChangeState(GameState.SpawningBlocks);
        });
    }

    void MergeBlocks(Block baseBlock, Block mergingBlock)
    {
        int mergedValue = baseBlock.Value * 2;
        _score = _score + mergedValue;
        _scoreText.text = _score.ToString();

        if (_score > PlayerPrefs.GetInt("Highscore", 0))
        {
            _highscore = _score;
            _highscoreText.text = "High Score:\n" + _highscore.ToString();

            // Store highscore in PlayerPrefs
            PlayerPrefs.SetInt("Highscore", _highscore);
        }

        if (mergedValue == _winCondition)
        {
            ChangeState(GameState.Win);
            RemoveBlock(baseBlock);
            RemoveBlock(mergingBlock);
            SpawnBlock(baseBlock.Node, mergedValue, baseBlock.Node.Pos);
            return; // Stop further block spawning
        }
        RemoveBlock(mergingBlock);
        // Update highscore
        
        RemoveBlock(baseBlock);
        SpawnBlock(baseBlock.Node, mergedValue, baseBlock.Node.Pos);
    }

    void RemoveBlock(Block block)
    {
        _blocks.Remove(block);
        Destroy(block.gameObject);
    }

    Node GetNodeAtPosition(Vector2 pos)
    {
        return _nodes.FirstOrDefault(n => n.Pos == pos);
    }
}

[Serializable]
public struct BlockType
{
    public int Value;
    public Color Color;
}

public enum GameState
{
    GenerateLevel,
    SpawningBlocks,
    WaitingInput,
    Moving,
    Win,
    Lose
}