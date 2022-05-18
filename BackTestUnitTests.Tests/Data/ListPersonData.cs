using BackTest.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackTestUnitTests.Tests.Data
{
    public class ListPersonData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                new List<Person>
                {
                    new Person
                    {
                        Id = 1,
                        Name = "Egor",
                        DisplayName = "duevi",
                        PersonSkills = new List<PersonSkills>()
                        {
                            new PersonSkills {PersonId = 1, SkillName = "c#", Level = 3 },
                            new PersonSkills {PersonId = 1, SkillName = "c++", Level = 3 }
                        }
                    },
                    new Person
                    {
                        Id = 2,
                        Name = "Egor2",
                        DisplayName = "duevi2",
                        PersonSkills = new List<PersonSkills>()
                        {
                            new PersonSkills {PersonId = 2, SkillName = "go", Level = 2 },
                            new PersonSkills {PersonId = 2, SkillName = "python", Level = 3 }
                        }
                    }
                }
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
