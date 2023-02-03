using UnityEngine;

namespace Klonk.TileEntity.Data
{
    [CreateAssetMenu(fileName = nameof(TileEntityGenerationData), menuName = nameof(TileEntityGenerationData))]
    public class TileEntityGenerationData : ScriptableObject
    {
        public int GenerationAmount => _generationAmount;
        public int GenerationWidth => _generationWidth;
        public int GenerationHeight => _generationHeight;

        [SerializeField] private int _generationAmount = 1000;
        [SerializeField] private int _generationWidth = 1000;
        [SerializeField] private int _generationHeight = 1000;

        private void OnValidate()
        {
            _generationAmount = Mathf.Min(_generationAmount, _generationWidth * _generationHeight);
        }
    }
}