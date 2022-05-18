using System.Collections;
using System.Collections.Generic;

namespace BackTestIntegrationTests.Tests.Data
{
    public class PersonSkillsData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                new Dictionary<string,byte> { { "c#", 3}, { "c++", 2 } },
                1
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
