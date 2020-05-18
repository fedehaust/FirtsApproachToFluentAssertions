using FluentAssertions;
using FluentAssertions.Common;
using FluentAssertions.Extensions;
using System;
using Xunit;
using Xunit.Sdk;

namespace FirstApproachToFluentAssertions
{
  public class Primitivos
  {
    bool? _boolFalso;
    bool? _boolNulo;
    string _cadenaVacia;
    string _cadenaNoVacia;
    string _cadenaEspacios;
    string _cadenaNoVacioXL;
    string _correoElectronico;
    int _enteroQuince;
    float _pi;
    double _piDouble;
    DateTimeOffset _fechaYHora;
    DateTime _ahora;

    public Primitivos()
    {
      _boolFalso = false;
      _boolNulo = null;
      _cadenaVacia = string.Empty;
      _cadenaNoVacia = "Solo se, que no cenaba";
      _cadenaNoVacioXL = "Si la inflamación no se va, ¿el dolor vuelve? No lo sabemos ahora, pero pero pero... ya haremos una encuesta.";
      _cadenaEspacios = "           ";
      _correoElectronico = "fedehaust@gmail.com";
      _enteroQuince = 15;
      _pi = 3.1415927F;
      _piDouble = 3.1415927;
      _fechaYHora = DateTime.Now.Day.May(DateTime.Now.Year).At(DateTime.Now.Hour - 1, 00).AsLocal().ToDateTimeOffset(-3.Hours());
      _ahora = DateTime.Now;
    }

    [Fact]
    public void VerificarFalso()
    {
      _boolFalso.Should().BeFalse("Y si! ¿Qué mas va a ser?");
    }

    [Fact]
    public void VerificarFalsoConOtro()
    {
      bool falsoInterno = false;
      _boolFalso.Should().Be(falsoInterno, "Mmmm... no lo se Rick... Parece Falso");
    }

    [Fact]
    public void VerificarQueNoEsFalso()
    {
      _boolNulo.Should().NotBeFalse();
    }

    [Fact]
    public void CadenaVacia()
    {
      _cadenaVacia.Should().BeEmpty();
      _cadenaVacia.Should().BeNullOrEmpty();
    }

    [Fact]
    public void CadenaNoVacia()
    {
      _cadenaNoVacia.Should().NotBeNullOrWhiteSpace();
      _cadenaNoVacia.Should().BeOneOf("Solo se, que no cenaba",
        "Si la inflamación no se va, ¿el dolor vuelve? No lo sabemos ahora, pero ya haremos una encuesta.");
    }

    [Fact]
    public void CadenaSoloEspacios()
    {
      _cadenaEspacios.Should().BeNullOrWhiteSpace();
    }

    [Fact]
    public void CadenaConMasDeUnaOcurrencia()
    {
      _cadenaNoVacioXL.Should().Contain("pero", MoreThan.Twice());
      _cadenaNoVacioXL.Should().Contain("pero", MoreThan.Times(2));
      _cadenaNoVacioXL.Should().Contain("pero", Exactly.Times(3));
      _cadenaNoVacioXL.Should().Contain("pero", Exactly.Thrice());
    }

    [Fact]
    public void CadenasConAlgunPatron()
    {
      _correoElectronico.Should().Match("*@*.com");
      _correoElectronico.Should().Match("*@gmail.com");
      _cadenaNoVacioXL.Should().MatchRegex(@"\b(dolor|ahora)\b");
    }

    [Fact]
    public void ElNumeroEsQuince()
    {
      _enteroQuince.Should().BeGreaterOrEqualTo(15);
      _enteroQuince.Should().BeGreaterOrEqualTo(3);
      _enteroQuince.Should().BeGreaterThan(4);
      _enteroQuince.Should().BeLessOrEqualTo(20);
      _enteroQuince.Should().BeLessThan(26);
    }

    [Fact]
    public void ElNumeroEsReal()
    {
      int enteroNegativo = _enteroQuince * -1;
      _enteroQuince.Should().BePositive();
      enteroNegativo.Should().BeNegative();

    }

    [Fact]
    public void ElNumeroEstaEnUnRango()
    {
      _enteroQuince.Should().BeInRange(10, 20);
      _enteroQuince.Should().NotBeInRange(1, 10);
    }

    [Fact]
    public void ElNumeroDebeCumplirUnPredicado()
    {
      _enteroQuince.Should().Match(x => x % 2 == 1);
    }

    [Fact]
    public void ElNumeroAproximado()
    {
      _pi.Should().BeApproximately(3.14F, 0.01F);
      _pi.Should().NotBeApproximately(2.5F, 0.5F);
      _piDouble.Should().BeApproximately(3.14, 0.01);
      _piDouble.Should().NotBeApproximately(2.5, 0.5);
    }

    [Fact]
    public void ElNumeroCoincideConAlMenosUno()
    {
      _enteroQuince.Should().BeOneOf(new[] { 7, 6, 8 , 15 });
    }

    [Fact]
    public void ElTiempoPasa()
    {
      _fechaYHora.Should().BeOnOrBefore(_ahora);
      _ahora.Should().BeOnOrAfter(_fechaYHora.DateTime);
    }

    [Fact]
    public void PeroSigueSiendoHoy()
    {
      _fechaYHora.Should().BeSameDateAs(18.May(2020).At(23, 00));
    }

    [Fact]
    public void EstaFechaAlgoTieneQueTener()
    {
      _fechaYHora.Should().HaveDay(18);
      _fechaYHora.Should().HaveMonth(5);
      _fechaYHora.Should().HaveYear(2020);
    }

    [Fact]
    public void EstaMasOMenosCerca()
    {
      _fechaYHora.Should().BeLessThan(360.Minutes()).Before(_ahora);
      _fechaYHora.Should().BeWithin(12.Hours()).After(_ahora);
      _ahora.Should().BeCloseTo(18.May(2020).At(DateTime.Now.Hour, DateTime.Now.Minute - 10),2000000000);

      _fechaYHora.Should().NotBeCloseTo(2.March(2010), 1.Hours());
    }
  }
}
