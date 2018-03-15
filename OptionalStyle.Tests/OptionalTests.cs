using System;
using System.Linq;
using OptionalStyle.exceptions;
using OptionalStyle.Tests.model;
using Shouldly;
using Xunit;

namespace OptionalStyle.Tests
{
    public class OptionalTests
    {
        [Fact]
        public void IsPresent_ShouldBeTrue()
        {
            var car = Optional<Car>.ValueOf(new Car());
            car.IsPresent().ShouldBe(true);
        }

        [Fact]
        public void IsPresent_ShouldBeFalse()
        {
            var car = Optional<Car>.Empty();
            car.IsPresent().ShouldBe(false);
        }

        [Fact]
        public void OrElseObjectSelf()
        {
            var car = Optional<Car>.ValueOf(new Car { Name = "opel" });
            var opel = car.OrElse(new Car { Name = "audi" });
            opel.Name.ShouldBe("opel");
        }

        [Fact]
        public void OrElseObject()
        {
            var car = Optional<Car>.Empty();
            var audi = car.OrElse(new Car { Name = "audi" });
            audi.Name.ShouldBe("audi");
        }

        [Fact]
        public void OrElseFunc()
        {
            var car = Optional<Car>.Empty();
            var funcCar = car.OrElseGet(() => new Car { Name = "func car" });
            funcCar.Name.ShouldBe("func car");
        }

        [Fact]
        public void OrElseFuncSelf()
        {
            var car = Optional<Car>.ValueOf(new Car { Name = "opel" });
            var funcCar = car.OrElseGet(() => new Car { Name = "func car" });
            funcCar.Name.ShouldBe("opel");
        }

        [Fact]
        public void IfPresentCallAction()
        {
            var car = Optional<Car>.ValueOf(new Car { Name = "opel" });
            car.IfPresent(_ => _.Name = "audi");

            car.First().Name = "audi";
        }

        [Fact]
        public void IfPresentDontCallAction()
        {
            var car = Optional<Car>.Empty();
            car.IfPresent(_ => _.Name = "audi");
            car.Any().ShouldBe(false);
        }

        [Fact]
        public void OrElseThrowsException()
        {
            var car = Optional<Car>.Empty();
            Should.Throw<Exception>(() => car.OrElseThrow(() => new Exception()));
        }

        [Fact]
        public void OrElseDoesNotThrowsException()
        {
            var car = Optional<Car>.ValueOf(new Car());
            car.OrElseThrow(() => new Exception());
        }

        [Fact]
        public void ValueOf_PassNull_IsPresentShouldBeNull()
        {
            var car = Optional<Car>.ValueOf(null);
            car.IsPresent().ShouldBe(false);
        }

        [Fact]
        public void Map_ChangeKiaToOpel_NameShouldBeOpel()
        {
            var car = Optional<Car>.ValueOf(new Car {Name = "kia"});

            var mapKiaToOpel = car.Map(c =>
            {
                c.Name = "opel";
                return Optional<Car>.ValueOf(c);
            });

            mapKiaToOpel.IsPresent().ShouldBeTrue();
            mapKiaToOpel.Get().Name.ShouldBe("opel");
        }

        [Fact]
        public void Map_MapOnEmpty_IsPresentShouldBeFalse()
        {
            var car = Optional<Car>.Empty();
            var emptyCar = car.Map(c => Optional<Car>.ValueOf(new Car()));
            emptyCar.IsPresent().ShouldBeFalse();
        }

        [Fact]
        public void Get_ReturnsCar_ShouldReturnOpel()
        {
            var car = Optional<Car>.ValueOf(new Car {Name = "opel"});
            car.Get().Name.ShouldBe("opel");
        }

        [Fact]
        public void Get_EmptyCar_ShouldThrowException()
        {
            var car = Optional<Car>.Empty();
            Should.Throw<OptionalValueNotSetException>(() => car.Get());
        }
    }
}
