using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using GitHub.Helpers;

namespace GitHub.Collections
{
    public interface ITrackingCollection<T> : IDisposable, 
        // ObservableCollection<T> interfaces
        IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable, IList, ICollection, IReadOnlyList<T>, IReadOnlyCollection<T>
        where T : ICopyable<T>
    {
        ITrackingCollection<T> Listen(IObservable<T> obs);
        IDisposable Subscribe();
        IDisposable Subscribe(Action onCompleted);
        void SetComparer(Func<T, T, int> comparer);
        void SetFilter(Func<T, int, IList<T>, bool> filter);
        event NotifyCollectionChangedEventHandler CollectionChanged;
    }
}