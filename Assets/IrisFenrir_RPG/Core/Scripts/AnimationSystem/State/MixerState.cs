using UnityEngine.Animations;
using UnityEngine.Playables;

namespace IrisFenrir.AnimationSystem
{
    public class MixerState : IAnimationState
    {
        public MixerState(PlayableGraph graph)
        {
            Playable = AnimationMixerPlayable.Create(graph);
        }

        public void SetWeight(int port, float weight)
        {
            if (port < 0 || port >= Playable.GetInputCount()) return;
            Playable.SetInputWeight(port, weight);
        }

        public float GetWeight(int port)
        {
            if (port < 0 || port >= Playable.GetInputCount()) return 0f;
            return Playable.GetInputWeight(port);
        }
    }
}
