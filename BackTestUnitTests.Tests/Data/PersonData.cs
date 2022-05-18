using BackTest.Models;
using System.Collections;
using System.Collections.Generic;

namespace BackTestUnitTests.Tests.Data
{
    public class PersonData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                new Person { Id = 1, Name = "Egor", DisplayName = "duevi", PersonSkills = new List<PersonSkills>()
                    {
                        new PersonSkills {PersonId = 1, SkillName = "c#", Level = 3 },
                        new PersonSkills {PersonId = 1, SkillName = "c++", Level = 2 }
                    }
                }        
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
