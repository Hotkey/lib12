﻿using lib12.DependencyInjection;

namespace lib12.Test.DependencyInjectionTest.Classes
{
    [Singleton]
    internal sealed class WireChoosenProperties
    {
        [WireUp]
        public TransientClass Wired { get; set; }
        public NotRegisteredClass NotWired { get; set; }
    }
}
