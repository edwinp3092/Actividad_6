using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Rentas
{
    public class ProveedorBL
    {
        Contexto _contexto;
        public BindingList<Proveedor> InfoProveedor { get; set; }

        public ProveedorBL()
        {
            _contexto = new Contexto();
            InfoProveedor = new BindingList<Proveedor>();
        }

        public BindingList<Proveedor> ObtenerProveedor()
        {
            _contexto.Proveedorx.Load();
            InfoProveedor = _contexto.Proveedorx.Local.ToBindingList();

            return InfoProveedor;
        }

        public Resultado3 GuardarProveedor(Proveedor proveedor)
        {
            var resultado3 = Validar(proveedor);
            if (resultado3.Exitoso == false)
            {
                return resultado3;
            }

            _contexto.SaveChanges();

            resultado3.Exitoso = true;
            return resultado3;
        }

        public void AgregarProveedor()
        {
            var nuevoProveedor = new Proveedor();
            InfoProveedor.Add(nuevoProveedor);
        }

        public void CancelarCambios()
        {
            foreach (var item in _contexto.ChangeTracker.Entries())
            {
                item.State = EntityState.Unchanged;
                item.Reload();
            }
        }

        public bool EliminarProveedor(int id)
        {
            foreach (var Proveedor in InfoProveedor)
            {
                if (Proveedor.IdProve == id)
                {
                    InfoProveedor.Remove(Proveedor);
                    _contexto.SaveChanges();
                    return true;
                }
            }

            return false;
        }

        private Resultado3 Validar(Proveedor proveedor)
        {
            var resultado3 = new Resultado3();
            resultado3.Exitoso = true;

            if (string.IsNullOrEmpty(proveedor.NombreProve) == true)
            {
                resultado3.Mensaje = "Ingrese nombre de proveedor";
                resultado3.Exitoso = false;
            }

            if (string.IsNullOrEmpty(proveedor.DireccionProve) == true)
            {
                resultado3.Mensaje = "Ingrese numero de direccion del proveedor";
                resultado3.Exitoso = false;
            }

            if (string.IsNullOrEmpty(proveedor.TelefonoProve ) == true)
            {
                resultado3.Mensaje = "Ingrese telefono de proveedor";
                resultado3.Exitoso = false;
            }

            if (string.IsNullOrEmpty(proveedor.CorreoElectronico) == true)
            {
                resultado3.Mensaje = "Ingrese correo electronico de proveedor";
                resultado3.Exitoso = false;
            }

            return resultado3;
        }
    }

    public class Proveedor
    {
        [Key]
        public int IdProve { get; set; }
        public string NombreProve { get; set; }
        public string TelefonoProve { get; set; }
        public string DireccionProve { get; set; }
        public string CorreoElectronico { get; set; }
        public bool Activo { get; set; }
    }

    public class Resultado3
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; }
    }


}
