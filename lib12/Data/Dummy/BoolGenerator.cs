﻿using System;
using System.Linq.Expressions;
using lib12.Extensions;
using lib12.Reflection;

namespace lib12.Data.Dummy
{
    public class BoolGenerator<T> : PropertyGenerator<T, bool>
    {

        public BoolGenerator(Expression<Func<T, bool>> selector)
            : base(selector)
        {

        }

        public override void GenerateProperty(T item, Random random)
        {
            Selector.SetValue(item, random.NextBool());
        }
    }
}