using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Rentas
{
    public class DatosdeInicio : CreateDatabaseIfNotExists<Contexto>
    {
        protected override void Seed(Contexto contexto)
    {
            var NombreCiudad = new Ciudades();
            NombreCiudad.Nom_Ciudad = "Choloma";
            contexto.Ciudadesx.Add(NombreCiudad);

            var NombreCiudad2 = new Ciudades();
            NombreCiudad2.Nom_Ciudad = "San Pedro Sula";
            contexto.Ciudadesx.Add(NombreCiudad2);

            var NombreCiudad3 = new Ciudades();
            NombreCiudad3.Nom_Ciudad = "Tegucigalpa";
            contexto.Ciudadesx.Add(NombreCiudad3);

            base.Seed(contexto);

        }
    }
}
