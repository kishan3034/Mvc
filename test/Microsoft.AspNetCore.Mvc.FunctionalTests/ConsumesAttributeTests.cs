// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.AspNetCore.Mvc.FunctionalTests
{
    public class ConsumesAttributeTests : ConsumesAttributeTestsBase<BasicWebSite.Startup>
    {
        public ConsumesAttributeTests(MvcTestFixture<BasicWebSite.Startup> fixture)
            : base(fixture)
        {
        }
    }
}