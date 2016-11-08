﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphView.GremlinTranslationOps.map
{
    internal class GremlinOrderOp: GremlinTranslationOperator
    {
        public GremlinOrderOp() { }

        public override GremlinToSqlContext GetContext()
        {
            GremlinToSqlContext inputContext = GetInputContext();

            return inputContext;
        }
    }

    public enum Order
    {
        shuffle,
        desr,
        incr
    }
}
