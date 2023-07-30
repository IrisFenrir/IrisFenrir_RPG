using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace IrisFenrir.AnimationSystem
{
    public class AnimClipState : IAnimationState
    {
        public AnimClipState(PlayableGraph graph, AnimationClip clip)
        {
            Playable = AnimationClipPlayable.Create(graph, clip);
        }
    }
}
