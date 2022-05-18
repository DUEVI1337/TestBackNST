using BackTest.Models;
using System.Collections;
using System.Collections.Generic;

namespace BackTestUnitTests.Tests.Data
{
    public class UpdatePersonDtoData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                new UpdatePersonDto
                {
                     Name = "EgorUpdate",
                     DisplayName = "DuvanovUpdate",
                     PersonSkills = new Dictionary<string,byte> { {"c#", 3}, {"c--", 2},{"c##", 3 } }
                },
                1 //id person
            };
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
