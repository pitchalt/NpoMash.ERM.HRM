using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntecoAG.XafExt.DataStruct.IndexedList {

    public interface IIndexValue<Tv, Ti>: IEnumerable<Tv> {
        IIndex<Tv, Ti> Index { get; }
        Ti Key { get; }

        Tv Value { get; }

        Tv this[int index] { get; }
    }

    public interface IIndex<Tv, Ti>: IEnumerable<Ti> {
        IIndexable<Tv> Source { get; }
        
        IIndexValue<Tv, Ti> this[Ti index] { get; }
    }

    public interface IIndexable<Tv>: IEnumerable<Tv> {
        Tv this[int index] { get; }
    }

    public class IndexValue<Tv, Ti> : List<Tv>, IIndexValue<Tv, Ti>, IIndexable<Tv> {
        private IIndex<Tv, Ti> _Index;
        public IIndex<Tv, Ti> Index {
            get { return _Index; }
            set { _Index = value; }
        }

        private Ti _Key;
        public Ti Key {
            get { return _Key; }
            set { _Key = value; }
        }

        private Tv _Value;
        public Tv Value {
            get { return _Value; }
            set { _Value = value; }
        }
    }

    class IndexEnumerator<Tv, Ti> : IEnumerator<Ti> {

        private IIndex<Tv, Ti> _Index;
        private IEnumerator<Ti> _Enumerator;
 
        public Ti Current {
            get { return _Enumerator.Current; }
        }

        public IndexEnumerator(Index<Tv, Ti> index) {
            _Index = index;
            _Enumerator = index.Keys.GetEnumerator();
        }

        public void Dispose() {
            _Enumerator.Dispose();
            _Enumerator = null;
            _Index = null;
        }

        object IEnumerator.Current {
            get {
                return _Enumerator.Current;
            }
        }

        public bool MoveNext() {
            return _Enumerator.MoveNext();
        }

        public void Reset() {
            _Enumerator.Reset();
        }

    }

    class Index<Tv, Ti> : Dictionary<Ti, IndexValue<Tv, Ti>>, IIndex<Tv, Ti> {

        IIndexable<Tv> _Source;
        public IIndexable<Tv> Source {
            get { return _Source; }
        }

        public new IEnumerator<Ti> GetEnumerator() {
            return new IndexEnumerator<Tv, Ti>(this);
        }

        //IEnumerator GetEnumerator() {
        //    return new IndexEnumerator<Tv, Ti>(this);
        //}
    
        IIndexValue<Tv,Ti>  IIndex<Tv, Ti>.this[Ti index] {
	        get { return this[index]; }
        }

        public Index(IIndexable<Tv> source) {
            _Source = source;
        }

    }

    public static class IndexableList { 

        public static IIndex<Tv, Ti> CreateIndex<Tv, Ti>(IIndexable<Tv> list, Func<Tv, Ti> func_index, Func<Tv, Tv, Tv> func_reduce) {
            Index<Tv, Ti> index = new Index<Tv,Ti>(list);
            foreach (Tv value in list) {
                Ti index_value = func_index(value);
                if (index.ContainsKey(index_value))
                    index[index_value].Value = func_reduce(index[index_value].Value, value);
                else 
                    index[index_value].Value = func_reduce(default(Tv), value);
            }
            return index;
        }

    }
}
