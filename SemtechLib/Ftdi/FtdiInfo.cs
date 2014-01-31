using System;
using System.Runtime.InteropServices;

namespace SemtechLib.Ftdi
{
	[StructLayout(LayoutKind.Sequential)]
	public struct FtdiInfo
	{
		private uint deviceIndex;
		private uint flags;
		private string type;
		private uint id;
		private uint locId;
		private string serialNumber;
		private string description;
		public uint DeviceIndex
		{
			get { return this.deviceIndex; }
			set { this.deviceIndex = value; }
		}
		public uint Flags
		{
			get { return this.flags; }
			set { this.flags = value; }
		}
		public string Type
		{
			get { return this.type; }
			set { this.type = value; }
		}
		public uint Id
		{
			get { return this.id; }
			set { this.id = value; }
		}
		public uint LocId
		{
			get { return this.locId; }
			set { this.locId = value; }
		}
		public string SerialNumber
		{
			get { return this.serialNumber; }
			set { this.serialNumber = value; }
		}
		public string Description
		{
			get { return this.description; }
			set { this.description = value; }
		}
	}
}