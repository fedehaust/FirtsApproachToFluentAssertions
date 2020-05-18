using FluentAssertions;
using FluentAssertions.Common;
using FluentAssertions.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xunit;
using Xunit.Sdk;

namespace FirstApproachToFluentAssertions
{
  public class Restantes
  {
    List<string> _lista;
    Dictionary<int, string> _diccionarioVacio;
    Dictionary<int, string> _diccionario;
    Guid _id;
    public Restantes()
    {
      _lista = new List<string> { "elemento1", "elemento2", "elemento3" };
      _diccionario = new Dictionary<int, string> { { 1, "Elemento1"}, { 2, "Elemento2"}, { 3, "Elemento3"} };
      _diccionarioVacio = new Dictionary<int, string>();
      _id = Guid.NewGuid();
    }

    [Fact]
    public void LaListaNoDebeEstarVacia()
    {
      _lista.Should().NotBeEmpty().And.HaveCount(3).And.ContainInOrder(new List<string> { "elemento2", "elemento3" }).And.ContainItemsAssignableTo<string>()
        .And.BeEquivalentTo("elemento3", "elemento1", "elemento2" ).And.OnlyHaveUniqueItems().And.HaveCountGreaterThan(2)
        .And.BeSubsetOf(new List<string> { "elemento1", "elemento2", "elemento3", "elemento4" });
    }

    [Fact]
    public void ElDiccionarioTambienAlgoDebeTener()
    {
      _diccionario.Should().NotBeEmpty().And.ContainKey(1).And.NotContainKeys(25,26).And.ContainValues("Elemento1", "Elemento2").And.NotContainValue("El quinto elemento");
    }

    [Fact]
    public void NoHayNadaQueVerAqui()
    {
      _diccionarioVacio.Should().NotBeNull().And.BeEmpty();
    }

    [Fact]
    public void AlgoMasConXML()
    {
      // Arrange
      var document = XDocument.Parse(
          @"<parent xmlns='http://www.example.com/2012/test'>
                    <child/>
                  </parent>");


      // Act
      Action act = () =>
          document.Should().HaveRoot(XName.Get("parent", "http://www.example.com/2012/test"));

      // Assert
      act.Should().NotThrow();
    }

    [Fact]
    public void VamosADormirUnPoco()
    {
      Action act = () => Thread.Sleep(100);

      act.ExecutionTime().Should().BeLessOrEqualTo(150.Milliseconds(), "Tu si que eres lento...");
    }

    [Fact]
    public void DeberiaEstarDecorado()
    {
      typeof(DataContractSerializableClassNotRestoringAllProperties).Should().BeDecoratedWith<DataContractAttribute>();
      typeof(DataContractSerializableClassNotRestoringAllProperties).Should().NotBeDecoratedWith<SerializableAttribute>();
    }

    [Fact]
    public void AlgunosModificadores()
    {
      typeof(DataContractSerializableClassNotRestoringAllProperties).Should().NotBeStatic();
      MethodInfo methodInfo = typeof(DataContractSerializableClassNotRestoringAllProperties).GetParameterlessMethod("MetodoSalvaje");
      methodInfo.Should().BeVirtual().And.NotBeAsync().And.ReturnVoid();
    }

    [Fact]
    public void AlgoeEstrucutra()
    {
      var types = typeof(DataContractSerializableClassNotRestoringAllProperties).Assembly.Types()
                  .ThatAreDecoratedWith<DataContractAttribute>()
                  .ThatImplement<InterfazSalvaje>();

      var properties = types.Properties().ThatArePublicOrInternal;
      properties.Should().BeVirtual();
    }

  }

}
