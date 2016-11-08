﻿using GraphView.GremlinTranslationOps;
using GraphView.GremlinTranslationOps.map;
using GraphView.GremlinTranslationOps.filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphView.GremlinTranslationOps.sideEffect;
using GraphView.GremlinTranslationOps.branch;
using static GraphView.GremlinTranslationOps.filter.GremlinHasOp;

namespace GraphView
{
    public class GraphTraversal2
    {
        internal List<GremlinTranslationOperator> GremlinTranslationOpList = new List<GremlinTranslationOperator>();
        internal GremlinTranslationOperator LastGremlinTranslationOp { set; get; }

        public GraphTraversal2() {

        }

        public GraphTraversal2(GraphTraversal rhs)
        {

        }

        public GraphTraversal2(GraphViewConnection pConnection)
        {
        }

        public void next()
        {
            GraphViewConnection connection = new GraphViewConnection("https://graphview.documents.azure.com:443/",
                "MqQnw4xFu7zEiPSD+4lLKRBQEaQHZcKsjlHxXn2b96pE/XlJ8oePGhjnOofj1eLpUdsfYgEhzhejk2rjH/+EKA==",
                "GroupMatch", "MarvelTest"); ;
            var sqlQuery = LastGremlinTranslationOp.ToWSqlFragment().Generate(connection);
        }

        internal void AddGremlinOperator(GremlinTranslationOperator newGremlinTranslationOp)
        {
            GremlinTranslationOpList.Add(newGremlinTranslationOp);
            if (LastGremlinTranslationOp == null)
            {
                LastGremlinTranslationOp = newGremlinTranslationOp;
            } else
            {
                newGremlinTranslationOp.InputOperator = LastGremlinTranslationOp;
                LastGremlinTranslationOp = newGremlinTranslationOp;
            }
        }

        //GremlinTranslationOperator

        //public GraphTraversal addE(Direction direction, string firstVertexKeyOrEdgeLabel, string edgeLabelOrSecondVertexKey, params Object[] propertyKeyValues)

        public GraphTraversal2 addE(string edgeLabel)
        {
            AddGremlinOperator(new GremlinAddEOp(edgeLabel));
            return this;
        }

        public GraphTraversal2 addInE(string firstVertexKeyOrEdgeLabel, string edgeLabelOrSecondVertexKey, params Object[] propertyKeyValues)
        {
            return this;
        }

        public GraphTraversal2 addOutE(string firstVertexKeyOrEdgeLabel, string edgeLabelOrSecondVertexKey, params Object[] propertyKeyValues)
        {
            return this;
        }

        public GraphTraversal2 addV()
        {
            return this;
        }

        public GraphTraversal2 addV(params Object[] propertyKeyValues)
        {
            return this;
        }

        public GraphTraversal2 addV(string vertexLabel)
        {
            return this;
        }

        public GraphTraversal2 aggregate(string sideEffectKey)
        {
            return this;
        }

        public GraphTraversal2 and(params GraphTraversal2[] andTraversals)
        {
            GremlinAndOp AndOp = new GremlinAndOp();
            foreach (var traversal in andTraversals)
            {
                AndOp.ConjunctiveOperators.Add(traversal.LastGremlinTranslationOp);
            }
            AddGremlinOperator(AndOp);
            return this;
        }


        public GraphTraversal2 As(params string[] GremlinTranslationOperatorLabels) {
            AddGremlinOperator(new GremlinAsOp(GremlinTranslationOperatorLabels));
            return this;    
        }
        //public GraphTraversal2 barrier()
        //public GraphTraversal2 barrier(Comsumer<org.apache.tinkerpop.gremlin.process.traversal.traverser.util,.TraverserSet<Object>> barrierConsumer)
        //public GraphTraversal2 both(params string[] edgeLabels)
        //public GraphTraversal2 bothE(params string[] edgeLabels)
        //public GraphTraversal2 branch(Function<Traversal<E>, M> function)
        //public GraphTraversal2 branch(Traversal<?, M> branchTraversal)

        public GraphTraversal2 by()
        {
            AddGremlinOperator(new GremlinByOp());
            return this;
        }

        //public GraphTraversal2 by(Comparator<E> comparator)
        //public GraphTraversal2 by(Function<U, Object> function, Comparator comparator)
        //public GraphTraversal2 by(Function<V, Object> function)

        public GraphTraversal2 by(Order order)
        {
            AddGremlinOperator(new GremlinByOp(order));
            return this;
        }

        public GraphTraversal2 by(string key)
        {
            AddGremlinOperator(new GremlinByOp(key));
            return this;
        }

