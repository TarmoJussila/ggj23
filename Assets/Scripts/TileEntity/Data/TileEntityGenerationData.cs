using UnityEngine;

namespace Klonk.TileEntity.Data
{
    [CreateAssetMenu(fileName = nameof(TileEntityGenerationData), menuName = nameof(TileEntityGenerationData))]
    public class TileEntityGenerationData : ScriptableObject
    {
        public int SolidPatchGenerationAmount => _solidPatchGenerationAmount;
        public int SolidGenerationAmount => _solidGenerationAmount;
        public int WaterGenerationAmount => _waterGenerationAmount;
        public int AcidGenerationAmount => _acidGenerationAmount;
        public int GenerationWidth => _generationWidth;
        public int GenerationHeight => _generationHeight;

        [SerializeField] private int _solidPatchGenerationAmount = 10;
        [SerializeField] private int _solidGenerationAmount = 1000;
        [SerializeField] private int _waterGenerationAmount = 1000;
        [SerializeField] private int _acidGenerationAmount = 500;
        [SerializeField] private int _generationWidth = 1000;
        [SerializeField] private int _generationHeight = 1000;

        private void OnValidate()
        {
            _solidGenerationAmount = Mathf.Min(_solidGenerationAmount, _generationWidth * _generationHeight);
            _waterGenerationAmount = Mathf.Min(_waterGenerationAmount, _generationWidth * _generationHeight);
            _acidGenerationAmount = Mathf.Min(_acidGenerationAmount, _generationWidth * _generationHeight);
        }
    }
}