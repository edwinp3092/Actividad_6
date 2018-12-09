using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Rentas
{
    public class CompraBL
    {
        Contexto _contexto;
        public BindingList<Compra> ListaCompras { get; set; }

        public CompraBL()
        {
            _contexto = new Contexto();
        }

        public BindingList<Compra> ObtenerCompras()
        {
            _contexto.Comprasx.Include("CompraDetalle").Load();
            ListaCompras = _contexto.Comprasx.Local.ToBindingList();

            return ListaCompras;
        }

        public void AgregarCompra()
        {
            var nuevaCompra = new Compra();
            _contexto.Comprasx.Add(nuevaCompra);
        }

        public void AgregarCompraDetalle(Compra compra)
        {
            if (compra != null)
            {
                var nuevoDetalle = new CompraDetalle();
                compra.CompraDetalle.Add(nuevoDetalle);
            }
        }

        public void RemoverCompraDetalle(Compra compra, CompraDetalle compraDetalle)
        {
            if (compra != null && compraDetalle != null)
            {
                compra.CompraDetalle.Remove(compraDetalle);
            }
        }

        public void CancelarCambios()
        {
            foreach (var item in _contexto.ChangeTracker.Entries())
            {
                item.State = EntityState.Unchanged;
                item.Reload();
            }
        }

        public Resultado GuardarCompra(Compra compra)
        {
            var resultado = Validar(compra);
            if (resultado.Exitoso == false)
            {
                return resultado;
            }

            CalcularCompraExistencia(compra);

            _contexto.SaveChanges();
            resultado.Exitoso = true;
            return resultado;
        }

        private void CalcularCompraExistencia(Compra compra)
        {
            foreach (var detalle in compra.CompraDetalle)
            {
                var producto = _contexto.Productox.Find(detalle.ProductoId);
                if (producto != null)
                {
                    if (compra.Activo == true)
                    {
                        producto.Existencia = producto.Existencia + detalle.Cantidad;
                    }
                    else
                    {
                        producto.Existencia = producto.Existencia - detalle.Cantidad;
                    }
                }
            }
        }

        private Resultado Validar(Compra compra)
        {
            var resultado = new Resultado();
            resultado.Exitoso = true;

            if (compra == null)
            {
                resultado.Mensaje = "Agregue una compra para poderla salvar";
                resultado.Exitoso = false;

                return resultado;
            }

            if (compra.Id != 0 && compra.Activo == true)
            {
                resultado.Mensaje = "La compra ya fue emitida y no se pueden realizar cambios";
                resultado.Exitoso = false;
            }

            if (compra.Activo == false)
            {
                resultado.Mensaje = "La compra esta anulada y no se pueden realizar cambios en ella";
                resultado.Exitoso = false;
            }

            if (compra.ProveedorId == 0)
            {
                resultado.Mensaje = "Seleecione un proveedor";
                resultado.Exitoso = false;
            }

            if (compra.CompraDetalle.Count == 0)
            {
                resultado.Mensaje = "Agregue producto a la compra";
                resultado.Exitoso = false;
            }

            foreach (var detalle in compra.CompraDetalle)
            {
                if (detalle.ProductoId == 0)
                {
                    resultado.Mensaje = "Seleccione productos validos";
                    resultado.Exitoso = false;
                }
            }

            return resultado;
        }

        public void CalcularCompra(Compra compra)
        {
            if (compra != null)
            {
                double subtotal = 0;
                foreach (var detalle in compra.CompraDetalle)
                {
                    var producto = _contexto.Productox.Find(detalle.ProductoId);
                    if (producto != null)
                    {
                        detalle.Precio = producto.Precio;
                        detalle.Total = detalle.Cantidad * producto.Precio;

                        subtotal += detalle.Total;
                    }
                }
                compra.Subtotal = subtotal;
                compra.Impuesto = subtotal * 0.15;
                compra.Total = subtotal + compra.Impuesto;
            }
        }

        public bool AnularCompra(int id)
        {
            foreach (var compra in ListaCompras)
            {
                if (compra.Id == id)
                {
                    compra.Activo = false;
                    CalcularCompraExistencia(compra);
                    _contexto.SaveChanges();
                    return true;
                }
            }
            return false;
        }

    }

    public class Compra
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public int ProveedorId { get; set; }
        public Proveedor Proveedor { get; set; }
        public BindingList<CompraDetalle> CompraDetalle { get; set; }
        public double Subtotal { get; set; }
        public double Impuesto { get; set; }
        public double Total { get; set; }
        public bool Activo { get; set; }

        public Compra()
        {
            Fecha = DateTime.Now;
            CompraDetalle = new BindingList<CompraDetalle>();
            Activo = true;
        }
    }

    public class CompraDetalle
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public Producto Producto { get; set; }
        public int Cantidad { get; set; }
        public double Precio { get; set; }
        public double Total { get; set; }


        public CompraDetalle()
        {
            Cantidad = 1;
        }
    }
}
