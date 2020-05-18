using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Extensions;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace FirstApproachToFluentAssertions
{
  public class Introduccion
  {
    #region Variables dummy
    int _enteroVeinte;
    string _cadenaMayuscula;
    string _cadenaCorta;
    List<string> _lista;
    object _objetoNulo;
    object _objetoConValor;
    #endregion
    public Introduccion()
    {
      _enteroVeinte = 20;
      _cadenaMayuscula = "ESTERNOCLEIDOMASTOIDEO";
      _cadenaCorta = "Y EIA?";
      _lista = new List<string> { "elemento1", "elemento2", "elemento3" };
      //_list = new List<string> { "elemento1", "elemento2", "elemento3", "elemento4" };
      _objetoNulo = null;
      _objetoConValor = new { propiedad1 = "y la mami?", propiedad2 = "y la chiquita?" };
    }

    [Fact]
    public void TestEnteroVeinte()
    {
      //Assert.Equal(20, _intTwenty);
      _enteroVeinte.Should().Be(20, because: "nosotros le pusimos el valor de 20");
    }

    [Fact]
    public void TestEnteroVeinteMensajeCustom()
    {
      //Assert.Equal(20, _intTwenty);
      const string because = "nosotros pensamos que pusimos {0} en la {1}, {2}";
      var becauseArgs = new[] { "20", "variable", "seguí participando" };
      _enteroVeinte.Should().Be(20, because, becauseArgs);
    }

    [Fact]
    public void TestStringMayus()
    {
      _cadenaMayuscula.Should().StartWith("ES").And.EndWith("DEO")
        .And.Contain("CLEIDO");
    }

    [Fact]
    public void TestStringMayusEquivalente()
    {
      _cadenaMayuscula.Should().StartWithEquivalent("es").And.EndWithEquivalent("deo")
        .And.ContainEquivalentOf("cleido");
    }

    [Fact]
    public void TestListaStringTieneTres()
    {
      //Assert.Equal(3, _list.Count);
      //Criminal...
      //Assert.True(_list.Count == 3);
      _lista.Should().HaveCount(3, "nosotros creemos que pusimos 3 elementos en la lista, vofi...");
    }

    [Fact]
    public void TestStringMayusLongitud()
    {
      _cadenaMayuscula.Should().HaveLength(22);
    }

    [Fact]
    public void TestExcpcion()
    {
      Action act = () => throw new ArgumentNullException();

      act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void TestNoExcpcion()
    {
      Action act = () => { };

      act.Should().NotThrow<ArgumentNullException>();
    }

    [Fact]
    public void TestExcpcionMensaje()
    {
      //Arrange
      Action act = () => throw new ArgumentNullException();

      try
      {
        // Act
        act.Should().ThrowExactly<ArgumentException>("porque {0} deberia hacer eso, otsea...", "NuestraClase.Metodo");
        throw new XunitException("No se llega hasta aca.");
      }
      catch (XunitException ex)
      {
        // Assert
        ex.Message.Should().Match("Expected type to be System.ArgumentException because porque NuestraClase.Metodo deberia hacer eso, otsea..., but found System.ArgumentNullException.");
      }
    }

    [Fact]
    public void TestExcpcionNombrePropiedad()
    {
      Action act = () => throw new ArgumentNullException(nameof(_lista));

      // Act
      act.Should().Throw<ArgumentNullException>()
        .And.ParamName.Should().Be("_lista");
    }

    [Fact]
    public void TestThrowAsincronico()
    {
      Action act = () => Task.Delay(1).Wait(1);

      act.Should().NotThrowAfter(1.Milliseconds(), 1.Milliseconds());
    }

    [Fact]
    public void TestsConScope()
    {
      using (new AssertionScope())
      {
        _enteroVeinte.Should().Be(20);
        _cadenaCorta.Should().Be("Y EIA?");
        _cadenaCorta.Should().BeEquivalentTo("y eia?");
      }
    }

    [Fact]
    public void TestsSinScope()
    {
      _enteroVeinte.Should().Be(20);
      _cadenaCorta.Should().Be("Y EIA?");
      _cadenaCorta.Should().BeEquivalentTo("y eia?");
    }

    [Fact]
    public void TestsObjetoNulo()
    {
      _objetoNulo.Should().BeNull();
      //A tener en cuenta...
      //_objetoNulo.Should().BeOfType<Object>();
    }

    [Fact]
    public void TestsObjetoConValor()
    {
      _objetoConValor.Should().NotBeNull();
      _objetoConValor.Should().NotBeOfType<string>();
    }

    [Fact]
    public void TestsObjetoConValorIgualImplementacíon()
    {
      var objetoInterno = _objetoConValor;
      using (new AssertionScope())
      {
        objetoInterno.Should().Be(_objetoConValor, "lo igualamos antes viteh...");
        objetoInterno.Should().NotBe(_objetoNulo, "lo mismo que el anterior...");
      }
    }

    [Fact]
    public void TestsObjetoConValorIgualObjetoEnMemoria()
    {
      var objetoInterno = _objetoConValor;
      using (new AssertionScope())
      {
        objetoInterno.Should().BeSameAs(_objetoConValor, "lo igualamos antes viteh...");
        objetoInterno.Should().NotBeSameAs(_objetoNulo, "lo mismo que el anterior...");
      }
    }

    [Fact]
    public void TestsExcepcionArgumentoNuloHeredaDeExcepcion()
    {
      var ex = new ArgumentNullException();

      ex.Should().BeAssignableTo<Exception>("tampoco vamos a inventar la polvora...");
    }

    [Fact]
    public void TestObjetoEsXMLSerializable()
    {
      // Arrange
      var subject = new XmlSerializableClass
      {
        Nombre = "Juan",
        Id = 1
      };

      // Act
      Action act = () => subject.Should().BeXmlSerializable();

      // Assert
      act.Should().NotThrow();
    }

    [Fact]
    public void TestObjetoEsSerializable()
    {
      // Arrange
      var subject = new SerializableClass
      {
        Nombre = "Juan",
        Id = 1
      };

      // Act
      Action act = () => subject.Should().BeBinarySerializable();

      // Assert
      act.Should().NotThrow();
    }

    [Fact]
    public void TestObjetoEsDataContractSerializable()
    {
      // Arrange
      var subject = new DataContractSerializableClassNotRestoringAllProperties
      {
        Nombre = "Juan",
        Cumpleanios = new DateTime(1990, 02, 14)
      };

      // Act
      Action act = () => subject.Should().BeDataContractSerializable<DataContractSerializableClassNotRestoringAllProperties>(
          options => options.Excluding(x => x.Nombre));

      // Assert
      act.Should().NotThrow();
    }

    [Fact]
    public void TestEnumTieneElValorEsperado()
    {
      EnumTest enumTest = EnumTest.Dos;
      enumTest.Should().HaveFlag(EnumTest.Dos);
    }
  }

  #region Class Helpers

  public class XmlSerializableClass
  {
    public string Nombre { get; set; }

    public int Id;
  }

  [Serializable]
  public class SerializableClass
  {
    public string Nombre { get; set; }

    public int Id;
  }

  [DataContract]
  public class DataContractSerializableClassNotRestoringAllProperties : InterfazSalvaje
  {
    public virtual string Nombre { get; set; }

    [DataMember]
    public virtual DateTime Cumpleanios { get; set; }

    public virtual void MetodoSalvaje() { }
  }

  public enum EnumTest
  {
    Nada,
    Uno,
    Dos,
    Tres
  }

  public interface InterfazSalvaje { }
  #endregion
}
