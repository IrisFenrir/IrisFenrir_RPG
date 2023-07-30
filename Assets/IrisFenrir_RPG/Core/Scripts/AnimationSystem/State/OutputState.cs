using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace IrisFenrir.AnimationSystem
{
    public class OutputState : IAnimationState
    {
        private AnimationPlayableOutput m_output;

        public OutputState(PlayableGraph graph, Animator animator)
        {
            m_output = AnimationPlayableOutput.Create(graph, "Anim", animator);
        }

        public override void AddInput(IAnimationState state, float weight)
        {
            if (state == null) return;
            m_output.SetSourcePlayable(state.Playable);
        }
    }
}
