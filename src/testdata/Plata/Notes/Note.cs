using System;
using System.Collections.Generic;
using System.Text;

namespace Plata.Notes
{
	public class Note : PlataDM.IvdPersistable, IComparable<Note>
	{
		public string Text;
		public int OrderNumber;
		public DateTime RegardingDate = DateTime.MinValue;
		public DateTime Created = DateTime.MinValue;
		public DateTime Changed = DateTime.MinValue;

		public int UniqueSessionID;
		public int StartingCharIndex;

		public Note()
		{
		}

		public Note( DateTime dateCreated )
		{
			Created = dateCreated;
		}

		void PlataDM.IvdPersistable.Persist( PlataDM.vdPersist po )
		{
			po.x( "text", ref Text );
			po.x( "ordernumber", ref OrderNumber );
			po.x( "regardingdate", ref RegardingDate );
			po.x( "created", ref Created );
			po.x( "changed", ref Changed );
		}


		public int CompareTo( Note other )
		{
			return RegardingDate.CompareTo( other.RegardingDate );
		}

	}

}
