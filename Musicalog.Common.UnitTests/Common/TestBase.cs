using Moq;
using Moq.AutoMock;

namespace Musicalog.Common.UnitTests
{

    public abstract class TestBase<T> where T : class
    {
        protected T ObjectUnderTest;
        private readonly AutoMocker _container;

        protected TestBase()
        {
            _container = new AutoMocker(MockBehavior.Default);
        }

        protected void InitializeObjectUnderTest()
        {
            ObjectUnderTest = _container.CreateInstance<T>();
        }

        protected Mock<T1> For<T1>() where T1 : class
        {
            return _container.GetMock<T1>();
        }
    }
}
