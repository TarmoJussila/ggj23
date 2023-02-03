using UnityEngine;

namespace Klonk.TileEntity.Data
{
    [CreateAssetMenu(fileName = nameof(TileEntityGenerationData), menuName = nameof(TileEntityGenerationData))]
    public class TileEntityGenerationData : ScriptableObject
    {
        public int SolidPatchGenerationAmount => _solidPatchGenerationAmount;
        public int SolidGenerationAmount => _solidGenerationAmount;
        public int LiquidGenerationAmount => _liquidGenerationAmount;
        public int GenerationWidth => _generationWidth;
        public int GenerationHeight => _generationHeight;

        [SerializeField] private int _solidPatchGenerationAmount = 10;
        [SerializeField] private int _solidGenerationAmount = 1000;
        [SerializeField] private int _liquidGenerationAmount = 1000;
        [SerializeField] private int _generationWidth = 1000;
        [SerializeField] private int _generationHeight = 1000;

        private void OnValidate()
        {
            _liquidGenerationAmount = Mathf.Min(_liquidGenerationAmount, _generationWidth * _generationHeight);
            _solidGenerationAmount = Mathf.Min(_solidGenerationAmount, _generationWidth * _generationHeight);
        }
    }
}