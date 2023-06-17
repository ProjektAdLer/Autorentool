using System.Collections.Specialized;
using NUnit.Framework;
using Shared;

namespace SharedTest
{
    [TestFixture]
    public class ObservableDictionaryTests
    {
        private ObservableDictionary<string, int> CreateObservableDictionary(
            IDictionary<string, int>? innerDictionary = null)
        {
            return innerDictionary is null
                ? new ObservableDictionary<string, int>()
                : new ObservableDictionary<string, int>(innerDictionary);
        }

        [Test]
        public void GetEnumerator_EmptyDictionary_ReturnsEmptyEnumerator()
        {
            // Arrange
            var dictionary = CreateObservableDictionary();

            // Act
            using var enumerator = dictionary.GetEnumerator();

            // Assert
            Assert.That(enumerator.MoveNext(), Is.False);
        }

        [Test]
        public void Add_AddsKeyValuePairToDictionaryAndRaisesCollectionChangedEvent()
        {
            // Arrange
            var dictionary = CreateObservableDictionary();
            var item = new KeyValuePair<string, int>("key", 10);
            NotifyCollectionChangedEventArgs? eventArgs = null;
            dictionary.CollectionChanged += (_, e) => eventArgs = e;

            // Act
            dictionary.Add(item);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(dictionary, Does.Contain(item));
                Assert.That(eventArgs!.Action, Is.EqualTo(NotifyCollectionChangedAction.Add));
                Assert.That(eventArgs.NewItems, Is.EquivalentTo(new[] { item }));
            });
        }

        [Test]
        public void Clear_RemovesAllItemsFromDictionaryAndRaisesCollectionChangedEvent()
        {
            // Arrange
            var dictionary = CreateObservableDictionary(new Dictionary<string, int>
            {
                { "key1", 10 },
                { "key2", 20 },
                { "key3", 30 }
            });
            NotifyCollectionChangedEventArgs? eventArgs = null;
            dictionary.CollectionChanged += (_, e) => eventArgs = e;

            // Act
            dictionary.Clear();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(dictionary, Is.Empty);
                Assert.That(eventArgs!.Action, Is.EqualTo(NotifyCollectionChangedAction.Reset));
            });
        }

        [Test]
        public void Remove_RemovesKeyValuePairFromDictionaryAndRaisesCollectionChangedEvent()
        {
            // Arrange
            var dictionary = CreateObservableDictionary(new Dictionary<string, int>
            {
                { "key", 10 }
            });
            var item = new KeyValuePair<string, int>("key", 10);
            NotifyCollectionChangedEventArgs? eventArgs = null;
            dictionary.CollectionChanged += (_, e) => eventArgs = e;

            // Act
            var result = dictionary.Remove(item);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.True);
                Assert.That(dictionary, Does.Not.Contain(item));
                Assert.That(eventArgs!.Action, Is.EqualTo(NotifyCollectionChangedAction.Remove));
                Assert.That(eventArgs.OldItems, Is.EquivalentTo(new[] { item }));
            });
        }

        [Test]
        public void CopyTo_CopiesDictionaryToTargetArray()
        {
            // Arrange
            var dictionary = CreateObservableDictionary(new Dictionary<string, int>
            {
                { "key1", 10 },
                { "key2", 20 },
                { "key3", 30 }
            });
            var array = new KeyValuePair<string, int>[3];

            // Act
            dictionary.CopyTo(array, 0);

            // Assert
            Assert.That(array, Is.EquivalentTo(dictionary));
        }

        [Test]
        public void Add_KeyValuePair_AddsKeyValuePairToDictionaryAndRaisesCollectionChangedEvent()
        {
            // Arrange
            var dictionary = CreateObservableDictionary();
            var key = "key";
            var value = 10;
            NotifyCollectionChangedEventArgs? eventArgs = null;
            dictionary.CollectionChanged += (_, e) => eventArgs = e;

            // Act
            dictionary.Add(key, value);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(dictionary.ContainsKey(key), Is.True);
                Assert.That(dictionary[key], Is.EqualTo(value));
                Assert.That(eventArgs!.Action, Is.EqualTo(NotifyCollectionChangedAction.Add));
                Assert.That(eventArgs.NewItems, Is.EquivalentTo(new[] { new KeyValuePair<string, int>(key, value) }));
            });
        }

        [Test]
        public void ContainsKey_KeyExists_ReturnsTrue()
        {
            // Arrange
            var dictionary = CreateObservableDictionary(new Dictionary<string, int>
            {
                { "key", 10 }
            });

            // Act
            var result = dictionary.ContainsKey("key");

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void Remove_KeyExists_RemovesKeyValuePairFromDictionaryAndRaisesCollectionChangedEvent()
        {
            // Arrange
            var dictionary = CreateObservableDictionary(new Dictionary<string, int>
            {
                { "key", 10 }
            });
            var key = "key";
            NotifyCollectionChangedEventArgs? eventArgs = null;
            dictionary.CollectionChanged += (_, e) => eventArgs = e;

            // Act
            var result = dictionary.Remove(key);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.True);
                Assert.That(dictionary.ContainsKey(key), Is.False);
                Assert.That(eventArgs!.Action, Is.EqualTo(NotifyCollectionChangedAction.Remove));
                Assert.That(eventArgs.OldItems, Is.EquivalentTo(new[] { new KeyValuePair<string, int>(key, 10) }));
            });
        }

        [Test]
        public void Remove_KeyDoesNotExist_ReturnsFalseAndDoesNotModifyDictionary()
        {
            // Arrange
            var dictionary = CreateObservableDictionary(new Dictionary<string, int>
            {
                { "key", 10 }
            });
            var key = "notkey";
            NotifyCollectionChangedEventArgs? eventArgs = null;
            dictionary.CollectionChanged += (_, e) => eventArgs = e;

            // Act
            var result = dictionary.Remove(key);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.False);
                Assert.That(dictionary.ContainsKey("key"), Is.True);
                Assert.That(dictionary["key"], Is.EqualTo(10));
                Assert.That(eventArgs, Is.Null);
            });
        }

        [Test]
        public void TryGetValue_KeyExists_ReturnsTrueAndSetsValue()
        {
            // Arrange
            var dictionary = CreateObservableDictionary(new Dictionary<string, int>
            {
                { "key", 10 }
            });
            var key = "key";

            // Act
            var result = dictionary.TryGetValue(key, out var value);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.True);
                Assert.That(value, Is.EqualTo(10));
            });
        }

        [Test]
        public void TryGetValue_KeyDoesNotExist_ReturnsFalseAndSetsDefaultValue()
        {
            // Arrange
            var dictionary = CreateObservableDictionary();
            var key = "key";

            // Act
            var result = dictionary.TryGetValue(key, out var value);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.False);
                Assert.That(value, Is.EqualTo(default(int)));
            });
        }

        [Test]
        public void Indexer_GetExistingKey_ReturnsValue()
        {
            // Arrange
            var dictionary = CreateObservableDictionary(new Dictionary<string, int>
            {
                { "key", 10 }
            });

            // Act
            var value = dictionary["key"];

            // Assert
            Assert.That(value, Is.EqualTo(10));
        }

        [Test]
        public void Indexer_SetExistingKey_ReplacesValueAndRaisesCollectionChangedEvent()
        {
            // Arrange
            var dictionary = CreateObservableDictionary(new Dictionary<string, int>
            {
                { "key", 10 }
            });
            var key = "key";
            var value = 20;
            NotifyCollectionChangedEventArgs? eventArgs = null;
            dictionary.CollectionChanged += (_, e) => eventArgs = e;

            // Act
            dictionary[key] = value;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(dictionary[key], Is.EqualTo(value));
                Assert.That(eventArgs!.Action, Is.EqualTo(NotifyCollectionChangedAction.Replace));
                Assert.That(eventArgs.NewItems, Is.EquivalentTo(new[] { new KeyValuePair<string, int>(key, value) }));
                Assert.That(eventArgs.OldItems, Is.EquivalentTo(new[] { new KeyValuePair<string, int>(key, 10) }));
            });
        }

        [Test]
        public void Indexer_SetNewKey_AddsKeyValuePairAndRaisesCollectionChangedEvent()
        {
            // Arrange
            var dictionary = CreateObservableDictionary();
            var key = "key";
            var value = 10;
            NotifyCollectionChangedEventArgs? eventArgs = null;
            dictionary.CollectionChanged += (_, e) => eventArgs = e;

            // Act
            dictionary[key] = value;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(dictionary.ContainsKey(key), Is.True);
                Assert.That(dictionary[key], Is.EqualTo(value));
                Assert.That(eventArgs!.Action, Is.EqualTo(NotifyCollectionChangedAction.Add));
                Assert.That(eventArgs.NewItems, Is.EquivalentTo(new[] { new KeyValuePair<string, int>(key, value) }));
            });
        }

        [Test]
        public void Keys_ReturnsDictionaryKeys()
        {
            // Arrange
            var dictionary = CreateObservableDictionary(new Dictionary<string, int>
            {
                { "key1", 10 },
                { "key2", 20 },
                { "key3", 30 }
            });

            // Act
            var keys = dictionary.Keys;

            // Assert
            Assert.That(keys, Is.EquivalentTo(new[] { "key1", "key2", "key3" }));
        }

        [Test]
        public void Values_ReturnsDictionaryValues()
        {
            // Arrange
            var dictionary = CreateObservableDictionary(new Dictionary<string, int>
            {
                { "key1", 10 },
                { "key2", 20 },
                { "key3", 30 }
            });

            // Act
            var values = dictionary.Values;

            // Assert
            Assert.That(values, Is.EquivalentTo(new[] { 10, 20, 30 }));
        }
    }
}