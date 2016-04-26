﻿using System;
using NUnit.Framework;

namespace DryIoc.IssuesTests
{
    [TestFixture]
    public class Issue274_Lazy_resolution_of_dependency_registered_with_ReuseInResolutionScope_breaks_subsequent_resolutions_without_wrappers
    {
        private interface IDependency { }

        private class Dependency : IDependency { }

        private interface IService { }

        private class Service : IService
        {
            public Service(IDependency dependency) { }
        }

        private interface ILazyService { }
        private class LazyService : ILazyService
        {
            public LazyService(Lazy<IDependency> dependency)
            {
                var a = dependency.Value;
            }
        }

        [Test]
        public void FirstLazyResolutionShouldNotBreakSubsequentResolutions()
        {
            var container = new Container();
            container.Register<IDependency, Dependency>(Reuse.InResolutionScope);
            container.Register<IService, Service>();
            container.Register<ILazyService, LazyService>();

            Assert.DoesNotThrow(() => container.Resolve<ILazyService>());
            Assert.DoesNotThrow(() => container.Resolve<IService>());
        }
    }
}
