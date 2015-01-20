using System;
using System.Collections;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Reflection;

namespace Plata
{
	public class enumBase<T> : enumBaseTag<T,int> where T : struct, IComparable
	{
		protected static void add( T t, string strText )
		{
			add( t, strText, 0 );
		}

		protected static void initFromDescriptionAttributes()
		{
			foreach ( T t in Enum.GetValues( typeof(T) ) )
			{
				FieldInfo fi = typeof( Enum ).GetField( t.ToString() );
				DescriptionAttribute[] attrs =
							(DescriptionAttribute[])fi.GetCustomAttributes(
							typeof( DescriptionAttribute ), false );
				add( t, attrs.Length>0 ? attrs[0].Description : t.ToString() );
			}
		}

	}

	public class enumBaseTag<TEnum,TTag> where TEnum : struct, IComparable
	{
		public enum Sorting
		{
			AlphaNullFirst,
			Alpha,
			AddedOrder,
			Enum
		}

		public static readonly TEnum Null;
		public static Sorting SortOrder = Sorting.AlphaNullFirst;

		private static Hashtable s_hash = new Hashtable();

		protected static void add( TEnum t, string strText, TTag tag )
		{
			s_hash.Add( t, new Info( t, tag, strText, s_hash.Count ) );
		}

		public static Info getInfo( TEnum t )
		{
			return (Info)s_hash[t];
		}

		public static string Text( TEnum t )
		{
			Info info = getInfo( t );
			return info != null ? info.Text : "???";
		}

		public static TEnum Value( Info info )
		{
			return info != null ? info.Value : Null;
		}

		public static TEnum Value( object obj )
		{
			if ( obj is System.Windows.Forms.ComboBox )
				obj = (obj as System.Windows.Forms.ComboBox).SelectedItem; 
			return Value( obj as Info );
		}

		public static ICollection AllInfos
		{
			get { return s_hash.Values; }
		}

		public static ICollection AllInfosSorted()
		{
			return AllInfosSorted( true );
		}

		public static ICollection AllInfosSorted( bool fIncludeNull )
		{
			ArrayList al = new ArrayList( s_hash.Values );
			if ( !fIncludeNull )
				al.Remove( getInfo(Null) );
			al.Sort();
			return al;
		}

		public static void fillComboBox(
			ComboBox cbo,
			bool fIncludeNull,
			TEnum selected )
		{
			cbo.Items.Clear();
			foreach ( Info info in AllInfosSorted() )
				if ( fIncludeNull || !info.Value.Equals( Null ) )
					cbo.Items.Add( info );
			cbo.SelectedItem = getInfo( selected );
		}

		public class Info : IComparable
		{
			public readonly TEnum Value;
			public readonly TTag Tag;
			public readonly string Text;
			public readonly int Ordinal;

			public Info( TEnum t, TTag tag, string strText, int nOrdinal )
			{
				Value = t;
				Tag = tag;
				Text = strText;
				Ordinal = nOrdinal;
			}

			public override string ToString()
			{
				return Text;
			}

			int IComparable.CompareTo( object obj )
			{
				if ( obj is TEnum )
					return Value.CompareTo( obj );
				Info that = obj as Info;

				switch ( SortOrder )
				{
					case Sorting.AlphaNullFirst:
						if ( Value.Equals( Null ) || that.Value.Equals( Null ) )
							goto case Sorting.Enum;
						else
							goto case Sorting.Alpha;
					case Sorting.Alpha:
						return Text.CompareTo( that.Text );
					case Sorting.AddedOrder:
						return Ordinal.CompareTo( that.Ordinal );
					case Sorting.Enum:
						return Value.CompareTo( that.Value );
				}
				return 0;
			}

		}
	}

}