        //public GraphTraversal2 by(string key, Comparator<V> comparator)
        //public GraphTraversal2 by(T token)
        //public GraphTraversal2 by(Traversal<?, ?> traversal)
        //public GraphTraversal2 by(Traversal<?, ?> traversal, Comparator comparator)
        //public GraphTraversal2 cap(string sideEffectKey, params string[] sideEffectKeys)
        //public GraphTraversal2 choose(Function<E, M> choiceFunction)
        //public GraphTraversal2 choose(Predicate<E> choosePredicate, Traversal<?, E2> trueChoice, Traversal<?, E2> falseChoice)
        //public GraphTraversal2 choose(Traversal<?, ?>t traversalPredicate, Travaersal<?, E2> trueChoice, Traversal<?, E2> falseChoice)
        //public GraphTraversal2 choose(Traversal<?, M> choiceTraversal)
        //public GraphTraversal2 coalesce(Traversal<?, E2> ..coalesceTraversals)
        //public GraphTraversal2 coin(double probability)
        //public GraphTraversal2 constant(E2 e)
        public GraphTraversal2 count()
        {
            AddGremlinOperator(new GremlinCountOp());
            return this;
        }

        //public GraphTraversal2 count(Scope scope)
        //public GraphTraversal2 cyclicPath()
        //public GraphTraversal2 dedup(Scope scope, params string[] dedupLabels)
        //public GraphTraversal2 dedup(params string[] dedupLabels)

        public GraphTraversal2 drop()
        {
            AddGremlinOperator(new GremlinDropOp());
            return this;
        }

        public GraphTraversal2 E()
        {
            AddGremlinOperator(new GremlinEOp());
            return this;
        }


        //public GraphTraversal2 emit()
        //public GraphTraversal2 emit(Predicate<Traversal<E>> emitPredicate)
        //public GraphTraversal2 emit(Traversal<?, ?> emitTraversal)
        //public GraphTraversal2 filter(Predicate<Traversal<E>> predicate)
        //public GraphTraversal2 filter(Traversal<?, ?> filterTraversal)
        //public GraphTraversal2 flatMap(Funtion<Traversal<E>, Iterator<E>> funtion)
        //public GraphTraversal2 flatMap(Traversal<?, E2> flatMapTraversal)
        //public GraphTraversal2 fold()
        //public GraphTraversal2 fold(E2 seed, BiFuntion<E2, E, E2> foldFunction)
        //public GraphTraversal2 from(string fromGremlinTranslationOperatorLabel)
        //public GraphTraversal2 from(Traversal<E, Vertex> fromVertex)
        //public GraphTraversal2 group()
        //public GraphTraversal2 group(string sideEffectKey)
        //public GraphTraversal2 groupCount()
        //public GraphTraversal2 groupCount(string sideEffectKey)
        //public GraphTraversal2 groupV3d0() //Deprecated
        //public GraphTraversal2 groupV3d0(string sideEffectKey) //Deprecated

        public GraphTraversal2 has(string propertyKey)
        {
            AddGremlinOperator(new GremlinHasOp(propertyKey));
            return this;
        }
        public GraphTraversal2 has(string propertyKey, Object value)
        {
            AddGremlinOperator(new GremlinHasOp(propertyKey, value));
            return this;
        }
        public GraphTraversal2 has(string label, string propertyKey, Object value)
        {
            AddGremlinOperator(new GremlinHasOp(label, propertyKey, value));
            return this;
        }
        public GraphTraversal2 has(string propertyKey, Predicate predicate)
        {
            AddGremlinOperator(new GremlinHasOp(propertyKey, predicate));
            return this;
        }
        //public GraphTraversal2 has(string label, string propertyKey, Predicate predicate)

        public GraphTraversal2 has(string propertyKey, GraphTraversal2 propertyTraversal)
        {
            AddGremlinOperator(new GremlinHasOp(propertyKey, propertyTraversal.LastGremlinTranslationOp));
            return this;
        }
        
        public GraphTraversal2 hasId(params object[] values)
        {
            AddGremlinOperator(new GremlinHasOp(HasOpType.hasId, values));
            return this;
        }

        public GraphTraversal2 hasKey(params object[] values)
        {
            AddGremlinOperator(new GremlinHasOp(HasOpType.hasKeys, values));
            return this;
        }

        public GraphTraversal2 hasLabel(params object[] values)
        {
            AddGremlinOperator(new GremlinHasOp(HasOpType.hasLabel, values));
            return this;
        }

