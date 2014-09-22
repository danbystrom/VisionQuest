using System;
using System.Collections.Generic;
using System.Text;

namespace Plata.Notes
{
	class Notes : PlataDM.IvdPersistableCollection
	{
		private List<Note> _notes = new List<Note>();

		public bool hasRegardingDate( DateTime date )
		{
			return _notes.Exists( delegate( Note note ) { return note.RegardingDate.Date == date.Date; } );
		}

		public Note getNoteByID( int nID )
		{
			foreach ( Note not in _notes )
				if ( not.UniqueSessionID == nID )
					return not;
			return null;
		}

		public System.Collections.ObjectModel.ReadOnlyCollection<Note> AllNotes
		{
			get { return _notes.AsReadOnly(); }
		}

		public void addNote( Note note )
		{
			int nUniqueSessionID = 1;
			foreach ( Note not in _notes )
				nUniqueSessionID = Math.Max( nUniqueSessionID, not.UniqueSessionID + 1 );
			note.UniqueSessionID = nUniqueSessionID;
			_notes.Add( note );
		}

		public void removeNote( Note note )
		{
			_notes.Remove( note );
		}

		public List<Note> GetFilteredNotes(
			DateTime dateFirst,
			DateTime dateLast,
			bool IncludeNotesWithoutDate,
			IList<int> OrderNumbers )
		{
			bool fUseOrderFilter = OrderNumbers != null && OrderNumbers.Count != 0;
			List<Note> list = new List<Note>();
			foreach ( Note note in _notes )
				if (
						((IncludeNotesWithoutDate && note.RegardingDate==DateTime.MinValue) ||
							(dateFirst <= note.RegardingDate && dateLast >= note.RegardingDate) ) &&
						(!fUseOrderFilter || OrderNumbers.Contains( note.OrderNumber )) )
					list.Add( note );
			list.Sort();
			return list;
		}

		PlataDM.IvdPersistable PlataDM.IvdPersistableCollection.ConstructNewItem()
		{
			return new Note();
		}

		void PlataDM.IvdPersistableCollection.StoreItem( PlataDM.IvdPersistable item )
		{
			addNote( (Note)item );
		}

		System.Collections.IEnumerator PlataDM.IvdPersistableCollection.GetEnumerator()
		{
			return _notes.GetEnumerator();
		}

	}

}
