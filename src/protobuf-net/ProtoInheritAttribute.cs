using System;

namespace ProtoBuf
{
	/// <summary>
	/// ProtoInheritAttribute
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class ProtoInheritAttribute : Attribute
	{
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
		public ProtoInheritAttribute(int tag)
		{
			Tag = tag;
		}

		/// <summary>
		/// TagNumber
		/// </summary>
		public int Tag { get; set; }
	}
}
