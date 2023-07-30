using IrisFenrir.AnimationSystem;
using UnityEngine;
using UnityEngine.Playables;

public class Test2_SimpleStateTest : MonoBehaviour
{
    public Animator animator;
    public AnimationClip clip;
    public bool canSetTime;
    public double time;
    public double speed;
    public bool isPaused;

    private PlayableGraph m_graph;
    private AnimClipState m_clipState;

    private void Start()
    {
        m_graph = PlayableGraph.Create();

        OutputState output = new OutputState(m_graph, animator);
        m_clipState = new AnimClipState(m_graph, clip);
        output.AddInput(m_clipState, 1f);

        m_graph.Play();
    }

    private void Update()
    {
        if (canSetTime)
            m_clipState.Time = time;

        m_clipState.Speed = speed;
        m_clipState.IsPaused = isPaused;
    }

    private void OnDisable()
    {
        m_graph.Destroy();
    }
}
