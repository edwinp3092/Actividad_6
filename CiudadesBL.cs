using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Rentas
{
    public class CiudadesBL
    {
        Contexto _contexto;
        public BindingList<Ciudades> ListaCiudades { get; set; }


        public CiudadesBL()
        {
            _contexto = new Contexto();
            ListaCiudades = new BindingList<Ciudades>();
        }

        public BindingList <Ciudades> ObtenerCiudades()
        {
            _contexto.Ciudadesx.Load();
            ListaCiudades = _contexto.Ciudadesx.Local.ToBindingList();

            return ListaCiudades;
        }


    }

    public class Ciudades
    {
        public int Id { get; set; }
        public string Nom_Ciudad { get; set; }
    }
}


