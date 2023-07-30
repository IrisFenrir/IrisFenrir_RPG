using IrisFenrir.AnimationSystem;
using UnityEngine;
using UnityEngine.Playables;

public class Test3_MixerStateTest : MonoBehaviour
{
    public Animator animator;
    public AnimationClip clip1, clip2;
    [Range(0f,1f)]
    public float weight;

    private PlayableGraph m_graph;
    private MixerState m_mixer;


    private void Start()
    {
        m_graph = PlayableGraph.Create();

        OutputState output = new OutputState(m_graph, animator);
        m_mixer = new MixerState(m_graph);
        m_mixer.AddInput(new AnimClipState(m_graph, clip1), 1f);
        m_mixer.AddInput(new AnimClipState(m_graph, clip2), 0f);
        output.AddInput(m_mixer, 1f);

        m_graph.Play();
    }

    private void Update()
    {
        m_mixer.SetWeight(0, 1 - weight);
        m_mixer.SetWeight(1, weight);
    }

    private void OnDisable()
    {
        m_graph.Destroy();
    }
}