        public GraphTraversal2 hasValue(string value, params string[] values)
        {
            AddGremlinOperator(new GremlinHasOp(HasOpType.hasValue, values));
            return this;
        }
        public GraphTraversal2 hasNot(string propertyKey)
        {
            return this;
        }



        public GraphTraversal2 id()
        {
            return this;
        }

        public GraphTraversal2 In(params string[] edgeLabels)
        {
            AddGremlinOperator(new GremlinInOp(edgeLabels));
            return this;
        }

        public GraphTraversal2 inE(params string[] edgeLabels)
        {
            AddGremlinOperator(new GremlinInEOp(edgeLabels));
            return this;
        }

        //public GraphTraversal2 inject()

        public GraphTraversal2 inV()
        {
            AddGremlinOperator(new GremlinInVOp());
            return this;
        }

        public GraphTraversal2 Is(string value)
        {
            return this;
        }

        //public GraphTraversal2 Is(P<E> predicate)

        public GraphTraversal2 iterate()
        {
            return this;
        }

        public GraphTraversal2 key()
        {
            return this;
        }

        public GraphTraversal2 label()
        {
            return this;
        }

        public GraphTraversal2 limit(long limit)
        {
            AddGremlinOperator(new GremlinLimitOp(limit));
            return this;
        }

        //public GraphTraversal2 limit(Scope scope, long limit)

        //public GraphTraversal2 local(Traversal<?, E2> localTraversal)
        //public GraphTraversal2 loops()
        //public GraphTraversal2 map(Function<Traversal<?, E2>> function)
        //public GraphTraversal2 map(Traversal<?, E2> mapTraversal)
        //public GraphTraversal2 mapKeys() //Deprecated
        //public GraphTraversal2 mapvalues() //Deprecated
        //public GraphTraversal2 match(Traversal<?, ?>..matchTraversals)

        public GraphTraversal2 max()
        {
            AddGremlinOperator(new GremlinMaxOp());
            return this;
        }

        public GraphTraversal2 max(Scope scope)
        {
            AddGremlinOperator(new GremlinMaxOp(scope));
            return this;
        }

        public GraphTraversal2 mean()
        {
            AddGremlinOperator(new GremlinMeanOp());
            return this;
        }

        public GraphTraversal2 mean(Scope scope)
        {
            AddGremlinOperator(new GremlinMeanOp(scope));
            return this;
        }

        public GraphTraversal2 min()
        {
            AddGremlinOperator(new GremlinMinOp());
            return this;
        }

        public GraphTraversal2 min(Scope scope)
        {
            AddGremlinOperator(new GremlinMinOp(scope));
            return this;
        }


        //public GraphTraversal2 not(Traversal<?, ?> notTraversal)
        //public GraphTraversal2 option(M pickToken, Traversal<E, E2> traversalOption)
        //public GraphTraversal2 option(Traversal<E, E2 tarversalOption>
        //public GraphTraversal2 optional(Traversal<E, E2> traversalOption)

        public GraphTraversal2 Or(params GraphTraversal2[] orTraversals)
        {
            GremlinOrOp orOp = new GremlinOrOp();
            foreach (var traversal in orTraversals)
            {
                orOp.ConjunctiveOperators.Add(traversal.LastGremlinTranslationOp);
            }
            AddGremlinOperator(orOp);
            return this;
        }

        public GraphTraversal2 order()
        {
            AddGremlinOperator(new GremlinOrderOp());
            return this;
        }

        //public GraphTraversal2 order(Scope scope)


        public GraphTraversal2 otherV()
        {
            return this;
        }

        public GraphTraversal2 Out(params string[] edgeLabels)
        {
            AddGremlinOperator(new GremlinOutOp(edgeLabels));
            return this;
        }

        public GraphTraversal2 outE(params string[] edgeLabels)
        {
            AddGremlinOperator(new GremlinOutEOp(edgeLabels));
            return this;
        }

        public GraphTraversal2 outV()
        {
            AddGremlinOperator(new GremlinOutVOp());
            return this;
        }

        //public GraphTraversal2 pageRank()
        //public GraphTraversal2 pageRank(double alpha)
        //public GraphTraversal2 path()
        //public GraphTraversal2 peerPressure()
        //public GraphTraversal2 profile()
        //public GraphTraversal2 profile(string sideEffectKey)
        //public GraphTraversal2 program(VertexProgram<?> vertexProgram)
        //public GraphTraversal2 project(string projectKey, params string[] otherProjectKeys)

        public GraphTraversal2 properties(params string[] propertyKeys)
        {
            return this;
        }

