using System;
using System.Linq;
using OptionalStyle;
using OptionalSyle.Tests.model;
using Shouldly;
using Xunit;

namespace OptionalSyle.Tests
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
            Should.Throw<Exception>(() => car.OrElseThrow(new Exception()));
        }

        [Fact]
        public void OrElseDoesNotThrowsException()
        {
            var car = Optional<Car>.ValueOf(new Car());
            car.OrElseThrow(new Exception());
        }

        [Fact]
        public void ValueOfNull()
        {
            var car = Optional<Car>.ValueOf(null);
            car.IsPresent().ShouldBe(false);
        }
    }
}
