﻿using YAF.Lucene.Net.Index;
using YAF.Lucene.Net.Queries.Function.DocValues;
using YAF.Lucene.Net.Search;
using YAF.Lucene.Net.Util;
using YAF.Lucene.Net.Util.Mutable;
using System.Collections;

namespace YAF.Lucene.Net.Queries.Function.ValueSources
{
    /*
     * Licensed to the Apache Software Foundation (ASF) under one or more
     * contributor license agreements.  See the NOTICE file distributed with
     * this work for additional information regarding copyright ownership.
     * The ASF licenses this file to You under the Apache License, Version 2.0
     * (the "License"); you may not use this file except in compliance with
     * the License.  You may obtain a copy of the License at
     *
     *     http://www.apache.org/licenses/LICENSE-2.0
     *
     * Unless required by applicable law or agreed to in writing, software
     * distributed under the License is distributed on an "AS IS" BASIS,
     * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
     * See the License for the specific language governing permissions and
     * limitations under the License.
     */

    /// <summary>
    /// Obtains <see cref="double"/> field values from <see cref="IFieldCache.GetDoubles(AtomicReader, string, FieldCache.IDoubleParser, bool)"/> and makes
    /// those values available as other numeric types, casting as needed.
    /// </summary>
    public class DoubleFieldSource : FieldCacheSource
    {
        protected readonly FieldCache.IDoubleParser m_parser;

        public DoubleFieldSource(string field)
            : this(field, null)
        {
        }

        public DoubleFieldSource(string field, FieldCache.IDoubleParser parser)
            : base(field)
        {
            this.m_parser = parser;
        }

        public override string GetDescription()
        {
            return "double(" + m_field + ')';
        }

        public override FunctionValues GetValues(IDictionary context, AtomicReaderContext readerContext)
        {
            var arr = m_cache.GetDoubles(readerContext.AtomicReader, m_field, m_parser, true);
            var valid = m_cache.GetDocsWithField(readerContext.AtomicReader, m_field);
            return new DoubleDocValuesAnonymousInnerClassHelper(this, arr, valid);
        }

        private class DoubleDocValuesAnonymousInnerClassHelper : DoubleDocValues
        {
            private readonly FieldCache.Doubles arr;
            private readonly IBits valid;

            public DoubleDocValuesAnonymousInnerClassHelper(DoubleFieldSource @this, FieldCache.Doubles arr, IBits valid)
                : base(@this)
            {
                this.arr = arr;
                this.valid = valid;
            }

            public override double DoubleVal(int doc)
            {
                return arr.Get(doc);
            }

            public override bool Exists(int doc)
            {
                return arr.Get(doc) != 0 || valid.Get(doc);
            }

            public override ValueFiller GetValueFiller()
            {
                return new ValueFillerAnonymousInnerClassHelper(this);
            }

            private class ValueFillerAnonymousInnerClassHelper : ValueFiller
            {
                private readonly DoubleDocValuesAnonymousInnerClassHelper outerInstance;

                public ValueFillerAnonymousInnerClassHelper(DoubleDocValuesAnonymousInnerClassHelper outerInstance)
                {
                    this.outerInstance = outerInstance;
                    mval = new MutableValueDouble();
                }

                private readonly MutableValueDouble mval;

                public override MutableValue Value
                {
                    get
                    {
                        return mval;
                    }
                }

                public override void FillValue(int doc)
                {
                    mval.Value = outerInstance.arr.Get(doc);
                    mval.Exists = mval.Value != 0 || outerInstance.valid.Get(doc);
                }
            }
        }

        public override bool Equals(object o)
        {
            var other = o as
                DoubleFieldSource;
            if (other == null)
            {
                return false;
            }
            return base.Equals(other) &&
                   (this.m_parser == null ? other.m_parser == null : this.m_parser.GetType() == other.m_parser.GetType());
        }

        public override int GetHashCode()
        {
            int h = m_parser == null ? typeof(double?).GetHashCode() : m_parser.GetType().GetHashCode();
            h += base.GetHashCode();
            return h;
        }
    }
}