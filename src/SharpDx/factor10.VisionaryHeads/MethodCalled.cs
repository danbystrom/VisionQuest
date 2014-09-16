using Mono.Cecil;
using Mono.Cecil.Cil;

namespace factor10.VisionQuest.Metrics
{
    public interface IMemberReference : IMetadataTokenProvider
    {

        string Name { get; set; }
        TypeReference DeclaringType { get; }
    }

    public class MethodCalled
    {
        public IMemberReference memberReference { get; set; }
        public SequencePoint sequencePoint { get; set; }

        public MethodCalled(IMemberReference _memberReference, SequencePoint _sequencePoint)
        {
            memberReference = _memberReference;
            sequencePoint = _sequencePoint;
        }
    }
}
