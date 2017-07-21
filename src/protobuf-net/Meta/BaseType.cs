using System;
using System.Reflection;
using ProtoBuf.Serializers;

namespace ProtoBuf.Meta
{
    public class BaseType
    {
        private IProtoSerializer _serializer;
        private readonly MetaType _derivedType;
        private readonly DataFormat _dataFormat;

        public int FieldNumber { get; }

        public Type Type { get; set; }

        public Type DeclaringType { get; set; }

        internal IProtoSerializer Serializer
        {
            get
            {
                if (_serializer == null) _serializer = BuildSerializer();
                return _serializer;
            }
        }

        public BaseType(int fieldNumber, MetaType derivedType, DataFormat format)
        {
            FieldNumber = fieldNumber;
            _derivedType = derivedType;
            _dataFormat = format;
        }

        private IProtoSerializer BuildSerializer()
        {
            // note the caller here is MetaType.BuildSerializer, which already has the sync-lock
            var wireType = WireType.String;
            if (_dataFormat == DataFormat.Group) wireType = WireType.StartGroup; // only one exception

            var baseClassType =
#if COREFX
                _derivedType.Type.GetTypeInfo().BaseType;
#else
                _derivedType.Type.BaseType;
#endif
            var key = _derivedType.Model.GetKey(ref baseClassType);

            var ser = new BaseTypeSerializer(baseClassType, _derivedType.Type, key, _derivedType, false);
            var tagDecorator = new TagDecorator(FieldNumber, wireType, false, ser);
            return tagDecorator;
        }

    }
}
