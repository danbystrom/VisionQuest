using System;
using System.Collections.Generic;
using System.Text;
using Photomic.Common;

namespace Plata
{
	public class ProtectedIdExtra : enumBase<PersonSkyddad>
	{
		static ProtectedIdExtra()
		{
            add(PersonSkyddad.EjSkydd, "OK");
            add(PersonSkyddad.HeltNej, "Ej p� bild");
            add(PersonSkyddad.UtanNamn, "Ej namn");
            add(PersonSkyddad.VetEj, "Vet ej");
        }
	}

}
