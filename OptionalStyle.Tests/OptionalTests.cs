using System;
using System.Threading.Tasks;
using OptionalStyle.exceptions;
using OptionalStyle.Tests.model;
using Shouldly;
using Xunit;

namespace OptionalStyle.Tests
{
    public class OptionalTests
    {
        [Fact]
        public void Map_MapChainedWithException_ShouldThrowArgumentException()
        {
            var car = Optional<Car>.OfNullable(null);

            ShouldThrowExtensions.ShouldThrow<ArgumentException>(() =>
            {
                car.Map(c =>
                {
                    c.Name = "new Car";
                    return c;
                }).OrElseThrow(() => new ArgumentException());
            });
        }
        
        [Fact]
        public async Task MapAsync_ConvertCarToCell_ShouldBeSuccess()
        {
            var car = Optional.Of(new Car {Name = "Rocket"});
            var cell = await car.Map(async c => await Task.FromResult(new Cell {Name = c.Name}));
            cell.Get().Name.ShouldBe("Rocket");
        }
        
        [Fact]
        public void Map_ConvertCarToCell_ShouldBeSuccess()
        {
            var car = Optional.Of(new Car {Name = "Rocket"});
            var cell = car.Map(c => new Cell {Name = c.Name}).Get();
            cell.Name.ShouldBe("Rocket");
        }

        [Fact]
        public void IsPresent_ShouldBeTrue()
        {
            var car = Optional.Of(new Car());
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
            var car = Optional<Car>.Of(new Car {Name = "opel"});
            var opel = car.OrElse(new Car {Name = "audi"});
            opel.Name.ShouldBe("opel");
        }

        [Fact]
        public void OrElseObject()
        {
            var car = Optional<Car>.Empty();
            var audi = car.OrElse(new Car {Name = "audi"});
            audi.Name.ShouldBe("audi");
        }

        [Fact]
        public void OrElseFunc()
        {
            var car = Optional<Car>.Empty();
            var funcCar = car.OrElseGet(() => new Car {Name = "func car"});
            funcCar.Name.ShouldBe("func car");
        }

        [Fact]
        public void OrElseFuncSelf()
        {
            var car = Optional<Car>.Of(new Car {Name = "opel"});
            var funcCar = car.OrElseGet(() => new Car {Name = "func car"});
            funcCar.Name.ShouldBe("opel");
        }

        [Fact]
        public void IfPresentCallAction()
        {
            var car = Optional<Car>.Of(new Car {Name = "opel"});
            car.IfPresent(_ => _.Name = "audi");

            car.Get().Name.ShouldBe("audi");
        }

        [Fact]
        public void IfPresentDontCallAction()
        {
            var car = Optional<Car>.Empty();
            car.IfPresent(_ => _.Name = "audi");
            ShouldThrowExtensions.ShouldThrow<OptionalValueNotSetException>((() => car.Get()));
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
            var car = Optional<Car>.Of(new Car());
            car.OrElseThrow(() => new Exception());
        }

        [Fact]
        public void Of_PassNull_ShouldThrowArgumentNullException()
        {
            ShouldThrowExtensions.ShouldThrow<ArgumentNullException>(() => Optional<Car>.Of(null));
        }

        [Fact]
        public void OfNullable_PassNull_IsPresentShouldBeFalse()
        {
            var car = Optional<Car>.OfNullable(null);
            car.IsPresent().ShouldBe(false);
        }

        [Fact]
        public void Map_ChangeKiaToOpel_NameShouldBeOpel()
        {
            var car = Optional<Car>.Of(new Car {Name = "kia"});

            var mapKiaToOpel = car.Map(c =>
            {
                c.Name = "opel";
                return c;
            });

            mapKiaToOpel.IsPresent().ShouldBeTrue();
            mapKiaToOpel.Get().Name.ShouldBe("opel");
        }

        [Fact]
        public void Map_MapOnEmpty_IsPresentShouldBeFalse()
        {
            var car = Optional<Car>.Empty();
            var emptyCar = car.Map(c => Optional<Car>.Of(new Car()));
            emptyCar.IsPresent().ShouldBeFalse();
        }

        [Fact]
        public void Get_ReturnsCar_ShouldReturnOpel()
        {
            var car = Optional<Car>.Of(new Car {Name = "opel"});
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