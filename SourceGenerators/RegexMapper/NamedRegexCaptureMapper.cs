using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Threading;

namespace SourceGenerators.RegexMapper
{
    [Generator]
    internal class NamedRegexCaptureMapper : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            IncrementalValuesProvider<AttributeSyntax> mapperProvider =
            context.SyntaxProvider.CreateSyntaxProvider(
                predicate: (SyntaxNode node, CancellationToken token) =>
                {
                    if (node is AttributeSyntax attributeSyntax && attributeSyntax.Name.ToString() == "NamedRegexMappingAttribute")
                    {
                        return true;
                    }
                    return false;
                },
                transform: (GeneratorSyntaxContext ctx, CancellationToken cancelToken) =>
                {

                    //the transform is called only when the predicate returns true
                    //so for example if we have one class named Calculator
                    //this will only be called once, regardless of how many other classes exist
                    return (AttributeSyntax)ctx.Node;
                });

            //next, we register the Source Output to call the Execute method so we can do something with these filtered items
            context.RegisterSourceOutput(mapperProvider, (sourceProductionContext, calculatorClass)
              => Execute(calculatorClass, sourceProductionContext));
        }

        public void Execute(AttributeSyntax calculatorClass, SourceProductionContext context)
        {
            //Code to perform work on the calculator class
        }
    }
}
