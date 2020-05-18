using System;
using System.Threading.Tasks;

namespace FluentAssertionsDummyClasses
{
  public class TestHelper
  {
    public Task MetodoConExcepcion() {
      throw new ArgumentNullException();
    }
  }
}