        public GraphTraversal2 property(string key, string value, params string[] keyValues)
        {
            return this;
        }

        //public GraphTraversal2 property(VertexProperty.Cardinality cardinality, string key, string value, params string[] keyValues)

        public GraphTraversal2 propertyMap(params string[] propertyKeys)
        {
            return this;
        }

        public GraphTraversal2 range(long low, long high)
        {
            return this;
        }

        public GraphTraversal2 repeat(GraphTraversal2 repeatTraversal)
        {
            AddGremlinOperator(new GremlinRepeatOp(repeatTraversal.LastGremlinTranslationOp));
            return this;
        }

        //public GraphTraversal2 sack() //Deprecated
        //public GraphTraversal2 sack(BiFunction<V, U, V>) sackOperator) //Deprecated
        //public GraphTraversal2 sack(BiFunction<V, U, V>) sackOperator, string, elementPropertyKey) //Deprecated

        public GraphTraversal2 sample(int amountToSample)
        {
            return this;
        }

        //public GraphTraversal2 sample(Scope scope, int amountToSample)
        //public GraphTraversal2 select(Column column)
        //public GraphTraversal2 select(Pop pop, string selectKey)
        //public GraphTraversal2 select(Pop pop, string selectKey1, string selectKey2, params string[] otherSelectKeys)
        //public GraphTraversal2 select(string selectKey)
        //public GraphTraversal2 select(string selectKey1, string selectKey2, params string[] otherSelectKeys)
        //public GraphTraversal2 sideEffect(Consumer<Traverser<E>> consumer)
        //public GraphTraversal2 sideEffect(Traversal<?, ?> sideEffectTraversal)
        //public GraphTraversal2 simplePath()
        //public GraphTraversal2 store(string sideEffectKey)
        //public GraphTraversal2 subgraph(string sideEffectKey)

        public GraphTraversal2 sum()
        {
            return this;
        }

        //public GraphTraversal2 sum(Scope scope)


        public GraphTraversal2 tail()
        {
            return this;
        }

        public GraphTraversal2 tail(long limit)
        {
            return this;
        }

        //public GraphTraversal2 tail(Scope scope)


        //public GraphTraversal2 tail(Scope scope, long limit)

        public GraphTraversal2 timeLimit(long timeLimit)
        {
            return this;
        }

        public GraphTraversal2 times(int maxLoops)
        {
            return this;
        }

        //public GraphTraversal2 to(Direction direction, params string[] edgeLabels)

        public GraphTraversal2 to(string toGremlinTranslationOperatorLabel)
        {
            return this;
        }

        //public GraphTraversal2 to(Traversal<E, Vertex> toVertex)
        //public GraphTraversal2 toE(Direction direction, params string[] edgeLabels)
        //public GraphTraversal2 toV(Direction direction)
        //public GraphTraversal2 tree()
        //public GraphTraversal2 tree(string sideEffectKey)

        public GraphTraversal2 unfold()
        {
            return this;
        }

        //public GraphTraversal2 union(params Traversal<?, E2>[] unionTraversals)
        //public GraphTraversal2 until(Predicate<Traverser<E>> untilPredicate)
        //public GraphTraversal2 unitl(Traversal<?, ?> untilTraversal)

        public GraphTraversal2 V(params object[] vertexIdsOrElements)
        {
            AddGremlinOperator(new GremlinVOp(vertexIdsOrElements));
            return this;
        }

        public GraphTraversal2 value()
        {
            return this;
        }

        public GraphTraversal2 valueMap(Boolean includeTokens, params string[] propertyKeys)
        {
            return this;
        }

        public GraphTraversal2 valueMap(params string[] propertyKeys)
        {
            return this;
        }

        public GraphTraversal2 values(params string[] propertyKeys)
        {
            return this;
        }

        public GraphTraversal2 where(Predicate predicate)
        {
            AddGremlinOperator(new GremlinWhereOp(predicate));
            return this;
        }

        public GraphTraversal2 where(string startKey, Predicate predicate)
        {
            AddGremlinOperator(new GremlinWhereOp(startKey, predicate));
            return this;
        }

        public GraphTraversal2 where(GraphTraversal2 whereTraversal)
        {
            AddGremlinOperator(new GremlinWhereOp(whereTraversal.LastGremlinTranslationOp));
            return this;
        }

        public static GraphTraversal2 underscore()
        {
            GraphTraversal2 newGraphTraversal = new GraphTraversal2();
            newGraphTraversal.AddGremlinOperator(new GremlinParentContextOp());
            return newGraphTraversal;
        }
    }
}


