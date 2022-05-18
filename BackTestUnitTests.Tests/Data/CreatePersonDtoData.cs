using BackTest.Models;
using System.Collections;
using System.Collections.Generic;

namespace BackTestUnitTests.Tests.Data
{
    public class CreatePersonDtoData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                new CreatePersonDto
                {
                    Name = "Egor",
                    DisplayName = "Duvanov",
                }
            };
        }

        IEnumerator IEnumerable.GetEnumerator()=> GetEnumerator();
    }
}
