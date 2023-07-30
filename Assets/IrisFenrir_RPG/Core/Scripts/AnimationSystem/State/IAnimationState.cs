using UnityEngine.Playables;

namespace IrisFenrir.AnimationSystem
{
    public abstract class IAnimationState
    {
        public Playable Playable { get; protected set; }

        public double Time
        {
            get { return Playable.GetTime(); }
            set { Playable.SetTime(value); }
        }

        public double Speed
        {
            get { return Playable.GetSpeed(); }
            set 
            { 
                if(!m_isPaused)
                    Playable.SetSpeed(value); 
            }
        }

        private bool m_isPaused;
        private double m_speedBeforePaused = 1f;
        public bool IsPaused
        {
            get { return m_isPaused; }
            set
            {
                if (m_isPaused == value) return;
                m_isPaused = value;
                if(value)
                {
                    m_speedBeforePaused = Playable.GetSpeed();
                    Playable.SetSpeed(0);
                }
                else
                {
                    Playable.SetSpeed(m_speedBeforePaused);
                }
            }
        }

        public virtual void AddInput(IAnimationState state, float weight)
        {
            if (state == null) return;
            Playable.AddInput(state.Playable, 0, weight);
        }
    }
}
