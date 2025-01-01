using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityAnimationWindow.Custom_Animation_Window_Tryhard.Editor.Swifter
{
    public class ParticleSystemPlayback
    {
        private List<ParticleSystemPlayer> m_ParticleSystemPlayers = new List<ParticleSystemPlayer>();
        
        public void Setup(GameObject root, AnimationClip clip)
        {
            m_ParticleSystemPlayers.Clear();

            IEnumerable<ParticleSystem> particleSystems = CollectParticleSystems(root);
            
            // temp
            foreach (ParticleSystem particleSystem in particleSystems)
            {
                m_ParticleSystemPlayers.Add(new ParticleSystemPlayer(particleSystem, new List<float>()));
            }
        }

        private IEnumerable<ParticleSystem> CollectParticleSystems(GameObject root)
        {
            if (root.TryGetComponent(out ParticleSystem rootParticleSystem))
            {
                yield return rootParticleSystem;
            }

            foreach (ParticleSystem childParticleSystem in root.GetComponentsInChildren<ParticleSystem>())
            {
                yield return childParticleSystem;
            }
        }
        
        public void Pause()
        {
            foreach (ParticleSystemPlayer particleSystemPlayer in m_ParticleSystemPlayers)
            {
                particleSystemPlayer.Pause();
            }
        }
        
        public void Seek(float time)
        {
            foreach (ParticleSystemPlayer particleSystemPlayer in m_ParticleSystemPlayers)
            {
                particleSystemPlayer.Seek(time);
            }
        }
        
        public void Reset(float time)
        {
            foreach (ParticleSystemPlayer particleSystemPlayer in m_ParticleSystemPlayers)
            {
                particleSystemPlayer.Reset(time);
            }
        }
    }
}