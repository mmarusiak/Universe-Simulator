using System.Collections.Generic;

namespace UniverseSound
{
    /// <summary>
    /// Hash table for Universe Sounds.
    /// </summary>
    public class UniverseSoundHash
    {
        private List<List<UniverseSoundNode>> _hashSet = new ();

        public int GetHash(UniverseSoundNode node) => node.Name.ToLower()[0] % 4;
        public int GetHash(string name) => name.ToLower()[0] % 4;
        
        public void AddToSet(UniverseSoundNode node)
        {
            int index = GetHash(node);
            while (index > _hashSet.Count - 1)
                _hashSet.Add(new());
            _hashSet[index].Add(node);
        }

        public UniverseSoundNode GetNodeByName(string name)
        {
            int index = GetHash(name);
            foreach (var node in _hashSet[index])
                if (node.Name == name) return node;
            return null;
        }
    }
}