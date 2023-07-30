using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class Test1_PlayableTest : MonoBehaviour
{
    public Animator animator;
    public AnimationClip clip1;
    public AnimationClip clip2;
    [Range(0,1)]
    public float weight;
    public double speed;
    public double time;

    private PlayableGraph m_graph;
    private AnimationMixerPlayable m_mixer;

    private void Start()
    {
        m_graph = PlayableGraph.Create();

        var anim1 = AnimationClipPlayable.Create(m_graph, clip1);
        var anim2 = AnimationClipPlayable.Create(m_graph, clip2);

        m_mixer = AnimationMixerPlayable.Create(m_graph);
        m_mixer.AddInput(anim1, 0, 1f);
        m_mixer.AddInput(anim2, 0, 0f);
        m_mixer.SetPropagateSetTime(true);

        var output = AnimationPlayableOutput.Create(m_graph, "Anim", animator);
        output.SetSourcePlayable(m_mixer);

        m_graph.Play();
    }

    private void Update()
    {
        //m_mixer.SetSpeed(speed);
        //m_mixer.SetTime(time);

        m_mixer.SetInputWeight(0, 1 - weight);
        m_mixer.SetInputWeight(1, weight);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (m_mixer.GetSpeed() == 0)
            {
                m_mixer.SetSpeed(1);
            }
            else
            {
                m_mixer.SetSpeed(0);
            }
        }
    }

    private void OnDisable()
    {
        m_graph.Destroy();
    }
}
