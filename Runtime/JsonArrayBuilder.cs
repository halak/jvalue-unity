﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace Halak
{
    public struct JsonArrayBuilder : IDisposable
    {
        private readonly JsonWriter writer;
        private readonly int startOffset;

        public JsonArrayBuilder(int capacity) : this(new JsonWriter(capacity)) { }
        public JsonArrayBuilder(StringBuilder stringBuilder) : this(new JsonWriter(stringBuilder)) { }
        public JsonArrayBuilder(TextWriter writer) : this(new JsonWriter(writer)) { }
        internal JsonArrayBuilder(JsonWriter writer)
        {
            this.writer = writer;
            this.writer.WriteStartArray();
            this.startOffset = writer.Offset;
        }

        public void Dispose()
        {
            writer.WriteEndArray();
        }

        public JsonArrayBuilder PushNull()
        {
            EnsureState();
            writer.WriteCommaIf(startOffset);
            writer.WriteNull();
            return this;
        }

        public JsonArrayBuilder Push(bool value)
        {
            EnsureState();
            writer.WriteCommaIf(startOffset);
            writer.Write(value);
            return this;
        }

        public JsonArrayBuilder Push(int value)
        {
            EnsureState();
            writer.WriteCommaIf(startOffset);
            writer.Write(value);
            return this;
        }

        public JsonArrayBuilder Push(long value)
        {
            EnsureState();
            writer.WriteCommaIf(startOffset);
            writer.Write(value);
            return this;
        }

        public JsonArrayBuilder Push(float value)
        {
            EnsureState();
            writer.WriteCommaIf(startOffset);
            writer.Write(value);
            return this;
        }

        public JsonArrayBuilder Push(double value)
        {
            EnsureState();
            writer.WriteCommaIf(startOffset);
            writer.Write(value);
            return this;
        }

        public JsonArrayBuilder Push(decimal value)
        {
            EnsureState();
            writer.WriteCommaIf(startOffset);
            writer.Write(value);
            return this;
        }

        public JsonArrayBuilder Push(string value)
        {
            EnsureState();
            writer.WriteCommaIf(startOffset);
            writer.Write(value);
            return this;
        }

        public JsonArrayBuilder Push(JValue value)
        {
            EnsureState();
            writer.WriteCommaIf(startOffset);
            writer.Write(value);
            return this;
        }

        public JsonArrayBuilder PushArray(Action<JsonArrayBuilder> push)
        {
            EnsureState();
            writer.WriteCommaIf(startOffset);
            var arrayBuilder = new JsonArrayBuilder(writer);
            push(arrayBuilder);
            arrayBuilder.Dispose();
            return this;
        }

        public JsonArrayBuilder PushArray<T>(T value, Action<JsonArrayBuilder, T> push)
        {
            EnsureState();
            writer.WriteCommaIf(startOffset);
            var arrayBuilder = new JsonArrayBuilder(writer);
            push(arrayBuilder, value);
            arrayBuilder.Dispose();
            return this;
        }

        public JsonArrayBuilder PushObject(Action<JsonObjectBuilder> push)
        {
            EnsureState();
            writer.WriteCommaIf(startOffset);
            var objectBuilder = new JsonObjectBuilder(writer);
            push(objectBuilder);
            objectBuilder.Dispose();
            return this;
        }

        public JsonArrayBuilder PushObject<T>(T value, Action<JsonObjectBuilder, T> push)
        {
            EnsureState();
            writer.WriteCommaIf(startOffset);
            var objectBuilder = new JsonObjectBuilder(writer);
            push(objectBuilder, value);
            objectBuilder.Dispose();
            return this;
        }

        public JValue Build()
        {
            Dispose();
            return writer.BuildJson();
        }

        #region Shorthand Methods
        public JsonArrayBuilder PushArray(IEnumerable<string> values)
            => PushArrayOf(values, (arrayBuilder, value) => arrayBuilder.Push(value));

        public JsonArrayBuilder PushObject(IEnumerable<KeyValuePair<string, string>> members)
            => PushObjectOf(members, (objectBuilder, member) => objectBuilder.Put(member.Key, member.Value));

        public JsonArrayBuilder PushArrayOfArray<T>(IEnumerable<T> source, Action<JsonArrayBuilder, T> build)
            => PushArray((source, build), Internal.ArrayOfArray);

        public JsonArrayBuilder PushObjectOfArray<T>(IEnumerable<KeyValuePair<string, T>> source, Action<JsonArrayBuilder, T> build)
            => PushObject((source, build), Internal.ObjectOfArray);

        public JsonArrayBuilder PushArrayOfObject<T>(IEnumerable<T> source, Action<JsonObjectBuilder, T> build)
            => PushArray((source, build), Internal.ArrayOfObject);

        public JsonArrayBuilder PushObjectOfObject<T>(IEnumerable<KeyValuePair<string, T>> source, Action<JsonObjectBuilder, T> build)
            => PushObject((source, build), Internal.ObjectOfObject);

        public JsonArrayBuilder PushArrayOf<T>(IEnumerable<T> source, Func<JsonArrayBuilder, T, JsonArrayBuilder> build)
            => PushArray((source, build), Internal.ArrayOf);

        public JsonArrayBuilder PushObjectOf<T>(IEnumerable<KeyValuePair<string, T>> source, Func<JsonObjectBuilder, KeyValuePair<string, T>, JsonObjectBuilder> build)
            => PushObject((source, build), Internal.ObjectOf);
        #endregion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void EnsureState()
        {
            if (writer == null)
                throw new InvalidOperationException("this object created by default constructor. please use parameterized constructor.");
        }
    }
}
