using System;
using System.Collections;
using System.Text;
using System.Windows.Forms;

namespace Plata
{
	public class ListBoxItem : IComparable
	{
		public string Text;
		public object Tag;
		public ListBoxItem(string Text, object Tag)
		{
			this.Text = Text != null ? Text : string.Empty;
			this.Tag = Tag;
		}

		public override string ToString()
		{
			return Text;
		}

		public static object selectedTag(ListBox lst)
		{
			ListBoxItem lbi = lst.SelectedItem as ListBoxItem;
			return lbi != null ? lbi.Tag : null;
		}

		public static object selectedTag(vdUsr.vdComboBox cbo)
		{
			ListBoxItem lbi = cbo.SelectedItem as ListBoxItem;
			return cbo != null ? lbi.Tag : null;
		}

		public static void select(ListBox lst, object Tag)
		{
			lst.SelectedItem = find(lst.Items, Tag);
		}

		public static void select(vdUsr.vdComboBox cbo, object Tag)
		{
			cbo.SelectedItem = find(cbo.Items, Tag);
		}

		private static object find(IEnumerable ie, object Tag)
		{
			foreach (object obj in ie)
			{
				ListBoxItem lbi = obj as ListBoxItem;
				if (lbi != null)
				{
					if (lbi.Tag == null && Tag == null)
						return lbi;
					if (lbi.Tag != null && lbi.Tag.Equals(Tag))
						return lbi;
				}
			}
			return null;
		}

		public static ListBoxItem add(IList list, string strText, object Tag)
		{
			ListBoxItem lbi = new ListBoxItem(strText, Tag);
			list.Add(lbi);
			return lbi;
		}

		public static ListBoxItem add(ComboBox cbo, string strText, object Tag)
		{
			return add(cbo.Items, strText, Tag);
		}

		public static ListBoxItem add(ListBox lst, string strText, object Tag)
		{
			return add(lst.Items, strText, Tag);
		}

		public int CompareTo(object obj)
		{
			ListBoxItem that = obj as ListBoxItem;
			return Text.CompareTo(that.Text);
		}

		public override bool Equals(object obj)
		{
			ListBoxItem that = obj as ListBoxItem;
			if (that == null)
				return false;
			return Text.Equals(that.Text);
		}

		public override int GetHashCode()
		{
			return Text.GetHashCode();
		}

	}

	public class ListBoxItemF : ListBoxItem
	{
		public ListBoxItemF(object Tag, string format, params object[] args)
			: base(string.Format(format, args), Tag)
		{
		}
	}

	public class ListBoxHelpers
	{
		public static void setCBItems(vdUsr.vdComboBox cbo, ICollection col, Type converter, object selected, object objDefault)
		{
			System.Reflection.ConstructorInfo constructorInfoObj = converter.GetConstructor(new Type[] { typeof(object) });
			object obj2Select = null;

			cbo.Items.Clear();
			if (objDefault != null)
				cbo.Items.Add(objDefault);
			foreach (object obj in col)
			{
				object obj2stuff = constructorInfoObj.Invoke(new object[] { obj });
				cbo.Items.Add(obj2stuff);
				if (obj == selected)
					obj2Select = obj2stuff;
			}
			if (obj2Select != null)
				cbo.SelectedItem = obj2Select;
			else if (objDefault != null)
				cbo.SelectedItem = objDefault;
		}

		public static void setCBItems(vdUsr.vdComboBox cbo, ICollection col1, ICollection col2, Type converter, object selected, object objDefault)
		{
			ArrayList al = new ArrayList(col1.Count + col2.Count);
			al.AddRange(col1);
			al.AddRange(col2);
			setCBItems(cbo, al, converter, selected, objDefault);
		}

		public static void loadCBSimple(vdUsr.vdComboBox cbo, IEnumerable list, object selected)
		{
			ArrayList al = new ArrayList();
			foreach (object obj in list)
				al.Add(obj);
			al.Sort();
			cbo.Items.Clear();
			foreach (object obj in al)
				cbo.Items.Add(obj);
			if (selected != null)
				cbo.SelectedItem = selected;
		}

		public static void selectTag(vdUsr.vdComboBox cbo, object tag)
		{
			foreach (ListBoxItem lbi in cbo.Items)
				if (lbi.Tag.Equals(tag))
				{
					cbo.SelectedItem = lbi;
					return;
				}
		}

		public static void selectText(vdUsr.vdComboBox cbo, string strText)
		{
			foreach (object obj in cbo.Items)
				if (obj.ToString().Equals(strText))
				{
					cbo.SelectedItem = obj;
					return;
				}
		}

	}


}
