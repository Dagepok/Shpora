﻿using System.ComponentModel;
using FluentAssertions;
using NUnit.Framework;

namespace HomeExercises
{
    public class ObjectComparison
    {
        [Test]
        [NUnit.Framework.Description("Проверка текущего царя")]
        [NUnit.Framework.Category("ToRefactor")]
        public void CheckCurrentTsar()
        {
            var actualTsar = TsarRegistry.GetCurrentTsar();

            var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
                new Person("Vasili III of Russia", 28, 170, 60, null));


            //Assert.AreEqual(actualTsar.Name, expectedTsar.Name);
            //Assert.AreEqual(actualTsar.Age, expectedTsar.Age);
            //Assert.AreEqual(actualTsar.Height, expectedTsar.Height);
            //Assert.AreEqual(actualTsar.Weight, expectedTsar.Weight);

            //Assert.AreEqual(expectedTsar.Parent.Name, actualTsar.Parent.Name);
            //Assert.AreEqual(expectedTsar.Parent.Age, actualTsar.Parent.Age);
            //Assert.AreEqual(expectedTsar.Parent.Height, actualTsar.Parent.Height);
            //Assert.AreEqual(expectedTsar.Parent.Parent, actualTsar.Parent.Parent);

            actualTsar.ShouldBeEquivalentTo(expectedTsar, options => options
                .Excluding(x => x.SelectedMemberInfo.Name.Equals("Id") && x.SelectedMemberInfo.DeclaringType == typeof(Person)));

        }

        [Test]
        [NUnit.Framework.Description("Альтернативное решение. Какие у него недостатки?")]
        public void CheckCurrentTsar_WithCustomEquality()
        {
            var actualTsar = TsarRegistry.GetCurrentTsar();
            var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null));

            // Какие недостатки у такого подхода? 
            Assert.True(AreEqual(actualTsar, expectedTsar));



            //Недостатки
            //Необходимо менять метод AreEqual после каждого изменения класса Person (если изменения влияют на сравнение)
            //Читаемость не очень




            //1.В решении выше после изменения класса Person нет необходимости менять тест
            //если не добавляется поле/свойство, которое не нужно сравнивать, в плохом случае нужно добавить 
            //еще один Excluding 
            //2.Читаемость
            //3.Пишется меньше кода
        }

        private bool AreEqual(Person actual, Person expected)
        {
            if (actual == expected) return true;
            if (actual == null || expected == null) return false;
            return
            actual.Name == expected.Name
            && actual.Age == expected.Age
            && actual.Height == expected.Height
            && actual.Weight == expected.Weight
            && AreEqual(actual.Parent, expected.Parent);
        }
    }

    public class TsarRegistry
    {
        public static Person GetCurrentTsar()
        {
            return new Person(
                "Ivan IV The Terrible", 54, 170, 70,
                new Person("Vasili III of Russia", 28, 170, 60, null));
        }
    }

    public class Person
    {
        public static int IdCounter = 0;
        public int Age, Height, Weight;
        public string Name;
        public Person Parent;
        public int Id;

        public Person(string name, int age, int height, int weight, Person parent)
        {
            Id = IdCounter++;
            Name = name;
            Age = age;
            Height = height;
            Weight = weight;
            Parent = parent;
        }

    }
}
