using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Rentas
{
   public class ClientesBL
   {

        Contexto _contexto;
        public BindingList<Cliente> infocliente { get; set; }


            public ClientesBL()
            {
                _contexto = new Contexto();
                infocliente = new BindingList<Cliente>();

            }

        public BindingList<Cliente> ObtenerCliente()
        {

            _contexto.Clientex.Load();
            infocliente = _contexto.Clientex.Local.ToBindingList();

            return infocliente;

        }


        public Resultado2 GuardarCliente(Cliente cliente)
        {
            var resultado2 = Validar(cliente);
            if (resultado2.Exitoso == false)
            {
                return resultado2;
            }

            _contexto.SaveChanges();

            /*if (cliente.Id == 0)
            {
                cliente.Id = infocliente.Max(item => item.Id) + 1;
            }*/

            resultado2.Exitoso = true;
            return resultado2;
        }

        public void AgregarCliente()
        {
            var nuevoCliente = new Cliente();
            infocliente.Add(nuevoCliente);
        }

        public void CancelarCambios()
        {
            foreach (var item in _contexto .ChangeTracker .Entries ())
            {
                item.State = EntityState.Unchanged;
                item.Reload();
            }
        }


        public bool EliminarCliente(int id)
        {
            foreach (var Cliente in infocliente)
            {
                if (Cliente.Id == id)
                {
                    infocliente.Remove(Cliente);
                    _contexto.SaveChanges();
                    return true;
                }
            }

            return false;
        }

        private Resultado2 Validar(Cliente cliente)
        {
            var resultado2 = new Resultado2();
            resultado2.Exitoso = true;

            if (string.IsNullOrEmpty(cliente.Nombre) == true)
            {
                resultado2.Mensaje = "Ingrese nombre de Cliente";
                resultado2.Exitoso = false;
            }

            if (string.IsNullOrEmpty(cliente.Telefono) == true)
            {
                resultado2.Mensaje = "Ingrese numero de telefono";
                resultado2.Exitoso = false;
            }

            if (string.IsNullOrEmpty(cliente.Direccion) == true)
            {
                resultado2.Mensaje = "Ingrese numero direccion";
                resultado2.Exitoso = false;
            }

            if (cliente.CiudadId == 0)
            {
                resultado2.Mensaje = "Seleccione la ciudad";
                resultado2.Exitoso = false;
            }



            return resultado2;
        }
    }

    public class Cliente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public int CiudadId { get; set; }
        public Ciudades Ciudades { get; set; }
        public byte[] Foto { get; set; }
        public bool Activo { get; set; }
        
    }



    

    public class Resultado2
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; }
    }
}


