using DotNetWrapperGen.CodeModel;
using DotNetWrapperGen.CodeStructure;

namespace DotNetWrapperGen.Transformer
{
    public interface ITransformer
    {
        void Transform(NamespaceDefinition globalNamespace, RootFolderDefinition rootFolder);
    }
}
