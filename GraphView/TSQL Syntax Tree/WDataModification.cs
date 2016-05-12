﻿// GraphView
// 
// Copyright (c) 2015 Microsoft Corporation
// 
// All rights reserved. 
// 
// MIT License
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace GraphView
{
    public abstract partial class WDataModificationSpecification : WSqlStatement
    {
        // Omit the OUTPUT clause
        public WTableReference Target { get; set; }
        public WTopRowFilter TopRowFilter { get; set; }

        public override void Accept(WSqlFragmentVisitor visitor)
        {
            if (visitor != null)
                visitor.Visit(this);
        }

        public override void AcceptChildren(WSqlFragmentVisitor visitor)
        {
            if (Target != null)
                Target.Accept(visitor);
            if (TopRowFilter != null)
                TopRowFilter.Accept(visitor);
            base.AcceptChildren(visitor);
        }
    }

    public abstract partial class WUpdateDeleteSpecificationBase : WDataModificationSpecification
    {
        public WFromClause FromClause { get; set; }
        public WWhereClause WhereClause { get; set; }

        public override void Accept(WSqlFragmentVisitor visitor)
        {
            if (visitor != null)
                visitor.Visit(this);
        }

        public override void AcceptChildren(WSqlFragmentVisitor visitor)
        {
            if (FromClause != null)
                FromClause.Accept(visitor);
            if (WhereClause != null)
                WhereClause.Accept(visitor);
            base.AcceptChildren(visitor);
        }
    }

    public partial class WInsertSpecification : WDataModificationSpecification
    {
        public InsertOption InsertOption { get; set; }
        public WInsertSource InsertSource { get; set; }

        public IList<WColumnReferenceExpression> Columns { get; set; }

        internal override bool OneLine()
        {
            return false;
        }

        internal override string ToString(string indent)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("{0}INSERT INTO {1}", indent, Target.ToString());
            if (Columns != null && Columns.Count > 0)
            {
                sb.AppendFormat(" ({0}", Columns[0].ToString(indent));
                for (var i = 1; i < Columns.Count; ++i)
                {
                    sb.AppendFormat(", {0}", Columns[i].ToString(indent));
                }
                sb.Append(")");
            }
            sb.Append("\r\n");
            sb.Append(InsertSource.ToString(indent));

            return sb.ToString();
        }

        public override void Accept(WSqlFragmentVisitor visitor)
        {
            if (visitor != null)
                visitor.Visit(this);
        }

        public override void AcceptChildren(WSqlFragmentVisitor visitor)
        {
            if (InsertSource != null)
                InsertSource.Accept(visitor);

            var index = 0;
            if (Columns!=null)
                for (var count = Columns.Count; index < count; ++index)
                    Columns[index].Accept(visitor);

            base.AcceptChildren(visitor);
        }
    }

    public partial class WInsertNodeSpecification : WInsertSpecification
    {
        public WInsertNodeSpecification(WInsertSpecification insertSpec)
        {
            InsertOption = insertSpec.InsertOption;
            InsertSource = insertSpec.InsertSource;
            FirstTokenIndex = insertSpec.FirstTokenIndex;
            LastTokenIndex = insertSpec.LastTokenIndex;
            TopRowFilter = insertSpec.TopRowFilter;
            Target = insertSpec.Target;
            Columns = new List<WColumnReferenceExpression>();
            foreach (var col in insertSpec.Columns)
                Columns.Add(col);
        }

        public override void Accept(WSqlFragmentVisitor visitor)
        {
            if (visitor != null)
                visitor.Visit(this);
        }

        //need : Url , Key , DatabaseID , CollectionID
        public override string ToDocDbScript(string EndpointUrl, string AuthorizationKey, string DatabaseID, string CollectionID)
        {
            string json_str = "{}";
            var cnt = InsertSource as WValuesInsertSource;
            for (int i = 0; i < Columns.Count(); i++)
            {
                string s1 = Columns[i].MultiPartIdentifier.Identifiers[0].Value;
                var cnt2 = (cnt.RowValues[0].ColumnValues[i] as WValueExpression);
                string s2 =cnt2.Value;
                if (cnt2.SingleQuoted)
                    s2 = '\"' + s2 + '\"';

                if (s2[0] == '@') return "";
                json_str = GraphViewJsonCommand.insert_property(json_str, s2, s1).ToString();
            }
            json_str = json_str.Replace("\"", "\"\"");
            //cnt.RowValues[0].ColumnValues
            return (DocDBScript_head(EndpointUrl, AuthorizationKey, DatabaseID, CollectionID)
                  + "string json_str = @\"" +json_str + "\";\r\n"
                  + "\t\t\tawait client.CreateDocumentAsync(\"dbs/\" + database.Id + \"/colls/\" + documentCollection.Id, JObject.Parse(json_str));"
                  + DocDBScript_tail()
                );

        }
    }

    public partial class WInsertEdgeSpecification : WInsertSpecification
    {
        public WSelectInsertSource SelectInsertSource { get; set; }
        public WColumnReferenceExpression EdgeColumn { get; set; }

        public WInsertEdgeSpecification(WInsertSpecification insertSpec)
        {
            SelectInsertSource = insertSpec.InsertSource as WSelectInsertSource;
            if (SelectInsertSource == null)
            {
                throw new SyntaxErrorException("The insert source of the INSERT EDGE statement must be a SELECT statement.");
            }

            InsertOption = insertSpec.InsertOption;
            FirstTokenIndex = insertSpec.FirstTokenIndex;
            LastTokenIndex = insertSpec.LastTokenIndex;
            TopRowFilter = insertSpec.TopRowFilter;
            Target = insertSpec.Target;
            Columns = new List<WColumnReferenceExpression>();
            foreach (var col in insertSpec.Columns)
                Columns.Add(col);
        }

        public override void Accept(WSqlFragmentVisitor visitor)
        {
            if (visitor != null)
                visitor.Visit(this);
        }

        public override void AcceptChildren(WSqlFragmentVisitor visitor)
        {
            if (SelectInsertSource != null)
                SelectInsertSource.Accept(visitor);
            base.AcceptChildren(visitor);
        }

        internal override string ToString(string indent)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("{0}INSERT EDGE INTO {1}.{2}\r\n", indent, Target.ToString(), EdgeColumn.ToString());
            sb.Append(SelectInsertSource.ToString(indent));
            return sb.ToString();
        }

        public override string ToDocDbScript(string endpointUrl, string authorizationKey, string databaseId, string collectionId)
        {
            var attachPredicateVisitor = new AttachWhereClauseVisitor();
            var columnTableMapping = _context.GetColumnToAliasMapping(_graphMetaData.ColumnsOfNodeTables);
            attachPredicateVisitor.Invoke(whereClause, graph, columnTableMapping);

            return base.ToDocDbScript(endpointUrl, authorizationKey, databaseId, collectionId);
        }
    }

    public partial class WDeleteSpecification : WUpdateDeleteSpecificationBase
    {
        public WDeleteSpecification()
        {
            WhereClause = new WWhereClause();
        }

        internal override bool OneLine()
        {
            return false;
        }

        internal override string ToString(string indent)
        {
            var sb = new StringBuilder(128);

            sb.AppendFormat("{0}DELETE", indent);
            if (TopRowFilter != null)
            {
                if (TopRowFilter.OneLine())
                {
                    sb.Append(TopRowFilter.ToString(""));
                }
                else
                {
                    sb.Append("\r\n");
                    sb.Append(TopRowFilter.ToString(indent + "  "));
                }
            }

            sb.AppendFormat(" FROM {0}", Target.ToString());

            if (FromClause != null)
            {
                sb.Append("\r\n");
                sb.Append(FromClause.ToString(indent));
            }

            if (WhereClause != null && WhereClause.SearchCondition != null)
            {
                sb.Append("\r\n");
                sb.Append(WhereClause.ToString(indent));
            }

            return sb.ToString();
        }

        public override void Accept(WSqlFragmentVisitor visitor)
        {
            if (visitor != null)
                visitor.Visit(this);
        }
    }

    public partial class WDeleteNodeSpecification : WDeleteSpecification
    {
        public WDeleteNodeSpecification(WDeleteSpecification deleteSpec)
        {
            FirstTokenIndex = deleteSpec.FirstTokenIndex;
            LastTokenIndex = deleteSpec.LastTokenIndex;
            TopRowFilter = deleteSpec.TopRowFilter;
            Target = deleteSpec.Target;
            WhereClause = deleteSpec.WhereClause;
        }

        public override void Accept(WSqlFragmentVisitor visitor)
        {
            if (visitor != null)
                visitor.Visit(this);
        }

        internal override bool OneLine()
        {
            return WhereClause == null;
        }

        internal override string ToString(string indent)
        {
            StringBuilder sb = new StringBuilder(128);

            sb.AppendFormat("{0}DELETE NODE", indent);
            if (TopRowFilter != null)
            {
                if (TopRowFilter.OneLine())
                {
                    sb.Append(TopRowFilter.ToString(""));
                }
                else
                {
                    sb.Append("\r\n");
                    sb.Append(TopRowFilter.ToString(indent + "  "));
                }
            }
            sb.AppendFormat(" FROM {0}", Target.ToString());

            if (FromClause != null)
            {
                sb.Append("\r\n");
                sb.Append(FromClause.ToString(indent));
            }

            if (WhereClause != null && WhereClause.SearchCondition != null)
            {
                sb.Append("\r\n");
                sb.Append(WhereClause.ToString(indent));
            }

            return sb.ToString();
        }
    }

    public partial class WDeleteEdgeSpecification : WDeleteSpecification
    {
        public WSelectQueryBlock SelectDeleteExpr { get; set; }
        public WEdgeColumnReferenceExpression EdgeColumn { get; set; } 
        public WDeleteEdgeSpecification(WSelectQueryBlock deleteSpec)
        {
            SelectDeleteExpr = deleteSpec;
            //FromClause = new WFromClause
            //{
            //    TableReferences = new List<WTableReference>
            //    {
            //        new WQueryDerivedTable
            //        {
            //            QueryExpr = deleteSpec
            //        }
            //    }
            //};
            EdgeColumn = deleteSpec.MatchClause.Paths[0].PathEdgeList[0].Item2;
        }

        public override void Accept(WSqlFragmentVisitor visitor)
        {
            if (visitor != null)
                visitor.Visit(this);
        }

        public override void AcceptChildren(WSqlFragmentVisitor visitor)
        {

            if (EdgeColumn != null)
                EdgeColumn.Accept(visitor);
            if (SelectDeleteExpr != null)
                SelectDeleteExpr.Accept(visitor);
            base.AcceptChildren(visitor);
        }

        internal override bool OneLine()
        {
            return false;
        }

        internal override string ToString(string indent)
        {
            StringBuilder sb = new StringBuilder(128);

            WSelectElement sourceElement = SelectDeleteExpr.SelectElements[0];
            WSelectElement sinkElement = SelectDeleteExpr.SelectElements[1];
            sb.AppendFormat("{0}DELETE EDGE {1}-{2}->{3}",
                indent,
                sourceElement.ToString(""),
                EdgeColumn.ToString(""),
                sinkElement.ToString(""));

            sb.Append("\r\n");
            sb.Append(SelectDeleteExpr.FromClause.ToString(indent));

            // For the DELETE EDGE statement, the first path in the parsed MATCH clause is 
            // the one-hop path, i.e., the edge, to be deleted. 
            if (SelectDeleteExpr.MatchClause.Paths.Count > 1)
            {
                sb.Append("\r\n");
                sb.AppendFormat("{0}MATCH {1}", indent, SelectDeleteExpr.MatchClause.Paths[1].ToString(""));

                for (int i = 2; i < SelectDeleteExpr.MatchClause.Paths.Count; i++)
                {
                    sb.Append("\r\n");
                    sb.AppendFormat("  {0}{1}", indent, SelectDeleteExpr.MatchClause.Paths[i].ToString(""));
                }
            }

            if (SelectDeleteExpr.WhereClause != null && SelectDeleteExpr.WhereClause.SearchCondition != null)
            {
                sb.Append("\r\n");
                sb.Append(SelectDeleteExpr.WhereClause.ToString(indent));
            }

            return sb.ToString();
        }
    }

    public partial class WUpdateSpecification : WUpdateDeleteSpecificationBase
    {
        public IList<WSetClause> SetClauses { get; set; }

        public WUpdateSpecification()
        {
            WhereClause = new WWhereClause();
        }

        internal override bool OneLine()
        {
            return false;
        }

        internal override string ToString(string indent)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("{0}UPDATE ", indent);
            if (TopRowFilter != null)
                sb.Append(TopRowFilter.ToString(indent));

            sb.AppendFormat(" {0} SET \r\n", Target.ToString(indent));
            
            var first = true;
            if (SetClauses != null)
            {
                foreach (var setclause in SetClauses)
                {
                    if (first)
                    {
                        first = false;
                        sb.AppendFormat("{0}{1}", indent, setclause.ToString(indent));
                    }
                    else
                    {
                        sb.AppendFormat(", {0}", setclause.ToString(indent));
                    }
                }
            }

            if (FromClause != null)
                sb.AppendFormat("\r\n{0}{1}", indent, FromClause.ToString(indent));
            if (WhereClause != null && WhereClause.SearchCondition!=null) 
                sb.AppendFormat("\r\n{0}{1}", indent, WhereClause.ToString(indent));
            return sb.ToString();
        }

        public override void Accept(WSqlFragmentVisitor visitor)
        {
            if (visitor != null)
                visitor.Visit(this);
        }

        public override void AcceptChildren(WSqlFragmentVisitor visitor)
        {
            if (SetClauses != null)
            {
                var index = 0;
                for (var count = SetClauses.Count; index < count; ++index)
                    SetClauses[index].Accept(visitor);
            }

            base.AcceptChildren(visitor);
        }
    }

}