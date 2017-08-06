using System;
using System.IO;
using Xunit;

namespace ProtoBuf.Issues
{
    public class ProtoInheritTest
    {
        [Fact]
        public void Test1()
        {
            var name = "Hello";
            var id = 9;
            var petId = 67;
            var petName = "Cat";

            var person = new MessageChild
            {
                Id = id,
                Name = name,
                Pet = new Pet
                {
                    PetId = petId,
                    PetName = petName
                }
            };

            using (var ms = new MemoryStream())
            {
                Serializer.Serialize(ms, person);

                ms.Position = 0;
                var dobj = Serializer.Deserialize<MessageChild>(ms);

                Assert.Equal(id, dobj.Id);
                Assert.Equal(name, dobj.Name);
                Assert.Equal(petId, dobj.Pet.PetId);
                Assert.Equal(petName, dobj.Pet.PetName);

                ms.Position = 0;
                var dobj2 = Serializer.Deserialize<MessageChildB>(ms);
                Assert.Equal(id, dobj2.Base.Id);
                Assert.Equal(name, dobj2.Name);
                Assert.Equal(petId, dobj2.Pet.PetId);
                Assert.Equal(petName, dobj2.Pet.PetName);

                ms.Position = 0;
                Serializer.Serialize(ms, dobj);
                Assert.Equal(id, dobj.Id);
                Assert.Equal(name, dobj.Name);
                Assert.Equal(petId, dobj.Pet.PetId);
                Assert.Equal(petName, dobj.Pet.PetName);
            }
        }

        [ProtoContract]
        public class MessageBase
        {
            [ProtoMember(1)]
            public int Id { get; set; }
        }

        [ProtoContract]
        public class Pet
        {
            [ProtoMember(1)]
            public int PetId { get; set; }

            [ProtoMember(2)]
            public string PetName { get; set; }
        }

        [ProtoContract]
        [ProtoInherit(1)]
        public class MessageChild : MessageBase
        {
            [ProtoMember(2)]
            public string Name { get; set; }

            [ProtoMember(3)]
            public Pet Pet { get; set; }
        }

        [ProtoContract]
        public class MessageChildB
        {
            [ProtoMember(1)]
            public MessageBase Base { get; set; }

            [ProtoMember(2)]
            public string Name { get; set; }

            [ProtoMember(3)]
            public Pet Pet { get; set; }
        }

    }
}
